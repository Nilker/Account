/**
* jmpopups
* Copyright (c) 2009 Otavio Avila (http://otavioavila.com)
* Licensed under GNU Lesser General Public License
* 
* @docs http://jmpopups.googlecode.com/
* @version 0.5.1
* 
*/


(function ($) {
    var openedPopups = [];
    var popupLayerScreenLocker = false;
    var focusableElement = [];
    var setupJqueryMPopups = {
        screenLockerBackground: "#000",
        screenLockerOpacity: "0.5"
    };

    $.setupJMPopups = function (settings) {
        setupJqueryMPopups = jQuery.extend(setupJqueryMPopups, settings);
        return this;
    }

    $.openPopupLayer = function (settings) {     
        if (typeof (settings.name) != "undefined" && !checkIfItExists(settings.name)) {
            settings = jQuery.extend({
                width: "auto",
                height: "auto",
                parameters: {},
                target: "",
                success: function () { },
                error: function () { },
                beforeClose: function () { },
                afterClose: function () { },
                reloadSuccess: null,
                cache: false,
                draggable: true,
                type:"GET"
            }, settings);
            loadPopupLayerContent(settings, true);
            return this;
        }
    }

    $.closePopupLayer = function (name, effectiveAction, cData) {
        var ea = false;
        if (effectiveAction && effectiveAction == true) { ea = true; }
        if (name) {
            for (var i = 0; i < openedPopups.length; i++) {
                if (openedPopups[i].name == name) {
                    var thisPopup = openedPopups[i];

                    openedPopups.splice(i, 1)

                    thisPopup.beforeClose(ea, cData);

                    $("#popupLayer_" + name).fadeOut(function () {
                        var data = $("#popupLayer_" + name).children().not('.jmp-link-at-top,.jmp-link-at-bottom'); //获取弹出层中的DIV元素
                        var targetDivID = $("#popupLayer_" + name).data('targetDIVID'); //获取存储在DIV上的targetDIVID值
                        $("#popupLayer_" + name).remove();
                        if ($.trim(targetDivID) != '')
                            $("#" + targetDivID).html(data); //还原DIV中的内容
                        focusableElement.pop();

                        if (focusableElement.length > 0) {
                            $(focusableElement[focusableElement.length - 1]).focus();
                        }

                        thisPopup.afterClose(ea, cData);
                        hideScreenLocker(name);
                    });



                    break;
                }
            }

        } else {
            if (openedPopups.length > 0) {
                $.closePopupLayer(openedPopups[openedPopups.length - 1].name);
            }
        }

        return this;
    }

    $.reloadPopupLayer = function (name, callback) {
        if (name) {
            for (var i = 0; i < openedPopups.length; i++) {
                if (openedPopups[i].name == name) {
                    if (callback) {
                        openedPopups[i].reloadSuccess = callback;
                    }

                    loadPopupLayerContent(openedPopups[i], false);
                    break;
                }
            }
        } else {
            if (openedPopups.length > 0) {
                $.reloadPopupLayer(openedPopups[openedPopups.length - 1].name);
            }
        }

        return this;
    }

    function setScreenLockerSize() {
        if (popupLayerScreenLocker) {
            $('#popupLayerScreenLocker').height($(document).height() - 4 + "px"); //修改遮盖层高度，1.04倍，Modify=Masj，Date=2009-12-24
            $('#popupLayerScreenLocker').width($(document.body).outerWidth(true) + "px");
        }
    }

    function checkIfItExists(name) {
        if (name) {
            for (var i = 0; i < openedPopups.length; i++) {
                if (openedPopups[i].name == name) {
                    return true;
                }
            }
        }
        return false;
    }

    function showScreenLocker() {
        if ($("#popupLayerScreenLocker").length) {
            if (openedPopups.length == 1) {
                popupLayerScreenLocker = true;
                setScreenLockerSize();
                $('#popupLayerScreenLocker').fadeIn();
            }

            if ('undefined' == typeof (document.body.style.maxHeight)) {
                $("select:not(.hidden-by-jmp)").addClass("hidden-by-jmp hidden-by-" + openedPopups[openedPopups.length - 1].name).css("visibility", "hidden");
            }

            $('#popupLayerScreenLocker').css("z-index", parseInt(openedPopups.length == 1 ? 6999 : $("#popupLayer_" + openedPopups[openedPopups.length - 2].name).css("z-index")) + 1);
        } else {
            $("body").append("<div id='popupLayerScreenLocker'><!-- --></div>");
            $("#popupLayerScreenLocker").css({
                position: "absolute",
                background: setupJqueryMPopups.screenLockerBackground,
                left: "0",
                top: "0",
                opacity: setupJqueryMPopups.screenLockerOpacity,
                display: "none"
            });
            showScreenLocker();

            $("#popupLayerScreenLocker").click(function () {
                //$.closePopupLayer();
            });
        }
    }

    function hideScreenLocker(popupName) {
        if (openedPopups.length == 0) {
            screenlocker = false;
            $('#popupLayerScreenLocker').fadeOut();
        } else {
            $('#popupLayerScreenLocker').css("z-index", parseInt($("#popupLayer_" + openedPopups[openedPopups.length - 1].name).css("z-index")) - 1);
        }

        if ('undefined' == typeof (document.body.style.maxHeight)) {
            $("select.hidden-by-" + popupName).removeClass("hidden-by-jmp hidden-by-" + popupName).css("visibility", "visible");
        }
    }

    function setPopupLayersPosition(popupElement, animate) {
        if (popupElement) {
            if (popupElement.width() < $(window).width()) {
                var leftPosition = (document.documentElement.offsetWidth - popupElement.width()) / 2;
            } else {
                var leftPosition = document.documentElement.scrollLeft + 5;
            }
            var scrollTop = document.documentElement.scrollTop || window.pageYOffset || document.body.scrollTop;//add by jiayz，兼容各种浏览器获取scrolltop值
            if (popupElement.height() < $(window).height()) {
                var topPosition = scrollTop + ($(window).height() - popupElement.height()) / 2;
            } else {
                var topPosition = scrollTop + 5;
            }

            var positions = {
                left: leftPosition + "px",
                top: topPosition + "px"
            };

            if (!animate) {
                popupElement.css(positions);
            } else {
                popupElement.animate(positions, "slow");
            }

            setScreenLockerSize();
        } else {
            for (var i = 0; i < openedPopups.length; i++) {
                setPopupLayersPosition($("#popupLayer_" + openedPopups[i].name), true);
            }
        }
    }

    function showPopupLayerContent(popupObject, newElement, data) {
        var idElement = "popupLayer_" + popupObject.name;

        if (newElement) {
            showScreenLocker();

            $("body").append("<div id='" + idElement + "'><!-- --></div>");

            $("#" + idElement).data('targetDIVID', popupObject.target); //存储原始DIV的ID

            var zIndex = parseInt(openedPopups.length == 1 ? 7000 : $("#popupLayer_" + openedPopups[openedPopups.length - 2].name).css("z-index")) + 2;
        } else {
            var zIndex = $("#" + idElement).css("z-index");
        }

        var popupElement = $("#" + idElement);

        popupElement.css({
            visibility: "hidden",
            width: popupObject.width == "auto" ? "" : popupObject.width + "px",
            height: popupObject.height == "auto" ? "" : popupObject.height + "px",
            position: "absolute",
            "z-index": zIndex
        });

        var linkAtTop = "<a href='#' class='jmp-link-at-top' style='position:absolute; left:-9999px; top:-1px;'>&nbsp;</a><input class='jmp-link-at-top' style='position:absolute; left:-9999px; top:-1px;' />";
        var linkAtBottom = "<a href='#' class='jmp-link-at-bottom' style='position:absolute; left:-9999px; bottom:-1px;'>&nbsp;</a><input class='jmp-link-at-bottom' style='position:absolute; left:-9999px; top:-1px;' />";

        popupElement.html(linkAtTop + data + linkAtBottom);

        setPopupLayersPosition(popupElement);

        popupElement.css("display", "none");
        popupElement.css("visibility", "visible");

        if (newElement) {
            if (popupObject.draggable && jQuery.fn.jqDrag) {//draggable
                popupElement.jqDrag('.openwindow div:first');
            }
            popupElement.fadeIn();
        } else {
            popupElement.show();
        }

        $("#" + idElement + " .jmp-link-at-top, " +
		  "#" + idElement + " .jmp-link-at-bottom").focus(function () {
		      $(focusableElement[focusableElement.length - 1]).focus();
		  });

        var jFocusableElements = $("#" + idElement + " a:visible:not(.jmp-link-at-top, .jmp-link-at-bottom), " +
								   "#" + idElement + " *:input:visible:not(.jmp-link-at-top, .jmp-link-at-bottom)");

        if (jFocusableElements.length == 0) {
            var linkInsidePopup = "<a href='#' class='jmp-link-inside-popup' style='position:absolute; left:-9999px;'>&nbsp;</a>";
            popupElement.find(".jmp-link-at-top").after(linkInsidePopup);
            focusableElement.push($(popupElement).find(".jmp-link-inside-popup")[0]);
        } else {
            jFocusableElements.each(function () {
                if (!$(this).hasClass("jmp-link-at-top") && !$(this).hasClass("jmp-link-at-bottom")) {
                    focusableElement.push(this);
                    return false;
                }
            });
        }

        $(focusableElement[focusableElement.length - 1]).focus();

        popupObject.success();

        if (popupObject.reloadSuccess) {
            popupObject.reloadSuccess();
            popupObject.reloadSuccess = null;
        }
        //$('input').blur();
    }

    function loadPopupLayerContent(popupObject, newElement) {
        if (newElement) {
            openedPopups.push(popupObject);
        }

        if (popupObject.target != "") {
            showPopupLayerContent(popupObject, newElement, $("#" + popupObject.target).html());
            $("#" + popupObject.target).html(''); //清空原始层内容
        } else {
            if (popupObject.type == "GET") {
                $.ajax({
                    url: popupObject.url,
                    data: popupObject.parameters,
                    cache: popupObject.cache,
                    dataType: "html",
                    method: "GET",
                    beforeSend: function () {
                        $('body').append('<div style="width: 100%; height: 40px; top: 50%; left: 0px;position:absolute;visibility:visible;z-index:7002;" class="blue-loading"/>');
                    },
                    success: function (data) {
                        $('.blue-loading').remove();
                        showPopupLayerContent(popupObject, newElement, data);
                    },
                    error: popupObject.error
                });
            }
            else {
                $.post(
                    popupObject.url,
                    popupObject.parameters,
                    function (result) //回调函数     
                    {
                        showPopupLayerContent(popupObject, newElement, result);
                    },
                    "html" //返回类型     
                );
            }
        }
    }

    $(window).resize(function () {
        setScreenLockerSize();
        setPopupLayersPosition();
    });

    $(document).keydown(function (e) {
        if (e.keyCode == 27) {
            $.closePopupLayer();
        }
    });







    /*
    *弹出警告信息窗口
    *需要2个图片，info.gif和title.gif
    *Add=Masj,Date=2010-01-11
    */
    $.jAlert = function (message, afterCloseCallBack) {
        var alertName = 'alertName' + Math.round(Math.random() * 1000000);
        if (!afterCloseCallBack) { afterCloseCallBack = function () { }; }
        var popupObject = {
            width: "auto",
            height: "auto",
            parameters: {},
            target: "",
            success: function () { },
            error: function () { },
            beforeClose: function () { },
            afterClose: afterCloseCallBack,
            reloadSuccess: null,
            cache: false,
            draggable: true,
            name: alertName
        };
        openedPopups[openedPopups.length] = popupObject;

        var divPopup_container = document.createElement("div");
        $(divPopup_container).css('-moz-background-clip', 'border')
               .css('-moz-background-inline-policy', 'continuous')
               .css('-moz-background-origin', 'padding')
               .css('-moz-border-radius-bottomleft', '5px')
               .css('-moz-border-radius-bottomright', '5px')
               .css('-moz-border-radius-topleft', '5px')
               .css('-moz-border-radius-topright', '5px')
               .css('background', '#FFFFFF none repeat scroll 0 0')
        //.css('border', '5px solid #999999')
               .css('color', '#000000')
               .css('font-family', 'Arial,sans-serif')
               .css('font-size', '12px')
               .css('width', '400px')
               .addClass('openwindow');

        var h2 = document.createElement("h2");
        //        $(h2).css('cursor', 'move')
        //             .css('-moz-background-clip', 'border')
        //             .css('-moz-background-inline-policy', 'continuous')
        //             .css('-moz-background-origin', 'padding')
        //             .css('background', '#CCCCCC url(/images/title.gif) repeat-x scroll center top')
        //             .css('border-color', '#FFFFFF #FFFFFF #999999')
        //             .css('border-style', 'solid')
        //             .css('border-width', '1px')
        //             .css('color', '#666666')
        //             .css('font-size', '14px')
        //             .css('font-weight', 'bold')
        //             .css('line-height', '1.75em')
        //             .css('margin', '0')
        //             .css('padding', '0')
        //             .css('text-align', 'center');
        //        $(h2).text('提示对话框');
        // font-size: 14px;
        $(h2).css('cursor', 'move')
             .css('background-image', 'url("/images/popT.jpg")')
             .css('color', '#FFFFFF')
             .css('font-family', '微软雅黑')
             .css('height', '30px')
             .css('line-height', '28px')
             .css('padding-left', '15px')
             .css('font-size', '14px');
        $(h2).text('提示对话框');
        var divPopup_content = document.createElement("div");
        $(divPopup_content).css('background-image', 'url(/images/warnning.png)')
                           .css('-moz-background-clip', 'border')
                           .css('-moz-background-inline-policy', 'continuous')
                           .css('-moz-background-origin', 'padding')
                           .css('background', 'transparent url(/images/warnning.png) no-repeat scroll 16px 16px')
                           .css('margin', '0')
                           .css('padding', '1.75em')

        var divPopup_message = document.createElement("div");
        $(divPopup_message).css('padding-left', '48px')
                           .css('padding-right', '48px')
                           .css('font-size', '12px')
                           .css('color', '#595959')
                           .html(message);
        //.html('<img alt="" src="/images/warnning.png">' + message + '<span>Hello</span>');

        var divPopup_panel = document.createElement("div");
        $(divPopup_panel).css('margin', '1em 0 0 1em')
                         .css('text-align', 'center');

        var btnPopup_ok = '<div class="btn"><input type="button" value=" 确定 " onclick="javascript:$.closePopupLayer(\'' + alertName + '\');"/></div>';

        $(divPopup_panel).append(btnPopup_ok);
        $(divPopup_content).append($(divPopup_message))
                           .append($(divPopup_panel));

        $(divPopup_container).append($(h2))
	           .append($(divPopup_content));

        var divTemp = document.createElement("div");
        $(divTemp).append($(divPopup_container));

        showPopupLayerContent(popupObject, true, $(divTemp).html());
    }


    /*
    *弹出确认信息窗口
    *需要2个图片，info.gif和title.gif
    *Add=Masj,Date=2010-01-11
    */
    $.jConfirm = function (message, jConfirmAfterClose, title) {
        var alertName = 'alertName' + Math.round(Math.random() * 1000000);
        if (!jConfirmAfterClose) { jConfirmAfterClose = function () { }; }
        var popupObject = {
            width: "auto",
            height: "auto",
            parameters: {},
            target: "",
            success: function () { },
            error: function () { },
            beforeClose: function () { },
            afterClose: jConfirmAfterClose,
            reloadSuccess: null,
            cache: false,
            draggable: true,
            name: alertName
        };
        openedPopups[openedPopups.length] = popupObject;

        var divPopup_container = document.createElement("div");
        //        $(divPopup_container).css('-moz-background-clip', 'border')
        //               .css('-moz-background-inline-policy', 'continuous')
        //               .css('-moz-background-origin', 'padding')
        //               .css('-moz-border-radius-bottomleft', '5px')
        //               .css('-moz-border-radius-bottomright', '5px')
        //               .css('-moz-border-radius-topleft', '5px')
        //               .css('-moz-border-radius-topright', '5px')
        //               .css('background', '#FFFFFF none repeat scroll 0 0')
        //               .css('border', '5px solid #999999')
        //               .css('color', '#000000')
        //               .css('font-family', 'Arial,sans-serif')
        //               .css('font-size', '12px')
        //               .css('width', '400px')
        //.addClass('openwindow')
        $(divPopup_container).addClass('popup')
               .addClass('confirm');

        //        var h2 = document.createElement("h2");
        //        $(h2).css('cursor', 'move')
        //             .css('-moz-background-clip', 'border')
        //             .css('-moz-background-inline-policy', 'continuous')
        //             .css('-moz-background-origin', 'padding')
        //             .css('background', '#CCCCCC url(/images/title.gif) repeat-x scroll center top')
        //             .css('border-color', '#FFFFFF #FFFFFF #999999')
        //             .css('border-style', 'solid')
        //             .css('border-width', '1px')
        //             .css('color', '#666666')
        //             .css('font-size', '14px')
        //             .css('font-weight', 'bold')
        //             .css('line-height', '1.75em')
        //             .css('margin', '0')
        //             .css('padding', '0')
        //             .css('text-align', 'center');
        var h2 = document.createElement("div");
        $(h2).addClass('title').addClass('ft14');
        if (typeof (title) != "undefined") {
            title.length > 0 ? $(h2).text(title) : $(h2).text('确认对话框');
        } else {
            $(h2).text('确认对话框');
        }

        /**为img赋值***/
        var img = '';
        if (typeof (type) != "undefined") {
            switch (type) {
                case 1: img = 'what.png'; break;
                case 2: img = 'warnning.png'; break;
                default: img = 'what.png'; break;
            }
        }
        else {
            img = 'what.png';
        }
        //        var divPopup_content = document.createElement("div");
        //        $(divPopup_content).css('background-image', 'url(/images/important.gif)')
        //                           .css('-moz-background-clip', 'border')
        //                           .css('-moz-background-inline-policy', 'continuous')
        //                           .css('-moz-background-origin', 'padding')
        //                           .css('background', 'transparent url(/images/' + img + ') no-repeat scroll 16px 16px')
        //                           .css('margin', '0')
        //                           .css('padding', '1.75em')
        var divPopup_content = document.createElement("div");
        $(divPopup_content).addClass("content");

        var divPopup_image = document.createElement("p");
        $(divPopup_image).append('<img border="0" src="/images/' + img + '" complete="complete"/>');

        var divPopup_message = document.createElement("span");
        divPopup_message.style.fontSize = '12px';
        $(divPopup_message).html(message);

        var divPopup_panel = document.createElement("div");
        $(divPopup_panel).addClass('btn');

        var btnPopup_ok = '<input type="button" class="button" value=" 确定 " onclick="javascript:$.closePopupLayer(\'' + alertName + '\',true);" />';
        var btnPopup_cancel = '<input type="button" class="cancel" value=" 取消 " onclick="javascript:$.closePopupLayer(\'' + alertName + '\',false);" />';

        $(divPopup_image).append($(divPopup_message));

        $(divPopup_panel).append(btnPopup_ok)
                         .append(btnPopup_cancel);

        $(divPopup_content).append($(divPopup_image))
                           .append($(divPopup_panel));

        $(divPopup_container).append($(h2))
	           .append($(divPopup_content));

        var divTemp = document.createElement("div");
        $(divTemp).append($(divPopup_container));

        showPopupLayerContent(popupObject, true, $(divTemp).html());
    }
    /*
    *弹出确认信息窗口
    *需要2个图片，info.gif和title.gif
    *Add=zhangzijie,Date=2014-05-14
    */
    $.jBackMoneyConfirm = function (message, jConfirmAfterClose, title, doName, cancelName) {
        if (!doName) {
            doName = '确认';
        }
        if (!cancelName) {
            cancelName = '取消';
        }
        var alertName = 'alertName' + Math.round(Math.random() * 1000000);
        if (!jConfirmAfterClose) { jConfirmAfterClose = function () { }; }
        var popupObject = {
            width: "auto",
            height: "auto",
            parameters: {},
            target: "",
            success: function () { },
            error: function () { },
            beforeClose: function () { },
            afterClose: jConfirmAfterClose,
            reloadSuccess: null,
            cache: false,
            draggable: true,
            name: alertName
        };
        openedPopups[openedPopups.length] = popupObject;

        var divPopup_container = document.createElement("div");

        $(divPopup_container).addClass('popup')
               .addClass('confirm');

        var h2 = document.createElement("div");
        $(h2).addClass('title').addClass('ft14');
        if (typeof (title) != "undefined") {
            title.length > 0 ? $(h2).text(title) : $(h2).text('确认对话框');
        } else {
            $(h2).text('确认对话框');
        }
        var closelink = '<a href="javascript:void(0);" onclick="javascript:$.closePopupLayer(\'' + alertName + '\',false,0);" class="right"><img src="/images/c_btn.png" border="0" /></a>';
        $(h2).append(closelink);

        /**为img赋值***/
        var img = '';
        if (typeof (type) != "undefined") {
            switch (type) {
                case 1: img = 'what.png'; break;
                case 2: img = 'warnning.png'; break;
                default: img = 'what.png'; break;
            }
        }
        else {
            img = 'what.png';
        }

        var divPopup_content = document.createElement("div");
        $(divPopup_content).addClass("content");

        var divPopup_image = document.createElement("p");
        $(divPopup_image).append('<img border="0" src="/images/' + img + '" complete="complete"/>');

        var divPopup_message = document.createElement("span");
        divPopup_message.style.fontSize = '12px';
        $(divPopup_message).html(message);

        var divPopup_panel = document.createElement("div");
        $(divPopup_panel).addClass('btn');

        var btnPopup_ok = '<input type="button" class="button w80" value=" ' + doName + ' " onclick="javascript:$.closePopupLayer(\'' + alertName + '\',true,1);" />';
        var btnPopup_cancel = '<input type="button" class="cancel w80" value=" ' + cancelName + ' " onclick="javascript:$.closePopupLayer(\'' + alertName + '\',true,2);" />';

        $(divPopup_image).append($(divPopup_message));

        $(divPopup_panel).append(btnPopup_ok)
                         .append(btnPopup_cancel);

        $(divPopup_content).append($(divPopup_image))
                           .append($(divPopup_panel));

        $(divPopup_container).append($(h2))
	           .append($(divPopup_content));

        var divTemp = document.createElement("div");
        $(divTemp).append($(divPopup_container));

        showPopupLayerContent(popupObject, true, $(divTemp).html());
    }
    /*
    *弹出提示用户进行输入信息窗口
    *Add=ZhaoXinxin,Date=2013-01-06
    */
    $.jPrompt = function (message, areaID, jPromptAfterClose, title) {
        var alertName = 'alertName' + Math.round(Math.random() * 1000000);
        if (!jPromptAfterClose) { jPromptAfterClose = function () { }; }
        var popupObject = {
            width: "auto",
            height: "auto",
            parameters: {},
            target: "",
            success: function () { },
            error: function () { },
            beforeClose: jPromptAfterClose,
            afterClose: function () { },
            reloadSuccess: null,
            cache: false,
            draggable: true,
            name: alertName
        };
        openedPopups[openedPopups.length] = popupObject;

        var divPopup_container = document.createElement("div");
        $(divPopup_container).addClass('popup')
               .addClass('confirm');
        var h2 = document.createElement("div");
        $(h2).addClass('title').addClass('ft14');
        if (typeof (title) != "undefined") {
            title.length > 0 ? $(h2).text(title) : $(h2).text('输入对话框');
        } else {
            $(h2).text('输入对话框');
        }

        var divPopup_content = document.createElement("div");
        $(divPopup_content).addClass("content");

        var divPopup_image = document.createElement("p");
        divPopup_image.style.textAlign = 'center';

        var divPopup_text = document.createElement('textarea');
        divPopup_text.id = areaID;
        divPopup_text.style.width = '300px';
        divPopup_text.style.height = '70px';
        $(divPopup_text).text(message);

        var divPopup_panel = document.createElement("div");
        $(divPopup_panel).addClass('btn');

        var btnPopup_ok = '<input type="button" class="button" value=" 确定 " onclick="javascript:$.closePopupLayer(\'' + alertName + '\',true);" />';
        var btnPopup_cancel = '<input type="button" class="cancel" value=" 取消 " onclick="javascript:$.closePopupLayer(\'' + alertName + '\',false);" />';

        $(divPopup_image).append($(divPopup_text));

        $(divPopup_panel).append(btnPopup_ok)
                         .append(btnPopup_cancel);

        $(divPopup_content).append($(divPopup_image))
                           .append($(divPopup_panel));

        $(divPopup_container).append($(h2))
	           .append($(divPopup_content));

        var divTemp = document.createElement("div");
        $(divTemp).append($(divPopup_container));

        showPopupLayerContent(popupObject, true, $(divTemp).html());
    }
})(jQuery);