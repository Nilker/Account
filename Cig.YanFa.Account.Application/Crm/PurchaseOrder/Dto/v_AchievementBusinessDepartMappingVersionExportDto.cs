using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Cig.YanFa.Account.Application.Crm.PurchaseOrder.Dto
{
    /// <summary>
    /// 业务架构----管理架构关系 导出
    /// </summary>
    public  class v_AchievementBusinessDepartMappingVersionExportDto
    {
        [Description("年")]
        public int Year { get; set; }

        [Description("月")]
        public int Month { get; set; }

        [Description("业务架构Id")]
        public string BusinessDepartID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("业务架构名称")]
        public string BusinessDepartName { get; set; }

        [Description("管理架构Id")]
        public string AchievementDepartID { get; set; }

        /// <summary>        
        ///
        /// </summary>
        [Description("管理架构名称")]
        public string AchievementDepartName { get; set; }
        

        [Description("状态")]
        public int Status { get; set; }
    }
}
