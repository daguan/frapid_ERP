function loadGrid() {
    function filteredRequest(pageNumber, queryStrings) {
        var url = window.scrudFactory.viewAPI + "/get-where/" + pageNumber;
        var data = JSON.stringify(getAjaxFilters(queryStrings));
        return getAjaxRequest(url, "POST", data);
    };

    function request(pageNumber, filterName, byOffice) {
        if (getFilterQueryStringCount()) {
            var queryStrings = getQueryStrings();
            return filteredRequest(pageNumber, queryStrings, byOffice);
        };

        var url = window.scrudFactory.viewAPI + "/page/" + pageNumber;

        if (filterName) {
            url = window.scrudFactory.viewAPI + "/get-filtered/" + pageNumber + "/" + filterName;
        };

        return getAjaxRequest(url);
    };

    var pageNumber = getPageNumber();
    var filterName = getFilterName();

    if (checkIfProcedure()) {
        $(".view.dimmer").removeClass("active");
        $("#Pager").remove();
        loadAnnotation();
        return;
    };

    $(".current.page.anchor").text(pageNumber);
    var ajax = request(pageNumber, filterName, false);

    ajax.success(function (response) {
        onViewSuccess(response);
    });

    ajax.fail(function (xhr) {
        logAjaxErrorMessage(xhr);
    });
};