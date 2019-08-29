using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using AutoMapper;
using Cig.YanFa.Account.Application.Common;
using Cig.YanFa.Account.Application.Crm.CommonData.Dto;
using Cig.YanFa.Account.Application.Crm.HeSuan.Dto;
using Cig.YanFa.Account.Application.Crm.PurchaseOrder.Dto;
using Cig.YanFa.Account.Application.Dto;
using Cig.YanFa.Navigation;
using Cig.YanFa.Account.Application.PurchaseOrder.Dto;
using Cig.YanFa.Account.Core;
using Cig.YanFa.Account.Core.Crm2009;
using Cig.YanFa.Account.Repository;
using Cig.YanFa.Account.Repository.Repository;
using Cig.YanFa.Account.Repository.Repository.Interface;
using Newtonsoft.Json;
using xLiAd.DapperEx.MsSql.Core.Helper;

namespace Cig.YanFa.Account.Application.PurchaseOrder
{
    public class PurchaseOrderService : YanFaAppServiceBase, IPurchaseOrderService
    {
        #region Injection
        private readonly IMapper _mapper;
        public readonly IUnitOfWorkFactory<CrmDBContext> _uowCrm;
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IDapperRepositoryBase<PurchaseOrderLineConfirmHistory, CrmDBContext> _polchRepository;
        private readonly IDapperRepositoryBase<CostAccountConfig, CrmDBContext> _costAccountConfigRepository;
        private readonly IDapperRepositoryBase<PurchaseRequirementDeptShareRatioVersion, CrmDBContext> _purchaseRequirementDeptShareRatioVersionRepository;
        private readonly IDapperRepositoryBase<CostAccount, CrmDBContext> _costAccountRepository;
        private readonly IDapperRepositoryBase<BudgetTargetLeaderCostSplitVersion, CrmDBContext> _costSplitVersionRepository;
        private readonly ICommonDataService _commonDataService;
        private readonly IDapperRepositoryBase<BusinessRedirectSplitInfoVersion, CrmDBContext> _businessRedirectSplitRepository;
        private readonly IDapperRepositoryBase<v_AchievementBusinessDepartMappingVersion, CrmDBContext> _achMappingRepository;
        private readonly IDapperRepositoryBase<DepartMentVersion, CrmDBContext> _departRepository;



        public PurchaseOrderService(IPurchaseOrderRepository purchaseOrderRepository
                , IUnitOfWorkFactory<CrmDBContext> uowCrm
                , IDapperRepositoryBase<PurchaseOrderLineConfirmHistory, CrmDBContext> polchRepository
                , IDapperRepositoryBase<CostAccountConfig, CrmDBContext> costAccountConfigRepository
                , IDapperRepositoryBase<PurchaseRequirementDeptShareRatioVersion, CrmDBContext> purchaseRequirementDeptShareRatioVersionRepository
                , ICommonDataService commonDataService
                , IDapperRepositoryBase<CostAccount, CrmDBContext> costBeforeSplitRepository
                , IDapperRepositoryBase<BudgetTargetLeaderCostSplitVersion, CrmDBContext> costSplitVersionRepository
                , IDapperRepositoryBase<BusinessRedirectSplitInfoVersion, CrmDBContext> businessRedirectSplitRepository
                , IDapperRepositoryBase<v_AchievementBusinessDepartMappingVersion, CrmDBContext> achMappingRepository
                , IDapperRepositoryBase<DepartMentVersion, CrmDBContext> departRepository
                , IMapper mapper)
        {
            _purchaseOrderRepository = purchaseOrderRepository;
            _polchRepository = polchRepository;
            _costAccountConfigRepository = costAccountConfigRepository;
            _purchaseRequirementDeptShareRatioVersionRepository = purchaseRequirementDeptShareRatioVersionRepository;
            _commonDataService = commonDataService;
            _costAccountRepository = costBeforeSplitRepository;
            _costSplitVersionRepository = costSplitVersionRepository;
            _businessRedirectSplitRepository = businessRedirectSplitRepository;
            _achMappingRepository = achMappingRepository;
            _departRepository = departRepository;
            _uowCrm = uowCrm;
            _mapper = mapper;
        }
        #endregion

