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
        success: get_user_info_success_func,
        error: get_user_info_errorFunc
    });

    function get_user_info_success_func(data, status) {

        var list = data;

        // Remove the loading-spinner
        $("#spn-user-table-loading").remove();

        // Paste in the table containers and header
        $("#see-users").append('<div id="user-table-container" class="container nested-tabs">' +
                                                '<div class="panel panel-default">' +
                                                    '<!-- Table -->' +
                                                    '<table id="user-table" class="table table-striped">' +
                                                        '<tr>' +
                                                            '<th>#</th>' +
                                                            '<th>First name</th>' +
                                                            '<th>Last name</th>' +
                                                            '<th>Type</th>' +
                                                            '<th>Active</th>' +
                                                            '<th>Edit</th>' +
                                                        '</tr>' +
                                                    '</table>' +
                                                '</div>' +
                                            '</div>');

        // Pull values out of object list and inject table elements into table

        // For each userInfo object in the list
        for (var i = 0; i < list.length; i++)
        {
            $("#user-table").append('<tr>' +
                '<td>' + list[i].userID + '</td>' +
                '<td>' + list[i].firstName + '</td>' +
                '<td>' + list[i].lastName + '</td>' +
                '<td>' + list[i].userType + '</td>' +
                '<td>' + list[i].isActive + '</td>' +
                '<td>' + '<button id="' + list[i].userID + '" class="edit-user-button btn btn-default">' +
                '<span class="glyphicon glyphicon-pencil"></span>' +
                '<input type="hidden" id="firstName-value" value="' + list[i].firstName + '" />' +
                '<input type="hidden" id="lastName-value" value="' + list[i].lastName + '" />' +
                '<input type="hidden" id="userType-value" value="' + list[i].userTypeID + '" />' +
                '<input type="hidden" id="isActive-value" value="' + list[i].isActive + '" />' +
                '</button>' + '</td>' +
                '</tr>');
        }

        //----------------------------------------------------------------------------------------

        invoker_id = 0;

        // Launch modal when an edit button is clicked
        $(".edit-user-button").on('click', function (e) {
            // Launch the modal
            $("#edit-user-modal").modal();
            // Get the invoking element
            invoker = $(this);
            // Get the invoking element's id
            invoker_id = this.id;

            // We need to populate fields on the form with default values
            // Get all values we need
            var userID = invoker_id;
            var firstName = invoker.find("#firstName-value").val();
            var lastName = invoker.find("#lastName-value").val();
            var userTypeID = invoker.find("#userType-value").val();
            var isActive = invoker.find("#isActive-value").val();
            
            // This will fire when the modal has fully revealed itself.
            $('#edit-user-modal').on('shown.bs.modal', function (e) {

                // Get the available user types
                var get_user_types_url = "/Admin/GetUserTypes";

                $.ajax({
                    type: "GET",
                    url: get_user_types_url,
                    data: null,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: get_usertypes_success_func,
                    error: get_usertypes_errorFunc
                });

                function get_usertypes_success_func(data, status) {

                    var list = data;

                    // Pull values out of object list and inject table elements into option list

                    // For each usertype object in the list
                    for (var i = 0; i < list.length; i++) {
                        // It's important to initialise the controls with current values
                        if (userTypeID == list[i].UserTypeID)
                        {
                            $("#edit-user-userType").append('<option selected="selected" value="' + list[i].UserTypeID + '">' + list[i].Type + '</option>');
                        }
                        else
                        {
                            $("#edit-user-userType").append('<option value="' + list[i].UserTypeID + '">' + list[i].Type + '</option>');
                        }                        
                    }

                }

                function get_usertypes_errorFunc(error) {
                    alert('Error getting user types: ' + error.responseText);
                }

                $("#edit-user-modal-title").text("Edit: " + firstName + " " + lastName);

                // We need to populate and enable all fields on the form
                $("#edit-user-firstName").val(firstName).prop('disabled', false);
                $("#edit-user-lastName").val(lastName).prop('disabled', false);
                $("#edit-user-isActive").prop('disabled', false)

                if (isActive == "True")
                {
                    $("#edit-user-isActive").prop('checked', true);
                }
                
            });
        });
        
    }

    function get_user_info_errorFunc(error) {
        // Remove the loading-spinner
        $("#spn-user-table-loading").remove();

        // Report the error
        $("#see-users").append("There was an error. " + error.responseText);
    }

    //-----------------------------------------------------------------------------------------

    // Click handler for submit button on modal form
    $("#update-user-button").on("click", function (e) {

        e.preventDefault();

        var form = $("#edit-user-form");

        // Inject a hidden field into the form for the UserID
        form.append('<input type="hidden" name="userID" value="' + invoker_id + '" />');

        form.submit();

    });

    //// Intercept the form submittal, only submit if the userID is valid
    //$("#edit-user-form").on("submit", function (e) {

    //    e.preventDefault();
    //    alert("here");
    //    var form = $("#edit-user-form");
    //    var userID = form.find("#userID").val();

    //    if (userID != undefined && userID != null && userID != 0)
    //        form.submit();
    //});

    // Unbind the trigger for our modal form submission
    // ...doing this stops the form being submitted multiple times.
    $('#edit-user-modal').on('hide.bs.modal', function (e) {
        $("#edit-user-modal form").trigger('submit');
        $("#edit-user-modal form").unbind('submit');
    });
});