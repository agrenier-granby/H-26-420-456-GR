namespace BackgroundTasks.Web.Exemple.Models
{
    public class Taxe
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public decimal Value { get; set; }

        public int Order { get; set; }
    }
}
