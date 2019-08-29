//选择ERP用户
var BitAtuoERPUser = {
    UserInfo: { ERPUserID: '', FullName: '', EmployeeNumber: '' },
    TextValue: { textId: '', hidId: '' },
    UserList: [],
    BindClick: { SelectId: "", ClickType: "" },
    IsSingle: true,
    OpenPopup: function () {
        $.openPopupLayer({
            name: "UserSelectAjaxPopup",
            parameters: { nameList: $("#" + BitAtuo.TextValue.textId).val(), IdList: $("#" + BitAtuo.TextValue.hidId).val() },
            url: "/BaseDataManager/SelectERPUser.aspx",
            beforeClose: function () {
                if (BitAtuo.IsSingle == false) {
                    var name = '';
                    var id = '';
                    for (var i = 0; i < BitAtuo.UserList.length; i++) {
                        name += BitAtuo.UserList[i].FullName + ";";
                        id += BitAtuo.UserList[i].ERPUserID + ";";
                    }
                    $("#" + BitAtuo.TextValue.textId).val(name);
                    $("#" + BitAtuo.TextValue.hidId).val(id);

                }
                else {
                    $("#" + BitAtuo.TextValue.textId).val(BitAtuo.UserInfo.FullName);
                    $("#" + BitAtuo.TextValue.hidId).val(BitAtuo.UserInfo.ERPUserID);
                }
            }
        });
    }
}

$(document).ready(function () {
    if (BitAtuo.BindClick.SelectId != "") {
        $("#" + BitAtuo.BindClick.SelectId).bind(BitAtuo.BindClick.ClickType, { obj: this }, BitAtuo.OpenPopup);
    }
});