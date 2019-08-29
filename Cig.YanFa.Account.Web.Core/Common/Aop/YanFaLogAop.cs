using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Cig.YanFa.Account.Web.Core.Common.Aop
{
    /// <summary>
    /// 拦截器 YanFaLogAop  继承IInterceptor接口
    /// </summary>
    public class YanFaLogAop : IInterceptor
    {

        private ILogger<YanFaLogAop> _logger;

        public YanFaLogAop(ILogger<YanFaLogAop> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 实例化IInterceptor唯一方法 
        /// </summary>
        /// <param name="invocation">包含被拦截方法的信息</param>
        public void Intercept(IInvocation invocation)
        {
            var name = invocation.Method.DeclaringType.FullName + "." + invocation.Method.Name;
           //记录被拦截方法信息的日志信息
            var dataIntercept = $"当前执行方法：{name} " +
                                $"参数是： {string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray())} ======>";

            //在被拦截的方法执行完毕后 继续执行当前方法
            try
            {
                Stopwatch st = new Stopwatch();
                st.Start();
                invocation.Proceed();
                st.Stop();
                dataIntercept += $"耗时：{st.ElapsedMilliseconds}毫秒 <======";
            }
            catch (Exception ex)
            {
                dataIntercept +=
                    $"被拦截方法:{name}  执行中出现异常：{ex.Message + ex.InnerException + ex.StackTrace} \r\n";
                throw;
            }

            dataIntercept += $"被拦截方法:{name}  执行完毕;";//返回结果：{JsonConvert.SerializeObject(invocation.ReturnValue)}";
            
            _logger.LogInformation(dataIntercept);

        }
    }
}