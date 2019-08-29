using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using Cig.YanFa.Account.Application.Common;
using Cig.YanFa.Account.Application.Crm.Expense.Adapter;
using Cig.YanFa.Account.Application.Crm.Expense.Dto;
using Cig.YanFa.Account.Application.Crm.PurchaseOrder.Dto;
using Cig.YanFa.Account.Application.Dto;
using Cig.YanFa.Account.Application.PurchaseOrder;
using Cig.YanFa.Account.Core;
using Cig.YanFa.Account.Core.Crm2009;
using Cig.YanFa.Account.Repository;
using Cig.YanFa.Account.Repository.Repository;
using Newtonsoft.Json;
using xLiAd.DapperEx.MsSql.Core.Helper;

namespace Cig.YanFa.Account.Application.Crm.Expense
{
    public class ExpenseService : IExpenseService
    {
        private readonly IMapper _mapper;
        private readonly CrmDBContext _crmDbContext;
        public readonly IUnitOfWorkFactory<CrmDBContext> _uowCrm;
        private readonly IPurchaseOrderService _purchaseOrderService;
        private readonly IDapperRepositoryBase<ExpenseAccount, CrmDBContext> _expenseAccountRepository;
        private readonly IDapperRepositoryBase<ExpenseLineConfirmHistory, CrmDBContext> _expenseHistoryRepository;
        private readonly IDapperRepositoryBase<BusinessRedirectSplitInfoVersion, CrmDBContext> _businessRedirectSplitRepository;
        private readonly IDapperRepositoryBase<DepartMentVersion, CrmDBContext> _departRepository;
        private readonly IDapperRepositoryBase<v_AchievementBusinessDepartMappingVersion, CrmDBContext> _achMappingRepository;

        private readonly IDapperRepositoryBase<FixedAssetsDepreciationHistory, CrmDBContext> _fixedAssetsDepreciationRepository;


        public ExpenseService(IDapperRepositoryBase<ExpenseAccount, CrmDBContext> expenseAccountRepository
                , IDapperRepositoryBase<ExpenseLineConfirmHistory, CrmDBContext> expenseHistoryRepository
                , IMapper mapper
                , CrmDBContext crmDbContext
                , IUnitOfWorkFactory<CrmDBContext> uowCrm
                , IPurchaseOrderService purchaseOrderService
                , IDapperRepositoryBase<BusinessRedirectSplitInfoVersion, CrmDBContext> businessRedirectSplitRepository
                , IDapperRepositoryBase<DepartMentVersion, CrmDBContext> departRepository
                , IDapperRepositoryBase<v_AchievementBusinessDepartMappingVersion, CrmDBContext> achMappingRepository
                , IDapperRepositoryBase<FixedAssetsDepreciationHistory, CrmDBContext> fixedAssetsDepreciationRepository
                )
        {
            _expenseAccountRepository = expenseAccountRepository;
            _expenseHistoryRepository = expenseHistoryRepository;
            _mapper = mapper;
            _crmDbContext = crmDbContext;
            _uowCrm = uowCrm;
            _businessRedirectSplitRepository = businessRedirectSplitRepository;
            _purchaseOrderService = purchaseOrderService;
            _departRepository = departRepository;
            _achMappingRepository = achMappingRepository;
            _fixedAssetsDepreciationRepository = fixedAssetsDepreciationRepository;
        }

