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
                    list[i].Name +
                    '<span class="pull-right badge">Out</span>' +
                    '</button>');
            }
            else {
                // Group is in. (or 'Inactive')
                list_group.append('<button type="button" class="list-group-item btn_group_is_in btn_group_name" data-dismiss="modal" value="' + list[i].GroupID + '">' +
                    '<div class="group_name_inner">' + list[i].Name + '</div>' +
                    '<span class="pull-right badge">In</span>' +
                    '</button>');
            }
            
        }

        // Open group member modal when a group is selected from the 'see groups' modal.
        $(".btn_group_name").on('click', function (e) {
            // Inject the GroupID into an element on the modal
            $('#group_id_for_members').val($(this).val());
            group_name = $(this).find('.group_name_inner').text();
            // Inject the Group name into the modal heading.
            $('#member_list_heading').text(group_name);
            // Launch the members modal 
            $('#see_group_members_modal').modal();
            
        });
    }

    function get_groups_errorFunc(error) {
        // Inject a button explaining an error has occurred.
        list_group.append('<button type="button" class="list-group-item">An Error Occurred.</button>');
    }
});