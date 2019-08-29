using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using Cig.YanFa.Account.Application.Common;
using Cig.YanFa.Account.Application.Crm.Expense.Dto;
using Cig.YanFa.Account.Application.Dto;
using Cig.YanFa.Account.Core;
using Cig.YanFa.Account.Core.Crm2009;
using Cig.YanFa.Account.Repository;
using Cig.YanFa.Account.Repository.Repository;
using xLiAd.DapperEx.MsSql.Core.Helper;

namespace Cig.YanFa.Account.Application.Crm.Expense.Factory
{
    /// <summary>
    /// 抽象工厂，
    /// </summary>
    public abstract class TransExpenseAccountAbstractFactory
    {
        /// <summary>
        /// 获取 ExpenseAccount 从不同历史表中
        /// </summary>
        /// <returns></returns>
        public abstract List<ExpenseAccount> GetExpenseAccountFromHistory();

        /// <summary>
        /// 获取 历史表 分页数据
        /// </summary>
        /// <param name="expenseInput"></param>
        /// <returns></returns>
        public abstract PagedResultDto<ExpenseOut> GetHistoryDatas(ExpenseInput expenseInput);
    }

    /// <summary>
    /// 报销借款 工厂
    /// </summary>
    public class ReimbursementAndBorrowingFactory : TransExpenseAccountAbstractFactory
    {
        private CrmDBContext _crmDbContext;
        private IMapper _mapper;
        private string _expenseCode;
        private string _period;
        private readonly DapperRepositoryBase<ExpenseLineConfirmHistory, CrmDBContext> _expenseHistoryRepository;

        public ReimbursementAndBorrowingFactory(CrmDBContext crmDbContext, IMapper mapper,  string period, string expenseCode = "")
        {
            _crmDbContext = crmDbContext;
            _mapper = mapper;
            _period = period;
            _expenseCode = expenseCode;
             _expenseHistoryRepository = new DapperRepositoryBase<ExpenseLineConfirmHistory, CrmDBContext>(_crmDbContext);
        }

        public override List<ExpenseAccount> GetExpenseAccountFromHistory()
        {
            _period = (string.IsNullOrEmpty(_period) || _period.Length != 7) ? DateTime.Now.AddMonths(-1).ToString("yyyy-MM") : _period;
            Expression<Func<ExpenseLineConfirmHistory, bool>> expression = s => s.Period == _period && s.Status == 0;
            if (!string.IsNullOrEmpty(_expenseCode))
            {
                expression = expression.And(s => s.ExpenseCode == _expenseCode);
            }

            var entities = _expenseHistoryRepository.Where(expression).Select(s => _mapper.Map<ExpenseAccount>(s)).ToList();
            entities.ForEach(s => s.ExpenseType = (int)CommonEnum.EnumExpenseType.ReimbursementAndBorrowing);
            return entities;
        }

        public override PagedResultDto<ExpenseOut> GetHistoryDatas(ExpenseInput queryModel)
        {
            var expression = Expp(queryModel);
            var data = _expenseHistoryRepository.PageList(expression, queryModel.CurrentPage,
                queryModel.PageSize
                , new Tuple<Expression<Func<ExpenseLineConfirmHistory, object>>, SortOrder>(s => s.ExpenseCode, SortOrder.Descending)
                , new Tuple<Expression<Func<ExpenseLineConfirmHistory, object>>, SortOrder>(s => s.ExpenseLineNum, SortOrder.Ascending));
            var models = _mapper.Map<List<ExpenseOut>>(data.Items);
            var dic = EnumHelper.GetValueAndDesc<CommonEnum.EnumCostAccountSplitStep>();
            models.ForEach(s =>
            {
                s.Step = s.Step == 0 ? -1 : s.Step;
                s.StepName = dic[s.Step];
            });
            return new PagedResultDto<ExpenseOut>(data.Total, models); 
        }

        private Expression<Func<ExpenseLineConfirmHistory, bool>> Expp(ExpenseInput queryModel)
        {
            Expression<Func<ExpenseLineConfirmHistory, bool>> expression = s => s.Status == 0 ;
            if (!string.IsNullOrEmpty(queryModel.Period))
            {
                expression = expression.And(s => s.Period == queryModel.Period);
            }

            if (!string.IsNullOrEmpty(queryModel.POCode))
            {
                expression = expression.And(s => s.ExpenseCode == queryModel.POCode);
            }

            if (!string.IsNullOrEmpty(queryModel.ProjectName))
            {
                expression = expression.And(s => s.ProjectName.Contains(queryModel.ProjectName));
            }

            if (!string.IsNullOrEmpty(queryModel.DepartNamePath))
            {
                expression = expression.And(s => s.DepartNamePath.Contains(queryModel.DepartNamePath));
            }
            return expression;
        }
    }

