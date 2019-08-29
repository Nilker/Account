using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Cig.YanFa.Account.Application;
using Cig.YanFa.Account.Core.SysRightsManager;
using Cig.YanFa.Account.Repository;
using Cig.YanFa.Account.Repository.Repository;
using Microsoft.Extensions.Configuration;

namespace Cig.YanFa.Navigation
{

    public class NavigationService : YanFaAppServiceBase, INavigationService
    {
        public readonly IConfiguration _configuration;
        private readonly IDapperRepositoryBase<NavigationInfo, SysRightsManagerDBContext> _navigationDapperRepository;
        private readonly IDapperRepositoryBase<SysInfo, SysRightsManagerDBContext> _sysInfoRepository;
        private readonly IDapperRepositoryBase<SysLoginKey, SysRightsManagerDBContext> _sysLoginKeyRepository;

        public NavigationService(IConfiguration configuration
            , IDapperRepositoryBase<NavigationInfo, SysRightsManagerDBContext> navigationDapperRepository
            , IDapperRepositoryBase<SysInfo, SysRightsManagerDBContext> sysInfoRepository
            , IDapperRepositoryBase<SysLoginKey, SysRightsManagerDBContext> sysLoginKeyRepository)
        {
            _navigationDapperRepository = navigationDapperRepository;
            _configuration = configuration;
            _sysInfoRepository = sysInfoRepository;
            _sysLoginKeyRepository = sysLoginKeyRepository;
        }

        /// <summary>
        /// 获取一级菜单
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="sysId">系统ID</param>
        /// <returns></returns>
        public List<NavigationInfo> GetParentModuleInfoByUserId(int userId, string sysId)
        {
            string sqlstr = $@"
select  moduleinfo.* ,DomainInfo.Domain 
FROM SysRightsManager.dbo.moduleinfo  
LEFT JOIN SysRightsManager.dbo.DomainInfo on moduleinfo.DomainCode=DomainInfo.DomainCode
where moduleinfo.moduleid in (select  distinct pid from moduleinfo   where 
                    moduleid in(SELECT DISTINCT  moduleid FROM RoleModule   WHERE  Status = 0 and  sysid=@sysId  and  roleid IN(select roleid from userrole where userid=@userId and sysid=@sysId   and status=0)) 
    and status=0 and  sysid=@sysId  and [level]=2) 
and moduleinfo.status=0 and  moduleinfo.sysid=@sysId  
order by moduleinfo.OrderNum";

            var navigationInfos = _navigationDapperRepository.QueryBySql<NavigationInfo>(sqlstr, new { userId = userId, sysId = sysId }).ToList();
            return navigationInfos;
        }


        /// <summary>
        /// 获取子模块表
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="sysID">系统ID</param>
        /// <param name="pid">父ID</param>
        /// <returns></returns>
        public List<NavigationInfo> GetChildModuleByUserId(int userID, string sysID, string pid)
        {
            string sqlstr = $@"
select  moduleinfo.* ,DomainInfo.Domain 
FROM SysRightsManager.dbo.moduleinfo  
LEFT JOIN SysRightsManager.dbo.DomainInfo on moduleinfo.DomainCode=DomainInfo.DomainCode
where moduleinfo.pid=@pId and  moduleinfo.SYSID=@sysId AND moduleinfo.status=0 
and moduleinfo.moduleid in (SELECT DISTINCT  moduleid FROM RoleModule WHERE status = 0 and roleid IN(select roleid from userrole where userID=@userId and  status=0)) 
ORDER by moduleinfo.OrderNum
";
            var navigationInfos = _navigationDapperRepository.QueryBySql<NavigationInfo>(sqlstr, new { userId = userID, sysId = sysID, pId = pid }).ToList();

            //var domain = _sysInfoRepository.Where(s => s.SysID == sysID && s.Status == 0).Select(s => s.SysURL).FirstOrDefault();

            //navigationInfos.ForEach(s => { s.Url = string.IsNullOrEmpty(domain) ? s.Url : domain + s.Url; });

            return navigationInfos;
        }

