using Microsoft.AspNetCore.Mvc;
using Practice_03_10_2026.DTOs;
using Practice_03_10_2026.Models;

namespace Practice_03_10_2026.Controllers;

/// <summary>
/// Управління позиціями товарів у замовленнях
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrderItemController : ControllerBase
{
    private static readonly List<OrderItem> _items = new()
    {
        new OrderItem { Id = 1, ProductId = 1, ProductName = "Ноутбук ASUS ZenBook", UnitPrice = 38999.00m, Quantity = 1 },
        new OrderItem { Id = 2, ProductId = 2, ProductName = "Миша бездротова Logitech", UnitPrice = 1299.00m, Quantity = 2 }
    };

    private static int _nextId = 3;
    private static readonly object _lock = new();

    /// <summary>Отримати перелік усіх позицій замовлень</summary>
    /// <returns>Список позицій</returns>
    /// <response code="200">Успішне повернення списку</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderItem>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<OrderItem>> GetAll()
    {
        lock (_lock)
        {
            return Ok(_items.ToList());
        }
    }

    /// <summary>Отримати позицію замовлення за ID</summary>
    /// <param name="id">Ідентифікатор позиції</param>
    /// <returns>Дані позиції</returns>
    /// <response code="200">Позицію знайдено</response>
    /// <response code="404">Позицію не знайдено</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(OrderItem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        lock (_lock)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound(new { Message = $"Позицію з ID {id} не знайдено." });
            }
            return Ok(item);
        }
    }

    /// <summary>Створити нову позицію замовлення</summary>
    /// <param name="dto">Дані нової позиції</param>
    /// <returns>Створена позиція</returns>
    /// <response code="201">Позиція успішно додана</response>
    /// <response code="400">Некоректні дані</response>
    [HttpPost]
    [ProducesResponseType(typeof(OrderItem), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CreateOrderItemDto dto)
    {
        if (dto == null || dto.ProductId <= 0 || dto.Quantity <= 0)
        {
            return BadRequest(new { Message = "ProductId та Quantity мають бути більшими за нуль." });
        }

        lock (_lock)
        {
            var newItem = new OrderItem
            {
                Id = _nextId++,
                ProductId = dto.ProductId,
                ProductName = $"Товар #{dto.ProductId}",
                UnitPrice = 1000m,
                Quantity = dto.Quantity
            };

            _items.Add(newItem);

            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
        }
    }

    /// <summary>Видалити позицію замовлення</summary>
    /// <param name="id">Ідентифікатор позиції</param>
    /// <response code="204">Позицію успішно видалено</response>
    /// <response code="404">Позицію не знайдено</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        lock (_lock)
        {
            var item = _items.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                return NotFound(new { Message = $"Позицію з ID {id} не знайдено." });
            }

            _items.Remove(item);
            return NoContent();
        }
    }
}
