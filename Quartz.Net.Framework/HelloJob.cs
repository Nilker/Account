using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*----------------------------------------------------------------
* 项目名称 ：Quartz.Net.Framework
* 项目描述 ：
* 类 名 称 ：HelloJob
* 类 描 述 ： 
* 作    者 ：lihongli
* 创建时间 ：2019/12/2 17:24:41
* 版 本 号 ：v1.0.0.0
----------------------------------------------------------------*/
namespace Quartz.Net.Framework
{
    [PersistJobDataAfterExecution]
    [DisallowConcurrentExecution]
    public class HelloJob : IJob
    {

        public string Name { get; set; }

        public int Times { get; set; }


        /// <summary>
        /// 作业调度定时执行的方法
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            //    context.Scheduler 调度器信息
            //    context.Trigger 触发器信息
            //    context.JobDetail 作业信息
            //    context.ScheduledFireTimeUtc 当前触发时间
            //    context.NextFireTimeUtc 下一次将被触发时间

            context.JobDetail.JobDataMap["times"] = Times + 1;

            await Console.Out.WriteLineAsync(
               $"{context.JobDetail.Key}--》{context.NextFireTimeUtc.ToString()}-----{Name}----{Times}"  );

        }
    }
}
