using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

/// <summary>
/// add by lhl 2019/5/22 17:06:54
/// </summary>
namespace  Cig.YanFa.Account.Core.SysRightsManager  
{
    /// <summary>
    ///  SysInfo 描述：
    /// </summary>
    [Table("SysInfo")]
    public class SysInfo
    {
        
            [Key]
            [Identity]
            public int RecID{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string SysID{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string SysName{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string SysURL{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int Status{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string Intro{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public DateTime CreateTime{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int CreateUserID{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int OrderNum{ get;set; }
     }
 }