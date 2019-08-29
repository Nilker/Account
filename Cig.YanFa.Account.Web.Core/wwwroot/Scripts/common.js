function testasdf() {
    alert("ok");
}
$.PageJS = $.PageJS || {};
 


/*
*获取checkbox的值
*name:name属性值
*defaultValue:默认返回值
*/
function GetCheckboxValue(name, defaultValue) {
    var values = $.trim($("input[type='checkbox'][name='" + name + "']").map(function () {
        if ($(this).attr("checked")) {
            return $(this).val();
        }
    }).get().join(','));

    return values.toString() == '' ? defaultValue : values;
}

//绑定媒介单据各审批状态记录数
var recodeCountHtml = '<div class="cxTab mt5"><ul id="selectli" class="">{{each List}}<li style="width: 100px;" class="w180"><a  onclick="${ActionName}(${ApproveStatus},this)"class="${addClass}" ' +
'href="javascript:void(0)">${ApproveStatusName}(${RecordCount})</a></li>{{/each}}</ul><div class="clearfix"></div></div>';

function BindStatusRecord(data, div, action, approveStatus) {
    for (var i = 0; i < data.length; i++) {
        data[i].ActionName = action;
        if (data[i].ApproveStatus == approveStatus) {
            data[i].addClass = "cur";
        }
        else {
            data[i].addClass = "";
        }
    }
    var model = { List: data };
    $.template('template', recodeCountHtml);
    $("#" + div).html($.tmpl('template', model));
}

//捕捉控件回车事件
//path:绑定元素
//fun:回调函数名
function BindEnterEvent(path, fun) {
    $(path).live("keydown", function (e) {
        var curKey = e.which;
        if (curKey == 13) {
            fun();
        }
    });
}
/*
*判断输入日期是否比今天大
*dt:输入日期
*/
function GreaterThanToday(dt) {
    var dtArg = new Date(dt.replace(/\-/g, "\/"));
    var dateNow = new Date(getDate(new Date()).replace(/\-/g, "\/"));
    return dtArg > dateNow;
}

/*
*判断输入日期是否比今天小
*dt:输入日期
*/
function LessThanToday(dt) {
    var dtArg = new Date(dt.replace(/\-/g, "\/"));
    var dateNow = new Date(getDate(new Date()).replace(/\-/g, "\/"));
    return dtArg < dateNow;
}

//提示无权查看页面
function ShowInlegalInfo() {
    $.blockUI({
        message: '<img alt="无单据查看权限" src="/images/correct.png"><span style="margin-left:20px">无单据查看权限</span>',
        timeout: 1000,
        showOverlay: false
    });
    setTimeout(function () { closePageExecOpenerSearch() }, 1000);
}

/*
*删除确认提示
*ids：checkbox的name属性
*/
function validateDel(ids) {
    var b = false;
    var ar = document.getElementsByName(ids);
    for (var ii = 0; ii < ar.length; ii++) {
        if (ar[ii].type == "checkbox") {
            if (ar[ii].checked) {
                b = true;
            }
        }
    }
    if (b) {
        return confirm("确定要删除吗?");

    }
    else {
        alert("请至少选择一项!");
        return false;
    }
}


/*
*全选反选取消
*objName:name属性
*showType：1-全选，2-反选，3-取消
*/
function selectCheckBoxDelAll(objName, showType) {
    var delAllObj = document.getElementsByName(objName);
    if (showType == 1) {
        //全选
        for (var i = 0; i < delAllObj.length; i++) {
            delAllObj[i].checked = true;
        }
    }
    else if (showType == 2) {
        //反选
        for (var i = 0; i < delAllObj.length; i++) {
            if (delAllObj[i].checked) {
                delAllObj[i].checked = false;
            }
            else {
                delAllObj[i].checked = true;
            }
        }
    }
    else if (showType == 3) {
        //取消
        for (var i = 0; i < delAllObj.length; i++) {
            delAllObj[i].checked = false;
        }
    }
}
/*
*全选反选取消
*objName:name属性
*obj：判断对象，true-全选，false-取消
*/
function selectCheckBoxDelAllCheck(obj, objName) {
    var delAllObj = document.getElementsByName(objName);
    if (obj.checked == true) {
        //全选
        for (var i = 0; i < delAllObj.length; i++) {
            delAllObj[i].checked = true;
        }
    }
    else {
        //取消
        for (var i = 0; i < delAllObj.length; i++) {
            delAllObj[i].checked = false;
        }
    }
}
/*
*验证浮点数
*val:输入
*/
function isFloat(val) {
    var re = /^[0-9]+.?[0-9]*$/;
    if (re.test(val)) {
        return true;
    }
    else {
        return false;
    }
}
/*
*判断字符串是否为正整数
*判断正整数 /^[1-9]+[0-9]*]*$/
*/
function checkZS(input) {
    var re = /^[1-9]+[0-9]*]*$/;
    if (!re.test(input)) {
        return false;
    }
    else {
        return true;
    }
}

/*
*判断字符串是否为非负整数
*判断正整数 /^[1-9]+[0-9]*]*$/
*/
function checkZSIncludeZero(input) {
    var re = /^[1-9]+[0-9]*]*$/;
    if (!re.test(input)) {
        if (Number(input) == 0) {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        return true;
    }
}
//验证是否为数字，且小数点后最多保留2位小数，可以为负数
function isNum(s) {
    var pattern = /^([+-]{0,1})\d+(\.[0-9]{0,2})?$/;
    if (pattern.test(s)) {
        return true;
    }
    return false;
}
//手机验证
function isMobile(mobile) {
    return (/^(?:13\d|14\d|15\d|17\d|18\d)-?\d{5}(\d{3}|\*{3})$/.test(mobile));
}
//电话验证
function isTel(tel) {
    return (/^(([0\+]\d{2,3}-)?(0\d{2,3})-)(\d{7,8})(-(\d{3,}))?/.test(tel));
}
//电话或者手机验证
function isTelOrMobile(s) {
    return (isMobile(s) || isTel(s));
}
//邮件验证
function isEmail(s) {
    return (/^(\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)$/.test(s));
}

//中文验证
function isChinese(s) {
    return (/^[\u4e00-\u9fa5]+$/.test(s));
}

//11位手机验证
function isPhone11(mobile) {
    return (/^1[3|4|5|7|8]\d{9}$/.test(mobile));
}


//验证电话
function validatePhone(phone) {
    if (phone == '') {
        return true;
    }

    var arrPhone = phone.split(',');
    var l = arrPhone.length;
    if (l > 3) {
        return false;
    }

    for (var i = 0; i < l; i++) {
        if (!/(^0[0-9]{2,3}-[0-9]{7,8}$)|(^0[0-9]{2,3}-[0-9]{7,8}-[0-9]{1,5}$)|(^13[0-9]{9}$)|(^14[0-9]{9}$)|(^15[0-9]{9}$)|(^17[0-9]{9}$)|(^18[0-9]{9}$)|(^400\d{7}$)/.test(arrPhone[i])) {
            return false;
        }
    }

    return true;
}
//验证传真
function validateFax(phone) {
    if (phone == '') {
        return true;
    }

    var arrPhone = phone.split(',');
    var l = arrPhone.length;
    if (l > 3) {
        return false;
    }

    for (var i = 0; i < l; i++) {
        if (!/(^0[0-9]{2,3}-[0-9]{7,8}$)|(^0[0-9]{2,3}-[0-9]{7,8}-[0-9]{1,5}$)|(^13[0-9]{9}$)|(^14[0-9]{9}$)|(^15[0-9]{9}$)|(^17[0-9]{9}$)|(^18[0-9]{9}$)|(^400\d{7}$)/.test(arrPhone[i])) {
            return false;
        }
    }
    return true;
}

//验证电话号码
function checkTelephone(phone) {
    if (phone == '') {
        return true;
    }
    if (!/(^0[0-9]{2,3}-[0-9]{7,8}$)|(^0[0-9]{2,3}-[0-9]{7,8}-[0-9]{1,5}$)|(^13[0-9]{9}$)|(^14[0-9]{9}$)|(^15[0-9]{9}$)|(^17[0-9]{9}$)|(^18[0-9]{9}$)|(^400\d{7}$)/.test(phone)) {
        return false;
    }
    return true;
}

// 验证url
function isURL(str_url) {
    var RegUrl = new RegExp();
    RegUrl.compile("^[A-Za-z]+://[A-Za-z0-9-_]+\\.[A-Za-z0-9-_%&\?\/.=]+$");
    if (!RegUrl.test(str_url)) {
        return false;
    }
    return true;
}

//营业执照验证
//目前只验证长度和是否都为数字
function isLicense(s) {
    return (!isNaN(s) && s.length == 13);
}
//身份证验证
function checkIdcard(idcard) {
    idcard = idcard.replace('x', 'X');
    var area = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" }
    var idcard, Y, JYM;
    var S, M;
    var idcard_array = new Array();
    idcard_array = idcard.split("");
    //地区检验 
    if (area[parseInt(idcard.substr(0, 2))] == null) return false;
    //身份号码位数及格式检验 
    switch (idcard.length) {
        case 15:
            if ((parseInt(idcard.substr(6, 2)) + 1900) % 4 == 0 || ((parseInt(idcard.substr(6, 2)) + 1900) % 100 == 0 && (parseInt(idcard.substr(6, 2)) + 1900) % 4 == 0)) {
                ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}$/; //测试出生日期的合法性 
            }
            else {
                ereg = /^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}$/; //测试出生日期的合法性 
            }
            if (ereg.test(idcard)) return true;
            else return false;
            break;
        case 18:
            //18位身份号码检测 
            //出生日期的合法性检查  
            //闰年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9])) 
            //平年月日:((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8])) 
            if (parseInt(idcard.substr(6, 4)) % 4 == 0 || (parseInt(idcard.substr(6, 4)) % 100 == 0 && parseInt(idcard.substr(6, 4)) % 4 == 0)) {
                ereg = /^[1-9][0-9]{5}19[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}[0-9Xx]$/; //闰年出生日期的合法性正则表达式 
            } else {
                ereg = /^[1-9][0-9]{5}19[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}[0-9Xx]$/; //平年出生日期的合法性正则表达式 
            }
            if (ereg.test(idcard)) {//测试出生日期的合法性 
                //计算校验位 
                S = (parseInt(idcard_array[0]) + parseInt(idcard_array[10])) * 7
                        + (parseInt(idcard_array[1]) + parseInt(idcard_array[11])) * 9
                        + (parseInt(idcard_array[2]) + parseInt(idcard_array[12])) * 10
                        + (parseInt(idcard_array[3]) + parseInt(idcard_array[13])) * 5
                        + (parseInt(idcard_array[4]) + parseInt(idcard_array[14])) * 8
                        + (parseInt(idcard_array[5]) + parseInt(idcard_array[15])) * 4
                        + (parseInt(idcard_array[6]) + parseInt(idcard_array[16])) * 2
                        + parseInt(idcard_array[7]) * 1
                        + parseInt(idcard_array[8]) * 6
                        + parseInt(idcard_array[9]) * 3;
                Y = S % 11;
                M = "F";
                JYM = "10X98765432";
                M = JYM.substr(Y, 1); //判断校验位 
                if (M == idcard_array[17]) return true; //检测ID的校验位 
                else return false;
            }
            else return false;
            break;
        default:
            return false;
            break;
    }
}
/*
*页面调转
*
*/
function redirect(url) {
    window.location.href = url;
}
//将时间格式化年-月-日形式
//function getDate(datetime) {
//    datetime = datetime.toLocaleDateString();
//    datetime = datetime.replace(/年/, "-");
//    datetime = datetime.replace(/月/, "-");
//    datetime = datetime.replace(/日/, "");
//    return datetime;
//}

//function ChangeDateFormat(jsondate) {
//    jsondate = jsondate.replace("/Date(", "").replace(")/", "");
//    if (jsondate.indexOf("+") > 0) {
//        jsondate = jsondate.substring(0, jsondate.indexOf("+"));
//    }
//    else if (jsondate.indexOf("-") > 0) {
//        jsondate = jsondate.substring(0, jsondate.indexOf("-"));
//    }

//    var date = new Date(parseInt(jsondate, 10));
//    var month = date.getMonth() + 1 < 10 ?
//		"0" + (date.getMonth() + 1) : date.getMonth() + 1;
//    var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
//    return date.getFullYear() + "-" + month + "-" + currentDate;
//}
/*
*获取年月：YYYY-MM
*/
function getYearMonth(date) {
    var year = date.getFullYear();
    var month = date.getMonth() + 1;
    month = (month < 10 ? "0" + month : month);
    var mydate = (year.toString() + "-" + month.toString());
    return mydate;
}

/*
*json参数传递日期转为普通显示。(2015-08-14T02:41:16.883)-》//2009-06-12 
*datetime：输入参数
*/
function getJsonDate(datetime) {
    var time = "1970-01-01";
    if (datetime != "" && datetime.indexOf("T") > 0) {
        time = datetime.substring(0, datetime.indexOf("T"));

    } else {
        datetime = datetime.replace("/Date(", "").replace(")/", "");
        if (datetime.indexOf("+") > 0) {
            datetime = datetime.substring(0, datetime.indexOf("+"));
        } else if (datetime.indexOf("-") > 0) {
            datetime = datetime.substring(0, datetime.indexOf("-"));
        }
        datetime = new Date(parseInt(datetime, 10));
        var year = datetime.getFullYear();
        var month = datetime.getMonth() + 1;
        var date = datetime.getDate();
        if (month < 10) {
            month = "0" + month;
        }
        if (date < 10) {
            date = "0" + date;
        }
        time = year + "-" + month + "-" + date;
    }
    return time;
}

