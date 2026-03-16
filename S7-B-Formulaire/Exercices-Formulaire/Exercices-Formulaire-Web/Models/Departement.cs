namespace Exercices_Formulaire_Web.Models
{
    public class Departement
    {
        public int Id { get; set; }

        public string Nom { get; set; } = default!;

        public double Budget { get; set; }

        public List<Employe> Employes { get; set; } = default!;
    }
}