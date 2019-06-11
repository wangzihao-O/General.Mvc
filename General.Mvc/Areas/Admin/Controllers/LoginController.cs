using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using General.Entities;
using General.Framework.Controllers.Admin;
using General.Framework.Security.Admin;
using General.Services.SysUser;
using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace General.Mvc.Areas.Admin.Controllers
{
    /// <summary>
    /// 后台登录控制器
    /// </summary>
    [Route("Admin")]
    public class LoginController : AdminAreaController
    {
        private const string R_KEY = "R_KEY";
        private ISysUserService _sysUserService;
        private IAdminAuthService _authenticationService;
        private IMemoryCache _memoryCache;

        public LoginController(ISysUserService sysUserService,
            IAdminAuthService authenticationService,
            IMemoryCache memoryCache)
        {
            this._sysUserService = sysUserService;
            this._authenticationService = authenticationService;
            this._memoryCache = memoryCache;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("login",Name ="adminLogin")]
        public IActionResult LoginIndex()
        {
            string r= Core.Librs.EncryptorHelper.GetMD5(Guid.NewGuid().ToString());
            string sessionId = HttpContext.Session.Id;
            HttpContext.Session.SetString(R_KEY,r);
            LoginModel loginModel = new LoginModel() { R = r };

            string a = HttpContext.Session.GetString(R_KEY);
            return View(loginModel);
        }
        [HttpPost]
        [Route("login",Name ="adminLogin1")]
        public IActionResult LoginIndex(LoginModel model)
        {
            string r= HttpContext.Session.GetString(R_KEY);
             r=r ?? "";
            if(!ModelState.IsValid)
            {
                AjaxData.Message = "请输入用户账号和密码";
                return Json(AjaxData);
            }
            var result= _sysUserService.validateUser(model.Account,model.Password, r);
            AjaxData.Status = result.Status;
            AjaxData.Message = result.Message;
            if (result.Item1)
            {
                //保存登录状态 ClaimsIdentity,ClaimPrincipal,Asp.Net HttpContext,Current
                _authenticationService.signIn(result.Token, result.User.Name);
            }
            return Json(AjaxData);
        } 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [Route("getSalt",Name ="getSalt")]
        public IActionResult getSalt(string account)
        {
            var user = _sysUserService.getByAccount(account);
            return Content(user?.Salt);
        }
    }
}