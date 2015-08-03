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
        var last_item = list[list.length - 1];

        // Init slider bar
        $("#time_slider").slider({
            min: list[0].numeric_repr,
            max: last_item.numeric_repr,
            scale: logarithmic,
            step: (list[1].numeric_repr - list[0].numeric_repr),
            value: list[0].numeric_repr
        });

        // Force width of slider to inherit (slider width seems buggy)
        $("#time_slider").css("width", "inherit");

        // Update time display when slider is slid.
        $("#time_slider").on("slide", function (slideEvt) {
            $("#time_slider_val").text(slideEvt.value);
        });

        for (var i = 0; i < list.length; i++) {

        }
    }

    function errorFunc(error) {
        alert('error' + error.responseText);
    }

});