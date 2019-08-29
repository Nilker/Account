
/*
选择客户类别
*/
//客户类别集合
var CustomerTypeData = [{ ID: '20004', Name: '特许经销商' }, { ID: '20005', Name: '综合店' }, { ID: '20011', Name: '经纪公司' }, { ID: '20010', Name: '经纪人' },
 { ID: '20001', Name: '厂商' }, { ID: '20008', Name: '厂商大区' }, { ID: '20002', Name: '集团' }
, { ID: '20007', Name: '汽车服务商' }, { ID: '20009', Name: '展厅' }, { ID: '20012', Name: '交易市场' }, { ID: '20013', Name: '车易达' }, { ID: '20006', Name: '其它'}];
var CustomerModel = { FirstID: '20003', FirstName: '4s', List: CustomerTypeData };
//客户类别弹层html模板
var CustomerTemplateHtml = '<ul><li style="width: 382px;"><a href="#" class="active" id="linkAll" CateID="1"   onclick="SelectCusType(1);">全部</a></li><li style="clear: both;"><a href="#"  CateID="${FirstID}"   onclick="SelectCusType(${FirstID});">${FirstName}</a></li>{{each List}} <li><a  href="#" CateID="${ID}"  onclick="SelectCusType(${ID});">${Name}</a></li> {{/each}}</ul>'
+ '<div class="clearfix"></div><div class="line"></div><div class="btn btnS"><input type="button" class="save w60" value="确定" onclick="ConfirmCusType();"><input type="button" class="cancel w60 gray" value="取消"  onclick="CancelCusType();"><a href="#"  onclick="ClearCusType();">清空已选择项</a></div>';

//所选类别数组
var allSelectedID = "";
var allSelectedName = "";
var allSelectArr = [];

$(document).ready(function () {
    allSelectedID = "";
    allSelectedName = "";
    $("#ddlCustCategory").val(allSelectedName);
    //点击类别弹层之外区域，弹层隐藏
    $(".searchTj ul .kh_pop2").click(function (event) {
        event.stopPropagation();
    });
    $("body").click(function () {
        $(".searchTj ul .kh_pop2").hide();
    });

    $("#ddlCustCategory").click(function (event) {
        $(".searchTj ul .kh_pop2").show();
        //初始化类别数据
        if ($("#ddlCustCategory").val() != "") {
            $(".searchTj ul .kh_pop2 ul li a").each(function () {
                $(this).removeClass("active");
                var curID = $(this).attr("cateid");
                var curItem = $(this);
                allSelectArr.each(function () {
                    if (curID == this) {
                        curItem.addClass("active");
                    };
                });
            });
        }
        else {
            $(".searchTj ul .kh_pop2 ul li a").each(function () {
                $(this).removeClass("active");
            });
            $("#linkAll").addClass("active");
        }

        event.stopPropagation();
    });
    //绑定客户类别弹层数据
    $.template('template', CustomerTemplateHtml);
    $.tmpl('template', CustomerModel).appendTo('.searchTj ul .kh_pop2');

});


function SelectCusType(id) {
    if (id == 1) {
        
        $(".searchTj ul .kh_pop2 ul li a").each(function () {
            $(this).removeClass("active");
        });
        $("#linkAll").addClass("active");
    }
    else {
        $(".searchTj ul .kh_pop2 ul li a[cateid=" + id + "]").toggleClass("active");
        $("#linkAll").removeClass("active");
    }
};

function ConfirmCusType() {
    allSelectArr = $(".searchTj ul .kh_pop2 ul li .active").map(function () {
        if ($(this).attr("cateid") != 1) {
            return $(this).attr("cateid");
        }
    });
    allSelectedID = allSelectArr.get().join(",");

    allSelectedName = $(".searchTj ul .kh_pop2 ul li .active").map(function () {
        if ($(this).html() != "全部") {
            return $(this).html();
        }
    }).get().join(",");

    $("#ddlCustCategory").val(allSelectedName);
    $(".searchTj ul .kh_pop2").hide();
};

function CancelCusType() {
    $(".searchTj ul .kh_pop2").hide();
};

function ClearCusType() {
    $(".searchTj ul .kh_pop2").hide();
    allSelectedID = "";
    allSelectedName = "";
    $("#ddlCustCategory").val(allSelectedName);
};
