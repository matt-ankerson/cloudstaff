// Author: Matt Ankerson
// Date: 15 October 2015

$(document).ready(function () {

    see_groups_modal = $('#see_groups_modal');

    // Launch see group modal on button press.
    $('#see_groups_button').click(function (e) {
        see_groups_modal.modal();
    });

    // Use ajax to pull groups from the server, inject into the group list.
    var get_users_url = "/Home/GetAllGroups";

    $.ajax({
        type: "GET",
        url: get_users_url,
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: get_groups_success_func,
        error: get_groups_errorFunc
    });


    function get_groups_success_func(data, status) {
        var list = data
        // Inject buttons onto list group on the see groups modal.
        list_group = $('#current_group_list');

        // For each GroupInfo object in the list.
        // Let the value of each button hold the appropriate GroupID.
        for (var i = 0; i < list.length; i++)
        {
            if (list[i].Active == 'True') {
                // Group is out. (or 'Active')
                list_group.append('<button type="button" class="list-group-item btn_group_is_out btn_group_name" data-dismiss="modal" value="' + list[i].GroupID + '">' +
                    '<div class="group_name_inner jsOnly">' + list[i].Name + '</div>' + list[i].Name +
                    '<span class="pull-right badge">Out</span>' +
                    '</button>');
                
            }
            else {
                // Group is in. (or 'Inactive')
                list_group.append('<button type="button" class="list-group-item btn_group_is_in btn_group_name" data-dismiss="modal" value="' + list[i].GroupID + '">' +
                    '<div class="group_name_inner jsOnly">' + list[i].Name + '</div>' + list[i].Name +
                    '<span class="pull-right badge">In</span>' +
                    '</button>');
            }
            
        }

        control_container = $('#alternate_options_for_groups_in_or_out');
        control_container_previous_elements = control_container.contents();

        // Open group member modal when a group is selected from the 'see groups' modal.
        $(".btn_group_name").on('click', function (e) {
            // Inject the GroupID into an element on the modal
            $('#group_id_for_members').val($(this).val());
            group_name = $(this).find('.group_name_inner').text();
            // Inject the Group name into the modal heading.
            $('#member_list_heading').text(group_name);

            // We also need to present a slightly different modal when an 'out' group is pressed.
            // The modal needs to provide a multiselect list of all members, and a single button for returning to the office.
            if ($(this).hasClass('btn_group_is_out')) {
                
                default_in_status = $('#default_in_status_id').val();
                default_in_location = $('#default_in_location_id').val();
                // Clear the container contents.
                control_container.html('');
                // Inject appropriate controls.
                control_container.html('<input type="hidden" name="statusID" value="' + default_in_status + '" />' +
                                       '<input type="hidden" name="locationID" value="' + default_in_location + '" />' +
                                       '<input type="hidden" name="returnTime" value="" />' +
                                       '<input type="hidden" name="returningGroup" value="returnGroup" />' +
                                       '<button id="update-group-submit" type="submit" class="btn btn-info btn-block submit">Return to Office</button>');
            }
            else {
                // Show the ordinary controls. (For a group which is in)
                control_container.html('');
                control_container.append(control_container_previous_elements);

                set_group_out_options();
            }

            // Launch the members modal 
            $('#see_group_members_modal').modal();
            
        });
    }

    function get_groups_errorFunc(error) {
        // Inject a button explaining an error has occurred.
        list_group.append('<button type="button" class="list-group-item">An Error Occurred.</button>');
    }

    //-------------------------------------------------------------------------------------------------------
    // Set Group Out options:
    // This code is similar to that found in init_modal.js and datetime_selection.js

    function set_group_out_options() {

        // Time slider element is held independently of the functions in this script for performance reasons.
        var time_slider_for_group;
        // Keep a track of the number of times the slider has been changed.
        // This is due to a complication with resetting the slider bar.
        var n_changes_for_group = 0;
        // Declare the main time interval list here, events triggered in the UI will mutate this list appropriately.
        //var time_intervals;
        // Declare the 'all day' and 'remaining time' interval lists here. Ajax success functions will populate these lists.
        var all_day_intervals_for_group;
        var remaining_intervals_for_group;

        //--------------------------------------------------------------------
        // Date picker setup:
        var date_picker_for_group = $("#datepicker_for_group");

        // Set up JQueryUI datepicker.
        date_picker_for_group.datepicker({
            showOn: "focus",
            minDate: new Date(),
            onSelect: date_selected_for_group,
            dateFormat: 'dd/mm/yy'
        });

        // Function fired when date is selected.
        function date_selected_for_group(date_text, inst) {

            var nice_date = pretty_date(date_text);
            var date_for_display = display_date(date_text);

            // Hide the datepicker
            date_picker_for_group.datepicker('hide');

            // Update the return day display:
            $("#date_picker_val_for_group").text(date_for_display);

            // Make the cancel button appear.)
            $("#cancel_time_for_group").show();
            $("#cancel_time_for_group").css("visibility", "visible");

            // Call the AJAX function to get a list of time intervals relavent for the selected day.
            get_all_day_times_from_server_for_group(all_day_time_success_func_for_group, errorFunc_for_group, nice_date);

            // Refresh the time slider with all day times
            refresh_time_slider_for_group(all_day_intervals_for_group);
        }

        function pretty_date(ugly_date) {
            // Convert a date from the datepicker into something C# can parse.
            var arr = ugly_date.split('/');
            // arr[0] = dd
            // arr[1] = mm
            // arr[2] = yyyy
            var pretty_date = arr[2] + '-' + arr[1] + '-' + arr[0]; // yyyy/mm/dd
            return pretty_date;
        }
        function display_date(ugly_date) {
            // Convert a date from the datepicker into a date format dd-mm-yyyy
            var arr = ugly_date.split('/');
            // arr[0] = dd
            // arr[1] = mm
            // arr[2] = yyyy
            var pretty_date = arr[0] + '-' + arr[1] + '-' + arr[2]; // yyyy/mm/dd
            return pretty_date;
        }
        // end date picker setup
        //--------------------------------------------------------------------


        // Get time from the server via ajax in order to populate the values of our slider bar.
        // -- When a date is selected, we need to offer a full day's time on the time slider.

        // Call the AJAX functions:
        //get_all_day_times_from_server(all_day_time_success_func, errorFunc, new Date());
        get_remaining_times_from_server_for_group(remaining_time_success_func_for_group, errorFunc_for_group);

        function get_all_day_times_from_server_for_group(success_func, error_func, desired_day) {
            // Get time intervals from the start of a working day (defined in biz logic constants) until midnight.
            var get_all_day_time_url = "/Home/GetAllDayTimes";
            $.ajax({
                type: "GET",
                url: get_all_day_time_url,
                data: { date: desired_day },
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: success_func,
                error: error_func
            });
        }

        function get_remaining_times_from_server_for_group(success_func, error_func) {
            // Get time intervals ranging from now until the end of the day.
            var get_remaining_time_url = "/Home/GetRemainderOfToday";
            $.ajax({
                type: "GET",
                url: get_remaining_time_url,
                data: null,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: success_func,
                error: error_func
            });
        }

        // Recreate the time slider using the time_intervals list.
        function refresh_time_slider_for_group(time_intervals) {

            // Define a list for slider ticks
            var tick_list = [];

            // Build list of integers for slider ticks
            for (var i = 0; i < time_intervals.length; i++) {
                tick_list.push(time_intervals[i].numeric_repr);
            }

            // Define change event handler for slider bar.
            var change_func_for_group = function (value) {
                if (n_changes_for_group > 0) {
                    // index the tick_list using value from the slider
                    $("#time_slider_val_for_group").text(tick_list[value]);
                    // Set the value of our hidden field
                    $("#time_value_for_group").val(time_intervals[value].dateString);
                    // Show the cancel button
                    $("#cancel_time_for_group").show();
                    $("#cancel_time_for_group").css("visibility", "visible");
                }
                n_changes_for_group++;
            }

            // Init slider bar
            time_slider_for_group = new MobileRangeSlider('time_slider_for_group', {
                min: 0,
                max: (tick_list.length - 1),
                value: 0,
                change: change_func_for_group
            });

            // Force width of slider to inherit (slider width seems buggy)
            $("#time_slider_for_group").css("width", "inherit");
        }

        function remaining_time_success_func_for_group(data, status) {
            // Save the remaining time intervals
            remaining_intervals_for_group = data;
            // Create the time slider. (we want remaining time by default.)
            refresh_time_slider_for_group(remaining_intervals_for_group);
        }

        function all_day_time_success_func_for_group(data, status) {
            // Save the all day intervals
            all_day_intervals_for_group = data
            refresh_time_slider_for_group(all_day_intervals_for_group);
        }

        function errorFunc_for_group(error) {
            // There was a silent issue server-side, do a hard refresh of the page. 
            //window.location.href = '/Home/Index';
        }

        // Click handler for time selection cancel button.
        $("#cancel_time_for_group").on('click', function (e) {
            e.preventDefault();
            n_changes_for_group = 0

            // Hide the button
            $("#cancel_time_for_group").hide();

            // Set the time back to undefined
            $("#time_value_for_group").val("");
            $("#time_slider_val_for_group").text(" ");

            // Set the slider back to the start
            time_slider_for_group.setValue(0);    // this will increment n_changes
            n_changes_for_group = 0

            // Remove the day selection from the input box and display span
            $("#datepicker_for_group").val("");
            $("#date_picker_val_for_group").text(" ");

            // Refresh the time slider with time remaining today
            refresh_time_slider_for_group(remaining_intervals_for_group);
        });
    }
    // /Set group out options.
    //-------------------------------------------------------------------------------------------------------
});