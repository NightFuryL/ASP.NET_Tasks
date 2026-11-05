using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using AspNetAssignments.DTOs.Invoice;
using AspNetAssignments.Models.Invoice;

namespace AspNetAssignments.Controllers;

/// <summary>
/// Контролер для операцій із фінансовими рахунками та інвойсами
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
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

    /// <summary>
    /// Отримує перелік усіх виставлених рахунків
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<InvoiceReadDto>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<InvoiceReadDto>> GetAll()
    {
        lock (_lock)
        {
            var dtos = _mapper.Map<IEnumerable<InvoiceReadDto>>(_invoices);
            return Ok(dtos);
        }
    }

    /// <summary>
    /// Знаходить рахунок за його ідентифікатором
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(InvoiceReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        lock (_lock)
        {
            var invoice = _invoices.FirstOrDefault(inv => inv.Id == id);
            if (invoice == null)
            {
                return NotFound(new { message = $"Рахунок з ID {id} не знайдено." });
            }

            var dto = _mapper.Map<InvoiceReadDto>(invoice);
            return Ok(dto);
        }
    }

    /// <summary>
    /// Створює новий рахунок для клієнта
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(InvoiceReadDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] InvoiceCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
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

    /// <summary>
    /// Оновлює інформацію про наявний рахунок
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(InvoiceReadDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Update(int id, [FromBody] InvoiceUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        lock (_lock)
        {
            var invoice = _invoices.FirstOrDefault(inv => inv.Id == id);
            if (invoice == null)
            {
                return NotFound(new { message = $"Рахунок з ID {id} не знайдено." });
            }

            _mapper.Map(dto, invoice);

            var readDto = _mapper.Map<InvoiceReadDto>(invoice);
            return Ok(readDto);
        }
    }
}
