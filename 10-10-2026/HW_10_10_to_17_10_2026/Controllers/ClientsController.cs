using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HW_10_10_to_17_10_2026.Data;
using HW_10_10_to_17_10_2026.DTOs;
using HW_10_10_to_17_10_2026.Models;

namespace HW_10_10_to_17_10_2026.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ClientsController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClientReadDto>>> GetAll()
    {
        var clients = await _context.Clients.ToListAsync();
        return Ok(_mapper.Map<IEnumerable<ClientReadDto>>(clients));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var client = await _context.Clients.FindAsync(id);
        if (client == null)
        {
            return NotFound(new { Message = $"Клієнта з ID {id} не знайдено." });
        }

        return Ok(_mapper.Map<ClientReadDto>(client));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClientCreateDto dto)
    {
        if (dto == null ||
            string.IsNullOrWhiteSpace(dto.FullName) ||
            string.IsNullOrWhiteSpace(dto.Email))
        {
            return BadRequest(new { Message = "Поля FullName та Email є обов'язковими." });
        }

        var client = _mapper.Map<Client>(dto);
        client.RegistrationDate = DateTime.UtcNow;

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        var readDto = _mapper.Map<ClientReadDto>(client);
        return CreatedAtAction(nameof(GetById), new { id = client.Id }, readDto);
    }
}
