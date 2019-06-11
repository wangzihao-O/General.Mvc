using System;
using System.Collections.Generic;
using System.Text;

namespace General.Framework.Infrastructure
{
    public interface IWorkContext
    {
        /// <summary>
        /// 当前登录用户
        /// </summary>
        Entities.SysUser CurrentUser { get; }
    }
}
