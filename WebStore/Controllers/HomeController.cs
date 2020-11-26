using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _Configuration;

        public HomeController(IConfiguration Configuration) => _Configuration = Configuration;

        public IActionResult Index()
        {
            // Data processing
            return Content(_Configuration["ControllerActionText"]);
        }

        public IActionResult SecondAction()
        {
            return Content("Second controller action");
        }
    }
}
