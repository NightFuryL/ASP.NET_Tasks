using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using AspNetAssignments.Services.Abstract;

namespace AspNetAssignments.Tests.Integration.Product;

public class ProductWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IProductService> ProductService { get; set; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var productServiceDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IProductService));

            if (productServiceDescriptor != null)
            {
                services.Remove(productServiceDescriptor);
            }

            services.AddScoped(_ => ProductService.Object);
        });
    }
}