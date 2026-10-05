using Microsoft.AspNetCore.Mvc;
using Practice_03_10_2026.DTOs;
using Practice_03_10_2026.Models;

namespace Practice_03_10_2026.Controllers;

/// <summary>
/// Управління каталогом товарів
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductController : ControllerBase
{
    private static readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Ноутбук ASUS ZenBook", Price = 38999.00m },
        new Product { Id = 2, Name = "Миша бездротова Logitech", Price = 1299.00m },
        new Product { Id = 3, Name = "Монітор Dell UltraSharp 27", Price = 15499.00m }
    };

    private static int _nextId = 4;
    private static readonly object _lock = new();

    /// <summary>Отримати перелік усіх товарів</summary>
    /// <returns>Список доступних товарів</returns>
    /// <response code="200">Успішне повернення списку товарів</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        lock (_lock)
        {
            return Ok(_products.ToList());
        }
    }

    /// <summary>Отримати інформацію про товар за його унікальним ідентифікатором</summary>
    /// <param name="id">Унікальний ID товару</param>
    /// <returns>Дані товару</returns>
    /// <response code="200">Товар успішно знайдено</response>
    /// <response code="404">Товар з таким ID не знайдено</response>
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

    /// <summary>Створити новий товар у каталозі</summary>
    /// <param name="dto">Дані нового товару</param>
    /// <returns>Створений товар із присвоєним ідентифікатором</returns>
    /// <response code="201">Товар успішно створено</response>
    /// <response code="400">Передано некоректні дані товару</response>
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
    /// <param name="dto">Оновлені дані товару</param>
    /// <returns>Оновлений товар</returns>
    /// <response code="200">Товар успішно оновлено</response>
    /// <response code="400">Некоректні дані</response>
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

    /// <summary>Видалити товар з каталогу</summary>
    /// <param name="id">Ідентифікатор товару для видалення</param>
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