        public List<PurchaseOrderLineConfirmHistory> GetAll()
        {
            var sql = $@"
                SELECT po.POCode,pol.POLineNum,poac.ActualPeriod as Period ,poac.ActualAmount as Amount,
                pol.MaterialClassId as MaterialClassCode,mc.MaterialClassName,pol.MaterialId as MaterialCode,m.MaterialName,pol.ExpenseItem,dic.DictName as ExpenseItemName,
                pro.ProjectCode,ISNULL(pro.ProjectType,0) as ProjectType,pro.ProjectName,po.CompanyCode,ci.CompanyName,po.SupplierId,po.SupplierName,
                ISNULL(po.ContentType,0) ContentType,
                (select dictname from CRM2009..DictInfo where dictid = po.ContentType) as ContentTypeName,
                po.OrderEnum,
                (SELECT TOP 1 Remark FROM Relevance WHERE Type=5 AND SubType=po.OrderEnum) AS OrderEnumName,
                 pro.CostDepartID as DepartID ,dp.NamePath as DepartNamePath,poac.ConfirmationID
                 --新增 2019年6月25日16:31:00
                 ,pr.RequirementCode AS PRCode ,pr.DepartID AS PRDepartID, pdp.NamePath AS PRDepartNamePath
                 ,po.CreateTime AS POCreateTime ,pol.Amount AS POLineAmount , pol.BeginTime AS POLineBeginTime,pol.EndTime AS POLineEndTime
                 ,pro.CustID AS CustID ,cust.CustName AS CustName,pol.TaxCode
                FROM CRM2009.dbo.PurchaseOrder po
                LEFT JOIN CRM2009..PurchaseExecution pe ON pe.POCode=po.POCode
                LEFT JOIN CRM2009..PurchaseRequirement pr ON pe.RequireCode=pr.RequirementCode
                LEFT JOIN CRM2009..v_DepartMent pdp ON pdp.DepartID = pr.DepartID

                LEFT JOIN CRM2009.dbo.PurchaseOrderLine pol ON pol.POCode=po.POCode AND pol.Status=0
                LEFT JOIN CRM2009.dbo.Project pro ON pro.ProjectCode=pol.ProjectCode AND pro.Status=0
                LEFT JOIN CRM2009..CustInfo cust ON cust.CustID=pro.CustID AND cust.Status=0
                LEFT JOIN CRM2009.dbo.POActualConfirmation poac ON poac.POCode = po.POCode AND pol.POLineNum=poac.POLineNum and poac.ConfirmationID!=0
                LEFT JOIN CRM2009..MaterialClass mc on mc.MaterialClassCode = pol.MaterialClassId
                LEFT JOIN CRM2009..v_MaterialAll m on m.MaterialCode = pol.MaterialId
                LEFT JOIN CRM2009..DictInfo dic on dic.DictID = pol.ExpenseItem and dic.DictType = 710
                LEFT JOIN CRM2009..CompanyInfo ci on ci.CompanyCode = po.CompanyCode
                LEFT JOIN CRM2009..v_DepartMent dp on dp.DepartID = pro.CostDepartID
                WHERE po.ApproveStatus>-1 and  poac.Status=0 
                --AND po.POCode='PO0116315' 
                AND poac.ActualPeriod=convert(varchar(7), DATEADD(MONTH,-1,GETDATE()),120)
                ";
            var POs = _purchaseOrderRepository.QueryBySql<PurchaseOrderLineConfirmHistory>(sql, param: null).ToList();
            return POs;
        }

        public bool AddPurchaseOrderLineConfirmHistorys(string poCode)
        {
            var POs = GetAll();
            if (!string.IsNullOrEmpty(poCode))
            {
                POs = POs.Where(s => s.POCode == poCode).ToList();
            }

            POs.ForEach(s =>
            {
                s.CreateTime = DateTime.Now;
                //TODO lhl 创建人 
                //s.CreateUserID=
                //s.CreateUserName=
            });

            var num = _polchRepository.Add(POs);
            return num > 0;
        }


