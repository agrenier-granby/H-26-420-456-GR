using Microsoft.AspNetCore.Mvc.Rendering;

namespace S5_B_LT_LINQ.ViewModels
{
    public class EmployeFormViewModel
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime DateEmbauche { get; set; }
        public double SalaireAnnuel { get; set; }
        public int PaysId { get; set; }
        public int DepartementId { get; set; }
        public List<SelectListItem> PaysList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> DepartementsList { get; set; } = new List<SelectListItem>();
    }
}
