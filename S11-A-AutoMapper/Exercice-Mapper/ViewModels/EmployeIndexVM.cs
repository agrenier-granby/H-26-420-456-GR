using System.ComponentModel.DataAnnotations;

namespace Exercice_Mapper.ViewModels
{
    public class EmployeIndexVM
    {
        public int Id { get; set; }

        public string Nom { get; set; } = default!;

        public int Age { get; set; }

        [Display(Name = "Date d'embauche")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        [DataType(DataType.DateTime)]
        public DateTime DateEmbauche { get; set; } = DateTime.Now.Date;             // Pour éviter de commencer au tout début

        [DataType(DataType.Currency)]
        public int Salaire { get; set; }

        [Display(Name = "Présentement à l'emploi")]
        public bool EstEnEmploi { get; set; }

        [Display(Name = "Pays d'origine")]
        public string Pays { get; set; } = default!;

        [Display(Name = "Département de travail")]
        public string Departement { get; set; } = default!;
    }
}