        public bool AddPurchaseRequirementDeptShareRatioVersionRepository(string poCode)
        {
            var sql = $@"
SELECT distinct po.POCode ,po.Period,po.POLineNum
,pr.RequirementCode as PRCode
,pe.ExecutionCode as PECode
,prDR.DeptID as DepartId
,(select NamePath from v_DepartMent where DepartID = prDR.DeptID) as DepartNamePath
 ,ISNULL(prDR.ShareProportion,0) ShareProportion
FROM  CRM2009.dbo.PurchaseOrderLineConfirmHistory po
inner JOIN CRM2009.dbo.PurchaseExecution pe ON pe.POCode=po.POCode and pe.Status=0
inner JOIN CRM2009.dbo.PurchaseRequirement pr ON pr.RequirementCode=pe.RequireCode and pr.status = 0
inner JOIN CRM2009.dbo.PurchaseRequirementDeptShareRatio prDR ON prdr.PRCode=pe.RequireCode AND prDR.State=0
WHERE 1=1 
--AND po.POCode='PO0102878'  
AND po.Period= convert(varchar(7), DATEADD(MONTH,-1,GETDATE()),120)--上个月
";
            var entities = _purchaseRequirementDeptShareRatioVersionRepository.QueryBySql<PurchaseRequirementDeptShareRatioVersion>(sql, param: null).ToList();

            if (!string.IsNullOrEmpty(poCode))
            {
                entities = entities.Where(s => s.POCode == poCode).ToList();
            }

            var num = _purchaseRequirementDeptShareRatioVersionRepository.Add(entities);
            return num > 0;
        }


        public bool AddCostAccountBeforeSplit(string poCode = "", string period = null)
        {
            //计入日期
            if (string.IsNullOrEmpty(period) || period.Length != 7)
            {
                period = DateTime.Now.AddMonths(-1).ToString("yyyy-MM");
            }


            #region 获取范围数据

            Expression<Func<PurchaseOrderLineConfirmHistory, bool>> histExp = s => s.Status == 0 && s.Period == period;
            Expression<Func<PurchaseRequirementDeptShareRatioVersion, bool>> verExp = s =>
                s.Status == 0 && s.Period == period;
            if (!string.IsNullOrEmpty(poCode))
            {
                histExp = histExp.And(s => s.POCode == poCode);
                verExp = verExp.And(s => s.POCode == poCode);
            }

            var historys = _polchRepository.Where(histExp);
            var versions = _purchaseRequirementDeptShareRatioVersionRepository.Where(verExp);
            #endregion

            var configs = _costAccountConfigRepository.Where(s => s.Status == 0);
            var dicInfos = _commonDataService.GetDicInfos();

            //1.历史记录表
            var models = new List<PurchaseOrderLineConfirmHistory>();
            foreach (var confirmHistory in historys)
            {
                var count = versions.Count(s => s.POCode == confirmHistory.POCode && s.POLineNum == confirmHistory.POLineNum);
                if (count > 0)
                {
                    int i = 1;
                    decimal sum = 0;
                    decimal allAmount = confirmHistory.Amount;
                    foreach (var ver in versions.Where(s => s.POCode == confirmHistory.POCode && s.POLineNum == confirmHistory.POLineNum))
                    {
                        var toDepartId = ver.DepartID;
                        var toRate = ver.ShareProportion * (decimal)0.01;

                        var tem = Newtonsoft.Json.JsonConvert.DeserializeObject<PurchaseOrderLineConfirmHistory>(Newtonsoft.Json.JsonConvert.SerializeObject(confirmHistory));// JsonConvert (confirmHistory);
                        tem.DepartID = toDepartId;
                        tem.DepartNamePath = ver.DepartNamePath;
                        tem.Amount = i == count ? allAmount - sum : decimal.Round(confirmHistory.Amount * toRate, 2);//满足总和一致

                        tem.Ratio = toRate;
                        tem.Formula += i == count ? $"拆分前：({allAmount}-{sum})---->" : $"拆分前：({confirmHistory.Amount}*{toRate})---->";

                        models.Add(tem);
                        i++;
                        sum += tem.Amount;
                    }
                }
                else
                {
                    models.Add(confirmHistory);
                }
            }

            var costAccounts = new List<CostAccount>();
            foreach (var item in models)
            {
                var tem = configs.Where(s=>(s.CompanyCode==item.CompanyCode||s.CompanyCode=="-99")
                                                          &&s.ContentType==item.ContentType
                                                          &&s.ProductCat1==item.MaterialClassCode
                                                          &&(s.ProductCat2==item.MaterialCode||s.ProductCat2=="-99")).ToList();
                var costAccountConfig = tem.OrderByDescending(s=>s.CompanyCode).ThenByDescending(s=>s.ProductCat2).FirstOrDefault()??new CostAccountConfig();
                var ca = new CostAccount
                {
                    POCode = item.POCode,
                    POLineNum = item.POLineNum,
                    DepartID = item.DepartID,
                    DepathNamePath = item.DepartNamePath,
                    Period = item.Period,
                    Amount = item.Amount,
                    MaterialClassCode = item.MaterialClassCode,
                    MaterialClassName = item.MaterialClassName,
                    MaterialCode = item.MaterialCode,
                    MaterialName = item.MaterialName,
                    ExpenseItem = item.ExpenseItem,
                    ExpenseItemName = item.ExpenseItemName,
                    ProjectCode = item.ProjectCode,
                    ProjectName = item.ProjectName,
                    CompanyCode = item.CompanyCode,
                    CompanyName = item.CompanyName,
                    SupplierID = item.SupplierID,
                    SupplierName = item.SupplierName,
                    ContentType = item.ContentType,
                    ContentTypeName = item.ContentTypeName,
                    CostAccountConfigID = costAccountConfig.Id,
                    SubjectCode = costAccountConfig.PayOutType1,
                    SubjectName = costAccountConfig.PayOutType1 ==0?null:dicInfos.FirstOrDefault(s => s.DictId == costAccountConfig.PayOutType1)?.DictName,
                    SecondSubjectCode = costAccountConfig.PayOutType2,
                    SecondSubjetName = costAccountConfig.PayOutType2 == 0 ? null : costAccountConfig.PayOutType2==-99?"缺省" :dicInfos.FirstOrDefault(s => s.DictId == costAccountConfig.PayOutType2)?.DictName,
                    Status = 0,
                    CreateTime = DateTime.Now,

                    Ratio = item.Ratio,
                    Step = (int) CommonEnum.EnumCostAccountSplitStep.BeforeSplit,
                    Formula = string.IsNullOrEmpty(item.Formula) ? "拆分前：计入---->" : item.Formula,


                    PRCode = item.PRCode,
                    PRDepartID = item.PRDepartID,
                    PRDepartNamePath = item.PRDepartNamePath,
                    POCreateTime = item.POCreateTime,
                    POLineAmount = item.POLineAmount,
                    POLineBeginTime = item.POLineBeginTime,
                    POLineEndTime = item.POLineEndTime,
                    CustID = item.CustID,
                    CustName = item.CustName,
                    OrderEnum = item.OrderEnum,
                    OrderEnumName = item.OrderEnumName,
                    TaxCode = item.TaxCode

                };
                costAccounts.Add(ca);
            }

           

            using (var uow = _uowCrm.Create())
            {
                //更新状态
                Expression<Func<CostAccount, bool>> delExpression = s => s.Status == 0 && s.Period == period;
                if (!string.IsNullOrEmpty(poCode))
                {
                    delExpression = delExpression.And(s => s.POCode == poCode);
                }
                var cc = _costAccountRepository.UpdateWhere(delExpression, new CostAccount() { Status = -1, LastUpdateTime = DateTime.Now }, t => t.Status, t => t.LastUpdateTime);
                var num = _costAccountRepository.Add(costAccounts);
                uow.SaveChanges();
                return true;
            }
        }

