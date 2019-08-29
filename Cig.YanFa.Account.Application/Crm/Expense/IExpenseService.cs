using System.Collections.Generic;
using Cig.YanFa.Account.Application.Crm.Expense.Dto;
using Cig.YanFa.Account.Application.Crm.PurchaseOrder.Dto;
using Cig.YanFa.Account.Application.Dto;
using Cig.YanFa.Account.Core.Crm2009;

namespace Cig.YanFa.Account.Application.Crm.Expense
{
    public interface IExpenseService
    {
        bool AddExpenseLineConfirmHistory(string expenseCode = "");

        bool AddExpenseAccount(int expenseType, string expenseCode = "", string period = null);

        bool ExpenseAccountDoSplit(int expenseType, string expenseCode = "", string period = null);
        PagedResultDto<ExpenseOut> GetListDatas(ExpenseInput queryModel);
        List<ExpenseAccountExportDto> GetExpenseAccountExportDtos(int expenseType, string period);
        IList<ExpenseLineConfirmHistoryExportDto> GetExpenseHistory(int expenseType,string period);
        int AddFixedAssetsDepreciation(List<FixedAssetsDepreciationHistory> histories);
        dynamic GetSummary(ExpenseInput queryModel);
    }
}