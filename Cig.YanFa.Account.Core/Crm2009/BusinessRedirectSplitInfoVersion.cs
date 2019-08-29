using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Cig.YanFa.Account.Core.Crm2009
{
    /// <summary>
    /// 业务定向 拆分比例
    /// add by lhl 2019年4月10日09:39:16
    /// </summary>
    [Table("BusinessRedirectSplitInfoVersion")]
    public class BusinessRedirectSplitInfoVersion
    {

        [Key]
        [Identity]
        public int Id { get; set; }

        /// <summary>
        /// 来源部门 成本部门
        /// </summary>
        [MaxLength(50)]
        public string FromDepartId { get; set; }

        /// <summary>
        /// 定向给谁  利润部门
        /// </summary>
        [MaxLength(50)]
        public string ToDepartId { get; set; }

        /// <summary>
        /// 拆分比例
        /// </summary>
        [MaxLength(100)]
        public string Split { get; set; }

        /// <summary>
        /// 状态 空、0：未拆分， 1已拆分  2 异常
        /// </summary>
        public int? State { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int Month { get; set; }

        /// <summary>        
        ///
        /// </summary>
        public int Year { get; set; }



        
    }
}
