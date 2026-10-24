using Classwork_10_10_2026.Data;
using Classwork_10_10_2026.Mapping;
using Classwork_10_10_2026.Services;
using Microsoft.EntityFrameworkCore;

namespace Classwork_10_10_2026;
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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

        // Configure the HTTP request pipeline.
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
