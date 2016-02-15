$(document).ready(function ($) {
    if ($.fn.dropdown) {
        $(".dropdown").dropdown();
    };

    var el = $('.stories');
    el.sliderPro({
        width: '100%',
        autoplayDelay: 12000,
        fadeDuration: 800,
        arrows: true,
        buttons: true,
        fade: true,
        fadeOutPreviousSlide: true,
        visibleSize: '100%',
        forceSize: 'fullWidth',
        slideDistance: 0,
        imageScaleMode: 'contain',
        autoHeight: true,
        responsive: true,
        fullScreen: true,
        waitForLayers: true
    });

    $("a[data-module]").click(function() {
        var target = $(this).attr("data-module");
        $("div[data-module]").hide();
        $("div[data-module=" + target + "]").fadeIn(1000);
    });


    if (localStorage.getItem("resourceCached") == null) {
        setTimeout(function () {
            function cacheResource(url) {
                var client = new XMLHttpRequest();
                client.open('GET', url);
                client.send();
            };

            cacheResource('/Scripts/semantic-ui/semantic.min.css');
            cacheResource('/Scripts/semantic-ui/semantic.min/semantic.min.js');
            localStorage.setItem("resourceCached", "true");
        }, 10000);
    };
});


$(".cookie.policy.accept.button").click(function() {
    window.localStorage.setItem("cookie_policy", "accepted");
    $(".cookie.policy.message").remove();
});

var cookiePolicyWasAccepted = window.localStorage.getItem("cookie_policy") === "accepted";

if (cookiePolicyWasAccepted) {
    $(".cookie.policy.message").remove();
};