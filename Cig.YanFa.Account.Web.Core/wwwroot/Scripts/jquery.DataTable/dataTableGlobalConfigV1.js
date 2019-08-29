//
// Pipelining function for DataTables. To be used to the `ajax` option of DataTables
//
$.fn.dataTable.pipeline = function (opts) {
    // Configuration options
    var conf = $.extend({
        pages: 10,     // number of pages to cache
        url: '',      // script url
        data: null,   // function or object with parameters to send to the server
        // matching how `ajax.data` works in DataTables
        method: 'POST' // Ajax HTTP method
    }, opts);

    return function (request, drawCallback, settings) {
        var ajax = true;
        //add by lhl 2019年5月27日17:19:07
        request.currentPage = request.start / request.length + 1;
        request.pageSize = request.length;


        if (ajax) {
            // Provide the same `data` options as DataTables.
            if (typeof conf.data === 'function') {
                // As a function it is executed with the data object as an arg
                // for manipulation. If an object is returned, it is used as the
                // data object to submit
                var d = conf.data(request);
                if (d) {
                    $.extend(request, d);
                }
            }
            else if ($.isPlainObject(conf.data)) {
                // As an object, the data given extends the default
                $.extend(request, conf.data);
            }

            settings.jqXHR = $.ajax({
                "type": conf.method,
                "url": conf.url,
                "data": request,
                "dataType": "json",
                "cache": false,
                "success": function (json) {
                    drawCallback(json);
                }
            });
        }
        else {
            json = $.extend(true, {}, cacheLastJson);
            json.draw = request.draw; // Update the echo for each response
            json.data.splice(0, requestStart - cacheLower);
            json.data.splice(requestLength, json.data.length);

            drawCallback(json);
        }
    }
};

// Register an API method that will empty the pipelined data, forcing an Ajax
// fetch on the next draw (i.e. `table.clearPipeline().draw()`)
$.fn.dataTable.Api.register('clearPipeline()', function () {
    return this.iterator('table', function (settings) {
        settings.clearCache = true;
    });
});


////
//// DataTables initialisation
////
//$(document).ready(function () {
//    $('#example').DataTable({
//        "processing": true,
//        "serverSide": true,
//        "ajax": $.fn.dataTable.pipeline({
//            url: 'scripts/server_processing.php',
//            pages: 5 // number of pages to cache
//        })
//    });
//});


// Disable search and ordering by default
$.extend($.fn.dataTable.defaults, {
    "searching": false,
    "pagingType": "full_numbers",//full_numbers  / input
    "processing": true,
    "serverSide": true,
    "paging": true,//分页默认true
    "lengthMenu": [[10, 25, 50], [10, 25, 50]],//每页多少条
    "ordering": false,
    "info": true, //分页信息
    //"order": [[3, "desc"]],//第一列：0  列号
    //"columnDefs": [{
    //    targets: [0],
    //    orderData: [0, 1],
    //    //"render": function (data, type, row) { //渲染
    //    //    return data + ' (' + row[3] + ')';
    //    //},
    //}, {
    //    targets: [1],
    //    orderData: [1, 0],
    //    visible: true, //是否可见
    //    searchable: false  //是否可以搜索
    //}],
    "dom": '<"top"i>rt<"bottom"flp><"clear">',  //位置布局
    stateSave: false, //择保存表的状态（其分页位置，排序状态等）（默认为2小时）  保存方法使用HTML5 localStorage和sessionStorageAPI来有效存储数据。请注意，这意味着内置状态保存选项不适用于IE6 / 7，因为这些浏览器不支持这些API
    "language": {
        "paginate": {
            "first": "首页",
            "last": "末页",
            "previous": "上一页",
            "next": "下一页"
        },
        "lengthMenu": "每页 _MENU_ 条",
        "zeroRecords": "暂无数据",
        "emptyTable":"暂无满足条件的数据",
        "info": "当前页 _PAGE_ of _PAGES_ 共 _TOTAL_ 条",
        "infoEmpty": "暂无满足条件的数据",
        "infoFiltered": "(从 _MAX_ total 过滤)"
    },
    "drawCallback": function (settings ) {
        appendSkipPage(this,settings.sTableId);
    },
    // "scrollY": "200px", //高度200
    // "scrollCollapse": true, //有垂直滚动条
    // "scrollX": true,//水平

    ////行创建 回调函数，createdCell 要在columnDefs中使用
    //"createdRow": function (row, data, index) {
    //    if (data[5].replace(/[\$,]/g, '') * 1 > 150000) {
    //        $('td', row).eq(5).addClass('highlight');
    //    }
    //}
});


function appendSkipPage(table,id) {
    //var table = $("#example").dataTable();
    var template =
        $(//"<li class='paginate_button active'>" +
            //"   <div class=' input-group' style='float:right ; margin-left:3px;padding-top: 0.25em; '>" +
            "       <span class='paginate_button active input-group-addon' style='padding:0px 2px;background-color:#fff;font-size: 12px;'>跳转页</span>" +
            "       <input type='text' class=' form-control' style='text-align:center;padding: 8px 2px;height:6px;width:30px;' />" +
            "       <span class='paginate_button input-group-addon active' style='padding:0px 2px;'><a href='javascript:void(0)'> Go </a></span>" //+
            //"   </div>" //+
            //"</li>"
            );

    template.find("a").click(function () {
        //var pn = template.find("input").val();
        var pn = $(template[2]).val();
        if (pn > 0) {
            --pn;
        } else {
            pn = 0;
        }
        table.fnPageChange(pn);
    });

    var tem = "#" + id + "_paginate";
    $(tem).append(template);
}