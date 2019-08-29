using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cig.YanFa.Account.Core.Crm2009
{
    /// <summary>
    ///  ExpenseLineConfirmHistory 描述：
    /// </summary>
    [Table("ExpenseLineConfirmHistory")]
    public class ExpenseLineConfirmHistory
    {

        /// <summary>        
        ///
        /// </summary>
        [Key]
        [Identity]
        public int Id { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ExpenseCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int ExpenseLineNum { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string DepartID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string DepartNamePath { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public DateTime ConfirmDate { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string Period { get; set; }

        /// <summary>        
        ///
        /// </summary>
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
        public string MaterialClassCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string MaterialClassName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string MaterialName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ExpenseItem { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ExpenseItemName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int ProjectType { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string CompanyName { get; set; }
    }
}

