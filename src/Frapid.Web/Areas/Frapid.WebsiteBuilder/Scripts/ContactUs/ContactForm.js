$(".phone.number.field").hide();

$(document).ready(function () {
    window.validator.initialize($(".contact.form"));
});

function sendEmail(el) {
    function request(model) {
        var url = "/contact-us";
        var data = JSON.stringify(model);
        return window.getAjaxRequest(url, "POST", data);
    };

    function validate(el) {
        var isValid = window.validator.validate(el);

        var hp = el.find(".phone.number.field input").val();

        if (hp) {
            isValid = false;
        };

        return isValid;
    };


    el = $(el);
    var formEl = el.closest(".contact.form");
    var isValid = validate(formEl);

    if (!isValid) {
        return;
    };


    formEl.addClass("loading");
    var model = window.getForm(formEl);


    var ajax = request(model);
    ajax.success(function () {
        var message = '<div class="ui positive message">Thank you for contacting us.</div>';
        formEl.html(message).removeClass("loading");
    });
};