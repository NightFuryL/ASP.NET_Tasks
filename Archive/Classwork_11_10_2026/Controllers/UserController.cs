using Classwork_11_10_2026.DTO;
using Classwork_11_10_2026.Services;
using Microsoft.AspNetCore.Mvc;

namespace Classwork_11_10_2026.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UserController : Controller
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }
    [HttpGet("{id}")]

    public async Task<IActionResult> GetUserById(Guid id)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        var user = await _userService.CreateUserAsync(createUserDto);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
