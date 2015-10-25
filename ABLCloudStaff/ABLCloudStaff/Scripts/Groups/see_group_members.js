// Author: Matt Ankerson
// Date: 25 October 2015

$(document).ready(function () {

    // Handler for modal.show
    $('#see_group_members_modal').on('shown.bs.modal', function (e) {
        // Scrape the groupID off the modal and query for that group's members using AJAX.
        groupID = $('group_id_for_members').val();
        alert(groupID);

    });

});