        public List<CostAccount> GetAllBeforeSplits()
        {
            var models = _costAccountRepository.Where(s => s.Status == 0).ToList();
            return models;
        }

        public bool DoSplit(string poCode = "", string period = null)
        {
            /**
             * 1、获取所有拆分前数据             * 
             * 2、获取定向数据 拆分比例
             */
            //计入日期
            period = (string.IsNullOrEmpty(period) || period.Length != 7) ? DateTime.Now.AddMonths(-1).ToString("yyyy-MM") : period;
            int month = (string.IsNullOrEmpty(period) || period.Length != 7) ? DateTime.Now.AddMonths(-1).Month : Convert.ToInt32(period.Split('-')[1]);
            int year = (string.IsNullOrEmpty(period) || period.Length != 7) ? DateTime.Now.AddMonths(-1).Year : Convert.ToInt32(period.Split('-')[0]);

            //更新状态
            var dic = EnumHelper.GetValueAndDesc<CommonEnum.EnumCostAccountSplitStep>();
            dic.Remove((int)CommonEnum.EnumCostAccountSplitStep.BeforeSplit);
            var steps = dic.Select(s => s.Key).ToList();

            Expression<Func<CostAccount, bool>> delExpression = s => s.Status == 0 && s.Period == period && steps.Contains(s.Step);
            if (!string.IsNullOrEmpty(poCode))
            {
                delExpression = delExpression.And(s => s.POCode == poCode);
            }

            var cc = _costAccountRepository.UpdateWhere(delExpression, new CostAccount() { Status = -1, LastUpdateTime = DateTime.Now }, t => t.Status, t => t.LastUpdateTime);

            //1
            var be = (int)CommonEnum.EnumCostAccountSplitStep.BeforeSplit;
            var befores = _costAccountRepository.Where(s => s.Period == period && s.Status == 0 && s.Step == be)
                .WhereIf(!string.IsNullOrEmpty(poCode), s => s.POCode == poCode)
                .ToList();

            //2
            var businessRedirectSplitInfos = _businessRedirectSplitRepository.Where(s => s.Month == month && s.Year == year);

            #region 公共成本拆分
            //拆分
            //拆到末级集合；
            var lastDepInfo = new List<CostAccount>();
            //循环递归---到末级
            CommonSplit(befores, lastDepInfo, month, year);
            var commonSplits = _costAccountRepository.Add(lastDepInfo);
            if (commonSplits == 0)
            {
                return false;
            }
            #endregion

            #region 定向拆分
            //定向
            //查询出所有已经 完成公共成本拆分的
            var ss = (int)CommonEnum.EnumCostAccountSplitStep.CommonSplit;
            var hasCommonSplits = _costAccountRepository.Where(s => s.Step == ss && s.Period == period && s.Status == 0)
                .WhereIf(!string.IsNullOrEmpty(poCode), s => s.POCode == poCode)
                .ToList();
            var departsVersion = _departRepository.Where(s => s.Period == period);
            var redirectList = new List<CostAccount>();
            foreach (var item in hasCommonSplits)
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

                        var tem = JsonConvert.DeserializeObject<CostAccount>(JsonConvert.SerializeObject(item));
                        tem.DepartID = redict.ToDepartId;
                        tem.DepathNamePath = departsVersion.Where(s => s.DepartID == redict.ToDepartId).Select(s => s.NamePath).FirstOrDefault();
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
            var num = _costAccountRepository.Add(redirectList);
            if (num == 0)
            {
                return false;
            }
            #endregion

            #region 业务--》管理
            //业务---》管理关系
            var ss1 = (int)CommonEnum.EnumCostAccountSplitStep.RedirectSplit;
            var hasRedirect = _costAccountRepository.Where(s => s.Step == ss1 && s.Period == period && s.Status == 0)
                .WhereIf(!string.IsNullOrEmpty(poCode), s => s.POCode == poCode)
                .ToList();
            var achs = _achMappingRepository.Where(s => s.Status == 0 && s.Month == month && s.Year == year);

            foreach (var item in hasRedirect)
            {
                var ach = achs.FirstOrDefault(s => s.BusinessDepartID == item.DepartID);

                item.SourceId = item.Id;
                item.Id = 0;
                item.Step = (int)CommonEnum.EnumCostAccountSplitStep.BusinessToAchDepartSplit;
                item.DepartID = ach?.AchievementDepartID;
                item.DepathNamePath = ach?.AchievementDepartName;
                item.CreateTime = DateTime.Now;
            }
            var n = _costAccountRepository.Add(hasRedirect);

            if (n == 0)
            {
                return false;
            }
            #endregion

            return true;
        }

