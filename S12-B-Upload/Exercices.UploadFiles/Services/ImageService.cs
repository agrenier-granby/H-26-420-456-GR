using Exercices.UploadFiles.Data;
using Exercices.UploadFiles.Models;
using Microsoft.EntityFrameworkCore;

namespace Exercices.UploadFiles.Services
{
    public class ImageService : IImageService
    {
        private readonly ApplicationDbContext _context;

        public ImageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Image>> GetAllAsync()
        {
            return await _context.Images.OrderByDescending(i => i.UploadedAt).ToListAsync();
        }

        public async Task<Image?> GetByIdAsync(int id)
        {
            return await _context.Images.FindAsync(id);
        }

        public async Task<Image> CreateAsync(Image image)
        {
            image.UploadedAt = DateTime.Now;
            _context.Images.Add(image);
            await _context.SaveChangesAsync();
            return image;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
                return false;

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
