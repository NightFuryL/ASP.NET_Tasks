using Microsoft.AspNetCore.Mvc;
using HW_03_10_to_10_10_2026.DTOs;
using HW_03_10_to_10_10_2026.Models;

namespace HW_03_10_to_10_10_2026.Controllers;

/// <summary>
/// Контролер управління замовленнями
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrderController : ControllerBase
{
    private static readonly List<Order> _orders = new()
    {
        new Order
        {
            Id = 1,
            OrderNumber = "ORD-2026-888",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            Items = new List<OrderItem>
            {
                new OrderItem { Id = 1, ProductId = 1, ProductName = "Смартфон Google Pixel 8", UnitPrice = 28999.00m, Quantity = 1 }
            }
        }
    };

    private static int _nextId = 2;
    private static readonly object _lock = new();

    /// <summary>Отримати перелік усіх замовлень</summary>
    /// <returns>Список оформлених замовлень</returns>
    /// <response code="200">Повертає список замовлень</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Order>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Order>> GetAll()
    {
        lock (_lock)
        {
            return Ok(_orders.ToList());
        }
    }

    /// <summary>Отримати детальну інформацію про замовлення за ID</summary>
    /// <param name="id">Унікальний ID замовлення</param>
    /// <returns>Дані замовлення</returns>
    /// <response code="200">Замовлення знайдено</response>
    /// <response code="404">Замовлення не знайдено</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        lock (_lock)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound(new { Message = $"Замовлення з ID {id} не знайдено." });
            }

            return Ok(order);
        }
    }

    /// <summary>Створити нове замовлення</summary>
    /// <param name="dto">Параметри створення замовлення</param>
    /// <returns>Створене замовлення</returns>
    /// <response code="201">Замовлення успішно створено</response>
    /// <response code="400">Некоректні вхідні дані</response>
    [HttpPost]
    [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CreateOrderDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.OrderNumber))
        {
            return BadRequest(new { Message = "Номер замовлення (OrderNumber) є обов'язковим." });
        }

        lock (_lock)
        {
            int itemIdCounter = 100;
            var newOrder = new Order
            {
                Id = _nextId++,
                OrderNumber = dto.OrderNumber.Trim(),
                CreatedAt = DateTime.UtcNow,
                Items = dto.Items.Select(i => new OrderItem
                {
                    Id = itemIdCounter++,
                    ProductId = i.ProductId,
                    ProductName = $"Товар #{i.ProductId}",
                    UnitPrice = 750.00m,
                    Quantity = i.Quantity
                }).ToList()
            };

            _orders.Add(newOrder);

            return CreatedAtAction(nameof(GetById), new { id = newOrder.Id }, newOrder);
        }
    }

    /// <summary>Видалити замовлення за ID</summary>
    /// <param name="id">Ідентифікатор замовлення</param>
    /// <response code="204">Замовлення видалено</response>
    /// <response code="404">Замовлення не знайдено</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        lock (_lock)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order == null)
            {
                return NotFound(new { Message = $"Замовлення з ID {id} не знайдено." });
            }

            _orders.Remove(order);
            return NoContent();
        }
    }
}
