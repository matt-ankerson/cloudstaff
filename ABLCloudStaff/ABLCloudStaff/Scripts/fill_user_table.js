// Author: Matt Ankerson
// Date: 10 July 2015
$(document).ready(function () {

    // Use ajax to fetch all user names, inject into the user table
    var get_users_url = "/Admin/GetFullUserInformations";

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

        var list = data;

        // Pull values out of object list and inject table elements into table

        // For each userInfo object in the list
        for (var i = 0; i < list.length; i++)
        {
            $("#user-table").append('<tr>' +
                '<td>' + list[i].userID + '</td>' +
                '<td>' + list[i].firstName + '</td>' +
                '<td>' + list[i].lastName + '</td>' +
                '<td>' + list[i].userType + '</td>' +
                '<td>' + list[i].isDeleted + '</td>' +
                '<td>' + '<button id="' + list[i].userID + '" class="edit-user-button btn btn-default"><span class="glyphicon glyphicon-pencil"></span></button>' + '</td>' +
                '</tr>');
        }

        //----------------------------------------------------------------------------------------

        // Launch modal when a edit button is clicked
        $(".edit-user-button").on('click', function (e) {
            // Launch the modal
            $("#edit-user-modal").modal();
            // Get the invoking element
            invoker = $(this);
            // Get the invoking element's id
            invoker_id = this.id;

            alert(invoker_id);
        });

        //-----------------------------------------------------------------------------------------

        // Unbind the trigger for our modal form submission
        // ...doing this stops the form being submitted multiple times.
        $('#edit-user-modal').on('hide.bs.modal', function (e) {
            $("#edit-user-modal form").trigger('submit');
            $("#edit-user-modal form").unbind('submit');
        });
        
    }

    function errorFunc(error) {
        alert('error' + error.responseText);
    }
});