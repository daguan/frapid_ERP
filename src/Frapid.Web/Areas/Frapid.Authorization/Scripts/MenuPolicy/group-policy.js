$(".selector.table tbody tr").click(function() {
    var el = $(this);
    var selector = el.find("input[type=checkbox]");
    selector.prop("checked", !(selector.prop("checked")));
});

$("[data-toggle-all]").click(function() {
    var el = $(this);

    var state = el.prop("checked");
    var table = el.closest("table");

    if (!state) {
        $("[data-menu-id]").removeAttr("checked");
        return;
    };

    table.find("[data-menu-id]").prop("checked", true);
});

$(window).keypress(function(event) {
    if (!(event.which === 115 && event.ctrlKey) && !(event.which === 19)) return true;
    save();
    event.preventDefault();
    return false;
});

function save() {
    function request(model) {
        var url = "/dashboard/authorization/menu-access/group-policy";
        var data = JSON.stringify(model);
        return window.getAjaxRequest(url, "PUT", data);
    };

    var confirmed = window.confirmAction();
    if (!confirmed) {
        return;
    };

    var roleId = parseInt($("#RoleDropdown").val() || 0);
    var officeId = parseInt($("#OfficeDropdown").val() || 0);

    if (!roleId || !officeId) {
        return;
    };

    var menuIds = $("[data-menu-id]:checked").map(function() {
        return $(this).attr("data-menu-id");
    }).get();

    var model = {
        RoleId: roleId,
        OfficeId: officeId,
        MenuIds: menuIds
    };

    var ajax = request(model);

    ajax.success(function() {
        window.displaySuccess();
    });

    ajax.fail(function(xhr) {
        window.logAjaxErrorMessage(xhr);
    });
};

$("[data-save-button]").off("click").on("click", function() {
    save();
});

$("[data-get-menu-policy-button]").off("click").on("click", function() {
    function request(officeId, roleId) {
        var url = "/dashboard/authorization/menu-access/group-policy/{officeId}/{roleId}";
        url = url.replace("{officeId}", officeId);
        url = url.replace("{roleId}", roleId);

        return window.getAjaxRequest(url);
    };

    var roleId = parseInt($("#RoleDropdown").val() || 0);
    var officeId = parseInt($("#OfficeDropdown").val() || 0);

    if (!roleId || !officeId) {
        return;
    };

    var ajax = request(officeId, roleId);

    ajax.success(function(response) {
        $("[data-toggle-all]").removeAttr("checked");
        $("[data-menu-id]").removeAttr("checked");

        $.each(response.MenuIds, function() {
            var selector = "[data-menu-id={menuId}]";
            selector = selector.replace("{menuId}", this);

            $(selector).prop("checked", true);
        });
    });

});