        public List<CommonSplitDto> GetCommonSplit(string sourceDepartId, int month, int year)
        {
            var sql = $@"
SELECT DISTINCT				
		sp.SourceDepartID AS SourceDepartID
		,cc.TargetDepart AS FromDepartID
		,vaFrom.NamePath AS FromDepartName		

		,sp.DepartID AS ToDepartID
		,vaTo.NamePath AS ToDepartName					
		, sp.SplitRate
		
	FROM dbo.CommonCastVersion cc
     INNER JOIN BudgetTargetLeaderCostSplitVersion sp ON cc.TargetCode=sp.TargetCode AND sp.Month=@month AND sp.Year=@year
     LEFT JOIN dbo.DepartmentVersion vaFrom ON vaFrom.DepartID=cc.TargetDepart AND vaFrom.Month=@month AND vaFrom.Year=@year
     LEFT JOIN dbo.DepartmentVersion vaTo ON vaTo.DepartID=sp.DepartID AND vaTo.Month=@month AND vaTo.Year=@year
    WHERE cc.IsDelete=0 AND cc.Month=@month AND cc.Year=@year
	AND sp.SourceDepartID=@SourceDepartID
    AND (SELECT COUNT(*) FROM dbo.DepartMentVersion WHERE month=@month AND year=@year and DepartID=sp.DepartID AND Pid=cc.TargetDepart AND Status=1)=0
";
            //圈定一定范围
            var models = _costSplitVersionRepository.QueryBySql<CommonSplitDto>(sql, new { SourceDepartID = sourceDepartId, month, year }).ToList();

            var ss2 = models.Where(s => s.FromDepartID == sourceDepartId && s.SplitRate != 0).ToList();
            var yy = new List<CommonSplitDto>();
            tt(models, ss2, yy);

            return yy;
        }

