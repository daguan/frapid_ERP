function tryParseLocalizedResource(text) {
    function toProperCase(str) {
        var result = str.replace(/_([a-z])/g, function (g) { return g[1].toUpperCase(); });
        return result.charAt(0).toUpperCase() + result.slice(1);
    };

    var key = toProperCase(text);
    var parsed = window.i18n[key];

    if(!parsed){
        parsed = key;
    };

    return parsed;
};

function localizeHeaders(el) {
    el.find("thead tr:first-child th").each(function () {
        var cell = $(this);
        var name = toUnderscoreCase(cell.text());
        var text = tryParseLocalizedResource(name);
        cell.text(text);

        var column = new Object();

        column.columnName = name;
        column.localized = text;

        localizedHeaders.push(column);
    });
};