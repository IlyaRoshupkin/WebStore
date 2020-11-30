using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Models;

namespace WebStore.Data
{
    public static class TestData
    {
        public static  List<Employee> Employees { get; } = new()
        {
            new Employee { Id = 1, LastName = "Ivanov", FirstName = "Ivan", Patronymic = "Ivanovich", Age = 28 },
            new Employee { Id = 2, LastName = "Petrov", FirstName = "Sergey", Patronymic = "Sergeevich", Age = 23 },
            new Employee { Id = 3, LastName = "Bodrova", FirstName = "Marina", Patronymic = "Antonovna", Age = 19 }
        };

    }
}
