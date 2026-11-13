using System.Net;
using System.Net.Http.Json;
using AspNetAssignments.DTOs.Order;
using AspNetAssignments.DTOs.User;
using Moq;

namespace AspNetAssignments.Tests.Integration.User;

public class UserControllerIntegrationTests : IClassFixture<UserManagerWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly UserManagerWebApplicationFactory _factory;

    public UserControllerIntegrationTests(UserManagerWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
        _factory.UserService.Reset();
        _factory.OrderService.Reset();
    }

    #region GET api/User (GetAll)

    [Fact]
    public async Task GetAll_WhenUsersExist_Returns200OK_WithUsersList()
    {
        // Arrange
        var expectedUsers = new List<UserDTO>
        {
            new UserDTO
            {
                Id = Guid.NewGuid(),
                Name = "Іван",
                LastName = "Петренко",
                Email = "ivan@test.com",
                Role = "User",
                Age = 22
            },
            new UserDTO
            {
                Id = Guid.NewGuid(),
                Name = "Олена",
                LastName = "Коваленко",
                Email = "olena@test.com",
                Role = "Admin",
                Age = 26
            }
        };

        _factory.UserService
            .Setup(service => service.GetAllUsersAsync())
            .ReturnsAsync(expectedUsers);

        // Act
        var response = await _client.GetAsync("api/User");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultDtos = await response.Content.ReadFromJsonAsync<List<UserDTO>>();
        Assert.NotNull(resultDtos);
        Assert.Equal(2, resultDtos.Count);
        _factory.UserService.Verify(service => service.GetAllUsersAsync(), Times.Once());
    }

    #endregion

    #region GET api/User/{id} (GetById)

    [Fact]
    public async Task GetById_WhenUserExists_Returns200OK_WithUserDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var expectedUser = new UserDTO
        {
            Id = userId,
            Name = "Test",
            LastName = "Test",
            Email = "test@test.com"
        };

        _factory.UserService
            .Setup(service => service.GetUserByIdAsync(userId))
            .ReturnsAsync(expectedUser);

        // Act
        var response = await _client.GetAsync($"api/User/{userId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultDto = await response.Content.ReadFromJsonAsync<UserDTO>();
        Assert.NotNull(resultDto);
        Assert.Equal(userId, resultDto.Id);
        Assert.Equal("test@test.com", resultDto.Email);
        _factory.UserService.Verify(service => service.GetUserByIdAsync(userId), Times.Once());
    }

    [Fact]
    public async Task GetById_WhenUserDoesNotExist_Returns404NotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _factory.UserService
            .Setup(service => service.GetUserByIdAsync(userId))
            .ReturnsAsync((UserDTO?)null);

        // Act
        var response = await _client.GetAsync($"api/User/{userId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        _factory.UserService.Verify(service => service.GetUserByIdAsync(userId), Times.Once());
    }

    #endregion

    #region GET api/User/{id}/orders (GetUserOrders)

    [Fact]
    public async Task GetUserOrders_WhenUserExists_Returns200OK_WithOrdersList()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new UserDTO
        {
            Id = userId,
            Name = "Іван",
            LastName = "Петренко",
            Email = "ivan@test.com"
        };
        var expectedOrders = new List<OrderDTO>
        {
            new OrderDTO(),
            new OrderDTO()
        };

        _factory.UserService
            .Setup(service => service.GetUserByIdAsync(userId))
            .ReturnsAsync(user);

        _factory.OrderService
            .Setup(service => service.GetOrdersByUserIdAsync(userId))
            .ReturnsAsync(expectedOrders);

        // Act
        var response = await _client.GetAsync($"api/User/{userId}/orders");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultOrders = await response.Content.ReadFromJsonAsync<List<OrderDTO>>();
        Assert.NotNull(resultOrders);
        Assert.Equal(2, resultOrders.Count);
        _factory.UserService.Verify(service => service.GetUserByIdAsync(userId), Times.Once());
        _factory.OrderService.Verify(service => service.GetOrdersByUserIdAsync(userId), Times.Once());
    }

    [Fact]
    public async Task GetUserOrders_WhenUserDoesNotExist_Returns404NotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _factory.UserService
            .Setup(service => service.GetUserByIdAsync(userId))
            .ReturnsAsync((UserDTO?)null);

        // Act
        var response = await _client.GetAsync($"api/User/{userId}/orders");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        _factory.OrderService.Verify(service => service.GetOrdersByUserIdAsync(It.IsAny<Guid>()), Times.Never());
    }

    #endregion

    #region POST api/User (Create)

    [Fact]
    public async Task Create_WhenFormDataIsValid_Returns201Created_WithUserDto()
    {
        // Arrange
        var createdUser = new UserDTO
        {
            Id = Guid.NewGuid(),
            Name = "Тарас",
            LastName = "Шевченко",
            Email = "taras@test.com",
            Role = "User",
            Age = 30
        };

        _factory.UserService
            .Setup(service => service.CreateUserAsync(It.IsAny<CreateUserDTO>()))
            .ReturnsAsync(createdUser);

        using var formData = new MultipartFormDataContent
        {
            { new StringContent("Тарас"), nameof(CreateUserDTO.Name) },
            { new StringContent("Шевченко"), nameof(CreateUserDTO.LastName) },
            { new StringContent("taras@test.com"), nameof(CreateUserDTO.Email) },
            { new StringContent("User"), nameof(CreateUserDTO.Role) },
            { new StringContent("30"), nameof(CreateUserDTO.Age) }
        };

        // Act
        var response = await _client.PostAsync("api/User", formData);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var resultDto = await response.Content.ReadFromJsonAsync<UserDTO>();
        Assert.NotNull(resultDto);
        Assert.Equal(createdUser.Id, resultDto.Id);
        Assert.Equal("taras@test.com", resultDto.Email);
        _factory.UserService.Verify(service => service.CreateUserAsync(It.IsAny<CreateUserDTO>()), Times.Once());
    }

    [Fact]
    public async Task Create_WhenFormDataIsInvalid_Returns400BadRequest()
    {
        // Arrange — передаємо порожнє ім'я та невалідний email
        using var formData = new MultipartFormDataContent
        {
            { new StringContent(""), nameof(CreateUserDTO.Name) },
            { new StringContent(""), nameof(CreateUserDTO.LastName) },
            { new StringContent("invalid-email"), nameof(CreateUserDTO.Email) },
            { new StringContent("-5"), nameof(CreateUserDTO.Age) }
        };

        // Act
        var response = await _client.PostAsync("api/User", formData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        _factory.UserService.Verify(service => service.CreateUserAsync(It.IsAny<CreateUserDTO>()), Times.Never());
    }

    #endregion

    #region PUT api/User/{id} (Update)

    [Fact]
    public async Task Update_WhenUserExistsAndFormDataIsValid_Returns200OK_WithUpdatedUserDto()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var updatedUser = new UserDTO
        {
            Id = userId,
            Name = "ОновленеІм'я",
            LastName = "ОновленеПрізвище",
            Email = "updated@test.com",
            Role = "Admin",
            Age = 28
        };

        _factory.UserService
            .Setup(service => service.UpdateUserAsync(userId, It.IsAny<UpdateUserDTO>()))
            .ReturnsAsync(updatedUser);

        using var formData = new MultipartFormDataContent
        {
            { new StringContent("ОновленеІм'я"), nameof(UpdateUserDTO.Name) },
            { new StringContent("ОновленеПрізвище"), nameof(UpdateUserDTO.LastName) },
            { new StringContent("updated@test.com"), nameof(UpdateUserDTO.Email) },
            { new StringContent("Admin"), nameof(UpdateUserDTO.Role) },
            { new StringContent("28"), nameof(UpdateUserDTO.Age) }
        };

        // Act
        var response = await _client.PutAsync($"api/User/{userId}", formData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultDto = await response.Content.ReadFromJsonAsync<UserDTO>();
        Assert.NotNull(resultDto);
        Assert.Equal(userId, resultDto.Id);
        Assert.Equal("updated@test.com", resultDto.Email);
        _factory.UserService.Verify(service => service.UpdateUserAsync(userId, It.IsAny<UpdateUserDTO>()), Times.Once());
    }

    [Fact]
    public async Task Update_WhenUserDoesNotExist_Returns404NotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _factory.UserService
            .Setup(service => service.UpdateUserAsync(userId, It.IsAny<UpdateUserDTO>()))
            .ReturnsAsync((UserDTO?)null);

        using var formData = new MultipartFormDataContent
        {
            { new StringContent("Іван"), nameof(UpdateUserDTO.Name) },
            { new StringContent("Петренко"), nameof(UpdateUserDTO.LastName) },
            { new StringContent("ivan@test.com"), nameof(UpdateUserDTO.Email) },
            { new StringContent("User"), nameof(UpdateUserDTO.Role) },
            { new StringContent("25"), nameof(UpdateUserDTO.Age) }
        };

        // Act
        var response = await _client.PutAsync($"api/User/{userId}", formData);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        _factory.UserService.Verify(service => service.UpdateUserAsync(userId, It.IsAny<UpdateUserDTO>()), Times.Once());
    }

    [Fact]
    public async Task Update_WhenFormDataIsInvalid_Returns400BadRequest()
    {
        // Arrange
        var userId = Guid.NewGuid();

        using var formData = new MultipartFormDataContent
        {
            { new StringContent(""), nameof(UpdateUserDTO.Name) },
            { new StringContent(""), nameof(UpdateUserDTO.LastName) },
            { new StringContent("wrong-email"), nameof(UpdateUserDTO.Email) }
        };

        // Act
        var response = await _client.PutAsync($"api/User/{userId}", formData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        _factory.UserService.Verify(
            service => service.UpdateUserAsync(It.IsAny<Guid>(), It.IsAny<UpdateUserDTO>()),
            Times.Never());
    }

    #endregion

    #region DELETE api/User/{id} (Delete)

    [Fact]
    public async Task Delete_WhenUserExists_Returns204NoContent()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _factory.UserService
            .Setup(service => service.DeleteUserAsync(userId))
            .ReturnsAsync(true);

        // Act
        var response = await _client.DeleteAsync($"api/User/{userId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        _factory.UserService.Verify(service => service.DeleteUserAsync(userId), Times.Once());
    }

    [Fact]
    public async Task Delete_WhenUserDoesNotExist_Returns404NotFound()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _factory.UserService
            .Setup(service => service.DeleteUserAsync(userId))
            .ReturnsAsync(false);

        // Act
        var response = await _client.DeleteAsync($"api/User/{userId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        _factory.UserService.Verify(service => service.DeleteUserAsync(userId), Times.Once());
    }

    #endregion
}