using Classwork_27_09_2026.DTOs;
using Classwork_27_09_2026.Services;
using Classwork_27_09_2026.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Classwork_27_09_2026.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_productService.GetAll());
    }
    [HttpGet("{id:guid}")]
    public IActionResult GetById(Guid id)
    {
        return Ok(_productService.GetById(id));
    }
    [HttpPost("create")]
    public IActionResult Create([FromBody] CreateProductDto dto)
    {
        var newProduct = _productService.Create(dto);
        return CreatedAtAction(nameof(GetById), new { id = newProduct. Id }, newProduct);
    }
    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] UpdateProductDto dto)
    {
        _productService.Update(id, dto);
        return NoContent();
    }
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        _productService.Delete(id);
        return NoContent();
    }
}
