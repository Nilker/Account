/// <reference path="jquery-1.3.2-vsdoc2.js"/>
/*
jQuery.initAjaxTabPanel({
tabGroupId: 'TabGroup',
firstLoad: 'tab1',//not exist firstLoad, default load the first one.
saveTabState: true,//if not, fresh tab every time to click tab.
items: [{
tabId: 'tab1',
contentId: 'tab1Content',
url: 'ContactInfoList.aspx',
data: { contentId: '客户联系人' }
}, {
tabId: 'tab2',
contentId: 'tab2Content',
url: 'ContactInfoList.aspx',
data: { contentId: '合作项' }
}]//,
//errorHandler: function(content, responseText, textStatus){content.html('load error');}
});
*/
; (function($) {
    $.freeAjaxTabPanel = {
        init: function(options) {
            options = $.extend({}, $.freeAjaxTabPanel.defaults, options);
            if (!options.items) { return; }
            var tabGroup = $('#' + options.tabGroupId);
            if (tabGroup.size()==0) { return; }
            var firstItem; //the first item to load.
            var opt = {
                saveTabState: options.saveTabState,
                errorHandler: options.errorHandler
            };
            $.each(options.items, function(i, n) {
                var tab = $('#' + n.tabId);
                var content = $('#' + n.contentId);
                var url = n.url;
                var data = n.data;
                if (tab.size()==0 || content.size()==0) { return true; }                
                tab.click(function(e, forceUpdate) {
                    $.freeAjaxTabPanel._toggleTab(tabGroup, tab, content, url, data, opt, forceUpdate);
                });
                if(!firstItem){ firstItem = n; }//not exist firstLoad, default load the first one.
                if (options.firstLoad && options.firstLoad == tab[0].id) {
                    firstItem = n;//exist firstLoad.
                }
            });
            if(firstItem){
                $.freeAjaxTabPanel._toggleTab(tabGroup, $('#' + firstItem.tabId), $('#' + firstItem.contentId), firstItem.url, firstItem.data, opt, true); 
            }
        },
        _toggleTab: function(tabGroup, tab, content, url, data, opt, forceUpdate) {
            var currentTab = tabGroup.find('.current'); //(1)currentTab
            if ((!forceUpdate) && currentTab.size()>0 && currentTab[0].id == tab[0].id) { return; } //click itself
            if(forceUpdate || tab.hasClass('forceUpdate')){//to update whatever it had been updated
                content.removeClass('loaded');
                tab.removeClass('forceUpdate');
            }
            if (!opt.saveTabState || content.hasClass('loaded') == false) {
                if (url) {
                    content.html('<div style="width:100%; height:40px;padding-top:15px;"><div class="blue-loading" style="width:50%;float:left;background-position:right;"></div><div style="float:left;padding:20px 0px 0px 10px;">正在加载中...</div></div>'); 
                    $.freeAjaxTabPanel._tabAction(tabGroup, tab, content);                                       
                    content.load(url, data, function(responseText, textStatus, XMLHttpRequest) {
                        if (textStatus != 'success') {
                            if (opt.errorHandler) { opt.errorHandler(content, responseText, textStatus); }
                        }
                        else {
                            content.addClass('loaded');
                        }
                    });
                }
                else {
                    $.freeAjaxTabPanel._tabAction(tabGroup, tab, content);
                }
            }
            else {
                $.freeAjaxTabPanel._tabAction(tabGroup, tab, content);
            }
        },
        _tabAction: function(tabGroup, tab, content) {
            //get currentTab/currentContent again to prevent the errors on account of timelag
            var currentTab = tabGroup.find('.current'); //(1)currentTab
            var currentContent = tabGroup.data('currentContent'); //(2)currentContent
            if (currentContent) { currentContent.hide(); }
            currentTab.removeClass('current');
            tab.addClass('current');
            tabGroup.data('currentContent', content);
            content.show();
        },
        fresh: function(tabGroupId, tabId){
            var tabGroup = $('#' + tabGroupId);
            if (tabGroup.size()==0) { return; }
            var tab = $('#' + tabId);
            var freshCurrent = true;//fresh current tab
            if(tab.size()>0 && tab.hasClass('current')==false){
                tab.addClass('forceUpdate');//mark it to update when click
                freshCurrent = false;
            }
            if(freshCurrent){
                var currentTab = tabGroup.find('.current');
                if(currentTab.size()>0) { currentTab.trigger("click", [true]); }
            }
        },
        defaults: {
            saveTabState: false,    //if not, fresh tab every time to click tab.
            errorHandler: function(content, responseText, textStatus) { content.html('loaded error'); }
        }
    };
    $.initAjaxTabPanel = $.freeAjaxTabPanel.init;
    $.freshAjaxTabPanel = $.freeAjaxTabPanel.fresh;
})(jQuery);
