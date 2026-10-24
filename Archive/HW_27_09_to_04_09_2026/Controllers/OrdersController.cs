using Microsoft.AspNetCore.Mvc;
using HW_27_09_to_04_09_2026.DTOs;
using HW_27_09_to_04_09_2026.Models;

namespace HW_27_09_to_04_09_2026.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private static readonly List<Order> _orders = new()
    {
        new Order { Id = 1, Number = "ORD_2026_001", Total = 1250.00m, CreatedAt = DateTime.UtcNow.AddDays(-5) },
        new Order { Id = 2, Number = "ORD_2026_002", Total = 450.50m, CreatedAt = DateTime.UtcNow.AddDays(-3) },
        new Order { Id = 3, Number = "ORD_2026_003", Total = 3800.75m, CreatedAt = DateTime.UtcNow.AddDays(-1) }
    };

    private static int _nextId = 4;
    private static readonly object _lock = new();

    // GET /api/orders
    [HttpGet]
    public ActionResult<IEnumerable<Order>> GetAll()
    {
        lock (_lock)
        {
            return Ok(_orders.ToList());
        }
    }

    // GET /api/orders/{id:int}
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        lock (_lock)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound("Order not found");
            }

            return Ok(order);
        }
    }

    // GET /api/orders/search?number=...&minTotal=...
    [HttpGet("search")]
    public IActionResult Search([FromQuery] string? number, [FromQuery] decimal? minTotal = null)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return BadRequest("Обов'язковий параметр 'number' відсутній або порожній.");
        }

        lock (_lock)
        {
            var query = _orders.Where(o => o.Number.Contains(number, StringComparison.OrdinalIgnoreCase));

            if (minTotal.HasValue)
            {
                query = query.Where(o => o.Total >= minTotal.Value);
            }

            return Ok(query.ToList());
        }
    }

    // POST /api/orders
    [HttpPost]
    public IActionResult Create([FromBody] OrderDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Number))
        {
            return BadRequest("Номер замовлення (Number) є обов'язковим.");
        }

        if (dto.Total <= 0)
        {
            return BadRequest("Сума замовлення (Total) повинна бути більшою за нуль.");
        }

        lock (_lock)
        {
            var newOrder = new Order
            {
                Id = _nextId++,
                Number = dto.Number.Trim(),
                Total = dto.Total,
                CreatedAt = DateTime.UtcNow
            };

            _orders.Add(newOrder);
            return CreatedAtAction(nameof(GetById), new { id = newOrder.Id }, newOrder);
        }
    }

    // PUT /api/orders/{id:int}
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] OrderDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Number) || dto.Total <= 0)
        {
            return BadRequest("Некоректні дані у вхідному DTO. Перевірте Number та Total (> 0).");
        }

        lock (_lock)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound("Order not found");
            }

            order.Number = dto.Number.Trim();
            order.Total = dto.Total;

            return Ok(order);
        }
    }

    // DELETE /api/orders/{id:int}
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        lock (_lock)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound("Order not found");
            }

            _orders.Remove(order);
            return NoContent();
        }
    }
}