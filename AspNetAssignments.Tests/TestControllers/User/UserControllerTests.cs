using AspNetAssignments.Controllers;
using AspNetAssignments.DTOs.Order;
using AspNetAssignments.DTOs.User;
using AspNetAssignments.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AspNetAssignments.Tests.TestControllers.User;

public class UserControllerTests
{
    private readonly UserController _usersController;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IOrderService> _orderServiceMock;

    public UserControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _orderServiceMock = new Mock<IOrderService>();

        _usersController = new UserController(
            _userServiceMock.Object,
            _orderServiceMock.Object);
    }

    #region GetAll

    [Fact]
    public async Task GetAll_WhenUsersExist_ReturnsOkObjectResult()
    {
        // Arrange
        var users = new List<UserDTO>
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
                Age = 25
            }
        };

        _userServiceMock
            .Setup(service => service.GetAllUsersAsync())
            .ReturnsAsync(users);

        // Act
        var result = await _usersController.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        var returnedUsers = Assert.IsAssignableFrom<IEnumerable<UserDTO>>(okResult.Value);
        Assert.Equal(2, returnedUsers.Count());
    }

    #endregion

    #region GetById

    [Fact]
    public async Task GetById_WhenUserExists_ReturnsOkObjectResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new UserDTO
        {
            Id = userId,
            Email = "test@test.com",
            Name = "Name",
            LastName = "lastName"
        };

        _userServiceMock
            .Setup(service => service.GetUserByIdAsync(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _usersController.GetById(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        var returnedUser = Assert.IsType<UserDTO>(okResult.Value);
        Assert.Equal(userId, returnedUser.Id);
    }

    [Fact]
    public async Task GetById_WhenUserDoesNotExist_ReturnsNotFoundObjectResult()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userServiceMock
            .Setup(service => service.GetUserByIdAsync(userId))
            .ReturnsAsync((UserDTO?)null);

        // Act
        var result = await _usersController.GetById(userId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    #endregion

    #region GetUserOrders

    [Fact]
    public async Task GetUserOrders_WhenUserExists_ReturnsOkObjectResult()
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
        var orders = new List<OrderDTO>
        {
            new OrderDTO(),
            new OrderDTO()
        };

        _userServiceMock
            .Setup(service => service.GetUserByIdAsync(userId))
            .ReturnsAsync(user);

        _orderServiceMock
            .Setup(service => service.GetOrdersByUserIdAsync(userId))
            .ReturnsAsync(orders);

        // Act
        var result = await _usersController.GetUserOrders(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        var returnedOrders = Assert.IsAssignableFrom<IEnumerable<OrderDTO>>(okResult.Value);
        Assert.Equal(2, returnedOrders.Count());
        _orderServiceMock.Verify(service => service.GetOrdersByUserIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task GetUserOrders_WhenUserDoesNotExist_ReturnsNotFoundObjectResult()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userServiceMock
            .Setup(service => service.GetUserByIdAsync(userId))
            .ReturnsAsync((UserDTO?)null);

        // Act
        var result = await _usersController.GetUserOrders(userId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        _orderServiceMock.Verify(service => service.GetOrdersByUserIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    #endregion

    #region Create

    [Fact]
    public async Task Create_WhenModelIsValid_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var createDto = new CreateUserDTO
        {
            Name = "Тарас",
            LastName = "Шевченко",
            Email = "taras@test.com",
            Role = "User",
            Age = 30
        };

        var createdUser = new UserDTO
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name,
            LastName = createDto.LastName,
            Email = createDto.Email,
            Role = createDto.Role,
            Age = createDto.Age
        };

        _userServiceMock
            .Setup(service => service.CreateUserAsync(createDto))
            .ReturnsAsync(createdUser);

        // Act
        var result = await _usersController.Create(createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal(nameof(UserController.GetById), createdResult.ActionName);
        var returnedUser = Assert.IsType<UserDTO>(createdResult.Value);
        Assert.Equal(createdUser.Id, returnedUser.Id);
    }

    [Fact]
    public async Task Create_WhenModelStateIsInvalid_ReturnsBadRequestObjectResult()
    {
        // Arrange
        var createDto = new CreateUserDTO();
        _usersController.ModelState.AddModelError("Email", "Email є обов'язковим.");

        // Act
        var result = await _usersController.Create(createDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        _userServiceMock.Verify(service => service.CreateUserAsync(It.IsAny<CreateUserDTO>()), Times.Never);
    }

    #endregion

    #region Update

    [Fact]
    public async Task Update_WhenUserExistsAndModelIsValid_ReturnsOkObjectResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var updateDto = new UpdateUserDTO
        {
            Name = "ОновленеІм'я",
            LastName = "ОновленеПрізвище",
            Email = "updated@test.com",
            Role = "Admin",
            Age = 28
        };

        var updatedUser = new UserDTO
        {
            Id = userId,
            Name = updateDto.Name,
            LastName = updateDto.LastName,
            Email = updateDto.Email,
            Role = updateDto.Role,
            Age = updateDto.Age
        };

        _userServiceMock
            .Setup(service => service.UpdateUserAsync(userId, updateDto))
            .ReturnsAsync(updatedUser);

        // Act
        var result = await _usersController.Update(userId, updateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        var returnedUser = Assert.IsType<UserDTO>(okResult.Value);
        Assert.Equal(updateDto.Email, returnedUser.Email);
    }

    [Fact]
    public async Task Update_WhenUserDoesNotExist_ReturnsNotFoundObjectResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var updateDto = new UpdateUserDTO
        {
            Name = "Іван",
            LastName = "Петренко",
            Email = "ivan@test.com",
            Age = 25
        };

        _userServiceMock
            .Setup(service => service.UpdateUserAsync(userId, updateDto))
            .ReturnsAsync((UserDTO?)null);

        // Act
        var result = await _usersController.Update(userId, updateDto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task Update_WhenModelStateIsInvalid_ReturnsBadRequestObjectResult()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var updateDto = new UpdateUserDTO();
        _usersController.ModelState.AddModelError("Name", "Ім'я є обов'язковим.");

        // Act
        var result = await _usersController.Update(userId, updateDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        _userServiceMock.Verify(service => service.UpdateUserAsync(It.IsAny<Guid>(), It.IsAny<UpdateUserDTO>()), Times.Never);
    }

    #endregion

    #region Delete

    [Fact]
    public async Task Delete_WhenUserExists_ReturnsNoContentResult()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userServiceMock
            .Setup(service => service.DeleteUserAsync(userId))
            .ReturnsAsync(true);

        // Act
        var result = await _usersController.Delete(userId);

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, noContentResult.StatusCode);
    }

    [Fact]
    public async Task Delete_WhenUserDoesNotExist_ReturnsNotFoundObjectResult()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _userServiceMock
            .Setup(service => service.DeleteUserAsync(userId))
            .ReturnsAsync(false);

        // Act
        var result = await _usersController.Delete(userId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    #endregion
}