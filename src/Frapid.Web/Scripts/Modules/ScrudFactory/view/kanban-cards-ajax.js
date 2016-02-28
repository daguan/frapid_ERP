function createCards() {
    function request(kanbanIds, resourceIds) {
        var url = "/api/kanban/get-by-resources/";
        url += "?kanbanIds=";
        url += kanbanIds.join("&kanbanIds=");

        url += "&resourceIds=";
        url += resourceIds.join("&resourceIds=");

        return getAjaxRequest(url);
    };

    if (!window.json) {
        return;
    };

    if (!window.kanbans) {
        return;
    };

    var keyField = (window.scrudFactory.card.keyField || getIdField());
    var resourceIds = Enumerable.From(window.json).Select(function (x) { return x[keyField]; }).ToArray();
    var kanbanIds = Enumerable.From(window.kanbans).Select(function (x) { return x.kanban_id; }).ToArray();

    var ajax = request(kanbanIds, resourceIds);

    ajax.success(function (response) {
        $(".kanban.holder").html("");

        $.each(window.json, function (i, v) {
            var key = getCardKey(v);
            var kanbanDetail = (Enumerable.From(response).Where(function(detail) {
                    return detail.resource_id.toString() === key.toString();
            }).ToArray()[0] || new Object());

            createCard(v, key, kanbanDetail);
        });

        makeSortable();
        makeRatable();
        displayFlaggedCards();
    });
};