using Microsoft.AspNetCore.Mvc;
using AspNetAssignments.DTOs.Order;
using AspNetAssignments.Services.Abstract;

namespace AspNetAssignments.Controllers;

/// <summary>
/// Контролер для операцій із окремими позиціями замовлень
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrderItemController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderItemController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Отримує перелік усіх окремих позицій замовлень
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<OrderItemDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var items = await _orderService.GetAllItemsAsync();
        return Ok(items);
    }

    /// <summary>
    /// Знаходить позицію замовлення за її числовим ідентифікатором
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(OrderItemDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await _orderService.GetItemByIdAsync(id);
        if (item == null)
        {
            return NotFound(new { message = $"Позицію з ID {id} не знайдено." });
        }
        return Ok(item);
    }

    /// <summary>
    /// Створює нову позицію для товару в замовленні
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(OrderItemDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOrderItemDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdItem = await _orderService.CreateItemAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdItem.Id }, createdItem);
    }

    /// <summary>
    /// Оновлює дані наявної позиції замовлення
    /// </summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(OrderItemDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateOrderItemDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedItem = await _orderService.UpdateItemAsync(id, dto);
        if (updatedItem == null)
        {
            return NotFound(new { message = $"Позицію з ID {id} не знайдено для оновлення." });
        }

        return Ok(updatedItem);
    }

    /// <summary>
    /// Видаляє позицію замовлення за її ідентифікатором
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _orderService.DeleteItemAsync(id);
        if (!result)
        {
            return NotFound(new { message = $"Позицію з ID {id} не знайдено." });
        }

        return NoContent();
    }
}
