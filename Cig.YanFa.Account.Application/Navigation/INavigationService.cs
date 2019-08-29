using System;
using System.Collections.Generic;
using System.Text;

namespace Cig.YanFa.Navigation
{
    public interface INavigationService 
    {
        void GetAll();
        List<NavigationInfo> GetParentModuleInfoByUserId(int userId, string sysId);
        List<NavigationInfo> GetChildModuleByUserId(int userID, string sysID, string pid);

        string checkUserRight(int userId, string uri, ref string pid);

        /// <summary>
        /// 获取用户 所具有的所有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<string> GetModuleByUserId(int userId);

        /// <summary>
        /// 根据当前用户+请求uri 获取父菜单Id;
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="uri"></param>
        /// <returns></returns>
        Tuple<string, string, string> GetParentModuleIds(int userId, string uri);

        string GetUserLoginToken(int userId);

        /// <summary>
        /// 获取登陆Key
        /// </summary>
        /// <returns></returns>
        string GetLoginKey();
    }
}
