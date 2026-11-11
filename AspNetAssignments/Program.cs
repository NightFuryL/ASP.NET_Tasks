using AspNetAssignments.Data;
using AspNetAssignments.Filters;
using AspNetAssignments.Mapping;
using AspNetAssignments.Middleware;
using AspNetAssignments.Services;
using AspNetAssignments.Services.Abstract;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

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

            // Строка нижче потрібна для обмеження доступу до контролерів лише з дозволеного списку
            // Якщо список дозволених контролерів порожній, то доступ до всіх контролерів буде дозволений
            options.Conventions.Add(new OnlyAllowedControllersConvention("User"));
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
        builder.Services.AddSingleton<IFileLoggerService, FileLoggerService>();
        #region Додаткові сервіси
        // Реєстрація бізнес-сервісів у DI
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<ITaskItemService, TaskItemService>();
        #endregion

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

            c.AddSecurityDefinition("ApiKey", new Microsoft.OpenApi.OpenApiSecurityScheme
            {
                Description = "Введите API Ключ (your-api-key)",
                Name = "X-API-KEY",
                In = Microsoft.OpenApi.ParameterLocation.Header,
                Type = Microsoft.OpenApi.SecuritySchemeType.ApiKey
            });

            c.AddSecurityDefinition("UserId", new Microsoft.OpenApi.OpenApiSecurityScheme
            {
                Description = "Введите ID пользователя (GUID)",
                Name = "X-User-Id",
                In = Microsoft.OpenApi.ParameterLocation.Header,
                Type = Microsoft.OpenApi.SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(_ => new Microsoft.OpenApi.OpenApiSecurityRequirement
            {
                {
                    new Microsoft.OpenApi.OpenApiSecuritySchemeReference("ApiKey"),
                    new List<string>()
                },
                {
                    new Microsoft.OpenApi.OpenApiSecuritySchemeReference("UserId"),
                    new List<string>()
                }
            });
        });

        var app = builder.Build();

        // Глобальне перехоплення помилок
        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseBlockedUser();

        app.UseApiKeyAuthentication();

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
