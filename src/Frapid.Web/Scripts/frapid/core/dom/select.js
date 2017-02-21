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

function displayFieldBinder(el, url, notNull, filters, callback) {
	function request() {
		function getRequest(){
			return window.getAjaxRequest(url);			
		};
		
		function postRequest(){
			var data = JSON.stringify(filters);
			return window.getAjaxRequest(url, "POST", data);
		};
		
		if(!filters){
			return getRequest();
		};
		
		return postRequest();
	};

	var ajax = request();

	ajax.success(function (response) {
		var options = "";

		if (!notNull) {
			options += "<option>Select</option>";
		};
		
		var totalItems = response.length;
		var selectedValue;
		
		$.each(response, function (i) {
			var option = "<option value='{key}' {selected}>{value}</option>";
			option = option.replace("{key}", this.Key);
			option = option.replace("{value}", this.Value);
			
			if(totalItems === 1){
				option = option.replace("{selected}", "selected='selected'");
				selectedValue = this.Value;
			} else{
				if(notNull && i === 0){
					option = option.replace("{selected}", "selected='selected'");									
					selectedValue = this.Value;
				}else{
					option = option.replace("{selected}", "");													
				};
			};

			options += option;
		});

		el.html(options);

	    //Todo: Remove Semantic UI Dropdown dependency 
		// if(el.parent().is(".dropdown")){
		// 	setTimeout(function(){
		// 		el.dropdown("clear");
		// 		el.dropdown("restore defaults");

		// 		if(selectedValue){
		// 			el.dropdown("set selected", selectedValue);

		// 			setTimeout(function(){
		// 				el.trigger("change").trigger("blur");						
		// 			}, 100);
		// 		};
		// 	}, 100)
		// };

		if(selectedValue){
			el.val(selectedValue).trigger("change").trigger("blur");
		};
		
		if(typeof(callback) === "function"){
			callback();
		};
	});
};
