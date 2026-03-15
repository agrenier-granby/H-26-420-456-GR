namespace S5_A_SEEDER.ViewModels
{
    public class EmployeCreateViewModel
    {
        public string Nom { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime DateEmbauche { get; set; }
        public double SalaireAnnuel { get; set; }
        public int PaysId { get; set; }
        public int DepartementId { get; set; }
    }
}
