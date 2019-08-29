using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cig.YanFa.Account.Core.Crm2009
{
    /// <summary>
    ///  v_AchievementBusinessDepartMapping 描述：
    /// </summary>
    [Table("v_AchievementBusinessDepartMappingVersion")]
    public class v_AchievementBusinessDepartMappingVersion
    {

        /// <summary>        
        ///
        /// </summary>
        public int RecID { get; set; }

        public DateTime VersionTime { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string AchievementDepartID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string AchievementDepartName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string BusinessDepartID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string BusinessDepartName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public DateTime ExpiredTime { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string CreateUserCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string LastUpdateUserCode { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int Status { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string NamePath { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int BudgetManagerID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string UserName { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string HRDepartID { get; set; }
    }
}