
//选择ERP数据字典
var BitAutoERP = {
    ERPDict: { RecId: '', Name: '' },
    DictList: [],
    IsSingle: true,
    Method: function (e) { },
    OpenPopup: function (textId, hideId, listType, isAll, Methods, erpstatus) {
        if (Methods != undefined && Methods != null) {
            BitAutoERP.Method = Methods;
        }
        else
        { BitAutoERP.Method = function () { } }
        if (isAll != false) {
            isAll = true
            BitAutoERP.IsSingle = true;
        }
        else {
            isAll = false;
            BitAutoERP.IsSingle = false;
        }
        if (erpstatus != undefined && erpstatus != null) {
            erpstatus = erpstatus;
        }
        else {
            erpstatus = -1;
        }
        $.openPopupLayer({
            name: "DictSelectAjaxPopup",
            parameters: { nameList: $("#" + textId).val(), IdList: $("#" + hideId).val(), type: listType, Status: erpstatus },
            url: "/BaseDataManager/SelectERPDictInfo.aspx",
            success: function () {
                if (isAll != false) {
                    $("#divUserSelect").hide();
                    BitAutoERP.ERPDict.Name = $("#" + textId).val();
                    BitAutoERP.ERPDict.RecId = $("#" + hideId).val();
                }
                else {
                    var arrName = S_nameList.split(';');
                    var arrId = S_idList.split(';');
                    for (var i = 0; i < arrName.length; i++) {
                        if (arrName[i] != "") {
                            $("#tabUserSelect tr").last().after("<tr><td><span  style=\"cursor: pointer\" onclick='S_delUserInfo(this)'>删除</span></td><td>" + arrId[i] + "</td><td><span>" + arrName[i] + "</span></td></tr>");
                        }
                    }
                }
            },
            beforeClose: function (e) {
                if (isAll == false) {
                    var name = '';
                    var id = '';
                    for (var i = 0; i < BitAutoERP.DictList.length; i++) {
                        name += BitAutoERP.DictList[i].Name + ";";
                        id += BitAutoERP.DictList[i].RecId + ";";
                    }
                    $("#" + textId).val(name);
                    $("#" + hideId).val(id);
                }
                else {
                    $("#" + textId).val(BitAutoERP.ERPDict.Name);
                    $("#" + hideId).val(BitAutoERP.ERPDict.RecId);
                }
                BitAutoERP.Method(e);
            }
        });
    }
}
//选择ERP数据字典
var ERP = {
    SelectProject: function (textId, hideId, isAll, Method, status) {
        BitAutoERP.OpenPopup(textId, hideId, 5, isAll, Method, status);
    },
    SelectDepart: function (textId, hideId, isAll, Method, status) {
        BitAutoERP.OpenPopup(textId, hideId, 1, isAll, Method, status);
    },
    SelectArea: function (textId, hideId, isAll, Method, status) {
        BitAutoERP.OpenPopup(textId, hideId, 3, isAll, Method, status);
    },
    SelectBusinessLine: function (textId, hideId, isAll, Method, status) {
        BitAutoERP.OpenPopup(textId, hideId, 2, isAll, Method, status);
    },
    SelectCompany: function (textId, hideId, isAll, Method, status) {
        BitAutoERP.OpenPopup(textId, hideId, 4, isAll, Method, status);
    },
    SelectEndCustomer: function(textId, hideId, isAll, Method, status){
        BitAutoERP.OpenPopup(textId, hideId, 10, isAll, Method, status);
    }
}