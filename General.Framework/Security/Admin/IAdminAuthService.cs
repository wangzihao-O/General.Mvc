using System;
using System.Collections.Generic;
using System.Text;

namespace General.Framework.Security.Admin
{
    public interface IAdminAuthService
    {
        /// <summary>
        /// 保存等状态
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        void signIn(string token, string name);

        /// <summary>
        /// 退出登录
        /// </summary>
        void signOut();

        /// <summary>
        /// 获取当前登录用户
        /// (缓存)
        /// </summary>
        /// <returns></returns>
        Entities.SysUser getCurrentUser();

        
    }
}
