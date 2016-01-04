var taskCompletedSuccessfully = window.Resources.Labels.TaskCompletedSuccessfully() || "Task completed successfully.";

function displayMessage(a, b) {
    if ($.notify) {
        $.notify(a, b);
    };
};

function displaySuccess() {
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