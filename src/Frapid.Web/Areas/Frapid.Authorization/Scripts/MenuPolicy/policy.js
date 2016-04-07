$("[data-toggle-allow]").click(function() {
    var el = $(this);

    var state = el.prop("checked");
    var table = el.closest("table");

    if (!state) {
        $("[data-menu-allow]").removeAttr("checked");
        return;
    };

    table.find("[data-menu-allow]").prop("checked", true).trigger("change");
});

$("[data-menu-allow]").change(function() {
    var el = $(this);
    var target = el.parent().parent().find("[data-menu-deny]");
    target.removeAttr("checked");
});

$("[data-menu-deny]").change(function() {
    var el = $(this);
    var target = el.parent().parent().find("[data-menu-allow]");
    target.removeAttr("checked");
});

$("[data-toggle-deny]").click(function() {
    var el = $(this);

    var state = el.prop("checked");
    var table = el.closest("table");

    if (!state) {
        $("[data-menu-deny]").removeAttr("checked");
        return;
    };

    table.find("[data-menu-deny]").prop("checked", true).trigger("change");
});


$(window).keypress(function(event) {
    if (!(event.which === 115 && event.ctrlKey) && !(event.which === 19)) return true;
    save();
    event.preventDefault();
    return false;
});

function save() {
    function request(model) {
        var url = "/dashboard/authorization/menu-access/user-policy";
        var data = JSON.stringify(model);
        return window.getAjaxRequest(url, "PUT", data);
    };

    var confirmed = window.confirmAction();
    if (!confirmed) {
        return;
    };

    var userId = parseInt($("#UserSelect").val() || 0);
    var officeId = parseInt($("#OfficeSelect").val() || 0);

    if (!userId || !officeId) {
        return;
    };

    var allowed = $("[data-menu-allow]:checked").map(function () {
        return $(this).attr("data-menu-id");
    }).get();

    var disallowed = $("[data-menu-deny]:checked").map(function () {
        return $(this).attr("data-menu-id");
    }).get();

    var model = {
        UserId: userId,
        OfficeId: officeId,
        Allowed: allowed || [],
        Disallowed: disallowed || []
    };

    var ajax = request(model);

    ajax.success(function () {
        window.displaySuccess();
    });

    ajax.fail(function (xhr) {
        window.logAjaxErrorMessage(xhr);
    });
};

$("[data-save-button]").off("click").on("click", function () {
    save();
});

$("[data-get-menu-policy-button]").off("click").on("click", function() {
    function request(officeId, userId) {
        var url = "/dashboard/authorization/menu-access/user-policy/{officeId}/{userId}";
        url = url.replace("{officeId}", officeId);
        url = url.replace("{userId}", userId);

        return window.getAjaxRequest(url);
    };

    var userId = parseInt($("#UserSelect").val() || 0);
    var officeId = parseInt($("#OfficeSelect").val() || 0);

    if (!userId || !officeId) {
        return;
    };

    $("[data-toggle-allow]").removeAttr("checked");
    $("[data-toggle-deny]").removeAttr("checked");
    $("[data-menu-id]").removeAttr("checked");

    var ajax = request(officeId, userId);

    ajax.success(function(response) {
        $.each(response, function() {
            var menuId = this.MenuId;

            var selector = "[data-menu-id={menuId}]";

            if (this.AllowAccess) {
                selector += "[data-menu-allow]";
            } else if (this.DisallowAccess) {
                selector += "[data-menu-deny]";
            } else {
                selector = "";
            };

            if (selector) {
                selector = selector.replace("{menuId}", menuId);
                $(selector).prop("checked", true).trigger("change");
            };
        });
    });

});