namespace Exercice_Mapper.Models
{
    public class Pays
    {
        public int PaysId { get; set; }
        public string Nom { get; set; } = default!;
        public int Superficie { get; set; }
        public List<Employe> Employes { get; set; } = default!;
    }
}