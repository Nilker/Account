using System;
using System.Collections.Generic;
using System.Text;
using Cig.YanFa.Account.Application.Dto;

namespace Cig.YanFa.Account.Application.Crm.PurchaseOrder.Dto
{
    public class CostAccountInput : CrmPagerOption
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
        public string DepathNamePath { get; set; }

        /// <summary>        
        ///核算一级科目Code
        /// </summary>
        public int SubjectCode { get; set; }

        /// <summary>        
        ///核算二级科目Code
        /// </summary>
        public int SecondSubjectCode { get; set; }

        /// <summary>
        /// 计算阶段
        /// </summary>
        public int Step { get; set; }
    }
}
