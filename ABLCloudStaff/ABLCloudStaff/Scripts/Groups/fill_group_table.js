// Author: Matt Ankerson
// Date: 27 October 2015

$(document).ready(function () {

    group_edit_invoker_value = 0;

    // Use ajax to fetch all groups, inject into the group table
    var get_groups_url = "/Admin/GetAllGroups";

    $.ajax({
        type: "GET",
        url: get_groups_url,
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: get_groups_success_func,
        error: get_groups_errorFunc
    });

    function get_groups_success_func(data, status) {
        list = data;
        // Inject groups into table.
        for (var i = 0; i < list.length; i++) {
            $("#all-groups-table").append('<tr>' +
                '<td>' + list[i].GroupID + '</td>' +
                '<td>' + list[i].Name + '</td>' +
                '<td>' + list[i].Active + '</td>' +
                '<td>' + list[i].Priority + '</td>' +
                '<td>' + '<button value="' + list[i].GroupID + '" class="edit-group-button btn btn-default">' +
                '<span class="glyphicon glyphicon-pencil"></span>' +
                '<input type="hidden" id="group-name-value" value="' + list[i].Name + '" />' +
                '<input type="hidden" id="group-active-value" value="' + list[i].Active + '" />' +
                '<input type="hidden" id="group-priority-value" value="' + list[i].Priority + '" />' +
                '</button>' + '</td>' +
                '</tr>');
        }

        // Launch modal when an edit button is clicked
        $(".edit-group-button").on('click', function (e) {
            // Launch the modal
            $("#edit-group-modal").modal();
            // Get the invoking element
            invoker = $(this);
            // Get the invoking element's val. (This is our group ID)
            invoker_id = invoker.val();
            group_edit_invoker_value = invoker.val();

            // We need to populate fields on the form with default values
            // Get all values we need
            var groupID = invoker_id;
            var name = invoker.find("#group-name-value").val();
            var priority = invoker.find("#group-priority-value").val();

            // This will fire when the modal has fully revealed itself.
            $('#edit-group-modal').on('shown.bs.modal', function (e) {
                // Get all users for the members multiselect. (current members need to be selected)
                // Get all users, then get the members of the indicated group.

                // Get all general and admin users of the system
                var get_general_admin_users_url = "/Admin/GetGeneralAndAdminUsers";

                $.ajax({
                    type: "GET",
                    url: get_general_admin_users_url,
                    data: null,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: get_general_admin_users_success_func,
                    error: get_general_admin_users_errorFunc
                });

                function get_general_admin_users_success_func(data, status) {

                    all_users_dict = data;

                    // Now get members of the selected group:
                    var get_members_of_group_url = "/Admin/GetMembersOfGroup";

                    $.ajax({
                        type: "GET",
                        url: get_members_of_group_url,
                        data: { groupID: invoker_id },
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: get_members_of_group_success_func,
                        error: get_members_of_group_errorFunc
                    });

                    function get_members_of_group_success_func(current_members, status) {
                        // Put the keys of this dict into a set
                        members_of_this_group_set = new Set();
                        for (var key in Object.keys(current_members)) {
                            members_of_this_group_set.add(parseInt(key));
                            console.log(parseInt(key));
                        }

                        // Populate the members multiselect.
                        // If a user's userID is in 'members_of_this_group_set', make it selected.

                        // Get the multi select element
                        members_of_group_multiselect = $('#edit_members_of_group')

                        for (var key in all_users_dict) {
                            if (key in members_of_this_group_set) {
                                members_of_group_multiselect.append('<option value="' + key + '" selected="selected">' + all_users_dict[key] + '</option>');
                            }
                            else {
                                members_of_group_multiselect.append('<option value="' + key + '">' + all_users_dict[key] + '</option>');
                            }
                        }

                        // Define some special options for the multiselect.
                        // Call to make it a multiselect.
                        members_of_group_multiselect.multiselect({
                            includeSelectAllOption: true,
                            enableFiltering: true,
                            filterPlaceholder: 'Search...',
                            maxHeight: 400,
                        });

                        // Now populate the remainder of our controls:

                        max_priority = $('#max_priority').val();

                        $("#edit-group-priority").html("");
                        for (var i = 0; i <= max_priority; i++) {
                            // It's important to initialise the controls with current values
                            if (priority == i) {
                                $("#edit-group-priority").append('<option selected="selected" value="' + i + '">' + i + '</option>');
                            }
                            else {
                                $("#edit-group-priority").append('<option value="' + i + '">' + i + '</option>');
                            }
                        }

                        $("#edit-group-modal-title").text("Edit: " + name);
                        $("#edit-group-name").val(name).prop('disabled', false);
                        $("#edit-group-priority").prop('disabled', false);
                    }

                    function get_members_of_group_errorFunc(error) {
                        // Do nothing.
                        //alert('error getting members of group.');
                    }
                }

                function get_general_admin_users_errorFunc(error) {
                    // Do nothing.
                    //alert('error getting general and admin users.');
                }
            });
        });
    }

    function get_groups_errorFunc(error) {
        // Do nothing.
    }

    //--------------------------------------------------------------------

    

    //-----------------------------------------------------------------------------------------

    // Click handler for submit button on modal form
    $("#update-group-button").on("click", function (e) {
        e.preventDefault();

        var form = $("#edit-group-form");

        // Inject a hidden field into the form for the UserID
        form.append('<input type="hidden" name="groupID" value="' + group_edit_invoker_value + '" />');

        form.submit();

    });

});