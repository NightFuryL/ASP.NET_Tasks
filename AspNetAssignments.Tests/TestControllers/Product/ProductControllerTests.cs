using AspNetAssignments.Controllers;
using AspNetAssignments.DTOs.Pagination;
using AspNetAssignments.DTOs.Product;
using AspNetAssignments.Services.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AspNetAssignments.Tests.TestControllers.Product;

public class ProductControllerTests
{
    private readonly ProductController _productController;
    private readonly Mock<IProductService> _productServiceMock;

    public ProductControllerTests()
    {
        _productServiceMock = new Mock<IProductService>();
        _productController = new ProductController(_productServiceMock.Object);
    }

    #region GetPagedProducts

    [Fact]
    public async Task GetPagedProducts_ReturnsOkObjectResult_WithPageResponse()
    {
        // Arrange
        var queryParams = new ProductQueryParams
        {
            PageNumber = 1,
            PageSize = 10
        };

        var items = new List<ProductDTO>
        {
            new ProductDTO { Id = Guid.NewGuid(), Name = "Ноутбук", Price = 25000, Quantity = 5 },
            new ProductDTO { Id = Guid.NewGuid(), Name = "Мишка", Price = 800, Quantity = 20 }
        };

        var pagedResponse = new PageResponse<ProductDTO>(items, totalCount: 2, pageNumber: 1, pageSize: 10);

        _productServiceMock
            .Setup(service => service.GetPagedAsync(queryParams))
            .ReturnsAsync(pagedResponse);

        // Act
        var result = await _productController.GetPagedProducts(queryParams);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        var returnedPage = Assert.IsType<PageResponse<ProductDTO>>(okResult.Value);
        Assert.Equal(2, returnedPage.Items.Count);
    }

    #endregion

    #region GetAllProducts

    [Fact]
    public async Task GetAllProducts_WhenProductsExist_ReturnsOkObjectResult()
    {
        // Arrange
        var products = new List<ProductDTO>
        {
            new ProductDTO { Id = Guid.NewGuid(), Name = "Клавіатура", Price = 1500, Quantity = 12 },
            new ProductDTO { Id = Guid.NewGuid(), Name = "Монітор", Price = 9000, Quantity = 4 }
        };

        _productServiceMock
            .Setup(service => service.GetAllProductsAsync())
            .ReturnsAsync(products);

        // Act
        var result = await _productController.GetAllProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        var returnedProducts = Assert.IsAssignableFrom<IEnumerable<ProductDTO>>(okResult.Value);
        Assert.Equal(2, returnedProducts.Count());
    }

    #endregion

    #region GetProductById

    [Fact]
    public async Task GetProductById_WhenProductExists_ReturnsOkObjectResult()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var product = new ProductDTO
        {
            Id = productId,
            Name = "Навушники",
            Price = 3200,
            Quantity = 8
        };

        _productServiceMock
            .Setup(service => service.GetProductByIdAsync(productId))
            .ReturnsAsync(product);

        // Act
        var result = await _productController.GetProductById(productId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        var returnedProduct = Assert.IsType<ProductDTO>(okResult.Value);
        Assert.Equal(productId, returnedProduct.Id);
    }

    [Fact]
    public async Task GetProductById_WhenProductDoesNotExist_ReturnsNotFoundObjectResult()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _productServiceMock
            .Setup(service => service.GetProductByIdAsync(productId))
            .ReturnsAsync((ProductDTO?)null);

