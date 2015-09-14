// Author: Matt Ankerson
// Date: 14 September 2015

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

    // Time slider element is held independently of the functions in this script.
    var time_slider;
    // Keep a track of the number of times the slider has been changed.
    var n_changes = 0

    function success_func(data, status) {
        var list = data;
        var tick_list = [];

        // Build list of integers for slider ticks
        for (var i = 0; i < list.length; i++) {
            tick_list.push(list[i].numeric_repr);
        }

        // Define change event handler for slider bar.
        var change_func = function (value) {
            if (n_changes > 0)
            {
                // index the tick_list using value from the slider
                $("#time_slider_val").text(tick_list[value]);
                // Set the value of our hidden field
                $("#time_value").val(list[value].dateString);
                // Show the cancel button
                $("#cancel_time").show();
                $("#cancel_time").css("visibility", "visible");
                console.log('running');
            }
            n_changes++;
        };

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

    function errorFunc(error) {
        alert('error' + error.responseText);
    }

    // Click handler for time selection cancel button.
    $("#cancel_time").on('click', function (e) {
        e.preventDefault();
        n_changes = 0
        // Hide the button
        $("#cancel_time").hide();

        // Set the time back to undefined
        $("#time_value").val("");
        $("#time_slider_val").text("Not applicable");
        // Set the slider back to the start
        time_slider.setValue(0);    // this will increment n_changes
        n_changes = 0
    });

    // If the current selected status is "In Office", then there's no need to present the option to set a time span.

});