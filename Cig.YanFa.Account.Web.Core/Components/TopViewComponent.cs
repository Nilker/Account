using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cig.YanFa.Configuration;
using Cig.YanFa.Navigation;
using Cig.YanFa.Navigation.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Cig.YanFa.Account.Web.Core.Components
{
    public class TopViewComponent : ViewComponent
    {
        private INavigationService _navigationService;
        private IConfiguration _configuration { get; }
        public TopViewComponent(INavigationService navigationService, IHostingEnvironment env)
        {
            _navigationService = navigationService;
            _configuration = env.GetAppConfiguration(); ;
        }


        public async Task<IViewComponentResult> InvokeAsync()
        {


            // var pid = string.Empty;
            var userId = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value);
            var sysId = _configuration["ThisSysID"];  //HttpContext.User.Claims.FirstOrDefault(c => c.Type == "sysId")?.Value;

            var tuple = _navigationService.GetParentModuleIds(userId, Request.Path.Value);
            //SecondModuleId = _navigationService.checkUserRight(userId, Request.Path.Value, ref pid);

            //一级菜单
            var firstModules = _navigationService.GetParentModuleInfoByUserId(userId, sysId);
            firstModules.ForEach(s =>
            {
                s.Url = _navigationService.GetChildModuleByUserId(userId, sysId, s.ModuleID).Select(t => t.Url)
                        .FirstOrDefault();
            });

            //二级菜单
            var secondModules = _navigationService.GetChildModuleByUserId(userId, sysId, tuple.Item1);

            //三级菜单
            var threeModules = _navigationService.GetChildModuleByUserId(userId, sysId, tuple.Item2);


            //交换位置，插入前十；
            //一级菜单
            var tem = firstModules.FirstOrDefault(s => s.ModuleID == tuple.Item1);
            if (firstModules.IndexOf(tem) > 9 && firstModules.Remove(tem))
            {
                firstModules.Insert(9, tem);
            }

            //二级菜单
            var tem2 = secondModules.FirstOrDefault(s => s.ModuleID == tuple.Item2);
            if (secondModules.IndexOf(tem2) > 9 && secondModules.Remove(tem2))
            {
                secondModules.Insert(9, tem2);
            }

            var o = new NavigationOutput
            {
                firstModules = firstModules,
                secondModules = secondModules,
                threeModules = threeModules,
                currentFirstModule = tuple.Item1,
                currentSecondModule = tuple.Item2,
                currentThreeModule = tuple.Item3
            };
            return View(o);
        }
    }
}
