namespace S5_A_SEEDER.ViewModels
{
    public class EmployeDeleteViewModel
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime DateEmbauche { get; set; }
        public double SalaireAnnuel { get; set; }
        public string? PaysOrigineNom { get; set; }
        public string? DepartementNom { get; set; }
    }
}
