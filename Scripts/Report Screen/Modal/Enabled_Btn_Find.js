/*
 * This function is responsible to put the find button on enabled or disabled,
 * depending the entered value in every select element. This is because the user 
 * should not be able to click on "Find" if the form is not filled out. In order 
 * to perform this task, we need to determine what value has entered by the user 
 * in every select-element.
 * 
 * CASE 1: YEAR-SELECT ELEMENT IS CHANGED
 *         Our first case is when the user changes his selected option in the
 *         year field. It doesn't matter what option he chooses, since 
 *         month-select field is going to acquire/get "Choose one..." value and the 
 *         btn-find button will be disabled.
 * 
 * CASE 2: VALUE OF MONTH-SELECT ELEMENT IS NOT A MONTH (Choose one...)
 *         This case can have two slopes to activate it. The first one 
 *         occurs when the user has selected a year and want to select an
 *         option in month-select element, but that one is "Choose one..." option.
 *         The second one occurs when the user changes his selected option,
 *         which is a valid option, for "Choose one..." option. However, no matter 
 *         what the case, the btn-find button is going to be disabled, since the 
 *         user has selected "Choose one..." option, which is not a valid option 
 *         for the system.
 * 
 * CASE 3: VALUE OF MONTH-SELECT ELEMENT IS A MONTH
 *         Finally, this third case takes place when the user select a valid option,
 *         it means, the user has selected a month in the respective field. This results
 *         in the activation of the btn-find button.
 * 
 */

$(document).ready(function () {
    $("#month-select").change(function () {

        if ($("#month-select").val() != "Choose one...") {
            $("#btn-find").removeAttr("disabled");
            $("#btn-find").attr("enabled", "enabled");
        } else {
            $("#btn-find").removeAttr("enabled");
            $("#btn-find").attr("disabled", "disabled");
        }
    });

    $("#year-select").change(function () {
        $("#month-select").val("Choose one...");
        $("#btn-find").removeAttr("enabled");
        $("#btn-find").attr("disabled", "disabled");
    });

});