        public List<string> GetModuleByUserId(int userId)
        {
            string sqlstr = $@"select moduleid from moduleinfo where status=0 
                                and moduleid in 
                                (SELECT DISTINCT  moduleid FROM RoleModule WHERE roleid 
                                        IN(select roleid from userrole where userID=@userId and  status=0)) order by level desc";
            var lis = _navigationDapperRepository.QueryBySql<string>(sqlstr, new { userId = userId }).ToList();
            return lis;
        }

        /// <summary>
        /// 根据当前用户+请求uri 获取父菜单Id;
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        public Tuple<string, string, string> GetParentModuleIds(int userId, string uri)
        {
            string sqlstr =
                "select moduleid from moduleinfo where status=0 and moduleid in (SELECT DISTINCT  moduleid FROM RoleModule WHERE roleid IN(select roleid from userrole where userID=@userId and  status=0)) order by level desc";
            //获取用户已经有的 moduleId；
            var userHaveModuleIds = _navigationDapperRepository.QueryBySql<string>(sqlstr, new { userId = userId });

            var sysId = _configuration["ThisSysID"];
            var nav2or3 =
                _navigationDapperRepository.Find(s =>
                    s.Status == 0 && s.SysID == sysId && s.Links.Contains(uri));

            if (nav2or3 == null)
            {
                return new Tuple<string, string, string>("", "", "");
            }
            var b = userHaveModuleIds.Contains(nav2or3.ModuleID);
            //三级
            if (nav2or3.ModuleID.Contains("SUB"))
            {
                var moduleId2 = _navigationDapperRepository.Where(s => s.ModuleID == nav2or3.ModuleID).Select(s => s.PID).FirstOrDefault();
                var moduleId1 = _navigationDapperRepository.Where(s => s.ModuleID == moduleId2).Select(s => s.PID).FirstOrDefault();

                return new Tuple<string, string, string>(moduleId1, moduleId2, nav2or3.ModuleID);
            }
            else
            {
                var moduleId1 = _navigationDapperRepository.Where(s => s.ModuleID == nav2or3.ModuleID).Select(s => s.PID).FirstOrDefault();
                return new Tuple<string, string, string>(moduleId1, nav2or3.ModuleID, "");
            }
        }

        public string GetUserLoginToken(int userId)
        {
            string sqlstr = "SELECT top 1 [Token] FROM [SysRightsManager].[dbo].[UserLoginKey]" +
                            "where UserID = @UserID";
            var token = _navigationDapperRepository.QueryBySql<string>(sqlstr, new { UserID = userId }).FirstOrDefault();
            return token;
        }

        public string GetLoginKey()
        {
            string str = _sysLoginKeyRepository.FindField(s => true, s => s.Key);
            return str;
        }

        /// <summary>
        /// 用户是否具有当前页面权限
        /// </summary>
        /// <param name="pid">当前页面所属模块父ID</param>
        /// <returns>当前页面所属ID（第一个授权模块）</returns>
        public string checkUserRight(int userId, string uri, ref string pid)
        {
            //Check();
            bool haveRight = false;
            string moduleID = "";
            var sysId = _configuration["ThisSysID"];
            var navigationInfos = _navigationDapperRepository.Where(s => s.Status == 0 && s.SysID == sysId && s.Links.Contains(uri));

            if (navigationInfos.Any())
            {
                string sqlstr =
                    "select moduleid from moduleinfo where status=0 and moduleid in (SELECT DISTINCT  moduleid FROM RoleModule WHERE roleid IN(select roleid from userrole where userID=@userId and  status=0)) order by level desc";

                var modules = _navigationDapperRepository.QueryBySql<string>(sqlstr, new { userId = userId });

                foreach (var module in modules)
                {
                    foreach (var nav in navigationInfos)
                    {
                        if (module == nav.ModuleID)
                        {
                            haveRight = true;
                            moduleID = module;
                            break;
                        }
                    }
                    if (haveRight) break;
                }
            }
            else
            {
                haveRight = false;
            }
            //if (!haveRight)
            //{
            //    BitAuto.Utils.ScriptHelper.ShowAlertScript("对不起，您还没权限访问此页面!");
            //}
            if (!string.IsNullOrEmpty(moduleID))
            {
                pid = _navigationDapperRepository.Where(s => s.ModuleID == moduleID).Select(s => s.PID)
                    .FirstOrDefault();
            }
            return moduleID;
        }

        public void GetAll()
        {
            var infos = _navigationDapperRepository.All();
        }
    }
}
