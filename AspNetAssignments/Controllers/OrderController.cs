using Microsoft.AspNetCore.Mvc;
using AspNetAssignments.DTOs.Order;
using AspNetAssignments.Services.Abstract;

namespace AspNetAssignments.Controllers;

/// <summary>
/// Контролер для операцій із замовленнями клієнтів
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Отримує перелік усіх наявних замовлень
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<OrderDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }

    /// <summary>
    /// Отримує детальну інформацію про замовлення за його ідентифікатором
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound(new { message = $"Замовлення з ID {id} не знайдено." });
        }
        return Ok(order);
    }

    /// <summary>
    /// Отримує перелік усіх замовлень конкретного користувача
    /// </summary>
    [HttpGet("user/{userId:guid}")]
    [ProducesResponseType(typeof(List<OrderDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByUserId(Guid userId)
    {
        var orders = await _orderService.GetOrdersByUserIdAsync(userId);
        return Ok(orders);
    }

    /// <summary>
    /// Шукає замовлення за номером та мінімальною сумою
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<OrderDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Search([FromQuery] string? number, [FromQuery] decimal? minTotal = null)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return BadRequest(new { message = "Обов'язковий параметр 'number' відсутній або порожній." });
        }

        var results = await _orderService.SearchOrdersAsync(number, minTotal);
        return Ok(results);
    }

    /// <summary>
    /// Оформлює та зберігає нове замовлення
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(OrderDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateOrderDTO dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var createdOrder = await _orderService.CreateOrderAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id }, createdOrder);
    }

    /// <summary>
    /// Видаляє замовлення за його числовим ідентифікатором
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _orderService.DeleteOrderAsync(id);
        if (!result)
        {
            return NotFound(new { message = $"Замовлення з ID {id} не знайдено." });
        }

        return NoContent();
    }
}
