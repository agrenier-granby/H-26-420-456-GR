namespace S4_A_EF.ViewModels
{
    public class EmployeCreateViewModel
    {
        public string Nom { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime DateEmbauche { get; set; }
        public double SalaireAnnuel { get; set; }
        public int PaysId { get; set; }
    }
}