    /// <summary>
    /// 固资折旧 工厂
    /// </summary>
    public class FixedAssetsDepreciationFactory : TransExpenseAccountAbstractFactory
    {
        private CrmDBContext _crmDbContext;
        private IMapper _mapper;
        private string _expenseCode;
        private string _period;
        private readonly DapperRepositoryBase<FixedAssetsDepreciationHistory, CrmDBContext> _historyRepository;

        public FixedAssetsDepreciationFactory(CrmDBContext crmDbContext, IMapper mapper, string period, string expenseCode = "")
        {
            _crmDbContext = crmDbContext;
            _mapper = mapper;
            _period = period;
            _expenseCode = expenseCode;
            _historyRepository = new DapperRepositoryBase<FixedAssetsDepreciationHistory, CrmDBContext>(_crmDbContext);
        }

        public override List<ExpenseAccount> GetExpenseAccountFromHistory()
        {

            _period = (string.IsNullOrEmpty(_period) || _period.Length != 7) ? DateTime.Now.AddMonths(-1).ToString("yyyy-MM") : _period;
            Expression<Func<FixedAssetsDepreciationHistory, bool>> expression = s => s.Period == _period && s.Status == 0;
            if (!string.IsNullOrEmpty(_expenseCode))
            {
                expression = expression.And(s => s.FixedAssetsCode == _expenseCode);
            }

            var entities = _historyRepository.Where(expression).Select(s => new ExpenseAccount()
            {
                ExpenseCode = s.FixedAssetsCode,
                ExpenseLineNum = s.LineNum,
                DepartID = s.DepartID,
                DepartNamePath = s.DepartNamePath,
                ConfirmDate = s.CreateTime,
                Period = s.Period,
                Amount = s.Amount,
                Status = s.Status,
                CreateTime = s.CreateTime,
                CreateUserID = s.CreateUserID,
                CreateUserName = s.CreateUserName,
                LastUpdateTime = s.LastUpdateTime,
                LastUpdateUserID = s.LastUpdateUserID,
                LastUpdateUserName = s.LastUpdateUserName,
                CompanyCode = s.CompanyCode,
                CompanyName = s.CompanyName,
                SourceId = 0,
                ExpenseType = (int)CommonEnum.EnumExpenseType.FixedAssetsDepreciation

            }).ToList();

            return entities;
        }

        public override PagedResultDto<ExpenseOut> GetHistoryDatas(ExpenseInput queryModel)
        {
            var expression = Expp(queryModel);
            var data = _historyRepository.PageList(expression, queryModel.CurrentPage,
                queryModel.PageSize
                , new Tuple<Expression<Func<FixedAssetsDepreciationHistory, object>>, SortOrder>(s => s.FixedAssetsCode, SortOrder.Descending)
                , new Tuple<Expression<Func<FixedAssetsDepreciationHistory, object>>, SortOrder>(s => s.LineNum, SortOrder.Ascending));

            var models = new List<ExpenseOut>();
            var dic = EnumHelper.GetValueAndDesc<CommonEnum.EnumCostAccountSplitStep>();
            foreach (var item in data.Items)
            {
                models.Add(new ExpenseOut()
                {
                    ExpenseCode = item.FixedAssetsCode,
                    ExpenseLineNum = item.LineNum,
                    Period = item.Period,
                    DepartNamePath = item.DepartNamePath,
                    Amount = item.Amount,
                    CompanyName = item.CompanyName,
                    StepName = dic[(int)CommonEnum.EnumCostAccountSplitStep.History],
                });
            }
            return new PagedResultDto<ExpenseOut>(data.Total, models); ;
        }
        private Expression<Func<FixedAssetsDepreciationHistory, bool>> Expp(ExpenseInput queryModel)
        {
            Expression<Func<FixedAssetsDepreciationHistory, bool>> expression = s => s.Status == 0;
            if (!string.IsNullOrEmpty(queryModel.Period))
            {
                expression = expression.And(s => s.Period == queryModel.Period);
            }

            if (!string.IsNullOrEmpty(queryModel.POCode))
            {
                expression = expression.And(s => s.FixedAssetsCode == queryModel.POCode);
            }

            if (!string.IsNullOrEmpty(queryModel.DepartNamePath))
            {
                expression = expression.And(s => s.DepartNamePath.Contains(queryModel.DepartNamePath));
            }
            return expression;
        }
    }
}
