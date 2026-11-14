using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AspNetAssignments.AuthTemp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : Controller
{
    [HttpGet("me")]
    public IActionResult GetProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var email = User.FindFirstValue(ClaimTypes.Email);
        var fullName = User.FindFirstValue("FullName");
        var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();
        if (userId == null || email == null || fullName == null)
        {
            return Unauthorized();
        }
        return Ok(new
        {
            Id = userId,
            Email = email,
            FullName = fullName,
            Roles = roles
        });
    }
    [HttpGet("admin-dashboard")]
    [Authorize(Roles = "admin")]
    public IActionResult GetAdminStates()
    {
        return Ok(new { SecretData = "Secret information for admin users only." });
    }
}