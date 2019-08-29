using System;
using System.Collections.Generic;
using System.Text;

namespace Cig.YanFa.Account.Application.Crm.HeSuan.Dto
{
    /// <summary>
    /// 公共成本拆分 比例
    /// </summary>
    public class CommonSplitDto
    {
        /// <summary>
        /// 来源部门
        /// </summary>
        public string SourceDepartID { get; set; }

        public string FromDepartID { get; set; }
        public string FromDepartName { get; set; }
        public string ToDepartID { get; set; }
        public string ToDepartName { get; set; }
        public decimal SplitRate { get; set; }

    }
}
