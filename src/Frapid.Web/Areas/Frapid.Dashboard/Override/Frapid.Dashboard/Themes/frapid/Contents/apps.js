function loadIcons(apps) {
	function addIcon(app, target) {
		var div = $("<div/>");
		var anchor = $("<a/>");
		var icon = $("<i/>");
		var span = $("<span/>");

		div.attr("class", "two wide computer three wide tablet six wide mobile column");
		anchor.attr("class", "ui link icons");
		anchor.attr("data-app-name", app.AppName);
		anchor.attr("href", app.LandingUrl || "javascript:void(0);");


		icon.attr("class", "ui inverted circular large " + (app.Icon || "user") + " icon");
		span.text(app.Name);

		anchor.append(icon);
		anchor.append(span);
		div.append(anchor);

		target.append(div);
	};

	var target = $("#PhoneMenu .ui.grid");

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
