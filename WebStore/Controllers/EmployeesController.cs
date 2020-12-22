using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Infrastucture.Interfaces;
using WebStore.Models;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    //[Route("Users")]
    [Authorize]
    public class EmployeesController : Controller
    {
        private readonly IEmployeesData _Employees;
        public EmployeesController(IEmployeesData Employees) => _Employees = Employees;

        //[Route("List")]
        public IActionResult Index()
        {
            var employees = _Employees.Get();
            return View(employees);
        }

        //[Route("Info({id})")]
        public IActionResult Details(int id)
        {
            var employee = _Employees.Get(id);
            if (employee is not null)
                return View(employee);
            return NotFound();
        }
        #region Edit
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id is null)
                return View(new EmployeesViewModel());
            if (id < 0) return BadRequest();
            var employee = _Employees.Get((int)id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(new EmployeesViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age
            });
        }

        [HttpPost]
        public IActionResult Edit(EmployeesViewModel Model)
        {
            if (Model.Age == 16)
                ModelState.AddModelError("Age", "An employee`s age can`t be 16");

            if (Model.LastName == "Иванов" && Model.FirstName == "Иван")
                ModelState.AddModelError("", "This is a suspicious person.");

            if (!ModelState.IsValid) return View(Model);

            if(Model is null)
                throw new ArgumentNullException(nameof(Model));
            var employee = new Employee
            {
                Id = Model.Id,
                LastName = Model.LastName,
                FirstName = Model.FirstName,
                Patronymic = Model.Patronymic,
                Age = Model.Age
            };
            if (employee.Id == 0)
                _Employees.Add(employee);
            else
                _Employees.Update(employee);
            return RedirectToAction("Index");
        }
        #endregion
        #region Delete
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest();
            var employee = _Employees.Get(id);
            if (employee is null) return NotFound();
            return View(new EmployeesViewModel
            {
                Id = employee.Id,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                Patronymic = employee.Patronymic,
                Age = employee.Age
            });
        }
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _Employees.Delete(id);
            return RedirectToAction("Index");
        }
        #endregion

        #region Create
        public IActionResult Create() => View("Edit", new EmployeesViewModel());
        #endregion
    }
}

