// Author: Matt Ankerson
// Date: 21 July 2015

$(document).ready(function () {

    // Event handler for the 'all users / one user' checkbox
    $("#add_status_for_one_or_all").change(function () {

        // Assess whether or not the checkbox is checked or not
        if($(this).is(":checked"))
        {
            // Disable the option to add status for a single user
        }
        else
        {
            // Enable the option to add status for a single user
        }
    });

});