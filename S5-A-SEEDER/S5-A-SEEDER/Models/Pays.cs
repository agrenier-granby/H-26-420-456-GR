namespace S5_A_SEEDER.Models
{
    public class Pays
    {
        public int Id { get; set; }
        public string Nom { get; set; } = string.Empty;
        public int SuperficieM2 { get; set; }
        public List<Employe> Employes { get; set; } = new List<Employe>();
    }
}
