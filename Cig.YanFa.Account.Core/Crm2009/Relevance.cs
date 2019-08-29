using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

/// <summary>
/// add by lhl 2019/5/22 17:05:12
/// </summary>
namespace  Cig.YanFa.Account.Core.Crm2009  
{
    /// <summary>
    ///  Relevance 描述：
    /// </summary>
    [Table("Relevance")]
    public class Relevance
    {
        
            /// <summary>        
            ///
            /// </summary>
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int RecID{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string MainType{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string SubType{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int Type{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int Status{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public DateTime CreateTime{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public int CreateUserId{ get;set; }
        
            /// <summary>        
            ///
            /// </summary>
            public string Remark{ get;set; }
     }
 }