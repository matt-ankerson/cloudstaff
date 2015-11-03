// Author: Matt Ankerson
// Date: 21 October 2015

$(document).ready(function () {

    add_group_modal = $('#add_group_modal');

    // Launch add group modal on button press.
    $('#add_group_button').click(function (e) {
        add_group_modal.modal();
    });

    // Use AJAX to fetch all users 
    var get_users_url = "/Home/GetGeneralAndAdminUsersOrdered";

    $.ajax({
        type: "GET",
        url: get_users_url,
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: get_appropriate_users_success_func,
        error: get_appropriate_users_error_func
    });

    function get_appropriate_users_success_func(data, status) {
        var dict = data;

        // Get the multi select element
        members_of_group_multiselect = $('#members_of_group')

        for (var key in dict) {
            // key = key
            // value = dict[key]
            members_of_group_multiselect.append('<option value="' + key + '">' + dict[key] + '</option>');
        }

        // Define some special options for the multiselect.
        // Call to make it a multiselect.
        members_of_group_multiselect.multiselect({
            includeSelectAllOption: true,
            enableFiltering: true,
            filterPlaceholder: 'Search...',
            maxHeight: 400,
        });
    }

    function get_appropriate_users_error_func(error) {
        // Do nothing.
    }

});