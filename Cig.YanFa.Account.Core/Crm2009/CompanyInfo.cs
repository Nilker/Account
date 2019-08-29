using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

/// <summary>
/// add by lhl 2019/5/22 17:04:33
/// </summary>
namespace Cig.YanFa.Account.Core.Crm2009
{
    /// <summary>
    ///  CompanyInfo 描述：
    /// </summary>
    [Table("CompanyInfo")]
    public class CompanyInfo
    {

        /// <summary>        
        ///
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int CompanyType { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int LockState { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string Address { get; set; }

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
        public DateTime LastUpdateTime { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int LastUpdateUserID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string Notes { get; set; }
    }
}