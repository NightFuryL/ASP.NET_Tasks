using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Practice_04_10_2026.DTOs;
using Practice_04_10_2026.Models;

namespace Practice_04_10_2026.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IMapper _mapper;

    private static readonly List<Client> _clients = new()
    {
        new Client { Id = 1, FullName = "Олександр Коваленко", Email = "oleksandr@example.com", Phone = "+380501112233", RegistrationDate = DateTime.UtcNow.AddMonths(-6) },
        new Client { Id = 2, FullName = "Марія Шевченко", Email = "maria@example.com", Phone = "+380672223344", RegistrationDate = DateTime.UtcNow.AddMonths(-3) },
        new Client { Id = 3, FullName = "Дмитро Бондаренко", Email = "dmytro@example.com", Phone = "+380933334455", RegistrationDate = DateTime.UtcNow.AddDays(-10) }
    };

    private static int _nextId = 4;
    private static readonly object _lock = new();

    public ClientsController(IMapper mapper)
    {
        _mapper = mapper;
    }

    //повертає список клієнтів
    [HttpGet]
    public ActionResult<IEnumerable<ClientReadDto>> GetAll()
    {
        lock (_lock)
        {
            var dtos = _mapper.Map<IEnumerable<ClientReadDto>>(_clients);
            return Ok(dtos);
        }
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        lock (_lock)
        {
            var client = _clients.FirstOrDefault(c => c.Id == id);
            if (client == null)
            {
                return NotFound(new { Message = $"Клієнта з ID {id} не знайдено." });
            }

            var dto = _mapper.Map<ClientReadDto>(client);
            return Ok(dto);
        }
    }
    [HttpPost]
    public IActionResult Create([FromBody] ClientCreateDto dto)
    {
        if (dto == null ||
            string.IsNullOrWhiteSpace(dto.FullName) ||
            string.IsNullOrWhiteSpace(dto.Email))
        {
            return BadRequest(new { Message = "Поля FullName та Email є обов'язковими." });
        }

        lock (_lock)
        {
            var client = _mapper.Map<Client>(dto);
            client.Id = _nextId++;
            client.RegistrationDate = DateTime.UtcNow;

            _clients.Add(client);

            var readDto = _mapper.Map<ClientReadDto>(client);

            return CreatedAtAction(nameof(GetById), new { id = client.Id }, readDto);
        }
    }
}
