using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cig.YanFa.Account.Application.Common;
using Cig.YanFa.Account.Application.Crm.Expense;
using Cig.YanFa.Account.Application.Crm.Expense.Dto;
using Cig.YanFa.Account.Application.Crm.PurchaseOrder.Dto;
using Cig.YanFa.Account.Application.PurchaseOrder;
using Cig.YanFa.Account.Core;
using Cig.YanFa.Account.Core.Crm2009;
using Cig.YanFa.Account.Web.Core.Common;
using Cig.YanFa.Account.Web.Core.Common.EpPlus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Utilities;

namespace Cig.YanFa.Account.Web.Core.Controllers
{
    public class ExpenseController : BaseController
    {
        private IExpenseService _expenseService;
        private ICommonDataService _commonDataService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ExpenseController(IExpenseService expenseService, IHostingEnvironment hostingEnvironment, ICommonDataService commonDataService)
        {
            _expenseService = expenseService;
            _commonDataService = commonDataService;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET
        public IActionResult Index()
        {
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

        /// <summary>
        /// 获取计算阶段
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetSteps()
        {
            var dic = EnumHelper.GetValueAndDesc<CommonEnum.EnumCostAccountSplitStep>().Select(s => new { key = s.Key, value = s.Value });
            return Json(new JsonFlag(dic));
        }

        /// <summary>
        /// 获取费用类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetExpenseType()
        {
            var dic = EnumHelper.GetValueAndDesc<CommonEnum.EnumExpenseType>().Select(s => new { key = s.Key, value = s.Value });
            return Json(new JsonFlag(dic));
        }

        /// <summary>
        /// 获取拆分数据
        /// </summary>
        /// <param name="queryModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetListDatas(ExpenseInput queryModel)
        {
            var model = _expenseService.GetListDatas(queryModel);

            return Json(new JsonFlagPage(queryModel.Draw, model.TotalCount, model.Items));
        }

        [HttpPost]
        public JsonResult GetSummary(ExpenseInput queryModel)
        {
            var model = _expenseService.GetSummary(queryModel);
            return Json(model);
        }

        /// <summary>
        /// 拆分前
        /// </summary>
        /// <param name="period"></param>
        /// <param name="expenseType"></param>
        /// <returns></returns>
        public JsonResult SplitBefore(string period, int expenseType)
        {
            bool b = _expenseService.AddExpenseAccount(expenseType, period: period);
            return Json(new JsonFlag(b ? 1 : 0, b ? "操作成功" : "操作失败,有可能原始数据计入部门为空"));
        }

        /// <summary>
        /// 拆分后
        /// </summary>
        /// <param name="period"></param>
        /// <param name="expenseType"></param>
        /// <returns></returns>
        public JsonResult SplitAfter(string period, int expenseType)
        {
            bool b = _expenseService.ExpenseAccountDoSplit(expenseType, period: period);
            return Json(new JsonFlag(b ? 1 : 0, b ? "操作成功" : "操作失败"));
        }

        /// <summary>
        /// 上传 文件
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> FileSave(List<IFormFile> filesss)
        {
            var count = 0;

            var files = Request.Form.Files;
            long size = files.Sum(f => f.Length);
            string webRootPath = _hostingEnvironment.WebRootPath;
            string contentRootPath = _hostingEnvironment.ContentRootPath;

            Dictionary<string, double> dic = new Dictionary<string, double>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {

                    string fileExt = FileUtils.GetFileExt(formFile.FileName); //文件扩展名，不含“.”
                    double fileSize = Math.Round(Convert.ToDouble(Convert.ToDouble(formFile.Length) / 1024 / 1024), 2); //获得文件大小，以字节为单位
                    string newFileName = formFile.FileName.Substring(0, formFile.FileName.LastIndexOf(".", StringComparison.Ordinal) - 1) + $"-{DateTime.Now:yyyy-MM-dd-HH-mm}." + fileExt; //随机生成新的文件名
                    var filePath = contentRootPath + "/upload/" + newFileName;
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    //读取Excel ----->DB
                    var importDtos = new List<FixedAssetsDepreciationImportDto>();

                    #region 读取Excel
                    FileInfo ff = new FileInfo(filePath);
                    using (ExcelPackage package = new ExcelPackage(ff))
                    {
                        var sheet = package.Workbook.Worksheets.ToList()[0];

                        importDtos = (from cell in sheet.Cells[sheet.Dimension.Address]
                                      where cell.Start.Row > 1
                                      group cell by cell.Start.Row into rr
                                      select new FixedAssetsDepreciationImportDto()
                                      {
                                          CurrentRow = rr.Key,
                                          FixedAssetsCode = rr.ToList()[0].Value.ToString(),
                                          LineNum = rr.ToList()[1].Value == null ? (int?)null : Convert.ToInt32(rr.ToList()[1].Value),
                                          Period = rr.ToList()[2].Value.ToString(),
                                          DepartNamePath = rr.ToList()[3].Value.ToString(),
                                          Amount = rr.ToList()[4].Value == null ? (decimal?)null : Convert.ToDecimal(rr.ToList()[4].Value),
                                          CompanyName = rr.ToList()[5].Value.ToString(),
                                      }).ToList();
                    }
                    #endregion

                    #region 数据校验
                    //为空校验
                    var isNull = importDtos.Where(s => s.Amount == null
                                                              || s.LineNum == null
                                                              || string.IsNullOrEmpty(s.FixedAssetsCode)
                                                              || string.IsNullOrEmpty(s.DepartNamePath)
                                                              || string.IsNullOrEmpty(s.CompanyName)
                                                              || string.IsNullOrEmpty(s.Period))
                                   .Select(s => s.CurrentRow).Distinct().ToList();
                    if (isNull.Count > 0)
                    {
                        return Json(new JsonFlag(0, "有空数据，行号：" + string.Join(',', isNull)));
                    }
                    //多月数据校验
                    var nu = importDtos.Select(s=>s.Period).Distinct();
                    if (nu.Count() > 1)
                    {
                        return Json(new JsonFlag(0, "核算期间存在多月数据，如下：<br/>" + string.Join("，<br/>", nu)));
                    }

                    //数据准确性校验
                    var excelDepartNames = importDtos.Select(s => s.DepartNamePath).Distinct();
                    var departMentsByVersion = _commonDataService.GetDepartMentsByVersion(importDtos.Select(s => s.Period).FirstOrDefault()).ToList();
                    var exceptDeparts = excelDepartNames.Except(departMentsByVersion.Select(s => s.NamePath));//不在 部门版本中的 部门名称
                    if (exceptDeparts.Any())
                    {
                        return Json(new JsonFlag(0, "部门名称无法找到对应的部门编号，如下：<br/>" + string.Join("，<br/>", exceptDeparts)));

                    }
                    var excelCompanyNames = importDtos.Select(s => s.CompanyName).Distinct();
                    var companys = _commonDataService.CompanyInfos().ToList();
                    var exceptCompanys = excelCompanyNames.Except(companys.Select(s => s.CompanyName));
                    if (exceptCompanys.Any())
                    {
                        return Json(new JsonFlag(0, "签约公司无法找打对应的编号，如下：<br/>" + string.Join("，<br/>", exceptCompanys)));
                    }

                    #endregion

                    var histories = new List<FixedAssetsDepreciationHistory>();

                    foreach (var item in importDtos)
                    {
                        var model = new FixedAssetsDepreciationHistory()
                        {
                            FixedAssetsCode = item.FixedAssetsCode,
                            LineNum = item.LineNum.Value,
                            Period = item.Period,
                            DepartID = departMentsByVersion.Where(s => s.NamePath == item.DepartNamePath).Select(s => s.DepartID).FirstOrDefault(),
                            DepartNamePath = item.DepartNamePath,
                            Amount = item.Amount.Value,
                            CompanyCode = companys.Where(s => s.CompanyName == item.CompanyName).Select(s => s.CompanyCode).FirstOrDefault(),
                            CompanyName = item.CompanyName,
                            CreateTime = DateTime.Now,
                            CreateUserID = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value),
                            CreateUserName = User.Identity.Name,
                            LastUpdateTime = DateTime.Now
                        };
                        histories.Add(model);
                    }

                    count = _expenseService.AddFixedAssetsDepreciation(histories);

                    dic.Add(newFileName, fileSize);
                }
            }

            var tem = dic.Select(s => new { newFileName = s.Key, size = s.Value });
            return Json(new JsonFlag(1, $"成功导入{count}条数据，到数据库", tem));
        }


