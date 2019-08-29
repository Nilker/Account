using System;
using System.Collections.Generic;
using System.Text;
using Cig.YanFa.Account.Application.Dto;
using Cig.YanFa.Account.Core.Crm2009;

namespace Cig.YanFa.Account.Application.PurchaseOrder.Dto
{
    public class CostAccountConfigInput: CrmPagerOption
    {
        /// <summary>        
        ///签约公司
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>        
        ///订单来源
        /// </summary>
        public int OrderType { get; set; }

        /// <summary>        
        ///采购类型
        /// </summary>
        public int ContentType { get; set; }

        /// <summary>        
        ///物料分类
        /// </summary>
        public string ProductCat1 { get; set; }

        /// <summary>        
        ///物料
        /// </summary>
        public string ProductCat2 { get; set; }

        /// <summary>        
        ///支出类型一级
        /// </summary>
        public int PayOutType1 { get; set; }

        /// <summary>        
        ///支出类型二级
        /// </summary>
        public int PayOutType2 { get; set; }
    }
}
