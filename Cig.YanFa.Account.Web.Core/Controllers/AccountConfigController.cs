using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cig.YanFa.Account.Application.Crm.CommonData.Dto;
using Cig.YanFa.Account.Application.Crm.PurchaseOrder;
using Cig.YanFa.Account.Application.PurchaseOrder;
using Cig.YanFa.Account.Application.PurchaseOrder.Dto;
using Cig.YanFa.Account.Web.Core.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cig.YanFa.Account.Web.Core.Controllers
{
    public class AccountConfigController : BaseController
    {
        private ICommonDataService _commonDataService;
        private ICostAccountConfigService _costAccountConfigService;

        public AccountConfigController(ICommonDataService commonDataService
                                    , ICostAccountConfigService costAccountConfigService)
        {
            _commonDataService = commonDataService;
            _costAccountConfigService = costAccountConfigService;
        }

        public IActionResult Index()
        {
            //签约公司
            var companyInfoDtos = _commonDataService.CompanyInfos();
            var selectListItems = companyInfoDtos.Select(s => new SelectListItem(s.CompanyName, s.CompanyCode)).ToList();
            selectListItems.Insert(0, new SelectListItem("缺省", "-99"));
            selectListItems.Insert(0, new SelectListItem("请选择", ""));
            ViewBag.Company = selectListItems;

            //订单来源（采购大类）
            var types = _commonDataService.GetRelevanceInfos(5).ToList();
            var typesListItem = types.Select(s => new SelectListItem(s.Value, s.Key)).ToList();
            typesListItem.Insert(0, new SelectListItem("请选择", "0"));
            ViewBag.Types = typesListItem;


            //物料分类
            var productCategoryDtos = _commonDataService.GetProductCategorys(level: 1, type: "MaterialClass");
            var productListItem = productCategoryDtos.Select(s => new SelectListItem(s.Name, s.ProductCatID)).ToList();
            productListItem.Insert(0, new SelectListItem("请选择", ""));
            ViewBag.productCategories = productListItem;

            //支出类型一级
            var payOutTypes = _commonDataService.GetDicInfos(723).ToList();
            var payOutTypeListItem = payOutTypes.Select(s => new SelectListItem(s.DictName, s.DictId.ToString())).ToList();
            payOutTypeListItem.Insert(0, new SelectListItem("请选择", "0"));
            ViewBag.payOutTypes = payOutTypeListItem;


            return View();
        }

        public JsonResult GetContentType(int type)
        {
            var types = _commonDataService.GetRelevanceInfos(6, type).ToList();
            //采购类型
            var contentTypes = new List<DicInfoDto>();
            var tem = _commonDataService.GetDicInfos();
            foreach (var item in types)
            {
                var ss = tem.Where(s => s.DictId.ToString() == item.Key).Select(s => new DicInfoDto()
                {
                    DictId = s.DictId,
                    DictName = s.DictName,
                    DictType = s.DictType
                }).ToList();
                contentTypes.AddRange(ss);
            }

            contentTypes.Insert(0, new DicInfoDto() { DictId = -99, DictName = "缺省" });
            return Json(contentTypes);
        }

        public JsonResult GetProductCat2(int cat1)
        {
            var productCategoryDtos = _commonDataService.GetProductCategorys(level: 2, parentId: cat1, type: "Material");
            productCategoryDtos.Insert(0, new ProductCategoryDto() { ProductCatID = "-99", Name = "缺省" });
            return Json(productCategoryDtos);
        }

        public JsonResult GetpayOutType2(int payOutType1)
        {
            var payOutTypes = _commonDataService.GetDicInfos(payOutType1).ToList();
            payOutTypes.Insert(0, new DicInfoDto() { DictId = -99, DictName = "缺省" });
            return Json(payOutTypes);
        }

        [HttpPost]
        public JsonResult AddConfig(CostAccountConfigInput model)
        {
            var b = _costAccountConfigService.AddCostAccountConfig(model);
            return Json(new JsonFlag(b ? 1 : 0, b ? "操作成功" : "操作失败"));
        }

        public JsonResult GetListDatas(CostAccountConfigInput queryModel)
        {
            var model = _costAccountConfigService.GetAccountConfigsByPage(queryModel);
            return Json(new JsonFlagPage(queryModel.Draw,model.TotalCount,model.Items));
        }

        public JsonResult DeletCostAccountConfig(int id)
        {
            var userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            bool b = _costAccountConfigService.DeleteCostAccountConfig(id, Convert.ToInt32(userId));
            return Json(new JsonFlag(b ? 1 : 0, b ? "操作成功" : "操作失败"));
        }
    }
}