        // Act
        var result = await _productController.GetProductById(productId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    #endregion

    #region CreateProduct

    [Fact]
    public async Task CreateProduct_WhenCalled_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var createDto = new CreateProductDTO
        {
            Name = "Смартфон",
            Price = 18500,
            Description = "Нова модель",
            Quantity = 15
        };

        var createdProduct = new ProductDTO
        {
            Id = Guid.NewGuid(),
            Name = createDto.Name,
            Price = createDto.Price,
            Description = createDto.Description,
            Quantity = createDto.Quantity
        };

        _productServiceMock
            .Setup(service => service.CreateProductAsync(createDto))
            .ReturnsAsync(createdProduct);

        // Act
        var result = await _productController.CreateProduct(createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(201, createdResult.StatusCode);
        Assert.Equal(nameof(ProductController.GetProductById), createdResult.ActionName);
        var returnedProduct = Assert.IsType<ProductDTO>(createdResult.Value);
        Assert.Equal(createdProduct.Id, returnedProduct.Id);
    }

    #endregion

    #region UploadImages

    [Fact]
    public async Task UploadImages_WhenFilesAreValidAndProductExists_ReturnsOkObjectResult()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var fileMock = new Mock<IFormFile>();
        var files = new List<IFormFile> { fileMock.Object };

        var updatedProduct = new ProductDTO
        {
            Id = productId,
            Name = "Планшет",
            Images = new List<string> { "/images/products/photo1.jpg" }
        };

        _productServiceMock
            .Setup(service => service.AddImagesAsync(productId, files))
            .ReturnsAsync(updatedProduct);

        // Act
        var result = await _productController.UploadImages(productId, files);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        var returnedProduct = Assert.IsType<ProductDTO>(okResult.Value);
        Assert.Single(returnedProduct.Images);
    }

    [Fact]
    public async Task UploadImages_WhenFilesListIsEmpty_ReturnsBadRequestObjectResult()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var emptyFiles = new List<IFormFile>();

        // Act
        var result = await _productController.UploadImages(productId, emptyFiles);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
        _productServiceMock.Verify(service => service.AddImagesAsync(It.IsAny<Guid>(), It.IsAny<List<IFormFile>>()), Times.Never);
    }

    [Fact]
    public async Task UploadImages_WhenProductDoesNotExist_ReturnsNotFoundObjectResult()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var fileMock = new Mock<IFormFile>();
        var files = new List<IFormFile> { fileMock.Object };

        _productServiceMock
            .Setup(service => service.AddImagesAsync(productId, files))
            .ReturnsAsync((ProductDTO?)null);

        // Act
        var result = await _productController.UploadImages(productId, files);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    #endregion

    #region UpdateProduct

    [Fact]
    public async Task UpdateProduct_WhenProductExists_ReturnsOkObjectResult()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var updateDto = new UpdateProductDTO
        {
            Name = "Оновлений ноутбук",
            Price = 27000,
            Description = "Оновлений опис",
            Quantity = 7
        };

        var updatedProduct = new ProductDTO
        {
            Id = productId,
            Name = updateDto.Name,
            Price = updateDto.Price,
            Description = updateDto.Description,
            Quantity = updateDto.Quantity
        };

        _productServiceMock
            .Setup(service => service.UpdateProductAsync(productId, updateDto))
            .ReturnsAsync(updatedProduct);

        // Act
        var result = await _productController.UpdateProduct(productId, updateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        var returnedProduct = Assert.IsType<ProductDTO>(okResult.Value);
        Assert.Equal(updateDto.Name, returnedProduct.Name);
    }

    [Fact]
    public async Task UpdateProduct_WhenProductDoesNotExist_ReturnsNotFoundObjectResult()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var updateDto = new UpdateProductDTO
        {
            Name = "Неіснуючий товар",
            Price = 100,
            Quantity = 1
        };

        _productServiceMock
            .Setup(service => service.UpdateProductAsync(productId, updateDto))
            .ReturnsAsync((ProductDTO?)null);

        // Act
        var result = await _productController.UpdateProduct(productId, updateDto);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    #endregion

    #region DeleteProduct

    [Fact]
    public async Task DeleteProduct_WhenProductExists_ReturnsNoContentResult()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _productServiceMock
            .Setup(service => service.DeleteAsync(productId))
            .ReturnsAsync(true);

        // Act
        var result = await _productController.DeleteProduct(productId);

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, noContentResult.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_WhenProductDoesNotExist_ReturnsNotFoundObjectResult()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _productServiceMock
            .Setup(service => service.DeleteAsync(productId))
            .ReturnsAsync(false);

        // Act
        var result = await _productController.DeleteProduct(productId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    #endregion

    #region GetClientInfo

    [Fact]
    public void GetClientInfo_WhenClientAppIdExistsInHttpContext_ReturnsOkObjectResult()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        httpContext.Items["ClientAppId"] = "test-client-id";

        _productController.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        // Act
        var result = _productController.GetClientInfo();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    #endregion
}