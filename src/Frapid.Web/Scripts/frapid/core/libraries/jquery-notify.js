function displayMessage(a, b) {
    if ($.notify) {
        $.notify(a, b);
    };
};

function displaySucess() {
    var taskCompletedSuccessfully = "Task completed successfully";
    if ($.notify) {
        $.notify(taskCompletedSuccessfully, "success");
    };
};

var logError = function (a, b) {
    if ($.notify) {
        $.notify(a, b);
    };
};

function logAjaxErrorMessage(xhr) {
    logError(getAjaxErrorMessage(xhr));
};