/*
*普通new date()形式参数日期转为普通显示。(Tue Oct 27 2015 17:39:11 GMT+0800)-》//2009-06-12 
*datetime：输入参数
*/
function getDate(datetime) {
    var year = datetime.getFullYear();
    var month = datetime.getMonth() + 1;
    var date = datetime.getDate();
    if (month < 10) {
        month = "0" + month;
    }
    if (date < 10) {
        date = "0" + date;
    }
    var time = year + "-" + month + "-" + date;
    return time;
}
/*
*普通new date()形式参数日期转为普通显示。(Tue Oct 27 2015 17:39:11 GMT+0800)-》//2009-06-12 17:18:05
*datetime：输入参数
*/
function getDateTime(datetime) {
    var year = datetime.getFullYear();
    var month = datetime.getMonth() + 1;
    var date = datetime.getDate();
    var hour = datetime.getHours();
    var Minutes = datetime.getMinutes();
    var Seconds = datetime.getSeconds();

    if (month < 10) {
        month = "0" + month;
    }
    if (date < 10) {
        date = "0" + date;
    }
    if (hour < 10) {
        hour = "0" + hour;
    }
    if (Minutes < 10) {
        Minutes = "0" + Minutes;
    }
    if (Seconds < 10) {
        Seconds = "0" + Seconds;
    }

    var time = year + "-" + month + "-" + date + " " + hour + ":" + Minutes + ":" + Seconds;
    return time;
}
//获取当月最后一天
function getFirstDate(newDate) {
    var year = newDate.getFullYear();
    var month = newDate.getMonth();
    return getDate(new Date(year, month, 1));//获取当月最后一天日期   
}
//获取当月最后一天
function getLastDate(newDate) {
    var year = newDate.getFullYear();//取当前的年份   
    var month = newDate.getMonth() + 1;//取下一个月的第一天，方便计算（最后一天不固定）
    if (month >= 12)      //如果当前大于12月，则年份转到下一年   
    {
        month -= 12;    //月份减   
        year++;      //年份增   
    }
    var new_date = new Date(year, month, 1);        //取当年当月中的第一天   
    return getDate(new Date(new_date.getTime() - 1000 * 60 * 60 * 24));//获取当月最后一天日期   
}
//判断日期格式是否合法
String.prototype.isDate = function () {
    var r = this.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
    if (r == null) return false; var d = new Date(r[1], r[3] - 1, r[4]);
    return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4]);
}
//计算字符串长度，汉字算2
function Len(str) {
    var i, sum;
    sum = 0;
    for (i = 0; i < str.length; i++) {
        if ((str.charCodeAt(i) >= 0) && (str.charCodeAt(i) <= 255))
            sum = sum + 1;
        else
            sum = sum + 2;
    }
    return sum;
}
//检查是否含有汉字
function checkChars(s) {
    if (/[^\x00-\xff]/g.test(s)) {
        return true; //含有汉字
    }
    else {
        return false; //全是字符
    }
}
//trim
function trim(s) {
    return s.replace(/(^[\s\u3000]*)|([\s\u3000]*$)/g, "");
}
//左trim
function LTrimChar(s, chars) {
    var i = 0;
    var length = s.length;
    var retval = "";
    for (i = 0; i < length; i++) {
        if (retval == "") {
            if (s.charAt(i) != chars) {
                retval = retval + s.charAt(i);
            }
        }
        else {
            retval = retval + s.charAt(i);
        }
    }
    return retval;
}
//右trim
function RTrimChar(s, chars) {
    var i = 0;
    var length = s.length;
    var retval = "";
    for (i = 0; i < length; i++) {
        if (retval == "") {
            if (s.charAt(length - i - 1) != chars) {
                retval = s.charAt(length - i - 1) + retval;
            }
        }
        else {
            retval = s.charAt(length - i - 1) + retval;
        }
    }
    return retval;
}
//左右trim
function TrimChar(s, chars) {
    s = LTrimChar(s, chars);
    s = RTrimChar(s, chars);
    return s;
}

//Tab标签点击切换函数
function Show_TabADSMenu(tabadid_num, tabadnum) {
    for (var i = 0; i < 2; i++) { document.getElementById("tabadcontent_" + tabadid_num + i).style.display = "none"; }
    for (var i = 0; i < 2; i++) { document.getElementById("tabadmenu_" + tabadid_num + i).className = ""; }
    document.getElementById("tabadmenu_" + tabadid_num + tabadnum).className = "linknow";
    document.getElementById("tabadcontent_" + tabadid_num + tabadnum).style.display = "block";
}
//编辑数据
function toggles(obj_id) {
    var target = document.getElementById(obj_id);

    var bgObj = document.getElementById("bgDiv");
    bgObj.style.width = document.body.offsetWidth + "px";
    bgObj.style.height = screen.height + "px";

    var bgOifm = document.getElementById("iframe_top");
    bgOifm.style.width = document.body.offsetWidth + "px";
    bgOifm.style.height = screen.height + "px";

    if (target.style.display == "none") {
        target.style.display = "block";
        target.style.top = (document.documentElement.scrollTop + document.documentElement.clientHeight / 2);
        bgObj.style.display = "block";
        bgOifm.style.display = "block";
    }
    else {
        target.style.display = "none";
        bgObj.style.display = "none";
        bgOifm.style.display = "none";
    }
}

