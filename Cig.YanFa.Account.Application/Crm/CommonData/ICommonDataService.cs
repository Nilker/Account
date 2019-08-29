using System;
using System.Collections.Generic;
using System.Text;
using Cig.YanFa.Account.Application.Crm.CommonData.Dto;
using Cig.YanFa.Account.Application.PurchaseOrder.Dto;
using Cig.YanFa.Account.Core.Crm2009;
using Cig.YanFa.Account.Core.SysRightsManager;

namespace Cig.YanFa.Account.Application.PurchaseOrder
{
    public interface ICommonDataService
    {
        /// <summary>
        /// 获取签约公司
        /// </summary>
        /// <returns></returns>
        List<CompanyInfoDto> CompanyInfos();

        /// <summary>
        /// 获取所有 业务部门数据
        /// </summary>
        /// <returns></returns>
        List<DepartMent> GetDepartMents();


        /// <summary>
        /// 获取所有 业务部门数据 版本表
        /// </summary>
        /// <returns></returns>
        List<DepartMentVersion> GetDepartMentsByVersion(string period);

        /// <summary>
        /// 获取字典 集合
        /// </summary>
        List<DicInfoDto> GetDicInfos(int dicType=-1);


        /// <summary>
        /// 获取全部 Relevance
        /// </summary>
        /// <returns></returns>
        List<RelevanceDto> GetRelevanceInfos(int type, int mainType = 0);

        /// <summary>
        /// 获取所有  物料
        /// </summary>
        /// <param name="level"></param>
        /// <param name="parentId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        List<ProductCategoryDto> GetProductCategorys(int level = 0, int parentId = 0, string type = null);
    }
}
