
$(document).ready(function () {

    //*************************左侧***************************************//
    //    //加载代办事项
    //    LoadPage("divToDoList", "/Home/HomeLeft/ToDoList.aspx?r=" + Math.random());
    //    //加载日程安排
    //    LoadPage("divWorkPlan", "/Home/HomeLeft/WorkPlan.aspx?r=" + Math.random());
    //    //加载工作报告
    //    LoadPage("divWorkReport", "/Home/HomeLeft/WorkReport.aspx?r=" + Math.random(), callBackWorkReportTip);
    //*************************中间***************************************//
    //加载销售员回款信息
    //LoadPage("divSalerBackMoneyInfo", "/Home/HomeCenter/SalerBackMoneyList.aspx?r=" + Math.random());
    //加载销售经理回款信息
    //    LoadPage("divSalerManageBackMoneyInfo", "/Home/HomeCenter/SalerManageBackMoneyList.aspx?r=" + Math.random(),
    //    function () {
    //        $("#divSalerManageBackMoneyInfo").show();
    //        $($("#divSalerManageBackMoneyInfo")[0]).replaceWith($("#divSalerManageBackMoneyInfo").html());
    //        if ($('#divSalerManageBackMoneyList').find('ul[bitShow="expend"]').size() > 0) {
    //            $('#divSalerManageBackMoneyList').find('ul[bitShow="expend"]').hide();
    //            $('#btnExpendBack').show();
    //        }
    //    });


    //*************************右侧***************************************//
    //加载会员合作
    //    LoadPage("divMemberCooperation", "/Home/HomeRight/MemberCooperation.aspx?r=" + Math.random());

    //    setTimeout("LoadData()", 2000);

});

function LoadData() {
    //*************************左侧***************************************//
    //    //加载代办事项
    //    LoadPage("divToDoList", "/Home/HomeLeft/ToDoList.aspx");
    //    //加载日程安排
    //    LoadPage("divWorkPlan", "/Home/HomeLeft/WorkPlan.aspx");
    //    //加载工作报告
    //    LoadPage("divWorkReport", "/Home/HomeLeft/WorkReport.aspx", callBackWorkReportTip);
    //    //加载系统公告
    //    LoadPage("divNoticeList", "/Home/HomeLeft/NoticeList.aspx?r=" + Math.random());
    //    //加载资料下载
    //    LoadPage("divRemindList", "/Home/HomeLeft/RemindList.aspx?r=" + Math.random());

    //*************************中间***************************************//
    //加载销售员回款信息
    //LoadPage("divSalerBackMoneyInfo", "/Home/HomeCenter/SalerBackMoneyList.aspx");
    //加载销售人员应收款信息
    //    LoadPage("divSalerReceivablesAmount", "/Home/HomeCenter/SalerReceivablesAmountList.aspx?r=" + Math.random());
    //    //加载销售经理回款信息
    //    LoadPage("divSalerManageBackMoneyInfo", "/Home/HomeCenter/SalerManageBackMoneyList.aspx");
    //加载销售经理应收款信息
    //    LoadPage("divManageReceivablesAmount", "/Home/HomeCenter/SalerManageReceivablesAmountList.aspx?r=" + Math.random(),
    //    function () {
    //        $("#divManageReceivablesAmount").show();
    //        $($("#divManageReceivablesAmount")[0]).replaceWith($("#divManageReceivablesAmount").html());
    //        if ($('#divSalerManageReceivablesAmountList').find('ul[bitShow="expend"]').size() > 0) {
    //            $('#divSalerManageReceivablesAmountList').find('ul[bitShow="expend"]').hide();
    //            $('#btnExpendRecei').show();
    //        }
    //    });
    //加载待处理订单
    //LoadPage("divWaitingProcessOrder", "/Home/HomeCenter/WaitingProcessOrderList.aspx?r=" + Math.random());
    //加载客户拜访统计
    //    LoadPage("divCustVisitStat", "/Home/HomeCenter/CustReturnVisitStatistics.aspx?r=" + Math.random());
    //加载会员统计
    //    $.post('AjaxServers/HomeCenter/MemberStatByArea.aspx',
    //        { 'random': Math.random() },
    //        function (data) {
    //            $("#span_MemberCount").replaceWith(data);
    //        });

    //加载广告售卖统计
    //    $.post('AjaxServers/HomeCenter/OrderInfoPage.aspx',
    //        { 'random': Math.random() },
    //        function (data) {
    //            $("#span_OrderInfo").replaceWith(data);
    //        });

    //*************************右侧***************************************//
    //加载60天内到期的会员
    //    LoadPage("divSixtyDaysExpireMember", "/Home/HomeRight/SixtyDaysExpireMember.aspx?r=" + Math.random());
    //加载区域经理回款信息
    //    LoadPage("divDepartBackMoneyInfo", "/Home/HomeRight/DepartBackMoneyList.aspx?r=" + Math.random());
    //加载区域经理应收款信息
    //    LoadPage("divDepartReceivablesAmount", "/Home/HomeRight/DepartReceivablesAmountList.aspx?r=" + Math.random());
    //    //加载合同审核
    //    LoadPage("divContractList", "/Home/HomeRight/ContractList.aspx?r=" + Math.random());
    //    //加载合同审核状态
    //    LoadPage("divContractStatistics", "/Home/HomeRight/ContractStatistics.aspx?r=" + Math.random(), function () {
    //        callBackContainer("divContractStatistics");
    //        //应用合同审核状态提示信息
    //        ContractStatisticsTip();
    //    });
}


