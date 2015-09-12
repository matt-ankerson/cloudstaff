// Author: Matt Ankerson
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
            // If the update was performed successfully, we don't need to do anything
            if (data == 'request-ok') {

            }
            else {
                // There was an issue, do a hard refresh of the page. 
                window.location.href = '/Home/Index';
            }
        }

        function error_func() {
            // The update was unsuccessful.
            // Invoke a page refesh, so the switch goes back to representing information consistent with server-side
            window.location.href = '/Home/Index';
        }
    });
});