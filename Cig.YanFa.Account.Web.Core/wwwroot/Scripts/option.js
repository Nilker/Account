/***************************************** 
���÷���Ϊ�� 
Jselect($("#inputid"),{ 
bindid:'bindid', 
hoverclass:'hoverclass', 
optionsbind:objects;},
onchange: function () { }
}); 
inputidΪ������Ҫ�󶨵��ı���id 
bindidΪ�������/�ջ�������Ŀؼ�id 
hoverclassΪ����Ƶ�ѡ��ʱ����ʽ 
objectsΪjson���ݼ���
hqhtmlΪ�󶨵����� 
******************************************/
(function ($) {
    $.showselect = {
        init: function (o, options) {
            var defaults = {
                bindid: null, //�¼�����bindid�� 
                hoverclass: null, //����Ƶ�ѡ��ʱ��ʽ���� 
                objects: { Obejects: [] }, //�����������
                onchange: function () { }, //ѡ���¼��ص�����
                content: "--��ѡ��--", //Ĭ�ϵ�һ������
                defaultvalue: "",
                idName: "id",
                valueName: "value",     //Ĭ�ϵ�Text
                ismuliload: false,  //�Ƿ��μ���
                IsHasDefaultValue: true //�Ƿ���Ĭ��ֵ
            }

            var options = $.extend(defaults, options);
            if (options.objects != null) {//��������ݲ�Ϊ�� 
                this._setbind(o, options);
            }
            if (options.bindid != null) {
                if (!options.ismuliload) {
                    this._showcontrol(o, options);
                }
            }
        },

        _showcontrol: function (o, options) {//������������ʾ 
            $("#" + options.bindid).click(function (event) {
                $(".selecttanchu").each(function (i, n) {
                    if ($(this).attr("id") != $(o).next().attr("id"))
                    { $(this).hide(); }
                })
                event.stopPropagation();
                if ($(o).next().is(":hidden")) {
                    var offset = $(o).offset();
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
                    $(o).next().css({ left: lw });
                    $(o).next().show();
                }
                else {
                    $(o).next().hide();
                }
            })
            $().click(function (event) { $(o).next().hide(); })
        },

        _setbind: function (o, options) {//������ 
            $(o).find("input[type='text']").val(options.content);
            $(o).find("input[type='hidden']").val(options.defaultvalue);
            if ($(o).next().attr("class") == "selecttanchu") {
                $(o).next().remove();
            }
            var optionshtml = "<div class='selecttanchu' id='tanchu" + $.trim(options.bindid) + "'>";
            var id = options.idName;
            var value = options.valueName;
            var jsonObjects = options.objects;
            optionshtml += "<table style='width:100%;color:#333;' cellpadding=\"0\" cellspacing=\"0\" >";
            if (options.IsHasDefaultValue) {
                optionshtml += "<tr class='trSelect' id='" + options.defaultvalue + "'><td align='left'>" + options.content + "</td></tr>";
            }
            for (var i = 0; i < jsonObjects.length; i++) {
                optionshtml += "<tr class='trSelect' id='" + jsonObjects[i][id] + "'><td align='left'>" + jsonObjects[i][value] + "</td></tr>";
            }
            optionshtml += "</table>";
            optionshtml += "</div>";
            $(o).after(optionshtml);
            var offset = $(o).offset();
            var w = $(o).width();
            var lw = offset.left;
            $(o).next().css({ top: offset.top + $(o).height() - 1, left: offset.left - 8 });
            $(o).next().css("min-width", w - 2);

            if (options.hoverclass != null) {
                $(o).next().find("tr").hover(function () { $(this).addClass(options.hoverclass); },
function () { $(this).removeClass(options.hoverclass); });
            }

            $(o).next().find("tr").click(function () {
                var $ckflag = $(this).find("td");
                $(o).find("input[type='text']").val($ckflag.text());
                $(o).find("input[type='hidden']").val(this.id);
                $(o).next().hide();
                options.onchange(this.id);
            });

        }
    }
    Jselect = function (o, json) {
        $.showselect.init(o, json);
    };
})(jQuery); 
