using Microsoft.AspNetCore.Mvc;
using S4_A_MVC.Models;
using S4_A_MVC.ViewModels;

namespace S4_A_MVC.Controllers
{
    public class EmployeesController : Controller
    {
        private static IList<Employee> GenerateEmployees()
        {
            return
            [
                new Employee() { Id = 1, Name = "Antoine", HiringDate = DateTimeOffset.Parse("2016-11-23"), YearlySalary = 2000000 },
                new Employee() { Id = 2, Name = "Benoit", HiringDate = DateTimeOffset.Parse("2006-08-17"), YearlySalary = 70000 },
                new Employee() { Id = 3, Name = "Charles", HiringDate = DateTimeOffset.Parse("2014-03-14"), YearlySalary = 60000 },
                new Employee() { Id = 4, Name = "Denis", HiringDate = DateTimeOffset.Parse("2019-01-22"), YearlySalary = 19000 },
                //new Employee() { Id = 5, Name = "Émile", HiringDate = DateTimeOffset.Parse("2017-06-01"), YearlySalary = 84000 },
                //new Employee() { Id = 6, Name = "Fanny", HiringDate = DateTimeOffset.Parse("2022-07-12"), YearlySalary = 30000 },
                //new Employee() { Id = 7, Name = "Gaetan", HiringDate = DateTimeOffset.Parse("2020-12-07"), YearlySalary = 40000 },
                //new Employee() { Id = 8, Name = "Hugo", HiringDate = DateTimeOffset.Parse("2003-08-23"), YearlySalary = 80000 },
                //new Employee() { Id = 9, Name = "Ibrahem", HiringDate = DateTimeOffset.Parse("2018-04-09"), YearlySalary = 65000 },
                //new Employee() { Id = 10, Name = "Jonathan", HiringDate = DateTimeOffset.Parse("2016-01-19"), YearlySalary = 70000 }
            ];
        }

        private string EmployeesToString(IList<Employee> employees)
        {
            return string.Join("\r\n", employees.Select(e => e.ToString()));
        }

        // Route: /employees
        // Route: /employees/index
        // Route: /employees/index/2
        // Route: /employees/index?id=2
        // Route: /employees?id=2
        public IActionResult Index(int? id)
        {
            var employees = GenerateEmployees();

            if (id.HasValue)
            {
                var employee = employees.FirstOrDefault(e => e.Id == id.Value);
                if (employee == null)
                {
                    return Content(string.Empty);
                }
                return Content(employee.ToString());
            }

            var viewModel = new EmployeesIndexVM { Employees = employees };
            return View(viewModel);
        }

        // Route: /employees/aleatoire
        [Route("employees/aleatoire")]
        public IActionResult Aleatoire()
        {
            var employees = GenerateEmployees();
            var random = new Random();
            var randomEmployee = employees[random.Next(employees.Count)];
            return Content(randomEmployee.ToString());
        }

        // Route: /employees/chercher/[Nom]
        [Route("employees/chercher/{nom}")]
        public IActionResult Chercher(string nom)
        {
            var employees = GenerateEmployees();
            var employee = employees.FirstOrDefault(e => e.Name.Equals(nom, StringComparison.OrdinalIgnoreCase));
            
            if (employee == null)
            {
                return NotFound();
            }

            return Content(employee.ToString());
        }

        // Route: /employees/dateembauche/[annee]/[mois]
        [Route("employees/dateembauche/{annee:int:length(4)}/{mois:int:range(1,12)}")]
        public IActionResult DateEmbauche(int annee, int mois)
        {
            var employees = GenerateEmployees();
            var filteredEmployees = employees
                .Where(e => e.HiringDate.Year == annee && e.HiringDate.Month == mois)
                .ToList();

            if (!filteredEmployees.Any())
            {
                return new EmptyResult();
            }

            return Content(EmployeesToString(filteredEmployees));
        }

        // Route: /employees/moyenne/[propriete]
        [Route("employees/moyenne/{propriete}")]
        public IActionResult Moyenne(string propriete)
        {
            var employees = GenerateEmployees();

            if (propriete.Equals("salaire", StringComparison.OrdinalIgnoreCase))
            {
                var moyenne = employees.Average(e => e.YearlySalary);
                return Content(moyenne.ToString());
            }
            else if (propriete.Equals("experience", StringComparison.OrdinalIgnoreCase))
            {
                var moyenne = employees.Average(e => e.YearsOfExperience);
                return Content(moyenne.ToString());
            }

            return Content(string.Empty);
        }
    }
}
