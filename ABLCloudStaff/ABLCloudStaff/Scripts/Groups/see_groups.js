// Author: Matt Ankerson
// Date: 15 October 2015

$(document).ready(function () {

    see_groups_modal = $('#see_groups_modal');

    // Launch see group modal on button press.
    $('#see_groups_button').click(function (e) {
        see_groups_modal.modal();
    });

    // Use ajax to pull groups from the server, inject into the group list.
    var get_users_url = "/Home/GetAllGroups";

    $.ajax({
        type: "GET",
        url: get_users_url,
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: get_groups_success_func,
        error: get_groups_errorFunc
    });


    function get_groups_success_func(data, status) {
        var list = data
        // Inject buttons onto list group on the see groups modal.
        list_group = $('#current_group_list');

        // For each GroupInfo object in the list.
        // Let the value of each button hold the appropriate GroupID.
        for (var i = 0; i < list.length; i++)
        {
            if (list[i].Active == 'True') {
                // Group is out. (or 'Active')
                list_group.append('<button type="button" class="list-group-item btn_group_is_out btn_group_name" data-dismiss="modal" value="' + list[i].GroupID + '">' +
                    '<div class="group_name_inner jsOnly">' + list[i].Name + '</div>' + list[i].Name +
                    '<span class="pull-right badge">Out</span>' +
                    '</button>');
                
            }
            else {
                // Group is in. (or 'Inactive')
                list_group.append('<button type="button" class="list-group-item btn_group_is_in btn_group_name" data-dismiss="modal" value="' + list[i].GroupID + '">' +
                    '<div class="group_name_inner jsOnly">' + list[i].Name + '</div>' + list[i].Name +
                    '<span class="pull-right badge">In</span>' +
                    '</button>');
            }
            
        }

        control_container = $('#alternate_options_for_groups_in_or_out');
        control_container_previous_elements = control_container.contents();

        // Open group member modal when a group is selected from the 'see groups' modal.
        $(".btn_group_name").on('click', function (e) {
            // Inject the GroupID into an element on the modal
            $('#group_id_for_members').val($(this).val());
            group_name = $(this).find('.group_name_inner').text();
            // Inject the Group name into the modal heading.
            $('#member_list_heading').text(group_name);

            // We also need to present a slightly different modal when an 'out' group is pressed.
            // The modal needs to provide a multiselect list of all members, and a single button for returning to the office.
            if ($(this).hasClass('btn_group_is_out')) {
                
                default_in_status = $('#default_in_status_id').val();
                default_in_location = $('#default_in_location_id').val();
                // Clear the container contents.
                control_container.html('');
                // Inject appropriate controls.
                control_container.html('<input type="hidden" name="statusID" value="' + default_in_status + '" />' +
                                       '<input type="hidden" name="locationID" value="' + default_in_location + '" />' +
                                       '<input type="hidden" name="returnTime" value="" />' +
                                       '<button id="update-group-submit" type="submit" class="btn btn-info btn-block">Return to Office</button>');
            }
            else {
                // Show the ordinary controls. (For a group which is in)
                control_container.html('');
                control_container.append(control_container_previous_elements);
            }

            // Launch the members modal 
            $('#see_group_members_modal').modal();
            
        });
    }

    function get_groups_errorFunc(error) {
        // Inject a button explaining an error has occurred.
        list_group.append('<button type="button" class="list-group-item">An Error Occurred.</button>');
    }
});