//选择客户
function popup_Customer(textId, hideId, obj, fn) {
    $.openPopupLayer({
        name: "SelectAjaxPopup_CustInfo",
        parameters: obj,
        url: "/BaseDataManager/SelectCustInfo.aspx?CustIDs=" + $("#" + hideId).val(),
        success: function () {
            if (fn != undefined && fn != null) {
            }
        },
        beforeClose: function (e) {
            if (!e) {
                if (fn != undefined && fn != null) {
                    fn(e, null);
                }
                return;
            }
            var name = '';
            var id = '';
            var isblack = '';
            var islock = '';
            var dateList = $('#popupLayer_SelectAjaxPopup_CustInfo').data('CustList');
            var dateInfo = $('#popupLayer_SelectAjaxPopup_CustInfo').data('CustInfo');
            if (dateList != undefined) {
                for (var i = 0; i < dateList.length; i++) {
                    name += dateList[i].CustName + ",";
                    id += dateList[i].CustID + ",";
                }
                name = name.substring(0, name.length - 1);
                id = id.substring(0, id.length - 1);

            }
            else if (dateInfo != undefined) {
                id = dateInfo.CustID;
                name = dateInfo.CustName;
                isblack = dateInfo.IsBlack;
                islock = dateInfo.IsLock;
            }
            $("#" + textId).val(name);
            $("#" + hideId).val(id);
            $("#" + textId).data("IsBlack", isblack);
            $("#" + textId).data("IsLock", islock);
            if (fn != undefined && fn != null) {
                fn(e, dateList);
            }
        }
    });
}
//选择品牌
function popup_Brands(textId, hideId, obj, fn) {
    $.openPopupLayer({
        name: "SelectAjaxPopup_Brands",
        parameters: obj,
        url: "/BaseDataManager/SelectBrands.aspx?BrandIDs=" + $("#" + hideId).val(),
        success: function () {

        },
        beforeClose: function (e) {
            if (!e) {
                if (fn != undefined && fn != null) {
                    fn(e, null);
                }
                return;
            }
            var name = '';
            var id = '';
            var dateList = $('#popupLayer_SelectAjaxPopup_Brands').data('BrandsList');
            var dateInfo = $('#popupLayer_SelectAjaxPopup_Brands').data('BrandsInfo');
            if (dateList != undefined) {
                for (var i = 0; i < dateList.length; i++) {
                    name += dateList[i].BrandsName + ",";
                    id += dateList[i].BrandsID + ",";
                }
                name = name.substring(0, name.length - 1);
                id = id.substring(0, id.length - 1);
                if (fn != undefined && fn != null) {
                    fn(e, dateList);
                }
            }
            else if (dateInfo != undefined) {
                id = dateInfo.BrandsID;
                name = dateInfo.BrandsName;
                if (fn != undefined && fn != null) {
                    fn(e, dateInfo);
                }
            }

            $("#" + textId).val(name);
            $("#" + hideId).val(id);
        }
    });
}

//选择管理架构部门
function popup_DepartAch(textId, hideId, obj, fn) {
    $.openPopupLayer({
        name: "SelectAjaxPopup_Depart",
        parameters: obj,
        url: "/BaseDataManager/SelectDepartAchievement.aspx?DepartIDs=" + $("#" + hideId).val(),
        success: function () {
            if (fn != undefined && fn != null) {
            }
        },
        beforeClose: function (e) {
            if (!e) {
                if (fn != undefined && fn != null) {
                    fn(e, null);
                }
                return;
            }
            var name = '';
            var id = '';
            var dateList = $('#popupLayer_SelectAjaxPopup_Depart').data('DepartList');
            var dateInfo = $('#popupLayer_SelectAjaxPopup_Depart').data('DepartInfo');
            if (dateList != undefined) {
                for (var i = 0; i < dateList.length; i++) {
                    name += dateList[i].DepartName + ",";
                    id += dateList[i].DepartID + ",";
                }
                name = name.substring(0, name.length - 1);
                id = id.substring(0, id.length - 1);
                if (fn != undefined && fn != null) {
                    fn(e, dateList);
                }
            }
            else if (dateInfo != undefined) {
                id = dateInfo.DepartID;
                name = dateInfo.DepartName;
                if (fn != undefined && fn != null) {
                    fn(e, dateInfo);
                }
            }

            $("#" + textId).val(name);
            $("#" + hideId).val(id);
        }
    });
}

