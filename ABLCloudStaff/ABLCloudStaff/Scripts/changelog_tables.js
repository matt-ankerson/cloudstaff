// Author: Matt Ankerson
// Date: 10 July 2015
$(document).ready(function () {

    // Use ajax to fetch all status changes
    var get_status_url = "/Admin/GetStatusChanges";

    $.ajax({
        type: "GET",
        url: get_status_url,
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: status_success_func,
        error: errorFunc
    });

    function status_success_func(data, status) {
        var list = data;

        // Pull values out of javascript object and inject table elements into table

        // For each status changed item
        for (var i = 0; i < list.length; i++) {

            $("#status-changes-table").append('<tr>' +
                '<td>' + list[i].firstName + '</td>' +
                '<td>' + list[i].lastName + '</td>' +
                '<td>' + list[i].oldState + '</td>' +
                '<td>' + list[i].newState + '</td>' +
                '<td>' + list[i].stateChangeTimestamp + '</td>' +
                '<td>' + list[i].prevStateInitTimestamp + '</td>' +
                '</tr>');
        }

    }

    function errorFunc(error) {
        alert('error' + error.responseText);
    }

    // Use ajax to fetch all location changes
    var get_location_url = "/Admin/GetLocationChanges";

    $.ajax({
        type: "GET",
        url: get_location_url,
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: location_success_func,
        error: errorFunc
    });

    function location_success_func(data, status) {
        var list = data;

        // Pull values out of javascript object and inject table elements into table

        // For each location changed item
        for (var i = 0; i < list.length; i++) {

            $("#location-changes-table").append('<tr>' +
                '<td>' + list[i].firstName + '</td>' +
                '<td>' + list[i].lastName + '</td>' +
                '<td>' + list[i].oldState + '</td>' +
                '<td>' + list[i].newState + '</td>' +
                '<td>' + list[i].stateChangeTimestamp + '</td>' +
                '<td>' + list[i].prevStateInitTimestamp + '</td>' +
                '</tr>');
        }

    }
});