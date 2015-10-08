// Author: Matt Ankerson
// Date: 6 July 2015

$(document).ready(function () {
    // Launch modal when a module is clicked
    $(".staff-modules").on('click', function (e) {

        // Check if this is a visitor modal.
        if ($(this).hasClass('visitor-modules')) {
            // Don't launch modal.
            e.preventDefault();
        }
        else {
            // Launch the modal
            $("#myModal").modal();
            // Get the invoking element
            invoker = $(this);
            // Get the invoking element's id
            invoker_id = this.id;

            // This function exists within the click function so it can have access to the
            // element which was clicked.
            $('#myModal').on('shown.bs.modal', function (e) {

                // Get the name of the selected user
                this_user = invoker.find(".staff-name-text").text();

                // Get the status, location (and ETA) of the person who was clicked.
                thisUsersStatusID = invoker.find(".thisUsersStatusID").val();
                thisUsersLocationID = invoker.find(".thisUsersLocationID").val();

                // Change the modal title.
                $("#modal-title").text(this_user);

                // Populate the Status and Location Dropdowns
                var getStatusesURL = '/Home/GetStatusesAjax';
                var getLocationsURL = '/Home/GetLocationsAjax';

                // Get statuses
                $.ajax({
                    type: "GET",
                    url: getStatusesURL,
                    data: { userID: invoker_id },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: get_statuses_success_func,
                    error: errorFunc
                });

                // Get locations
                $.ajax({
                    type: "GET",
                    url: getLocationsURL,
                    data: { userID: invoker_id },
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: get_locations_success_func,
                    error: errorFunc
                });

                function get_statuses_success_func(data, status) {
                    var list = data;
                    // Populate the Dropdown list for statuses
                    $.each(list, function (index, item) {
                        // If the status index is equal to 'thisUsersStatusID', make the option selected
                        if (index == thisUsersStatusID) {
                            $("#status-list").append('<option value="' + index + '" selected>' + item + '</option>');
                        }
                        else {
                            $("#status-list").append('<option value="' + index + '">' + item + '</option>');
                        }
                    });
                }

                function get_locations_success_func(data, status) {
                    var list = data;
                    // Populate the Dropdown list for locations
                    $.each(list, function (index, item) {
                        // if the location index is equal to 'thisUsersLocationID', make the option selected.
                        if (index == thisUsersLocationID) {
                            $("#location-list").append('<option value="' + index + '" selected>' + item + '</option>');
                        }
                        else {
                            $("#location-list").append('<option value="' + index + '">' + item + '</option>');
                        }
                    });
                }

                function errorFunc(error) {
                    //alert('error' + error.responseText);
                    // There was a silent issue server-side, do a hard refresh of the page. 
                    window.location.href = '/Home/Index';
                }
            });

            //----------------------------------------------------------------------

            // Click handler for submit button on modal form
            $("#update-singular-user-state").on("click", function (e) {

                e.preventDefault();

                if ($("#status-list").val != null && $("location-list").val != null) {
                    var form = $("#form-update-status-or-location");

                    // Inject a hidden field into the form for the UserID
                    form.append('<input type="hidden" name="userID" value="' + invoker_id + '" />');

                    form.submit();
                }
            });

            //----------------------------------------------------------------------

            // Unbind the trigger for our modal form submission
            // ...doing this stops the form being submitted multiple times.
            $('#myModal').on('hide.bs.modal', function (e) {
                $("#myModal form").trigger('submit');
                $("#myModal form").unbind('submit');
            });
        }

        
    });
});