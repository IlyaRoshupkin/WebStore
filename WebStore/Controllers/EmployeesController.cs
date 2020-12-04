using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Data;
using WebStore.Models;

namespace WebStore.Controllers
{
    //[Route("Users")]
    public class EmployeesController : Controller
    {
        private List<Employee> __Employees;
        public EmployeesController() => __Employees = TestData.Employees;
        //[Route("List")]
        public IActionResult Index() => View(__Employees);

        //[Route("Info({id})")]
        public IActionResult Details(int id)
        {
            var employee = TestData.Employees.FirstOrDefault(e => e.Id == id);
            if (employee is not null)
                return View(employee);
            return NotFound();
        }
        //[Route("Edit({id})")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = __Employees.Find(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            __Employees.Remove(employee);
            return View(employee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Route("Edit({id})")]
        public IActionResult Edit(int id, [Bind("Id,LastName,FirstName,Patronymic,Age")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                __Employees.Add(employee);
                __Employees.Sort((e1, e2) => e1.Id.CompareTo(e2.Id));
                return View(employee);
            }
            return NotFound();
        }
        //[Route("Delete({id})")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = __Employees
                .FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            __Employees.Remove(employee);
            __Employees.Sort((e1, e2) => e1.Id.CompareTo(e2.Id));

            return RedirectToAction("Index");
        }
    }
}
