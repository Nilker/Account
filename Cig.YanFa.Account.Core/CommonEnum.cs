using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Cig.YanFa.Account.Core
{
    public  class CommonEnum
    {
        /// <summary>
        /// 核算 拆分阶段
        /// </summary>
        public enum EnumCostAccountSplitStep
        {
            /// <summary>
            /// 业务历史数据
            /// </summary>
            [Description("历史数据")]
            History=-1,
            
            /// <summary>
            /// 拆分前
            /// </summary>
            [Description("拆分前")]
            BeforeSplit=1,

            /// <summary>
            /// 公共拆分
            /// </summary>
            [Description("公共拆分")]
            CommonSplit=2,

            /// <summary>
            /// 定向拆分
            /// </summary>
            [Description("定向拆分")]
            RedirectSplit=3,

            /// <summary>
            /// 业务转管理
            /// </summary>
            [Description("业务转管理")]
            BusinessToAchDepartSplit=4
        }

        /// <summary>
        /// 费用类型
        /// </summary>
        public enum EnumExpenseType
        {
            /// <summary>
            /// 火车月结
            /// </summary>
            [Description("火车月结")]
            TrainMonth=1,

            /// <summary>
            /// 机票月结
            /// </summary>
            [Description("机票月结")]
            AirTicketMonth=2,

            /// <summary>
            /// 住宿月结
            /// </summary>
            [Description("住宿月结")]
            AccommodationMonth =3,

            /// <summary>
            /// 报销借款
            /// </summary>
            [Description("报销借款")]
            ReimbursementAndBorrowing=4,

            /// <summary>
            /// 固资折旧
            /// </summary>
            [Description("固资折旧")]
            FixedAssetsDepreciation=5,

            /// <summary>
            /// 快递
            /// </summary>
            [Description("快递")]
            Express=6
        }
    }
}
