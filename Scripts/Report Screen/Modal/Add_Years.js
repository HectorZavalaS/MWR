/*
 * This function is responsible to add new child nodes (options) into year-select element.
 * It starts in 2022 and goes through the current year.
 * 
 * This script has no slopes or cases, so it just needs to run a for-loop that starts in 
 * 2022 year until the current year.
 */


$(document).ready(function () {
    var currentYear = new Date().getFullYear();

    for (var i = 2022; i <= currentYear; i++) {
        $("#sl-y-co").before("<option value='" + i + "' id='sl-y-op" + i + "'>" + i + "</option > ");
    }
});