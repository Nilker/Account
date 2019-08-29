using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cig.YanFa.Account.Application.Common;
using Cig.YanFa.Account.Application.Crm.PurchaseOrder.Dto;
using Cig.YanFa.Account.Application.PurchaseOrder;
using Cig.YanFa.Account.Core;
using Cig.YanFa.Account.Web.Core.Common;
using Cig.YanFa.Account.Web.Core.Common.EpPlus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Utilities;

namespace Cig.YanFa.Account.Web.Core.Controllers
{
    public class CostAccountController : BaseController
    {
        #region Ioc
        private readonly IMapper _mapper;
        private ICommonDataService _commonDataService;
        private IPurchaseOrderService _purchaseOrderService;

        public CostAccountController(ICommonDataService commonDataService
            , IPurchaseOrderService purchaseOrderService
            , IMapper mapper)
        {
            _commonDataService = commonDataService;
            _purchaseOrderService = purchaseOrderService;
            _mapper = mapper;
        }
        #endregion

        public IActionResult Index()
        {

            //支出类型一级
            var payOutTypes = _commonDataService.GetDicInfos(723).ToList();
            var payOutTypeListItem = payOutTypes.Select(s => new SelectListItem(s.DictName, s.DictId.ToString())).ToList();
            payOutTypeListItem.Insert(0, new SelectListItem("请选择", "-1"));
            ViewBag.payOutTypes = payOutTypeListItem;

            //阶段：
            var dic = EnumHelper.GetValueAndDesc<CommonEnum.EnumCostAccountSplitStep>();
            var steps = new List<SelectListItem>();
            foreach (var i in dic)
            {
                steps.Add(new SelectListItem(i.Value, i.Key.ToString()));
            }
            steps.Insert(0, new SelectListItem("请选择", "0"));
            ViewBag.steps = steps;

            return View();
        }

        [HttpPost]
        public JsonResult GetListDatas(CostAccountInput queryModel)
        {
            var model = _purchaseOrderService.GetCostAccountByPage(queryModel);

            return Json(new JsonFlagPage(queryModel.Draw, model.TotalCount, model.Items));
        }


        public JsonResult SplitBefore()
        {
            bool b = _purchaseOrderService.AddCostAccountBeforeSplit();
            return Json(new JsonFlag(b ? 1 : 0, b ? "操作成功" : "操作失败"));
        }

        public JsonResult SplitAfter()
        {
            bool b = _purchaseOrderService.DoSplit();
            return Json(new JsonFlag(b ? 1 : 0, b ? "操作成功" : "操作失败"));
        }

