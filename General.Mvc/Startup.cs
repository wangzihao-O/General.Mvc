using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using General.Core;
using General.Core.Data;
using General.Core.Librs;
using General.Entities;
using General.Services.Category;
using General.Core.Data.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using General.Framework;
using General.Framework.Infrastructure;
using General.Framework.Security.Admin;
using Microsoft.Extensions.Caching.Memory;

namespace General.Mvc
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            

            //数据库依赖注入
            services.AddDbContextPool<GeneralDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddSession();
            //权限注入
            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = CookieAdminAuthInfo.AuthenticationScheme;
                o.DefaultChallengeScheme = CookieAdminAuthInfo.AuthenticationScheme;
            }).AddCookie(CookieAdminAuthInfo.AuthenticationScheme, o =>
            {
                o.LoginPath = "/Admin/Login";  
            });

            

            //单例注入
            //services.AddScoped<ICategoryService, CategoryService>();
            //提供者服务单例注入
            //services.BuildServiceProvider().GetService<ICategoryService>();


            //程序集依赖注入
            services.AddAssembly("General.Services");

            

            //泛型注入到DI内
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            services.AddHttpContextAccessor();
            services.AddScoped<IWorkContext, WorkContext>();
            services.AddScoped<IAdminAuthService, AdminAuthService>();
            services.AddSingleton<IMemoryCache, MemoryCache>();
            

            //引擎上下文提供服务初始化注入
            EnginContext.Initialize(new GeneralEngine(services.BuildServiceProvider()));
            //登录用户上下文

            //创建服务对象
            //new GeneralEngine(services.BuildServiceProvider());
            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            //权限
            app.UseAuthentication();

            app.UseCookiePolicy();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Login}/{action=Index}/{id?}"
                );
            });
        }
    }
}
