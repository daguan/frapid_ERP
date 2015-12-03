$(".email.address.field").hide();

$(document).ready(function () {
    window.validator.initialize($(".signup.segment"));
});

function getPassword(username, password) {
    var hex = new window.jsSHA(username + password, 'TEXT').getHash('SHA-512', 'HEX');
    return hex;
};

$("#EmailInputEmail").blur(function () {
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
            $(".big.error").html("");
            window.removeDirty(el);
            $("#SignUpButton").removeClass("disabled");
        } else {
            $(".big.error").html("This email address is already in use.");
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
        $(".big.error").html("");
        var formEl = $(".signup.segment");

        if (!$("#AgreementCheckbox").is(":checked")) {
            $(".big.error").html("Please agree to terms and conditions to create an account.");
            return false;
        };

        var isValid = window.validator.validate(formEl);

        var hp = $("#EmailAddressInput").val();

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


    var formEl = $(".signup.segment");
    formEl.addClass("loading");
    var model = window.getForm(formEl);

    model.Password = getPassword(model.Email, model.Password);
    model.ConfirmPassword = getPassword(model.Email, model.ConfirmPassword);


    var ajax = request(model);
    ajax.success(function (response) {
        if (response) {
            window.location = "/account/sign-up/confirmation-email-sent";
        };
    });
});
