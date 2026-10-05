using Microsoft.AspNetCore.Mvc;
using HW_26_09_to_03_09_2026.DTOs;
using HW_26_09_to_03_09_2026.Models;

namespace HW_26_09_to_03_09_2026.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private static readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop Dell XPS 15", Price = 1899.99m },
        new Product { Id = 2, Name = "Wireless Mouse Logitech", Price = 49.99m },
        new Product { Id = 3, Name = "Mechanical Keyboard Keychron", Price = 99.50m }
    };

    private static int _nextId = 4;
    private static readonly object _lock = new();

    // GET: /api/products
    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll()
    {
        lock (_lock)
        {
            return Ok(_products.ToList());
        }
    }

    // GET: /api/products/{id:int}
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound("Product not found");
            }
            return Ok(product);
        }
    }

    // GET: /api/products/search?name=...
    [HttpGet("search")]
    public IActionResult Search([FromQuery] string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest("Обов'язковий параметр рядка запиту 'name' відсутній або порожній.");
        }

        lock (_lock)
        {
            var results = _products
                .Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(results);
        }
    }

    // POST: /api/products
    [HttpPost]
    public IActionResult Create([FromBody] CreateProductDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Поле Name є обов'язковим.");
        }

        if (dto.Price < 0)
        {
            return BadRequest("Ціна не може бути від'ємною.");
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

    // PUT: /api/products/{id:int}
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] CreateProductDto dto)
    {
        if (dto == null || string.IsNullOrWhiteSpace(dto.Name) || dto.Price < 0)
        {
            return BadRequest("Некоректні вхідні дані для оновлення продукту.");
        }

        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            product.Name = dto.Name.Trim();
            product.Price = dto.Price;

            return Ok(product);
        }
    }

    // DELETE: /api/products/{id:int}
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        lock (_lock)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            _products.Remove(product);
            return NoContent();
        }
    }
}
