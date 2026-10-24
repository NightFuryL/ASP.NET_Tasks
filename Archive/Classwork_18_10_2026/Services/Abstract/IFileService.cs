using Classwork_18_10_2026.DTO;

namespace Classwork_18_10_2026.Services.Abstract;
public interface IFileService
{
    Task<FileResponseDTO> SaveFileAsync(IFormFile file, string subFolder);
    void DeleteFile(string reletivePath);
}
