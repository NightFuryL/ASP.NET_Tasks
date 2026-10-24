using AutoMapper;
using Classwork_18_10_2026.Data;
using Classwork_18_10_2026.Data.Models;
using Classwork_18_10_2026.DTO;
using Classwork_18_10_2026.Services.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Classwork_18_10_2026.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UserController : Controller
{
    private readonly IFileService _fileService;
    private readonly IMapper _mapper;
    private readonly AppDbContext _context;
    private const string SubFolder = "avatars";
    public UserController(AppDbContext appDbContext, IFileService fileService, IMapper mapper)
    {
        _context = appDbContext;
        _fileService = fileService;
        _mapper = mapper;
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDTO dto)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            LastName = dto.LastName,
            Email = dto.Email,
            Role = dto.Role,
            Age = dto.Age,
            CreatedAt = DateTime.Now,
        };

        if (dto.Avatar != null && dto.Avatar.Length > 0)
        {
            var fileResult = await _fileService.SaveFileAsync(dto.Avatar, SubFolder);
            user.AvatarURL = fileResult.RelativePath;
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok();
    }
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    /*[HttpPost("avatar/{id:guid}")]
    [Consumes("application/json")]
    public async Task<IActionResult> UploadAvatar(Guid id, [FromBody] )*/
}
