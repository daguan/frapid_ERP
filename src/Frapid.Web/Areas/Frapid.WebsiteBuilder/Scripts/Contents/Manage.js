window.overridePath = "/dashboard/website/contents";
$(".dropdown").dropdown();

function appendTag(select, text, value, selected) {
    if (select.find("option[value='" + value + "']").length) {
        return;
    };

    var option = $("<option />");
    if (selected) {
        option.attr("selected", "selected");
    };

    option.attr("value", value);


    option.html(text);
    select.append(option);
};

$(".tag.dropdown input.search").keyup(function (e) {
    if (e.keyCode === 188) {
        var val = $(this).val();

        appendTag($("#TagsSelect"), val, val, true);
    };
});


$('[data-entity="title"]').keyup(function () {
    function getAlias(title) {
        return title.toLowerCase().replace(/ +(?= )/g, '').replace(/ /g, '-').replace(/[^\w-]+/g, '');
    };

    $('[data-entity="alias"]').val(getAlias($(this).val()));
});

$("#SaveButton").click(function () {
    function request(model) {
        var url = "/api/website/content/add-or-edit";
        var form = [];
        form.push(model);
        form.push(null);

        var data = JSON.stringify(form);
        return window.getAjaxRequest(url, "POST", data);
    };

    var attribute = "data-entity";
    var validationEl = ".error";
    var validationSummary = ".error .bulleted.list";

    var model = window.entityParser.getModel(attribute, validationEl, validationSummary);
    if (!model) {
        return;
    };

    var ajax = request(model);


    ajax.success(function () {
        location.href = "../contents";
    });
});

function displayContent() {
    var editor = window.ace.edit("editor");
    var isMarkdown = $("#IsMarkdownInputCheckbox").is(":checked");
    var contents = editor.getSession().getValue();

    if (isMarkdown) {
        $("input[data-entity='markdown']").val(contents);
    };

    var html;

    if (isMarkdown) {
        html = window.marked(contents);
        $("#content").html(html);
        return;
    };

    html = contents;
    $("#content").html(html);
};

var stringUnEncode = function (str) {
    return str.replace(/&amp;/g, '&').replace(/&quot;/g, "\"");
};

function initializeAceEditor() {
    if (!window.ace) {
        return;
    };

    window.html = stringUnEncode(window.html);

    var content = $("input[data-entity='markdown']").val();

    if (!content) {
        content = html;
    };

    var editor = window.ace.edit("editor");
    editor.$blockScrolling = Infinity;
    $("#editor").removeClass("initially, hidden");
    editor.setTheme("ace/theme/ambience");
    editor.getSession().setMode("ace/mode/html");
    editor.setValue(content, -1);

    editor.on('input', function () {
        displayContent();
    });
};

initializeAceEditor();

$(document).ready(function () {
    window.initalizeSelectApis();
});

function maximize(target, width) {
    var items = $("[data-target]");
    var el = $("[data-target=" + target + "]");
    items.hide();

    if (!el.hasClass('sixteen wide')) {
        el.removeClass(width).addClass('sixteen wide');
        el.fadeIn();
        return;
    };

    el.removeClass('sixteen wide').addClass(width);
    items.fadeIn();
};
