using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cig.YanFa.Account.Application.Crm.Expense;
using Cig.YanFa.Account.Application.Crm.PurchaseOrder;
using Cig.YanFa.Test;
using Cig.YanFa.Account.Application.PurchaseOrder;
using Cig.YanFa.Account.Application.PurchaseOrder.Dto;
using Cig.YanFa.Account.Web.Core.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Cig.YanFa.Account.Web.Core.Controllers
{
    /// <summary>
    /// 测试 接口，用来测试一些方法
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        private ILogger<TestController> _logger;

        private ITestService _testService;
        private IPurchaseOrderService _purchaseOrderService;
        private ICostAccountConfigService _costAccountConfigService;

        private IExpenseService _expenseService;

        public TestController(ILogger<TestController> logger
            , ITestService testService
            , IPurchaseOrderService purchaseOrderService
            , ICostAccountConfigService costAccountConfigService
            ,IExpenseService expenseService)
        {
            _logger = logger;
            _testService = testService;
            _purchaseOrderService = purchaseOrderService;
            _costAccountConfigService = costAccountConfigService;
            _expenseService = expenseService;
        }

        /// <summary>
        /// 获取po --poline --poacConfim
        /// </summary>
        /// <param name="poCode">不传就是全部</param>
        /// <returns></returns>
        [HttpGet("GetPos")]
        public dynamic GetPos(string poCode = null)
        {
            var purchaseOrders = _purchaseOrderService.GetAll();
            if (!string.IsNullOrEmpty(poCode))
            {
                purchaseOrders = purchaseOrders.Where(s => s.POCode == poCode).ToList();
            }

            return purchaseOrders;
        }

        /// <summary>
        /// 将Po汇总信息--写入--PurchaseOrderLineConfirmHistory
        /// </summary>
        /// <param name="poCode"></param>
        /// <returns></returns>
        [HttpGet("AddPos")]
        public dynamic AddPos(string poCode)
        {
            var b = _purchaseOrderService.AddPurchaseOrderLineConfirmHistorys(poCode);
            return b;
        }

        /// <summary>
        /// 根据 成本支出月成本历史表【PurchaseOrderLineConfirmHistorys】--->写入 集采需求拆分版本表【PurchaseRequirementDeptShareRatioVersion】
        /// </summary>
        /// <param name="poCode"></param>
        /// <returns></returns>
        [HttpGet("AddRationVersion")]
        public dynamic AddRationVersion(string poCode)
        {
            var b = _purchaseOrderService.AddPurchaseRequirementDeptShareRatioVersionRepository(poCode);
            return b;
        }

        /// <summary>
        /// 添加 成本支出核算科目配置表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("AddCostAccountConfig")]
        public dynamic AddCostAccountConfig(CostAccountConfigInput model)
        {
            var b = _costAccountConfigService.AddCostAccountConfig(model);
            return b;
        }

        /// <summary>
        /// 成本-----》拆分-----》 拆分前
        /// </summary>
        /// <param name="poCode">PoCode</param>
        /// <param name="period">计算月份：如 “2019-05”</param>
        /// <returns></returns>
        [HttpGet("AddCostAccountBeforeSplit")]
        public dynamic AddCostAccountBeforeSplit(string poCode="",string period=null)
        {
            bool b = _purchaseOrderService.AddCostAccountBeforeSplit(poCode,period);
            return b;
        }


        /// <summary>
        /// 获取拆分前
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetBeforeSplits")]
        public JsonResult GetBeforeSplits()
        {
            var befores = _purchaseOrderService.GetAllBeforeSplits();
            return Json(new JsonFlag(befores));
        }

        /// <summary>
        /// 成本-----》拆分-----》  进行 公共拆分 定向拆分 转架构
        /// </summary>
        /// <param name="poCode">PoCode</param>
        /// <param name="period">计算月份：如 “2019-05”</param>
        /// <returns></returns>
        [HttpGet("DoSplit")]
        public dynamic DoSplit(string poCode = "", string period = null)
        {
            try
            {
                var befores = _purchaseOrderService.DoSplit(poCode, period);
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return true;
        }

        /// <summary>
        /// 获取公共成本拆分比例，计入部门----》末级
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCommonSplit")]
        public JsonResult GetCommonSplit()
        {
            var tem = _purchaseOrderService.GetCommonSplit("DP01384",DateTime.Now.AddMonths(-1).Month,DateTime.Now.AddMonths(-1).Year);
            return Json(new JsonFlag(tem));
        }
        
        /// <summary>
        /// 费用---添加---》汇总表
        /// </summary>
        /// <param name="expenseCode">费用单号：如IE******</param>
        /// <returns></returns>
        [HttpGet("AddExpenseLineConfirmHistory")]
        public JsonResult AddExpenseLineConfirmHistory(string expenseCode="")
        {
            var b = _expenseService.AddExpenseLineConfirmHistory(expenseCode);
            return Json(new JsonFlag(b));
        }

        /// <summary>
        /// 费用---拆分---》拆分前
        /// </summary>
        /// <param name="expenseType">费用类型</param>
        /// <param name="expenseCode">费用Code</param>
        /// <param name="period">计算月份：如 “2019-05”</param>
        /// <returns></returns>
        [HttpGet("AddExpenseAccount")]
        public JsonResult AddExpenseAccount(int expenseType,string expenseCode="",string period=null)
        {
            var b = _expenseService.AddExpenseAccount(expenseType, expenseCode,period);
            return Json(new JsonFlag(b ? 1 : 0, b ? "操作成功" : "操作失败,有可能原始数据计入部门为空"));
        }

        /// <summary>
        /// 费用---拆分---》公共拆分 定向拆分 转架构
        /// </summary>
        /// <param name="expenseType">费用类型</param>
        /// <param name="expenseCode">费用Code</param>
        /// <param name="period">计算月份：如 “2019-05”</param>
        /// <returns></returns>
        [HttpGet("ExpenseAccountCommonSplit")]
        public JsonResult ExpenseAccountCommonSplit(int expenseType,string expenseCode = "", string period = null)
        {
            var b = _expenseService.ExpenseAccountDoSplit(expenseType,expenseCode, period);
            return Json(new JsonFlag(b));
        }

    }
}
