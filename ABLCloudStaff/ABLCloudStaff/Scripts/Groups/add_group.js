// Author: Matt Ankerson
// Date: 21 October 2015

$(document).ready(function () {

    add_group_modal = $('#add_group_modal');

    // Launch add group modal on button press.
    $('#add_group_button').click(function (e) {
        add_group_modal.modal();
    });

});