using System;
using System.Collections.Generic;
using System.Text;
using Cig.YanFa.Account.Application.Dto;

namespace Cig.YanFa.Account.Application.Crm.PurchaseOrder.Dto
{
    public class CostAccountOut 
    {
        /// <summary>        
        ///PO号
        /// </summary>
        public string POCode { get; set; }

        /// <summary>        
        ///PO行号
        /// </summary>
        public int POLineNum { get; set; }

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
        ///采购类型
        /// </summary>
        public int ContentType { get; set; }

        /// <summary>        
        ///采购类型名称
        /// </summary>
        public string ContentTypeName { get; set; }

        /// <summary>        
        ///核算一级科目Code
        /// </summary>
        public int SubjectCode { get; set; }

        /// <summary>        
        ///核算二级科目Code
        /// </summary>
        public int SecondSubjectCode { get; set; }

        /// <summary>        
        ///核算一级科目名称
        /// </summary>
        public string SubjectName { get; set; }

        /// <summary>        
        ///二级核算科目名称
        /// </summary>
        public string SecondSubjetName { get; set; }

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