        public bool AddExpenseLineConfirmHistory(string expenseCode = "")
        {
            var sql = $@"
----项目费用
WITH etcExpMain AS  (
SELECT p.CostDepartID,e.CRMProjectCode,p.ProjectName,p.ProjectType
	,e.ExpenseCode,e.EHRCompanyCode,e.EHRCompanyName
FROM dbo.Expense e
LEFT JOIN Project p ON e.CRMProjectCode = p.ProjectCode
WHERE e.CRMProjectCode IS NOT NULL
UNION ALL
--部门费用
SELECT e.CRMDepartID,NULL,NULL,NULL
	,e.ExpenseCode,e.EHRCompanyCode,e.EHRCompanyName
FROM dbo.Expense e
WHERE e.CRMProjectCode IS NULL AND e.CRMDepartID IS NOT NULL
)


INSERT INTO [dbo].[ExpenseLineConfirmHistory]  
([ExpenseCode],[ExpenseLineNum],[DepartID],[DepartNamePath]
,[ConfirmDate],[Period],[Amount]
,[Status],[MaterialClassCode],[MaterialClassName],[MaterialCode],[MaterialName]
,[ExpenseItem],[ExpenseItemName]
,[ProjectCode],[ProjectName],[ProjectType],[CompanyCode],[CompanyName]
,[CreateTime],[CreateUserID],[CreateUserName],[LastUpdateTime],[LastUpdateUserID],[LastUpdateUserName])

SELECT main.ExpenseCode,exLine.LineID AS ExpenseLineNum, dv.DepartID AS DepartId,dv.NamePath AS DepartNamePath 
		,exLine.ConfirmDate,convert(varchar(7), CONVERT(DATE,exLine.ConfirmDate) ,120) AS Period,exLine.Amount
		,exLine.Status,ma.ProductCatID AS MaterialClassCode,ma.Name AS MaterialClassName, vp.MaterialCode,exLine.MaterialName
		,exLine.ExpenseItem,exLine.ExpenseItemName
		,main.CRMProjectCode AS ProjectCode,main.ProjectName,main.ProjectType,main.EHRCompanyCode AS CompanyCode, main.EHRCompanyName AS CompanyName
		,GETDATE(),NULL,NULL,NULL,NULL,NULL
 FROM etcExpMain main
LEFT JOIN dbo.ExpenseLineStatement exLine ON exLine.ExpenseCode = main.ExpenseCode AND exLine.Status=0
LEFT JOIN dbo.DepartMentVersion dv ON (dv.DepartID=main.CostDepartID OR dv.HRDepartID=main.CostDepartID) AND (main.CostDepartID !='') AND dv.Period= CONVERT(varchar(7), DATEADD(MONTH,-1,GETDATE()),120)
LEFT JOIN dbo.v_MaterialAll vp ON vp.MaterialCode=exLine.MaterialCode  AND vp.Status = 0
LEFT JOIN  AC2010..v_ProductCategory  ma ON ma.ProductCatID=vp.MaterialClassCode 
";
            if (!string.IsNullOrEmpty(expenseCode))
            {
                sql += $@" WHERE main.ExpenseCode='{expenseCode}' AND  dv.Period= CONVERT(varchar(7), DATEADD(MONTH,-1,GETDATE()),120) AND exLine.Status = 0";
            }
            else
            {
                sql +=
                    $@" WHERE convert(varchar(7), CONVERT(DATE,exLine.ConfirmDate) ,120)=CONVERT(varchar(7), DATEADD(MONTH,-1,GETDATE()),120) AND  dv.Period= CONVERT(varchar(7), DATEADD(MONTH,-1,GETDATE()),120) AND exLine.Status = 0";
            }

            var b = _expenseHistoryRepository.ExecuteSql(sql);
            return b;
        }

