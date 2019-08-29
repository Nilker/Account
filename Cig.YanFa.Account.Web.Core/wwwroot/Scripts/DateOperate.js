/**
* 获取本周、本季度、本月、上月的开始日期、结束日期
*/

//给时间添加天  
function AddDay(strInterval, Number, dtTmp) {
    switch (strInterval) {
        case 's': return new Date(Date.parse(dtTmp) + (1000 * Number));
        case 'n': return new Date(Date.parse(dtTmp) + (60000 * Number));
        case 'h': return new Date(Date.parse(dtTmp) + (3600000 * Number));
        case 'd': return new Date(Date.parse(dtTmp) + (86400000 * Number));
        case 'w': return new Date(Date.parse(dtTmp) + ((86400000 * 7) * Number));
        case 'q': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number * 3, dtTmp.getDate());
        case 'm': return new Date(dtTmp.getFullYear(), (dtTmp.getMonth()) + Number, dtTmp.getDate());
        case 'y': return new Date((dtTmp.getFullYear() + Number), dtTmp.getMonth(), dtTmp.getDate());
    }
}

//格式化日期：yyyy-MM-dd   
function formatDate(date) {
    var myyear = date.getFullYear();
    var mymonth = date.getMonth() + 1;
    var myweekday = date.getDate();

    if (mymonth < 10) {
        mymonth = "0" + mymonth;
    }
    if (myweekday < 10) {
        myweekday = "0" + myweekday;
    }
    return (myyear + "." + mymonth + "." + myweekday);
}

//获得某月的天数   
function getMonthDays(myYear, myMonth) {
    var monthStartDate = new Date(myYear, myMonth, 1);
    var monthEndDate = new Date(myYear, myMonth + 1, 1);
    var days = (monthEndDate - monthStartDate) / (1000 * 60 * 60 * 24);
    return days;
}


//获得本周的开始日期   
function getWeekStartDate(nowYear, nowMonth, nowDay, nowDayOfWeek) {
    var weekStartDate = new Date(nowYear, nowMonth, nowDay - nowDayOfWeek);
    return formatDate(weekStartDate);
}

//获得本周的结束日期   
function getWeekEndDate(nowYear, nowMonth, nowDay, nowDayOfWeek) {
    var weekEndDate = new Date(nowYear, nowMonth, nowDay + (6 - nowDayOfWeek));
    return formatDate(weekEndDate);
}

//获取上周的开始时间
function getLastWeekStartDate(currentDay) {
    var dayArr = currentDay.split(".");
    var lastWeekDate = new Date(dayArr[0], dayArr[1], dayArr[2]);
    lastWeekDate.setMonth(lastWeekDate.getMonth() - 1);
    lastWeekDate = AddDay('d', -7, lastWeekDate);
    var lastDay = lastWeekDate.getDate();
    var lastDayOfWeek = lastWeekDate.getDay();
    var lastYear = lastWeekDate.getFullYear();
    var lastMonth = lastWeekDate.getMonth();

    var lastWeekStartDate = new Date(lastYear, lastMonth, lastDay - lastDayOfWeek);
    return formatDate(lastWeekStartDate);
}

//获取上周的结束时间
function getLastWeekEndDate(currentDay) {
    var dayArr = currentDay.split(".");
    var lastWeekDate = new Date(dayArr[0], dayArr[1], dayArr[2]);
    lastWeekDate.setMonth(lastWeekDate.getMonth() - 1);
    lastWeekDate = AddDay('d', -8, lastWeekDate);
    var lastDay = lastWeekDate.getDate();
    var lastDayOfWeek = lastWeekDate.getDay();
    var lastYear = lastWeekDate.getFullYear();
    var lastMonth = lastWeekDate.getMonth();

    var lastWeekEndDate = new Date(lastYear, lastMonth, lastDay + (6 - lastDayOfWeek));
    return formatDate(lastWeekEndDate);
}

//获取下周的开始时间
function getNextWeekStartDate(currentDay) {
    var dayArr = currentDay.split(".");
    var nextWeekDate = new Date(dayArr[0], dayArr[1], dayArr[2]);
    nextWeekDate.setMonth(nextWeekDate.getMonth() - 1);
    var nextWeekDate = AddDay('d', 7, nextWeekDate);
    var nextDay = nextWeekDate.getDate();
    var nextDayOfWeek = nextWeekDate.getDay();
    var nextYear = nextWeekDate.getFullYear();
    var nextMonth = nextWeekDate.getMonth();

    var nextWeekStartDate = new Date(nextYear, nextMonth, nextDay - nextDayOfWeek);
    return formatDate(nextWeekStartDate);
}

