var rc = window.rc || {};
rc.ajax = {
    get: function(url, data, callback) {
        $.get(url, data, function(result) {
            callback(result);
        });
    },
    getXML: function(url, data, callback) {
        $.get(url, data, function(result) {
            callback(result);
        });
    },
    htmlEncode: function(text) {
        var value = text;
        try {
            value = value.replace(/&emsp;/g, "&nbsp;");
            value = value.replace(/&/, "&amp;");
            value = value.replace(/</g, "&lt;");
            value = value.replace(/>/g, "&gt;");
            value = value.replace(/'/g, "&apos;");
            value = value.replace(/"/g, "&quot;");
        } catch (e) {
            var span = $("<span>");
            span.html(value);
            value = span.html();
            value = value.replace(/&/, "&amp;");
            value = value.replace(/</g, "&lt;");
            value = value.replace(/>/g, "&gt;");
            value = value.replace(/'/g, "&apos;");
            value = value.replace(/"/g, "&quot;");
        }
        return value;
    },
    post: function(url, params) {
        var defaultParms = {
            url: url,
            data: {},
            dataType: "json",
            type: "POST",
            success: function(response) {
            }
        };
        var settings = $.extend(defaultParms, params);
        $.ajax(settings);
    },
    fixJsonDate: function(jsonDate, format) { // 发现实体转换的日期会多了8个小时，使用该方法会修正这8个小时
        jsonDate = jsonDate.replace(/\//g, "-");
        var year = "", month = "", day = "", hour = "", min = "", second = "";
        var arr = jsonDate.split(" ");
        if (arr.length == 2) {
            var y = arr[0].split("-");
            if (y.length == 3) {
                year = y[0];
                month = y[1];
                day = y[2];
            }
            var t = arr[1].split(":");
            if (t.length == 3) {
                hour = t[0];
                min = t[1];
                second = t[2];
            }
        }
        var date = {
            year: year,
            month: month,
            day: day,
            hour: hour,
            min: min,
            second: second
        };
        switch (format) {
        case "yyyy-MM-dd":
            return date.year + "-" + rc.ajax.fixTime(date.month) + "-" + rc.ajax.fixTime(date.day);
        case "yyyy-MM-dd HH:mm":
            return date.year + "-" + rc.ajax.fixTime(date.month) + "-" + rc.ajax.fixTime(date.day) + " " + rc.ajax.fixTime(date.hour) + ":" + rc.ajax.fixTime(date.min);
        case "zh":
            return date.year + "年" + rc.ajax.fixTime(date.month) + "月" + rc.ajax.fixTime(date.day) + "日";
        default:
            return date.year + "-" + rc.ajax.fixTime(date.month) + "-" + rc.ajax.fixTime(date.day) + " " + rc.ajax.fixTime(date.hour) + ":" + rc.ajax.fixTime(date.min) + ":" + rc.ajax.fixTime(date.second);
        }
        return jsonDate;
    },
    fixTime: function(value) {
        return value.toString().length > 1 ? value : "0" + value;
    },
    subString: function(data, subLenth) {
        if (data != "" && data != null) {
            data = data.replace(/<[^>]+>/g, "");
            if (data.length > subLenth) {
                return data.substring(0, subLenth) + ".....";
            }
        }
        return data;
    },
    SetButtonState: function (btn, isEnable, text) {
        if (isEnable) {
            $(btn).removeAttr("disabled").val(text);
        } else {
            $(btn).attr({ "disabled": "disabled" }).val(text);
        }
    }
};