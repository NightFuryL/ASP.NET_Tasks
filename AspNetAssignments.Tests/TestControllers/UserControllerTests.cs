using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using AspNetAssignments.Controllers;
using AspNetAssignments.DTOs.User;
using AspNetAssignments.Models.User;
using AspNetAssignments.Services;
using AspNetAssignments.Services.Abstract;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace AspNetAssignments.Tests.TestControllers;

public class UserControllerTests
{
    private readonly UserController _usersController;
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IOrderService> _orderServiceMock;
    private readonly Mock<IMapper> _mapperMock;

    public UserControllerTests()
    {
        _mapperMock = new Mock<IMapper>();
        _userServiceMock = new Mock<IUserService>();
        _orderServiceMock = new Mock<IOrderService>();

        _usersController = new UserController(
            _userServiceMock.Object,
            _orderServiceMock.Object);
    }

    [Fact]
    public async Task GetById_WhenUserExist_ReturnsOkObjectResult()
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
    }
}
