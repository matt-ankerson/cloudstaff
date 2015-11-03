// Author: Matt Ankerson
// Date: 22 July 2015
$(document).ready(function () {

    // The list of available autocomplete options:
    var availableTags = [];

    // Use ajax to fetch all users
    var get_users_url = "/Admin/GetAllUsers";

    $.ajax({
        type: "GET",
        url: get_users_url,
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
        //alert('error' + error.responseText);
    }

    $("#add-status-user-autocomplete").autocomplete({
        source: availableTags,
        select: function (event, ui) {
            $('#add-status-user-autocomplete').val(ui.item.label);
            $('#add-status-userID').val(ui.item.value);
            return false;
        }
    });
});