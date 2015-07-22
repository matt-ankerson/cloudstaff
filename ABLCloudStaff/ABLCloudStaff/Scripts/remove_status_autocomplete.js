// Date: 22 July
// Author: Matt Ankerson
$(document).ready(function () {

    // The list of available autocomplete options:
    var availableTags = [];

    // Use ajax to fetch all statuses
    var get_statuses_url = "/Admin/GetAllStatusesForAutoComplete";

    $.ajax({
        type: "GET",
        url: get_statuses_url,
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

    $("#remove-status-user-autocomplete").autocomplete({
        source: availableTags,
        select: function (event, ui) {
            $('#remove-status-user-autocomplete').val(ui.item.label);
            $('#remove-status-userID').val(ui.item.value);
            return false;
        }
    });
});