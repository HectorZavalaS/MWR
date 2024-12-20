/*
 * This function is used to detect when the screen is resized (getting short o large), which is so useful 
 * due to it helps us to determine if it is necessary using red or yellow color to show messages like:
 *      - Attempt to login invalid
 *      - This field must be filled.
 *      - etc.
 * 
 * If the screen has a white background, a better option is using red color to show messages 
 * because they contrast perfectly with each other. On the other hand, when the screen has a dark
 * background, it is better if we use a light color, which, in our case, is yellow color.
 * 
 * In brief, if the screen is shorter than 851px, the message is displayed in yellow color. Otherwise, 
 * the message is displayed in red color.
 * 
 */

$(document).ready(function () {
    changeTC();
});

window.addEventListener("resize", function () {
    changeTC();
});

function changeTC() {
    if (window.matchMedia('screen and (min-width: 851px)').matches) {

        $("#failed-login-warning").removeClass("text-warning");
        $("#missed-value-1").removeClass("text-warning");
        $("#missed-value-2").removeClass("text-warning");

        $("#failed-login-warning").addClass("text-danger");
        $("#missed-value-1").addClass("text-danger");
        $("#missed-value-2").addClass("text-danger");


    } else if (window.matchMedia('screen and (max-width: 850px)').matches) {

        $("#failed-login-warning").removeClass("text-danger");
        $("#missed-value-1").removeClass("text-danger");
        $("#missed-value-2").removeClass("text-danger");

        $("#failed-login-warning").addClass("text-warning");
        $("#missed-value-1").addClass("text-warning");
        $("#missed-value-2").addClass("text-warning");

    }
}