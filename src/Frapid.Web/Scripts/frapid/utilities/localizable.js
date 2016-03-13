function localize() {
    /*
    The HTML nodes having the data-localize attribute 
    will be localized with their respective culture values.
    
    For example:
    
    <span data-localize="ScrudResource.company_name"></span>
    
    will be converted to 
    
    Company Name in English or
    Firmenname in German
    */
    var localizable = document.querySelectorAll("[data-localize]");

    for (var i = 0; i < localizable.length; i++) {
        var key = "Resources." + localizable[i].getAttribute("data-localize");
        var localized = executeFunctionByName(key, window);

        if (localized) {
            localizable[i].innerHTML = localized;

            //Replace this node with the localized text.
            localizable[i].parentNode.insertBefore(localizable[i].firstChild, localizable[i]);
            localizable[i].parentNode.removeChild(localizable[i]);
        };
    };

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

localize();