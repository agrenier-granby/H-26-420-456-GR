namespace BackgroundTasks.Web.Exemple.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public bool IsActive { get; set; }

        public ICollection<Taxe>? Taxes { get; set; }

        public int? LegalAgeToBuy { get; set; }
    }
}
