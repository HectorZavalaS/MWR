/*
 * This script is in charge of resetting the F-Modal, to keep it clean. 
 * That is, keeping it without previously entered data or options. In order
 * to get this, we only need to set the value of the year-select element to
 * "Choose one..." and then trigger the change.
 */


$(document).ready(function () {
    $("#F-REPORT").on("hidden.bs.modal", function () {
        $("#year-select").val("Choose one...").trigger("change");
    });
});