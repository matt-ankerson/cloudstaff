// Author: Matt Ankerson
// Date: 6 October 2015

$(document).ready(function () {

    visitor_modal = $('#visitor_modal');
    // Launch visitor modal on button press.
    $('#visitor_button').click(function (e) {
        visitor_modal.modal();
    });

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

    

});