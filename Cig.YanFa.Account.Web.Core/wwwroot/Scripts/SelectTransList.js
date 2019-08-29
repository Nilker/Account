var C_CIG = { code: '', hideCode: '' }
function AjaxSelectTrans(txtId, hideId, method, parm) {

    if (parm == undefined) {
        parm = null;
    }

    $.openPopupLayer({
        name: "CIGTranstionListAjaxPopup",
        url: "/CIG/Project/SelectTranstionList.aspx",
        parameters: parm,
        beforeClose: function (e) {
            if (e) {
                $("#" + txtId).val(C_CIG.code);
                $("#" + hideId).val(C_CIG.hideCode);
                if (method != null && method != undefined) {
                    method(e);
                }
            }
        }
    });
}

function AjaxSelectPurchaseOder(txtId, hideId, status, method,filterstate) {
    var parm = "";
    if (status != null && status != undefined) {
        parm = status;
    }
    $.openPopupLayer({
        name: "CIGPurchaseOrderListAjaxPopup",
        url: "/CIG/Project/SelectPurchaseOrderList.aspx?status=" + parm + "",
        beforeClose: function (e) {
            if (e) {
                $("#" + txtId).val(C_CIG.code);
                $("#" + hideId).val(C_CIG.hideCode);
                if (method != null && method != undefined) {
                    method(e);
                }
            }
        }
    });
}