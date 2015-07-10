// Author: Matt Ankerson
// Date: 10 July 2015
$(document).ready(function () {

    // Use ajax to fetch all user names, inject into the user table
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
        // Pull values out of dictionary and inject table elements into table
        for (var key in dict)
        {
            $("#user-table").append('<tr><td>' + key + '</td><td>' + dict[key] + '</td></tr>');
        }
        
    }

    function errorFunc(error) {
        alert('error' + error.responseText);
    }
});