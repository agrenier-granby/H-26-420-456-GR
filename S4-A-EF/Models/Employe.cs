namespace S4_A_EF.Models
{
    public class Employe
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public int Age { get; set; }
        public DateTime DateEmbauche { get; set; }
        public double SalaireAnnuel { get; set; }
        public int PaysId { get; set; }
        public Pays? Pays { get; set; }
    }
}
