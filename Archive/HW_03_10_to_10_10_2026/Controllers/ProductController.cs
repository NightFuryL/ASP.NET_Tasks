using Microsoft.AspNetCore.Mvc;
using HW_03_10_to_10_10_2026.DTOs;
using HW_03_10_to_10_10_2026.Models;

namespace HW_03_10_to_10_10_2026.Controllers;

/// <summary>
/// Контролер для роботи з товарами магазину
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductController : ControllerBase
{
    private static readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Смартфон Google Pixel 8", Price = 28999.00m },
        new Product { Id = 2, Name = "Бездротові навушники Sony", Price = 9499.00m },
        new Product { Id = 3, Name = "Планшет Apple iPad Air", Price = 29999.00m }
    };

    private static int _nextId = 4;
    private static readonly object _lock = new();

    /// <summary>Отримати перелік усіх товарів</summary>
    /// <returns>Список доступних товарів</returns>
    /// <response code="200">Повертає список усіх товарів</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        lock (_lock)
        {
            return Ok(_products.ToList());
        }
    }

    /// <summary>Отримати товар за ідентифікатором</summary>
    /// <param name="id">Ідентифікатор товару</param>
    /// <returns>Знайдений товар</returns>
    /// <response code="200">Товар успішно знайдено</response>
    /// <response code="404">Товар не знайдено</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(int id)
    {
        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(new { Message = $"Товар з ID {id} не знайдено." });
            }

            return Ok(product);
        }
    }

    /// <summary>Створити новий товар</summary>
    /// <param name="dto">Дані створюваного товару</param>
    /// <returns>Створений об'єкт товару</returns>
    /// <response code="201">Товар успішно створено</response>
    /// <response code="400">Некоректні дані товару</response>
    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CreateProductDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || dto.Price <= 0)
        {
            return BadRequest(new { Message = "Поле Name є обов'язковим, а Price має бути більше нуля." });
        }

        lock (_lock)
        {
            var newProduct = new Product
            {
                Id = _nextId++,
                Name = dto.Name.Trim(),
                Price = dto.Price
            };

            _products.Add(newProduct);

            return CreatedAtAction(nameof(GetById), new { id = newProduct.Id }, newProduct);
        }
    }

    /// <summary>Оновити існуючий товар</summary>
    /// <param name="id">Ідентифікатор товару</param>
    /// <param name="dto">Оновлені дані</param>
    /// <returns>Оновлений товар</returns>
    /// <response code="200">Товар успішно оновлено</response>
    /// <response code="400">Некоректні вхідні дані</response>
    /// <response code="404">Товар не знайдено</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(int id, [FromBody] CreateProductDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || dto.Price <= 0)
        {
            return BadRequest(new { Message = "Некоректні дані товару." });
        }

        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(new { Message = $"Товар з ID {id} не знайдено." });
            }

            product.Name = dto.Name.Trim();
            product.Price = dto.Price;

            return Ok(product);
        }
    }

    /// <summary>Видалити товар за його ідентифікатором</summary>
    /// <param name="id">Ідентифікатор товару</param>
    /// <response code="204">Товар успішно видалено</response>
    /// <response code="404">Товар не знайдено</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound(new { Message = $"Товар з ID {id} не знайдено." });
            }

            _products.Remove(product);
            return NoContent();
        }
    }
}
