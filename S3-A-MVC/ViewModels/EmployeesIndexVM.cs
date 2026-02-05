using S3_A_MVC.Models;

namespace S3_A_MVC.ViewModels
{
    public class EmployeesIndexVM
    {
        public IList<Employee> Employees { get; set; } = new List<Employee>();
    }
}
