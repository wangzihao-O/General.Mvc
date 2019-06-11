using General.Entities;
using General.Framework.Infrastructure;
using General.Framework.Security.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace General.Framework
{
    public class WorkContext : IWorkContext
    {
        private IAdminAuthService _authenticationService;
        public WorkContext(IAdminAuthService authenticationService)
        {
            this._authenticationService = authenticationService;
        }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public SysUser CurrentUser
        {
            get { return _authenticationService.getCurrentUser(); }
        }
    }
}