//日历控件start*************
//兼容性的日历控件,显示年-月-日
function L_calendar() { }
L_calendar.prototype = {
    _VersionInfo: "Version:1.0&#13", //版本
    IsMoreThanToday: true, //日期是否可以大于今天
    Moveable: true,
    NewName: "",
    insertId: "",
    ClickObject: null, //点击对象来源
    InputObject: null, //输入对象来原
    InputDate: null, //输入日期
    CallBackFunction: null, //回调函数
    IsOpen: false,
    MouseX: 0, //
    MouseY: 0, //
    GetDateLayer: function () { //显示层
        if (window.parent) {
            return window.parent.L_DateLayer;
        }
        else { return window.L_DateLayer; }
    },
    L_TheYear: new Date().getFullYear(), //定义年的变量的初始值
    L_TheMonth: new Date().getMonth() + 1, //定义月的变量的初始值
    L_WDay: new Array(39), //定义写日期的数组
    MonHead: new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31),  //定义阳历中每个月的最大天数
    GetY: function () {
        var obj;
        if (arguments.length > 0) {
            obj == arguments[0];
        }
        else {
            obj = this.ClickObject;
        }
        if (obj != null) {
            var y = obj.offsetTop;
            while (obj = obj.offsetParent) y += obj.offsetTop;
            return y;
        }
        else { return 0; }
    },
    GetX: function () {
        var obj;
        if (arguments.length > 0) {
            obj == arguments[0];

        }
        else {
            obj = this.ClickObject;
        }
        if (obj != null) {
            var y = obj.offsetLeft;
            while (obj = obj.offsetParent) y += obj.offsetLeft;
            return y;
        }
        else { return 0; }
    },
    CreateHTML: function () {
        var htmlstr = "";
        htmlstr += "<div id=\"L_calendar\">\r\n";
        htmlstr += "<span id=\"SelectYearLayer\" style=\"z-index: 100100;position: absolute;top: 3; left: 19;display: none\"></span>\r\n";
        htmlstr += "<span id=\"SelectMonthLayer\" style=\"z-index: 100100;position: absolute;top: 3; left: 78;display: none\"></span>\r\n";
        htmlstr += "<div id=\"L_calendar-year-month\"><div id=\"L_calendar-PrevM\" onclick=\"parent." + this.NewName + ".PrevM()\" title=\"前一月\"><b>&lt;</b></div><div id=\"L_calendar-year\" onclick=\"parent." + this.NewName + ".SelectYearInnerHTML('" + this.L_TheYear + "')\"></div><div id=\"L_calendar-month\"   onclick=\"parent." + this.NewName + ".SelectMonthInnerHTML('" + this.L_TheMonth + "')\"></div><div id=\"L_calendar-NextM\" onclick=\"parent." + this.NewName + ".NextM()\" title=\"后一月\"><b>&gt;</b></div></div>\r\n";
        htmlstr += "<div id=\"L_calendar-week\"><ul  onmouseup=\"StopMove()\"><li>日</li><li>一</li><li>二</li><li>三</li><li>四</li><li>五</li><li>六</li></ul></div>\r\n";
        htmlstr += "<div id=\"L_calendar-day\">\r\n";
        htmlstr += "<ul>\r\n";
        for (var i = 0; i < this.L_WDay.length; i++) {
            htmlstr += "<li id=\"L_calendar-day_" + i + "\" style=\"background:#EFEFEF\" onmouseover=\"this.style.background='#ffffff'\"  onmouseout=\"this.style.background='#e0e0e0'\"></li>\r\n";
        }
        htmlstr += "</ul>\r\n";
        //htmlstr+="<span id=\"L_calendar-today\" onclick=\"parent."+this.NewName+".Today()\"><b>Today</b></span>\r\n";
        htmlstr += "</div>\r\n";
        //htmlstr+="<div id=\"L_calendar-control\"></div>\r\n";
        htmlstr += "</div>\r\n";
        htmlstr += "<scr" + "ipt type=\"text/javas" + "cript\">\r\n";
        htmlstr += "var MouseX,MouseY;";
        htmlstr += "var Moveable=" + this.Moveable + ";\r\n";
        htmlstr += "var MoveaStart=false;\r\n";
        htmlstr += "document.onmousemove=function(e)\r\n";
        htmlstr += "{\r\n";
        htmlstr += "var DateLayer=parent.document.getElementById(\"L_DateLayer\");\r\n";
        htmlstr += "	e = window.event || e;\r\n";
        htmlstr += "var DateLayerLeft=DateLayer.style.posLeft || parseInt(DateLayer.style.left.replace(\"px\",\"\"));\r\n";
        htmlstr += "var DateLayerTop=DateLayer.style.posTop || parseInt(DateLayer.style.top.replace(\"px\",\"\"));\r\n";
        htmlstr += "if(MoveaStart){DateLayer.style.left=(DateLayerLeft+e.clientX-MouseX)+\"px\";DateLayer.style.top=(DateLayerTop+e.clientY-MouseY)+\"px\"}\r\n";
        htmlstr += ";\r\n";
        htmlstr += "}\r\n";

        htmlstr += "document.getElementById(\"L_calendar-week\").onmousedown=function(e){\r\n";
        htmlstr += "if(Moveable){MoveaStart=true;}\r\n";
        htmlstr += "	e = window.event || e;\r\n";
        htmlstr += "  MouseX = e.clientX;\r\n";
        htmlstr += "  MouseY = e.clientY;\r\n";
        htmlstr += "	}\r\n";

        htmlstr += "function StopMove(){\r\n";
        htmlstr += "MoveaStart=false;\r\n";
        htmlstr += "	}\r\n";
        htmlstr += "</scr" + "ipt>\r\n";
        var stylestr = "";
        stylestr += "<style type=\"text/css\">";
        stylestr += "body{background:transparent;font-size:12px;margin:0px;padding:0px;text-align:left;font-family:宋体,Arial, Helvetica, sans-serif;line-height:1.6em;color:#666666;}\r\n";
        stylestr += "#L_calendar{background:#fff;border:1px solid #7AB8DF;width:158px;padding:1px;height:180px;z-index:100099;text-align:center}\r\n";
        stylestr += "#L_calendar-year-month{height:23px;line-height:23px;z-index:100099;background-color:#2670C1;color:#fff}\r\n";
        stylestr += "#L_calendar-year{line-height:23px;width:60px;float:left;z-index:100099;position: absolute;top: 3; left: 19;cursor:default}\r\n";
        stylestr += "#L_calendar-month{line-height:23px;width:48px;float:left;z-index:100099;position: absolute;top: 3; left: 78;cursor:default}\r\n";
        stylestr += "#L_calendar-PrevM{position: absolute;top: 3; left: 5;cursor:pointer}"
        stylestr += "#L_calendar-NextM{position: absolute;top: 3; left: 145;cursor:pointer}"
        stylestr += "#L_calendar-week{height:23px;line-height:23px;z-index:100099;}\r\n";
        stylestr += "#L_calendar-day{height:136px;z-index:100099;}\r\n";
        stylestr += "#L_calendar-week ul{cursor:move;list-style:none;margin:0px;padding:0px;}\r\n";
        stylestr += "#L_calendar-week li{width:20px;height:20px;float:left;;margin:1px;padding:0px;text-align:center;}\r\n";
        stylestr += "#L_calendar-day ul{list-style:none;margin:0px;padding:0px;}\r\n";
        stylestr += "#L_calendar-day li{cursor:pointer;width:20px;height:20px;float:left;;margin:1px;padding:0px;}\r\n";
        stylestr += "#L_calendar-control{height:25px;z-index:100099;}\r\n";
        stylestr += "#L_calendar-today{cursor:pointer;float:left;width:63px;height:20px;line-height:20px;margin:1px;text-align:center;background:#3B80CB}"
        stylestr += "</style>";
        var TempLateContent = "<html>\r\n";
        TempLateContent += "<head>\r\n";
        TempLateContent += "<title></title>\r\n";
        TempLateContent += stylestr;
        TempLateContent += "</head>\r\n";
        TempLateContent += "<body>\r\n";
        TempLateContent += htmlstr;
        TempLateContent += "</body>\r\n";
        TempLateContent += "</html>\r\n";
        this.GetDateLayer().document.writeln(TempLateContent);
        this.GetDateLayer().document.close();
    },
    InsertHTML: function (id, htmlstr) {
        var L_DateLayer = this.GetDateLayer();
        if (L_DateLayer) { L_DateLayer.document.getElementById(id).innerHTML = htmlstr; }
    },
    WriteHead: function (yy, mm)  //往 head 中写入当前的年与月
    {
        this.InsertHTML("L_calendar-year", yy + " 年");
        this.InsertHTML("L_calendar-month", mm + " 月");
    },
    IsPinYear: function (year)            //判断是否闰平年
    {
        if (0 == year % 4 && ((year % 100 != 0) || (year % 400 == 0))) return true; else return false;
    },
    GetMonthCount: function (year, month)  //闰年二月为29天
    {
        var c = this.MonHead[month - 1]; if ((month == 2) && this.IsPinYear(year)) c++; return c;
    },
    GetDOW: function (day, month, year)     //求某天的星期几
    {
        var dt = new Date(year, month - 1, day).getDay() / 7; return dt;
    },
    GetText: function (obj) {
        if (obj.innerText) { return obj.innerText }
        else { return obj.textContent }
    },
    PrevM: function ()  //往前翻月份
    {
        if (this.L_TheMonth > 1) { this.L_TheMonth-- } else { this.L_TheYear--; this.L_TheMonth = 12; }
        this.SetDay(this.L_TheYear, this.L_TheMonth);
    },
    NextM: function ()  //往后翻月份
    {
        if (this.L_TheMonth == 12) { this.L_TheYear++; this.L_TheMonth = 1 } else { this.L_TheMonth++ }
        this.SetDay(this.L_TheYear, this.L_TheMonth);
    },
    Today: function ()  //Today Button
    {
        var today;
        this.L_TheYear = new Date().getFullYear();
        this.L_TheMonth = new Date().getMonth() + 1;
        today = new Date().getDate();
        if (this.InputObject) {
            this.InputObject.value = this.L_TheYear + "-" + this.L_TheMonth + "-" + today;
        }
        this.CloseLayer();
    },
    SetDay: function (yy, mm)   //主要的写程序**********
    {
        this.WriteHead(yy, mm);
        //设置当前年月的公共变量为传入值
        this.L_TheYear = yy;
        this.L_TheMonth = mm;
        for (var i = 0; i < 39; i++) {
            this.L_WDay[i] = "";
        };  //将显示框的内容全部清空
        var day1 = 1, day2 = 1, firstday = new Date(yy, mm - 1, 1).getDay();  //某月第一天的星期几

        for (i = 0; i < firstday; i++) this.L_WDay[i] = this.GetMonthCount(mm == 1 ? yy - 1 : yy, mm == 1 ? 12 : mm - 1) - firstday + i + 1; //上个月的最后几天
        for (i = firstday; day1 < this.GetMonthCount(yy, mm) + 1; i++) { this.L_WDay[i] = day1; day1++; }
        for (i = firstday + this.GetMonthCount(yy, mm); i < 39; i++) {
            this.L_WDay[i] = day2;
            day2++;
        }
        for (i = 0; i < 39; i++) {
            var da = this.GetDateLayer().document.getElementById("L_calendar-day_" + i + "");
            var month, day;
            if (this.L_WDay[i] != "") {
                if (i < firstday) {
                    //da.innerHTML="<b style=\"color:gray\">" + this.L_WDay[i] + "</b>";
                    da.innerHTML = "";
                    //month=(mm==1?12:mm-1);
                    //day=this.L_WDay[i];
                    if (document.all) {
                        da.onclick = null;
                    }
                    else {
                        da.setAttribute("onclick", "null");
                    }
                }
                else if (i >= firstday + this.GetMonthCount(yy, mm)) {
                    //da.innerHTML="<b style=\"color:gray\">" + this.L_WDay[i] + "</b>";
                    da.innerHTML = "";
                    //month=(mm==1?12:mm+1);
                    //day=this.L_WDay[i];
                    if (document.all) {
                        da.onclick = null;
                    }
                    else {
                        da.setAttribute("onclick", "null");
                    }
                }
                else {
                    da.innerHTML = "<b style=\"color:#000\">" + this.L_WDay[i] + "</b>";
                    //month=(mm==1?12:mm);
                    month = mm;
                    day = this.L_WDay[i];
                    if (document.all) {
                        da.onclick = Function("parent." + this.NewName + ".DayClick(" + month + "," + day + ")");
                    }
                    else {
                        da.setAttribute("onclick", "parent." + this.NewName + ".DayClick(" + month + "," + day + ")");
                    }
                    da.title = month + "月" + day + "日";
                    da.style.background = (yy == new Date().getFullYear() && month == new Date().getMonth() + 1 && day == new Date().getDate()) ? "#3B80CB" : "#EFEFEF";
                    if (this.InputDate != null) {
                        if (yy == this.InputDate.getFullYear() && month == this.InputDate.getMonth() + 1 && day == this.InputDate.getDate()) {
                            da.style.background = "#FF0000";
                        }
                    }
                }

            }
        }
    },
    SelectYearInnerHTML: function (strYear) //年份的下拉框
    {
        if (strYear.match(/\D/) != null) { alert("年份输入参数不是数字！"); return; }
        var m = (strYear) ? strYear : new Date().getFullYear();
        if (m < 1000 || m > 9999) { alert("年份值不在 1000 到 9999 之间！"); return; }
        var n = m - 20;
        if (n < 1000) n = 1000;
        if (n + 26 > 9999) n = 9974;
        var s = "<select name=\"L_SelectYear\" id=\"L_SelectYear\" style='font-size: 12px' "
        s += "onblur='document.getElementById(\"SelectYearLayer\").style.display=\"none\"' "
        s += "onchange='document.getElementById(\"SelectYearLayer\").style.display=\"none\";"
        s += "parent." + this.NewName + ".L_TheYear = this.value; parent." + this.NewName + ".SetDay(parent." + this.NewName + ".L_TheYear,parent." + this.NewName + ".L_TheMonth)'>\r\n";
        var selectInnerHTML = s;
        for (var i = n; i < n + 46; i++) {
            if (i == m)
            { selectInnerHTML += "<option value='" + i + "' selected>" + i + "年" + "</option>\r\n"; }
            else { selectInnerHTML += "<option value='" + i + "'>" + i + "年" + "</option>\r\n"; }
        }
        selectInnerHTML += "</select>";
        var DateLayer = this.GetDateLayer();
        DateLayer.document.getElementById("SelectYearLayer").style.display = "";
        DateLayer.document.getElementById("SelectYearLayer").innerHTML = selectInnerHTML;
        DateLayer.document.getElementById("L_SelectYear").focus();
    },
    SelectMonthInnerHTML: function (strMonth) //月份的下拉框
    {
        if (strMonth.match(/\D/) != null) { alert("月份输入参数不是数字！"); return; }
        var m = (strMonth) ? strMonth : new Date().getMonth() + 1;
        var s = "<select name=\"L_SelectYear\" id=\"L_SelectMonth\" style='font-size: 12px' "
        s += "onblur='document.getElementById(\"SelectMonthLayer\").style.display=\"none\"' "
        s += "onchange='document.getElementById(\"SelectMonthLayer\").style.display=\"none\";"
        s += "parent." + this.NewName + ".L_TheMonth = this.value; parent." + this.NewName + ".SetDay(parent." + this.NewName + ".L_TheYear,parent." + this.NewName + ".L_TheMonth)'>\r\n";
        var selectInnerHTML = s;
        for (var i = 1; i < 13; i++) {
            if (i == m)
            { selectInnerHTML += "<option value='" + i + "' selected>" + i + "月" + "</option>\r\n"; }
            else { selectInnerHTML += "<option value='" + i + "'>" + i + "月" + "</option>\r\n"; }
        }
        selectInnerHTML += "</select>";
        var DateLayer = this.GetDateLayer();
        DateLayer.document.getElementById("SelectMonthLayer").style.display = "";
        DateLayer.document.getElementById("SelectMonthLayer").innerHTML = selectInnerHTML;
        DateLayer.document.getElementById("L_SelectMonth").focus();
    },
    DayClick: function (mm, dd)  //点击显示框选取日期，主输入函数*************
    {
        var yy = this.L_TheYear;
        //判断月份，并进行对应的处理
        if (mm < 1) { yy--; mm = 12 + mm; }
        else if (mm > 12) { yy++; mm = mm - 12; }
        if (mm < 10) { mm = "0" + mm; }
        if (this.ClickObject) {
            if (!dd) { return; }
            if (dd < 10) { dd = "0" + dd; }
            if (this.IsMoreThanToday) {
                this.InputObject.value = yy + "-" + mm + "-" + dd; //注：在这里你可以输出改成你想要的格式
                this.CloseLayer();
                if (typeof (this.CallBackFunction) == 'function') {
                    this.CallBackFunction();
                }
            }
            else {
                if (GreaterThanToday(yy + "-" + mm + "-" + dd)) {
                    alert('所选日期不可大于当前日期！');
                }
                else {
                    this.InputObject.value = yy + "-" + mm + "-" + dd; //注：在这里你可以输出改成你想要的格式
                    this.CloseLayer();
                    if (typeof (this.CallBackFunction) == 'function') {
                        this.CallBackFunction();
                    }
                }
            }

        }
        else { this.CloseLayer(); alert("您所要输出的控件对象并不存在！"); }
    },
    SetDate: function () { //设置日期，参数1：点击对象，参数2：输入对象，参数3：回调函数,参数4：是否可以大于当前日期
        if (arguments.length < 1) { alert("对不起！传入参数太少！"); return; }
        else if (arguments.length > 4) { alert("对不起！传入参数太多！"); return; }
        this.InputObject = (arguments.length == 1 || arguments.length == 2) ? arguments[0] : arguments[1];
        this.ClickObject = arguments[0];
        if (arguments.length == 3) {
            this.CallBackFunction = arguments[2];
        }
        else if (arguments.length == 4) {
            this.IsMoreThanToday = false;
        }
        else { this.CallBackFunction = null; }
        var reg = /^(\d+)-(\d{1,2})-(\d{1,2})$/;
        var r = this.InputObject.value.match(reg);
        if (r != null) {
            r[2] = r[2] - 1;
            var d = new Date(r[1], r[2], r[3]);
            if (d.getFullYear() == r[1] && d.getMonth() == r[2] && d.getDate() == r[3]) {
                this.InputDate = d; 	//保存外部传入的日期
            }
            else this.InputDate = "";
            this.L_TheYear = r[1];
            this.L_TheMonth = r[2] + 1;
        }
        else {
            this.L_TheYear = new Date().getFullYear();
            this.L_TheMonth = new Date().getMonth() + 1;
        }
        this.CreateHTML();
        var top = this.GetY();
        var left = this.GetX();
        var DateLayer = document.getElementById("L_DateLayer");
        DateLayer.style.top = top + this.ClickObject.clientHeight + 5 + "px";
        DateLayer.style.left = left + "px";
        DateLayer.style.display = "block";
        if (document.all) {
            this.GetDateLayer().document.getElementById("L_calendar").style.width = "163px";
            this.GetDateLayer().document.getElementById("L_calendar").style.height = "189px";
        }
        else {
            this.GetDateLayer().document.getElementById("L_calendar").style.width = "154px";
            this.GetDateLayer().document.getElementById("L_calendar").style.height = "180px";
            DateLayer.style.width = "158px";
            DateLayer.style.height = "250px";
        }
        //alert(DateLayer.style.display)
        this.SetDay(this.L_TheYear, this.L_TheMonth);
    },
    CloseLayer: function () {
        try {
            var DateLayer = document.getElementById("L_DateLayer");
            if ((DateLayer.style.display == "" || DateLayer.style.display == "block") && arguments[0] != this.ClickObject && arguments[0] != this.InputObject) {
                DateLayer.style.display = "none";
            }
        }
        catch (e) { }
    }
}

document.writeln('<iframe id="L_DateLayer" name="L_DateLayer" frameborder="0" style="position:absolute;width:164px; height:190px;overflow:hidden;z-index:100099;display:none;backgorund-color:transparent;"></iframe>');
var MyCalendar = new L_calendar();
MyCalendar.NewName = "MyCalendar";
document.onclick = function (e) {
    e = window.event || e;
    var srcElement = e.srcElement || e.target;
    MyCalendar.CloseLayer(srcElement); //关闭日历控件
    MyCalendarMonth.CloseLayer(srcElement); //关闭日历控件
}

