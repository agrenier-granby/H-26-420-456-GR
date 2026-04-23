using System.ComponentModel.DataAnnotations;

namespace Exercices.UploadFiles.Models
{
    public class UploadImageViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [Length(0, 5, ErrorMessage = "La limite est de 5 images")]
        public List<IFormFile> Images { get; set; } = new();
    }
}
