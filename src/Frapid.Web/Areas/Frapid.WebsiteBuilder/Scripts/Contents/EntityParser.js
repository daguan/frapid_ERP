var entityParser = {
    attribute: "data-entity",
    validationSummary: ".error .bulleted.list",
    getValue: function (el, raw) {
        function parseValue(value, typeClass) {
            if (!typeClass) {
                return value;
            };

            if (typeClass.indexOf("integer") !== -1) {
                return parseInt(value) || null;
            };

            if (typeClass.indexOf("float") !== -1) {
                return parseFloat(value) || null;
            };

            if (typeClass.indexOf("date") !== -1) {
                return window.parseLocalizedDate(value) || null;
            };

            return null;
        };

        var value = null;
        var required = el.attr("required");
        var tag = el.prop("tagName").toLowerCase();

        switch (tag) {
            case "h1":
            case "h2":
            case "h3":
            case "h4":
            case "h5":
            case "span":
            case "p":
            case "div":
                if (raw) {
                    value = el.html().trim();
                } else {
                    value = el.text().trim();
                };

                break;
            case "input":
                if (el.is(":checkbox")) {
                    value = el.is(":checked");
                } else {
                    value = parseValue(el.val(), el.attr("class"));
                    el.closest(".field").removeClass("error");

                    if (!value && required) {
                        el.closest(".field").addClass("error");
                    };
                };

                break;
            case "select":
                if (el.attr("multiple")) {
                    value = el.val().join(",");
                } else {
                    value = el.val();
                    value = parseValue(value, el.parent().attr("class"));
                };

                break;
        };

        return value;
    },
    getModel: function (attribute, validationEl, validationSummary) {
        var dataEntities = $("[" + attribute + "]");

        var model = {};
        var valid = true;
        var invalidItems = [];

        dataEntities.each(function () {
            var el = $(this);
            var label = el.siblings("label").text();

            if (el.is("select")) {
                label = el.parent().parent().find("label").text();
            };

            var name = el.attr(attribute);
            var raw = el.attr("data-raw");
            var val = entityParser.getValue(el, raw);
            var required = el.attr("required");

            model[name] = val;

            if (required && !val) {
                valid = false;
                invalidItems.push(label);
            };
        });

        if (!valid) {
            var list = $(validationSummary);
            $(validationEl).removeClass("initially, hidden");
            list.html("");

            $.each(invalidItems, function () {
                var item = $("<div class='item' />");
                item.html(this + " is required");
                list.append(item);
            });

            return null;
        };

        return model;
    }
};
