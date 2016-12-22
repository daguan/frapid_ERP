function localize() {
	function humanize(key){
		var items = key.split('.');
		
		if(!items){
			return key;
		};
		
		var text = items[items.length -1];
		
		if(text){
			text = text.replace(/([A-Z])/g, ' $1').trim();
			return text;
		};

		return key;
	};

    /*
    The HTML nodes having the data-localize attribute 
    will be localized with their respective culture values.
    
    For example:
    
    <span data-localize="ScrudResource.company_name"></span>
    
    will be converted to 
    
    Company Name in English or
    Firmenname in German
    */
    var localizable = $("[data-localize]");
	
	$.each(localizable, function(){
		var el = $(this);
		
        var key = "Resources." + el.attr("data-localize");
        var localized = executeFunctionByName(key, window);

        if (!localized) {
			localized = humanize(key);
        };
		
		var tag = el.prop("tagName").toLowerCase();
		
		switch(tag){
			case "input":
				el.val(localized);
				break;
			default:
				el.html(localized);
				break;
		};
	});

    var localizable = $("[data-localized-placeholder]");
	
	$.each(localizable, function(){
		var el = $(this);
		
        var key = "Resources." + el.attr("data-localized-placeholder");
        var localized = executeFunctionByName(key, window);

        if (!localized) {
			localized = humanize(key);
        };
		
		el.attr("placeholder", localized);
	});


    var localizable = $("[data-localized-title]");
	
	$.each(localizable, function(){
		var el = $(this);
		
        var key = "Resources." + el.attr("data-localized-title");
        var localized = executeFunctionByName(key, window);

        if (!localized) {
			localized = humanize(key);
        };
		
		el.attr("title", localized);
	});

    /*
    The HTML nodes having the data-localized-resource attribute 
    will be localized with their respective culture values on the target attribute.
    
    For example:
    
    <input data-localized-resource="ScrudResource.company_name" data-localization-target="value" />
    
    will be converted to 
    
    <input data-localized-resource="ScrudResource.company_name" value="Company Name" />
    in English or

    <input data-localized-resource="ScrudResource.company_name" value="Firmenname " />
    in German
    */
    $("[data-localized-resource]").each(function () {
        var el = $(this);
        var key = "Resources." + el.attr("data-localized-resource");
        var localized = executeFunctionByName(key, window);

        if (localized) {
            var target = el.attr("data-localization-target");
            el.attr(target, localized);
        };
    });
};

$(document).ready(function(){
	setTimeout(function(){
		localize();
	}, 1000);	
});
