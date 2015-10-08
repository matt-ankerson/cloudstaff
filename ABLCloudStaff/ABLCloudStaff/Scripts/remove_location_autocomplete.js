// Author: Matt Ankerson
// Date: 23 July 2015
$(document).ready(function () {

    // The list of available autocomplete options:
    var availableTags = [];

    // Use ajax to fetch all locations
    var get_locations_url = "/Admin/GetAllLocationsForAutoComplete";

    $.ajax({
        type: "GET",
        url: get_locations_url,
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: success_func,
        error: errorFunc
    });

    function success_func(data, status) {
        var dict = data;

        // Pull values out of dictionary and assign to array
        for (var key in dict) {
            availableTags.push({ label: dict[key], value: key });
        }
    }

    function errorFunc(error) {
        alert('error' + error.responseText);
    }

    $("#remove-location-user-autocomplete").autocomplete({
        source: availableTags,
        select: function (event, ui) {
            $('#remove-location-user-autocomplete').val(ui.item.label);
            $('#remove-location-userID').val(ui.item.value);
            $('#remove-location-userID-display').text("# " + ui.item.value);
            return false;
        }
    });
});