using Microsoft.AspNetCore.Http;
using AspNetAssignments.DTOs.Common;

namespace AspNetAssignments.Services.Abstract;

/// <summary>
/// Контракт сервісу для збереження та видалення локальних файлів
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Зберігає переданий файл у вказану підпапку на сервері
    /// </summary>
    Task<FileResponseDTO> SaveFileAsync(IFormFile file, string subFolder);

    /// <summary>
    /// Видаляє файл з диска за відносним шляхом
    /// </summary>
    void DeleteFile(string relativePath);
}
