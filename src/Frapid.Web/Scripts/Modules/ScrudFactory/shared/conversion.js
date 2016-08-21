var stringTypes = ["varchar", "text", "nvarchar"];
var wholeNumbers = ["_bigint", "_int2", "_int4", "int", "_int8", "_integer", "_integer_strict", "_integer_strict2", "_serial4", "_serial8", "_smallint", "bigint", "bigint[]", "int2", "int2[]", "int4", "int4[]", "int8", "int8[]", "integer", "integer[]", "integer_strict", "integer_strict2", "integer_strict2[]", "integer_strict[]", "serial4", "serial4[]", "serial8", "serial8[]", "smallint", "smallint[]"];
var decimalNumber = ["_decimal_strict", "_decimal_strict2", "_float4", "_float8", "_money", "_money_strict", "_money_strict2", "_numeric", "decimal_strict", "decimal_strict2", "decimal_strict2[]", "decimal_strict[]", "float4", "float4[]", "float8", "float8[]", "money", "money[]", "money_strict", "money_strict2", "money_strict2[]", "money_strict[]", "numeric", "numeric[]", "decimal"];
var booleans = ["bit", "bool", "System.Boolean"];
var dateTypes = ["System.DateTime", "datetimeoffset", "datetime", "timestamp", "timestamptz", "date"];

function toUnderscoreCase(str) {
    return str.replace(/(?:^|\.?)([A-Z])/g, function (x, y) { return "_" + y.toLowerCase() }).replace(/^_/, "");
};

function toProperCase(str) {
    var result = str.replace(/_([a-z])/g, function (g) { return g[1].toUpperCase(); });
    return result.charAt(0).toUpperCase() + result.slice(1);
};
