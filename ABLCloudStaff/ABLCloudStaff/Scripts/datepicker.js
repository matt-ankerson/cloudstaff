// Author: Matt Ankerson
// Date: 23 September 2015

$(document).ready(function () {

    // Date picker setup:

    var date_picker = $("#datepicker");

    // Set up JQueryUI datepicker.
    date_picker.datepicker({
        showOn: "focus",
        minDate: new Date(),
        onSelect: date_selected
    });

    // Function fired when date is selected.
    function date_selected(date_text, inst) {
        
        // Update the return day display:
        $("#date_picker_val").text(date_text);

        // Make the cancel button appear. (note that the click handler for this button
        // is in 'time_slider.js')
        $("#cancel_time").show();
        $("#cancel_time").css("visibility", "visible");
    }
});