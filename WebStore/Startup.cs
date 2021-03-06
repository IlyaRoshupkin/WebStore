using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore
{
    public class Startup
    {
        private readonly IConfiguration _Configuration;

        public Startup(IConfiguration Configuration) => _Configuration = Configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseStaticFiles();

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
