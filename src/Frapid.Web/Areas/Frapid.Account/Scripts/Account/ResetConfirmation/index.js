$("#ConfirmEmailInputEmail").hide();
$(document).ready(function () {
    window.validator.initialize($(".reset.password.segment"));
});


$("#SetPasswordButton").click(function () {
    function request(token, password) {
        var url = "/account/reset/confirm?token=" + token;
        url += "&password=" + password;

        return window.getAjaxRequest(url, "POST");
    };

    function validate() {
        $(".big.error").html("");
        var formEl = $(".reset.password.segment");
        var isValid = window.validator.validate(formEl);
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
    var token = window.getQueryStringByName("token");

    var ajax = request(token, model.Password);

    ajax.success(function (response) {
        if (response) {
            window.location = "/account/sign-in";
        };
    });
});
