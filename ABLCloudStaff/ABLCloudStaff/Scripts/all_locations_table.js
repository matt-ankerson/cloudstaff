// Author: Matt Ankerson
// Date: 23 July 2015
$(document).ready(function () {

    // Use ajax to fetch all available locations, inject as table rows into the location table
    var get_locations_url = "/Admin/GetAllLocations";

    $.ajax({
        type: "GET",
        url: get_locations_url,
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: success_func,
        error: errorFunc
    });

    function success_func(data, status) {
        var list = data;

        for (var i = 0; i < list.length; i++) {
            $("#all-locations-table").append('<tr>' +
                '<td>' + list[i].locationID + '</td>' +
                '<td>' + list[i].name + '</td>' +
                '<td>' + '<button id="' + list[i].locationID + '" class="edit-location-button btn btn-default">' +
                '<span class="glyphicon glyphicon-pencil"></span>' +
                '<input type="hidden" id="loc-name-value" value="' + list[i].name + '" />' +
                '</button>' + '</td>' +
                '</tr>');
        }

        //----------------------------------------------------------------------------------------

        invoker_id = 0;

        // Launch modal when a edit button is clicked
        $(".edit-location-button").on('click', function (e) {
            // Launch the modal
            $("#edit-location-modal").modal();
            // Get the invoking element
            invoker = $(this);
            // Get the invoking element's id
            invoker_id = this.id;

            // We need to populate fields on the form with default values
            // Get all values we need
            var locationID = invoker_id;
            var name = invoker.find("#loc-name-value").val();

            // This will fire when the modal has fully revealed itself.
            $('#edit-location-modal').on('shown.bs.modal', function (e) {

                $("#edit-location-modal-title").text("Edit: #" + locationID + " - " + name);

                // We need to populate and enable all fields on the form
                $("#edit-location-name").val(name).prop('disabled', false);

            });
        });
    }

    function errorFunc(error) {
        alert('error' + error.responseText);
    }

    // Click handler for submit button on modal form
    $("#update-location-button").on("click", function (e) {

        e.preventDefault();

        var form = $("#edit-location-form");

        // Inject a hidden field into the form for the StatusID
        form.append('<input type="hidden" name="locationID" value="' + invoker_id + '" />');

        form.submit();

    });

    // Unbind the trigger for our modal form submission
    // ...doing this stops the form being submitted multiple times.
    $('#edit-location-modal').on('hide.bs.modal', function (e) {
        $("#edit-location-modal form").trigger('submit');
        $("#edit-location-modal form").unbind('submit');
    });
});