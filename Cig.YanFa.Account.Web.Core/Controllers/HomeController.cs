using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cig.YanFa.Test;
using Cig.YanFa.Account.Application.PurchaseOrder;
using Microsoft.AspNetCore.Mvc;
using Cig.YanFa.Account.Web.Core.Models;
using Cig.YanFa.Configuration;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Cig.YanFa.Account.Web.Core.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        private ILogger<HomeController> _logger;

        private ITestService _testService;
        private IPurchaseOrderService _purchaseOrderService;
        private IConfigurationRoot _config;

        public HomeController(IHostingEnvironment env, ILogger<HomeController> logger,ITestService testService, IPurchaseOrderService purchaseOrderService)
        {
            _logger = logger;
            _testService = testService;
            _purchaseOrderService = purchaseOrderService;
            _config = env.GetAppConfiguration();
        }

        
        public IActionResult Index()
        {
            //throw new NotImplementedException("没有实现异常");

            //_logger.LogWarning("Warning!!!!!!!!!!!!!!!!!!!!");
            //_logger.LogError("Error!!!!!!!!!!!!!!!!!!!!");
            //// _purchaseOrderService.GetAll();
            ////_testService.AddBoth();
            //var claims = User.Claims.FirstOrDefault(s=>s.Type== "userId");
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