//选择部门
function popup_Depart(textId, hideId, obj, fn) {
    $.openPopupLayer({
        name: "SelectAjaxPopup_Depart",
        parameters: obj,
        url: "/BaseDataManager/SelectDepart.aspx?DepartIDs=" + $("#" + hideId).val(),
        success: function () {
            if (fn != undefined && fn != null) {
            }
        },
        beforeClose: function (e) {
            if (!e) {
                if (fn != undefined && fn != null) {
                    fn(e, null);
                }
                return;
            }
            var name = '';
            var id = '';
            var dateList = $('#popupLayer_SelectAjaxPopup_Depart').data('DepartList');
            var dateInfo = $('#popupLayer_SelectAjaxPopup_Depart').data('DepartInfo');
            if (dateList != undefined) {
                for (var i = 0; i < dateList.length; i++) {
                    name += dateList[i].DepartName + ",";
                    id += dateList[i].DepartID + ",";
                }
                name = name.substring(0, name.length - 1);
                id = id.substring(0, id.length - 1);
                if (fn != undefined && fn != null) {
                    fn(e, dateList);
                }
            }
            else if (dateInfo != undefined) {
                id = dateInfo.DepartID;
                name = dateInfo.DepartName;
                if (fn != undefined && fn != null) {
                    fn(e, dateInfo);
                }
            }
            $("#" + textId).val(name);
            $("#" + hideId).val(id);
        }
    });
}



//仅选择二级部门
function popup_DepartOnly2(textId, hideId, obj, fn) {
    $.openPopupLayer({
        name: "SelectAjaxPopup_Depart",
        parameters: obj,
        url: "/BaseDataManager/SelectDepart.aspx?Level=2&DepartIDs=" + $("#" + hideId).val(),
        success: function () {
            if (fn != undefined && fn != null) {
            }
        },
        beforeClose: function (e) {
            if (!e) {
                if (fn != undefined && fn != null) {
                    fn(e, null);
                }
                return;
            }
            var name = '';
            var id = '';
            var dateList = $('#popupLayer_SelectAjaxPopup_Depart').data('DepartList');
            var dateInfo = $('#popupLayer_SelectAjaxPopup_Depart').data('DepartInfo');
            if (dateList != undefined) {
                for (var i = 0; i < dateList.length; i++) {
                    name += dateList[i].DepartName + ",";
                    id += dateList[i].DepartID + ",";
                }
                name = name.substring(0, name.length - 1);
                id = id.substring(0, id.length - 1);
                if (fn != undefined && fn != null) {
                    fn(e, dateList);
                }
            }
            else if (dateInfo != undefined) {
                id = dateInfo.DepartID;
                name = dateInfo.DepartName;
                if (fn != undefined && fn != null) {
                    fn(e, dateInfo);
                }
            }

            $("#" + textId).val(name);
            $("#" + hideId).val(id);
        }
    });
}
//选择品牌分类
function popup_CustomBrand(textId, hideId, obj, fn) {
    $.openPopupLayer({
        name: "BrandSelectAjaxPopup",
        parameters: obj,
        url: "/BaseDataManager/SelectCustomBrand.aspx?BrandIDs=" + $("#" + hideId).val(),
        success: function () {
            if (fn != undefined && fn != null) {
            }
        },
        beforeClose: function (e) {
            if (!e) {
                if (fn != undefined && fn != null) {
                    fn(e, null);
                }
                return;
            }
            var name = '';
            var id = '';
            var dateList = $('#popupLayer_BrandSelectAjaxPopup').data('BrandList');
            var dateInfo = $('#popupLayer_BrandSelectAjaxPopup').data('BrandInfo');
            if (dateList != undefined) {
                for (var i = 0; i < dateList.length; i++) {
                    name += dateList[i].BrandName + ",";
                    id += dateList[i].BrandID + ",";
                }
                name = name.substring(0, name.length - 1);
                id = id.substring(0, id.length - 1);
                if (fn != undefined && fn != null) {
                    fn(e, dateList);
                }
            }
            else if (dateInfo != undefined) {
                id = dateInfo.BrandID;
                name = dateInfo.BrandName;
                if (fn != undefined && fn != null) {
                    fn(e, dateInfo);
                }
            }

            $("#" + textId).val(name);
            $("#" + hideId).val(id);
        }
    });
}

