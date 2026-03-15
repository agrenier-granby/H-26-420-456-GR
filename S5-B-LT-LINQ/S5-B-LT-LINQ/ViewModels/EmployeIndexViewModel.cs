namespace S5_B_LT_LINQ.ViewModels
{
    public class EmployeIndexViewModel
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime DateEmbauche { get; set; }
        public double SalaireAnnuel { get; set; }
    }
}
