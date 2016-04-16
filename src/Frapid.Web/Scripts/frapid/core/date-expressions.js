function convertDate(d) {
    try {
        var date = new Date(parseInt(d.substr(6)));
        return date;
    } catch (e) {
        return null;
    };
};

function parseLocalizedDate(dateString) {
    if (!dateString) {
        return "";
    };

    var date = Date.parseExact(dateString, window.shortDateFormat);

    if (date) {
        var offset = date.getTimezoneOffset() * 60000;

        return removeTimezone(new Date(date.getTime() - offset).toISOString());
    }

    return dateString;
};

function removeTimezone(dateTime) {
    return dateTime.toString().replace("Z", "");
};

function getTime(dateTime) {
    if (!dateTime) {
        return "";
    };

    function padMinutes(minutes) {
        if (parseInt(minutes || 0) < 10) {
            return "0" + minutes;
        };

        return minutes;
    };

    var value = removeTimezone(dateTime);
    var d = new Date(value);
    value = d.getHours() + ":" + padMinutes(d.getMinutes());

    return value;
};

function dateAdd(dt, expression, number) {
    var d = Date.parseExact(removeTimezone(dt), window.shortDateFormat);
    var ret = new Date();

    if (expression === "d") {
        ret = new Date(d.getFullYear(), d.getMonth(), d.getDate() + parseInt(number));
    };

    if (expression === "m") {
        ret = new Date(d.getFullYear(), d.getMonth() + parseInt(number), d.getDate());
    };

    if (expression === "y") {
        ret = new Date(d.getFullYear() + parseInt(number), d.getMonth(), d.getDate());
    };

    return ret.toString(window.shortDateFormat);
};

function convertNetDateFormat(format)
{
    //Convert the date
    format = format.replace("dddd", "DD");
    format = format.replace("ddd", "D");
 
    //Convert month
    if (format.indexOf("MMMM") !== -1)
    {
        format = format.replace("MMMM", "MM");
    }

    if (format.indexOf("MMM") !== -1)
    {
        format = format.replace("MMM", "M");
    }

    if (format.indexOf("MM") !== -1)
    {
        format = format.replace("MM", "mm");
    }

    format = format.replace("M", "m");
 
    //Convert year
    format = format.indexOf("yyyy") >= 0 ? format.replace("yyyy", "yy") : format.replace("yy", "y");
 
    return format;
}

function loadDatepicker() {
    if (!$.isFunction($.fn.datepicker)) {
        return;
    };

    if (typeof (window.datepickerFormat) === "undefined") { window.datepickerFormat = ""; }
    if (typeof (window.datepickerShowWeekNumber) === "undefined") { window.datepickerShowWeekNumber = false; }
    if (typeof (window.datepickerWeekStartDay) === "undefined") { window.datepickerWeekStartDay = "1"; }
    if (typeof (window.datepickerNumberOfMonths) === "undefined") { window.datepickerNumberOfMonths = ""; }
    if (typeof (window.language) === "undefined") { window.language = ""; }

    var candidates = $("input.date:not([readonly]), input[type=date]:not([readonly])");

    candidates.datepicker(
    {
        dateFormat: datepickerFormat,
        showWeek: datepickerShowWeekNumber,
        firstDay: datepickerWeekStartDay,
        constrainInput: false,
        numberOfMonths: eval(datepickerNumberOfMonths)
    },
    $.datepicker.regional[language]);


    $("input[type=date], .date").blur(function () {
        if (today === "") return;
        var control = $(this);
        var value = control.val().trim().toLowerCase();
        var result;
        var number;

        if (value === "d") { result = dateAdd(today, "d", 0); }; //Today
        if (value === "w" || value === "+w") { result = dateAdd(today, "d", 7); }; //Next Week
        if (value === "m" || value === "+m") { result = dateAdd(today, "m", 1); }; //Next Month
        if (value === "y" || value === "+y") { result = dateAdd(today, "y", 1); }; //Next Year


        if (value === "-d") { result = dateAdd(today, "d", -1); };  //YesterDay      
        if (value === "+d") { result = dateAdd(today, "d", 1); };//Tomorrow
        if (value === "-w") { result = dateAdd(today, "d", -7); }; //Last Week
        if (value === "-m") { result = dateAdd(today, "m", -1); }; //Last Month
        if (value === "-y") { result = dateAdd(today, "y", -1); };

        if (!result) {
            if (value.indexOf("d") >= 0) { number = parseInt(value.replace("d")); result = dateAdd(today, "d", number); };
            if (value.indexOf("w") >= 0) { number = parseInt(value.replace("w")); result = dateAdd(today, "d", number * 7); };
            if (value.indexOf("m") >= 0) { number = parseInt(value.replace("m")); result = dateAdd(today, "m", number); };
            if (value.indexOf("y") >= 0) { number = parseInt(value.replace("y")); result = dateAdd(today, "y", number); };
        };

        if (result) {
            control.val(result).trigger("change");
        };
    });

    $('[data-type="time"], .time').timepicker({ timeFormat: "H:i" });
    $('[data-type="time"], .time').attr("placeholder", "hh:mm");
    candidates.trigger("blur");
};

$(document).ready(function () {
    loadDatepicker();
});