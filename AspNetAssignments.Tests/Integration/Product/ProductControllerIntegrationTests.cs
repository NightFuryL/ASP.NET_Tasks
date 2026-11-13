using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using AspNetAssignments.DTOs.Pagination;
using AspNetAssignments.DTOs.Product;
using Microsoft.AspNetCore.Http;
using Moq;

namespace AspNetAssignments.Tests.Integration.Product;

public class ProductControllerIntegrationTests : IClassFixture<ProductWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly ProductWebApplicationFactory _factory;

    public ProductControllerIntegrationTests(ProductWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
        _factory.ProductService.Reset();
    }

    #region GET api/Product (GetPagedProducts)

    [Fact]
    public async Task GetPagedProducts_Returns200OK_WithPageResponse()
    {
        // Arrange
        var items = new List<ProductDTO>
        {
            new ProductDTO { Id = Guid.NewGuid(), Name = "Ноутбук", Price = 25000, Quantity = 5 },
            new ProductDTO { Id = Guid.NewGuid(), Name = "Мишка", Price = 800, Quantity = 20 }
        };

        var pagedResponse = new PageResponse<ProductDTO>(items, totalCount: 2, pageNumber: 1, pageSize: 10);

        _factory.ProductService
            .Setup(service => service.GetPagedAsync(It.IsAny<ProductQueryParams>()))
            .ReturnsAsync(pagedResponse);

        // Act
        var response = await _client.GetAsync("api/Product?PageNumber=1&PageSize=10");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultPage = await response.Content.ReadFromJsonAsync<PageResponse<ProductDTO>>();
        Assert.NotNull(resultPage);
        Assert.Equal(2, resultPage.Items.Count);
        _factory.ProductService.Verify(service => service.GetPagedAsync(It.IsAny<ProductQueryParams>()), Times.Once());
    }

    #endregion

    #region GET api/Product/all (GetAllProducts)

    [Fact]
    public async Task GetAllProducts_WhenProductsExist_Returns200OK_WithProductsList()
    {
        // Arrange
        var expectedProducts = new List<ProductDTO>
        {
            new ProductDTO { Id = Guid.NewGuid(), Name = "Клавіатура", Price = 1500, Quantity = 12 },
            new ProductDTO { Id = Guid.NewGuid(), Name = "Монітор", Price = 9000, Quantity = 4 }
        };

        _factory.ProductService
            .Setup(service => service.GetAllProductsAsync())
            .ReturnsAsync(expectedProducts);

        // Act
        var response = await _client.GetAsync("api/Product/all");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultProducts = await response.Content.ReadFromJsonAsync<List<ProductDTO>>();
        Assert.NotNull(resultProducts);
        Assert.Equal(2, resultProducts.Count);
        _factory.ProductService.Verify(service => service.GetAllProductsAsync(), Times.Once());
    }

    #endregion

    #region GET api/Product/{id} (GetProductById)

    [Fact]
    public async Task GetProductById_WhenProductExists_Returns200OK_WithProductDto()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var expectedProduct = new ProductDTO
        {
            Id = productId,
            Name = "Навушники",
            Price = 3200,
            Quantity = 8
        };

        _factory.ProductService
            .Setup(service => service.GetProductByIdAsync(productId))
            .ReturnsAsync(expectedProduct);

        // Act
        var response = await _client.GetAsync($"api/Product/{productId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultDto = await response.Content.ReadFromJsonAsync<ProductDTO>();
        Assert.NotNull(resultDto);
        Assert.Equal(productId, resultDto.Id);
        Assert.Equal("Навушники", resultDto.Name);
        _factory.ProductService.Verify(service => service.GetProductByIdAsync(productId), Times.Once());
    }

    [Fact]
    public async Task GetProductById_WhenProductDoesNotExist_Returns404NotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _factory.ProductService
            .Setup(service => service.GetProductByIdAsync(productId))
            .ReturnsAsync((ProductDTO?)null);

        // Act
        var response = await _client.GetAsync($"api/Product/{productId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        _factory.ProductService.Verify(service => service.GetProductByIdAsync(productId), Times.Once());
    }

    #endregion

    #region POST api/Product (CreateProduct)

    [Fact]
    public async Task CreateProduct_WhenFormDataIsValid_Returns201Created_WithProductDto()
    {
        // Arrange
        var createdProduct = new ProductDTO
        {
            Id = Guid.NewGuid(),
            Name = "Смартфон",
            Price = 18500,
            Description = "Нова модель",
            Quantity = 15
        };

        _factory.ProductService
            .Setup(service => service.CreateProductAsync(It.IsAny<CreateProductDTO>()))
            .ReturnsAsync(createdProduct);

        using var formData = new MultipartFormDataContent
        {
            { new StringContent("Смартфон"), nameof(CreateProductDTO.Name) },
            { new StringContent("18500"), nameof(CreateProductDTO.Price) },
            { new StringContent("Нова модель"), nameof(CreateProductDTO.Description) },
            { new StringContent("15"), nameof(CreateProductDTO.Quantity) }
        };

        // Act
        var response = await _client.PostAsync("api/Product", formData);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var resultDto = await response.Content.ReadFromJsonAsync<ProductDTO>();
        Assert.NotNull(resultDto);
        Assert.Equal(createdProduct.Id, resultDto.Id);
        Assert.Equal("Смартфон", resultDto.Name);
        _factory.ProductService.Verify(service => service.CreateProductAsync(It.IsAny<CreateProductDTO>()), Times.Once());
    }

    [Fact]
    public async Task CreateProduct_WhenFormDataIsInvalid_Returns400BadRequest()
    {
        // Arrange — передаємо порожню назву, нульову ціну та від'ємну кількість
        using var formData = new MultipartFormDataContent
        {
            { new StringContent(""), nameof(CreateProductDTO.Name) },
            { new StringContent("0"), nameof(CreateProductDTO.Price) },
            { new StringContent("-5"), nameof(CreateProductDTO.Quantity) }
        };

        // Act
        var response = await _client.PostAsync("api/Product", formData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        _factory.ProductService.Verify(service => service.CreateProductAsync(It.IsAny<CreateProductDTO>()), Times.Never());
    }

    #endregion

    #region POST api/Product/{id}/images (UploadImages)

    [Fact]
    public async Task UploadImages_WhenFilesAreValidAndProductExists_Returns200OK_WithUpdatedProductDto()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var updatedProduct = new ProductDTO
        {
            Id = productId,
            Name = "Планшет",
            Images = new List<string> { "/images/products/photo1.jpg" }
        };

        _factory.ProductService
            .Setup(service => service.AddImagesAsync(productId, It.IsAny<List<IFormFile>>()))
            .ReturnsAsync(updatedProduct);

        using var formData = new MultipartFormDataContent();
        var fileBytes = Encoding.UTF8.GetBytes("fake-image-content");
        var fileContent = new ByteArrayContent(fileBytes);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        formData.Add(fileContent, "files", "photo1.jpg");

        // Act
        var response = await _client.PostAsync($"api/Product/{productId}/images", formData);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultDto = await response.Content.ReadFromJsonAsync<ProductDTO>();
        Assert.NotNull(resultDto);
        Assert.Single(resultDto.Images);
        _factory.ProductService.Verify(service => service.AddImagesAsync(productId, It.IsAny<List<IFormFile>>()), Times.Once());
    }

    [Fact]
    public async Task UploadImages_WhenNoFilesProvided_Returns400BadRequest()
    {
        // Arrange — валідна multipart-форма без поля "files"
        var productId = Guid.NewGuid();
        using var formData = new MultipartFormDataContent
        {
            { new StringContent("empty"), "placeholder" }
        };

        // Act
        var response = await _client.PostAsync($"api/Product/{productId}/images", formData);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        _factory.ProductService.Verify(
            service => service.AddImagesAsync(It.IsAny<Guid>(), It.IsAny<List<IFormFile>>()),
            Times.Never());
    }

    [Fact]
    public async Task UploadImages_WhenProductDoesNotExist_Returns404NotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _factory.ProductService
            .Setup(service => service.AddImagesAsync(productId, It.IsAny<List<IFormFile>>()))
            .ReturnsAsync((ProductDTO?)null);

        using var formData = new MultipartFormDataContent();
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes("fake-image-content"));
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
        formData.Add(fileContent, "files", "photo1.jpg");

        // Act
        var response = await _client.PostAsync($"api/Product/{productId}/images", formData);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        _factory.ProductService.Verify(service => service.AddImagesAsync(productId, It.IsAny<List<IFormFile>>()), Times.Once());
    }

    #endregion

    #region PUT api/Product/{id} (UpdateProduct)

    [Fact]
    public async Task UpdateProduct_WhenProductExistsAndDtoIsValid_Returns200OK_WithUpdatedProductDto()
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

        _factory.ProductService
            .Setup(service => service.UpdateProductAsync(productId, It.IsAny<UpdateProductDTO>()))
            .ReturnsAsync(updatedProduct);

        // Act
        var response = await _client.PutAsJsonAsync($"api/Product/{productId}", updateDto);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var resultDto = await response.Content.ReadFromJsonAsync<ProductDTO>();
        Assert.NotNull(resultDto);
        Assert.Equal(productId, resultDto.Id);
        Assert.Equal("Оновлений ноутбук", resultDto.Name);
        _factory.ProductService.Verify(service => service.UpdateProductAsync(productId, It.IsAny<UpdateProductDTO>()), Times.Once());
    }

    [Fact]
    public async Task UpdateProduct_WhenProductDoesNotExist_Returns404NotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var updateDto = new UpdateProductDTO
        {
            Name = "Неіснуючий товар",
            Price = 100,
            Quantity = 1
        };

        _factory.ProductService
            .Setup(service => service.UpdateProductAsync(productId, It.IsAny<UpdateProductDTO>()))
            .ReturnsAsync((ProductDTO?)null);

        // Act
        var response = await _client.PutAsJsonAsync($"api/Product/{productId}", updateDto);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        _factory.ProductService.Verify(service => service.UpdateProductAsync(productId, It.IsAny<UpdateProductDTO>()), Times.Once());
    }

    [Fact]
    public async Task UpdateProduct_WhenDtoIsInvalid_Returns400BadRequest()
    {
        // Arrange
        var productId = Guid.NewGuid();
        var invalidDto = new UpdateProductDTO
        {
            Name = "",
            Price = -10,
            Quantity = -1
        };

        // Act
        var response = await _client.PutAsJsonAsync($"api/Product/{productId}", invalidDto);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        _factory.ProductService.Verify(
            service => service.UpdateProductAsync(It.IsAny<Guid>(), It.IsAny<UpdateProductDTO>()),
            Times.Never());
    }

    #endregion

    #region DELETE api/Product/{id} (DeleteProduct)

    [Fact]
    public async Task DeleteProduct_WhenProductExists_Returns204NoContent()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _factory.ProductService
            .Setup(service => service.DeleteAsync(productId))
            .ReturnsAsync(true);

        // Act
        var response = await _client.DeleteAsync($"api/Product/{productId}");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        _factory.ProductService.Verify(service => service.DeleteAsync(productId), Times.Once());
    }

    [Fact]
    public async Task DeleteProduct_WhenProductDoesNotExist_Returns404NotFound()
    {
        // Arrange
        var productId = Guid.NewGuid();

        _factory.ProductService
            .Setup(service => service.DeleteAsync(productId))
            .ReturnsAsync(false);

        // Act
        var response = await _client.DeleteAsync($"api/Product/{productId}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        _factory.ProductService.Verify(service => service.DeleteAsync(productId), Times.Once());
    }

    #endregion

    #region GET api/Product/me (GetClientInfo)

    [Fact]
    public async Task GetClientInfo_Returns200OK()
    {
        // Act
        var response = await _client.GetAsync("api/Product/me");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    #endregion
}