function makeSortable() {
    $(function () {
        function deleteRequest(kanbanDetailId) {
            var url = "/api/forms/config/kanban-detail/delete/" + kanbanDetailId;

            return getAjaxRequest(url, "DELETE");
        };

        function request(kanbanDetail) {
            var url = "/api/forms/config/kanban-detail/add-or-edit";

            var form = [];
            form.push(kanbanDetail);
            form.push(null);

            var data = JSON.stringify(form);

            return getAjaxRequest(url, "POST", data);
        };

        $(".kanban.holder").sortable({
            connectWith: ".kanban.holder",
            placeholder: "kanban-placeholder ui-corner-all",
            receive: function (event, ui) {
                var card = $(ui.item[0]);

                var kanbanDetail = new Object();
                kanbanDetail.kanban_detail_id = parseInt(card.attr("data-kanban-detail-id") || null);
                kanbanDetail.kanban_id = parseInt(card.parent().parent().attr("id").replace("kanban", "") || 0);
                kanbanDetail.rating = card.find(".rating .active.icon").length;
                kanbanDetail.resource_id = card.attr("data-key");

                if (kanbanDetail.kanban_id) {
                    var ajax = request(kanbanDetail);

                    ajax.success(function (msg) {
                        card.attr("data-kanban-detail-id", msg);
                    });
                } else {
                    if (kanbanDetail.kanban_detail_id) {
                        var deleteAjax = deleteRequest(kanbanDetail.kanban_detail_id);

                        deleteAjax.success(function () {
                            card.removeAttr("data-kanban-detail-id");
                        });
                    };
                };

            }
        }).disableSelection();
    });
};