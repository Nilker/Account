using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Cig.YanFa.Account.Application.Common;
using Cig.YanFa.Account.Core;
using Cig.YanFa.Account.Core.Crm2009;

namespace Cig.YanFa.Account.Application.Crm.PurchaseOrder.Dto
{
    public class CostAccountExportDto : CostAccount
    {
        readonly Dictionary<int, string> _dic;
        public CostAccountExportDto()
        {
            _dic = EnumHelper.GetValueAndDesc<CommonEnum.EnumCostAccountSplitStep>();
        }
        /// <summary>
        /// 阶段名称
        /// </summary>
        [Description("阶段名称")]
        public string StepName => _dic[Step];
    }
}
