using Exercices.UploadFiles.Models;

namespace Exercices.UploadFiles.Services
{
    public interface IImageService
    {
        Task<List<Image>> GetAllAsync();
        Task<Image?> GetByIdAsync(int id);
        Task<Image> CreateAsync(Image image);
        Task<bool> DeleteAsync(int id);
    }
}
