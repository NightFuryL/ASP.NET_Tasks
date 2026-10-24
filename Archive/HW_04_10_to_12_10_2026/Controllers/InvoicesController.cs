using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using HW_04_10_to_12_10_2026.DTOs;
using HW_04_10_to_12_10_2026.Models;

namespace HW_04_10_to_12_10_2026.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoicesController : ControllerBase
{
    private readonly IMapper _mapper;

    private static readonly List<Invoice> _invoices = new()
    {
        new Invoice { Id = 1, Number = "INV_2026_001", ClientName = "ТОВ Агропром", Total = 15200.00m, CreatedAt = DateTime.UtcNow.AddDays(-10) },
        new Invoice { Id = 2, Number = "INV_2026_002", ClientName = "ФОП Іваненко", Total = 4350.50m, CreatedAt = DateTime.UtcNow.AddDays(-5) },
        new Invoice { Id = 3, Number = "INV_2026_003", ClientName = "ТОВ Схід-Захід", Total = 89000.00m, CreatedAt = DateTime.UtcNow.AddDays(-1) }
    };

    private static int _nextId = 4;
    private static readonly object _lock = new();

    public InvoicesController(IMapper mapper)
    {
        _mapper = mapper;
    }

    // GET /api/invoices
    [HttpGet]
    public ActionResult<IEnumerable<InvoiceReadDto>> GetAll()
    {
        lock (_lock)
        {
            var dtos = _mapper.Map<IEnumerable<InvoiceReadDto>>(_invoices);
            return Ok(dtos);
        }
    }

    // GET /api/invoices/{id:int}
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        lock (_lock)
        {
            var invoice = _invoices.FirstOrDefault(inv => inv.Id == id);
            if (invoice == null)
            {
                return NotFound(new { Message = $"Рахунок з ID {id} не знайдено." });
            }

            var dto = _mapper.Map<InvoiceReadDto>(invoice);
            return Ok(dto);
        }
    }

    // POST /api/invoices
    [HttpPost]
    public IActionResult Create([FromBody] InvoiceCreateDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.ClientName) || dto.Total <= 0)
        {
            return BadRequest(new { Message = "ClientName обов'язковий, а Total повинен бути більше нуля." });
        }

        lock (_lock)
        {
            var invoice = _mapper.Map<Invoice>(dto);
            invoice.Id = _nextId++;
            invoice.Number = $"INV-2026-{invoice.Id:D3}";
            invoice.CreatedAt = DateTime.UtcNow;

            _invoices.Add(invoice);

            var readDto = _mapper.Map<InvoiceReadDto>(invoice);

            return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, readDto);
        }
    }

    // PUT /api/invoices/{id:int}
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] InvoiceUpdateDto dto)
    {
        if (dto == null || dto.Total <= 0)
        {
            return BadRequest(new { Message = "Сума рахунку (Total) повинна бути більшою за нуль." });
        }

        lock (_lock)
        {
            var invoice = _invoices.FirstOrDefault(inv => inv.Id == id);
            if (invoice == null)
            {
                return NotFound(new { Message = $"Рахунок з ID {id} не знайдено." });
            }
            _mapper.Map(dto, invoice);

            var readDto = _mapper.Map<InvoiceReadDto>(invoice);
            return Ok(readDto);
        }
    }
}
