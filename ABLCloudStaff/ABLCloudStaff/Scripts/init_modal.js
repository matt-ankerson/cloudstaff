// Author: Matt Ankerson
// Date: 6 July 2015

$(document).ready(function () {
    // Launch modal when a module is clicked
    $(".staff-modules").on('click', function (e) {
        // Launch the modal
        $("#myModal").modal();
        // Get the invoking element
        invoker = $(this);

        // This function exists within the click function so it can have access to the
        // element which was clicked.
        $('#myModal').on('shown.bs.modal', function (e) {
            // Get the name of the selected user
            this_user = invoker.find(".staff-name-text").text();
            // Change the modal title.
            $("#modal-title").text(this_user);
        });
    });
});