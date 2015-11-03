// Author: Matt Ankerson
// Date: 24 September 2015

$(document).ready(function () {

    // Time slider element is held independently of the functions in this script for performance reasons.
    var time_slider;
    // Keep a track of the number of times the slider has been changed.
    // This is due to a complication with resetting the slider bar.
    var n_changes = 0;
    // Declare the main time interval list here, events triggered in the UI will mutate this list appropriately.
    //var time_intervals;
    // Declare the 'all day' and 'remaining time' interval lists here. Ajax success functions will populate these lists.
    var all_day_intervals;
    var remaining_intervals;

    //--------------------------------------------------------------------
    // Date picker setup:
    var date_picker = $("#datepicker");

    // Set up JQueryUI datepicker.
    date_picker.datepicker({
        showOn: "focus",
        minDate: new Date(),
        onSelect: date_selected,
        dateFormat: 'dd/mm/yy'
    });

    // Function fired when date is selected.
    function date_selected(date_text, inst) {

        var nice_date = pretty_date(date_text);
        var date_for_display = display_date(date_text);
        
        // Hide the datepicker
        date_picker.datepicker('hide');

        // Update the return day display:
        $("#date_picker_val").text(date_for_display);

        // Make the cancel button appear.)
        $("#cancel_time").show();
        $("#cancel_time").css("visibility", "visible");

        // Call the AJAX function to get a list of time intervals relavent for the selected day.
        get_all_day_times_from_server(all_day_time_success_func, errorFunc, nice_date);

        // Refresh the time slider with all day times
        refresh_time_slider(all_day_intervals);
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
    get_remaining_times_from_server(remaining_time_success_func, errorFunc);

    function get_all_day_times_from_server(success_func, error_func, desired_day) {
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

    function get_remaining_times_from_server(success_func, error_func) {
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
    function refresh_time_slider(time_intervals) {

        // Define a list for slider ticks
        var tick_list = [];

        // Build list of integers for slider ticks
        for (var i = 0; i < time_intervals.length; i++) {
            tick_list.push(time_intervals[i].numeric_repr);
        }

        // Define change event handler for slider bar.
        var change_func = function (value) {
            if (n_changes > 0) {
                // index the tick_list using value from the slider
                $("#time_slider_val").text(tick_list[value]);
                // Set the value of our hidden field
                $("#time_value").val(time_intervals[value].dateString);
                // Show the cancel button
                $("#cancel_time").show();
                $("#cancel_time").css("visibility", "visible");
            }
            n_changes++;
        }

        // Init slider bar
        time_slider = new MobileRangeSlider('time_slider', {
            min: 0,
            max: (tick_list.length - 1),
            value: 0,
            change: change_func
        });

        // Force width of slider to inherit (slider width seems buggy)
        $("#time_slider").css("width", "inherit");
    }

    function remaining_time_success_func(data, status) {
        // Save the remaining time intervals
        remaining_intervals = data;
        // Create the time slider. (we want remaining time by default.)
        refresh_time_slider(remaining_intervals);
    }

    function all_day_time_success_func(data, status) {
        // Save the all day intervals
        all_day_intervals = data
        refresh_time_slider(all_day_intervals);
    }

    function errorFunc(error) {
        // There was a silent issue server-side, do a hard refresh of the page. 
        //window.location.href = '/Home/Index';
    }

    // Click handler for time selection cancel button.
    $("#cancel_time").on('click', function (e) {
        e.preventDefault();
        n_changes = 0

        // Hide the button
        $("#cancel_time").hide();

        // Set the time back to undefined
        $("#time_value").val("");
        $("#time_slider_val").text(" ");

        // Set the slider back to the start
        time_slider.setValue(0);    // this will increment n_changes
        n_changes = 0

        // Remove the day selection from the input box and display span
        $("#datepicker").val("");
        $("#date_picker_val").text(" ");

        // Refresh the time slider with time remaining today
        refresh_time_slider(remaining_intervals);
    });
});