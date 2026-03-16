using System.ComponentModel.DataAnnotations;

namespace Exercices_Formulaire_Web.Models
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class DateDansLePasseAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is null)
            {
                return true;
            }

            if (value is DateTime date)
            {
                return date.Date < DateTime.Today;
            }

            return false;
        }
    }
}
