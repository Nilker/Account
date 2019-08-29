using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Cig.YanFa.Configuration;
using Cig.YanFa.Navigation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

using Cig.YanFa.Account.Web.Core.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace Cig.YanFa.Account.Web.Core.Common
{
    public class DemoHandle : IAuthenticationSignInHandler
    {
        private HttpContext _context;
        private AuthenticationScheme _authenticationScheme;
        public ILogger Logger;
        //private string strCookieKey = "1";//"8g7dk54d43";
        private string _cookieName = "cig-3244xx32323sdf";

        private INavigationService _navigationService;
        private IConfigurationRoot _configuration;
        public DemoHandle(INavigationService navigationService, IHostingEnvironment env, ILogger<DemoHandle> logger)
        {
            _navigationService = navigationService;
            _configuration = env.GetAppConfiguration();
            Logger = logger;

        }


        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            _context = context;
            _authenticationScheme = scheme;
            return Task.CompletedTask;
        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            var cookie = _context.Request.Cookies[_cookieName];
            if (string.IsNullOrEmpty(cookie))
            {
                return Task.FromResult(AuthenticateResult.Fail("qdl"));
            }
            var identity = new ClaimsIdentity(_authenticationScheme.Name);
            var b = IsLogin(identity);
            var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), _authenticationScheme.Name);


            return Task.FromResult(b ? AuthenticateResult.Success(ticket) : AuthenticateResult.Fail("请登录"));
        }

        private bool IsLogin(ClaimsIdentity identity)
        {
            var strCookieKey = _navigationService.GetLoginKey();
            // 如果Cookie存在，获取cookie值。
            //如果Session存在且与cookie信息一致，则返回true，否则并将用户信息保存到Session中，返回true。
            if (_context.Request.Cookies[_cookieName] != null &&
                !string.IsNullOrEmpty(_context.Request.Cookies[_cookieName]))
            {
                try
                {
                    string cookieValue = DESEncryptor.Decrypt(_context.Request.Cookies[_cookieName], strCookieKey);
                    if (string.IsNullOrEmpty(cookieValue))
                    {
                        Logger.LogError("IsLogin无法获取cookie");
                        return false;
                    }
                    string[] strCookieArr = cookieValue.Split('|');
                    if (strCookieArr.Length == 0)
                    {
                        Logger.LogError("IsLogin--》cookie格式不对：" + cookieValue);
                        return false;
                    }
                    string useridstr = DESEncryptor.Decrypt(strCookieArr[0], strCookieKey);

                    int userid;
                    if (int.TryParse(useridstr, out userid))
                    {
                        string token = _navigationService.GetUserLoginToken(userid);
                        if (string.Equals(token, DESEncryptor.Decrypt(strCookieArr[7], strCookieKey)))
                        {
                            userid = Convert.ToInt32(DESEncryptor.Decrypt(strCookieArr[0], strCookieKey));
                            var username = DESEncryptor.Decrypt(WebUtility.UrlDecode(strCookieArr[1]), strCookieKey);
                            var truename = DESEncryptor.Decrypt(WebUtility.UrlDecode(strCookieArr[2]), strCookieKey);
                            var sysID = "";
                            var gid = DESEncryptor.Decrypt(strCookieArr[4], strCookieKey);
                            var departID = DESEncryptor.Decrypt(strCookieArr[5], strCookieKey);
                            var pid = DESEncryptor.Decrypt(strCookieArr[6], strCookieKey);
                            var token1 = DESEncryptor.Decrypt(strCookieArr[7], strCookieKey);
                            var adname = DESEncryptor.Decrypt(strCookieArr[8], strCookieKey);
                            var usercode = DESEncryptor.Decrypt(strCookieArr[9], strCookieKey);

                            identity.AddClaim(new Claim("userId", userid.ToString()));
                            identity.AddClaim(new Claim(ClaimTypes.Name, truename));
                            identity.AddClaim(new Claim("truename", truename));
                            identity.AddClaim(new Claim("sysID", sysID));
                            identity.AddClaim(new Claim("gid", gid));
                            identity.AddClaim(new Claim("departID", departID));
                            identity.AddClaim(new Claim("pid", pid));
                            identity.AddClaim(new Claim("token", token1));
                            identity.AddClaim(new Claim("adname", adname));
                            identity.AddClaim(new Claim("usercode", usercode));

                            return true;
                        }
                        return false;
                    }
                    return false;
                }
                catch (Exception e)
                {
                    Logger.LogError("IsLogin异常", e);
                    throw;
                }
            }

            return false;
        }

        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            _context.Response.Cookies.Append(_cookieName, user.Identity.Name);
            return Task.CompletedTask;
        }



        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            //throw new NotImplementedException();
            //properties.RedirectUri = "/login";
            var gourl = _context.Request.Scheme + "://" + _context.Request.Host.Host + _context.Request.Path.Value;
            var loginUrl = _configuration["Wp2013old:SysLoginAddress"] + WebUtility.UrlEncode(gourl);
            _context.Response.Redirect(loginUrl);
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            throw new NotImplementedException();
        }
    }
}