//*********************加载数据模块****************************//
var arr = [];
var status = 0; //0未处理；1处理中
function LoadPage(container, page, callBack, type, sort) {
    if (!container) return
    if (typeof container == 'string') container = $(['#', container].join(''))
    page = [page, ['_', Math.random()].join('=')].join((page && page.indexOf('?') >= 0) ? '&' : '?')
    arr.push({ container: container, page: page, callBack: callBack, type: type, sort: sort });
}

window.onload = function () {
    if (status == 0) {
        arr.sort(function (a, b) { return a["sort"] > b["sort"] ? 1 : -1; }); //从小到大排序
        handler(arr);
    }
};

function handler(arr) {
    status = 1;
    var container = arr[0]["container"];
    var page = arr[0]["page"];
    var callBack = arr[0]["callBack"];
    var type = arr[0]["type"];
    if (callBack == undefined) {
        if (type == 'load') {
            container.load(page + " >*", null, function (response, status) {
                container.show();
                if (status == "success") {
                    if (container.size() > 0) {
                        $(container[0]).replaceWith(container.html());
                    }
                }
                else {
                    //                    $($("#" + container)[0]).html('');
                    // $($("#" + container)[0]).remove();
                }
                recursion();
            });
        } else {
            //        $.post(page, null, function (data) {
            //           
            //            $("#" + container).replaceWith(data);
            //            recursion();
            //        });
            //用Ajax请求方式解决请求失败问题
            $.ajax({
                type: 'POST',
                url: page,
                data: null,
                success: function (data) {
                    container.replaceWith(data);
                },
                complete: function () {
                    recursion();
                }
            });

        }
    }
    else {
        container.load(page + " >*", null, callBack);
    }
}

//递归调用
function recursion() {
    status = 0;
    arr.shift();
    if (status == 0 && arr.length > 0) {
        handler(arr);
    }
}

//*********************需要参数的回调*****************************//
function callBackContainer(container) {
    $($("#" + container)[0]).replaceWith($("#" + container).html());
    recursion();
}


