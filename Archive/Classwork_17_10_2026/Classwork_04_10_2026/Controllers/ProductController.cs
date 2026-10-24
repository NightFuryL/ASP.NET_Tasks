using Classwork_04_10_2026.DTO;
using Classwork_04_10_2026.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Classwork_04_10_2026.Controllers;

[ApiController]
[Route("[controller]")]
[Produces("application/json")] // Controller returns JSON responses
public class ProductController : Controller
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    /// <summary>Create new product</summary>
    /// <param name="product">The product to create</param>
    /// <returns>The created product</returns>
    /// <response code="201">Returns the created product</response>
    /// <response code="400">Invalid product data</response>
    [HttpPost]
    [ProducesResponseType(typeof(CreateProductDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public IActionResult Create([FromBody] CreateProductDTO product)
    {
        var createdProduct = _productService.Create(product);
        return Ok(new { product = createdProduct });
    }
}
