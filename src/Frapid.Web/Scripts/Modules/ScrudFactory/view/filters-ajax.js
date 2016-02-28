
function selectFilter(filterName) {
    function request(filterName) {
        var url = "/api/forms/config/filter/get-where/-1";
        var where = [];

        where.push(getAjaxColumnFilter("WHERE", "object_name", FilterConditions.IsEqualTo, window.scrudFactory.viewTableName));
        where.push(getAjaxColumnFilter("AND", "filter_name", FilterConditions.IsEqualTo, filterName));

        var data = JSON.stringify(where);
        return getAjaxRequest(url, "POST", data);
    };

    $("[data-filter-header]").text(filterName);
    var url = updateQueryString("Filter", filterName);
    window.history.replaceState({ path: url }, '', url);

    var ajax = request(filterName);

    ajax.success(function (response) {
        showFilters(response);
        loadPageCount(loadGrid);
    });

    ajax.fail(function (xhr) {
        logAjaxErrorMessage(xhr);
    });
};

function loadFilterNames() {
    function request() {
        var url = "/api/views/config/filter-name-view/get-where/1";
        var where = [];

        where.push(getAjaxColumnFilter("WHERE", "object_name", FilterConditions.IsEqualTo, window.scrudFactory.viewTableName));
        var data = JSON.stringify(where);

        return getAjaxRequest(url, "POST", data, false);
    };


    var ajax = request();

    ajax.success(function (response) {
        var selected = "";

        var option = "<option value=''>";
        option += window.Resources.Titles.SelectAFilter();
        option += "</option>";

        var items = "";
        var itemTemplate = "<div data-filter-item='{0}' class='item'> \
                    <div class='content'> \
                        <a class='header'>{0}</a> \
                    </div> \
                </div>";

        $.each(response, function () {
            var isDefault = this.is_default;

            if (isDefault) {
                selected = this.filter_name;

                if (!window.viewReady) {
                    if (getFilterQueryStringCount()) {
                        //If there are custom query string filters,
                        //the first priority will be given to 
                        //them during the view load event.
                        selected = "";
                    } else {
                        var filter = window.getQueryStringByName("Filter");

                        if (filter) {
                            selected = filter;
                        };
                    };
                };

            };

            option += stringFormat("<option value='{0}'>{0}</option>", this.filter_name);
            items += stringFormat(itemTemplate, this.filter_name);
        });

        $("[data-filter-list]").html(items);

        if (selected) {
            //loadPageCount(loadGrid); will be called instead from the function selectFilter(selected) 
            selectFilter(selected);
        } else {
            loadPageCount(loadGrid);
        };

        $("#FilterSelect").html(option).dropdown();


        $("[data-filter-item]").click(function () {
            var filterName = $(this).attr("data-filter-item");
            $("[data-filter-item]").removeAttr("data-selected-filter");
            $(this).attr("data-selected-filter", filterName);
            selectFilter(filterName);
        });
    });
};

function loadFilterForEdit() {
    function request(filterName) {
        var url = "/api/forms/config/filter/get-where/-1";
        var where = [];

        where.push(getAjaxColumnFilter("WHERE", "object_name", FilterConditions.IsEqualTo, window.scrudFactory.viewTableName));
        where.push(getAjaxColumnFilter("AND", "filter_name", FilterConditions.IsEqualTo, filterName));

        var data = JSON.stringify(where);
        return getAjaxRequest(url, "POST", data);
    };

    var filterName = $("#FilterSelect").getSelectedText();
    $("#FilterName").html(filterName);
    $("#FilterNameInputText").val(filterName);

    var ajax = request(filterName);

    ajax.success(function (response) {
        createFilters(response);
        $('.filter.modal').modal('hide');
    });

    ajax.fail(function (xhr) {
        logAjaxErrorMessage(xhr);
    });
};


function deleteSavedFilter() {
    function request(filterName) {
        var url = "/api/filter/delete/by-name/" + filterName;
        return getAjaxRequest(url, "DELETE");
    };

    var filterName = $("#FilterSelect").val();
    if (filterName) {
        var ajax = request(filterName);
        ajax.success(function () {
            loadFilterNames();
            displayMessage(window.Resources.Labels.TaskCompletedSuccessfully(), "success");
        });
    };
};
