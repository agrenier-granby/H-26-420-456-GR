using System.ComponentModel.DataAnnotations;

namespace Exercice_Mapper.Models
{
    public class Employe
    {
        public int EmployeId { get; set; }

        [Required]
        public string Nom { get; set; } = default!;

        public int Age { get; set; }

        public DateTime DateEmbauche { get; set; }

        public int Salaire { get; set; }

        public bool EstEnEmploi { get; set; }

        public int PaysId { get; set; }
        public Pays PaysOrigine { get; set; } = default!;

        public int DepartementId { get; set; }
        public Departement Departement { get; set; } = default!;
    }
}