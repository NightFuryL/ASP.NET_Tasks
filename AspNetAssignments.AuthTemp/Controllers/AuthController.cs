using AspNetAssignments.AuthTemp.DTO;
using AspNetAssignments.AuthTemp.Entity;
using AspNetAssignments.AuthTemp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _user;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly ITokenService _tokenService;

    public AuthController(UserManager<AppUser> user, RoleManager<IdentityRole<Guid>> roleManager, ITokenService tokenService)
    {
        _user = user;
        _roleManager = roleManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName
        };

        var cretedResult = await _user.CreateAsync(user, request.Password);
        if (!cretedResult.Succeeded)
        {
            return BadRequest(cretedResult.Errors);
        }
        var roleName = string.IsNullOrWhiteSpace(request.Role) ? "User" : request.Role;
        if (!await _roleManager.RoleExistsAsync(roleName))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
        }
        await _user.AddToRoleAsync(user, roleName);
        return Ok(new { Message = $"User {user.Email} is register with role {roleName}" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _user.FindByEmailAsync(request.Email);
        if (user == null || !await _user.CheckPasswordAsync(user, request.Password))
        {
            return Unauthorized(new { Message = "Invalid login or password" });
        }
        var role = await _user.GetRolesAsync(user);
        var token = _tokenService.CreateToken(user, role.ToList());

        return Ok(new AuthResponse(token, user.Email!, user.FullName, role));
    }
}