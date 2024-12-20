/* 
 * This script is listenting to resize the screen, due to the size of the screen changes regurlarly, 
 * CSS rules has to change too. When the width screen is larger than 851 px, it's necessary change 
 * logo-image for an image that contrast with the white background. On the other hand, when the width 
 * screen is shorter than 851 px, it's necessary change logo-image for an image that contrast with the 
 * dark background.
 * 
 */


//When the screen is ready, this function is called to check the size of the screen.
$(document).ready(function () {
    changeImg();
})

//When the size screen change, this listener catch the event and call changeImg function.
window.addEventListener("resize", function () {
    changeImg();
});

function changeImg() {
    if (window.matchMedia('screen and (min-width: 851px)').matches) {
        $(".img-logo").attr("src", "/Assets/Logo.png");
    } else if (window.matchMedia('screen and (max-width: 850px)').matches) {
        $(".img-logo").attr("src", "/Assets/logo-s.png");
    }
}