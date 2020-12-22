using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using WebStore.DAL.Context;
using WebStore.Domain.Entityes.Identity;

namespace WebStore.Data
{
    public class WebStoreDbInitializer
    {
        private readonly WebStoreDB _db;
        private readonly RoleManager<Role> _RoleManager;
        private readonly UserManager<User> _UserManager;
        private readonly ILogger<WebStoreDbInitializer> _Logger;

        public WebStoreDbInitializer(WebStoreDB db, 
            UserManager<User> UserManager,
            RoleManager<Role> RoleManager,
            ILogger<WebStoreDbInitializer> Logger)
        {
            _db = db;
            _RoleManager = RoleManager;
            _UserManager = UserManager;
            _Logger = Logger; 
        }
        public void Initialize()
        {
            _Logger.LogInformation("Initialization of DB...");
            var db = _db.Database;

            if (db.GetPendingMigrations().Any())
            {
                _Logger.LogInformation("There are no applied migrations...");
                db.Migrate();
                _Logger.LogInformation("Migrations of DB have been completed successfully.");
            }
            else
                _Logger.LogInformation("The DB`s stucture in an actual status.");

            try
            {
                InitializeProducts();
            }
            catch(Exception e)
            {
                _Logger.LogError(e, "Error during initialisation DB by products shop data.");
                throw;
            }

            try
            {
                InitializeIdentityAsync().Wait();
            }
            catch(Exception e)
            {
                _Logger.LogError(e, "Error during initialisation Identity system DB");
                throw;
            }
        }

        private void InitializeProducts()
        {
            var timer = Stopwatch.StartNew();
            if (_db.Products.Any())
            {
                _Logger.LogInformation("The addition of source data in DB is not required.");
                return;
            }

            _Logger.LogInformation("Addition of sections...");
            using (_db.Database.BeginTransaction())
            {
                _db.Sections.AddRange(TestData.Sections);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] ON");
                _db.SaveChanges();
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Sections] OFF");

                _db.Database.CommitTransaction();
            }
            _Logger.LogInformation("Addition of brands...");
            using (_db.Database.BeginTransaction())
            {
                _db.Brands.AddRange(TestData.Brands);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] ON");
                _db.SaveChanges();
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Brands] OFF");

                _db.Database.CommitTransaction();
            }
            _Logger.LogInformation("Addition of products...");
            using (_db.Database.BeginTransaction())
            {
                _db.Products.AddRange(TestData.Products);

                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] ON");
                _db.SaveChanges();
                _db.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [dbo].[Products] OFF");

                _db.Database.CommitTransaction();
            }
            _Logger.LogInformation("Addition of source data has been completed successfully for {0} ms.",timer.ElapsedMilliseconds);
        }

        private async Task InitializeIdentityAsync()
        {
            async Task CheckRole(string RoleName)
            {
                if (!await _RoleManager.RoleExistsAsync(RoleName))
                    await _RoleManager.CreateAsync(new Role { Name = RoleName });
            }

            await CheckRole(Role.Administrator);
            await CheckRole(Role.User);

            if(await _UserManager.FindByNameAsync(User.Administrator) is null)
            {
                var admin = new User
                {
                    UserName = User.Administrator
                };
                var creation_result = await _UserManager.CreateAsync(admin, User.DefaultAdminPassword);
                if (creation_result.Succeeded)
                    await _UserManager.AddToRoleAsync(admin, Role.Administrator);
                else
                {
                    var errors = creation_result.Errors.Select(e => e.Description);
                    throw new InvalidOperationException($"The error during creation of administrator`s account " +
                        $"{string.Join(",", errors)}");
                }
            }
        }
    }
}
