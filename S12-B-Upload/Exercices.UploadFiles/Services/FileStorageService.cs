namespace Exercices.UploadFiles.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _environment;

        public FileStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Le fichier est vide ou invalide.");

            var uploadFolder = Path.Combine(_environment.WebRootPath, folder);
            
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return Path.Combine(folder, uniqueFileName).Replace("\\", "/");
        }

        public bool DeleteFile(string filePath)
        {
            var fullPath = Path.Combine(_environment.WebRootPath, filePath);
            
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            
            return false;
        }
    }
}
