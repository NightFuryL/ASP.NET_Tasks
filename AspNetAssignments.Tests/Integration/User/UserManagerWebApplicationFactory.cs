using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using AspNetAssignments.Services.Abstract;

namespace AspNetAssignments.Tests.Integration.User;

public class UserManagerWebApplicationFactory : WebApplicationFactory<Program>
{
    public Mock<IUserService> UserService { get; set; } = new();
    public Mock<IOrderService> OrderService { get; set; } = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var userServiceDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IUserService));
            if (userServiceDescriptor != null)
            {
                services.Remove(userServiceDescriptor);
            }

            var orderServiceDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IOrderService));
            if (orderServiceDescriptor != null)
            {
                services.Remove(orderServiceDescriptor);
            }

            services.AddScoped(_ => UserService.Object);
            services.AddScoped(_ => OrderService.Object);
        });
    }
}