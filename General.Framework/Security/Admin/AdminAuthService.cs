using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using General.Entities;
using General.Services.SysUser;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace General.Framework.Security.Admin
{
    public class AdminAuthService : IAdminAuthService
    {
        private IHttpContextAccessor _httpContextAccessor;

        private ISysUserService _sysUserService;

        public AdminAuthService(IHttpContextAccessor httpContextAccessor,
            ISysUserService sysUserService)
        {
            this._httpContextAccessor = httpContextAccessor;
            this._sysUserService = sysUserService;
        }
  
        /// <summary>
        /// 获取当前登录用户
        /// </summary>
        /// <returns></returns>
        public SysUser getCurrentUser()
        {
            var result = _httpContextAccessor.HttpContext.AuthenticateAsync(CookieAdminAuthInfo.AuthenticationScheme).Result;
            if (result.Principal == null)
                return null;
            
            var token = result.Principal.FindFirst(ClaimTypes.Sid).Value;

            //var user = _sysUserService.getLogged(token);

            return _sysUserService.getLogged(token??null);
        }

        public void signIn(string token, string name)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity("Forms");
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Sid, token));
            claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, name));
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            _httpContextAccessor.HttpContext.SignInAsync(CookieAdminAuthInfo.AuthenticationScheme, claimsPrincipal);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public void signOut()
        {
            _httpContextAccessor.HttpContext.SignOutAsync(CookieAdminAuthInfo.AuthenticationScheme);
        }
    }
}
