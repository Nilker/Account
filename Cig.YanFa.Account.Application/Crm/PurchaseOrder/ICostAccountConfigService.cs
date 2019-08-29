using System;
using System.Collections.Generic;
using System.Text;
using Cig.YanFa.Account.Application.Crm.PurchaseOrder.Dto;
using Cig.YanFa.Account.Application.Dto;
using Cig.YanFa.Account.Application.PurchaseOrder.Dto;

namespace Cig.YanFa.Account.Application.Crm.PurchaseOrder
{
    public  interface ICostAccountConfigService
    {

        /// <summary>
        /// 添加 成本支出核算科目配置表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool AddCostAccountConfig(CostAccountConfigInput model);

        /// <summary>
        /// 分页查询 成本支出配置
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        PagedResultDto<CostAccountConfigOut> GetAccountConfigsByPage(CostAccountConfigInput queryModel);

        /// <summary>
        /// 删除配置文件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool DeleteCostAccountConfig(int id, int userId);
    }
}
