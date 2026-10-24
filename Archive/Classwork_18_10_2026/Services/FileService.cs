using Classwork_18_10_2026.DTO;
using Classwork_18_10_2026.Services.Abstract;

namespace Classwork_18_10_2026.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;
    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }
    public void DeleteFile(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath)) return;
        var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var cleanRelativePath = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(webRootPath, cleanRelativePath);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    public async Task<FileResponseDTO> SaveFileAsync(IFormFile file, string subFolder)
    {
        var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadsFolder = Path.Combine(webRootPath, "uploads", subFolder);
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(uploadsFolder, uniqueFileName);
        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
        var relativePath = $"/uploads/{subFolder}/{uniqueFileName}";
        return new FileResponseDTO
        {
            RelativePath = relativePath,
            FileName = uniqueFileName,
            FileSize = file.Length
        };

    }
}
