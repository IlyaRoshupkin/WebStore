using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.DAL.Context;
using WebStore.Data;
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

            app.UseRouting();
            app.UseStaticFiles();
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
