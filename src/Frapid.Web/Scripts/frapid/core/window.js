var areYouSure = window.Resources.Questions.AreYouSure() || "Are you sure?";

var confirmAction = function () {
    return confirm(areYouSure);
};

//Jason Bunting & Alex Nazarov
//http://stackoverflow.com/questions/359788/how-to-execute-a-javascript-function-when-i-have-its-name-as-a-string
function executeFunctionByName(functionName, context /*, args */) {
    var args = [].slice.call(arguments).splice(2);
    var namespaces = functionName.split(".");
    var func = namespaces.pop();
    for (var i = 0; i < namespaces.length; i++) {
        context = context[namespaces[i]];
    };

    if (context) {
        if (typeof (context[func]) === "function") {
            return context[func].apply(this, args);
        };
    };

    return undefined;
};