//********************新建联系人********************************//
//新增客户联系人
function AddNewContactInfoForHome() {

    $.openPopupLayer({
        name: "AddContactInfo",
        width: 550,
        url: "/ContactInfoManager/ContactInfoAdd.aspx?custID=0&random=" + Math.random() + "&type=customer&action=add&Source=3",
        afterClose: function (effectiveAction) {
            if (effectiveAction == true) {
            }
        }
    });

}

//********************新建项目********************************//
//新增合同
function AddNewProjectInfoForHome() {
$.openPopupLayer({
                name: "AddProjectInfoAjaxPopup",
                url: "/CIG/Project/ProjectAdd.aspx?from=homepage",
                beforeClose: function (e) {
                }
            });

//    var url = "/CIG/Project/ProjectApi.ashx?r=" + Math.random();
//    $.post(url, "Action=HasAddProject", function (data) {
//        if (data.success && data.result == 'yes') {
//            $.openPopupLayer({
//                name: "AddContractInfoAjaxPopup",
//                url: "/ContractInfo/ContractAdd.aspx?from=homepage",
//                beforeClose: function (e) {
//                }
//            });
//        }
//        else {
//            $.jAlert("您没有立项权限。请联系管理员进行设置！");
//            return;
//        }
//    }, 'json');
}

//********************新建合同********************************//
//新增合同
function AddNewContractInfoForHome() {
    var url = "/ContractInfo/ContractManage/ContractInfoManager.ashx?r=" + Math.random();
    $.post(url, "Action=HasErpCustCompany", function (data) {
        if (data.success && data.result == 'yes') {
            $.openPopupLayer({
                name: "AddContractInfoAjaxPopup",
                url: "/ContractInfo/ContractAdd.aspx?from=homepage",
                beforeClose: function (e) {
                }
            });
        }
        else {
            $.jAlert("您还未设定签约乙方公司，无法创建合同。请联系管理员进行设置！");
            return;
        }
    }, 'json');
}

//*********************日历翻页********************************//
function ShowWorkPlanMonth(type) {
    if (type == -1) {
        //加载日程安排
        $("#divSubWorkPlan").load("/Home/HomeLeft/WorkPlan.aspx?BeginDate="
                + $("#hidPerBeginDate").val()
                + "&EndDate="
                + $("#hidPerEndDate").val()
                + "&r=" + Math.random() + " #divSubWorkPlan >*");
    } else if (type == 1) {
        //加载日程安排
        $("#divSubWorkPlan").load("/Home/HomeLeft/WorkPlan.aspx?BeginDate="
                 + $("#hidNextBeginDate").val()
                + "&EndDate="
                + $("#hidNextEndDate").val()
                + "&r=" + Math.random() + " #divSubWorkPlan >*");
    }
}


//****************添加工作计划*********************&*******//
function addWorkPlanForHome(custId, day) {
    var popupName = "AddWorkPlan";
    var parameters = {};
    parameters["PopupName"] = popupName;
    if (custId) {
        parameters["CustID"] = custId;
    }
    if (day) {
        parameters["DefaultDay"] = day;
    }
    $.openPopupLayer({
        name: popupName,
        parameters: parameters,
        url: "/WorkPlanManager/WorkPlanAdd.aspx",
        afterClose: function (e) {
            if (e) {
                //加载日程安排
                $("#divSubWorkPlan").load("/Home/HomeLeft/WorkPlan.aspx?r=" + Math.random() + " #divSubWorkPlan >*");
            }
        }
    });
}

//********************工作报告******************************//
function callBackWorkReportTip() {
    callBackContainer("divWorkReport");
    $('#spanAddWorkReport').hover(function () {
        $(this).children("a").addClass('top_on').removeClass('csbg');
        $(this).children("ul").stop(true, true).show();
    }, function () {
        $(this).children("a").removeClass('top_on').addClass('csbg');
        $(this).children("ul").hide();
    });

    $('#spanAddWorkReport').hover(function () {
        $(this).children("a").addClass('top_on').removeClass('csbg2');
        $(this).children("ul").stop(true, true).show();
    }, function () {
        $(this).children("a").removeClass('top_on').addClass('csbg2');
        $(this).children("ul").hide();
    });

    $('#spanAddWorkReport').hover(function () {
        $(this).children("a").addClass('top_on').removeClass('csbg3');
        $(this).children("ul").stop(true, true).show();
    }, function () {
        $(this).children("a").removeClass('top_on').addClass('csbg3');
        $(this).children("ul").hide();
    });
}



