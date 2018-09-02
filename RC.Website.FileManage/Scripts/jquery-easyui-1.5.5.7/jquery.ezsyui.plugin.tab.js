﻿//--------------------------------------------------------------------------------
// 文件描述：后台Tab页右键菜单脚本 
// 文件作者： 
// 创建日期：2014-08-18 11:17:29
// 修改记录： 
//--------------------------------------------------------------------------------
$(function () {
    var tabsId = 'tt'; //tabs页签Id 
    var tabRightmenuId = 'tab_rightmenu'; //tabs右键菜单Id   
    //绑定tabs的右键菜单 
    $("#" + tabsId).tabs({
        onContextMenu: function (e, title) {
            //这时去掉 tabsId所在的div的这个属性：class="easyui-tabs"，否则会加载2次    
            e.preventDefault();
            $('#' + tabRightmenuId).menu('show', { left: e.pageX, top: e.pageY
            }).data("tabTitle", title);
        }
    });
    //实例化menu的onClick事件
    $("#" + tabRightmenuId).menu({
        onClick: function (item) {
        CloseTab(tabsId, tabRightmenuId, item.name);
    }
    });

});


//tab关闭事件  
//@param tabId  tab组件Id 
//@param tabMenuId tab组件右键菜单Id 
//@param type  tab组件右键菜单div中的name属性值 
function CloseTab(tabId, tabMenuId, type) {
    //tab组件对象 
    var tabs = $('#' + tabId); //tab组件右键菜单对象 
    var tabMenu = $('#' + tabMenuId);
    //获取当前tab的标题 
    var curTabTitle = tabMenu.data('tabTitle');
    tabs.tabs('select', curTabTitle);
    
    //关闭当前tab 
    if (type === 'tab_menu-tabclose') {
        //通过标题关闭tab 
        tabs.tabs("close", curTabTitle);
    }
    //关闭全部tab 
    else {
        var closeTabsTitle;
        if (type === 'tab_menu-tabcloseall') {
            //获取所有关闭的tab对象 
            closeTabsTitle = getAllTabObj(tabs); //循环删除要关闭的tab  
            $.each(closeTabsTitle, function () {
                var title = this;
                tabs.tabs('close', title);
            });
        }
        //关闭其他tab 
        else if (type === 'tab_menu-tabcloseother') {
            //获取所有关闭的tab对象 
            closeTabsTitle = getAllTabObj(tabs); //循环删除要关闭的tab   
            $.each(closeTabsTitle, function () {
                var title = this;
                if (title != curTabTitle) {
                    tabs.tabs('close', title);
                }
                tabs.tabs('select', curTabTitle);
            });
        }
        //关闭当前左侧tab 
        else if (type === 'tab_menu-tabcloseleft') {
            //获取所有关闭的tab对象 
            closeTabsTitle = getLeftToCurrTabObj(tabs, curTabTitle); //循环删除要关闭的tab  
            $.each(closeTabsTitle, function () {
                var title = this;
                tabs.tabs('close', title);
            });
            tabs.tabs('select', curTabTitle);

        }
        //关闭当前右侧tab 
        else if (type === 'tab_menu-tabcloseright') {
            //获取所有关闭的tab对象 
            closeTabsTitle = getRightToCurrTabObj(tabs, curTabTitle); //循环删除要关闭的tab 
            $.each(closeTabsTitle, function () {
                var title = this;
                tabs.tabs('close', title);
            });
            tabs.tabs('select', curTabTitle);
        }
        else {
            //添加到常用标签
            var url;
            if (type === 'tab_menu-tabAddFavorite') {
                url =$($(tabs).tabs('getSelected')).find("iframe").attr("src");
                coustomTabs = encodeURIComponent(curTabTitle + "|" + url);
                $.whir.confirm("确定要 <span style='color:red'>添加</span> 为常用标签吗？", function () {
                    $.whir.ajax('AdminPostHandler.AddAdminTabs', {
                        params: { coustomTabs: coustomTabs },
                        success: function (response) {
                            var item = eval(response)[0];
                            if (item.status.toLocaleLowerCase() == "true") {
                                $.whir.alertSucessMsg("操作成功，刷新页面或下次登录时生效！");
                            } else {
                                $.whir.alertErrorMsg("操作失败，请与管理员联系！");
                            }
                        }
                    });
                });

            }
            else if (type === 'tab_menu-tabDelFavorite') {
                url = $($(tabs).tabs('getSelected')).find("iframe").attr("src");
                coustomTabs = encodeURIComponent(curTabTitle + "|" + url);
                $.whir.confirm("确定要从常用标签 <span style='color:red'>移除</span> 吗？", function () {
                    $.whir.ajax('AdminPostHandler.DelAdminTabs', {
                        params: { coustomTabs: coustomTabs },
                        success: function (response) {
                            var item = eval(response)[0];
                            if (item.status.toLocaleLowerCase() == "true") {
                                $.whir.alertSucessMsg("操作成功，刷新页面或下次登录时生效！");
                            } else {
                                $.whir.alertErrorMsg("操作失败，请与管理员联系！");
                            }
                        }
                    });
                });

            }
        }
    }
}


//获取所有关闭的tab对象  
//@param tabs tab组件  
function getAllTabObj(tabs) {
    //存放所有tab标题  
    var closeTabsTitle = [];
    //所有所有tab对象  
    var allTabs = tabs.tabs('tabs');
    $.each(allTabs, function () {
        var tab = this;
        var opt = tab.panel('options');
        //获取标题   
        var title = opt.title;
        //是否可关闭 ture:会显示一个关闭按钮，点击该按钮将关闭选项卡 
        var closable = opt.closable; if (closable) {
            closeTabsTitle.push(title);
        }
    });
    return closeTabsTitle;
}

//获取左侧第一个到当前的tab  
//@param tabs  tab组件 
//@param 
//curTabTitle 到当前的tab 
function getLeftToCurrTabObj(tabs, curTabTitle) {
    //存放所有tab标题  
    var closeTabsTitle = [];
    //所有所有tab对象  
    var allTabs = tabs.tabs('tabs');
    for (var i = 0; i < allTabs.length; i++) {
        var tab = allTabs[i];
        var opt = tab.panel('options');
        //获取标题 
        var title = opt.title;
        //是否可关闭 ture:会显示一个关闭按钮，点击该按钮将关闭选项卡   
        var closable = opt.closable; if (closable) {
            //alert('title' + title + '  curTabTitle:' + curTabTitle);  
            if (title == curTabTitle) {
                return closeTabsTitle;
            }
            closeTabsTitle.push(title);
        }
    }
    return closeTabsTitle;
}


//获取当前到右侧最后一个的tab  
//@param tabs          tab组件 
//@param curTabTitle 到当前的tab 
function getRightToCurrTabObj(tabs, curTabTitle) {
    //存放所有tab标题  
    var closeTabsTitle = [];
    //所有所有tab对象  
    var allTabs = tabs.tabs('tabs');
    for (var i = (allTabs.length - 1); i >= 0; i--) {
        var tab = allTabs[i];
        var opt = tab.panel('options'); //获取标题  
        var title = opt.title; //是否可关闭 ture:会显示一个关闭按钮，点击该按钮将关闭选项卡  
        var closable = opt.closable;
        if (closable) {
            //alert('title' + title + '  curTabTitle:' + curTabTitle);  
            if (title == curTabTitle) {
                return closeTabsTitle;
            } closeTabsTitle.push(title);
        }
    }
    return closeTabsTitle;
} 