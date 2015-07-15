// Author: Matt Ankerson
// Date: 14 July 2015

$(document).ready(function () {

    // Use ajax to fetch all UserType names, inject into the Option list on the add-user form
    var get_user_types_url = "/Admin/GetUserTypes";

    $.ajax({
        type: "GET",
        url: get_user_types_url,
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: success_func,
        error: errorFunc
    });

    function success_func(data, status) {

        var list = data;

        // Pull values out of object list and inject table elements into option list

        // For each usertype object in the list
        for (var i = 0; i < list.length; i++) {
            $("#user-type-list").append('<option value="' + list[i].UserTypeID + '">' + list[i].Type + '</option>');
        }

    }

    function errorFunc(error) {
        alert('error' + error.responseText);
    }

    // Ensure that the form is not submitted without required data.
    $("#new-user-submit").on("click", function (e) {

        e.preventDefault();

        if ($("#user-type-list").val != null) {
            var form = $("#add-user-form");
            form.submit();
        }
    });
});