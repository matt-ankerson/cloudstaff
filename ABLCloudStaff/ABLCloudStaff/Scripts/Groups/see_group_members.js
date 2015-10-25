// Author: Matt Ankerson
// Date: 25 October 2015

$(document).ready(function () {

    // Handler for modal.show
    $('#see_group_members_modal').on('shown.bs.modal', function (e) {
        // Scrape the groupID off the modal and query for that group's members using AJAX.
        groupID = $('#group_id_for_members').val();

        // Ajax request for members.
        var get_members_url = "/Home/GetMembersOfGroup";

        $.ajax({
            type: "GET",
            url: get_members_url,
            data: {groupID: groupID},
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: get_members_success_func,
            error: get_members_errorFunc
        });

        function get_members_success_func(data, status) {
            list = data;
            // Inject members into member list.

            // Get refine members multiselect
            refine_group_members_multiselect = $('#refine_group_members');
        }

        function get_members_errorFunc(error) {
            // Do nothing.
        }

    });

});