        public FileResult ExportHistory(int expenseType, string period)
        {
            var obj = new ExpenseLineConfirmHistoryExportDto();
            var dataList = _expenseService.GetExpenseHistory(expenseType, period);

            using (var excelPackage = new ExcelPackage())
            {
                var sheet1 = excelPackage.Workbook.Worksheets.Add("费用月成本历史表");
                EpPlusHelper.AddHeader(sheet1,
                            obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.ExpenseCode)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.ExpenseLineNum)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.DepartID)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.DepartNamePath)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.Period)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.Amount)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.MaterialClassCode)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.MaterialClassName)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.MaterialCode)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.MaterialName)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.ExpenseItem)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.ExpenseItemName)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.ProjectCode)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.ProjectName)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.CompanyCode)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.CompanyName)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.Status)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.Ratio)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.Step)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.Formula)),
 obj.GetDescription(ClassHelper.GetPropertyName<ExpenseLineConfirmHistoryExportDto>(s => s.IsClose))
                         );
                EpPlusHelper.AddObjects(
                     sheet1, 2, dataList.ToList(),
                     _ => _.ExpenseCode,
                     _ => _.ExpenseLineNum,
                     _ => _.DepartID,
                     _ => _.DepartNamePath,
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
                     _ => _.Status,
                     _ => _.Ratio,
                     _ => _.Step,
                     _ => _.Formula,
                     _ => _.IsClose
                 );

                var sss = excelPackage.GetAsByteArray();
                string sFileName = $"费用历史{DateTime.Now}.xlsx";
                return File(sss, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileDownloadName: sFileName);

            }
        }

        public FileResult ExportSplit(int expenseType,string period)
        {
            var obj = new ExpenseAccountExportDto();
            var dataList = _expenseService.GetExpenseAccountExportDtos(expenseType, period );

            var _dic = EnumHelper.GetValueAndDesc<CommonEnum.EnumCostAccountSplitStep>();

            using (var excelPackage = new ExcelPackage())
            {
                foreach (var d in _dic)
                {
                    var sheet1 = excelPackage.Workbook.Worksheets.Add(d.Value);
                    EpPlusHelper.AddHeader(sheet1,
                           obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.ExpenseCode)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.ExpenseLineNum)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.DepartID)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.DepartNamePath)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.Period)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.Amount)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.MaterialClassCode)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.MaterialClassName)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.MaterialCode)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.MaterialName)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.ExpenseItem)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.ExpenseItemName)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.ProjectCode)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.ProjectName)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.CompanyCode)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.CompanyName)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.Status)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.Ratio)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.Step)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.Formula)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.IsClose)),
obj.GetDescription(ClassHelper.GetPropertyName<ExpenseAccountExportDto>(s => s.ExpenseTypeName))
    );
                    EpPlusHelper.AddObjects(
                         sheet1, 2, dataList.Where(s => s.Step == d.Key).ToList(),
                         _ => _.ExpenseCode,
                         _ => _.ExpenseLineNum,
                         _ => _.DepartID,
                         _ => _.DepartNamePath,
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
                         _ => _.Status,
                         _ => _.Ratio,
                         _ => _.StepName,
                         _ => _.Formula,
                         _ => _.IsClose,
                         _ => _.ExpenseTypeName
                     );
                }

                var sss = excelPackage.GetAsByteArray();
                string sFileName = $"费用-导出拆分结果{DateTime.Now}.xlsx";
                return File(sss, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileDownloadName: sFileName);
            }
        }

    }
}