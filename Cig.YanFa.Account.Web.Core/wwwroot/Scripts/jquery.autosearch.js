(function ($) {
    $.atuosearchselect = {
        init: function (o, options) {
            var defaults = {
                nextId: null, //级联ID
                onchange: function () { } //选择事件回调函数
            }
            var options = $.extend(defaults, options);
            if (o != undefined && o != null) {//如果绑定数据不为空 
                this._setbind(o, options);
                this._showcontrol(o, options);
                this._search(o, options);
            }
            $("#" + o).hide();
        },
        _search: function (o, options) {//模糊匹配选项
            $("#head" + o).find("input[type='text']").keyup(function () {
                var d = $(this).val();
                $("#tanchu" + o).show();
                $("#tanchu" + o).find("tr").each(function () {
                    var str = $(this).html();
                    if (str.indexOf(d) > -1) {
                        $(this).show();
                    }
                    else {
                        $(this).hide();
                    }
                });
            });
        },
        _showcontrol: function (o, options) {//控制下拉框显示 
            $("#head" + o).click(function (event) {
                event.stopPropagation();
                if ($("#tanchu" + o).is(":hidden")) {
                    $("#tanchu" + o).show();
                    $("#tanchu" + o + " table tr").show();
                    var offset = $("#head" + o).offset();
                    var lw = offset.left;
                    if (document.documentElement.clientWidth < document.documentElement.offsetWidth - 4) {
                        if ($.browser.msie) {
                            if ($.browser.version < 7.0) {
                                lw = offset.left - 8;
                            }
                        }
                        else {
                            lw = offset.left - 8;
                        }
                    }
                    $("#tanchu" + o).css({ left: offset.left });
                }
                else {
                    $("#tanchu" + o).hide();
                }
            })
            $().click(function (event) {
                if ($("#head" + o).find("input[type='text']").val() != $("#" + o + " option:selected").text()) {
                    var visibletr = $("#tanchu" + o).find("tr:visible");
                    if (visibletr.length > 0) {
                        var defaultText = visibletr.eq(0);
                        var $ckflag = $(defaultText).find("td");
                        var $id = $(defaultText).attr("id");
                        $("#" + o + " option[value='" + $id + "']").attr("selected", "selected");
                        $("#head" + o).find("input[type='text']").val($ckflag.text());
                        $("#" + o).change();
                        if (options.nextId != null) {
                            var deafultVal = $("#" + options.nextId + " option").eq(0).text();
                            $("#head" + options.nextId).find("input[type='text']").val(deafultVal);
                            var jsonNextObjects = $("#" + options.nextId);
                            $("#tanchu" + options.nextId).find("tr").remove();
                            jsonNextObjects.find("option").each(function (item, index) {
                                $("#tanchu" + options.nextId).find("table").append("<tr class='trSelect' id='" + $(this).val() + "'><td align='left'>" + $(this).text() + "</td></tr>");
                            });
                            $("#tanchu" + options.nextId).find("tr").click(function (event) {
                                event.stopPropagation();
                                var ckflag = $(this).find("td");
                                $("#" + options.nextId + " option[value='" + this.id + "']").attr("selected", "selected");
                                $("#tanchu" + options.nextId).hide();
                                $("#head" + options.nextId).find("input[type='text']").val(ckflag.text());
                                $("#" + options.nextId).change();
                            });

                        }

                        $("#tanchu" + o).hide();

                    }
                }
                else {
                    $("#tanchu" + o).hide();
                }
            })
        },
        _setbind: function (o, options) {//绑定数据 
            var selectedText = "";
            if ($("#" + o + " option:selected").length > 0) {
                selectedText = $("#" + o + " option:selected").eq(0).text();
            }
            else {
                selectedText = $("#" + o + " option").eq(0).text();
            }
            var selectshtml = "<div class='fd-left'  id='head" + o + "' style='position:realtion'><input  type='text' class='suggstionbox' /> <a><img  alt='' border='0' style='cursor: pointer;margin:0 0 0 0 ;top:0px' src='../../images/select.png' /></a></div>";
            var optionshtml = "<div class='selecttanchu'  style='max-height:320px;'   id='tanchu" + o + "'>";
            var jsonObjects = $("#" + o);
            optionshtml += "<table style='width:100%;color:#333;' cellpadding=\"0\" cellspacing=\"0\" >";
            jsonObjects.find("option").each(function (item, index) {
                optionshtml += "<tr class='trSelect' id='" + $(this).val() + "'><td align='left'>" + $(this).text() + "</td></tr>";
            });
            optionshtml += "</table>";
            optionshtml += "</div>";
            $("#" + o).before(selectshtml);
            document.writeln(optionshtml);
            $("#head" + o).find("input[type='text']").val(selectedText);
            $("#tanchu" + o).find("tr").hover(function () { $(this).addClass("selecthover"); },
function () { $(this).removeClass("selecthover"); });
            var w = $("#" + o).width();

            var offset = $("#head" + o).offset();
            $("#tanchu" + o).css({ top: offset.top + $("#head" + o).height() - 2 });

            $("#head" + o).find("input[type='text']").css("width", w - 18);
            $("#tanchu" + o).css("width", w + 3);
            $("#tanchu" + o).find("tr").click(function (event) {
                event.stopPropagation();
                var ckflag = $(this).find("td");
                $("#" + o + " option[value='" + this.id + "']").attr("selected", "selected");
                $("#tanchu" + o).hide();
                $("#head" + o).find("input[type='text']").val(ckflag.text());
                $("#" + o).change();
                if (options.nextId != null) {
                    var deafultVal = $("#" + options.nextId + " option").eq(0).text();
                    $("#head" + options.nextId).find("input[type='text']").val(deafultVal);
                    var jsonNextObjects = $("#" + options.nextId);
                    $("#tanchu" + options.nextId).find("tr").remove();
                    jsonNextObjects.find("option").each(function (item, index) {
                        $("#tanchu" + options.nextId).find("table").append("<tr class='trSelect' id='" + $(this).val() + "'><td align='left'>" + $(this).text() + "</td></tr>");
                    });
                    $("#tanchu" + options.nextId).find("tr").click(function (event) {
                        event.stopPropagation();
                        var ckflag = $(this).find("td");
                        $("#" + options.nextId + " option[value='" + this.id + "']").attr("selected", "selected");
                        $("#tanchu" + options.nextId).hide();
                        $("#head" + options.nextId).find("input[type='text']").val(ckflag.text());
                        $("#" + options.nextId).change();
                    });

                }
            });
        }
    }
    AutoSearch = function (o, options) {
        $.atuosearchselect.init(o, options);
    };
})(jQuery);


