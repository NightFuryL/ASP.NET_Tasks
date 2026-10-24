using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Classwork_25_10_2026.Data;
using Classwork_25_10_2026.Mapping;
using Classwork_25_10_2026.Middleware;
using Classwork_25_10_2026.Services;

namespace Classwork_25_10_2026;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Реєстрація валідаторів FluentValidation зі збірки
        builder.Services.AddValidatorsFromAssemblyContaining<Program>();

        // Додавання глобального ValidationFilterAttribute до контролерів (Classwork 25.10.2026)
        builder.Services.AddControllers(option =>
        {
            option.Filters.Add<ValidationFilterAttribute>();
        });

        builder.Services.AddOpenApi();

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(MappingProfile).Assembly);
        });

        builder.Services.AddScoped<IProductService, ProductService>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Проміжне ПЗ логування тривалості запитів (Classwork 25.10.2026)
        app.UseRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
