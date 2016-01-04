var bigError = $(".big.error");

$("#LoginForm").submit(function (e) {
    function request(model) {
        var url = "/account/sign-in";
        var data = JSON.stringify(model);

        return window.getAjaxRequest(url, "POST", data);
    };

    function getPassword(email, password) {
        var hashed = new window.jsSHA(email + password, 'TEXT').getHash('SHA-512', 'HEX');
        return hashed;
    };

    e.preventDefault();
    var formEl = $("#LoginForm");
    var isValid = window.validator.validate(formEl);

    if (!isValid) {
        return;
    };

    bigError.html("");
    var segment = $("#SignInSegment");
    segment.addClass("loading");
    var model = window.serializeForm(formEl);
    model.Password = getPassword(model.Email, model.Password);

    var ajax = request(model);

    ajax.success(function (response) {
        if (response) {
            localStorage.setItem("access_token", response);
            window.location = "/dashboard";
        } else {
            bigError.html("Access is denied.");
        };

        segment.removeClass("loading");
    });

    ajax.fail(function () {
        bigError.html("Access is denied.");
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