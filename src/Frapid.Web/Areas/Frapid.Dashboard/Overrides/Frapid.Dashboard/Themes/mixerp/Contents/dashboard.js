if (!window.meta) {
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
        window.metaView.Dns = location.host;
        window.metaView.Catalog = location.host.split(".").join("_").split("-").join("_");
        window.shortDateFormat = meta.ShortDateFormat;
        window.longDateFormat = meta.LongDateFormat;
        window.thousandSeparator = meta.ThousandSeparator;
        window.decimalSeparator = meta.DecimalSeparator;
        window.currencyDecimalPlaces = meta.CurrencyDecimalPlaces;
        window.currencySymbol = meta.CurrencySymbol;
        window.datepickerFormat = window.convertNetDateFormat(meta.ShortDateFormat);
        window.datepickerShowWeekNumber = meta.DatepickerShowWeekNumber;
        window.datepickerWeekStartDay = meta.DatepickerWeekStartDay;
        window.datepickerNumberOfMonths = meta.DatepickerNumberOfMonths;

        $(document).trigger("metaready");
    });
};

if (!frapidApp) {
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
};



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
                //Todo: Remove Semantic UI Dropdown dependency 
                // setTimeout(function () {
                //     el.dropdown("set selected", selectedValue.toString());
                // }, 100);

                el.val(selectedValue.toString());
            };

            if (selectedValues) {
                //Todo: Remove Semantic UI Dropdown dependency 
                // setTimeout(function () {
                //     var values = selectedValues.split(",");
                //     el.dropdown("set selected", values);
                // }, 100);

                var values = selectedValues.split(",");
                el.val(values);
            };


        }, keyField, valueField);
    });
};
