
$(document).ready(function () {
    //加载销售员回款信息
    LoadPage("divSalerBackMoneyInfo", "/Home/HomeCenter/SalerBackMoneyList.aspx");

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