//选择供应商
function popup_Supplier(hideId, obj, fn) {
    $.openPopupLayer({
        name: "SelectAjaxPopup_Supplier",
        parameters: obj,
        url: "/BaseDataManager/SelectSupplierByMedia.aspx?MediaIDs=" + $("#" + hideId).val(),
        success: function () {
            if (fn != undefined && fn != null) {
            }
        },
        beforeClose: function (e) {
            if (!e) {
                if (fn != undefined && fn != null) {
                    fn(e, null);
                }
                return;
            }
            var name = '';
            var id = '';
            var dateInfo = $('#popupLayer_SelectAjaxPopup_Supplier').data('SupplierInfo');
            if (dateInfo != undefined) {
                id = dateInfo.SupplierID;
                if (fn != undefined && fn != null) {
                    fn(e, dateInfo);
                }
            }
            $("#_tabMediaCost tr:gt(0)").each(function (i, item) {
                if ($(item).find(".ck").attr("checked") == true) {
                    $(item).find(".SelSupplier").val(id);
                }
            });
            $("#" + hideId).val(id);
        }
    });

}

//选择损益版本
function popup_Version(textId, hideId, paraId, obj, fn) {
    $.openPopupLayer({
        name: "SelectAjaxPopup_Version",
        parameters: obj,
        url: "/BaseDataManager/SelectProfitLossVersion.aspx?TotalTargetCode=" + $("#" + paraId).val(),
        success: function () {
            if (fn != undefined && fn != null) {
            }
        },
        beforeClose: function (e) {
            if (!e) {
                if (fn != undefined && fn != null) {
                    fn(e, null);
                }
                return;
            }
            var name = '';
            var id = '';
            var dateInfo = $('#popupLayer_SelectAjaxPopup_Version').data('VersionInfo');
            if (dateInfo != undefined) {
                id = dateInfo.RecID;
                name = dateInfo.VersionName;
                if (fn != undefined && fn != null) {
                    fn(e, dateInfo);
                }
            }

            $("#" + textId).val(name);
            $("#" + hideId).val(id);
        }
    });
}

//选择CRM元素
var crm = {
    selectCustomer: function (textId, hideId, obj, fn) {
        popup_Customer(textId, hideId, obj, fn);
    },
    selectBrands: function (textId, hideId, obj, fn) {
        popup_Brands(textId, hideId, obj, fn);
    },
    selectDepartAch: function (textId, hideId, obj, fn) {
        popup_DepartAch(textId, hideId, obj, fn);
    },
    selectDepart: function (textId, hideId, obj, fn) {
        popup_Depart(textId, hideId, obj, fn);
    },
    selectDepartOnly2: function (textId, hideId, obj, fn) {
        popup_DepartOnly2(textId, hideId, obj, fn);
    },
    selectCustomBrand: function (textId, hideId, obj, fn) {
        popup_CustomBrand(textId, hideId, obj, fn);
    },
    selectDepartLevel2ContainChild: function(hideId, obj, fn) {
        popup_DepartLevel2ContainChild(hideId, obj, fn);
    },
    selectSupplier: function (hideId, obj, fn) {
        popup_Supplier(hideId, obj, fn);
    },
    selectVersion: function (textId, hideId, paraId, obj, fn) {
        popup_Version(textId, hideId, paraId, obj, fn);
    }
}
