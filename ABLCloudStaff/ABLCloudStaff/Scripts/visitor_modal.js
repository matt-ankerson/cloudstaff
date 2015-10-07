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
        given_date = construct_date(date_text);
        console.log(given_date, date_today);
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
    function construct_date(date_str) {
        var arr = date_str.split('/');
        // arr[0] = mm
        // arr[1] = dd
        // arr[2] = yyyy
        mm = parseInt(arr[0]) - 1; // months are indexed from 0 in JS
        dd = parseInt(arr[1]);
        yyyy = parseInt(arr[2]);
        // Return a JS date.
        return new Date(yyyy, mm, dd);
    }

    function pretty_datetime(ugly_date, ugly_time) {
        // Convert a date from the datepicker into something C# can parse.
        var arr = ugly_date.split('/');
        // arr[0] = mm
        // arr[1] = dd
        // arr[2] = yyyy
        var pretty_datetime = arr[2] + '-' + arr[0] + '-' + arr[1];
        return pretty_datetime;
    }

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