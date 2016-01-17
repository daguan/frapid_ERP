$.getJSON("/dashboard/meta", function (response) {
    window.meta = response;
    window.culture = meta.Culture;
    window.language = meta.Language;
    window.jqueryUIi18nPath = meta.JqueryUIi18NPath;
    window.today = meta.Today;
    window.now = meta.Now;
    window.date = today;
    window.userId = meta.UserId;
    window.user = meta.User;
    window.office = meta.Office;
    window.metaView = meta.MetaView;
    window.shortDateFormat = meta.ShortDateFormat;
    window.longDateFormat = meta.LongDateFormat;
    window.thousandSeparator = meta.ThousandSeparator;
    window.decimalSeparator = meta.DecimalSeparator;
    window.currencyDecimalPlaces = meta.CurrencyDecimalPlaces;
    window.currencySymbol = meta.CurrencySymbol;
    window.datepickerFormat = window.convertNetDateFormat(meta.DatepickerFormat);
    window.datepickerShowWeekNumber = meta.DatepickerShowWeekNumber;
    window.datepickerWeekStartDay = meta.DatepickerWeekStartDay;
    window.datepickerNumberOfMonths = meta.DatepickerNumberOfMonths;
});

var lastPage;
var frapidApp = angular.module('FrapidApp', ['ngRoute']);


frapidApp.config(function ($routeProvider, $locationProvider, $httpProvider) {
    $httpProvider.defaults.headers.common["X-Requested-With"] = 'XMLHttpRequest';

    $locationProvider.html5Mode({
        enabled: true,
        requireBase: false
    });


    $routeProvider.
        when('/dashboard', {
            templateUrl: "/dashboard/my/template/Contents/apps.html"
        }).
        when('/dashboard/:url*', {
            templateUrl: function (url) {
                var path = '/dashboard/' + url.url;


                var qs = [];

                for (var q in url) {
                    if (url.hasOwnProperty(q)) {
                        if (q === "url") {
                            continue;;
                        };

                        if (url.hasOwnProperty(q)) {
                            qs.push(q + "=" + url[q]);
                        };
                    }
                };

                if (qs.length) {
                    path = path + "?" + qs.join("&");
                };

                return path;
            }
        });
});

frapidApp.run(function ($rootScope, $location) {
    $rootScope.$on('$locationChangeStart', function (e, n, o) {
        window.overridePath = null;
    });

    $rootScope.$on('$routeChangeStart', function () {
        $("#dashboard-container").addClass("loading");
    });

    $rootScope.$on('$routeChangeSuccess', function () {
        $("#dashboard-container").removeClass("loading");
        buildMenus();
    });

    $rootScope.toogleDashboard = function () {
        if (window.location.pathname !== "/dashboard") {
            lastPage = window.location.pathname;
            $location.path("/dashboard");
        } else {
            if (lastPage) {
                $location.path(lastPage);
            };
        };

    };
});
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
                container.addClass("ui dropdown");
                container.append('<i class="dropdown icon"></i>');
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

function buildMenus() {
    setTimeout(function () {
        var target = $('[data-scope="app-menus"]').html("");
        var path = window.overridePath || window.location.pathname;
        if (window.menuBuilder) {
            var application = window.Enumerable.From(window.appMenus)
                .Where(function (x) { return x.Url === path; })
                .FirstOrDefault();

            if (application) {
                window.menuBuilder.build(application.AppName, target, null);
                $(".dropdown").dropdown();
            };
        };

        target.fadeIn(200);
    }, 500);
};

(function () {
    function loadMenus() {
        function request() {
            var url = "/dashboard/my/menus";
            return window.getAjaxRequest(url);
        };

        var ajax = request();

        ajax.success(function (response) {
            window.appMenus = response;
            buildMenus();
        });
    };

    loadMenus();
})();


function initalizeSelectApis() {
    var candidates = $("select[data-api]");

    candidates.each(function () {
        var el = $(this);
        var apiUrl = el.attr("data-api");
        var valueField = el.attr("data-api-value-field");
        var keyField = el.attr("data-api-key-field");

        window.ajaxDataBind(apiUrl, el, null, null, null, function () {
            var selectedValue = el.attr("data-api-selected-value");
            var selectedValues = el.attr("data-api-selected-values");

            if (selectedValue) {
                setTimeout(function () {
                    el.dropdown("set selected", selectedValue.toString());
                }, 100);
            };

            if (selectedValues) {
                setTimeout(function () {
                    var values = selectedValues.split(",");
                    el.dropdown("set selected", values);
                }, 100);
            };


        }, keyField, valueField);
    });
};

var backgrounds = [];

$.getJSON("/dashboard/backgrounds", function (response) {
    backgrounds = response;

    if (backgrounds.length) {
        $('.background.slider').css("background-color", "black");
    };

    loadBackground();
});

function loadBackground() {
    $.each(backgrounds, function (i, v) {
        setTimeout(function () {
            if (i === backgrounds.length - 1) {
                setTimeout(function () {
                    loadBackground();
                }, 10000);
            };

            setBackground(v);
        }, i * 10000);


    });
};

function setBackground(image) {
    var slider = $('.background.slider:not(.active)');
    var activeSlider = $('.background.slider.active');

    slider.css('background-image', "url('" + image + "')");

    activeSlider.fadeOut(1500, function () {
        activeSlider.css('z-index', -2).show().removeClass('active');
        slider.css('z-index', -1).addClass('active');
    });

};
