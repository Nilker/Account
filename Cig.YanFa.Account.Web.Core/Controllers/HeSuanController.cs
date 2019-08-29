using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cig.YanFa.HeSuan;
using Cig.YanFa.HeSuan.Dto;
using Cig.YanFa.Account.Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Cig.YanFa.Account.Web.Core.Controllers
{
    public class HeSuanController : BaseController
    {
        private ISaleService _saleService;
        public HeSuanController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Index2()
        {
            return View();
        }

        public PartialViewResult SubList(SaleQueryInput query)
        {
            var pageModel = _saleService.GePageList(query);

            ViewBag.PagerOption = new CrmPagerOption() { Total = pageModel.TotalCount, CurrentPage = query.CurrentPage };
            return PartialView(pageModel.Items);
        }


        [HttpPost]
        public JsonResult GetListDatas(SaleQueryInput query)
        {
            var pageModel = _saleService.GePageList(query);

            //ViewBag.PagerOption = new CrmPagerOption() { Total = pageModel.TotalCount, CurrentPage = query.CurrentPage };
            return Json(new{draw=query.Draw, recordsTotal=pageModel.TotalCount, recordsFiltered= pageModel.TotalCount, data =pageModel.Items});
        }
    }
}