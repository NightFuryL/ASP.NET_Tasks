using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using AspNetAssignments.DTOs.Common;
using AspNetAssignments.Services.Abstract;

namespace AspNetAssignments.Services;

/// <summary>
/// Сервіс для фізичного збереження та видалення файлів у каталозі wwwroot
/// </summary>
public class FileService : IFileService
{
    private readonly IWebHostEnvironment _env;

    public FileService(IWebHostEnvironment env)
    {
        _env = env;
    }

    /// <summary>
    /// Видаляє файл із файлової системи за вказаним відносним шляхом
    /// </summary>
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

    /// <summary>
    /// Зберігає вхідний файл у папку uploads та повертає інформацію про нього
    /// </summary>
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
