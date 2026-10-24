using FluentValidation;
using Microsoft.EntityFrameworkCore;
using AspNetAssignments.Data;
using AspNetAssignments.Filters;
using AspNetAssignments.Mapping;
using AspNetAssignments.Middleware;
using AspNetAssignments.Services;
using AspNetAssignments.Services.Abstract;

namespace AspNetAssignments;

/// <summary>
/// Точка входу та конфігурація сервісів і конвеєра веб-додатка
/// </summary>
public class Program
{
    /// <summary>
    /// Ініціалізація та запуск веб-сервера
    /// </summary>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Реєстрація всіх валідаторів FluentValidation
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();

        // Реєстрація контролерів із глобальним фільтром валідації
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilterAttribute>();
        });

        // Підключення до бази даних SQL Server через EF Core
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));

        /*
        // Альтернативне підключення до бази даних PostgreSQL:
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));
        */

        // Реєстрація профілів AutoMapper
        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(MappingProfile).Assembly);
        });

        // Реєстрація бізнес-сервісів у DI контейнері
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IFileService, FileService>();
        builder.Services.AddScoped<IOrderService, OrderService>();

        // Налаштування Swagger документації API
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
            {
                Title = "AspNetAssignments API",
                Version = "v1",
                Description = "REST API сервіс для управління товарами, користувачами та замовленнями"
            });
        });

        var app = builder.Build();

        // Глобальне перехоплення помилок
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        // Логування запитів із вимірюванням тривалості
        app.UseRequestLogging();

        // Базове логування вхідних запитів
        app.UseMiddleware<LoggingMiddleware>();

        /*
        // Опціональне проміжне ПЗ для перевірки API ключа:
        // app.UseMiddleware<ApiKeyMiddleware>();
        */

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AspNetAssignments API v1");
            });
        }

        app.UseHttpsRedirection();

        // Роздача статичних файлів із wwwroot
        app.UseStaticFiles();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
