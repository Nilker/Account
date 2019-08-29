var JsonPMainPageBackMoneyInfo = function (data) {
    var list = '';
    list = MakeBackMoneyList(data);
    $('#divList').replaceWith(list);
};
var JsonPSumBackMoneyByQuarter = function (data) {
    var list = '';
    $('#test').append(data.BackAmountSum);
};
var JsonPBackMoneyRank = function (data) {
    var list = '';
    $('#test').append(data.backMoneyRank);
};
var JsonPReceivablesAmountByQuarter = function (data) {
    var list = '';
    $('#test').append(data.amountSum);
};
var JsonPOverdueReceivablesAmount = function (data) {
    var list = '';
    $('#test').append(data.amountSum);
};
var JsonPMainPageReceivables = function (data) {
    var list = '';
    list = MakeReceivalesList(data);
    $('#test').append(list);
};
var JsonPMainPageReceivablesByPeriod = function (data) {
    var list = '';
    list = MakeReceivablesByPeriodList(data);
    $('#test').append(list);
};
var JsonPMainPageReceivablesByQuarter = function (data) {
    var list = '';
    list = MakeReceivablesByQuarterList(data);
    $('#test').append(list);
};
var JsonPMainPageContractAudit = function (data) {
    var list = '';
    list = MakeContractAuditList(data);
    $('#test').append(list);
};
var JsonPMainPageBackMoneyByPeriodEachDepart = function (data) {
    var list = '';

};
var JsonPMainPageReceivablesByQuarterEachDepart = function (data) {
    var list = '';

};
var GetMainPageOverdueReceivablesByQuarterEachDepart = function (data) {
    var list = '';

};

function GetJsonPResult(pody) {
    $.ajax({
        type: "get",
        async: false,
        url: "http://ac.sys1.bitauto.com/AjaxServers/Account2013BaseData/Account2013BaseDataManage.ashx?" + pody,
        dataType: "jsonp",
        jsonp: "callback", //传递给请求处理程序或页面的，用以获得jsonp回调函数名的参数名(一般默认为:callback)
        jsonpCallback: "?" //自定义的jsonp回调函数名称，默认为jQuery自动生成的随机函数名，也可以写"?"，jQuery会自动为你处理数据
    });
}

//销售人员
function MakeBackMoneyList(data) {
    var result = '';
    for (var i = 0; i < data.length; i++) {
        result += '<ul>';
        result += '<li class="cName">' + data[i].CustName + '</li>';
        result += '<li class="hkje">' + data[i].BackAmount + '</li>';
        result += '<li class="hkTime">' + data[i].BackDate + '</li>';
        result += '<li class="hkBh"><a href="#">' + data[i].ContractCode + '</a></li>';
        result += '</ul>';
    }
    return result;
}
function MakeReceivalesList(data) {
    var result = '';
    for (var i = 0; i < data.length; i++) {
        result += '<ul>';
        result += '<li class="cName">' + data[i].CustName + '</li>';
        result += '<li class="hkje">' + data[i].Amount + '</li>';
        result += '<li class="hkTime">' + data[i].AgreedDate + '</li>';
        result += '<li class="hkBh"><a href="#">' + data[i].ContractCode + '</a></li>';
        result += '</ul>';
    }
    return result;
}
function MakeReceivablesByPeriodList(data) {
    var result = '';
    alert(data[0].UserID);
    return result;
}
function MakeReceivablesByQuarterList(data) {
    var result = '';
    alert(data[0].Amount);
    return result;
}
function MakeContractAuditList(data) {
    var result = '';
    for (var i = 0; i < data.length; i++) {
        result += '<ul>';
        result += '<li class="zt">' + data[i].ORDER_STATUS + '</li>';
        result += '<li class="cName">' + data[i].CUSTOMER_NAME + '</li>';
        result += '<li class="tjsj">' + data[i].ORDER_ENTER_DATE + '</li>';
        result += '</ul>';
    }
    return result;
}