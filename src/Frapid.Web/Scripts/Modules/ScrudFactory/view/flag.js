function getAllKeys() {
    var ids = [];
    $("#ScrudView").find("td:nth-child(" + getKeyColumnPosition() + ")").each(function () {
        ids.push($(this).text());
    });

    return ids;
};

function displayFlaggedCards() {
    if (!window.flagDefinition) {
        return;
    };

    if (!window.kanbans) {
        return;
    };


    $.each(window.flagDefinition, function (i, v) {
        var card = $('.card[data-key="' + v.ResourceId + '"]');
        card.attr("data-flag-id", v.FlagId);
        card.attr("data-flag", v.FlagTypeName);

        if (card.length) {
            var flag = $('<a class="ui red top right attached label" />');
            flag.text(v.FlagTypeName);

            card.find(".content .header").append(flag);

            card.css("background-color", v.BackgroundColor);
            card.css("color", v.ForegroundColor);
        };
    });
};

function displayFlaggedRows() {
    $("#ScrudView").find("tbody tr").each(function () {
        var row = $(this);
        var resourceId = row.find("td:nth-child(3)").text();

        var data = Enumerable.From(window.flagDefinition)
            .Where(function (x) { return x.ResourceId === resourceId }).ToArray()[0];

        if (data) {
            row.css("background-color", data.BackgroundColor);
            row.css("color", data.ForegroundColor);
            row.attr("data-flag", data.FlagTypeName);
            row.attr("data-flag-id", data.FlagId);
        };
    });
};

function initializeFlag() {
    function getFlagTypes() {
        var url = "/api/forms/config/flag-types/display-fields";
        return getAjaxRequest(url);
    };

    function addFlag(flag) {
        var url = "/api/forms/config/flags/add-or-edit";

        var form = [];

        form.push(null);
        form.push(flag);
        form.push(null);

        var data = JSON.stringify(form);
        return getAjaxRequest(url, "POST", data);
    };

    function deleteFlag(flagId) {
        var url = "/api/forms/config/flags/delete/" + flagId;
        return getAjaxRequest(url, "DELETE");
    };

    if (!flagSelect.find("option").length) {
        var getFlagTypesAjax = getFlagTypes();

        getFlagTypesAjax.success(function (msg) {
            bindSelect(flagSelect, msg, "Key", "Value");
            var options = "<option value='0'>" + window.Resources.Titles.None() + "</option>";
            options += flagSelect.html();
            flagSelect.html(options);
            flagSelect.dropdown();
        });
    };

    function getSelectionCardView() {
        var collection = [];

        var kanban = $('div[data-target="kanban"]');
        var selected = kanban.find("input:checked");

        if (selected.length === 0) {
            return collection;
        };

        selected.each(function () {
            var el = $(this);
            var card = el.closest(".card");

            var selection = new Object();
            selection.FlagId = (card.attr("data-flag-id") || 0);
            selection.ResourceId = (card.attr("data-key") || 0);

            collection.push(selection);
        });

        return collection;
    };

    function getSelection() {
        var collection = [];

        if (isCardView()) {
            return getSelectionCardView();
        };


        var selectedElements = $("input:checkbox:checked").closest("tr");
        if (selectedElements.length === 0) {
            return collection;
        };

        selectedElements.each(function () {
            var el = $(this);
            var selection = new Object();
            selection.FlagId = (el.attr("data-flag-id") || 0);
            selection.ResourceId = el.find("td:nth-child(3)").text();

            collection.push(selection);
        });


        return collection;
    };


    updateButton.unbind("click").click(function () {
        var selection = getSelection();

        if (selection.length === 0) {
            return;
        };

        $.each(selection, function (i, v) {
            var flag = new Object();

            flag.FlagId = v.FlagId;
            flag.ResourceId = v.ResourceId;
            flag.FlagTypeId = parseInt(flagSelect.val());
            flag.Resource = window.scrudFactory.viewTableName;
            flag.ResourceKey = "";

            var flagAjax;

            if (flag.FlagTypeId) {
                flagAjax = addFlag(flag);
            } else {
                flagAjax = deleteFlag(flag.FlagId);
            };


            flagAjax.success(function () {
                if (i + 1 === selection.length) {
                    if (flag.FlagTypeId) {
                        window.displayMessage(window.Resources.Labels.FlagSaved(), "success");
                    } else {
                        window.displayMessage(window.Resources.Labels.FlagRemoved());
                    };

                    loadPageCount(loadGrid);
                };
            });

            flagAjax.fail(function (xhr) {
                logAjaxErrorMessage(xhr);
            });
        });
    });

    function getFlagView(resource, userId, flagIds) {
        var url = "/api/flags/get/";
        url += resource + "/";
        url += userId;
        url += "?resourceIds=";
        url += flagIds.join("&resourceIds=");

        return getAjaxRequest(url);
    };

    function displayFlags() {
        var ids = getAllKeys();

        if(!ids.length){
            return;
        };

        var getFlagViewAjax = getFlagView(window.scrudFactory.viewTableName, window.userId, ids);

        getFlagViewAjax.success(function (msg) {
            window.flagDefinition = msg;

            displayFlaggedRows();
            displayFlaggedCards();
        });
    };

    setTimeout(function() {
        displayFlags();
    }, 1000);
};

