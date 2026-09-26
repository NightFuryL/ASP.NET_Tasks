using Classwork_26_09_2026.DTOs;
using Classwork_26_09_2026.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;

namespace Classwork_26_09_2026.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    // Простое in-memory хранилище для заказов (на время работы приложения)
    private static readonly ConcurrentDictionary<Guid, CreateOrderDto> _orders = new();

    // 1. Создание заказа: принимаем DTO, генерируем Guid и возвращаем его
    [HttpPost]
    public IActionResult CreateOrder([FromBody] CreateOrderDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.ItemName))
        {
            throw new ArgumentException("Название товара не может быть пустым.");
        }

        if (dto.Quantity <= 0)
        {
            throw new ArgumentException("Количество должно быть больше нуля.");
        }

        var orderId = Guid.NewGuid();
        _orders[orderId] = dto;

        // Возвращаем 201 Created со ссылкой на GetOrderById и телом ответа
        return CreatedAtAction(nameof(GetOrderById), new { id = orderId }, new
        {
            Id = orderId,
            dto.ItemName,
            dto.Quantity,
            Status = "Created"
        });
    }

    // 2. Получение заказа по созданному Guid
    [HttpGet("{id:Guid}")]
    public IActionResult GetOrderById([FromRoute] Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("Id не может быть пустым.");
        }

        if (!_orders.TryGetValue(id, out var order))
        {
            throw new NotFoundException($"Заказ с ID {id} не найден.");
        }

        return Ok(new
        {
            OrderId = id,
            order.ItemName,
            order.Quantity,
            Status = "Complete"
        });
    }
}