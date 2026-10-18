using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HW_10_10_to_17_10_2026.Data;

namespace HW_10_10_to_17_10_2026.Controllers;

[ApiController]
[Route("api/[controller]")]
//не знав як краще за всього перевірити підключення, тому зробив із за допомогою ШІ,
//тут максимум із такого я міг забути або не знати це пару строк, але все одно зробив так
public class DatabaseController : ControllerBase
{
    private readonly AppDbContext _context;

    public DatabaseController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("check-connection")]
    public async Task<IActionResult> CheckConnection()
    {
        var canConnect = await _context.Database.CanConnectAsync();
        if (canConnect)
        {
            return Ok(new
            {
                Status = "Success",
                Message = "Підключення до бази даних успішно встановлено.",
                DatabaseName = _context.Database.GetDbConnection().Database
            });
        }

        return StatusCode(500, new
        {
            Status = "Error",
            Message = "Не вдалося підключитися до бази даних."
        });
    }

    [HttpGet("test-query")]
    public async Task<IActionResult> TestQuery()
    {
        var canConnect = await _context.Database.CanConnectAsync();
        if (!canConnect)
        {
            return StatusCode(500, new
            {
                Status = "Error",
                Message = "База даних недоступна."
            });
        }

        var clientsCount = await _context.Clients.CountAsync();
        return Ok(new
        {
            Status = "Success",
            Message = "База даних доступна для виконання запитів.",
            ClientsCount = clientsCount
        });
    }
}