        public PagedResultDto<CostAccountOut> GetCostAccountByPage(CostAccountInput queryModel)
        {
            var dic = EnumHelper.GetValueAndDesc<CommonEnum.EnumCostAccountSplitStep>();

            #region expression表达式
            var expression = Expression(queryModel);
            #endregion
            var pageModels = _costAccountRepository.PageListSelect(expression, s => new CostAccountOut()
            {
                POCode = s.POCode,
                POLineNum = s.POLineNum,
                DepartID = s.DepartID,
                DepartNamePath = s.DepathNamePath,
                Period = s.Period,
                Amount = s.Amount,
                ProjectName = s.ProjectName,
                CompanyCode = s.CompanyCode,
                CompanyName = s.CompanyName,
                ContentType = s.ContentType,
                ContentTypeName = s.ContentTypeName,
                SubjectCode = s.SubjectCode,
                SubjectName = s.SubjectName,
                SecondSubjectCode = s.SecondSubjectCode,
                SecondSubjetName = s.SecondSubjetName,
                Step = s.Step,
                StepName = dic[s.Step]
            }, queryModel.CurrentPage, queryModel.PageSize
                , new Tuple<Expression<Func<CostAccount, object>>, SortOrder>(s => s.POCode, SortOrder.Descending)
                , new Tuple<Expression<Func<CostAccount, object>>, SortOrder>(s => s.POLineNum, SortOrder.Ascending));

            return new PagedResultDto<CostAccountOut>(pageModels.Total, pageModels.Items);
        }

        public List<PurchaseOrderLineConfirmHistoryExportDto> GetPurchaseOrderLineConfirmHistory(string period)
        {
            Expression<Func<PurchaseOrderLineConfirmHistory, bool>> expression = s => s.Status == 0;
            if (!string.IsNullOrEmpty(period))
            {
                expression = expression.And(s => s.Period == period);
            }
            var models = _polchRepository.Where(expression).Select(s => _mapper.Map<PurchaseOrderLineConfirmHistoryExportDto>(s)).ToList();
            return models;
        }

        public List<PurchaseRequirementDeptShareRatioVersionExportDto> GetPurchaseRequirementDeptShareRatioVersions(string period)
        {
            Expression<Func<PurchaseRequirementDeptShareRatioVersion, bool>> expression = s => s.Status == 0;
            if (!string.IsNullOrEmpty(period))
            {
                expression = expression.And(s => s.Period == period);
            }

            var datas = _purchaseRequirementDeptShareRatioVersionRepository.Where(expression).ToList();
            var models = _mapper.Map<List<PurchaseRequirementDeptShareRatioVersionExportDto>>(datas);
            return models;
        }

        public List<CostAccountExportDto> GetCostAccountExportDtos(string period)
        {
            Expression<Func<CostAccount, bool>> expression = s => s.Status == 0;
            if (!string.IsNullOrEmpty(period))
            {
                expression = expression.And(s => s.Period == period);
            }

            var models = _costAccountRepository.Where(expression).Select(s => _mapper.Map<CostAccountExportDto>(s)).ToList();
            return models;
        }

        public List<v_AchievementBusinessDepartMappingVersionExportDto> GetBusinessToAchMappingVersion(string period)
        {
            int month = (string.IsNullOrEmpty(period) || period.Length != 7) ? DateTime.Now.AddMonths(-1).Month : Convert.ToInt32(period.Split('-')[1]);
            int year = (string.IsNullOrEmpty(period) || period.Length != 7) ? DateTime.Now.AddMonths(-1).Year : Convert.ToInt32(period.Split('-')[0]);

            Expression<Func<v_AchievementBusinessDepartMappingVersion, bool>> expression = s => s.Status == 0;
            if (!string.IsNullOrEmpty(period))
            {
                expression = expression.And(s => s.Month == month && s.Year == year);
            }
            var tem = _achMappingRepository.Where(expression).ToList();
            var models = _mapper.Map<List<v_AchievementBusinessDepartMappingVersionExportDto>>(tem);
            return models;
        }


