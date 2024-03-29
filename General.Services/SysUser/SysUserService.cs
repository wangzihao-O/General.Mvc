﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using General.Core.Data;
using General.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace General.Services.SysUser
{
    public class SysUserService:ISysUserService
    {
        private IMemoryCache _memoryCache;

        private const string MODEL_KEY = "General.services.user_{0}";

        private IRepository<Entities.SysUser> _sysUserRepository;

        private IRepository<Entities.SysUserToken> _sysUserTokenRepository;

        public SysUserService(IRepository<Entities.SysUser> sysUserRepository,
            IMemoryCache memoryCache,
            IRepository<Entities.SysUserToken> sysUserTokenRepository)
        {
            this._memoryCache = memoryCache;
            this._sysUserRepository = sysUserRepository;
            this._sysUserTokenRepository = sysUserTokenRepository;
        }

        /// <summary>
        /// 验证登录状态
        /// </summary>
        /// <param name="account">登录账号</param>
        /// <param name="password">登录密码</param>
        /// <param name="r">登录随机数</param>
        /// <returns></returns>
        public (bool Status, string Message, string Token, Entities.SysUser User) validateUser(string account, string password, string r)
        {
            //var md5Passowrd1 = Core.Librs.EncryptorHelper.GetMD5(password + r);
            var user = getByAccount(account);
            if(user==null)
                return (false, "用户名或密码错误", null, null);
            if(!user.Enabled)
            {
                return (false, "您的账号已被冻结", null, null);
            }
            if(user.LoginLock)
            {
                if( user.AllowLoginTime>DateTime.Now)
                {
                    return (false, "账号已被锁定"+((int)(user.AllowLoginTime-DateTime.Now).Value.TotalMinutes+1)+"分钟。", null, null);
                }
            }
            var md5Passowrd =Core.Librs.EncryptorHelper.GetMD5(user.Password + r);
            //匹配密码
            if(password.Equals(md5Passowrd, StringComparison.InvariantCultureIgnoreCase))
            {
                user.LoginLock = false;
                user.LoginFailedNum = 0;
                user.LastLoginTime = null;
                user.LastIpAddress = "";
                user.AllowLoginTime = DateTime.Now;
                //登录日志
                user.SysUserLoginLogs.Add(new SysUserLoginLog() {
                    Id = Guid.NewGuid(),
                    IpAddress = "",
                    LoginTime = DateTime.Now,
                    Message = "登录：成功"
                });
                //单点登录,移除旧的登录token

                var userToken = new SysUserToken() {
                    Id = Guid.NewGuid(),
                    ExpireTime=DateTime.Now.AddDays(15)
                };
                user.SysUserTokens.Add(userToken);
                _sysUserRepository.DbContext.SaveChanges();
                return (true, "登录成功", userToken.Id.ToString(), user);
            }
            else
            {
                //登录日志
                user.SysUserLoginLogs.Add(new SysUserLoginLog()
                {
                    Id = Guid.NewGuid(),
                    IpAddress = "",
                    LoginTime = DateTime.Now,
                    Message = "登录：密码错误"
                });
                user.LoginFailedNum++;
                if(user.LoginFailedNum>5)
                {
                    user.LoginLock = true;
                    user.AllowLoginTime = DateTime.Now.AddHours(2);
                }
                _sysUserRepository.DbContext.SaveChanges();
            }
            return (false, "用户名或密码错误", null,null);
        }

        /// <summary>
        /// 通过账号获取用户    
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public Entities.SysUser getByAccount(string account)
        {
            return _sysUserRepository.Table.FirstOrDefault(o=>o.Account==account&&!o.IsDeleted);
        }

        /// <summary>
        /// 通过当前登录用户的token 获取用户信息并缓存
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Entities.SysUser getLogged(string token)
        {
            SysUserToken userToken = null;
            Entities.SysUser sysUser = null;
            _memoryCache.TryGetValue<SysUserToken>(token, out userToken);
            

            Guid tokenId = Guid.Empty;

            if(userToken!=null)
            {
                _memoryCache.TryGetValue(string.Format(MODEL_KEY, userToken.SysUserId), out sysUser);
            }

            if (sysUser != null)
                return sysUser;

            if (Guid.TryParse(token, out tokenId)) {
                var tokenItem=  _sysUserTokenRepository.Table.Include(x => x.SysUser).FirstOrDefault(o=>o.Id==tokenId);
                if(tokenItem!=null)
                {
                    _memoryCache.Set(token, tokenItem, DateTimeOffset.Now.AddHours(4));
                    //缓存
                    _memoryCache.Set(string.Format(MODEL_KEY,tokenItem.SysUserId),tokenItem.SysUser,DateTimeOffset.Now.AddHours(4));
                    return tokenItem.SysUser;
                }
            }
            return null;
        }
    }
}
