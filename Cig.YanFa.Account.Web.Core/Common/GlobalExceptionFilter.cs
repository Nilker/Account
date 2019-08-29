﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cig.YanFa.Account.Web.Core.Common.Email;
using Cig.YanFa.Configuration;
using log4net.Core;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cig.YanFa.Account.Web.Core.Common
{
    /// <summary>
    /// 全局异常错误日志
    /// </summary>
    public class GlobalExceptionsFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _env;
        private readonly ILogger<GlobalExceptionsFilter> _loggerHelper;
        private readonly IEmailSender _emailSender;
        private IConfigurationRoot _config;

        public GlobalExceptionsFilter(IHostingEnvironment env
            , ILogger<GlobalExceptionsFilter> loggerHelper
            , IEmailSender emailSender)
        {
            _env = env;
            _loggerHelper = loggerHelper;
            _emailSender = emailSender;
            _config = env.GetAppConfiguration();
        }
        public void OnException(ExceptionContext context)
        {
            var json = new JsonErrorResponse();
            json.Message = context.Exception.Message;//错误信息
            if (_env.IsDevelopment())
            {
                json.DevelopmentMessage = context.Exception.StackTrace;//堆栈信息
            }
            context.Result = new InternalServerErrorObjectResult(json);

            //采用log4net 进行错误日志记录
            _loggerHelper.LogError(context.Exception, context.Exception.Message);

            //邮件

            _emailSender.SendEmailAsync(_config["ExceptionSendTo"], "核算异常-.net Core",
                $"{context.Exception.Message}=====>{context.Exception.StackTrace}");
        }

        /// <summary>
        /// 自定义返回格式
        /// </summary>
        /// <param name="throwMsg"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public string WriteLog(string throwMsg, Exception ex)
        {
            return string.Format("【自定义错误】：{0} \r\n【异常类型】：{1} \r\n【异常信息】：{2} \r\n【堆栈调用】：{3}", new object[] { throwMsg,
                ex.GetType().Name, ex.Message, ex.StackTrace });
        }

    }
    public class InternalServerErrorObjectResult : ObjectResult
    {
        public InternalServerErrorObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
    }
    //返回错误信息
    public class JsonErrorResponse
    {
        /// <summary>
        /// 生产环境的消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 开发环境的消息
        /// </summary>
        public string DevelopmentMessage { get; set; }
    }

}