        public FileResult ExportHistory(string period)
        {
            var objHistory = new PurchaseOrderLineConfirmHistoryExportDto();
            var dataList = _purchaseOrderService.GetPurchaseOrderLineConfirmHistory(period);

            using (var excelPackage = new ExcelPackage())
            {
                var sheet1 = excelPackage.Workbook.Worksheets.Add("成本支出月成本历史表");
               EpPlusHelper.AddHeader(sheet1,
                    objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.POCode)),
                     objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.POLineNum)),
                       objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.Period)),
                       objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.Amount)),
                       objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.MaterialClassName)),
                       objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.MaterialName)),
                       objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.ExpenseItemName)),
                       objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.ProjectCode)),
                       objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.ProjectType)),
                       objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.ProjectName)),
                       objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.CompanyName)),
                       objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.DepartNamePath)),
                       objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.SupplierName)),
                       objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.ContentTypeName)),
                       objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.OrderEnumName)),
                       objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.CreateTime)),


                        objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.PRCode)),
                        objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.PRDepartID)),
                        objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.PRDepartNamePath)),
                        objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.POCreateTime)),
                        objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.POLineAmount)),
                        objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.POLineBeginTime)),
                        objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.POLineEndTime)),
                        objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.CustID)),
                        objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.CustName)),
                        objHistory.GetDescription(ClassHelper.GetPropertyName<PurchaseOrderLineConfirmHistoryExportDto>(s => s.TaxCode))
                    );
               EpPlusHelper.AddObjects(sheet1, 2, dataList,
                      _ => _.POCode,
                        _ => _.POLineNum,
                        _ => _.Period,
                        _ => _.Amount,
                        _ => _.MaterialClassName,
                        _ => _.MaterialName,
                        _ => _.ExpenseItemName,
                        _ => _.ProjectCode,
                        _ => _.ProjectType,
                        _ => _.ProjectName,
                        _ => _.CompanyName,
                        _ => _.DepartNamePath,
                        _ => _.SupplierName,
                        _ => _.ContentTypeName,
                        _ => _.OrderEnumName,
                        _ => _.CreateTime,
                        _ => _.PRCode,
                        _ => _.PRDepartID,
                        _ => _.PRDepartNamePath,
                        _ => _.POCreateTime,
                        _ => _.POLineAmount,
                        _ => _.POLineBeginTime,
                        _ => _.POLineEndTime,
                        _ => _.CustID,
                        _ => _.CustName,
                        _ => _.TaxCode
                    );
                var createTimeColumn = sheet1.Column(16);
                createTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                createTimeColumn.AutoFit();
                //excelPackage.Save();

                var sss = excelPackage.GetAsByteArray();
                string sFileName = $"成本支出{DateTime.Now}.xlsx";
                return File(sss, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",fileDownloadName:sFileName);

            }
        }

        public FileResult ExportVersion(string period)
        {
            var obj = new PurchaseRequirementDeptShareRatioVersionExportDto();
            var dataList = _purchaseOrderService.GetPurchaseRequirementDeptShareRatioVersions(period);

            using (var excelPackage = new ExcelPackage())
            {
                var sheet1 = excelPackage.Workbook.Worksheets.Add("集采需求拆分");
                EpPlusHelper.AddHeader(sheet1,
                       obj.GetDescription(ClassHelper.GetPropertyName<PurchaseRequirementDeptShareRatioVersionExportDto>(s => s.POCode)),
                       obj.GetDescription(ClassHelper.GetPropertyName<PurchaseRequirementDeptShareRatioVersionExportDto>(s => s.POLineNum)),
                       obj.GetDescription(ClassHelper.GetPropertyName<PurchaseRequirementDeptShareRatioVersionExportDto>(s => s.Period)),
                       obj.GetDescription(ClassHelper.GetPropertyName<PurchaseRequirementDeptShareRatioVersionExportDto>(s => s.PRCode)),
                       obj.GetDescription(ClassHelper.GetPropertyName<PurchaseRequirementDeptShareRatioVersionExportDto>(s => s.PECode)),
                       obj.GetDescription(ClassHelper.GetPropertyName<PurchaseRequirementDeptShareRatioVersionExportDto>(s => s.DepartID)),
                       obj.GetDescription(ClassHelper.GetPropertyName<PurchaseRequirementDeptShareRatioVersionExportDto>(s => s.DepartNamePath)),
                       obj.GetDescription(ClassHelper.GetPropertyName<PurchaseRequirementDeptShareRatioVersionExportDto>(s => s.ShareProportion)),
                       obj.GetDescription(ClassHelper.GetPropertyName<PurchaseRequirementDeptShareRatioVersionExportDto>(s => s.CreateTime))
                    );
                EpPlusHelper.AddObjects(
                    sheet1, 2, dataList,
                    _ => _.POCode,
                    _ => _.POLineNum,
                    _ => _.Period,
                    _ => _.PRCode,
                    _ => _.PECode,
                    _ => _.DepartID,
                    _ => _.DepartNamePath,
                    _ => _.ShareProportion,
                    _ => _.CreateTime
                );

                var createTimeColumn = sheet1.Column(9);
                createTimeColumn.Style.Numberformat.Format = "yyyy-mm-dd";
                createTimeColumn.AutoFit();

                var sss = excelPackage.GetAsByteArray();
                string sFileName = $"集采需求拆分{DateTime.Now}.xlsx";
                return File(sss, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileDownloadName: sFileName);
            }
        }

        public FileResult ExportSplit(string period)
        {
            var obj = new CostAccountExportDto();
            var dataList = _purchaseOrderService.GetCostAccountExportDtos(period);

            var _dic = EnumHelper.GetValueAndDesc<CommonEnum.EnumCostAccountSplitStep>();

            using (var excelPackage = new ExcelPackage())
            {
                foreach (var d in _dic)
                {
                    var sheet1 = excelPackage.Workbook.Worksheets.Add(d.Value);
                    EpPlusHelper.AddHeader(sheet1,
                           obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.POCode)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.POLineNum)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.DepartID)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.DepathNamePath)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.Period)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.Amount)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.MaterialClassCode)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.MaterialClassName)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.MaterialCode)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.MaterialName)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.ExpenseItem)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.ExpenseItemName)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.ProjectCode)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.ProjectName)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.CompanyCode)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.CompanyName)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.SupplierID)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.SupplierName)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.ContentType)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.ContentTypeName)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.CostAccountConfigID)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.SubjectCode)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.SecondSubjectCode)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.SubjectName)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.SecondSubjetName)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.Status)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.Ratio)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.Step)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.Formula)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.IsClose)),

obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.PRCode )),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.PRDepartID )),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.PRDepartNamePath)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.POCreateTime  )),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.POLineAmount  )),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.POLineBeginTime)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.POLineEndTime )),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.CustID )),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.CustName)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.OrderEnum)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.OrderEnumName)),
obj.GetDescription(ClassHelper.GetPropertyName<CostAccountExportDto>(s => s.TaxCode))
                    );
                    EpPlusHelper.AddObjects(
                        sheet1, 2, dataList.Where(s=>s.Step==d.Key).ToList(),
                        _ => _.POCode,
                        _ => _.POLineNum,
                        _ => _.DepartID,
                        _ => _.DepathNamePath,
                        _ => _.Period,
                        _ => _.Amount,
                        _ => _.MaterialClassCode,
                        _ => _.MaterialClassName,
                        _ => _.MaterialCode,
                        _ => _.MaterialName,
                        _ => _.ExpenseItem,
                        _ => _.ExpenseItemName,
                        _ => _.ProjectCode,
                        _ => _.ProjectName,
                        _ => _.CompanyCode,
                        _ => _.CompanyName,
                        _ => _.SupplierID,
                        _ => _.SupplierName,
                        _ => _.ContentType,
                        _ => _.ContentTypeName,
                        _ => _.CostAccountConfigID,
                        _ => _.SubjectCode,
                        _ => _.SecondSubjectCode,
                        _ => _.SubjectName,
                        _ => _.SecondSubjetName,
                        _ => _.Status,
                        _ => _.Ratio,
                        _ => _.Step,
                        _ => _.Formula,
                        _ => _.IsClose,
                        _ => _.PRCode,
                        _ => _.PRDepartID,
                        _ => _.PRDepartNamePath,
                        _ => _.POCreateTime,
                        _ => _.POLineAmount,
                        _ => _.POLineBeginTime,
                        _ => _.POLineEndTime,
                        _ => _.CustID,
                        _ => _.CustName,
                        _ => _.OrderEnum,
                        _ => _.OrderEnumName,
                        _ => _.TaxCode
                        );
                }

                var sss = excelPackage.GetAsByteArray();
                string sFileName = $"支出-导出拆分结果{DateTime.Now}.xlsx";
                return File(sss, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileDownloadName: sFileName);
            }
        }

        /// <summary>
        /// 导出业务---》管理 关系
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public FileResult BusinessToAch(string period)
        {
            var obj = new v_AchievementBusinessDepartMappingVersionExportDto();
            var dataList = _purchaseOrderService.GetBusinessToAchMappingVersion(period);
            using (var excelPackage = new ExcelPackage())
            {
                var sheet1 = excelPackage.Workbook.Worksheets.Add("业务架构-管理架构对应关系");
                EpPlusHelper.AddHeader(sheet1,
                       obj.GetDescription(ClassHelper.GetPropertyName<v_AchievementBusinessDepartMappingVersionExportDto>(s => s.Year)),
                       obj.GetDescription(ClassHelper.GetPropertyName<v_AchievementBusinessDepartMappingVersionExportDto>(s => s.Month)),
                       obj.GetDescription(ClassHelper.GetPropertyName<v_AchievementBusinessDepartMappingVersionExportDto>(s => s.BusinessDepartID)),
                       obj.GetDescription(ClassHelper.GetPropertyName<v_AchievementBusinessDepartMappingVersionExportDto>(s => s.BusinessDepartName)),
                       obj.GetDescription(ClassHelper.GetPropertyName<v_AchievementBusinessDepartMappingVersionExportDto>(s => s.AchievementDepartID)),
                       obj.GetDescription(ClassHelper.GetPropertyName<v_AchievementBusinessDepartMappingVersionExportDto>(s => s.AchievementDepartName)),
                       obj.GetDescription(ClassHelper.GetPropertyName<v_AchievementBusinessDepartMappingVersionExportDto>(s => s.Status))
                    );
                EpPlusHelper.AddObjects(
                    sheet1, 2, dataList,
                    _ => _.Year,
                    _ => _.Month,
                    _ => _.BusinessDepartID,
                    _ => _.BusinessDepartName,
                    _ => _.AchievementDepartID,
                    _ => _.AchievementDepartName,
                    _ => _.Status==0?"正常":"失效"
                );

                var sss = excelPackage.GetAsByteArray();
                string sFileName = $"业务架构-管理架构对应关系{DateTime.Now}.xlsx";
                return File(sss, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileDownloadName: sFileName);
            }
        }

    }
}