//获取下周的结束时间
function getNextWeekEndDate(currentDay) {
    var dayArr = currentDay.split(".");
    var nextWeekDate = new Date(dayArr[0], dayArr[1], dayArr[2]);
    nextWeekDate.setMonth(nextWeekDate.getMonth() - 1);
    var nextWeekDate = AddDay('d', 7, nextWeekDate);
    var nextDay = nextWeekDate.getDate();
    var nextDayOfWeek = nextWeekDate.getDay();
    var nextYear = nextWeekDate.getFullYear();
    var nextMonth = nextWeekDate.getMonth();

    var nextWeekEndDate = new Date(nextYear, nextMonth, nextDay + (6 - nextDayOfWeek));
    return formatDate(nextWeekEndDate);
}

//获得本月的开始日期   
function getMonthStartDate(nowYear, nowMonth) {
    var monthStartDate = new Date(nowYear, nowMonth, 1);
    return formatDate(monthStartDate);
}

//获得本月的结束日期   
function getMonthEndDate(nowYear, nowMonth) {
    var monthEndDate = new Date(nowYear, nowMonth, getMonthDays(nowYear,nowMonth));
    return formatDate(monthEndDate);
}

//获得上月开始时间
function getLastMonthStartDate(currentDay) {
    var dayArr = currentDay.split(".");
    var lastMonthDate = new Date(dayArr[0], dayArr[1], dayArr[2]);
    lastMonthDate.setDate(1);
    lastMonthDate.setMonth(lastMonthDate.getMonth() - 2);
    var lastYear = lastMonthDate.getFullYear();
    var lastMonth = lastMonthDate.getMonth();

    var lastMonthStartDate = new Date(lastYear, lastMonth, 1);
    return formatDate(lastMonthStartDate);
}

//获得上月结束时间
function getLastMonthEndDate(currentDay) {
    var dayArr = currentDay.split(".");
    var lastMonthDate = new Date(dayArr[0], dayArr[1], dayArr[2]);
    var nowYear = lastMonthDate.getYear();             //当前年   
    nowYear += (nowYear < 2000) ? 1900 : 0;
    lastMonthDate.setDate(1);
    lastMonthDate.setMonth(lastMonthDate.getMonth() - 2);
    var lastYear = lastMonthDate.getFullYear();
    var lastMonth = lastMonthDate.getMonth();

    var lastMonthEndDate = new Date(lastYear, lastMonth, getMonthDays(nowYear, lastMonth));
    return formatDate(lastMonthEndDate);
}

//获取下月开始时间
function getNextMonthStartDate(currentDay) {
    var dayArr = currentDay.split(".");
    var nextMonthDay = new Date(dayArr[0], dayArr[1], dayArr[2]);
    nextMonthDay.setDate(1);
    nextMonthDay.setMonth(nextMonthDay.getMonth());
    var nextYear = nextMonthDay.getFullYear();
    var nextMonth = nextMonthDay.getMonth();
    var nextMonthStartDate = new Date(nextYear, nextMonth, 1);
    return formatDate(nextMonthStartDate);
}

//获取下个月结束时间
function getNextMonthEndDate(currentDay) {
    var dayArr = currentDay.split(".");
    var nextMonthDay = new Date(dayArr[0], dayArr[1], dayArr[2]);
    nextMonthDay.setDate(1);
    nextMonthDay.setMonth(nextMonthDay.getMonth());
    var nextYear = nextMonthDay.getFullYear();
    var nextMonth = nextMonthDay.getMonth();
    var nextMonthEndDate = new Date(nextYear, nextMonth, getMonthDays(nextYear, nextMonth));
    return formatDate(nextMonthEndDate);
}


function getNextDay(date) {
    date = date.replace('.', '/').replace('.', '/');    
    var resultDay = new Date(date).getTime();    
    resultDay = resultDay + 24 * 60 * 60 * 1000;
    return formatDate(new Date(resultDay));
}

function getYestoday(date) {
    date = date.replace('.', '/').replace('.', '/');
    var resultDay = new Date(date).getTime();
    resultDay = resultDay - 24 * 60 * 60 * 1000;

    return formatDate(new Date(resultDay));
}