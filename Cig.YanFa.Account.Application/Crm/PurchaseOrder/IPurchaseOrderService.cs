using System;
using System.Collections.Generic;
using System.Text;
using Cig.YanFa.Account.Application.Crm.HeSuan.Dto;
using Cig.YanFa.Account.Application.Crm.PurchaseOrder.Dto;
using Cig.YanFa.Account.Application.Dto;
using Cig.YanFa.Account.Application.PurchaseOrder.Dto;
using Cig.YanFa.Account.Core.Crm2009;

namespace Cig.YanFa.Account.Application.PurchaseOrder
{
    public interface IPurchaseOrderService
    {
        /// <summary>
        /// 获取所有的 Po 数据汇总 详细信息
        /// </summary>
        /// <returns></returns>
        List<PurchaseOrderLineConfirmHistory> GetAll();

        /// <summary>
        /// 将Po汇总信息 插入PurchaseOrderLineConfirmHistory
        /// </summary>
        /// <param name="poCode"></param>
        /// <returns></returns>
        bool AddPurchaseOrderLineConfirmHistorys(string poCode="");

        /// <summary>
        /// 新增拆分 比例版本  之前上个月
        /// </summary>
        /// <param name="poCode"></param>
        /// <returns></returns>
        bool AddPurchaseRequirementDeptShareRatioVersionRepository(string poCode);



        /// <summary>
        /// 添加 拆分前 AddCostAccountBeforeSplit 
        /// </summary>
        /// <returns></returns>
        bool AddCostAccountBeforeSplit(string poCode = "", string period = null);

        /// <summary>
        /// 获取所有 拆分前数据
        /// </summary>
        /// <returns></returns>
        List<CostAccount> GetAllBeforeSplits();

        /// <summary>
        /// 拆分
        /// </summary>
        /// <returns></returns>
        bool DoSplit(string poCode="", string period = null);

        /// <summary>
        /// 获取公共成本拆分比例
        /// </summary>
        /// <param name="sourceDepartId"></param>
        /// <returns></returns>
        List<CommonSplitDto> GetCommonSplit(string sourceDepartId,int month,int year);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        PagedResultDto<CostAccountOut> GetCostAccountByPage(CostAccountInput queryModel);

        List<PurchaseOrderLineConfirmHistoryExportDto> GetPurchaseOrderLineConfirmHistory(string period);
        List<PurchaseRequirementDeptShareRatioVersionExportDto> GetPurchaseRequirementDeptShareRatioVersions(string period);
        List<CostAccountExportDto> GetCostAccountExportDtos(string period);
        List<v_AchievementBusinessDepartMappingVersionExportDto> GetBusinessToAchMappingVersion(string period);
    }
}
