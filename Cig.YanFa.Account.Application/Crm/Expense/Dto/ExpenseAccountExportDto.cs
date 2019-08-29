using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Cig.YanFa.Account.Application.Common;
using Cig.YanFa.Account.Core;
using Cig.YanFa.Account.Core.Crm2009;

namespace Cig.YanFa.Account.Application.Crm.Expense.Dto
{
    public class ExpenseAccountExportDto :ExpenseAccount
    {
        readonly Dictionary<int, string> _dic;
        readonly Dictionary<int, string> dicExpenseType;
        public ExpenseAccountExportDto()
        {
            _dic = EnumHelper.GetValueAndDesc<CommonEnum.EnumCostAccountSplitStep>();
            dicExpenseType = EnumHelper.GetValueAndDesc<CommonEnum.EnumExpenseType>();
        }
        /// <summary>
        /// 阶段名称
        /// </summary>
        [Description("阶段名称")]
        public string StepName => _dic[Step];

        /// <summary>
        /// 费用类型
        /// </summary>
        [Description("费用类型")]
        public string ExpenseTypeName => dicExpenseType[ExpenseType];
    }
}