//兼容性的日历控件,显示年-月
function L_calendarMonth() { }
L_calendarMonth.prototype = {
    _VersionInfo: "Version:1.0&#13", //版本号
    Moveable: true,
    NewName: "",
    insertId: "",
    ClickObject: null, //点击对象来源
    InputObject: null, //输入对象来源
    InputDate: null, //输入日期
    CallBackFunction: null, //回调函数
    IsOpen: false,
    MouseX: 0,
    MouseY: 0,
    GetDateLayer: function () {
        if (window.parent) {
            return window.parent.L_MonthLayer;
        }
        else { return window.L_MonthLayer; }
    },
    L_TheYear: new Date().getFullYear(), //定义年的变量的初始值
    L_TheMonth: new Date().getMonth() + 1, //定义月的变量的初始值
    L_WDay: new Array(12), //定义写日期的数组
    MonHead: new Array(31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31),    		   //定义阳历中每个月的最大天数
    GetY: function () {
        var obj;
        if (arguments.length > 0) {
            obj == arguments[0];
        }
        else {
            obj = this.ClickObject;
        }
        if (obj != null) {
            var y = obj.offsetTop;
            while (obj = obj.offsetParent) y += obj.offsetTop;
            return y;
        }
        else { return 0; }
    },
    GetX: function () {
        var obj;
        if (arguments.length > 0) {
            obj == arguments[0];

        }
        else {
            obj = this.ClickObject;
        }
        if (obj != null) {
            var y = obj.offsetLeft;
            while (obj = obj.offsetParent) y += obj.offsetLeft;
            return y;
        }
        else { return 0; }
    },
    CreateHTML: function () {
        var htmlstr = "";
        htmlstr += "<div id=\"L_calendar\">\r\n";
        htmlstr += "<span id=\"SelectYearLayer\" style=\"z-index: 100100;position: absolute;top: 3; left: 19;display: none\"></span>\r\n";
        htmlstr += "<span id=\"SelectMonthLayer\" style=\"z-index: 100100;position: absolute;top: 3; left: 78;display: none\"></span>\r\n";
        //htmlstr += "<div id=\"L_calendar-year-month\"><div id=\"L_calendar-PrevM\" onclick=\"parent." + this.NewName + ".PrevM()\" title=\"前一月\"><b>&lt;</b></div><div id=\"L_calendar-year\" onclick=\"parent." + this.NewName + ".SelectYearInnerHTML('" + this.L_TheYear + "')\"></div><div id=\"L_calendar-month\"   onclick=\"parent." + this.NewName + ".SelectMonthInnerHTML('" + this.L_TheMonth + "')\"></div><div id=\"L_calendar-NextM\" onclick=\"parent." + this.NewName + ".NextM()\" title=\"后一月\"><b>&gt;</b></div></div>\r\n";
        htmlstr += "<div id=\"L_calendar-year-month\"><div id=\"L_calendar-year\" onclick=\"parent." + this.NewName + ".SelectYearInnerHTML('" + this.L_TheYear + "')\"></div></div>\r\n";
        //htmlstr += "<div id=\"L_calendar-week\"><ul  onmouseup=\"StopMove()\"><li>日</li><li>一</li><li>二</li><li>三</li><li>四</li><li>五</li><li>六</li></ul></div>\r\n";
        htmlstr += "<div id=\"L_calendar-day\">\r\n";
        htmlstr += "<ul>\r\n";
        for (var i = 0; i < this.L_WDay.length; i++) {
            htmlstr += "<li id=\"L_calendar-day_" + i + "\" style=\"background:#EFEFEF\" onmouseover=\"this.style.background='#ffffff'\"  onmouseout=\"this.style.background='#e0e0e0'\"></li>\r\n";
        }
        htmlstr += "</ul>\r\n";
        //htmlstr+="<span id=\"L_calendar-today\" onclick=\"parent."+this.NewName+".Today()\"><b>Today</b></span>\r\n";
        htmlstr += "</div>\r\n";
        //htmlstr+="<div id=\"L_calendar-control\"></div>\r\n";
        htmlstr += "</div>\r\n";
        htmlstr += "<scr" + "ipt type=\"text/javas" + "cript\">\r\n";
        htmlstr += "var MouseX,MouseY;";
        htmlstr += "var Moveable=" + this.Moveable + ";\r\n";
        htmlstr += "var MoveaStart=false;\r\n";
        htmlstr += "document.onmousemove=function(e)\r\n";
        htmlstr += "{\r\n";
        htmlstr += "var DateLayer=parent.document.getElementById(\"L_MonthLayer\");\r\n";
        htmlstr += "	e = window.event || e;\r\n";
        htmlstr += "var DateLayerLeft=DateLayer.style.posLeft || parseInt(DateLayer.style.left.replace(\"px\",\"\"));\r\n";
        htmlstr += "var DateLayerTop=DateLayer.style.posTop || parseInt(DateLayer.style.top.replace(\"px\",\"\"));\r\n";
        htmlstr += "if(MoveaStart){DateLayer.style.left=(DateLayerLeft+e.clientX-MouseX)+\"px\";DateLayer.style.top=(DateLayerTop+e.clientY-MouseY)+\"px\"}\r\n";
        htmlstr += ";\r\n";
        htmlstr += "}\r\n";

        //        htmlstr += "document.getElementById(\"L_calendar-week\").onmousedown=function(e){\r\n";
        //        htmlstr += "if(Moveable){MoveaStart=true;}\r\n";
        //        htmlstr += "	e = window.event || e;\r\n";
        //        htmlstr += "  MouseX = e.clientX;\r\n";
        //        htmlstr += "  MouseY = e.clientY;\r\n";
        //        htmlstr += "	}\r\n";

        htmlstr += "function StopMove(){\r\n";
        htmlstr += "MoveaStart=false;\r\n";
        htmlstr += "	}\r\n";
        htmlstr += "</scr" + "ipt>\r\n";
        var stylestr = "";
        stylestr += "<style type=\"text/css\">";
        stylestr += "body{background:transparent;font-size:12px;margin:0px;padding:0px;text-align:left;font-family:宋体,Arial, Helvetica, sans-serif;line-height:1.6em;color:#666666;}\r\n";
        stylestr += "#L_calendar{background:#fff;border:1px solid #7AB8DF;width:110px;padding:1px;height:180px;z-index:100099;text-align:center}\r\n";
        stylestr += "#L_calendar-year-month{height:23px;line-height:23px;z-index:100099;background-color:#2670C1;color:#fff}\r\n";
        stylestr += "#L_calendar-year{line-height:23px;width:60px;float:left;z-index:100099;position: absolute;top: 3; left: 25;cursor:default}\r\n";
        stylestr += "#L_calendar-month{line-height:23px;width:48px;float:left;z-index:100099;position: absolute;top: 3; left: 78;cursor:default}\r\n";
        stylestr += "#L_calendar-PrevM{position: absolute;top: 3; left: 5;cursor:pointer}"
        stylestr += "#L_calendar-NextM{position: absolute;top: 3; left: 145;cursor:pointer}"
        stylestr += "#L_calendar-week{height:23px;line-height:23px;z-index:100099;}\r\n";
        stylestr += "#L_calendar-day{height:80px;z-index:100099;}\r\n";
        stylestr += "#L_calendar-week ul{cursor:move;list-style:none;margin:0px;padding:0px;}\r\n";
        stylestr += "#L_calendar-week li{width:20px;height:20px;float:left;;margin:1px;padding:0px;text-align:center;}\r\n";
        stylestr += "#L_calendar-day ul{list-style:none;margin:0px;padding:0px;}\r\n";
        stylestr += "#L_calendar-day li{cursor:pointer;width:25px;height:25px;float:left;;margin:1px;padding:0px;}\r\n";
        stylestr += "#L_calendar-control{height:25px;z-index:100099;}\r\n";
        stylestr += "#L_calendar-today{cursor:pointer;float:left;width:63px;height:20px;line-height:20px;margin:1px;text-align:center;background:#3B80CB}"
        stylestr += "</style>";
        var TempLateContent = "<html>\r\n";
        TempLateContent += "<head>\r\n";
        TempLateContent += "<title></title>\r\n";
        TempLateContent += stylestr;
        TempLateContent += "</head>\r\n";
        TempLateContent += "<body>\r\n";
        TempLateContent += htmlstr;
        TempLateContent += "</body>\r\n";
        TempLateContent += "</html>\r\n";
        this.GetDateLayer().document.writeln(TempLateContent);
        this.GetDateLayer().document.close();
    },
    InsertHTML: function (id, htmlstr) {
        var L_MonthLayer = this.GetDateLayer();
        if (L_MonthLayer) { L_MonthLayer.document.getElementById(id).innerHTML = htmlstr; }
    },
    WriteHead: function (yy, mm)  //往 head 中写入当前的年与月
    {
        this.InsertHTML("L_calendar-year", yy + " 年");
        //this.InsertHTML("L_calendar-month", mm + " 月");
    },
    IsPinYear: function (year)            //判断是否闰平年
    {
        if (0 == year % 4 && ((year % 100 != 0) || (year % 400 == 0))) return true; else return false;
    },
    GetMonthCount: function (year, month)  //闰年二月为29天
    {
        var c = this.MonHead[month - 1]; if ((month == 2) && this.IsPinYear(year)) c++; return c;
    },
    GetDOW: function (day, month, year)     //求某天的星期几
    {
        var dt = new Date(year, month - 1, day).getDay() / 7; return dt;
    },
    GetText: function (obj) {
        if (obj.innerText) { return obj.innerText }
        else { return obj.textContent }
    },
    PrevM: function ()  //往前翻月份
    {
        if (this.L_TheMonth > 1) { this.L_TheMonth-- } else { this.L_TheYear--; this.L_TheMonth = 12; }
        this.SetDay(this.L_TheYear, this.L_TheMonth);
    },
    NextM: function ()  //往后翻月份
    {
        if (this.L_TheMonth == 12) { this.L_TheYear++; this.L_TheMonth = 1 } else { this.L_TheMonth++ }
        this.SetDay(this.L_TheYear, this.L_TheMonth);
    },
    Today: function ()  //Today Button
    {
        var today;
        this.L_TheYear = new Date().getFullYear();
        this.L_TheMonth = new Date().getMonth() + 1;
        today = new Date().getDate();
        if (this.InputObject) {
            this.InputObject.value = this.L_TheYear + "-" + this.L_TheMonth + "-" + today;
        }
        this.CloseLayer();
    },
    SetDay: function (yy, mm)   //主要的写程序**********
    {
        this.WriteHead(yy, mm);
        //设置当前年月的公共变量为传入值
        this.L_TheYear = yy;
        this.L_TheMonth = mm;
        for (var i = 0; i < 12; i++) {
            this.L_WDay[i] = (i + 1).toString();
        };  //将显示框的内容全部清空
        var day1 = 1, day2 = 1, firstday = new Date(yy, mm - 1, 1).getDay();  //某月第一天的星期几

        //for (i = 0; i < firstday; i++) this.L_WDay[i] = this.GetMonthCount(mm == 1 ? yy - 1 : yy, mm == 1 ? 12 : mm - 1) - firstday + i + 1; //上个月的最后几天
        //for (i = firstday; day1 < this.GetMonthCount(yy, mm) + 1; i++) { this.L_WDay[i] = day1; day1++; }
        //for (i = firstday + this.GetMonthCount(yy, mm); i < 39; i++) {
        //            this.L_WDay[i] = day2;
        //            day2++;
        //        }
        for (i = 0; i < 12; i++) {
            var da = this.GetDateLayer().document.getElementById("L_calendar-day_" + i + "");
            var month, day;
            if (this.L_WDay[i] != "") {
                //                if (i < firstday) {
                //                    //da.innerHTML="<b style=\"color:gray\">" + this.L_WDay[i] + "</b>";
                //                    da.innerHTML = "";
                //                    //month=(mm==1?12:mm-1);
                //                    //day=this.L_WDay[i];
                //                    if (document.all) {
                //                        da.onclick = null;
                //                    }
                //                    else {
                //                        da.setAttribute("onclick", "null");
                //                    }
                //                }
                //                else if (i >= firstday + this.GetMonthCount(yy, mm)) {
                //                    //da.innerHTML="<b style=\"color:gray\">" + this.L_WDay[i] + "</b>";
                //                    da.innerHTML = "";
                //                    //month=(mm==1?12:mm+1);
                //                    //day=this.L_WDay[i];
                //                    if (document.all) {
                //                        da.onclick = null;
                //                    }
                //                    else {
                //                        da.setAttribute("onclick", "null");
                //                    }
                //                }
                //                else 
                {
                    da.innerHTML = "<b style=\"color:#000\">" + this.L_WDay[i] + "</b>";
                    //month=(mm==1?12:mm);
                    month = mm;
                    day = this.L_WDay[i];
                    if (document.all) {
                        da.onclick = Function("parent." + this.NewName + ".DayClick(" + month + "," + day + ")");
                    }
                    else {
                        da.setAttribute("onclick", "parent." + this.NewName + ".DayClick(" + month + "," + day + ")");
                    }
                    da.title = this.L_TheYear + "年" + day + "月";
                    da.style.background = (yy == new Date().getFullYear() && day == new Date().getMonth() + 1) ? "#3B80CB" : "#EFEFEF";
                    if (this.InputDate != null) {
                        if (yy == this.InputDate.getFullYear() && day == this.InputDate.getMonth() + 1) {
                            da.style.background = "#FF0000";
                        }
                    }
                }


            }
        }
    },
    SelectYearInnerHTML: function (strYear) //年份的下拉框
    {
        if (strYear.match(/\D/) != null) { alert("年份输入参数不是数字！"); return; }
        var m = (strYear) ? strYear : new Date().getFullYear();
        if (m < 1000 || m > 9999) { alert("年份值不在 1000 到 9999 之间！"); return; }
        var n = m - 20;
        if (n < 1000) n = 1000;
        if (n + 26 > 9999) n = 9974;
        var s = "<select name=\"L_SelectYear\" id=\"L_SelectYear\" style='font-size: 12px' "
        s += "onblur='document.getElementById(\"SelectYearLayer\").style.display=\"none\"' "
        s += "onchange='document.getElementById(\"SelectYearLayer\").style.display=\"none\";"
        s += "parent." + this.NewName + ".L_TheYear = this.value; parent." + this.NewName + ".SetDay(parent." + this.NewName + ".L_TheYear,parent." + this.NewName + ".L_TheMonth)'>\r\n";
        var selectInnerHTML = s;
        for (var i = n; i < n + 46; i++) {
            if (i == m)
            { selectInnerHTML += "<option value='" + i + "' selected>" + i + "年" + "</option>\r\n"; }
            else { selectInnerHTML += "<option value='" + i + "'>" + i + "年" + "</option>\r\n"; }
        }
        selectInnerHTML += "</select>";
        var DateLayer = this.GetDateLayer();
        DateLayer.document.getElementById("SelectYearLayer").style.display = "";
        DateLayer.document.getElementById("SelectYearLayer").innerHTML = selectInnerHTML;
        DateLayer.document.getElementById("L_SelectYear").focus();
    },
    SelectMonthInnerHTML: function (strMonth) //月份的下拉框
    {
        if (strMonth.match(/\D/) != null) { alert("月份输入参数不是数字！"); return; }
        var m = (strMonth) ? strMonth : new Date().getMonth() + 1;
        var s = "<select name=\"L_SelectYear\" id=\"L_SelectMonth\" style='font-size: 12px' "
        s += "onblur='document.getElementById(\"SelectMonthLayer\").style.display=\"none\"' "
        s += "onchange='document.getElementById(\"SelectMonthLayer\").style.display=\"none\";"
        s += "parent." + this.NewName + ".L_TheMonth = this.value; parent." + this.NewName + ".SetDay(parent." + this.NewName + ".L_TheYear,parent." + this.NewName + ".L_TheMonth)'>\r\n";
        var selectInnerHTML = s;
        for (var i = 1; i < 13; i++) {
            if (i == m)
            { selectInnerHTML += "<option value='" + i + "' selected>" + i + "月" + "</option>\r\n"; }
            else { selectInnerHTML += "<option value='" + i + "'>" + i + "月" + "</option>\r\n"; }
        }
        selectInnerHTML += "</select>";
        var DateLayer = this.GetDateLayer();
        DateLayer.document.getElementById("SelectMonthLayer").style.display = "";
        DateLayer.document.getElementById("SelectMonthLayer").innerHTML = selectInnerHTML;
        DateLayer.document.getElementById("L_SelectMonth").focus();
    },
    DayClick: function (mm, dd)  //点击显示框选取日期，主输入函数*************
    {
        var yy = this.L_TheYear;
        //判断月份，并进行对应的处理
        if (mm < 1) { yy--; mm = 12 + mm; }
        else if (mm > 12) { yy++; mm = mm - 12; }
        if (mm < 10) { mm = "0" + mm; }
        if (this.ClickObject) {
            if (!dd) { return; }
            if (dd < 10) { dd = "0" + dd; }
            this.InputObject.value = yy + "-" + dd; //注：在这里你可以输出改成你想要的格式
            this.CloseLayer();
            if (typeof (this.CallBackFunction) == 'function') {
                this.CallBackFunction();
            }
        }
        else { this.CloseLayer(); alert("您所要输出的控件对象并不存在！"); }
    },

    SetDate: function () { //设置日期，参数1：点击对象，参数2：输入对象，参数3：回调函数
        if (arguments.length < 1) { alert("对不起！传入参数太少！"); return; }
        else if (arguments.length > 3) { alert("对不起！传入参数太多！"); return; }
        this.InputObject = (arguments.length == 1) ? arguments[0] : arguments[1];
        this.ClickObject = arguments[0];
        if (arguments.length == 3) {
            this.CallBackFunction = arguments[2];
        }
        else { this.CallBackFunction = null; }
        var reg = /^(\d+)-(\d{1,2})-(\d{1,2})$/;
        var r = null;
        if (this.InputObject.value) {
            r = (this.InputObject.value + '-01').match(reg);
        }
        if (r != null) {
            r[2] = r[2] - 1;
            var d = new Date(r[1], r[2], r[3]);
            if (d.getFullYear() == r[1] && d.getMonth() == r[2] && d.getDate() == r[3]) {
                this.InputDate = d; 	//保存外部传入的日期
            }
            else this.InputDate = "";
            this.L_TheYear = r[1];
            this.L_TheMonth = r[2] + 1;
        }
        else {
            this.L_TheYear = new Date().getFullYear();
            this.L_TheMonth = new Date().getMonth() + 1;
            this.InputDate = null;
        }
        this.CreateHTML();
        var top = this.GetY();
        var left = this.GetX();
        var DateLayer = document.getElementById("L_MonthLayer");
        DateLayer.style.top = top + this.ClickObject.clientHeight + 5 + "px";
        DateLayer.style.left = left + "px";
        DateLayer.style.display = "block";
        if (document.all) {
            this.GetDateLayer().document.getElementById("L_calendar").style.width = "113px";
            this.GetDateLayer().document.getElementById("L_calendar").style.height = "105px";
        }
        else {
            this.GetDateLayer().document.getElementById("L_calendar").style.width = "110px";
            this.GetDateLayer().document.getElementById("L_calendar").style.height = "105px";
            DateLayer.style.width = "158px";
            DateLayer.style.height = "250px";
        }
        //alert(DateLayer.style.display)
        this.SetDay(this.L_TheYear, this.L_TheMonth);
    },
    CloseLayer: function () {
        try {
            var DateLayer = document.getElementById("L_MonthLayer");
            if ((DateLayer.style.display == "" || DateLayer.style.display == "block") && arguments[0] != this.ClickObject && arguments[0] != this.InputObject) {
                DateLayer.style.display = "none";
            }
        }
        catch (e) { }
    }
}

