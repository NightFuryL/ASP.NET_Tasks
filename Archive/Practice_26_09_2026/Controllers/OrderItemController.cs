using Microsoft.AspNetCore.Mvc;
using Practice_26_09_2026.DTOs;
using Practice_26_09_2026.Models;
using System.Collections.Concurrent;

namespace Practice_26_09_2026.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderItemController : ControllerBase
{
    private static readonly ConcurrentDictionary<Guid, OrderItem> _orderItems = new();

    static OrderItemController()
    {
        var item1 = new OrderItem { Id = Guid.NewGuid(), ProductName = "Mechanical Keyboard", Price = 89.99m, Quantity = 2 };
        var item2 = new OrderItem { Id = Guid.NewGuid(), ProductName = "Wireless Mouse", Price = 49.50m, Quantity = 1 };
        var item3 = new OrderItem { Id = Guid.NewGuid(), ProductName = "Gaming Mousepad", Price = 19.99m, Quantity = 3 };

        _orderItems[item1.Id] = item1;
        _orderItems[item2.Id] = item2;
        _orderItems[item3.Id] = item3;
    }

    // 1. GET ALL
    [HttpGet]
    public ActionResult<IEnumerable<OrderItem>> GetAll()
    {
        return Ok(_orderItems.Values.ToList());
    }

    // 2. Read
    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        if (_orderItems.TryGetValue(id, out var item))
        {
            return Ok(item);
        }

        return NotFound(new { Message = $"OrderItem з ID {id} не знайдено." });
    }

    // 3. Create
    [HttpPost]
    public IActionResult Create([FromBody] CreateOrderItemDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.ProductName))
        {
            return BadRequest(new { Message = "Назва товару (ProductName) обов'язкова." });
        }

        if (dto.Price <= 0)
        {
            return BadRequest(new { Message = "Ціна (Price) повинна бути більшою за нуль." });
        }

        if (dto.Quantity <= 0)
        {
            return BadRequest(new { Message = "Кількість (Quantity) повинна бути більшою за нуль." });
        }

        var newItem = new OrderItem
        {
            Id = Guid.NewGuid(),
            ProductName = dto.ProductName.Trim(),
            Price = dto.Price,
            Quantity = dto.Quantity
        };

        _orderItems[newItem.Id] = newItem;

        return CreatedAtAction(nameof(GetById), new { id = newItem.Id }, newItem);
    }

    // 4. Update
    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] UpdateOrderItemDto dto)
    {
        if (!_orderItems.TryGetValue(id, out var item))
        {
            return NotFound(new { Message = $"OrderItem з ID {id} не знайдено." });
        }

        if (dto == null || string.IsNullOrWhiteSpace(dto.ProductName))
        {
            return BadRequest(new { Message = "Назва товару (ProductName) обов'язкова." });
        }

        if (dto.Price <= 0 || dto.Quantity <= 0)
        {
            return BadRequest(new { Message = "Ціна та кількість повинні бути більшими за нуль." });
        }

        item.ProductName = dto.ProductName.Trim();
        item.Price = dto.Price;
        item.Quantity = dto.Quantity;

        return Ok(item);
    }

    // 5. Delete
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        if (_orderItems.TryRemove(id, out _))
        {
            return NoContent();
        }

        return NotFound(new { Message = $"OrderItem з ID {id} не знайдено." });
    }
}
