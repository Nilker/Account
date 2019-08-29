using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Cig.YanFa.Account.Core
{
    public class EntityBase
    {
        public EntityBase()
        {
            CreateTime = DateTime.Now;
            //IsDeleted = false;
        }
        /// <summary>
        /// 状态 ：0：正常 1 ：非正常
        /// </summary>
        [Description("状态 ：0：正常 1 ：非正常")]
        public int Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Description("创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        [Description("创建人Id")]
        public long? CreateUserID { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [Description("更新时间")]
        public DateTime? LastUpdateTime { get; set; }

        /// <summary>
        /// 更新人Id
        /// </summary>
        [Description("更新人Id")]
        public long? LastUpdateUserID { get; set; }
        //public bool IsDeleted { get; set; }
    }
}
