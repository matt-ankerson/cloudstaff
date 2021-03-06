﻿// Author: Matt Ankerson
// Date: 6 August 2015

$(document).ready(function () {
    // Handle the switch swipe event
    $('input[name="staff-state"]').on('switchChange.bootstrapSwitch', function (event, state) {

        // Get the userID from the value of this checkbox.
        userID = $(this).val();

        url = "";

        // Assess the state of the checkbox(switch)
        if (state == true)
        {
            // The switch is now on, set the user to 'in office' and reset all other fields.
            url = "/Home/SetStatusIn";
        }
        else
        {
            // The switch is now off, set the user to 'out of office' and reset all other fields.
            url = "/Home/SetStatusOut";
        }

        $.ajax({
            type: "GET",
            url: url,
            data: { userID: userID },
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: success_func,
            error: error_func
        });

        function success_func(data, status) {
            // If the update was performed successfully, we need to reflect the changes in the module box.
            // Check the returned data
            if (data == 'request-failed') {
                // There was a silent issue server-side, do a hard refresh of the page. 
                window.location.href = '/Home/Index';
            }
            else {
                // Update the module box in question.
                // The module box's element id is simply the userID to whom it belongs.
                raw_module = document.getElementById(userID.toString());
                module = $(raw_module);
                // Reset the following elements:
                // - thisUsersStatusID (get from server)
                var thisUsersStatusID = module.find(".thisUsersStatusID");
                thisUsersStatusID.val(data.statusID);
                // - thisUsersLocationID (get from server)
                var thisUsersLocationID = module.find('.thisUsersLocationID');
                thisUsersLocationID.val(data.locationID);
                // - status_location_details (status contained in 'data', remove location)
                var status_location_details = module.find('.status_location_details');
                status_location_details.html(data.status + '<br /><br />');
                // - time_details (remove time details)
                var time_details = module.find('.time_details');
                time_details.html('<small><br /><br /></small>');
            }
        }

        function error_func() {
            // The update was unsuccessful.
            // Invoke a page refesh, so the switch goes back to representing information consistent with server-side
            window.location.href = '/Home/Index';
        }
    });
});