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
                list_group.append('<button type="button" class="list-group-item btn_group_is_out" value="' + list[i].GroupID + '">' +
                    list[i].Name +
                    '<span class="pull-right">Out</span>' +
                    '</button>');
            }
            else {
                // Group is in. (or 'Inactive')
                list_group.append('<button type="button" class="list-group-item btn_group_is_in" value="' + list[i].GroupID + '">' +
                    list[i].Name +
                    '<span class="pull-right">In</span>' +
                    '</button>');
            }
            
        }
    }

    function get_groups_errorFunc(error) {
        // Inject a button explaining an error has occurred.
        list_group.append('<button type="button" class="list-group-item">An Error Occurred.</button>');
    }

    // Event handler for clicking a group's button. Save the groupID to global scope.
    //$('')
});