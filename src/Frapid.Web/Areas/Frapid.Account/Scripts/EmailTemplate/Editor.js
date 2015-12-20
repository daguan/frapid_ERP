$("#SaveButton").click(function () {
    function request(model) {
        var url = "/dashboard/account/email-templates";
        var data = JSON.stringify(model);
        return window.getAjaxRequest(url, "POST", data);
    };

    function getModel() {
        var editor = window.ace.edit("editor");
        var contents = editor.getSession().getValue();

        return {
            Title: window.title,
            Contents: contents
        };
    };

    var model = getModel();
    var ajax = request(model);

    ajax.success(function () {
        window.displaySucess();
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
    editor.renderer.setScrollMargin(10, 10);
    editor.$blockScrolling = Infinity;
    $("#editor").removeClass("initially, hidden");
    editor.setTheme("ace/theme/ambience");
    editor.getSession().setMode("ace/mode/html");
    editor.setValue(content);
};

initializeAceEditor();