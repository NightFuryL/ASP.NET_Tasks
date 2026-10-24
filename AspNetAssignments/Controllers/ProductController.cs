using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using AspNetAssignments.DTOs.Pagination;
using AspNetAssignments.DTOs.Product;
using AspNetAssignments.Services.Abstract;

namespace AspNetAssignments.Controllers;

/// <summary>
/// Контролер для операцій з каталогом товарів (пошук, пагінація, фільтрація, CRUD)
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Отримує сторінку товарів із можливістю фільтрації та сортування
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PageResponse<ProductDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPagedProducts([FromQuery] ProductQueryParams queryParams)
    {
        var pageResponse = await _productService.GetPagedAsync(queryParams);
        return Ok(pageResponse);
    }

    /// <summary>
    /// Отримує список усіх наявних активних товарів
    /// </summary>
    [HttpGet("all")]
    [ProducesResponseType(typeof(List<ProductDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productService.GetAllProductsAsync();
        return Ok(products);
    }

    /// <summary>
    /// Отримує детальні дані товару за його ідентифікатором
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var product = await _productService.GetProductByIdAsync(id);
        if (product == null)
        {
            return NotFound(new { message = $"Товар з ID {id} не знайдено." });
        }
        return Ok(product);
    }

    /// <summary>
    /// Створює новий товар у системі (валідація здійснюється автоматично через ValidationFilterAttribute)
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct(
        [FromBody] CreateProductDTO product,
        [FromServices] IValidator<CreateProductDTO>? validator = null)
    {
        // Ручна перевірка моделі закоментована, оскільки діє глобальний ValidationFilterAttribute:
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}
        //var validationResult = await validator.ValidateAsync(product);
        //if (!validationResult.IsValid)
        //{
        //    return BadRequest(validationResult.ToDictionary());
        //}

        var createdProduct = await _productService.CreateProductAsync(product);
        return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, createdProduct);
    }

    /// <summary>
    /// Оновлює інформацію про товар за ідентифікатором
    /// </summary>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ProductDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProduct(
        Guid id,
        [FromBody] UpdateProductDTO productDto,
        [FromServices] IValidator<UpdateProductDTO>? validator = null)
    {
        var updated = await _productService.UpdateProductAsync(id, productDto);
        if (updated == null)
        {
            return NotFound(new { message = $"Товар з ID {id} не знайдено для оновлення." });
        }

        return Ok(updated);
    }

    /// <summary>
    /// Виконує м'яке видалення товару з каталогу
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var result = await _productService.DeleteAsync(id);
        if (!result)
        {
            return NotFound(new { message = $"Товар з ID {id} не знайдено." });
        }

        return NoContent();
    }
}
