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
        success: success_func,
        error: errorFunc
    });

    function success_func(data, status) {
        var list = data;
        //alert(JSON.stringify(list));
        // Pull values out of dictionary and inject table elements into table
        //for (var key in dict) {
        //    $("#user-table").append('<tr><td>' + key + '</td><td>' + dict[key] + '</td></tr>');
       // }

    }

    function errorFunc(error) {
        //alert('error' + error.responseText);
    }

    // Use ajax to fetch all location changes

});