using System;
using System.Collections.Generic;
using System.Text;
using Cig.YanFa.Account.Application.Dto;

namespace Cig.YanFa.Account.Application.Crm.Expense.Dto
{
    public class ExpenseInput : CrmPagerOption
    {
        /// <summary>        
        ///计入日期
        /// </summary>
        public string Period { get; set; }

        /// <summary>        
        ///PO号
        /// </summary>
        public string POCode { get; set; }

        /// <summary>        
        ///项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>        
        ///成本计入部门
        /// </summary>
        public string DepartID { get; set; }

        /// <summary>
        /// 成本计入部门名称
        /// </summary>
        public string DepartNamePath { get; set; }
       

        /// <summary>
        /// 计算阶段
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// 费用类型 对应类型EnumExpenseType
        /// </summary>
        public int ExpenseType { get; set; }
    }
}