        private void CommonSplit(List<CostAccount> costAccounts, List<CostAccount> lastDepInfo, int month, int year)
        {
            //循环所有 拆分前
            foreach (var before in costAccounts)
            {
                var commonVersion = GetCommonSplit(before.DepartID, month, year);

                var b = commonVersion.Count(s => s.SourceDepartID == before.DepartID) == 0;
                //末级
                if (b)
                {
                    before.SourceId = before.Id;
                    before.Id = 0;
                    before.CreateTime = DateTime.Now;
                    before.Step = (int)CommonEnum.EnumCostAccountSplitStep.CommonSplit;
                    before.Formula += $"公共：计入---->";
                    lastDepInfo.Add(before);
                }
                else
                {
                    int i = 1;
                    decimal sum = 0;
                    decimal allAmount = before.Amount;
                    int count = commonVersion.Count(s => s.SourceDepartID == before.DepartID);
                    //循环 公共拆分给谁
                    foreach (var common in commonVersion.Where(s => s.SourceDepartID == before.DepartID).OrderBy(s=>s.SplitRate))
                    {
                        var toDep = common.ToDepartID; //公共成本拆给谁；
                        var toRate = common.SplitRate; //拆给谁的 比例

                        var afterAmount = decimal.Round(before.Amount * toRate, 2); //拆分后的钱
                        var tem = JsonConvert.DeserializeObject<CostAccount>(JsonConvert.SerializeObject(before));
                        tem.DepartID = toDep;
                        tem.DepathNamePath = common.ToDepartName;
                        tem.Amount = i == count ? allAmount - sum : afterAmount;

                        tem.Ratio = toRate;
                        tem.SourceId = before.Id;
                        tem.Step = (int)CommonEnum.EnumCostAccountSplitStep.CommonSplit;
                        tem.Formula += i == count ? $"公共：({allAmount}-{sum})---->" : $"公共：({before.Amount}*{toRate})---->";

                        lastDepInfo.Add(tem);
                        i++;
                        sum += afterAmount;
                    }
                }
            }
        }

        private void tt(List<CommonSplitDto> lis, List<CommonSplitDto> ss2, List<CommonSplitDto> dodo)
        {
            var temp = new List<CommonSplitDto>();
            foreach (var s2 in ss2)
            {
                var ss3 = lis.Where(s => s.FromDepartID == s2.ToDepartID && s.SplitRate != 0).ToList();

                if (ss3.Count == 0)
                {
                    dodo.Add(s2);
                }


                foreach (var s3 in ss3)
                {
                    var s = s2.SplitRate * s3.SplitRate;
                    s3.SplitRate = s;
                    temp.Add(s3);
                }
            }


            if (temp.Count > 0)
            {
                tt(lis, temp, dodo);
            }
        }

        private static Expression<Func<CostAccount, bool>> Expression(CostAccountInput queryModel)
        {
            Expression<Func<CostAccount, bool>> expression = s => s.Status == 0;
            if (!string.IsNullOrEmpty(queryModel.Period))
            {
                expression = expression.And(s => s.Period == queryModel.Period);
            }

            if (!string.IsNullOrEmpty(queryModel.POCode))
            {
                expression = expression.And(s => s.POCode == queryModel.POCode);
            }

            if (!string.IsNullOrEmpty(queryModel.ProjectName))
            {
                expression = expression.And(s => s.ProjectName.Contains(queryModel.ProjectName));
            }

            if (!string.IsNullOrEmpty(queryModel.DepathNamePath))
            {
                expression = expression.And(s => s.DepathNamePath.Contains(queryModel.DepathNamePath));
            }

            if (queryModel.SubjectCode != -1)
            {
                expression = expression.And(s => s.SubjectCode == queryModel.SubjectCode);
            }

            if (queryModel.SecondSubjectCode != 0 && queryModel.SecondSubjectCode != -99)
            {
                expression = expression.And(s => s.SecondSubjectCode == queryModel.SecondSubjectCode);
            }
            if (queryModel.Step != 0)
            {
                expression = expression.And(s => s.Step == queryModel.Step);
            }


            return expression;
        }


    }
}
