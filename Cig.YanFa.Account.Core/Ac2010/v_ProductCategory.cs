using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cig.YanFa.Account.Core.Ac2010
{
    /// <summary>
    ///  v_ProductCategory 描述：
    /// </summary>
    [Table("v_ProductCategory")]
    public class v_ProductCategory
    {

        /// <summary>        
        ///
        /// </summary>
        [Key]
        public string ProductCatID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string Name { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int Level { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int ParentID { get; set; }

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
        public int Status { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public string Type { get; set; }
    }
}