using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cig.YanFa.Account.Application.Crm.Expense;
using Cig.YanFa.Account.Application.PurchaseOrder;
using Cig.YanFa.Account.Core;
using Cig.YanFa.Configuration;
using Hangfire;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Cig.YanFa.Account.Web.Core.StartupTask
{
    /// <summary>
    /// 启动事件
    /// </summary>
    public class MyStartupTask : IMyStartupTask
    {
        private IPurchaseOrderService _purchaseOrderService;
        private IExpenseService _expenseServiceService;
        private ILogger<MyStartupTask> _logger;
        private IConfigurationRoot _config;
        private IRecurringJobManager _recurringJobManager;
        public MyStartupTask(IHostingEnvironment env
            , ILogger<MyStartupTask> logger
            , IPurchaseOrderService purchaseOrderService
            , IExpenseService expenseServiceService
            , IRecurringJobManager recurringJobManager
            , ILoggerFactory loggerFactory)
        {
            _purchaseOrderService = purchaseOrderService;
            _expenseServiceService = expenseServiceService;
            _logger = logger;
            _config = env.GetAppConfiguration();
            _recurringJobManager = recurringJobManager;
        }


        //todo:lhl 启动时日志无法记录；
        public Task ExecuteAsync(CancellationToken cancellationToken = default)
        {

            _recurringJobManager.AddOrUpdate("成本-->AddCostHistoryAndVer",
                Hangfire.Common.Job.FromExpression(() => AddCostHistoryAndVer()),
                _config["JobCrons:AddCostHistoryAndVer"], TimeZoneInfo.Local);

            _recurringJobManager.AddOrUpdate("成本-->AccountSplit",
                Hangfire.Common.Job.FromExpression(() => AccountSplit()),
                 _config["JobCrons:DoInSplit"], TimeZoneInfo.Local);

            _recurringJobManager.AddOrUpdate("费用-->AddExpenseHistory",
                Hangfire.Common.Job.FromExpression(() => AddExpenseHistory()),
                _config["JobCrons:AddExpenseHistory"], TimeZoneInfo.Local);

            _recurringJobManager.AddOrUpdate("费用-->ExpenseSplit",
                Hangfire.Common.Job.FromExpression(() => ExpenseSplit()),
                _config["JobCrons:ExpenseSplit"], TimeZoneInfo.Local);

            _recurringJobManager.AddOrUpdate("Account测试",
                Hangfire.Common.Job.FromExpression(() => Test()),
                "0 0 0/1 * * ? ", TimeZoneInfo.Local);


            return Task.CompletedTask;
        }

        [JobDisplayName("数据写入==>成本支出月成本历史表+集采需求拆分版本表")]
        [Queue("account")]
        public void AddCostHistoryAndVer()
        {
            var poCode = _config["NeedToDoCodes:POCode"];
            //添加 成本支出月成本历史表
            _purchaseOrderService.AddPurchaseOrderLineConfirmHistorys(poCode);
            //添加 集采需求拆分版本表
            _purchaseOrderService.AddPurchaseRequirementDeptShareRatioVersionRepository(poCode);
        }


        [Hangfire.JobDisplayName("成本==>计算拆分前+后")]
        [Queue("account")]
        public void AccountSplit()
        {
            _purchaseOrderService.AddCostAccountBeforeSplit();
            _purchaseOrderService.DoSplit();
        }


        [JobDisplayName("数据写入==>费用支出月成本历史表")]
        [Queue("account")]
        public void AddExpenseHistory()
        {
            var poCode = _config["NeedToDoCodes:ExpenseCode"];
            //添加 成本支出月成本历史表
            _expenseServiceService.AddExpenseLineConfirmHistory(poCode);
        }


        [Hangfire.JobDisplayName("费用==>计算拆分前+后")]
        [Queue("account")]
        public void ExpenseSplit()
        {
            var poCode = _config["NeedToDoCodes:ExpenseCode"];
            _expenseServiceService.AddExpenseAccount((int)CommonEnum.EnumExpenseType.ReimbursementAndBorrowing,poCode);
            _expenseServiceService.ExpenseAccountDoSplit((int)CommonEnum.EnumExpenseType.ReimbursementAndBorrowing,poCode);
        }

        [Hangfire.JobDisplayName("Account中的测试")]
        [Queue("account")]
        public void Test()
        {
            _logger.LogInformation($@"Account中的测试:记录时间：{DateTime.Now}");
        }
    }
}
