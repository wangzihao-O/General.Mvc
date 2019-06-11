using General.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace General.Entities
{
    public class GeneralDbContext:DbContext
    {
        public GeneralDbContext(DbContextOptions options) :base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SysUser> SysUsers { get; set; }
        public DbSet<SysUserToken> SysUserTokens { get; set; }
        public DbSet<SysUserLoginLog> SysUserLoginLogs { get; set; }
    }
}
