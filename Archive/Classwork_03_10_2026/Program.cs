using Microsoft.OpenApi;
using System.Security.Cryptography.X509Certificates;

namespace Classwork_03_10_2026;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddScoped<Services.Abstract.IProductService, Services.ProductService>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(static options => {
            //info about the API
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "Classwork_03_10_2026 API",
                Description = "Minishop - service for miniclients",
                Contact = new OpenApiContact
                {
                    Name = "Support",
                    Email = "support@testmail.com"
                }
            });
            //Include XML comments if available for swagger documentation
            var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
            if(File.Exists(xmlFilePath))
            {
                options.IncludeXmlComments(xmlFilePath);
            }

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });
            

        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Classwork_03_10_2026 API v1");
                c.RoutePrefix = "swagger"; //localhost:port/swagger
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
