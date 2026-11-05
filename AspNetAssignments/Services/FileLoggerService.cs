using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using AspNetAssignments.Services.Abstract;

namespace AspNetAssignments.Services;

/// <summary>
/// Сервіс для збереження логів HTTP-запитів у файл
/// </summary>
public class FileLoggerService : IFileLoggerService
{
    private readonly string _logFilePath;
    private static readonly SemaphoreSlim _lock = new(1, 1);

    public FileLoggerService(IWebHostEnvironment env)
    {
        var logsDirectory = Path.Combine(env.ContentRootPath, "Logs");
        if (!Directory.Exists(logsDirectory))
        {
            Directory.CreateDirectory(logsDirectory);
        }
        _logFilePath = Path.Combine(logsDirectory, "requestsLog.json");
    }

    /// <summary>
    /// Записує структуру запиту у файл (timestamp, method, path, statusCode, durationMs, message)
    /// </summary>
    public async Task LogRequestAsync(string method, string path, int statusCode, long durationMs, string message)
    {
        var logEntry = new
        {
            timestamp = DateTime.UtcNow.ToString("dd-MM-yyyy:HH-mm-ss"),
            method = method,
            path = path,
            statusCode = statusCode,
            durationMs = durationMs,
            message = message
        };

        var jsonLine = JsonSerializer.Serialize(logEntry);
        //Трошки інший синтаксис для потокобезпечності  
        await _lock.WaitAsync();
        try
        {
            await File.AppendAllTextAsync(_logFilePath, jsonLine + Environment.NewLine);
        }
        finally
        {
            _lock.Release();
        }
    }
}
