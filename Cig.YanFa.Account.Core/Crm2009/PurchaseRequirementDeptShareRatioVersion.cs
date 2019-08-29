using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cig.YanFa.Account.Core.Crm2009
{
    /// <summary>
    /// 集采需求拆分版本表
    /// </summary>
    [Table("PurchaseRequirementDeptShareRatioVersion")]
    public class PurchaseRequirementDeptShareRatioVersion : EntityBase
    {
        [Key]
        [Identity]
        public int Id { get; set; }

        /// <summary>        
        ///PO单号
        /// </summary>
        [Description("PO单号")]
        public string POCode { get; set; }

        /// <summary>        
        ///PO行号
        /// </summary>
        [Description("PO行号")]
        public int POLineNum { get; set; }

        /// <summary>        
        ///计入期间
        /// </summary>
        [Description("计入期间")]
        public string Period { get; set; }

        /// <summary>        
        ///PR单号
        /// </summary>
        [Description("PR单号")]
        public string PRCode { get; set; }

        /// <summary>        
        ///PE单号
        /// </summary>
        [Description("PE单号")]
        public string PECode { get; set; }

        /// <summary>        
        ///分摊部门
        /// </summary>
        [Description("分摊部门")]
        public string DepartID { get; set; }

        /// <summary>        
        ///部门全路径
        /// </summary>
        [Description("分摊部门")]
        public string DepartNamePath { get; set; }

        /// <summary>        
        ///分摊比例
        /// </summary>
        [Description("分摊比例")]
        public decimal ShareProportion { get; set; }
    }
}
