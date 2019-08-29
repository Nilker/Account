using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cig.YanFa.Account.Core.Crm2009
{
    /// <summary>
    ///  ExpenseAccount 描述：
    /// </summary>
    [Table("ExpenseAccount")]
    public class ExpenseAccount
    {

        /// <summary>        
        ///
        /// </summary>
        [Key]
        [Identity]
        public int Id { get; set; }

        
        /// <summary>        
        ///流水号
        /// </summary>
        [Description("流水号")]
        public string ExpenseCode { get; set; }

        /// <summary>        
        /// 行号
        /// </summary>
        [Description("行号")]
        public int ExpenseLineNum { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("成本计入部门")]
        public string DepartID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("成本计入部门")]
        public string DepartNamePath { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("确认时间")]
        public DateTime ConfirmDate { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("计入日期")]
        public string Period { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("计入金额")]
        public decimal Amount { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int Status { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int CreateUserID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int LastUpdateUserID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string LastUpdateUserName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("物料分类")]
        public string MaterialClassCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("物料分类")]
        public string MaterialClassName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("物料")]
        public string MaterialCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("物料")]
        public string MaterialName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("费用项")]
        public string ExpenseItem { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("费用项")]
        public string ExpenseItemName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("项目编号")]
        public string ProjectCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("项目名称")]
        public string ProjectName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("项目类型")]
        public int ProjectType { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("签约公司")]
        public string CompanyCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("签约公司")]
        public string CompanyName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("来源 主键 Id")]
        public int SourceId { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("比例")]
        public decimal Ratio { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("阶段")]
        public int Step { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("计算公式")]
        public string Formula { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("封账")]
        public int IsClose { get; set; }

        /// <summary>
        ///  费用类型 对应类型EnumExpenseType
        /// </summary>
        [Description("费用类型")]
        public int ExpenseType { get; set; }
    }
}