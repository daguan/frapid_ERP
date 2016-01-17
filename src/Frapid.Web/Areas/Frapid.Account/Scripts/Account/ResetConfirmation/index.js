$("#ConfirmEmailInputEmail").hide();
$(document).ready(function () {
    window.validator.initialize($(".reset.plainPassword.segment"));
});


$("#SetPasswordButton").click(function () {
    function request(token, password) {
        var url = "/account/reset/confirm?token=" + token;
        url += "&plainPassword=" + password;

        return window.getAjaxRequest(url, "POST");
    };

    function validate() {
        $(".big.error").html("");
        var formEl = $(".reset.plainPassword.segment");
        var isValid = window.validator.validate(formEl);
        return isValid;
    };

    $(".big.error").html("");

    var isValid = validate();
    if (!isValid) {
        return;
    };


    var formEl = $(".reset.plainPassword.segment");
    formEl.addClass("loading");
    var model = window.serializeForm(formEl);
    var token = window.getQueryStringByName("token");

    var ajax = request(token, model.Password);

    ajax.success(function (response) {
        if (response) {
            window.location = "/account/sign-in";
        };
    });
});
