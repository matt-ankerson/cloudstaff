// Author: Matt Ankerson
// Date: 6 October 2015

$(document).ready(function () {

    visitor_modal = $('#visitor_modal');
    // Launch visitor modal on button press.
    $('#visitor_button').click(function (e) {
        visitor_modal.modal();
    });

    //-----------------------------------------------------
    // visitor modal date / time
    //---------------------------------------------
    // Visitor modal datepicker for departure date:
    date_picker = $('#visitor_datepicker');

    // Set up datepicker.
    date_picker.datepicker({
        showOn: "focus",
        minDate: new Date(),
        onSelect: date_selected
    });

    //----------------------------------------------
    // Visitor modal time picker for departure time:
    time_picker = $('#visitor_timepicker');
    time_picker.timepicker({
        'minTime': new Date(),
        'maxTime': '6:00pm',
        'forceRoundTime': true,
        'showDuration': true
    });


    // Function fired when date is selected.
    function date_selected(date_text, inst) {

        // Get a simple date for today
        raw_date = new Date();
        date_today = new Date(raw_date.getFullYear(), raw_date.getMonth(), raw_date.getDate());
        // Get the js date representation of the date_text
        given_date = construct_js_date(date_text);
        // Compare today's date to the date text
        if (given_date <= date_today)
        {
            // Change the timepicker so it has min time of now.
            time_picker.timepicker('option', 'minTime', new Date());
        }
        else
        {
            // Change the timepicker so it has no min time.
            time_picker.timepicker('option', 'minTime', '6:00am');
        }
    }

    // Return a js date object from a given
    function construct_js_date(date_str) {
        var arr = date_str.split('/');
        mm = parseInt(arr[0]) - 1; // months are indexed from 0 in JS
        dd = parseInt(arr[1]);
        yyyy = parseInt(arr[2]);
        // Return a JS date.
        return new Date(yyyy, mm, dd);
    }

    function construct_str_date(js_date) {
        return js_date.getMonth() + '/' + js_date.getDate() + '/' + js_date.getFullYear();
    }

    function pretty_datetime(ugly_date, ugly_time) {
        // Convert a date from the datepicker into something C# can parse.
        var date_arr = ugly_date.split('/');
        // arr[0] = mm
        // arr[1] = dd
        // arr[2] = yyyy
        var time_left = ugly_time.replace(ugly_time.slice(-2), '');
        var time_right = ugly_time.slice(-2);
        var pretty_datetime = date_arr[2] + '-' + date_arr[0] + '-' + date_arr[1] + ' ' + time_left + ' ' + time_right;
        return pretty_datetime;
    }

    // Submit button click handler
    $('#save_new_visitor').on('click', function (e) {
        e.preventDefault();

        // Build a c# parseable datetime and save it in the hidden field.

        csharp_parsable_datetime = null;
        date = null;
        time = null;
        
        // Did the user supply a date?
        if ($('#visitor_datepicker').val() == null) {
            // No date supplied, use today's date.
            date = construct_str_date(new Date());
        }
        else {
            // Date supplied, pull from input box.
            date = $('#visitor_datepicker').val();
        }
        // Did the user supply a time?
        if ($('#visitor_timepicker').val() == null) {
            // No time supplied, use 5pm.
            time = '5:00pm';
        }
        else {
            // Time supplied, pull from input box
            time = $('#visitor_timepicker').val();
        }

        // Problems here.*****
        csharp_parsable_datetime = pretty_datetime(date, time);

        // Save in hidden field.
        $('intendedDepartTime').val(csharp_parsable_datetime);

        // Submit the form.
        $('#add_visitor_form').submit();
    });

    // /visitor modal date / time
    //-----------------------------------------------------

    //-----------------------------------------------------
    // New visitor / returning visitor toggle buttons.
    new_visitor_button = $('#new_visitor_button');
    returning_visitor_button = $('#returning_visitor_button');

    // Click handlers:
    new_visitor_button.on('click', function (e) {
        // Transfer bold font to this button.
        returning_visitor_button.html('Returning Visitor');
        new_visitor_button.html('<b>New Visitor</b>');
    });

    returning_visitor_button.on('click', function (e) {
        // Transfer bold font to this button.
        returning_visitor_button.html('<b>Returning Visitor</b>');
        new_visitor_button.html('New Visitor');
    });

    //-----------------------------------------------------
    // Autocomplete for user being visited.
    //      - get dict of all general / admin users with their userIDs.

    // Use ajax to fetch all users
    var get_users_url = "/Home/GetGeneralAndAdminUsers";

    $.ajax({
        type: "GET",
        url: get_users_url,
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: general_admin_users_success_func,
        error: general_admin_users_error_func
    });
    
    function general_admin_users_success_func(data, status) {
        var dict = data;
        availableTags = [];
        // Pull values out of dictionary and assign to array
        for (var key in dict) {
            availableTags.push({ label: dict[key], value: key });
        }

        setup_visiting_user_autocomplete(availableTags)
    }

    function general_admin_users_error_func(error) {
        // Do nothing.
    }

    function setup_visiting_user_autocomplete(availableTags) {

        $("#visiting_user_autocomplete").autocomplete({
            source: availableTags,
            appendTo: '#visitor_modal',
            select: function (event, ui) {
                $('#visiting_user_autocomplete').val(ui.item.label);
                $('#user_being_visited_ID').val(ui.item.value);
                $('#user_being_visited_ID_display').html('<small>' + ui.item.label + ' <span class="glyphicon glyphicon-ok"></span></small>');
                return false;
            }
        });
    }
});