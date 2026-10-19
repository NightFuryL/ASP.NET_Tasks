using HW_17_10_to_24_10_2026.Data;
using HW_17_10_to_24_10_2026.Mapping;
using HW_17_10_to_24_10_2026.Models;
using HW_17_10_to_24_10_2026.Services;
using Microsoft.EntityFrameworkCore;

namespace HW_17_10_to_24_10_2026;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                options.UseNpgsql(connectionString);
            }
        });

        builder.Services.AddAutoMapper(cfg =>
        {
            cfg.AddMaps(typeof(UserProfile).Assembly);
        });

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
