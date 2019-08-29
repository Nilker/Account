using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

/// <summary>
/// add by lhl 2019/5/22 17:06:55
/// </summary>
namespace  Cig.YanFa.Account.Core.SysRightsManager  
{
    /// <summary>
    ///  SysLoginKey 描述：
    /// </summary>
    [Table("SysLoginKey")]
    public class SysLoginKey
    {
        
            /// <summary>        
            ///
            /// </summary>
            public string Key{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int RecID{ get;set; }
     }
 }