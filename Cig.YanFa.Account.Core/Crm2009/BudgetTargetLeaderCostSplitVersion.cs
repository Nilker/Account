using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

/// <summary>
/// add by lhl 2019/5/22 17:04:27
/// </summary>
namespace  Cig.YanFa.Account.Core.Crm2009  
{
    /// <summary>
    ///  BudgetTargetLeaderCostSplitVersion 描述：
    /// </summary>
    [Table("BudgetTargetLeaderCostSplitVersion")]
    public class BudgetTargetLeaderCostSplitVersion
    {
        
            /// <summary>        
            ///
            /// </summary>
            [Key]
            [Identity]
            public int RecID{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string TargetCode{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string DepartID{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string SourceDepartID{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public decimal SplitRate{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int Month{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int Year{ get;set; }
     }
 }