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

    // Function fired when date is selected.
    function date_selected(date_text, inst) {

    }

    //----------------------------------------------
    // Visitor modal time picker for departure time:
    time_picker = $('#visitor_timepicker');

    // Set up time picker.
    time_picker.timepicker({
        'minTime': new Date()
    });

});