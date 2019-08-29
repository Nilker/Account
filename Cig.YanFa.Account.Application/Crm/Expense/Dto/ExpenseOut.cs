using System;
using System.Collections.Generic;
using System.Text;

namespace Cig.YanFa.Account.Application.Crm.Expense.Dto
{
    public class ExpenseOut
    {
        /// <summary>        
        ///PO号
        /// </summary>
        public string ExpenseCode { get; set; }

        /// <summary>        
        ///PO行号
        /// </summary>
        public int ExpenseLineNum { get; set; }

        /// <summary>        
        ///成本计入部门
        /// </summary>
        public string DepartID { get; set; }

        public string DepartNamePath { get; set; }

        /// <summary>        
        ///计入日期
        /// </summary>
        public string Period { get; set; }

        /// <summary>        
        ///计入金额
        /// </summary>
        public decimal Amount { get; set; }


        /// <summary>        
        ///项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>        
        ///签约公司
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>        
        ///签约公司名称
        /// </summary>
        public string CompanyName { get; set; }


        /// <summary>
        /// 阶段
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// 阶段名称
        /// </summary>
        public string StepName { get; set; }
    }
}
