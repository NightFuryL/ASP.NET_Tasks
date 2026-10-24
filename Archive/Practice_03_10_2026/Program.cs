using Microsoft.OpenApi;

namespace Practice_03_10_2026;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Practice_03_10_2026 Shop API",
                Description = "Авто-документація для проекту з продуктом (Product, OrderItem, Order)",
                Contact = new OpenApiContact
                {
                    Name = "Dev Team",
                    Email = "dev@shop.com"
                }
            });

            // Include XML comments if available for swagger documentation
            var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            if (File.Exists(xmlFilePath))
            {
                options.IncludeXmlComments(xmlFilePath);
            }
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Practice_03_10_2026 API v1");
                c.RoutePrefix = "swagger";
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
