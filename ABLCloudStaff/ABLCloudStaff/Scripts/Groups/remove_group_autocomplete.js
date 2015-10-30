// Author: Matt Ankerson
// Date: 27 October 2015
$(document).ready(function () {

    // The list of available autocomplete options:
    var availableTags = [];

    // Use ajax to fetch all statuses
    var get_groups_url = "/Admin/GetAllGroups";

    $.ajax({
        type: "GET",
        url: get_groups_url,
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: success_func,
        error: errorFunc
    });

    function success_func(data, status) {
        var list = data;

        // Pull values out of dictionary and assign to array
        for (var i = 0; i < list.length; i++) {
            availableTags.push({ label: list[i].Name, value: list[i].GroupID });
        }
    }

    function errorFunc(error) {
        //alert('error' + error.responseText);
    }

    $("#remove-group-user-autocomplete").autocomplete({
        source: availableTags,
        select: function (event, ui) {
            $('#remove-group-user-autocomplete').val(ui.item.label);
            $('#remove-group-groupID').val(ui.item.value);
            $('#remove-group-groupID-display').text("# " + ui.item.value);
            return false;
        }
    });
});