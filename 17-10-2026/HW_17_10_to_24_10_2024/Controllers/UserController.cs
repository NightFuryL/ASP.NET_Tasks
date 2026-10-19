using HW_17_10_to_24_10_2026.DTO;
using HW_17_10_to_24_10_2026.DTO.Pagination;
using HW_17_10_to_24_10_2026.Services;
using Microsoft.AspNetCore.Mvc;

namespace HW_17_10_to_24_10_2026.Controllers;

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

    [HttpGet]
    public async Task<IActionResult> GetPagedUsers([FromQuery] UserQueryParams queryParams)
    {
        var pageResponse = await _userService.GetPagedAsync(queryParams);
        return Ok(pageResponse);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
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

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var result = await _userService.DeleteUserAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}
