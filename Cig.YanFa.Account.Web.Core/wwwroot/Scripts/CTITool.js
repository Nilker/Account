
//检测是否是IE6，是-则关闭页面
$(function () {
    if ($.browser.msie && $.browser.version == 6) {
        alert("请升级IE6版本！");
        try {
            window.external.MethodScript('colsewindow');
        } catch (e) {
            window.opener = null; window.open('', '_self'); window.close();
        }
        return false;
    }
});

///需引用jquery.js
var callRecordID = "";
var callType = 1;
var taskTypeID = "";
var isDisconnected = 0;
//延迟时间，以毫秒为单位
var delayTime = 6000;
var telIsAvailable = false; //标记拨打电话是否能通话，true-能；false-不能 add lxw 11.22
//document.oncontextmenu = function (e) {
//    if ($.browser.msie) {
//        self.event.returnValue = false;
//    }
//    else {
//        e.preventDefault();
//    }
//    return false;
//};  //屏蔽右键
document.onkeydown = function () {
    if ($.browser.msie) {
        if (event.keyCode == 116) { event.keyCode = 0; event.cancelBubble = true; return false; }
    }
}
//function document.onkeydown() {
//    if (event.keyCode == 116) { event.keyCode = 0; event.cancelBubble = true; return false; } 
//}     //屏蔽F5按键

var ADTTool = window.ADTTool = (function () {
    //add by qizq 2013-1-9本地录音表主键呼出，呼入号码
    var businessNature = '', relationID = '', cRMCustID = '',


    //    init = function (bn, rID, ccID) {
    //        businessNature = bn || ''; //1:核对信息,2:回访
    //        relationID = rID || ''; //关联ID（任务ID或者回访ID）
    //        cRMCustID = ccID || ''; //CRM系统客户ID
    //    },

    newPage = function (url) {
        try {
            var flag = window.external.MethodScript('/browsercontrol/newpage?url=' + url);
            if (flag == 'failed')
            { return -1; }
        }
        catch (e) {
            return -1;
        }
    },

    callOut = function (TargetDN, ShowANI, clickObj, outnum, QingNiuCallOut) {//外呼
        //add lxw 13.11.28
        if (typeof $("#hidCallPhone").val() != "undefined") {
            $("#hidCallPhone").val(TargetDN);
        }
        //alert(businessNature + '|' + relationID + '|' + cRMCustID + "|" + TargetDN);
        //        if (businessNature == '' ||
        //            (businessNature == '1' && relationID == '') ||
        //            (businessNature == '2' && cRMCustID == ''))
        //        { alert('绑定录音参数无效，不能呼出电话'); return; }
        //010去掉区号
        if (TargetDN) {
            if (typeof (clickObj) == 'string')
                clickObj = $('#' + clickObj);
            if (ADTTool.beforeCallOut) {//外呼之前事件
                ADTTool.beforeCallOut(clickObj.attr('MemberCode'), clickObj.attr('ContactInfoUserID'), clickObj.attr('cTel'), clickObj.attr('cName'), clickObj.attr('cSex'));
                //ADTTool.beforeCallOut();
            }
            TargetDN = fullChar2halfChar(TargetDN);
            ShowANI = ShowANI || '';
            try {
                //window.external.Method4Script('/CallControl/MakeCallEX?TargetDN=TEL:' + TargetDN + '&MakeCallType=2&ShowANI=' + ShowANI);
                //alert(TargetDN);
                var ErrorMes = "";
                ErrorMes = window.external.MethodScript('/CallControl/MakeCall?targetdn=' + TargetDN + '&OutShowTag=' + outnum);
                if (ErrorMes != "") {

                    alert(ErrorMes);
                }
                isTelAvailable = true;
            }
            catch (e) {
                //if (ADTTool.onDisconnected) { ADTTool.onDisconnected(); }
                // alert('通话功能不可用');
                //alert(e.toString());

                if (QingNiuCallOut != undefined && typeof QingNiuCallOut == "function") {
                    QingNiuCallOut(TargetDN);
                }
                else {
                    alert('通话功能不可用');
                }
            }
        }
        else { alert('未初始化参数'); }
    },

    openCallOutPopup = function (clickObj, phoneNumText, ShowANI, outNum, QingNiuCallOut) {//若有多个呼出号码，则弹出层         
        if (window.ADTToolPopupContent) {
            window.ADTToolPopupContent.remove();
            $(document).unbind('mousemove', f);
            window.ADTToolPopupContent = null;
        }

        phoneNumText = $.trim(phoneNumText.replace(/\-/g, ""));
        if (phoneNumText.length == 0) { alert('电话号码为空'); return; }

        phoneNumText = $.trim(phoneNumText.replace(/\$/g, ',')); //把$符号替换为逗号

        if (phoneNumText.indexOf(',') < 0) {

            if (QingNiuCallOut != undefined && typeof QingNiuCallOut == "function") {
                callOut(phoneNumText, ShowANI, $(clickObj), outNum, QingNiuCallOut);
            }
            else {
                callOut(phoneNumText, ShowANI, $(clickObj), outNum);
            }
        }
        else {
            var co = $(clickObj); if (co.length == 0) { return; }

            var content = $('<div class="open_tell" stype="position:absolute;"></div>');
            //var content = $('<div class="open_tell" stype="position:absolute;"></div>');
            var ul = $('<ul class="list"></ul>');
            var array = phoneNumText.split(',');
            var i;
            for (i = 0; i < array.length; i++) {
                var v = array[i]; //alert(v);
                var ulObj = $('<li>').append($('<span>').html(v));
                var aObj = $('<a>').addClass('tell').attr('id', 'calltell' + getRandomStr(20) + v)
                .attr('ctel', v);
                //.html('点击电话')
                if (QingNiuCallOut != undefined && typeof QingNiuCallOut == "function") {
                    aObj.attr('href', 'javascript:ADTTool.callOut("' + v + '", "","' + aObj.attr('id') + '","' + outNum + '",QingNiuCallOut);');
                }
                else {
                    aObj.attr('href', 'javascript:ADTTool.callOut("' + v + '", "","' + aObj.attr('id') + '","' + outNum + '");');
                }
                //.click(function () { ADTTool.callOut(v, "", $(this)); });
                if ($(clickObj).attr('ContactInfoUserID') != undefined)
                    aObj.attr('ContactInfoUserID', $(clickObj).attr('ContactInfoUserID'));
                else if ($(clickObj).attr('MemberCode') != undefined)
                    aObj.attr('MemberCode', $(clickObj).attr('MemberCode'));
                ul.append(ulObj.append(aObj));
                //ul.append('<li><span>' + v + '</span><a class="tell" ContactInfoUserID="' + $(clickObj).attr('ContactInfoUserID') + '" MemberCode="' + $(clickObj).attr('MemberCode') + '" href="javascript:ADTTool.callOut(\'' + v + '\',\'\',$(this));">点击电话</a></li>');
            }
            content.append(ul).append('<em></em>').appendTo($('body'));
            height = 25 * (i + 1) + 29;
            top = (co.offset().top - height) + 100;
            left = co.offset().left - 21;
            content.css({ left: left, top: top });
            window.ADTToolPopupContent = content;
            $(document).bind('mousemove', f);
        }
    },

    top = 0, left = 0, height = 0,
    f = function (e) {
        if (e.pageX < left - 20 || e.pageX > left + 180 || e.pageY < top - 20 || e.pageY > top + height + 20) {
            $(document).unbind('mousemove', f);
            window.ADTToolPopupContent.remove();
        }
    },

    unserialise = function (Data) {
        Data = Data.substr(Data.indexOf('?') + 1);

        Data = Data.split("&");
        var Serialised = new Array();
        $.each(Data, function () {
            var Properties = this.split("=");
            Serialised[Properties[0]] = Properties[1];
        });
        return Serialised;
    };


    adtHandler = function (data) {//处理
        if (data.UserEvent == 'Initiated') {//拨出电话到坐席摘机为坐席振铃时间
            if (ADTTool.onInitiated) { ADTTool.onInitiated(data); }
        }
        else if (data.UserEvent == 'Established') {//呼出接通//alert(data.UserEvent + ' ' + data.SessionID + ' ' + data.DNIS);
            //alert('呼出接通' + businessNature + '|' + relationID + '|' + cRMCustID);
            ADTTool.connectOPRequesting = true;
            if (ADTTool.Established) {
                ADTTool.Established(data);
            }
            //ADTTool.CallStartTime = getDate();
            //ADTTool.CallEndTime = null;
        }
        else if (data.UserEvent == 'Released' ||
                 data.UserEvent == 'CA_CALL_EVENT_OP_DISCONNECT' ||
                 data.UserEvent == 'CA_CALL_EVENT_THIRD_PARTY_DISCONNECT' ||
                 data.UserEvent == 'CA_CALL_EVENT_FOURTH_PARTY_DISCONNECT') {//坐席挂断，客户挂断//alert(data.UserEvent + ' ' + data.SessionID);
            //alert(data.RecordID);
            //alert(data.RecordIDURL);

            if (ADTTool.onDisconnected) {
                //alert('进入onDisconnected事件');
                ADTTool.onDisconnected(data);
            }
            //ADTTool.CallEndTime = getDate();
        }
    }

    var getDate = function () {
        var s, d = new Date();
        s += d.getYear() + "-";
        s += (d.getMonth() + 1) + "-";
        s += d.getDate() + " ";
        s += d.getHours() + ':';
        s += d.getMinutes() + ':';
        s += d.getSeconds();
        return s;
    };

    return {
        //init: init,
        callOut: callOut,
        openCallOutPopup: openCallOutPopup,
        unserialise: unserialise,
        adtHandler: adtHandler,
        f: f,
        newPage: newPage
    }
})();

