

$(document).ready(function () {
    //加载会员合作
    LoadPage("divMemberCooperation", "/Home/HomeRight/MemberCooperation.aspx");
    //加载60天内到期的会员
    LoadPage("divSixtyDaysExpireMember", "/Home/HomeRight/SixtyDaysExpireMember.aspx");
    //加载区域经理回款信息
    LoadPage("divDepartBackMoneyInfo", "/Home/HomeRight/DepartBackMoneyList.aspx");
    //加载区域经理应收款信息
    LoadPage("divDepartReceivablesAmount", "/Home/HomeRight/DepartReceivablesAmountList.aspx");
    //加载合同审核
    LoadPage("divContractList", "/Home/HomeRight/ContractList.aspx");
    //加载合同审核状态
    LoadPage("divContractStatistics", "/Home/HomeRight/ContractStatistics.aspx");
    //应用合同审核状态提示信息
    ContractStatisticsTip();
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
//********************合同审核状态Tip************************//
function ContractStatisticsTip() {
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
        cssStyles: { lineHeight: 2 }
    });
}