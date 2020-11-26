using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _Configuration;
        private static readonly List<Employee> __Employees = new()
        {
            new Employee() { Id = 1, LastName = "Ivanov", FirstName = "Ivan", Patronymic = "Ivanovich", Age = 28 },
            new Employee() { Id = 2, LastName = "Petrov", FirstName = "Sergey", Patronymic = "Sergeevich", Age = 23 },
            new Employee() { Id = 3, LastName = "Bodrova", FirstName = "Marina", Patronymic = "Antonovna", Age = 19 }
        };

        public HomeController(IConfiguration Configuration) => _Configuration = Configuration;

        public IActionResult Index()
        {
            // Data processing
            //return Content(_Configuration["ControllerActionText"]);
            return View();
        }

        public IActionResult SecondAction()
        {
            return Content("Second controller action");
        }

        public IActionResult Employees()
        {
            return View(__Employees );
        }
    }
}
