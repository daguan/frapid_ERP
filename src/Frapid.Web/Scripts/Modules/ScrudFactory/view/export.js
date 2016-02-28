function getFileName() {
    var filterName = getFilterName();

    if (filterName) {
        filterName += "-";
    };

    return window.scrudFactory.title + "-" + filterName + stringFormat(window.Resources.Titles.PageN(), getPageNumber());
};

function createPDF() {
    printGridView(window.reportExportTemplatePath, window.reportHeaderPath, window.scrudFactory.title, "ScrudView", date, window.user, window.office, '', 2, 0, $("#MarkupHidden"), null, downloadPDF);
};

function createXls() {
    printGridView(window.reportExportTemplatePath, window.reportHeaderPath, window.scrudFactory.title, "ScrudView", date, window.user, window.office, '', 2, 0, $("#MarkupHidden"), null, downloadXls);
};

function createDoc() {
    printGridView(window.reportExportTemplatePath, window.reportHeaderPath, window.scrudFactory.title, "ScrudView", date, window.user, window.office, '', 2, 0, $("#MarkupHidden"), null, downloadDoc);
};

function print() {
    printGridView(window.reportExportTemplatePath, window.reportHeaderPath, window.scrudFactory.title, "ScrudView", date, window.user, window.office, '', 2, 0);
};


function startDownload(path) {
    var anchor = $("#DownloadAnchor");
    anchor.attr("href", path);
    anchor[0].click();
};