//响应事件
function MethodScript(message) {
    if (ADTTool) {
        ADTTool.adtHandler(ADTTool.unserialise(message));
    }
}

//响应客户消息
function Response2CC(message) {
    if (message == "CMDTransfer") {
        //保存客户信息
        if ($("#hdAddCustBaseInfo").val() != undefined) {
            if (SubmitData(false)) {
                $.jAlert("保存成功！");
            }
        }
    }
}

function fullChar2halfChar(str) {
    var result = '';
    for (i = 0; i < str.length; i++) {
        code = str.charCodeAt(i);             //获取当前字符的unicode编码
        if (code >= 65281 && code <= 65373)   //unicode编码范围是所有的英文字母以及各种字符
        {
            result += String.fromCharCode(str.charCodeAt(i) - 65248);    //把全角字符的unicode编码转换为对应半角字符的unicode码
        }
        else if (code == 12288)                                      //空格
        {
            result += String.fromCharCode(str.charCodeAt(i) - 12288 + 32); //半角空格
        } else {
            result += str.charAt(i);                                     //原字符返回
        }
    }
    return result;
}

//播放录音（弹出层）
ADTTool.PlayRecord = function (playurl) {
    if (playurl) {
        $.openPopupLayer({
            name: 'PlayRecordLayer',
            url: "/CTI/PlayRecord.aspx",
            parameters: { 'RecordURL': playurl },
            popupMethod: 'Post'
        });
    }
}