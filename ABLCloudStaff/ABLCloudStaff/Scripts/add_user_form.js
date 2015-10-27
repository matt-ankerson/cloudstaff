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
        //alert('Error getting user types: ' + error.responseText);
    }

    // Ensure that the form is not submitted without required data.
    $("#new-user-submit").on("click", function (e) {

        e.preventDefault();

        if ($("#user-type-list").val != null) {

            // Get the form.
            var form = $("#add-user-form");

            // Ensure that both passwords match:
            var password1 = form.find($("password-input"));
            var password2 = form.find($("password-confirm-input"));

            if (password1.val() == password2.val()) {
                // Hide the error message
                var error_message = form.find($("#add_user_password_error"));
                error_message.hide();
                // Proceed with submit.
                form.submit();
            }
            else {
                // Report the problem.
                var error_message = form.find($("#add_user_password_error"));
                error_message.show();
            }
        }
    });
});