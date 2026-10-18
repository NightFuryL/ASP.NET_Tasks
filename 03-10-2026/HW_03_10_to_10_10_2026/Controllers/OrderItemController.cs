using Microsoft.AspNetCore.Mvc;
using HW_03_10_to_10_10_2026.DTOs;
using HW_03_10_to_10_10_2026.Models;

namespace HW_03_10_to_10_10_2026.Controllers;

/// <summary>
/// Контролер позицій у замовленнях
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrderItemController : ControllerBase
{
    private static readonly List<OrderItem> _items = new()
    {
        new OrderItem { Id = 1, ProductId = 1, ProductName = "Смартфон Google Pixel 8", UnitPrice = 28999.00m, Quantity = 1 },
        new OrderItem { Id = 2, ProductId = 2, ProductName = "Бездротові навушники Sony", UnitPrice = 9499.00m, Quantity = 1 }
    };

    private static int _nextId = 3;
    private static readonly object _lock = new();

    /// <summary>Отримати перелік усіх елементів замовлень</summary>
    /// <returns>Список елементів замовлень</returns>
    /// <response code="200">Успішне отримання списку</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OrderItem>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<OrderItem>> GetAll()
    {
        lock (_lock)
        {
            return Ok(_items.ToList());
        }
    }

    /// <summary>Отримати елемент замовлення за ID</summary>
    /// <param name="id">Ідентифікатор елемента</param>
    /// <returns>Дані елемента замовлення</returns>
    /// <response code="200">Елемент успішно знайдено</response>
    /// <response code="404">Елемент не знайдено</response>
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

    /// <summary>Створити новий елемент замовлення</summary>
    /// <param name="dto">Дані нового елемента</param>
    /// <returns>Створений елемент замовлення</returns>
    /// <response code="201">Елемент успішно створено</response>
    /// <response code="400">Некоректні параметри</response>
    [HttpPost]
    [ProducesResponseType(typeof(OrderItem), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CreateOrderItemDto dto)
    {
        if (dto == null || dto.ProductId <= 0 || dto.Quantity <= 0)
        {
            return BadRequest(new { Message = "ProductId та Quantity мають бути більшими за 0." });
        }

        lock (_lock)
        {
            var newItem = new OrderItem
            {
                Id = _nextId++,
                ProductId = dto.ProductId,
                ProductName = $"Товар #{dto.ProductId}",
                UnitPrice = 1000.00m,
                Quantity = dto.Quantity
            };

            _items.Add(newItem);

            return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
        }
    }

    /// <summary>Видалити елемент замовлення</summary>
    /// <param name="id">Ідентифікатор для видалення</param>
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
