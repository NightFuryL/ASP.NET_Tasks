using Moq;
using System.Net;
using System.Net.Http.Json;
using AspNetAssignments.DTOs.User;
using AspNetAssignments.Services;

namespace AspNetAssignments.Tests.Integration;

public class UserControllerIntegrationTests : IClassFixture<UserManagerWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly UserManagerWebApplicationFactory _factory;

    public UserControllerIntegrationTests(UserManagerWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
        _factory.UserService.Reset();
    }

    [Fact]
    public async Task GetById_WhenUserExist_Returns200OK_WithUserDto()
    {
        var userId = Guid.NewGuid();
        var expectedUser = new UserDTO
        {
            Id = userId,
            Name = "Test",
            Email = "test@test.com",
            LastName = "Test"
        };

        _factory.UserService
            .Setup(repo => repo.GetUserByIdAsync(userId))
            .ReturnsAsync(expectedUser);

        var response = await _client.GetAsync($"api/User/{userId}");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultDto = await response.Content.ReadFromJsonAsync<UserDTO>();
        Assert.NotNull(resultDto);
        Assert.Equal(userId, resultDto.Id);
        Assert.Equal("test@test.com", resultDto.Email);
        _factory.UserService.Verify(repo => repo.GetUserByIdAsync(userId), Times.Once());
    }
}