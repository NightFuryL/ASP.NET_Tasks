using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using AspNetAssignments.DTOs.User;
using AspNetAssignments.Services.Abstract;

namespace AspNetAssignments.Controllers;

/// <summary>
/// Управління користувачами та профілями з можливістю завантаження аватара
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Отримати перелік усіх зареєстрованих користувачів
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<UserDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    /// <summary>
    /// Отримати користувача за унікальним ідентифікатором
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return NotFound(new { message = $"Користувача з ID {id} не знайдено." });
        }
        return Ok(user);
    }

    /// <summary>
    /// Створити нового користувача (підтримує передачу форми з аватаром та валідацію через FluentValidation)
    /// </summary>
    [HttpPost]
    [Consumes("multipart/form-data", "application/json")]
    [ProducesResponseType(typeof(UserDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromForm] CreateUserDTO dto,
        [FromServices] IValidator<CreateUserDTO>? validator = null)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdUser = await _userService.CreateUserAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
    }

    /// <summary>
    /// Оновити дані користувача та аватар (підтримує валідацію через FluentValidation)
    /// </summary>
    [HttpPut("{id:guid}")]
    [Consumes("multipart/form-data", "application/json")]
    [ProducesResponseType(typeof(UserDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromForm] UpdateUserDTO dto,
        [FromServices] IValidator<UpdateUserDTO>? validator = null)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedUser = await _userService.UpdateUserAsync(id, dto);
        if (updatedUser == null)
        {
            return NotFound(new { message = $"Користувача з ID {id} не знайдено." });
        }

        return Ok(updatedUser);
    }

    /// <summary>
    /// Видалити користувача (Soft delete)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result)
        {
            return NotFound(new { message = $"Користувача з ID {id} не знайдено." });
        }

        return NoContent();
    }
}
