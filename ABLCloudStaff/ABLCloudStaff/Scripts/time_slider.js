// Author: Matt Ankerson
// Date: 3 August 2015

$(document).ready(function () {

    // Get time from the server via ajax in order to populate the values of our slider bar.

    var get_time_url = "/Home/GetRemainderOfToday";

    $.ajax({
        type: "GET",
        url: get_time_url,
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: success_func,
        error: errorFunc
    });

    function success_func(data, status) {
        var list = data;
        var tick_list = [];

        // Build list of integers for slider ticks
        for (var i = 0; i < list.length; i++) {
            tick_list.push(list[i].numeric_repr);
        }

        // Init slider bar
        $("#time_slider").slider({
            min: 0,
            max: (tick_list.length - 1),
            step: 1,
            value: 0,
            tooltip: 'never'
        });

        // Force width of slider to inherit (slider width seems buggy)
        $("#time_slider").css("width", "inherit");

        // Update time display when slider is slid.
        $("#time_slider").on("slide", function (slideEvt) {
            // index the tick_list using value from the slider
            $("#time_slider_val").text(tick_list[slideEvt.value]);
            // Set the value of our hidden field
            $("#time_value").val(list[slideEvt.value].dateString);
            // Show the cancel button
            $("#cancel_time").show();
            $("#cancel_time").css("visibility", "visible");
        });
     
    }

    function errorFunc(error) {
        alert('error' + error.responseText);
    }

    // Click handler for time selection cancel button.
    $("#cancel_time").on('click', function (e) {
        e.preventDefault();

        // Hide the button
        $("#cancel_time").hide();
        // Set the time back to undefined
        $("#time_value").val(0);
        $("#time_slider_val").text("Not applicable");
        // Set the slider back to the start
        $("#time_slider").val(0);
    });

});