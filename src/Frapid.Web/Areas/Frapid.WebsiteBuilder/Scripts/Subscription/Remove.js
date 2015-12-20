function unsubscribe() {
    function request(model) {
        var url = "/subscription/remove";
        var data = JSON.stringify(model);
        return window.getAjaxRequest(url, "POST", data);
    };

    function validate(el) {
        var isValid = window.validator.validate(el, null, true);

        var hp = el.find(".ui.confirm.email.input input").val();

        if (hp) {
            isValid = false;
        };

        return isValid;
    };

    $("#ConfirmEmailAddressInputEmail").hide();
    var form = $(".subscription.form");

    var isValid = validate(form);

    if (!isValid) {
        return;
    };

    form.addClass("loading");
    var model = window.getForm(form);

    var ajax = request(model);

    ajax.success(function () {
        var thankYou = $(".thank.you");
        form.removeClass("loading").hide();
        thankYou.show();
    });

    ajax.fail(function () {
        form.removeClass("loading");
    });
};