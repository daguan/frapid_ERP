$("#SaveButton").click(function () {
    function request(model) {
        var url = window.saveUrl;
        var data = JSON.stringify(model);
        return window.getAjaxRequest(url, "POST", data);
    };

    function getModel() {
        var editor = window.ace.edit("editor");
        var contents = editor.getSession().getValue();

        return {
            Type: window.type,
            Contents: contents
        };
    };

    var model = getModel();
    var ajax = request(model);

    ajax.success(function () {
        window.displaySuccess();
    });
});

var stringUnEncode = function (str) {
    return str.replace(/&amp;/g, '&').replace(/&quot;/g, "\"");
};

function initializeAceEditor() {
    if (!window.ace) {
        return;
    };

    window.html = stringUnEncode(window.html);
    var content = window.html;

    var editor = window.ace.edit("editor");
    editor.$blockScrolling = Infinity;
    $("#editor").removeClass("initially, hidden");
    editor.setTheme("ace/theme/sqlserver");
    editor.getSession().setMode("ace/mode/html");
    editor.setValue(content, -1);
};

initializeAceEditor();
