﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.DAL.Migrations;
using WebStore.Data;
using WebStore.Domain.Entityes.Identity;
using WebStore.Infrastucture.Conventions;
using WebStore.Infrastucture.Interfaces;
using WebStore.Infrastucture.Middleware;
using WebStore.Infrastucture.Services;
using WebStore.Infrastucture.Services.InSQL;

namespace WebStore
{
    public class Startup
    {
        private readonly IConfiguration _Configuration;

        public Startup(IConfiguration Configuration) => _Configuration = Configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(_Configuration.GetConnectionString("Default")));
            services.AddTransient<WebStoreDbInitializer>();
            //services.AddTransient<IService, ServiceImplementation>();
            //services.AddSingleton<IService, ServiceImplementation>();
            //services.AddScoped<IService, ServiceImplementation>();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<WebStoreDB>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(opt =>
            {
#if DEBUG
                opt.Password.RequiredLength = 3;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequiredUniqueChars = 3;
#endif
                opt.User.RequireUniqueEmail = false;
                opt.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTVWXYZ1234567890";
                
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.MaxFailedAccessAttempts = 10;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            });

            services.ConfigureApplicationCookie(opt =>
            {
                opt.Cookie.Name = "WebStore.GB";
                opt.Cookie.HttpOnly = true;
                opt.ExpireTimeSpan = TimeSpan.FromDays(10);

                opt.LoginPath = "/Account/Login";
                opt.LogoutPath = "/Account/Logout";
                opt.AccessDeniedPath = "/Account/AccessDenied";

                opt.SlidingExpiration = true;

            });

            services.AddTransient<IEmployeesData, InMemoryEmployeesData>();
            //services.AddTransient<IEmployeesData>(service => new InMemoryEmployeesData());
            
            //services.AddTransient<IProductData, InMemoryProductData>();
            services.AddTransient<IProductData, SQLProductData>();

            //services.AddControllersWithViews(IServiceCollection services)
            {
                services
                    .AddControllersWithViews(opt =>
                    {
                        //opt.Conventions.Add(new WebStoreControllerConvention());
                    })
                    .AddRazorRuntimeCompilation();
                }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WebStoreDbInitializer db)
        {
            db.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseMiddleware<TestMiddleware>();
            app.UseWelcomePage("/welcome");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/greetings", async ctx => 
                await ctx.Response.WriteAsync(_Configuration["greetings"]));

                endpoints.MapGet("/HelloWorld", async ctx =>
                await ctx.Response.WriteAsync("HelloWorld!"));

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //http://localhost:5000 -> controller == "Home" action == "Index"
                //http://localhost:5000/Products -> controller == "Products" action == "Index"
                //http://localhost:5000/Products/Page -> controller == "Products" action == "Index"
                //http://localhost:5000/Products/Page -> controller == "Products" action == "Index" id = "5"

            });
        }
    }
}
