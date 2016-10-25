function loadIcons(apps) {
	function addIcon(app, target) {
		var div = $("<div/>");
		var anchor = $("<a/>");
		var icon = $("<i/>");
		var span = $("<span/>");

		div.attr("class", "item");
		anchor.attr("class", "app");
		anchor.attr("data-app-name", app.AppName);
		anchor.attr("href", app.LandingUrl || "javascript:void(0);");

	    var iContainer = $("<div class='icon'/>");
		icon.attr("class", (app.Icon || "user") + " inverted circular icon");
		iContainer.append(icon);

		span.text(app.Name);

		anchor.append(iContainer);
		anchor.append(span);
		div.append(anchor);

		target.append(div);
	};

	var target = $("#PhoneMenu");

	for (var i = 0; i < apps.length; i++) {
		addIcon(apps[i], target);
	};
};

function loadApps() {
	function request() {
		var url = "/dashboard/my/apps";
		return window.getAjaxRequest(url);
	};

	var ajax = request();

	ajax.success(function(response) {
		loadIcons(response);
	});
};
