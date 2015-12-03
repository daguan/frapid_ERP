var bigError = $(".big.error");

$("#SignInButton").click(function () {
    function request(model) {
        var url = "/account/sign-in";
        var data = JSON.stringify(model);
        return window.getAjaxRequest(url, "POST", data);
    };

    function getPassword(email, challenge, password) {
        var hashed = new window.jsSHA(email + password, 'TEXT').getHash('SHA-512', 'HEX');
        hashed = new window.jsSHA(challenge + hashed, 'TEXT').getHash('SHA-512', 'HEX');
        return hashed;
    };

    var formEl = $("#LoginForm");
    var isValid = window.validator.validate(formEl);

    if (!isValid) {
        return;
    };

    var el = $(this);
    bigError.html("");
    var segment = $("#SignInSegment");
    segment.addClass("loading");
    var model = window.getForm(formEl);
    model.Password = getPassword(model.Email, model.Challenge, model.Password);

    var ajax = request(model);
    ajax.success(function (response) {
        if (response.Status) {
            window.location = "/dashboard";
        } else {
            bigError.html(response.Message);
        };

        segment.removeClass("loading");
    });

    ajax.fail(function (xhr) {
        segment.removeClass("loading");
    });

});

$("#SocialLoginCheckbox").change(function () {
    var checked = $(this).is(":checked");
    $(".login.form").hide();

    if (checked) {
        $("#SocialLoginForm").fadeIn(500);
    } else {
        $("#LoginForm").fadeIn(500);
    };
});

function bindOffices() {
    function request() {
        var url = "/account/sign-in/offices";
        return window.getAjaxRequest(url);
    };

    var ajax = request();

    ajax.success(function (response) {
        $(".office.dropdown select").bindAjaxData(response, false, null, "OfficeId", "OfficeName");
        setTimeout(function () {
            var selected = response[0].OfficeId;
            if ($(".office.dropdown").find('option[value=' + selected + ']').length) {
                $(".office.dropdown").dropdown("set selected", selected);
            };
        }, 100);
    });
};

function bindLanguages() {
    function request() {
        var url = "/account/sign-in/languages";
        return window.getAjaxRequest(url);
    };

    var ajax = request();

    ajax.success(function (response) {
        $(".language.dropdown select").bindAjaxData(response, false, null, "CultureCode", "NativeName");

        setTimeout(function () {
            var userLang = navigator.language || navigator.userLanguage;
            if ($(".language.dropdown").find('option[value=' + userLang + ']').length) {
                $(".language.dropdown").dropdown("set selected", userLang);
            } else {
                $(".language.dropdown").dropdown("set selected", "en-US");
            };
        }, 100);
    });
};

$(document).ready(function () {
    $(".dropdown").dropdown();
    window.validator.initialize($("#LoginForm"));
    bindOffices();
    bindLanguages();
});