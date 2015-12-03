$("#ConfirmEmailInputEmail").hide();
$(document).ready(function () {
    window.validator.initialize($(".reset.password.segment"));
});

$("#EmailInputEmail").blur(function () {
    function request(email) {
        var url = "/account/reset/validate-email?email=" + email;
        return window.getAjaxRequest(url, "POST");
    };

    var el = $(this);
    var email = el.val();

    if (!email) {
        return;
    };

    $("#ResetButton").addClass("disabled");
    var ajax = request(email);

    ajax.success(function (response) {
        if (response) {
            $(".big.error").html("We do not have an account with this email address.");
            window.makeDirty(el);
        } else {
            $(".big.error").html("");
            window.removeDirty(el);
            $("#ResetButton").removeClass("disabled");
        };
    });
});


$("#ResetButton").click(function () {
    function request(model) {
        var url = "/account/reset";
        var data = JSON.stringify(model);

        return window.getAjaxRequest(url, "POST", data);
    };

    function validate() {
        $(".big.error").html("");
        var formEl = $(".reset.password.segment");
        var isValid = window.validator.validate(formEl);

        var hp = $("#ConfirmEmailInputEmail").val();

        if (hp) {
            isValid = false;
        };

        return isValid;
    };

    $(".big.error").html("");

    var isValid = validate();
    if (!isValid) {
        return;
    };


    var formEl = $(".reset.password.segment");
    formEl.addClass("loading");
    var model = window.getForm(formEl);
    var token = $("#TokenInputHidden").val();
    model.Token = token;

    var ajax = request(model);
    ajax.success(function (response) {
        if (response) {
            window.location = "/account/reset/email-sent";
        };
    });
});