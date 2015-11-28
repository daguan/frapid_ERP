var validator = new function () {
    var isValid = true;
    var requiredFieldMessage = window.requiredFieldMessage || "This field is required.";//Fallback to english language.

    this.logError = function (message) {
        console.log(message);
    };

    var validateField = function (field) {
        var val = field.val();

        if (window.isNullOrWhiteSpace(val)) {
            this.isValid = false;
            window.makeDirty(field);
        } else {
            window.removeDirty(field);
        };
    };

    this.initialize = function (el) {
        el.find(".dropdown input.search").blur(function () {
            $(this).parent().find("select").trigger("blur");
        });

        el.find("[required]:not(:disabled):not([readonly])").blur(function () {
            var field = $(this);
            validateField(field);
        });
    };

    this.validate = function (el, oninvalid, log) {
        var required = el.find(".image.form-field, [required]:not(:disabled):not([readonly]):visible");
        required.trigger("blur");

        if (jQuery().timepicker) {
            $(".ui-timepicker-input").timepicker("hide");
        };

        var emailFields = el.find('input[type=email]:visible');

        $.each(emailFields, function () {
            var el = $(this);
            var val = el.val();

            var exp = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!exp.test(val)) {
                window.makeDirty(el);
                isValid = false;
                return false;
            };
        });

        var errorFields = el.find(".error:not(.big.error)");

        isValid = errorFields.length === 0;

        $.each(errorFields, function (i, v) {
            var field = $(v);
            var label = field.closest(".field").find("label");
            var message = label.html() + " : " + requiredFieldMessage;

            if (log) {
                console.log(message);
            };
        });

        var expressions = el.find("[data-validation-expression]:visible");

        expressions.each(function () {
            var el = $(this);
            var val = el.val();
            var expression = el.attr("data-validation-expression");
            var message = el.attr("data-validation-message");
            var target = $(el.attr("data-validation-target"));
            target.html("");

            if (!val.match(expression)) {
                window.makeDirty(el);
                isValid = false;
                target.html(message);
                return false;
            };
        });


        var matchTargets = el.find("[data-match-target]");

        matchTargets.each(function () {
            var el = $(this);
            var name = el.siblings("label").html();
            var val = el.val();

            var targetSelector = "#" + el.attr("data-match-target");
            var target = $(targetSelector);
            var targetVal = target.val();
            var targetName = target.siblings("label").text();

            if (val !== targetVal) {
                window.makeDirty(el);
                window.makeDirty(target);
                alert("The " + name + " does not match with " + targetName + ".");
                isValid = false;
                return false;
            };
        });

        if (!isValid) {
            if (typeof (oninvalid) === "function") {
                oninvalid(field);
            };
        };

        return isValid;
    };
};
