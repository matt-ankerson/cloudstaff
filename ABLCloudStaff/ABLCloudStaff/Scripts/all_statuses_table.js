// Author: Matt Ankerson
// Date: 16 July 2015

$(document).ready(function () {

    // Use ajax to fetch all available statuses, inject as table rows into the status table
    var get_statuses_url = "/Admin/GetAllStatuses";

    $.ajax({
        type: "GET",
        url: get_statuses_url,
        data: null,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: success_func,
        error: errorFunc
    });

    function success_func(data, status) {
        var list = data;

        for(var i = 0; i < list.length; i++)
        {
            $("#all-statuses-table").append('<tr>' +
                '<td>' + list[i].statusID + '</td>' +
                '<td>' + list[i].name + '</td>' +
                '<td>' + list[i].available + '</td>' +
                '<td>' + '<button id="' + list[i].statusID + '" class="edit-status-button btn btn-default">' +
                '<span class="glyphicon glyphicon-pencil"></span>' +
                '<input type="hidden" id="name-value" value="' + list[i].name + '" />' +
                '<input type="hidden" id="worksite-value" value="' + list[i].available + '" />' +
                '</button>' + '</td>' +
                '</tr>');
        }

        //----------------------------------------------------------------------------------------

        invoker_id = 0;

        // Launch modal when a edit button is clicked
        $(".edit-status-button").on('click', function (e) {
            // Launch the modal
            $("#edit-status-modal").modal();
            // Get the invoking element
            invoker = $(this);
            // Get the invoking element's id
            invoker_id = this.id;

            // We need to populate fields on the form with default values
            // Get all values we need
            var statusID = invoker_id;
            var name = invoker.find("#name-value").val();
            var available = invoker.find("#worksite-value").val();

            // This will fire when the modal has fully revealed itself.
            $('#edit-status-modal').on('shown.bs.modal', function (e) {

                $("#edit-status-modal-title").text("Edit: #" + statusID + " - " + name);

                // We need to populate and enable all fields on the form
                $("#edit-status-name").val(name).prop('disabled', false);
                $("#edit-status-available").prop('disabled', false);
                $("#edit-status-available").prop('checked', false);

                if (available == "True") {
                    $("#edit-status-available").prop('checked', true);
                }

            });
        });
    }

    function errorFunc(error) {
        //alert('error' + error.responseText);
    }

    // Click handler for submit button on modal form
    $("#update-status-button").on("click", function (e) {

        e.preventDefault();

        var form = $("#edit-status-form");

        // Inject a hidden field into the form for the StatusID
        form.append('<input type="hidden" name="statusID" value="' + invoker_id + '" />');

        form.submit();

    });

    // Unbind the trigger for our modal form submission
    // ...doing this stops the form being submitted multiple times.
    $('#edit-status-modal').on('hide.bs.modal', function (e) {
        $("#edit-status-modal form").trigger('submit');
        $("#edit-status-modal form").unbind('submit');
    });
});