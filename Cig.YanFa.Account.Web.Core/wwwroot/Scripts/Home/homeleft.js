/// <reference path="jquery-1.4.4.min.js" />
//初始化
$(document).ready(function () {
    //加载代办事项
    //LoadPage("divToDoList", "/Home/HomeLeft/ToDoList.aspx");
    //加载日程安排
    LoadPage("divWorkPlan", "/Home/HomeLeft/WorkPlan.aspx");
    //加载工作报告
    LoadPage("divWorkReport", "/Home/HomeLeft/WorkReport.aspx", callBackWorkReportTip);
    //加载系统公告
    LoadPage("divNoticeList", "/Home/HomeLeft/NoticeList.aspx");
    //加载资料下载
    LoadPage("divRemindList", "/Home/HomeLeft/RemindList.aspx");
});
//加载页面
function LoadPage(container, page, callBack) {

    if (callBack == undefined) {
        $("#" + container).load(page + " >*", null, function () {
            $("#" + container).show();
            $($("#" + container)[0]).replaceWith($("#" + container).html());
        });
    }
    else {
        $("#" + container).load(page + " >*", null, callBack);
    }
}
//*********************需要参数的回调*****************************//
function callBackContainer(container) {
    $($("#" + container)[0]).replaceWith($("#" + container).html());
}

//*********************日历翻页********************************//
function ShowWorkPlanMonth(type) {
    if (type == -1) {
        //加载日程安排
        LoadPage("divWorkPlan", "/Home/HomeLeft/WorkPlan.aspx?BeginDate="
                + $("#hidPerBeginDate").val()
                + "&EndDate="
                + $("#hidPerEndDate").val());
    } else if (type == 1) {
        //加载日程安排
        LoadPage("divWorkPlan", "/Home/HomeLeft/WorkPlan.aspx?BeginDate="
                + $("#hidNextBeginDate").val()
                + "&EndDate="
                + $("#hidNextEndDate").val());
    }
}


//****************添加工作计划*********************&*******//
function addWorkPlanForHome(custID) {
    var popupName = "AddWorkPlan";
    var parameters = {};
    parameters["PopupName"] = popupName;
    if (custID) {
        parameters["CustID"] = custID;
    }
    $.openPopupLayer({
        name: popupName,
        parameters: parameters,
        url: "/WorkPlanManager/WorkPlanAdd.aspx",
        afterClose: function (e) {
            if (e) {
                $.jAlert("添加成功！", function () {
                    //加载日程安排
                    LoadPage("divWorkPlan", "/Home/HomeLeft/WorkPlan.aspx");
                });
            }
        }
    });
}

//********************工作报告******************************//
function callBackWorkReportTip() {
    callBackContainer("divWorkReport");
    $('#spanAddWorkReport').hover(function () {
        $(this).children("a").addClass('top_on').removeClass('csbg');
        $(this).children("ul").stop(true, true).show()
    }, function () {
        $(this).children("a").removeClass('top_on').addClass('csbg');
        $(this).children("ul").hide()
    });

    $('#spanAddWorkReport').hover(function () {
        $(this).children("a").addClass('top_on').removeClass('csbg2');
        $(this).children("ul").stop(true, true).show()
    }, function () {
        $(this).children("a").removeClass('top_on').addClass('csbg2');
        $(this).children("ul").hide()
    });

    $('#spanAddWorkReport').hover(function () {
        $(this).children("a").addClass('top_on').removeClass('csbg3');
        $(this).children("ul").stop(true, true).show()
    }, function () {
        $(this).children("a").removeClass('top_on').addClass('csbg3');
        $(this).children("ul").hide()
    });
}