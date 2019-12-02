using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz.Impl;

namespace Quartz.Net.Framework
{
    class Program
    {
        static void Main(string[] args)
        {
            RunScheduler();

            Console.ReadKey();
        }



        private static async Task RunScheduler()
        {
            // 创建作业调度池
            ISchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();


            var jobDataMap = new JobDataMap();
            jobDataMap.Add("name", "beck");
            jobDataMap.Add("times", 1);

            // 创建作业
            IJobDetail job = JobBuilder.Create<HelloJob>()
                .WithIdentity("job1", "group1")
                .UsingJobData(jobDataMap)
                .Build();

            // 创建触发器，每10s执行一次
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(10)
                    .RepeatForever())
                .WithCronSchedule("0 0 8 * * ?") //每天早上8点
                .Build();

            // 加入到作业调度池中
            await scheduler.ScheduleJob(job, trigger);

            // 开始运行
            await scheduler.Start();
        }
    }
}
