using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

/// <summary>
/// add by lhl 2019/5/22 17:04:41
/// </summary>
namespace  Cig.YanFa.Account.Core.Crm2009  
{
    /// <summary>
    ///  DictInfo 描述：
    /// </summary>
    [Table("DictInfo")]
    public class DictInfo
    {
        
            /// <summary>        
            ///
            /// </summary>
            [Key]
            public int DictID{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string DictName{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int DictType{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string Remark{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int CreateUserID{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public DateTime CreateTime{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int OrderNum{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int Status{ get;set; }
     }
 }