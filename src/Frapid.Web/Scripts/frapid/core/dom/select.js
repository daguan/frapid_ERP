jQuery.fn.getSelectedItem = function () {
    var listItem = $(this[0]);
    return listItem.find("option:selected");
};

jQuery.fn.getSelectedValue = function () {
    return $(this[0]).getSelectedItem().val();
};

jQuery.fn.getSelectedText = function () {
    return $(this[0]).getSelectedItem().text();
};

jQuery.fn.setSelectedText = function (text) {
    var target = $(this).find("option").filter(function () {
        return this.text === text;
    });

    target.prop('selected', true);
};

function displayFieldBinder(el, url, notNull) {
	function request() {
		return window.getAjaxRequest(url);
	};

	var ajax = request();

	ajax.success(function (response) {
		var options = "";

		if (!notNull) {
			options += "<option>Select</option>";
		};

		$.each(response, function () {
			var option = "<option value='{key}'>{value}</option>";
			option = option.replace("{key}", this.Key);
			option = option.replace("{value}", this.Value);

			options += option;
		});

		el.html(options);
	});
};
