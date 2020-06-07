var f = $('#theForm');
f.hide();

//var theButton = $('#theButton');
//theButton.on("click", function () {
//    console.log("Buying Items")
//});

var clickProductItems = $(".product-info li");
clickProductItems.on("click", function () {
    console.log($(this).text());
});

var $loginToggle = $("#loginToggle");
var $popupForm = $(".popup-form");
$loginToggle.on("click", function () {
    $popupForm.toggle(100);
});