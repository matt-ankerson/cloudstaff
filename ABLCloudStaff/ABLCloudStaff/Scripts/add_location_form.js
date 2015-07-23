// Author: Matt Ankerson
// Date: 23 July 2015

$(document).ready(function () {

    var userID = "";

    // Event handler for the 'all users / one user' checkbox
    $("#add_location_for_one_or_all").change(function () {

        // Assess whether or not the checkbox is checked or not
        if ($(this).is(":checked")) {
            // Disable the option to add status for a single user
            $("#fldst_location_for_single_user").attr("disabled", "disabled");
            // Save and remove the contents of the userID field.
            userID = $("#add-location-userID").val();
            $("#add-location-userID").val("");
        }
        else {
            // Enable the option to add location for a single user
            $("#fldst_location_for_single_user").removeAttr("disabled");
            // Repopulate the userID hidden field
            $("#add-location-userID").val(userID);
        }
    });

});