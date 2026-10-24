using Microsoft.EntityFrameworkCore;
using ExtraAspNetAssignments.Data;
using ExtraAspNetAssignments.Mapping;
using ExtraAspNetAssignments.Services;
using ExtraAspNetAssignments.Services.Abstract;

namespace ExtraAspNetAssignments;

/// <summary>
/// Точка входу та налаштування додаткового проєкту
/// </summary>
public class Program
{
    /// <summary>
    /// Ініціалізація та конфігурація сервісів і конвеєра додатка
    /// </summary>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Реєстрація контролерів
        builder.Services.AddControllers();

        // Підключення до бази даних SQL Server через EF Core
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));

        /*
        // Альтернативне підключення до PostgreSQL:
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSqlConnection")));
        */

        // Реєстрація AutoMapper
        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(ExtraMappingProfile).Assembly);
        });

        // Реєстрація бізнес-сервісів у DI
        builder.Services.AddScoped<IProjectService, ProjectService>();
        builder.Services.AddScoped<ITaskItemService, TaskItemService>();

        // Налаштування Swagger документації
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
            {
                Title = "ExtraAspNetAssignments API",
                Version = "v1",
                Description = "REST API для управління проєктами, задачами, інвойсами та книгами"
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExtraAspNetAssignments API v1");
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
