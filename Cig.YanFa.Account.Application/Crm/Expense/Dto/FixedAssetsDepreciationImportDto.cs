using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace Cig.YanFa.Account.Application.Crm.Expense.Dto
{
    /// <summary>
    /// 固定资产折旧 导入Excel 实体
    /// </summary>
    public class FixedAssetsDepreciationImportDto
    {
        /// <summary>
        /// 当前 行号
        /// </summary>
        public int CurrentRow { get; set; }

        /// <summary>        
        /// 固定资产编号---需要autoMapper -->ExpenseCode
        /// </summary>
        public string FixedAssetsCode { get; set; }

        /// <summary>        
        /// 行号  ---autoMapper ===>ExpenseLineNum
        /// </summary>
        public int? LineNum { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string Period { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string DepartNamePath { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string CompanyName { get; set; }

    }
}
