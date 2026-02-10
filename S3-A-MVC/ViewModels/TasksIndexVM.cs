using S3_A_MVC.Models;

namespace S3_A_MVC.ViewModels
{
    public class TasksIndexVM
    {
        public IList<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public string? Category { get; set; }
    }
}
