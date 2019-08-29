

/*
当文件选择对话框关闭消失时，如果选择的文件成功加入上传队列，
那么针对每个成功加入的文件都会触发一次该事件（N个文件成功加入队列，就触发N 次此事件）。
*/
function fileQueued(file) {

}

/*
当选择文件对话框关闭消失时，如果选择的文件加入到上传队列中失败，
那么针对每个出错的文件都会触发一次该事件(此事件和fileQueued事件是二选一触发，文件添加到队列只有两种可能，成功和失败)。
*/
function fileQueueError(file, errorCode, message) {
    try {
        switch (errorCode) {
            case SWFUpload.QUEUE_ERROR.QUEUE_LIMIT_EXCEEDED: //-100
                message = "超过文件队列数量限制";
                break;
            case SWFUpload.QUEUE_ERROR.FILE_EXCEEDS_SIZE_LIMIT: //-110
                message = "超过了上传大小限制";
                break;
            case SWFUpload.QUEUE_ERROR.ZERO_BYTE_FILE: //-120
                message = "文件为零字节";
                break;
            case SWFUpload.QUEUE_ERROR.INVALID_FILETYPE: //-130
                message = "设置之外的无效文件类型";
                break;
            default:
                break;
        }
        if (message !== "") { $.jAlert(message); }
    } catch (ex) {
        this.debug(ex);
    }

}

/*
当选择文件对话框关闭，并且所有选择文件已经处理完成（加入上传队列成功或者失败）时，此事件被触发，
number of files selected是选择的文件数目，number of files queued是此次选择的文件中成功加入队列的文件数目。
如果你希望文件在选择以后自动上传，那么在这个事件中调用this.startUpload() 是一个不错的选择。如果需要更严格的判断，
在调用上传之前，可以对入队文件的个数做一个判断，如果大于0，那么可以开始上传。
*/
function fileDialogComplete(numFilesSelected, numFilesQueued) {
    try {
        if (numFilesQueued > 0) {
            //this.startUpload();
            var fileName = this.getFile().name;
            jQuery('#UploadFileName').val(fileName);
            //this.getFile().name = encodeURIComponent(this.getFile().name);            
        }
    } catch (ex) {
        this.debug(ex);
    }
}

/*
该事件由flash定时触发，提供三个参数分别访问上传文件对象、已上传的字节数，总共的字节数。
因此可以在这个事件中来定时更新页面中的UI元素，以达到及时显示上传进度的效果。
*/
function uploadProgress(file, bytesLoaded) {
    try {
        var percent = Math.ceil((bytesLoaded / file.size) * 100);
        if (percent > 100) {
            percent = 100;
        }
        if (percent == 100) {
            jQuery('#SpanMsg').show().text('正在处理数据……');
        }
        else {
            jQuery('#SpanMsg').show().text('正在上传数据，' + percent + '%');
        }
    } catch (ex) {
        this.debug(ex);
    }
}

/*
无论什么时候，只要上传被终止或者没有成功完成，那么该事件都将被触发。
此时文件上传的周期还没有结束，不能在这里开始下一个文件的上传。
error code参数表示了当前错误的类型，更具体的错误类型可以参见SWFUpload.UPLOAD_ERROR中的定义。
Message参数表示的是错误的描述。File参数表示的是上传失败的文件对象。
*/
/*
SWFUpload.UPLOAD_ERROR = { 
HTTP_ERROR : -200, 
MISSING_UPLOAD_URL : -210, 
IO_ERROR : -220, 
SECURITY_ERROR : -230, 
UPLOAD_LIMIT_EXCEEDED : -240, 
UPLOAD_FAILED : -250, 
SPECIFIED_FILE_ID_NOT_FOUND : -260, 
FILE_VALIDATION_FAILED : -270, 
FILE_CANCELLED : -280, 
UPLOAD_STOPPED : -290 
}; 
*/
function uploadError(file, errorCode, message) {
    try {
        jQuery('#FileProgressBar').text(message);
    } catch (ex) {
        this.debug(ex);
    }
}

/*
当文件上传的处理已经完成（这里的完成只是指向目标处理程序发送了Files信息，只管发，不管是否成功接收），
并且服务端返回了200的HTTP状态时，触发此事件。
serverData是服务端处理程序返回的数据。
此时文件上传的周期还没有结束，不能在这里开始下一个文件的上传。
在window平台下，那么服务端的处理程序在处理完文件存储以后，必须返回一个非空值，否则此事件不会被触发，随后的 uploadComplete事件也无法执行。
*/
function uploadSuccess(file, serverData) {
    try {
        //alert('uploadSuccess: ' + serverData);
        var jsonResponse = JSON.parse(serverData); //先转换为json对象，否则会出错
        if (jsonResponse.success) {
            jQuery('#SpanMsg').show().text('已成功导入数据');
            jQuery('#TableMsg').hide();
        }
        else {
            jQuery('#SpanMsg').show().text('上传成功，但校验未通过.');
            if (jsonResponse.message) { alert(unescape(jsonResponse.message)); }
            else {
                var result = JSON.parse(unescape(jsonResponse.result));
                var tableMsg = jQuery('#TableMsg');
                tableMsg.find('tr.tableBody').remove();
                tableMsg.show();
                jQuery.each(result, function(i, v) {
                    tableMsg.append('<tr class="tableBody"><td>' + v.ExceptionType + '</td><td>' + v.CustName + '</td><td>' + v.Saleman + '</td><td>' + v.Infomation + '</td></tr>');
                });
            }
        }
    } catch (ex) {
        this.debug(ex);
    }
}

/*
当上传队列中的一个文件完成了一个上传周期，无论是成功(uoloadSuccess触发)还是失败(uploadError触发)，此事件都会被触发，
这也标志着一个文件的上传完成，可以进行下一个文件的上传了。
如果要进行多文件自动上传，那么在这个时候调用this.startUpload()来启动下一个文件的上传是不错的选择。
当在进行多文件上传的时候，中途用cancelUpload取消了正在上传的文件，或者用stopUpload停止了正在上传的文件，
那么在 uploadComplete中就要很小心的使用this. startUpload()，因为在上述情况下，
uploadError和uploadComplete会顺序执行，因此虽然停止了当前文件的上传，但会立即进行下一个文件的上传，
你可能会觉得这很奇怪，但事实上程序并没有错。如果你希望终止整个队列的自动上传，那么你需要做额外的程序处理了。
*/
/*
this.getStats()//获取当前状态的统计对象
{ 
in_progress : number // 值为1或0，1表示当前有文件正在上传，0表示当前没有文件正在上传 
files_queued : number // 当前上传队列中存在的文件数量 
successful_uploads : number // 已经上传成功（uploadSuccess触发）的文件数量 
upload_errors : number // 已经上传失败的文件数量 (不包括退出上传的文件) 
upload_cancelled : number // 退出上传的文件数量 
queue_errors : number // 入队失败（fileQueueError触发）的文件数量 
}
*/
function uploadComplete(file) {
    try {
        /*  I want the next upload to continue automatically so I'll call startUpload here */
        if (this.getStats().files_queued > 0) {
            this.startUpload();
        } else {
            //jQuery('#SpanMsg').show().text('上传完成');
        }
    } catch (ex) {
        this.debug(ex);
    }
}