        /// <summary>
        /// 费用----添加---拆分前
        /// </summary>
        /// <param name="expenseType">费用类型</param>
        /// <param name="expenseCode"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public bool AddExpenseAccount(int expenseType, string expenseCode = "", string period = null)
        {
            //根据不同的费用类型，进行适配转换
            ITarget adapter = new Adapter.Adapter(_crmDbContext, _mapper, expenseType, period, expenseCode);
            var entities = adapter.Transform();

            //如果有部门为空
            var count = entities.Count(s => string.IsNullOrEmpty(s.DepartID));
            if (count > 0)
            {
                return false;
            }

            entities.ToList().ForEach(s =>
            {
                s.Id = 0;
                s.LastUpdateTime = DateTime.Now;
                s.CreateTime = DateTime.Now;
                s.Step = (int)CommonEnum.EnumCostAccountSplitStep.BeforeSplit;
            });

            using (var uow = _uowCrm.Create())
            {
                Expression<Func<ExpenseAccount, bool>> expTem = s => s.Status == 0 && s.Period == period && s.ExpenseType == expenseType;
                if (!string.IsNullOrEmpty(expenseCode))
                {
                    expTem = expTem.And(s => s.ExpenseCode == expenseCode);
                }

                var n = _expenseAccountRepository.UpdateWhere(expTem
                    , new ExpenseAccount() { Status = -1, LastUpdateTime = DateTime.Now }, t => t.Status, t => t.LastUpdateTime);
                var i = _expenseAccountRepository.Add(entities);
                uow.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// 费用进行拆分
        /// </summary>
        /// <param name="expenseType">费用类型</param>
        /// <param name="expenseCode"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public bool ExpenseAccountDoSplit(int expenseType, string expenseCode = "", string period = null)
        {
            //计入日期
            period = (string.IsNullOrEmpty(period) || period.Length != 7) ? DateTime.Now.AddMonths(-1).ToString("yyyy-MM") : period;
            int month = (string.IsNullOrEmpty(period) || period.Length != 7) ? DateTime.Now.AddMonths(-1).Month : Convert.ToInt32(period.Split('-')[1]);
            int year = (string.IsNullOrEmpty(period) || period.Length != 7) ? DateTime.Now.AddMonths(-1).Year : Convert.ToInt32(period.Split('-')[0]);


            //更新删除
            #region 修改为删除状态
            var dic = EnumHelper.GetValueAndDesc<CommonEnum.EnumCostAccountSplitStep>();
            dic.Remove((int)CommonEnum.EnumCostAccountSplitStep.BeforeSplit);
            var steps = dic.Select(s => s.Key).ToList();
            Expression<Func<ExpenseAccount, bool>> exptem = s => s.Status == 0 && s.Period == period && s.ExpenseType == expenseType && steps.Contains(s.Step);
            if (!string.IsNullOrEmpty(expenseCode))
            {
                exptem = exptem.And(s => s.ExpenseCode == expenseCode);
            }



            var cc = _expenseAccountRepository.UpdateWhere(exptem
                , new ExpenseAccount() { Status = -1, LastUpdateTime = DateTime.Now }, t => t.Status, t => t.LastUpdateTime);

            #endregion

            var step = (int)CommonEnum.EnumCostAccountSplitStep.BeforeSplit;
            Expression<Func<ExpenseAccount, bool>> expression1 = s => s.Period == period && s.Step == step && s.ExpenseType == expenseType;
            if (!string.IsNullOrEmpty(expenseCode))
            {
                expression1 = expression1.And(s => s.ExpenseCode == expenseCode);
            }
            //获取数据
            var models1 = _expenseAccountRepository.Where(expression1);
            //拆到末级转换
            var entities2 = CommonSplit(models1, month, year);
            //插入数据库
            var num2 = _expenseAccountRepository.Add(entities2);
            if (num2 == 0)
            {
                return false;
            }

            //-------定向
            var step2 = (int)CommonEnum.EnumCostAccountSplitStep.CommonSplit;
            Expression<Func<ExpenseAccount, bool>> expression2 = s => s.Period == period && s.Step == step2 && s.ExpenseType == expenseType;
            if (!string.IsNullOrEmpty(expenseCode))
            {
                expression2 = expression2.And(s => s.ExpenseCode == expenseCode);
            }
            //获取数据
            var models2 = _expenseAccountRepository.Where(expression2);
            var entities3 = RedirectSplit(models2, year, month, period);
            //插入数据库
            var num3 = _expenseAccountRepository.Add(entities3);
            if (num3 == 0)
            {
                return false;
            }


            //-----管理架构
            var step3 = (int)CommonEnum.EnumCostAccountSplitStep.RedirectSplit;
            Expression<Func<ExpenseAccount, bool>> expression3 = s => s.Period == period && s.Step == step3 && s.ExpenseType == expenseType;
            if (!string.IsNullOrEmpty(expenseCode))
            {
                expression3 = expression3.And(s => s.ExpenseCode == expenseCode);
            }

            var hasRedirect = _expenseAccountRepository.Where(expression3);
            var achs = _achMappingRepository.Where(s => s.Status == 0 && s.Month == month && s.Year == year);

            foreach (var item in hasRedirect)
            {
                var ach = achs.FirstOrDefault(s => s.BusinessDepartID == item.DepartID);

                item.SourceId = item.Id;
                item.Id = 0;
                item.Step = (int)CommonEnum.EnumCostAccountSplitStep.BusinessToAchDepartSplit;
                item.DepartID = ach?.AchievementDepartID;
                item.DepartNamePath = ach?.AchievementDepartName;
                item.CreateTime = DateTime.Now;
            }
            var num4 = _expenseAccountRepository.Add(hasRedirect);

            if (num4 == 0)
            {
                return false;
            }

            return num2 + num3 + num4 > 0;

        }

        /// <summary>
        /// 获取核算 列表信息
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public PagedResultDto<ExpenseOut> GetListDatas(ExpenseInput queryModel)
        {
            if (queryModel.Step==(int)CommonEnum.EnumCostAccountSplitStep.History)
            {
                //根据不同的费用类型，进行适配转换
                ITarget adapter = new Adapter.Adapter(_crmDbContext, _mapper, queryModel.ExpenseType, queryModel.Period);
                var entities = adapter.GetHistoryData(queryModel);
                return entities;
            }

            var expression = Expression(queryModel);
            var data = _expenseAccountRepository.PageList(expression, queryModel.CurrentPage,
                 queryModel.PageSize
                 , new Tuple<Expression<Func<ExpenseAccount, object>>, SortOrder>(s => s.ExpenseCode, SortOrder.Descending)
                 , new Tuple<Expression<Func<ExpenseAccount, object>>, SortOrder>(s => s.ExpenseLineNum, SortOrder.Ascending));
            var models = _mapper.Map<List<ExpenseOut>>(data.Items);
            var dic = EnumHelper.GetValueAndDesc<CommonEnum.EnumCostAccountSplitStep>();
            models.ForEach(s => s.StepName = dic[s.Step]);
            return new PagedResultDto<ExpenseOut>(data.Total, models);
        }

        public List<ExpenseAccountExportDto> GetExpenseAccountExportDtos(int expenseType, string period)
        {
            Expression<Func<ExpenseAccount, bool>> expression = s => s.Status == 0 && s.ExpenseType == expenseType;
            if (!string.IsNullOrEmpty(period))
            {
                expression = expression.And(s => s.Period == period);
            }

            var models = _expenseAccountRepository.Where(expression).Select(s => _mapper.Map<ExpenseAccountExportDto>(s)).ToList();
            return models;
        }

        public IList<ExpenseLineConfirmHistoryExportDto> GetExpenseHistory(int expenseType,string period)
        {
            //根据不同的费用类型，进行适配转换
            ITarget adapter = new Adapter.Adapter(_crmDbContext, _mapper, expenseType, period);
            var entities = adapter.Transform();

            var models = _mapper.Map<List<ExpenseLineConfirmHistoryExportDto>>(entities);
            return models;

            //var models = _expenseHistoryRepository.Where(s => s.Period == period).Select(s => _mapper.Map<ExpenseLineConfirmHistoryExportDto>(s)).ToList();
            //return models;
        }

        /// <summary>
        /// 添加固资折旧历史表
        /// </summary>
        /// <param name="histories"></param>
        /// <returns></returns>
        public int AddFixedAssetsDepreciation(List<FixedAssetsDepreciationHistory> histories)
        {
            var period = histories.Select(s => s.Period).FirstOrDefault();
            using (var uow = _uowCrm.Create())
            {
                Expression<Func<FixedAssetsDepreciationHistory, bool>> expTem = s => s.Status == 0 && s.Period == period;

                var n = _fixedAssetsDepreciationRepository.UpdateWhere(expTem
                    , new FixedAssetsDepreciationHistory() { Status = -1, LastUpdateTime = DateTime.Now }, t => t.Status, t => t.LastUpdateTime);
                var i = _fixedAssetsDepreciationRepository.Add(histories);
                uow.SaveChanges();
                return i;
            }
        }

        /// <summary>
        /// 获取 汇总钱数和
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        public dynamic GetSummary(ExpenseInput queryModel)
        {
            var expression = Expression(queryModel);
            var tem = _expenseAccountRepository.Where(expression).GroupBy(s => s.Step)
                .Select(s => new { step = s.Key, summary = s.Sum(t => t.Amount) }).ToList();
            return tem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        private List<ExpenseAccount> CommonSplit(List<ExpenseAccount> models, int month, int year)
        {
            var lastDepInfo = new List<ExpenseAccount>();
            foreach (var item in models)
            {
                //获取公共拆分比例
                var commonVersion = _purchaseOrderService.GetCommonSplit(item.DepartID, month, year);

                var b = commonVersion.Count(s => s.SourceDepartID == item.DepartID) == 0;
                //末级
                if (b)
                {
                    item.SourceId = item.Id;
                    item.Id = 0;
                    item.CreateTime = DateTime.Now;
                    item.Step = (int)CommonEnum.EnumCostAccountSplitStep.CommonSplit;
                    item.Formula += $"公共：计入---->";
                    lastDepInfo.Add(item);
                }
                else
                {
                    int i = 1;
                    decimal sum = 0;
                    decimal allAmount = item.Amount;
                    int count = commonVersion.Count(s => s.SourceDepartID == item.DepartID);
                    //循环 公共拆分给谁
                    foreach (var common in commonVersion.Where(s => s.SourceDepartID == item.DepartID).OrderBy(s=>s.SplitRate))
                    {
                        var toDep = common.ToDepartID; //公共成本拆给谁；
                        var toRate = common.SplitRate; //拆给谁的 比例

                        var afterAmount = decimal.Round(item.Amount * toRate, 2); //拆分后的钱
                        var tem = JsonConvert.DeserializeObject<ExpenseAccount>(JsonConvert.SerializeObject(item));
                        tem.DepartID = toDep;
                        tem.DepartNamePath = common.ToDepartName;
                        tem.Amount = i == count ? allAmount - sum : afterAmount;

                        tem.Ratio = toRate;
                        tem.SourceId = item.Id;
                        tem.Step = (int)CommonEnum.EnumCostAccountSplitStep.CommonSplit;
                        tem.Formula += i == count ? $"公共：({allAmount}-{sum})---->" : $"公共：({item.Amount}*{toRate})---->";

                        lastDepInfo.Add(tem);
                        i++;
                        sum += afterAmount;
                    }
                }
            }

            return lastDepInfo;
        }

        private List<ExpenseAccount> RedirectSplit(List<ExpenseAccount> models, int year, int month, string period)
        {
            var redirectList = new List<ExpenseAccount>();
            var businessRedirectSplitInfos = _businessRedirectSplitRepository.Where(s => s.Month == month && s.Year == year);
            var departsVersion = _departRepository.Where(s => s.Period == period);

            foreach (var item in models)
            {
                var count = businessRedirectSplitInfos.Count(s => s.FromDepartId == item.DepartID);
                if (count == 0)
                {
                    item.SourceId = item.Id;
                    item.Id = 0;
                    item.CreateTime = DateTime.Now;
                    item.Step = (int)CommonEnum.EnumCostAccountSplitStep.RedirectSplit;
                    item.Formula += $"定向：计入---->";
                    redirectList.Add(item);
                }
                else
                {
                    int i = 1;
                    decimal sum = 0;
                    decimal allAmount = item.Amount;
                    foreach (var redict in businessRedirectSplitInfos.Where(s => s.FromDepartId == item.DepartID).OrderBy(s=>s.Split))
                    {
                        var toRate = string.IsNullOrEmpty(redict.Split)
                            ? 0
                            : decimal.Round(Convert.ToDecimal(redict.Split) * (decimal)0.01, 2);
                        var temAmount = decimal.Round(item.Amount * toRate, 2); //定向后的钱

                        var tem = JsonConvert.DeserializeObject<ExpenseAccount>(JsonConvert.SerializeObject(item));
                        tem.DepartID = redict.ToDepartId;
                        tem.DepartNamePath = departsVersion.Where(s => s.DepartID == redict.ToDepartId).Select(s => s.NamePath).FirstOrDefault();
                        tem.Amount = i == count ? allAmount - sum : temAmount;

                        tem.Ratio = toRate;
                        tem.SourceId = item.Id;
                        tem.Step = (int)CommonEnum.EnumCostAccountSplitStep.RedirectSplit;
                        tem.Formula += i == count ? $"定向：({allAmount}-{sum})---->" : $"定向：({item.Amount}*{toRate})---->";

                        redirectList.Add(tem);
                        i++;
                        sum += temAmount;
                    }
                }
            }

            return redirectList;
        }

        private static Expression<Func<ExpenseAccount, bool>> Expression(ExpenseInput queryModel)
        {
            Expression<Func<ExpenseAccount, bool>> expression = s => s.Status == 0 && s.ExpenseType == queryModel.ExpenseType;
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
            if (queryModel.Step != 0)
            {
                expression = expression.And(s => s.Step == queryModel.Step);
            }
            return expression;
        }

    }
}
