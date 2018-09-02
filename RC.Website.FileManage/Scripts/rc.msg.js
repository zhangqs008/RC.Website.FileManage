var rc = window.rc || {};
rc.msg =
{
    confirm: function (message, fnOk, fnCancel) {
        $.messager.confirm("提示信息", message, function (ok) {
            if (ok) {
                if (fnOk) {
                    fnOk();
                }
            } else {
                if (fnCancel) {
                    fnCancel();
                }
            }
        });
    },

    //iframe高度自适应
    frameAutoHeight: function (id) {
        var ifm = document.getElementById(id);
        var subWeb = document.frames ? document.frames["iframepage"].document : ifm.contentDocument;
        if (ifm != null && subWeb != null) {
            ifm.height = subWeb.body.scrollHeight;
            ifm.width = subWeb.body.scrollWidth;
        }
    },
    frameDialog: function (options) {
        var config = {
            id: 'dialog-' + Math.random(),
            title: 'dialog',
            width: 500,
            height: 200,
            url: '',
            closed: false,
            cache: false,
            modal: true,
            onClose: function () {
                $(this).dialog('destroy');
            }
        };
        $.extend(config, options);
        var container = $('<div id="' + config.id + '"><iframe allowtransparency="true" frameborder="0" style="width:100%;height:' + (config.height) + 'px;" scrolling="yes" onLoad="rc.msg.frameAutoHeight("' + config.id + '")"></iframe></div>').appendTo('body');
        container.find('iframe').attr('src', config.url);
        container.dialog(config);
    },
    alert: function (message) {
        top.$.messager.alert("提示信息", message, 'info');
    },
    alertSucessMsg: function (message) {
        top.$.messager.alert("成功提示信息", message, 'info');
    },
    alertErrorMsg: function (message) {
        top.$.messager.alert("错误提示信息", message, 'info');
    },
    openwin: function (url) {
        var a = document.createElement("a");
        a.setAttribute("href", url);
        a.setAttribute("target", "_blank");
        a.setAttribute("id", "openwin");
        document.body.appendChild(a);
        a.click();
    },
    popmsg: function (options) {
        var config = {
            title: "提示信息",
            content: "暂无信息",
            timeout: 3000
        };
        $.extend(config, options);
        $.messager.show({
            title: config.title,
            msg: config.content,
            timeout: config.timeout,
            showType: 'slide'
        });
    }
}