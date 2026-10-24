using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Classwork_20_09_2026.Models;
using Classwork_20_09_2026.Services;

namespace Classwork_20_09_2026.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : Controller
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService)
    {
        _productService = productService;
    }
    [HttpGet]
    public ActionResult<List<Product>> GetAllProducts()
    {
        return Ok(_productService.GetAllProducts()); //status code 200, Message: all products 
    }
    [HttpGet("{id:guid}")]
    public ActionResult<Product> GetProductById(Guid id)
    {
        var product = _productService.GetProductById(id);
        if (product == null)
        {
            return NotFound(new { Message = $"Product {id} not found" }); //status code 404, Message: Product not found
        }
        return Ok(product); //status code 200, Message: Product found
    }
    [HttpGet("Search")]
    public IActionResult SearchProduct([FromQuery] string name)
    {
        var result = _productService.GetAllProducts()
            .Where(propa => propa.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();
        return Ok(result);
    }
}
