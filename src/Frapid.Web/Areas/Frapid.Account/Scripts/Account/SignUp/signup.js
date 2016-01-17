var bigError = $(".big.error");
$(".email.address.field").hide();

$(document).ready(function () {
    window.validator.initialize($(".signup.segment"));
});

$("#EmailInputEmail").change(function () {
    function request(email) {
        var url = "/account/sign-up/validate-email?email=" + email;
        return window.getAjaxRequest(url, "POST");
    };

    var el = $(this);
    var email = el.val();

    if (!email) {
        return;
    };

    $("#SignUpButton").addClass("disabled");
    var ajax = request(email);

    ajax.success(function (response) {
        if (response) {
            bigError.html("");
            window.removeDirty(el);
            $("#SignUpButton").removeClass("disabled");
        } else {
            bigError.html("This email address is already in use.");
            window.makeDirty(el);
        };
    });
});

$("#SignUpButton").click(function () {
    function request(model) {
        var url = "/account/sign-up";
        var data = JSON.stringify(model);
        return window.getAjaxRequest(url, "POST", data);
    };

    function validate() {
        bigError.html("");
        var formEl = $(".signup.segment");

        if (!$("#AgreementCheckbox").is(":checked")) {
            bigError.html("Please agree to terms and conditions to create an account.");
            return false;
        };

        var isValid = window.validator.validate(formEl);

        var hp = $("#EmailAddressInput").val();

        if (hp) {
            isValid = false;
        };

        return isValid;
    };

    bigError.html("");

    var isValid = validate();
    if (!isValid) {
        return;
    };


    var formEl = $(".signup.segment");
    formEl.addClass("loading");
    var model = window.serializeForm(formEl);


    var ajax = request(model);
    ajax.success(function (response) {
        if (response) {
            window.location = "/account/sign-up/confirmation-email-sent";
        };
    });
});
