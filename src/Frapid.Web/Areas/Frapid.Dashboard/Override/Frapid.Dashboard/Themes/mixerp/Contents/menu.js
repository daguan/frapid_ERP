function loadApplications() {
    function request() {
        var url = "/dashboard/my/apps";
        return window.getAjaxRequest(url);
    };

    function append(app) {
        var item = $("<a />");
        item.addClass("ui dropdown item");
        item.attr("data-app-name", app.AppName);
        item.text(app.Name);

        if (app.Icon) {
            var icon = $("<i/>");
            icon.addClass(app.Icon);
            icon.addClass("icon");
            item.prepend(icon);
        };

        item.append('<i class="dropdown icon"></i>');
        $("#main-menu").append(item);

        buildMenus(item, app.AppName);
    };

    var ajax = request();

    ajax.success(function (response) {
        if (response) {
            $.each(response, function () {
                append(this);
            });

            $(".dropdown").dropdown();
        };
    });
};

function buildMenus(target, appName) {
    if (window.menuBuilder) {
        var application = window.Enumerable.From(window.appMenus)
            .Where(function (x) { return x.AppName === appName; })
            .FirstOrDefault();

        if (application) {
            target.append('<div class="menu" />');
            var container = target.find(".menu");

            window.menuBuilder.build(application.AppName, container, null);
        };
    };
};



var menuBuilder = {
    build: function (app, container, menuId) {
        var myMenus = window.Enumerable.From(window.appMenus)
            .Where(function (x) { return x.AppName === app; })
            .Where(function (x) { return x.ParentMenuId === menuId; })
            .OrderBy(function (x) { return x.Sort; })
            .ToArray();

        var isSubMenu = menuId != null && myMenus.length > 0;

        if (isSubMenu) {
            if (container.hasClass("item")) {
                container.prepend("<i class='dropdown icon' />");
                container.addClass("ui dropdown");
                container.append("<div class='sub menu' />");
            };
        };

        $.each(myMenus, function () {
            var anchor = $("<a />");
            var span = $("<span/>");
            anchor.addClass("item");
            anchor.attr("data-menu-id", this.MenuId);
            anchor.attr("data-app-name", this.AppName);
            anchor.attr("data-parent-menu-id", this.ParentMenuId);
            anchor.attr("href", this.Url || "javascript:void(0);");

            span.addClass("text");
            span.html(this.MenuName);

            if (this.Icon) {
                var i = $("<i/>");
                i.addClass(this.Icon);
                i.addClass("icon");

                anchor.append(i);
            };

            anchor.append(span);

            if (isSubMenu) {
                container.find(".sub.menu").append(anchor);
            } else {
                container.append(anchor);
            };


            window.menuBuilder.build(app, anchor, this.MenuId);
        });
    }
};

function loadMenus() {
    function request() {
        var url = "/api/core/menus/all";
        return window.getAjaxRequest(url);
    };

    if (window.appMenus) {
        return;
    };

    var ajax = request();

    ajax.success(function (response) {
        window.appMenus = response;

        loadApplications();
    });
};


loadMenus();
