/*
 * This function is responsible to define how many months adding as option into month-select element,
 * which depends the case.
 * 
 * CASE 1
 * The first case is when the user is trying to see the current report, thus we are talking about the 
 * current year, in which case, we are going to add options from January to the current month.
 * 
 * CASE 2
 * The second case is when the user is trying to see a report from 2022 year, whose months go from June 
 * to December.
 * 
 * CASE 3
 * The third case is when the user is trying to see a report without a restriction, it means, the user
 * can choose a month between January and December and he is going to get a valid report.
 * 
 * CASE DEFAULT
 * If the user choose "choose one" nothing is going to happen.
 * 
 */


$(document).ready(function () {
    $("#year-select").change(function () {

        //STEP 1: Declare variables/objects that we are going to use along the process.
        var currentYear = new Date().getFullYear();
        var currentMonth = new Date().getMonth();
        var months = new Array('Choose one...', 'January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'Octuber', 'November', 'December');

        //STEP 2: clean options of select-month element.
        $("#month-select").empty();
        $("#month-select").html("<option value='Choose one...' id='sl-m-co'>Choose one...</option > ");

        //STEP 3: determine what it is our current case.
        if ($("#year-select").val() == currentYear) {
            for (var i = 1; i <= currentMonth; i++) {
                $("#sl-m-co").before("<option value='" + i + "' id='sl-m-op" + i + "'>" + months[i] + "</option > ");
            }
        } else if ($("#year-select").val() == 2022) {
            for (var i = 6; i <= 12; i++) {
                $("#sl-m-co").before("<option value='" + i + "' id='sl-m-op" + i + "'>" + months[i] + "</option > ");
            }
        } else if ($("#year-select").val() != "Choose one...") {
            for (var i = 1; i <= 12; i++) {
                $("#sl-m-co").before("<option value='" + i + "' id='sl-m-op" + i + "'>" + months[i] + "</option > ");
            }
        }
    });
});