/*
 * This small function is responsible to remove the "multiple" attribute in both "year" and 
 * "month" select elements. In the case of "year-select" element is necessary getting the
 * first child node (when the web page is ready it should have just one child), and set it up
 * with an ID, whose value would be "sl-y-co" (select-year-chooseone).
 * 
 * This script allows the correct execution of the Add_Month and Add_Years scripts, which need 
 * the year-select and month-select elements don't have "multiple" attribute.
 * 
 */

$(document).ready(function () {
    $("#year-select").removeAttr("multiple");
    $("#month-select").removeAttr("multiple");

    var a = $("#year-select").children();
    a[0].setAttribute("id", "sl-y-co");
});