//********************合同审核状态Tip************************//
function ContractStatisticsTip() {

    $('.tdtip').hover(function () {
        $(".bt-wrapper").remove();
        $(this).btOn();
    }, function () {
        //$('.tdtip').btOff();
        //$(this).btOff();
    });
    $('.tdtip').bt({
        fill: '#FFFFCC',
        strokeStyle: '#B7B7B7',
        spikeLength: 10,
        spikeGirth: 15,
        padding: 10,
        width: 450,
        positions: ['bottom', 'left'],
        //        trigger: ['mouseover', 'mousemove'],
        //        overlap: -15,
        height: 100,
        centerPointY: 3,
        cssStyles: { lineHeight: 2 },
        postHide: function () {
            if (flagTip2 == 1) {
                $(this).btOn();
            }
            else {
            }
        }
    });
}
var flagTip2 = 0;
function ShowTip2(obj) {
    //    $(obj).parent().parent().show();
    //    flagTip2 = 1;
    $(obj).parent().parent().mouseover(function (e) {
        var tg = (window.event) ? e.srcElement : e.target;
        if (tg.nodeName == 'DIV' || tg.nodeName == "LI") {
            $(this).show();
            flagTip2 = 1;
        }
    });
}
function HideTip2(obj) {
    //    $(obj).parent().parent().hide();
    //    flagTip2 = 0;

    //    $(obj).parent().parent().mouseout(function (e) {
    //        var tg = (window.event) ? e.srcElement : e.target;
    //        if ($("#divRemindList").html().length > 300) {
    //            $("#divRemindList").html("");
    //        }
    //        var tgclass = $(tg).attr("class");
    //        $("#divRemindList").append($(tg).attr("class") + " " + tg.nodeName + "<br />");
    //        if (tgclass != "bt-content" && tgclass != "bt-content-li" && tg.nodeName != "shape") {
    //            $(".bt-content").parent().hide();
    //            flagTip2 = 0;
    //        }

    //    });
}

$(document).ready(function () {
    $(document).mouseover(function (e) {
        var tg = (window.event) ? e.srcElement : e.target;
        //console.debug($(tg).attr("class")+" "+tg.nodeName);

        var tgclass = $(tg).attr("class");
        //$("#divRemindList").append($(tg).attr("class") + " " + tg.nodeName + "<br />");
        if (tgclass != "bt-content" && tgclass != "bt-content-li" && tgclass != "tdtip bt-active" && tgclass != "notdtip" && tg.nodeName != "CANVAS" && tg.nodeName != "TBODY" && tg.nodeName != "TR" && tg.nodeName != "shape" && tg.nodeName != "DIV") {
            $(".bt-content").parent().hide();
            flagTip2 = 0;
        }
        if (tg.nodeName == "DIV" && tgclass != "" && tgclass != "bt-content") {
            $(".bt-content").parent().hide();
            flagTip2 = 0;
        }

        //                if (tg.nodeName == 'DIV' || tg.nodeName == "LI") {
        //                    $(this).hide();
        //                    flagTip2 = 0;
        //                }
    });
});

//********************应收款、回款的展开和收起************************//
function Expend(id, type) {
    $('#' + id).find('ul[bitShow="expend"]').show();
    $('#btnExpend' + type).hide();
    $('#btnPakBack' + type).show();
}

function PakBack(id, type) {
    $('#' + id).find('ul[bitShow="expend"]').hide();
    $('#btnPakBack' + type).hide();
    $('#btnExpend' + type).show();
}