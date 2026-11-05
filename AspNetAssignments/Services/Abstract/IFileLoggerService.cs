namespace AspNetAssignments.Services.Abstract;

/// <summary>
/// Контракт сервісу для збереження логів запитів у файл
/// </summary>
public interface IFileLoggerService
{
    /// <summary>
    /// Асинхронно записує інформацію про запит у лог-файл (timestamp, method, path, statusCode, durationMs, message)
    /// </summary>
    Task LogRequestAsync(string method, string path, int statusCode, long durationMs, string message);
}
