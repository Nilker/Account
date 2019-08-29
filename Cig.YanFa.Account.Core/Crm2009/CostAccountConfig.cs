using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cig.YanFa.Account.Core.Crm2009
{
    /// <summary>
    /// 成本支出核算科目配置表
    /// </summary>
    [Table("CostAccountConfig")]
    public class CostAccountConfig : EntityBase
    {
        [Key]
        [Identity]
        public int Id { get; set; }

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
