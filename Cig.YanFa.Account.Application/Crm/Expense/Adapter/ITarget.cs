using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Cig.YanFa.Account.Application.Crm.Expense.Dto;
using Cig.YanFa.Account.Application.Crm.Expense.Factory;
using Cig.YanFa.Account.Application.Dto;
using Cig.YanFa.Account.Core;
using Cig.YanFa.Account.Core.Crm2009;
using Cig.YanFa.Account.Repository;

namespace Cig.YanFa.Account.Application.Crm.Expense.Adapter
{
    /// <summary>
    /// 目标接口ITarget
    /// </summary>
    public interface ITarget
    {
        //这是源类Adapteee没有的方法
        List<ExpenseAccount> Transform();
        PagedResultDto<ExpenseOut> GetHistoryData(ExpenseInput input);
    }


    public class Adapter : ITarget
    {
        // 直接关联被适配类  
        private CrmDBContext _crmDbContext;
        private IMapper _mapper;
        private int _expenseType;
        private string _expenseCode;
        private string _period;

        // 可以通过构造函数传入具体需要适配的被适配类对象  
        public Adapter(CrmDBContext crmDbContext, IMapper mapper, int expenseType, string period, string expenseCode = "")
        {
            _crmDbContext = crmDbContext;
            _mapper = mapper;
            _expenseType = expenseType;
            _period = period;
            _expenseCode = expenseCode;

        }

        public List<ExpenseAccount> Transform()
        {
            TransExpenseAccountAbstractFactory factory;
            switch (_expenseType)
            {
                //报销借款
                case (int)CommonEnum.EnumExpenseType.ReimbursementAndBorrowing:
                    factory = new ReimbursementAndBorrowingFactory(_crmDbContext, _mapper, _period, _expenseCode);
                    break;
                //固资折旧
                case (int)CommonEnum.EnumExpenseType.FixedAssetsDepreciation:
                    factory = new FixedAssetsDepreciationFactory(_crmDbContext, _mapper, _period, _expenseCode);
                    break;
                default: return new List<ExpenseAccount>();
            }

            var models = factory.GetExpenseAccountFromHistory();
            return models;
        }

        public PagedResultDto<ExpenseOut> GetHistoryData(ExpenseInput input)
        {
            TransExpenseAccountAbstractFactory factory;
            switch (_expenseType)
            {
                //报销借款
                case (int)CommonEnum.EnumExpenseType.ReimbursementAndBorrowing:
                    factory = new ReimbursementAndBorrowingFactory(_crmDbContext, _mapper, _period, _expenseCode);
                    break;
                //固资折旧
                case (int)CommonEnum.EnumExpenseType.FixedAssetsDepreciation:
                    factory = new FixedAssetsDepreciationFactory(_crmDbContext, _mapper, _period, _expenseCode);
                    break;
                default: return new PagedResultDto<ExpenseOut>();
            }

            var models = factory.GetHistoryDatas(input);
            return models;
        }
    }

    ///// <summary>
    ///// 源类
    ///// </summary>
    //public class Adaptee
    //{
    //    public List<ExpenseAccount> ExpenseAccounts=new List<ExpenseAccount>();
    //}
}
