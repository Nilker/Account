
//inputs:输入控件ID
//sourceUrl:列表数据源url
//valueName:返回值(隐藏值）
//showItems:列表显示项
//matchItem:列表匹配项
//resultItem:返回值界面显示(文本框显示的内容)
//where:过滤值
//status:状态
function AutoBind(inputs, sourceUrl, valueName, showItems, matchItem, resultItem,where,status) {
        var data;
        if (where != null && where != "") {
            if (status != null && status != "") {
                data = {
                    Where: where,
                    Status: status
                };
            } else {
                data = { Where: where };
            }
        } else {
            if (status != null && status != "") {
                data = {Status: status};
            } else {
                data = "";
            }
        }
    $.ajax({
        url: sourceUrl,
        type: 'post',
        dataType: 'text',
        data: data,
        success: function (data) {
            if (data) {
                var datas = JSON.parse(data);
                if (typeof (inputs) == "string") {
                    
                    $("#" + inputs).autocomplete(datas, {
                        autoFill: false,
                        isContaintValue: true,
                        valueName: valueName,
                        scroll: false,
                        formatItem: function (row, i, max) {
                            var retItem = "";
                            var strArr = showItems.split(",");
                            for (var j = 0; j < strArr.length; j++) {
                                retItem += "<span style='width:200px;float:left;'>" + row[strArr[j]] + " </span>";
                            }
                            return retItem;
                        },
                        formatMatch: function (row, i, max) {
                            var retMatch = "";
                            var strArr = matchItem.split(",");
                            for (var j = 0; j < strArr.length; j++) {
                                retMatch += row[strArr[j]];
                            }
                            return retMatch;
                        },
                        formatResult: function (row) {
                            
                            var retVal = "";
                            retVal = row[resultItem];
                            return retVal;
                        }
                    });
                }
            }
        }
    });
}


function BindProductCat(suffix, inputs, sourceUrl, valueName, showItems, matchItem, resultItem) {
     
    $.ajax({
        url: sourceUrl,
        type: 'post',
        dataType: 'text',
        data: {},
        success: function (data) {
           
            if (data) {
                debugger;
                var datas = JSON.parse(data);
                if (typeof (inputs) == "string") {
                    ///////////////////////////////////清除上次的
                    $("#" + inputs).unautocomplete();
                    $("#" + inputs).parent().find("input[name='btnSelect']").remove();
                    //////////////////////////////////
                    $("#" + inputs).autocomplete(datas.data, {
                        autoFill: false,
                        isContaintValue: true,
                        valueName: valueName,
                        scroll: false,
                        formatItem: function (row, i, max) {
                            var retItem = "";
                            var strArr = showItems.split(",");
                            for (var j = 0; j < strArr.length; j++) {
                                retItem += "<span style='width:200px;float:left;'>" + row[strArr[j]] + " </span>";
                            }
                            return retItem;
                        },
                        formatMatch: function (row, i, max) {
                            var retMatch = "";
                            var strArr = matchItem.split(",");
                            for (var j = 0; j < strArr.length; j++) {
                                retMatch += row[strArr[j]];
                            }
                            return retMatch;
                        },
                        formatResult: function (row) {

                            var retVal = "";
                            retVal = row[resultItem];
                            return retVal;
                        }
                    });
                    if ($('#productCat1' + suffix).find("option:selected").text() == "代理业务") {
                        $('#li_MediaProperty' + suffix).show();
                    } else {
                        $('#li_MediaProperty' + suffix).hide();
                    }
                }
            }
        }
    });
}





function BindBank(suffix, inputs, sourceUrl, valueName, showItems, matchItem, resultItem) {
    $.ajax({
        url: sourceUrl,
        type: 'post',
        dataType: 'text',
        data: {},
        success: function (data) {
            if (data) {
                
                var datas = JSON.parse(data);
                if (typeof (inputs) == "string") {
                    ///////////////////////////////////清除上次的
                    $("#" + inputs).unautocomplete();
                    $("#" + inputs).parent().find("input[name='btnSelect']").remove();
                    //////////////////////////////////
                    $("#" + inputs).autocomplete(datas, {
                        autoFill: false,
                        isContaintValue: true,
                        valueName: valueName,
                        scroll: false,
                        formatItem: function (row, i, max) {
                            var retItem = "";
                            var strArr = showItems.split(",");
                            for (var j = 0; j < strArr.length; j++) {
                                retItem += "<span style='width:200px;float:left;'>" + row[strArr[j]] + " </span>";
                            }
                            return retItem;
                        },
                        formatMatch: function (row, i, max) {
                            var retMatch = "";
                            var strArr = matchItem.split(",");
                            for (var j = 0; j < strArr.length; j++) {
                                retMatch += row[strArr[j]];
                            }
                            return retMatch;
                        },
                        formatResult: function (row) {

                            var retVal = "";
                            retVal = row[resultItem];
                            return retVal;
                        }
                    });
                    
                }
            }
        }
    });
}