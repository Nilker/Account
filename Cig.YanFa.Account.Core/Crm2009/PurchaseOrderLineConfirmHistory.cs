using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cig.YanFa.Account.Core.Crm2009
{
    /// <summary>
    /// 成本支出月成本历史表
    /// </summary>
    [Table("PurchaseOrderLineConfirmHistory")]
    public class PurchaseOrderLineConfirmHistory : EntityBase
    {
        [Key]
        [Identity]
        public int Id { get; set; }

        /// <summary>        
        ///PO号
        /// </summary>
        [Description("Po订单")]
        public string POCode { get; set; }

        /// <summary>        
        ///PO行号
        /// </summary>
        [Description("PO行号")]
        public int POLineNum { get; set; }

        /// <summary>        
        ///计入期间（yyyy-mm）
        /// </summary>
        [Description("计入期间")]
        public string Period { get; set; }

        /// <summary>        
        ///计入金额
        /// </summary>
        [Description("计入金额")]
        public decimal Amount { get; set; }
        

        /// <summary>        
        ///物料分类
        /// </summary>
        [Description("物料分类")]
        public string MaterialClassCode { get; set; }

        /// <summary>        
        ///物料分类名称
        /// </summary>
        [Description("物料分类")]
        public string MaterialClassName { get; set; }

        /// <summary>        
        ///物料
        /// </summary>
        [Description("物料")]
        public string MaterialCode { get; set; }

        /// <summary>        
        ///物料名称
        /// </summary>
        [Description("物料")]
        public string MaterialName { get; set; }

        /// <summary>        
        ///费用项
        /// </summary>
        [Description("费用项")]
        public int ExpenseItem { get; set; }

        /// <summary>        
        ///费用项名称
        /// </summary>
        [Description("费用项")]
        public string ExpenseItemName { get; set; }

        /// <summary>        
        ///项目编号
        /// </summary>
        [Description("项目编号")]
        public string ProjectCode { get; set; }

        /// <summary>        
        ///项目类别
        /// </summary>
        [Description("项目类别")]
        public int ProjectType { get; set; }

        /// <summary>        
        ///项目名称
        /// </summary>
        [Description("项目类别")]
        public string ProjectName { get; set; }

        /// <summary>        
        ///签约公司
        /// </summary>
        [Description("签约公司")]
        public string CompanyCode { get; set; }

        /// <summary>        
        ///签约公司名称
        /// </summary>
        [Description("签约公司")]
        public string CompanyName { get; set; }

        /// <summary>        
        ///成本计入部门
        /// </summary>
        [Description("成本计入部门")]
        public string DepartID { get; set; }

        /// <summary>        
        ///成本计入部门名称（全路径）
        /// </summary>
        [Description("成本计入部门")]
        public string DepartNamePath { get; set; }

        /// <summary>        
        ///供应编码
        /// </summary>
        [Description("供应商")]
        public string SupplierID { get; set; }

        /// <summary>        
        ///供应商名称
        /// </summary>
        [Description("供应商")]
        public string SupplierName { get; set; }

        /// <summary>        
        ///采购类型
        /// </summary>
        [Description("采购类型")]
        public int ContentType { get; set; }

        /// <summary>        
        ///采购类型名称
        /// </summary>
        [Description("采购类型")]
        public string ContentTypeName { get; set; }

        /// <summary>        
        ///订单来源
        /// </summary>
        [Description("订单来源")]
        public int OrderEnum { get; set; }

        /// <summary>        
        ///订单来源名称
        /// </summary>
        [Description("订单来源")]
        public string OrderEnumName { get; set; }

        /// <summary>        
        ///创建人 TrueName
        /// </summary>
        [Description("创建人")]
        public string CreateUserName { get; set; }

        /// <summary>        
        ///更新人 TrueName
        /// </summary>
        [Description("更新人")]
        public string LastUpdateUserName { get; set; }


        /// <summary>
        /// 拆分比例--中间转换使用，不会存入数据库；
        /// </summary>
        [NotMapped]
        [Description("拆分比例")]
        public decimal Ratio { get; set; }

        /// <summary>
        /// 拆分公式
        /// </summary>
        [NotMapped]
        [Description("拆分公式")]
        public string Formula { get; set; }






        /// <summary>        
        ///PRCode
        /// </summary>
        [Description("PRCode")]
        public string PRCode { get; set; }

        /// <summary>        
        ///PR部门ID
        /// </summary>
        [Description("PR部门ID")]
        public string PRDepartID { get; set; }

        /// <summary>        
        ///PR部门名称
        /// </summary>
        [Description("PR部门名称")]
        public string PRDepartNamePath { get; set; }

        /// <summary>        
        ///PO创建时间
        /// </summary>
        [Description("PO创建时间")]
        public DateTime POCreateTime { get; set; }

        /// <summary>        
        ///POLine金额
        /// </summary>
        [Description("POLine金额")]
        public decimal POLineAmount { get; set; }

        /// <summary>        
        ///POLine开始时间
        /// </summary>
        [Description("POLine开始时间")]
        public string POLineBeginTime { get; set; }

        /// <summary>        
        ///POLine结束时间
        /// </summary>
        [Description("POLine结束时间")]
        public string POLineEndTime { get; set; }

        /// <summary>        
        ///PO关联项目的签约客户ID
        /// </summary>
        [Description("PO关联项目的签约客户ID")]
        public string CustID { get; set; }

        /// <summary>        
        ///PO关联项目的签约客户
        /// </summary>
        [Description("PO关联项目的签约客户")]
        public string CustName { get; set; }

        /// <summary>        
        ///PO行税码
        /// </summary>
        [Description("PO关联项目的签约客户")]
        public string TaxCode { get; set; }

    }
}
