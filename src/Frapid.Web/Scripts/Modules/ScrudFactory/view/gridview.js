function createGridView(appendTo, json) {
    appendTo.html("");

    if (isNullOrWhiteSpace(json || "")) {
        return null;
    };
    var excludedColumnIndices = [];

    if (!json) {
        return null;
    };

    var el = $('<table class="ui striped nowrap scrudview compact table" id="ScrudView">');
    var header = "<thead><tr>";

    var index = 0;

    if (!window.scrudFactory.excludedColumns) {
        window.scrudFactory.excludedColumns = [];
    };

    $.each(json[0], function (name) {
        if (window.scrudFactory.excludedColumns.indexOf(name) === -1) {
            header += "<th>" + name + "</th>";
        } else {
            excludedColumnIndices.push(index);
        };

        index++;
    });

    header += "</tr><tr class='ui small form'>";

    $.each(json[0], function (name) {
        if (window.scrudFactory.excludedColumns.indexOf(name) === -1) {
            var input = "<input type='text' class='grid filter' data-member='" + name + "' id='filter_" + name + "' />";
            header += "<th>" + input + "</th>";
        };
    });


    header += "</tr></thead>";



    $(header).appendTo(el);

    var body = $("<tbody />");

    $.each(json, function (i, value) {
        var row = "<tr>";

        index = 0;
        $.each(value, function (key, val) {
            if (excludedColumnIndices.indexOf(index) === -1) {
                var title = "";
                if (val === false) {
                    val = window.Resources.Titles.No();
                };

                if (val === true) {
                    val = window.Resources.Titles.Yes();
                };

                var column = Enumerable.From(metaDefinition.Columns)
                    .Where(function (x) { return x.PropertyName === key }).ToArray()[0];

                if (column) {
                    var dataType = column.DbDataType;

                    if (["date", "datetime", "datetimeoffset"].indexOf(dataType) > -1) {
                        if (val == null) {
                            val = "";
                        };

                        title = ' title ="' + val.toFormattedDate() + '"';
                        var d = new Date(removeTimezone(val));
                        val = moment(d).fromNow();
                    };

                    if (dataType === "time") {
                        val = getTime(val);
                    };
                };

                row += stringFormat("<td{0}>{1}</td>", title, (val || ""));
            };

            index++;
        });

        row += "</tr>";

        body.append(row);
    });

    $(el).append(body);
    $(appendTo).append(el);
};
