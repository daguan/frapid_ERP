function deleteRow(el, isCard) {
    function request(primaryKeyValue) {
        var url = window.scrudFactory.formAPI + "/delete/" + primaryKeyValue;
        return getAjaxRequest(url, "DELETE");
    };

    var confirmed = confirmAction();

    if (!confirmed) {
        return;
    };

    var primaryKeyValue = getPrimaryKeyValue($(el), isCard);


    if (typeof (window.scrudFactory.deleteHandler) === "function") {
        window.scrudFactory.deleteHandler(primaryKeyValue);
        return;
    };

    var ajax = request(primaryKeyValue);

    ajax.success(function () {
        var confirmed = confirm(window.Resources.Questions.TaskCompletedSuccessfullyRefreshView());

        if (confirmed) {
            loadPageCount(loadGrid);
        };
    });
};