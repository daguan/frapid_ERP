function popUnder(div, button) {
    $(".popunder").hide();
    div.removeClass("initially hidden");
    div.show().position({
        my: "left top",
        at: "left bottom",
        of: button
    });
};

$(document).on("viewready", function () {
    window.localize();

    flagButton.click(function () {
        popUnder(flagPopUnder, flagButton);
    });

    if (hasVerfication()) {
        var verifyButton = $("#VerifyButton");

        verifyButton.click(function () {
            popUnder(verificationPopUnder, verifyButton);
        });
    };

});

function triggerViewReadyEvent() {
    if (!window.viewReady) {
        window.viewReady = true;
        $(document).trigger("viewready");
    };
};