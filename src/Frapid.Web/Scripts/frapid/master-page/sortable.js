////Widget Support

$(document).ready(function () {
    if ($.isFunction($.fn.sortable)) {
        $('#sortable-container').sortable({ placeholder: "ui-state-highlight", helper: 'clone', handle: 'div' });
    };
});