document.writeln('<iframe id="L_MonthLayer" name="L_MonthLayer" frameborder="0" style="position:absolute;width:125px; height:115px;overflow:hidden;z-index:100099;display:none;backgorund-color:transparent;"></iframe>');
var MyCalendarMonth = new L_calendarMonth();
MyCalendarMonth.NewName = "MyCalendarMonth";
//document.onclick = function (e) {
//    e = window.event || e;
//    var srcElement = e.srcElement || e.target;
//    MyCalendarMonth.CloseLayer(srcElement);
//}
//日历控件end*************


/*
* 异步调用--公用方法,需要引用JQuery
* url:url地址
* postBody：参数体
* beforeSend：Send前执行函数。如没有内容，可以传入null
* CallbackName：回调函数
* Add=Masj,Date=20091207
*/
function AjaxPost(url, postBody, beforeSend, CallbackName) {
    $.ajax({
        type: "POST",
        url: url,
        data: postBody,
        beforeSend: beforeSend,
        success: CallbackName,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            // 通常 textStatus 和 errorThrown 之中
            // 只有一个会包含信息
            alert(XMLHttpRequest.responseText);
        }
    });
}

/*
* 异步调用--公用方法,需要引用JQuery
* url:url地址
* postBody：参数体
* beforeSend：Send前执行函数。如没有内容，可以传入null
* CallbackName：回调函数
* Add=Masj,Date=20091215
*/
function AjaxGet(url, postBody, beforeSend, CallbackName) {
    $.ajax({
        type: "GET",
        url: url,
        data: postBody,
        beforeSend: beforeSend,
        success: CallbackName,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            // 通常 textStatus 和 errorThrown 之中
            // 只有一个会包含信息
            alert(XMLHttpRequest.responseText);
        }
    });
}
/**
* @desc   escape字符串,escape不编码字符有69个：*，-，.，@，_，0-9，a-z，A-Z
* @param  字符串
* @return 返回string等对象
* @Add=Masj, Date: 2009-12-16
*/
function escapeStr(str) {
    return escape(str).replace(/\+/g, '%2B').replace(/\"/g, '%22').replace(/\'/g, '%27').replace(/\//g, '%2F');
}

/**
* @desc  JQuery扩展，将json字符串转换为对象，需要引用类库JQuery
* @param   json字符串
* @return 返回object,array,string等对象
* @Add=Masj, Date: 2009-12-07
*/
jQuery.extend(
 {
     evalJSON: function (strJson) {
         if ($.trim(strJson) == '')
             return '';
         else
             return eval("(" + strJson + ")");
     }
 });
/**
* @desc  JQuery扩展，将javascript数据类型转换为json字符串，需要引用类库JQuery
* @param 待转换对象,支持object,array,string,function,number,boolean,regexp
* @return 返回json字符串
* @Add=Masj, Date: 2009-12-07
*/
jQuery.extend(
{
    toJSONstring: function (object) {
        var type = typeof object;
        if ('object' == type) {
            if (Array == object.constructor)
                type = 'array';
            else if (RegExp == object.constructor)
                type = 'regexp';
            else
                type = 'object';
        }
        switch (type) {
            case 'undefined':
            case 'unknown':
                return;
                break;
            case 'function':
            case 'boolean':
            case 'regexp':
                return object.toString();
                break;
            case 'number':
                return isFinite(object) ? object.toString() : 'null';
                break;
            case 'string':
                return '"' + object.replace(/(\\|\")/g, "\\$1").replace(/\n|\r|\t/g,
                      function () {
                          var a = arguments[0];
                          return (a == '\n') ? '\\n' :
                                   (a == '\r') ? '\\r' :
                                   (a == '\t') ? '\\t' : ""
                      }) + '"';
                break;
            case 'object':
                if (object === null) return 'null';
                var results = [];
                for (var property in object) {
                    var value = jQuery.toJSONstring(object[property]);
                    if (typeof (value) != "undefined")
                        results.push(jQuery.toJSONstring(property) + ':' + value);
                }
                return '{' + results.join(',') + '}';
                break;
            case 'array':
                var results = [];
                for (var i = 0; i < object.length; i++) {
                    var value = jQuery.toJSONstring(object[i]);
                    if (typeof (value) != "undefined") results.push(value);
                }
                return '[' + results.join(',') + ']';
                break;
        }
    }
});

//绑定省、市、县 start*********************************
/*
*绑定省份New
*/
function BindProvinceNew(SelectID) {
    if (JSonData && JSonData.masterArea.length > 0) {
        Jselect($("#" + SelectID), {
            bindid: SelectID,
            hoverclass: 'selecthover',
            objects: JSonData.masterArea,
            onchange: ChangeProvince,
            content: "省/直辖市",
            defaultvalue: -1,
            valueName: "name"
        });
    }
}
/*
*切换省份New
*/
function ChangeProvince(pid) {
    if (pid && pid > 0) {
        BindCityNew(pid, 'divCity', true);
    }
    BindCountyNew(-1, -1, 'divCounty', true);
}
/*
*切换城市New
*/
function ChangeCity(cityid) {
    if (cityid && cityid > 0) {
        var pid = $("#txtProvince").val();
        if (pid && pid != "-1") {
            BindCountyNew(pid, cityid, 'divCounty', true);
        }
    }
    else {
        BindCountyNew(-1, -1, 'divCounty', true);
    }
}
/*
*绑定城市New
*/
function BindCityNew(provinceID, citySelectID, ismuliload) {
    if (provinceID && provinceID > 0) {
        for (var i = 0; i < JSonData.masterArea.length; i++) {
            if (JSonData.masterArea[i].id == provinceID) {
                if (JSonData.masterArea[i].subArea == 0) {
                    Jselect($("#" + citySelectID), {
                        bindid: citySelectID,
                        hoverclass: 'selecthover',
                        onchange: ChangeCity,
                        content: "城市",
                        defaultvalue: -1,
                        valueName: "name",
                        ismuliload: ismuliload
                    });
                }
                else {
                    Jselect($("#" + citySelectID), {
                        bindid: citySelectID,
                        hoverclass: 'selecthover',
                        objects: JSonData.masterArea[i].subArea,
                        onchange: ChangeCity,
                        content: "城市",
                        defaultvalue: -1,
                        valueName: "name",
                        ismuliload: ismuliload
                    });
                }
                return;
            }
        }
    }
    else if (provinceID && provinceID == -1) {
        Jselect($("#" + citySelectID), {
            bindid: citySelectID,
            hoverclass: 'selecthover',
            onchange: ChangeCity,
            content: "城市",
            defaultvalue: -1,
            valueName: "name",
            ismuliload: ismuliload
        });
    }
}
/*
*绑定区县New
*/
function BindCountyNew(provinceId, cityId, countySelectID, ismuliload) {
    if (provinceId && provinceId > 0 && cityId && cityId > 0) {
        for (var i = 0; i < JSonData.masterArea.length; i++) {
            if (JSonData.masterArea[i].id == provinceId) {
                var t1 = JSonData.masterArea[i];
                for (var j = 0; j < t1.subArea.length; j++) {
                    if (t1.subArea[j].id == cityId) {
                        var t2 = t1.subArea[j];
                        if (t2.subArea2.length == 0) {
                            Jselect($("#" + countySelectID), {
                                bindid: countySelectID,
                                hoverclass: 'selecthover',
                                content: "区/县",
                                defaultvalue: -1,
                                valueName: "name",
                                ismuliload: ismuliload
                            });
                        }
                        else {
                            Jselect($("#" + countySelectID), {
                                bindid: countySelectID,
                                hoverclass: 'selecthover',
                                objects: t2.subArea2,
                                content: "区/县",
                                defaultvalue: -1,
                                valueName: "name",
                                ismuliload: ismuliload
                            });
                        }
                        return;
                    }
                }

            }
        }
    }
    else if ((provinceId && provinceId == -1) || (cityId && cityId == -1)) {
        Jselect($("#" + countySelectID), {
            bindid: countySelectID,
            hoverclass: 'selecthover',
            content: "区/县",
            defaultvalue: -1,
            valueName: "name",
            ismuliload: ismuliload
        });
    }
}
/*
* 绑定省份，需要引用Area.js文件
* Area.js文件是生成的
* Add=Masj, Date: 2009-12-07 
*/
function BindProvince(SelectID) {
    if (JSonData && JSonData.masterArea.length > 0) {
        var masterObj = document.getElementById(SelectID);
        masterObj.options.length = 0;
        masterObj.options[0] = new Option("省/直辖市", -1);
        for (var i = 0; i < JSonData.masterArea.length; i++) {
            masterObj.options[masterObj.options.length] = new Option(JSonData.masterArea[i].name, JSonData.masterArea[i].id);
        }
    }
}

/*
* 绑定城市，需要引用Area.js文件
* Area.js文件是生成的
* 参数provinceSelectID为省份ID，citySelectID为城市ID
* Add=Masj, Date: 2009-12-07 
*/
function BindCity(provinceSelectID, citySelectID) {
    
    var masterObjid = document.getElementById(provinceSelectID).options[document.getElementById(provinceSelectID).selectedIndex].value;
    if (masterObjid && masterObjid > 0) {
        var subAreaObj = document.getElementById(citySelectID);
        subAreaObj.options.length = 0;
        subAreaObj.options[subAreaObj.options.length] = new Option("城市", -1);
        for (var i = 0; i < JSonData.masterArea.length; i++) {
            if (JSonData.masterArea[i].id == masterObjid) {
                for (var j = 0; j < JSonData.masterArea[i].subArea.length; j++) {
                    subAreaObj.options[subAreaObj.options.length] = new Option(JSonData.masterArea[i].subArea[j].name, JSonData.masterArea[i].subArea[j].id);
                }
            }
        }
    }
    else if (masterObjid && masterObjid == -1) {
        var subAreaObj = document.getElementById(citySelectID);
        subAreaObj.options.length = 0;
        subAreaObj.options[subAreaObj.options.length] = new Option("城市", -1);
    }
}

/*
* 绑定区县，需要引用Area2.js文件
* Area2.js文件是生成的
* 参数provinceSelectID为省份ID，citySelectID为城市ID, countyID为区县ID
*/
function BindCounty(provinceSelectID, citySelectID, countySelectID) {
    var provinceId = document.getElementById(provinceSelectID).options[document.getElementById(provinceSelectID).selectedIndex].value;
    var cityId = document.getElementById(citySelectID).options[document.getElementById(citySelectID).selectedIndex].value;
    if (provinceId && provinceId > 0 && cityId && cityId > 0) {
        var subAreaObj = document.getElementById(countySelectID);
        subAreaObj.options.length = 0;
        subAreaObj.options[subAreaObj.options.length] = new Option("区/县", -1);
        for (var i = 0; i < JSonData.masterArea.length; i++) {
            if (JSonData.masterArea[i].id == provinceId) {
                var t1 = JSonData.masterArea[i];
                for (var j = 0; j < t1.subArea.length; j++) {
                    if (t1.subArea[j].id == cityId) {
                        var t2 = t1.subArea[j];
                        for (var k = 0; k < t2.subArea2.length; k++) {
                            subAreaObj.options[subAreaObj.options.length] =
                                new Option(t2.subArea2[k].name, t2.subArea2[k].id);
                        }
                        return;
                    }
                }
            }
        }
    }
    else if ((provinceId && provinceId == -1) || (cityId && cityId == -1)) {
        var subAreaObj = document.getElementById(countySelectID);
        subAreaObj.options.length = 0;
        subAreaObj.options[subAreaObj.options.length] = new Option("区/县", -1);
    }
}
//绑定省、市、县 end*********************************

/*
* 绑定枚举列表，需要引用ShowEnum.js文件和类库JQuery
* ShowEnum.js文件是生成的
* 参数arrayObject为数组对象
* Add=Masj, Date: 2009-12-07 
*/
function BindArrayToSelect(arrayObject, selectID, str) {
    var selectObject = document.getElementById(selectID);
    selectObject.options.length = 0;
    if (str) {
        selectObject.options[0] = new Option(str, -1);
    }
    else {
        selectObject.options[0] = new Option("请选择", -1);
    }
    $.each(arrayObject, function (name, value) {
        selectObject.options[selectObject.options.length] = new Option(value[0], value[1]);
    });
}

//弹出伙伴代理品牌DIV层
function openFriendAgentBrandAjaxPopup(hidAgentBrandIDs, txtAgentBrandNames) {
    $.openPopupLayer({
        name: 'FriendAgentBrandAjaxPopup',
        url: "../BaseDataManager/SelectAgentBrand.aspx?page=1&AgentBrandIDs=" + $('#' + hidAgentBrandIDs).val(),
        beforeClose: function () {
            var brandids = $('#popupLayer_' + 'FriendAgentBrandAjaxPopup').data('brandids');
            var brandnames = $('#popupLayer_' + 'FriendAgentBrandAjaxPopup').data('brandnames');
            if (typeof (brandids) != "undefined") {
                //alert(brandids);
                $('#' + hidAgentBrandIDs).val(brandids);
                $('#' + txtAgentBrandNames).val(brandnames);
            }
        }
    });
}
//弹出伙伴上级单位DIV层
function openSelectSuperiorFriendAjaxPopup(hidIDs, txtNames, friendid) {
    $.openPopupLayer({
        name: 'SuperiorFriendAjaxPopup',
        parameters: { selffriendid: friendid },
        url: "/FriendManager/SelectSuperiorFriend.aspx?page=1",
        beforeClose: function () {
            var friendid = $('#popupLayer_' + 'SuperiorFriendAjaxPopup').data('friendid');
            var friendname = $('#popupLayer_' + 'SuperiorFriendAjaxPopup').data('friendname');
            if (typeof (friendid) != "undefined") {
                //alert(brandids);
                $('#' + hidIDs).val(friendid);
                $('#' + txtNames).val(friendname);
            }
        }
    });
}

//弹出伙伴DIV层:状态-1时为全部
function openSelectFriendAjaxPopup(hidIDs, txtNames, status) {
    $.openPopupLayer({
        name: 'FriendAjaxPopup',
        url: "/FriendManager/SelectFriend.aspx?search=yes&page=1&Status=" + status,
        beforeClose: function () {
            var friendid = $('#popupLayer_' + 'FriendAjaxPopup').data('friendid');
            var friendname = $('#popupLayer_' + 'FriendAjaxPopup').data('friendname');
            if (typeof (friendid) != "undefined") {
                $('#' + hidIDs).val(friendid);
                $('#' + txtNames).val(friendname);
            }
        }
    });
}

//获得客户浏览器类型
function GetBrowserName() {
    var Sys = {};
    var ua = navigator.userAgent.toLowerCase();
    var s;
    (s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :
        (s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :
        (s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :
        (s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :
        (s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;

    //以下进行测试
    if (Sys.ie) {
        //alert('IE: ' + Sys.ie);
        return 'IE';
    }
    else if (Sys.firefox) {
        //alert('Firefox: ' + Sys.firefox);
        return 'FF';
    }
    else
        return '';
    //        if (Sys.chrome) document.write('Chrome: ' + Sys.chrome);
    //        if (Sys.opera) document.write('Opera: ' + Sys.opera);
    //        if (Sys.safari) document.write('Safari: ' + Sys.safari);
}

//设置表格样式
function SetTableStyle(tableid) {
    //$('#'+tableid+' tr:even').addClass('color_hui');//设置列表行样式
    $('#' + tableid + ' tr').removeData('currentcolor');
    $('#' + tableid + ' tr').mouseover(function () {
        if (!($(this).data('currentcolor')))
            $(this).data('currentcolor', $(this).css('backgroundColor'));
        $(this).css('backgroundColor', '#e5edf1').css('fontWeight', '');
    }).mouseout(function () {
        $(this).css('backgroundColor', $(this).data('currentcolor')).css('fontWeight', '');
    });

}

//重置
function resetForm(id) {
    jQuery('#' + id).each(function () {
        this.reset();
    });
}
//关闭弹出层
/**
@name 弹出层的名字
@, isCancel 是否是取消之类的操作，默认为true
*/
function Close(name, effectiveAction) {
    $.closePopupLayer(name, effectiveAction);
}

/**
* @desc  返回字符串长度
* @param 字符串
* @return 返回字符串长度
* @Add=Masj, Date: 2009-12-11
*/
function GetStringRealLength(str) {
    var bytesCount = 0;
    for (var i = 0; i < str.length; i++) {
        var c = str.charAt(i);
        if (/^[\u0000-\u00ff]$/.test(c))   //匹配双字节
        {
            bytesCount += 1;
        }
        else {
            bytesCount += 2;
        }
    }
    return bytesCount;
}

/**
* @desc  选择厂商大品牌联动销售网络和主营品牌
* @param 字符串
* @return 返回字符串长度
* @Add=fangxc, Date: 2009-12-16
*/
function BindSaleNetwork(ddlProducerID, ddlSaleNetworkID, ddlBrandID, SaleNetworkID, BrandID) {
    var ddlProducer = document.getElementById(ddlProducerID);
    var ddlSaleNetwork = document.getElementById(ddlSaleNetworkID);
    var ddlBrand = document.getElementById(ddlBrandID);
    if (ddlProducer && ddlSaleNetwork) {
        ddlSaleNetwork.options.length = 0;
        ddlSaleNetwork.options[0] = new Option("请选择", -1);
        if (ddlBrand) {
            ddlBrand.options.length = 0;
            ddlBrand.options[0] = new Option("请选择", -1);
        }
        if (ddlProducer.value != "-1") {
            var pody = 'BindSaleNetwork=yes&producerID=' + ddlProducer.value;
            AjaxPost('/BaseDataManager/BaseDataBind.ashx', pody, null, function (data) {
                var jsonData = $.evalJSON(data);
                if ($.trim(jsonData.text) != "") {
                    var textList = jsonData.text.split('|');
                    var valueList = jsonData.value.split('|');
                    for (var i = 0; i < textList.length; i++) {
                        ddlSaleNetwork.options[i + 1] = new Option(textList[i], valueList[i]);
                    }
                    if (SaleNetworkID && SaleNetworkID != '-1') {
                        ddlSaleNetwork.value = SaleNetworkID;
                        if (ddlBrandID) {
                            BindBrand(ddlSaleNetworkID, ddlBrandID, BrandID)
                        }
                    }
                }
            });
        }
    }
}
/**
* @desc  选择销售网络联动主营品牌
* @param 字符串
* @return 返回字符串长度
* @Add=fangxc, Date: 2009-12-16
*/
function BindBrand(ddlSaleNetworkID, ddlBrandID, BrandID) {
    var ddlSaleNetwork = document.getElementById(ddlSaleNetworkID);
    var ddlBrand = document.getElementById(ddlBrandID);
    if (ddlSaleNetwork && ddlBrand) {

        ddlBrand.options.length = 0;
        ddlBrand.options[0] = new Option("请选择", -1);
        if (ddlSaleNetwork.value != "-1") {
            var pody = 'BindBrand=yes&snid=' + ddlSaleNetwork.value;
            AjaxPost('/BaseDataManager/BaseDataBind.ashx', pody, null, function (data) {
                var jsonData = $.evalJSON(data);
                if ($.trim(jsonData.text) != "") {
                    var textList = jsonData.text.split('|');
                    var valueList = jsonData.value.split('|');
                    for (var i = 0; i < textList.length; i++) {
                        ddlBrand.options[i + 1] = new Option(textList[i], valueList[i]);
                    }
                    if (BrandID && BrandID != '-1') {
                        ddlBrand.value = BrandID;
                    }
                }
            });
        }
    }
}


/*头部鼠标划过出现下拉层start*/
function addLoadEvent(func) {
    var oldonload = window.onload;
    if (typeof window.onload != 'function') {
        window.onload = func;
    } else {
        window.onload = function () {
            oldonload();
            func();
        }
    }
}
/*
*为元素增加class
* element:元素
* value：className
*/
function addClass(element, value) {
    if (!element.className) {
        element.className = value;
    } else {
        newClassName = element.className;
        newClassName += " ";
        newClassName += value;
        element.className = newClassName;
    }
}
/*
*为元素移除class
* element:元素
* value：className
*/
function removeClass(element, value) {
    var removedClass = element.className;
    var pattern = new RegExp("(^| )" + value + "( |$)");
    removedClass = removedClass.replace(pattern, "$1");
    removedClass = removedClass.replace(/ $/, "");
    element.className = removedClass;
    return true;
}

function bt_login_more(overID, boxID, add_Class) {
    if (!document.getElementById(overID)) return false;
    var btli = document.getElementById(overID);
    var btpop = document.getElementById(boxID);
    btli.onmouseover = function () {
        //jQuery('#topmenu').bgiframe({ top: '-5px', left: '178px', width: '200px', height: '500px' });
        addClass(btpop, add_Class);
    }
    btli.onmouseout = function () {
        removeClass(btpop, add_Class);
        //$(".bgiframe").remove();
    }
}
function all_login_box() {
    bt_login_more('goOther', 'goOtherContent', 'pop_block');
}
addLoadEvent(all_login_box);
/*头部鼠标划过出现下拉层end*/


/*点击更多显示，点击收起隐藏star*/
function showMore() {
    document.getElementById("more").style.display = "block";
}
function hideMore() {
    document.getElementById("more").style.display = "none";
}
/*点击更多显示，点击收起隐藏end*/


/*节选自jQueryString v2.0.2*/
(function ($) {
    $.unserialise = function (Data) {
        var Data = Data.split("&");
        var Serialised = new Array();
        $.each(Data, function () {
            var Properties = this.split("=");
            Serialised[Properties[0]] = Properties[1];
        });
        return Serialised;
    };
})(jQuery);


/*设置透明度，兼容IE和FF*/
; (function ($) {
    $.freeOpacity = {
        main: function (opacity) {
            this.each(function (i) {
                var _this = $(this);
                if ($.browser.msie) { _this.css('filter', 'alpha(opacity=' + opacity * 100 + ')'); }
                else { _this.css('opacity', opacity); this.style.Opacity = 0.5; }
            });
            return this;
        }
    }; $.fn.opacity = $.freeOpacity.main;
})(jQuery);

/*
* jqDnR - Minimalistic Drag'n'Resize for jQuery.
*
* Copyright (c) 2007 Brice Burgess <bhb@iceburg.net>, http://www.iceburg.net
* Licensed under the MIT License:
* http://www.opensource.org/licenses/mit-license.php
* 
* $Version: 2007.08.19 +r2
* Drag and Resize, 我觉得比jQuery.ui中写的还要好...
* 我将opacity注释掉，因为IE中会有BUG；添加了setCapture，解决IE中在浏览器外不响应mouseup事件的问题。
*/
(function ($) {
    $.fn.jqDrag = function (h) { return i(this, h, 'd'); };
    $.fn.jqResize = function (h) { return i(this, h, 'r'); };
    $.jqDnR = { dnr: {}, e: 0,
        drag: function (v) {
            if (M.k == 'd') E.css({ left: M.X + v.pageX - M.pX, top: M.Y + v.pageY - M.pY });
            else E.css({ width: Math.max(v.pageX - M.pX + M.W, 0), height: Math.max(v.pageY - M.pY + M.H, 0) });
            return false;
        },
        stop: function (h) {/*E.opacity(M.o);*/
            if (h[0].releaseCapture) { h[0].releaseCapture(); } //取消捕获范围
            else if (window.captureEvents) { window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP); }
            $(document).unbind('mousemove', J.drag).unbind('mouseup', J.stop);
        }
    };
    var J = $.jqDnR, M = J.dnr, E = J.e,
i = function (e, h, k) {
    return e.each(function () {
        h = (h) ? $(h, e) : e;
        h.bind('mouseover', function () { $(this).css('cursor', 'move'); })
 .bind('mouseout', function () { $(this).css('cursor', 'auto'); });
        h.bind('mousedown', { e: e, k: k }, function (v) {
            var d = v.data, p = {}; E = d.e;
            // attempt utilization of dimensions plugin to fix IE issues

            if (E.css('position') != 'relative') { try { E.position(p); } catch (e) { } }
            M = { X: p.left || f('left') || 0, Y: p.top || f('top') || 0, W: f('width') || E[0].scrollWidth || 0, H: f('height') || E[0].scrollHeight || 0, pX: v.pageX, pY: v.pageY, k: d.k, o: E.css('opacity') };
            /*E.opacity(0.8);*/
            //设置捕获范围
            if (h[0].setCapture) { h[0].setCapture(); }
            else if (window.captureEvents) { window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP); }
            $(document).mousemove($.jqDnR.drag).mouseup(function () {
                $.jqDnR.stop(h);
            });
            return false;
        });
    });
},
f = function (k) { return parseInt(E.css(k)) || false; };
})(jQuery);


//载入时的动画. eleId为容器ID
function LoadingAnimation(eleId, height, paddingTop) {
    var h = '840px';
    if (height) {
        h = height;
    }
    var pT = '15px';
    if (paddingTop) {
        pT = paddingTop;
    }
    //jQuery('#' + eleId).html('<div style="width:100%; height:' + h + ';padding-top:15px;"><div class="blue-loading" style="width:50%;float:left;background-position:right;"></div><div style="float:left;padding:20px 0px 0px 10px;">正在加载中...</div></div>');
    jQuery('#' + eleId).html('<div style="width:100%; height:' + h + ';padding-top:' + paddingTop + ';"><div style="float:left;padding-top:1px"><img style="margin-left:10px;vertical-align: middle;" alt="正在加载" src="/images/loading.gif"></div><div style="float:left;padding-left:3px">正在加载中.....</div></div>');
}

//保存要操作的id
function setDelHidValue(hidDelAll) {
    var hidDelAll = document.getElementById(hidDelAll);
    var inputs = document.documentElement.getElementsByTagName("input");

    hidDelAll.value = "";
    for (var i = 0; i < inputs.length; i++) {
        if (inputs[i].type == "checkbox" && inputs[i].name == 'checkBoxDelAll') {
            if (inputs[i].checked) {

                hidDelAll.value += inputs[i].value + ':';
            }
        }
    }
}
/*
*删除验证
*/
function validateDel() {
    var b = false;
    var ar = document.documentElement.getElementsByTagName("input");
    for (var ii = 0; ii < ar.length; ii++) {
        if (ar[ii].type == "checkbox") {
            if (ar[ii].checked) {
                b = true;
            }
        }
    }
    if (b) {
        return confirm("确定要删除吗?");
        //return true;
    }
    else {
        alert("请至少选择一项!");
        return false;
    }
}
/*
*编辑验证
*/
function validateEdit() {
    var b = false;
    var ar = document.documentElement.getElementsByTagName("input");
    for (var ii = 0; ii < ar.length; ii++) {
        if (ar[ii].type == "checkbox") {
            if (ar[ii].checked) {
                b = true;
            }
        }
    }
    if (b) {
        return true;
    }
    else {
        alert("请至少选择一项!");
        return false;
    }
}

/**
* @desc  根据AC库字典类型绑定下拉列表
* @Add=fangxc, Date: 2010-04-08
* type：类型
* ddlControlID：绑定控件
*/
function BindACDictInfo(type, ddlControlID) {
    var ddlControl = document.getElementById(ddlControlID);
    if (ddlControl) {
        ddlControl.options.length = 0;
        ddlControl.options[0] = new Option("请选择", -1);
        var pody = { Action: "GetDictInfo", type: type };

        $.post('/BaseDataManager/ACDictInfoManager.ashx?r=' + Math.random(), pody, function (data) {
            var jsonData = $.evalJSON(data);
            if ($.trim(jsonData.text) != "") {
                var textList = jsonData.text.split('|');
                var valueList = jsonData.value.split('|');
                for (var i = 0; i < textList.length; i++) {
                    ddlControl.options[i + 1] = new Option(textList[i], valueList[i]);
                }
            }
        });
    }
}
/**
* @desc  根据AC库字典类型绑定下拉列表
* @Add=fangxc, Date: 2010-04-08
* type：类型
* ddlControlID：绑定控件
* ddlControlValue：绑定控件默认值
*/
function BindACDictInfo(type, ddlControlID, ddlControlValue) {
    var ddlControl = document.getElementById(ddlControlID);
    if (ddlControl) {
        ddlControl.options.length = 0;
        ddlControl.options[0] = new Option("请选择", -1);
        var pody = { Action: "GetDictInfo", type: type };

        $.post('/BaseDataManager/ACDictInfoManager.ashx?r=' + Math.random(), pody, function (data) {
            var jsonData = $.evalJSON(data);
            if ($.trim(jsonData.text) != "") {
                var textList = jsonData.text.split('|');
                var valueList = jsonData.value.split('|');
                for (var i = 0; i < textList.length; i++) {
                    ddlControl.options[i + 1] = new Option(textList[i], valueList[i]);
                }
                $('#' + ddlControlID).val(ddlControlValue);
            }
        });
    }
}



//function OnlyDouble() {
//    if (((event.keyCode >= 48) && (event.keyCode <= 57) || event.keyCode == 46))
//    { event.returnValue = true; }
//    else { event.returnValue = false; }
//}


//function SetMoney(obj) {
//    if (isNaN(ParseMoney(obj.value))) {
//        obj.value = "0";
//    }
//    var useMoney = ParseMoney(obj.value);
//    obj.value = useMoney;
//    return useMoney;
//}

//关闭窗口，IE下也不提示
function closeWindow() {
    window.opener = null;
    window.open("", "_self");
    window.close();
}
//关闭页面，并刷新上一个页面
function closePageExecOpenerSearch(controlid) {
    
    var id = "btnSearch";
    if (controlid != undefined) {
        id = controlid;
    }
    try {
        if (window.opener.document.getElementById(id) != null) {
            window.opener.document.getElementById(id).click();
        }
        else {
            window.opener.location.href = window.opener.location.href;
        }
        window.opener = null; window.open('', '_self'); window.close();
    }
    catch (ex) {
        window.opener = null; window.open('', '_self'); window.close();
    }
}

/*
*绑定品牌
*/
function BindJsonBrand(SelectID) {
    if (JsonBrand && JsonBrand.Brands.length > 0) {
        var masterObj = document.getElementById(SelectID);
        masterObj.options.length = 0;
        masterObj.options[0] = new Option("品牌", -1);
        for (var i = 0; i < JsonBrand.Brands.length; i++) {
            masterObj.options[masterObj.options.length] = new Option(JsonBrand.Brands[i].Name, JsonBrand.Brands[i].Brandid);
        }
    }
}

/*
*绑定车型
*/
function BindJsonSerial(brandSelectID, serialSelectID) {
    var masterObjid = document.getElementById(brandSelectID).options[document.getElementById(brandSelectID).selectedIndex].value;
    if (masterObjid && masterObjid > 0) {
        var SerialsObj = document.getElementById(serialSelectID);
        SerialsObj.options.length = 0;
        SerialsObj.options[SerialsObj.options.length] = new Option("车型", -1);
        for (var i = 0; i < JsonBrand.Brands.length; i++) {
            if (JsonBrand.Brands[i].Brandid == masterObjid) {
                for (var j = 0; j < JsonBrand.Brands[i].Serials.length; j++) {
                    SerialsObj.options[SerialsObj.options.length] = new Option(JsonBrand.Brands[i].Serials[j].Name, JsonBrand.Brands[i].Serials[j].Serialid);
                }
            }
        }
    }
    else if (masterObjid && masterObjid == -1) {
        var SerialsObj = document.getElementById(serialSelectID);
        SerialsObj.options.length = 0;
        SerialsObj.options[SerialsObj.options.length] = new Option("车型", -1);
    }
}


/*
* 绑定
* 
* 
*/
function AutoBindText(obj, data, fun) {

    $(obj).autocomplete(data,
    {
        autoFill: false,
        isContaintValue: true,
        valueName: "UserID",
        scroll: false,
        formatItem: function (row, i, max) {
            return "<span style='width:60px;float:left;'>" + row.TrueName + "</span> &nbsp;&nbsp;<font style='margin-left:30px'>[" + row.DepartName + "]</font>";
        },
        formatMatch: function (row, i, max) {
            return row.TrueName;
        },
        formatResult: function (row) {
            return row.TrueName;
        }
    }).result(function (event, data, format) { fun(obj, data); });
}

/*
* 绑定人员
* inputs：绑定控件名
* paras:参数
*/
var UserDatas = [];
function AutoBindUser(inputs, paras) {

    $.ajax({ url: "/BaseDataManager/UserManager.ashx", type: 'post', dataType: 'text',
        data: paras,
        success: function (data) {
            if (data) {
            
                var users = JSON.parse(data);
                if (typeof (inputs) == "string") {
                    $("#" + inputs).autocomplete(users, {
                        autoFill: false,
                        isContaintValue: true,
                        valueName: "UserID",
                        scroll: false,
                        formatItem: function (row, i, max) {
                            return "<span style='width:60px;float:left;'>" + row.TrueName + "</span> &nbsp;&nbsp;<font style='margin-left:30px'>[" + row.DepartName + "]</font>";
                        },
                        formatMatch: function (row, i, max) {
                            return row.TrueName;
                        },
                        formatResult: function (row) {
                            return row.TrueName;
                        }
                    });
                }
                else {
                    for (i = 0; i < inputs.length; i++) {
                        $("#" + inputs[i]).autocomplete(users, {
                            autoFill: false,
                            isContaintValue: true,
                            valueName: "UserID",
                            scroll: false,
                            formatItem: function (row, i, max) {
                                return "<span style='width:60px;float:left;'>" + row.TrueName + "</span> &nbsp;&nbsp;<font style='margin-left:30px'>[" + row.DepartName + "]</font>";
                            },
                            formatMatch: function (row, i, max) {
                                return row.TrueName;
                            },
                            formatResult: function (row) {
                                return row.TrueName;
                            }
                        });
                    }
                }

            }
        }
    });

}
/*
* 绑定区域
* inputs：绑定控件名
* paras:参数
*/
function AutoBindArea(inputs, paras) {
    $.ajax({
        url: "/BaseDataManager/AreaManager.ashx",
        type: 'post',
        dataType: 'text',
        data: paras,
        success: function (data) {
            if (data) {
                var areaName = JSON.parse(data);
                if (typeof (inputs) == "string") {
                    $("#" + inputs).autocomplete(areaName, {
                        autoFill: false,
                        isContaintValue: true,
                        valueName: "AreaID",
                        scroll: false,
                        formatItem: function (row, i, max) {
                            return "<span style='width:60px;float:left;'>" + row.AreaName + "</span> ";
                        },
                        formatMatch: function (row, i, max) {
                            return row.AreaName;
                        },
                        formatResult: function (row) {
                            return row.AreaName;
                        }
                    });
                }
                else {
                    for (i = 0; i < inputs.length; i++) {
                        $("#" + inputs[i]).autocomplete(areaName, {
                            autoFill: false,
                            isContaintValue: true,
                            valueName: "AreaID",
                            scroll: false,
                            formatItem: function (row, i, max) {
                                return "<span style='width:60px;float:left;'>" + row.AreaName + "</span>";
                            },
                            formatMatch: function (row, i, max) {
                                return row.AreaName;
                            },
                            formatResult: function (row) {
                                return row.AreaName;
                            }
                        });
                    }
                }
            }
        }
    });

}
/*
* 绑定城市
* inputs：绑定控件名
* paras:参数
*/
function AutoBindCity(inputs, paras) {
    $.ajax({
        url: "/BaseDataManager/CityManager.ashx",
        type: 'post',
        dataType: 'text',
        data: paras,
        success: function (data) {
            if (data) {
                var cityName = JSON.parse(data);
                if (typeof (inputs) == "string") {
                    $("#" + inputs).autocomplete(cityName, {
                        autoFill: false,
                        isContaintValue: true,
                        valueName: "CityID",
                        scroll: false,
                        formatItem: function (row, i, max) {
                            return "<span style='width:60px;float:left;'>" + row.CityName + "</span> ";
                        },
                        formatMatch: function (row, i, max) {
                            return row.CityName;
                        },
                        formatResult: function (row) {
                            return row.CityName;
                        }
                    });
                }
                else {
                    for (i = 0; i < inputs.length; i++) {
                        $("#" + inputs[i]).autocomplete(cityName, {
                            autoFill: false,
                            isContaintValue: true,
                            valueName: "CityID",
                            scroll: false,
                            formatItem: function (row, i, max) {
                                return "<span style='width:60px;float:left;'>" + row.CityName + "</span>";
                            },
                            formatMatch: function (row, i, max) {
                                return row.CityName;
                            },
                            formatResult: function (row) {
                                return row.CityName;
                            }
                        });
                    }
                }
            }
        }
    });

}
/*
* 显示错误信息
* id：
* isShow:
* str：
* style:
*/
function ShowErrorMessage(id, isShow, str, style) {
    var parent = $("#" + id).parent();
    if (parent.find(".errorred").length > 0 && !isShow) {
        //        $("#" + id).removeClass("red");
        parent.find(".errorred").remove();
    }
    else if (isShow) {
        parent.find(".errorred").remove();
        var strShow = '<SPAN class="errorred"';
        if (style) {
            strShow += ' style="' + style + '" ';
        }
        strShow += '>' + str + '</SPAN>';
        parent.append(strShow);
        //        $("#" + id).addClass("red");
        //        $("#" + id).focus();
    }
}


Date.prototype.addMonths = function (months) {
    var year = this.getFullYear();
    var month = this.getMonth();
    var day = this.getDate();
    var monthNew = month + months;
    if (monthNew >= 0) {
        month = monthNew % 12;
        year += (monthNew / 12);
    } else {
        month = 12 + monthNew % 12;
        year += (monthNew - 11) / 12;
    }
    var arrDaysInMonth = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
    var daysInMonth = arrDaysInMonth[month];
    if (month == 2 && ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0))) {
        daysInMonth = 29;
    }
    if (day > daysInMonth) {
        day = daysInMonth;
    }
    return new Date(year, month, day);
}
Date.prototype.addDays = function (days) {
    return new Date(this.getTime() + days * 24 * 3600 * 1000);
}

/*选择人员
* selectusername 填写用户名控件  
* selectuserid 填写userid控件 
* selectdepartid 填写用户所在部门ID控件
* defaultdepartid 默认只显示指定部门ID
*/
function openSelectUserAjaxPopupCommon(selectusername, selectuserid, selectdepartid, defaultdepartid, method) {
    var popupName = "CustAssignUserAjaxPopup";

    var UserIDS = $('#' + selectuserid).val();
    var par = {
        "UserID": UserIDS,
        "max": "1",
        "action": "listall"
    };
    if (defaultdepartid != null) {
        par.departid = defaultdepartid;
    }

    $.openPopupLayer({
        name: popupName,
        parameters: par,
        url: "/BaseDataManager/SelectUser.aspx",
        success: function () {
            if (defaultdepartid != null) {
                //选择部门的下拉列表，赋当前默认值
                $("select[id$='dllAssingUserDepart']").val(defaultdepartid);
            }
        },
        beforeClose: function (e) {
            if (e) {
                if ($('#popupLayer_' + popupName).data("UserID") == "") {
                    $('#popupLayer_' + popupName).data("UserID", "0");
                }
                $("input[id$='" + selectuserid + "']").val($('#popupLayer_' + popupName).data("UserID"));
                $("input[id$='" + selectusername + "']").val($('#popupLayer_' + popupName).data("UserName"));

                if (selectdepartid != null) {
                    $("input[id$='" + selectdepartid + "']").val($('#popupLayer_' + popupName).data("DepartID"));
                }

                if (method != null && method != undefined) {
                    method(e);
                }
            }
        }
    });
}
/*
* 获取数值
* str：
* len:
*/
function Getdecimal(str, len) {
    if (isNaN(str)) {
        return 0;
    }
    if (len < 0) {
        alert(str);
    }
    str = str + '';
    var index = (str + '').indexOf(".");
    if (index == -1) {
        if (len !== 0) {
            str = str + ".";
            for (var i = 0; i < len; i++) {
                str = str + "0";
            }
        }
        else {
        }
    }
    else {
        var length = str.length;
        if (length - index < len + 2) {
            for (var j = 0; j < (len + 1 - length + index); j++) {
                str = str + "0";
            }
        }
        else {
            if (str.substr(index + len + 1, 1) > 4) {
                var additional = 1;
                for (var k = 0; k < len; k++) {
                    additional = additional * 10;
                }
                str = (Number(str.substr(0, index + len + 1)) * additional + 1) / additional;
                str = str + "";
                index = str.indexOf(".");
                if (index == -1) {
                    if (len != 0) {
                        str = str + ".";
                        for (i = 0; i < len; i++) {
                            str = str + "0";
                        }
                    }
                }
                else {
                    length = str.length;
                    if (length - index < len + 2) {
                        for (j = 0; j < (len + 1 - length + index); j++) {
                            str = str + "0";
                        }
                    }
                    else {
                        str = str.substr(0, index + len + 1);
                    }
                }

            }
            else {
                str = str.substr(0, index + len + 1);
            }
        }
    }

    return TrimChar(str, '.');
}
/**
* 将字符串形式金额转成数值形式,取小数点后2位
* @str:千分位格式化金额，如111,111.11或字符串金额111111.11
* @return;数值111111.11
*/
function getNumber(str) {
    if (str === undefined || str === "") {
        str = 0;
    }
    else {
        str = str.toString().replace(/,/g, '');
        if (isNaN(str))
            str = 0;
    }
    return Math.round(parseFloat(str) * 100) / 100;
}
/**
* 将数值形式转换为字符串形式,浮点数四舍五入，取小数点后2位，如果不足2位则补0,
* @x：数值111111.11
* @return:字符串：111111.11
*/
function changeTwoDecimal_f(x) {
    var f_x = parseFloat(x);
    if (isNaN(f_x)) {
        alert('function:changeTwoDecimal->parameter error');
        return false;
    }
    var f_x = Math.round(x * 100) / 100;
    var s_x = f_x.toString();
    var pos_decimal = s_x.indexOf('.');
    if (pos_decimal < 0) {
        pos_decimal = s_x.length;
        s_x += '.';
    }
    while (s_x.length <= pos_decimal + 2) {
        s_x += '0';
    }
    return s_x;
}
/**
* 去除千分位
*@param{Object}num
*/
function delcommafy(num) {
    if (trim(num + "") == "") {
        return "";
    }
    num = num.replace(/,/gi, '');
    return num;
}
/**
* 将数值四舍五入(保留1位小数)后格式化成金额形式
* @param num 数值(Number或者String)
* @return 金额格式的字符串,如'1,234,567.4'
* @type String
*/
function formatCurrencyTenThou(num) {
    num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 10 + 0.50000000001);
    cents = num % 10;
    num = Math.floor(num / 10).toString();
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' +
    num.substring(num.length - (4 * i + 3));
    return (((sign) ? '' : '-') + num + '.' + cents);
}
/**
* 将数值四舍五入(保留2位小数)后格式化成千分位金额形式
*
* @param num 数值(Number或者String)
* @return 千分位金额格式的字符串,如'1,234,567.45'
* @type String
*/
function formatCurrency(num) {
    if (num == null) { return 0.00 }
    num = num.toString().replace(/\$|\,/g, '');
    if (isNaN(num))
        num = "0";
    sign = (num == (num = Math.abs(num)));
    num = Math.floor(num * 100 + 0.50000000001);
    cents = num % 100;
    num = Math.floor(num / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
        num = num.substring(0, num.length - (4 * i + 3)) + ',' +
    num.substring(num.length - (4 * i + 3));
    return (((sign) ? '' : '-') + num + '.' + cents);
}

/**
整数录入限制
* 实时动态强制更改用户录入
* arg1 inputObject
**/
function IntFormat(obj) {
    var temp = obj.value;
    var pos = getCursortPosition(obj);
    temp = temp.replace(/[^0-9]*/g, "");
    var regStrs = [
        ['^0(\\d+)$', '$1'] //禁止录入整数部分两位以上，但首位为0
    ];
    for (i = 0; i < regStrs.length; i++) {
        var reg = new RegExp(regStrs[i][0]);
        temp = temp.replace(reg, regStrs[i][1]);
    }
    if (temp != obj.value) {
        obj.value = temp;
        setCaretToPos(obj, pos - 1);
    }
}

/**
价格录入限制
* 实时动态强制更改用户录入
* arg1 inputObject
* by zhaoyk
**/
function AmountFormat(obj) {
    var temp = obj.value;
    var pos = getCursortPosition(obj);
    if (!isNum(temp)) {
        temp = temp.replace(/[^0-9.]*/g, "");
    }
    var regStrs = [
        ['^0(\\d+)$', '$1'], //禁止录入整数部分两位以上，但首位为0
        ['[^\\-\\d\\.]+$', ''], //禁止录入任何非数字和点
        ['\\.(\\d?)\\.+', '.$1'], //禁止录入两个以上的点
        ['^(\\d+\\.\\d{2}).+', '$1'] //禁止录入小数点后两位以上
    ];
    for (i = 0; i < regStrs.length; i++) {
        var reg = new RegExp(regStrs[i][0]);
        temp = temp.replace(reg, regStrs[i][1]);
    }
    if (temp != obj.value) {
        obj.value = temp;
        setCaretToPos(obj, pos - 1);
    }
}

/**
编号录入限制
* 实时动态强制更改用户录入
* arg1 inputObject
* by jinfj
**/
function CodeFormat(obj) {
    var temp = obj.value;
    var pos = getCursortPosition(obj);
    temp = temp.replace(/[^0-9_A-Za-z]*/g, "");
    var regStrs = [
        ['[^\\w]+$', ''] //禁止录入任何非字母数字和下划线
    ];
    for (var i = 0; i < regStrs.length; i++) {
        var reg = new RegExp(regStrs[i][0]);
        temp = temp.replace(reg, regStrs[i][1]);
    }
    if (temp != obj.value) {
        obj.value = temp;
        setCaretToPos(obj, pos - 1);
    }
}

/**
名称录入限制
* 实时动态强制更改用户录入
* arg1 inputObject
* by jinfj
**/
function NameFormat(obj) {
    var temp = obj.value;
    var pos = getCursortPosition(obj);
    temp = temp.replace(/[\'\"]*/g, "");
    var regStrs = [
        ['[\\\'\\\"]+$', ''] //禁止录入半角引号
    ];
    for (var i = 0; i < regStrs.length; i++) {
        var reg = new RegExp(regStrs[i][0]);
        temp = temp.replace(reg, regStrs[i][1]);
    }
    if (temp != obj.value) {
        obj.value = temp;
        setCaretToPos(obj, pos - 1);
    }
}

/**
价格录入限制（可以录入负数）
* 实时动态强制更改用户录入
* arg1 inputObject
* by jinfj
**/
function AmountFormatCanMinus(obj) {
    var temp = obj.value;
    var pos = getCursortPosition(obj);
    if (!isNum(temp)) {
        if (temp.substr(0, 1) == "-") {
            var repTemp = temp.substr(1, temp.length);
            if (repTemp.substr(0, 1) == ".") {
                repTemp = repTemp.substr(1, repTemp.length);
            }
            temp = "-" + repTemp.replace(/[^0-9.]*/g, "");
        } else {
            var repTemp2 = temp;
            if (temp.substr(0, 1) == ".") {
                repTemp2 = repTemp2.substr(1, repTemp2.length);
            }
            temp = repTemp2.replace(/[^0-9.]*/g, "");
        }
    }
    var regStrs = [
        ['^\\-?0(\\d+)$', '$1'], //禁止录入整数部分两位以上，但首位为0
        ['[^\\-\\d\\.]+$', ''], //禁止录入任何非数字和点
        ['\\-?\\.(\\d?)\\.+', '.$1'], //禁止录入两个以上的点
        ['^(\\-?\\d+\\.\\d{2}).+', '$1'] //禁止录入小数点后两位以上
    ];
    for (i = 0; i < regStrs.length; i++) {
        var reg = new RegExp(regStrs[i][0]);
        temp = temp.replace(reg, regStrs[i][1]);
    }
    if (temp != obj.value) {
        obj.value = temp;
        setCaretToPos(obj, pos - 1);
    }
}

function setSelectionRange(input, selectionStart, selectionEnd) {
    // 非IE Support
    if (input.setSelectionRange) {
        input.focus();
        input.setSelectionRange(selectionStart, selectionEnd);
    }
    // IE Support
    else if (input.createTextRange) {
        var range = input.createTextRange();
        range.collapse(true);
        range.moveEnd('character', selectionEnd);
        range.moveStart('character', selectionStart);
        range.select();
    }
}

function setCaretToPos(input, pos) {
    setSelectionRange(input, pos, pos);
}

function getCursortPosition(ctrl) {
    var CaretPos = 0;
    // IE Support
    if (document.selection) {
        ctrl.focus();
        var Sel = document.selection.createRange();
        Sel.moveStart('character', -ctrl.value.length);
        CaretPos = Sel.text.length;
    }
    // Firefox support
    else if (ctrl.selectionStart || ctrl.selectionStart == '0')
        CaretPos = ctrl.selectionStart;
    return (CaretPos);
}

function FormatFocus(obj) {
    var temp = obj.value;
    var pos = getCursortPosition(obj);
    if (pos > 0) {
        temp = temp.substring(0, pos);
        if (temp.indexOf(',') != -1) {
            var array = temp.split(',');
            pos = pos - (array.length - 1);
        }
    }
    obj.value = delcommafy(obj.value);
    setCaretToPos(obj, pos);
}

/**
价格录入限制
* 录入完成后，输入模式失去焦点后对录入进行判断并强制更改，并对小数点进行0补全
* arg1 inputObject
* parseFloat('10');
* by zhaoyk
**/
function OverFormat(obj) {
    var v = obj.value;
    while (v.toString().indexOf(",") != -1) {
        v = v.toString().replace(",", "");
    }
    if (v === '') {
        v = '0.00';
    } else if (v === '0') {
        v = '0.00';
    } else if (v === '0.') {
        v = '0.00';
    } else if (/^0+\d+\.?\d*.*$/.test(v)) {
        v = v.replace(/^0+(\d+\.?\d*).*$/, '$1');
        v = inp.getRightPriceFormat(v).val;
    } else if (/^0\.\d$/.test(v)) {
        v = v + '0';
    } else if (!/^\d+\.\d{2}$/.test(v)) {
        if (/^\d+\.\d{2}.+/.test(v)) {
            v = v.replace(/^(\d+\.\d{2}).*$/, '$1');
        } else if (/^\d+$/.test(v)) {
            v = v + '.00';
        } else if (/^\d+\.$/.test(v)) {
            v = v + '00';
        } else if (/^\d+\.\d$/.test(v)) {
            v = v + '0';
        } else if (/^[^\d]+\d+\.?\d*$/.test(v)) {
            v = v.replace(/^[^\d]+(\d+\.?\d*)$/, '$1');
        } else if (/\d+/.test(v)) {
            v = v.replace(/^[^\d]*(\d+\.?\d*).*$/, '$1');
            ty = false;
        } else if (/^0+\d+\.?\d*$/.test(v)) {
            v = v.replace(/^0+(\d+\.?\d*)$/, '$1');
            ty = false;
        } else {
            v = '0.00';
        }
    }
    obj.value = formatCurrency(v);
}

function FixWidth(selectObj) {
    var newSelectObj = document.createElement("select");
    newSelectObj = selectObj.cloneNode(true);
    newSelectObj.selectedIndex = selectObj.selectedIndex;
    newSelectObj.id = "newSelectObj";

    var e = selectObj;
    var absTop = e.offsetTop;
    var absLeft = e.offsetLeft;
    while (e = e.offsetParent) {
        absTop += e.offsetTop;
        absLeft += e.offsetLeft;
    }
    with (newSelectObj.style) {
        position = "absolute";
        top = absTop + "px";
        left = absLeft + "px";
        width = "auto";
    }

    var rollback = function () { RollbackWidth(selectObj, newSelectObj); };
    if (window.addEventListener) {
        newSelectObj.addEventListener("blur", rollback, false);
        newSelectObj.addEventListener("change", rollback, false);
    }
    else {
        newSelectObj.attachEvent("onblur", rollback);
        newSelectObj.attachEvent("onchange", rollback);
    }

    selectObj.style.visibility = "hidden";
    document.body.appendChild(newSelectObj);

    var newDiv = document.createElement("div");
    with (newDiv.style) {
        position = "absolute";
        top = (absTop - 10) + "px";
        left = (absLeft - 10) + "px";
        width = newSelectObj.offsetWidth + 20;
        height = newSelectObj.offsetHeight + 20;;
        background = "transparent";
        //background = "green";
    }
    document.body.appendChild(newDiv);
    newSelectObj.focus();
    var enterSel = "false";
    var enter = function () { enterSel = enterSelect(); };
    newSelectObj.onmouseover = enter;

    var leavDiv = "false";
    var leave = function () { leavDiv = leaveNewDiv(selectObj, newSelectObj, newDiv, enterSel); };
    newDiv.onmouseleave = leave;
}

function RollbackWidth(selectObj, newSelectObj) {
    selectObj.selectedIndex = newSelectObj.selectedIndex;
    selectObj.style.visibility = "visible";
    if (document.getElementById("newSelectObj") != null) {
        document.body.removeChild(newSelectObj);
    }
}

function removeNewDiv(newDiv) {
    document.body.removeChild(newDiv);
}

function enterSelect() {
    return "true";
}

function leaveNewDiv(selectObj, newSelectObj, newDiv, enterSel) {
    if (enterSel == "true") {
        RollbackWidth(selectObj, newSelectObj);
        removeNewDiv(newDiv);
    }
}

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name] !== undefined) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

