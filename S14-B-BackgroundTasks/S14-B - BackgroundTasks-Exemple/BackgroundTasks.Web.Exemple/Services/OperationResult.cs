namespace BackgroundTasks.Web.Exemple.Services
{
    public class OperationResult<T> where T : class
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T Entity { get; set; } = default!;
    }
}
