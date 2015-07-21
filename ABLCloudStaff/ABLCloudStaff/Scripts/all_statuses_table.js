// Author: Matt Ankerson
// Date: 16 July 2015

$(document).ready(function () {

    // Use ajax to fetch all available statuses, inject as table rows into the status table
    var get_statuses_url = "/Admin/GetAllStatuses";

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
        var list = data;

        for(var i = 0; i < list.length; i++)
        {
            $("#all-statuses-table").append('<tr>' +
                '<td>' + list[i].statusID + '</td>' +
                '<td>' + list[i].name + '</td>' +
                '<td>' + list[i].worksite + '</td>' +
                '</tr>');
        }
    }

    function errorFunc(error) {
        alert('error' + error.responseText);
    }
});