if (!String.prototype.endsWith) {
    // ReSharper disable once NativeTypePrototypeExtending
    String.prototype.endsWith = function (searchString, position) {
        var subjectString = this.toString();
        if (typeof position !== "number" || !isFinite(position) || Math.floor(position) !== position || position > subjectString.length) {
            position = subjectString.length;
        };

        position -= searchString.length;
        var lastIndex = subjectString.indexOf(searchString, position);
        return lastIndex !== -1 && lastIndex === position;
    };
};


function getForm(el) {
    function getControlName(id) {
        if (id) {
            var conventions = ["InputTel", "InputColor", "InputDate", "InputDateTime", "InputDateTimeLocal", "InputHidden", "InputNumber", "InputSearch", "InputTime", "InputUrl", "InputText", "InputPassword", "InputEmail", "Select", "Checkbox", "TextArea"];
            for (var i = 0; i < conventions.length; i++) {
                var convention = conventions[i];
                if (id.endsWith(convention)) {
                    return id.replace(new RegExp(convention + "$"), "");
                };
            };
        };

        return "";
    };

    var members = el.find(".field input, .field select, .field textarea");
    var form = {};

    members.each(function () {
        var item = $(this);
        var id = item.attr("id");
        var name = item.attr("name");
        if (id) {
            var val = item.val();
            var member = getControlName(id);

            if (member) {
                form[member] = val;
            };
        } else if(name){
            var val = item.val();
            var member = getControlName(name);

            if (member) {
                form[member] = val;
            };            
        };
    });

    return form;
};
