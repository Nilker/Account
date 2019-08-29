using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

/// <summary>
/// add by lhl 2019/5/22 17:05:01
/// </summary>
namespace Cig.YanFa.Account.Core.Crm2009
{
    /// <summary>
    ///  POActualConfirmation 描述：
    /// </summary>
    [Table("POActualConfirmation")]
    public class POActualConfirmation
    {
        [Key]
        public int ConfirmationID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int POLineID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ActualPeriod { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public decimal ActualAmount { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string ActualNotes { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int ApproveStatus { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int Status { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int POLineNum { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string POCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public DateTime LastUpdateTime { get; set; }
    }
}