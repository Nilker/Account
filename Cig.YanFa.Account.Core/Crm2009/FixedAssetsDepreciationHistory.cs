using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cig.YanFa.Account.Core.Crm2009
{
    /// <summary>
    /// 固资折旧 --历史表
    /// </summary>
    [Table("FixedAssetsDepreciationHistory")]
    public class FixedAssetsDepreciationHistory
    {

        /// <summary>        
        ///
        /// </summary>
        [Key]
        [Identity]
        public long Id { get; set; }

        /// <summary>        
        /// 固定资产编号---需要autoMapper -->ExpenseCode
        /// </summary>
        [MaxLength(20)]
        public string FixedAssetsCode { get; set; }

        /// <summary>        
        /// 行号  ---autoMapper ===>ExpenseLineNum
        /// </summary>
        public int LineNum { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [MaxLength(10)]
        public string Period { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string DepartID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [MaxLength(200)]
        public string DepartNamePath { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [MaxLength(20)]
        public string CompanyCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [MaxLength(200)]
        public string CompanyName { get; set; }




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
        [MaxLength(20)]
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
        [MaxLength(20)]
        public string LastUpdateUserName { get; set; }
        public int Status { get; set; }
    }
}
