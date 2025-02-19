using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineStoreApp.Application.DTO.Products;
using OnlineStoreApp.Application.Interface.Services;
using OnlineStoreApp.Presentation.Controllers;

namespace OnlineStoreApp.Test.Controllers;

public class ProductApiControllerTest
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly ProductApiController _controller;

    public ProductApiControllerTest()
    {
        _mockProductService = new Mock<IProductService>();
        _controller = new ProductApiController(_mockProductService.Object);
    }

    [Fact]
    public async Task GetProducts_ReturnsOkResult_WithProducts()
    {
        var products = new List<GetProductDto> { new GetProductDto { Id = Guid.NewGuid(), Name = "Product1" } };
        _mockProductService.Setup(s => s.GetProducts()).ReturnsAsync(products);

        var result = await _controller.GetProducts();
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedProducts = Assert.IsType<List<GetProductDto>>(okResult.Value);
        Assert.NotNull(returnedProducts);
        Assert.Single(returnedProducts);
    }

    [Fact]
    public async Task GetProducts_ReturnsNotFound_WhenNoProducts()
    {
        _mockProductService.Setup(s => s.GetProducts()).ReturnsAsync((List<GetProductDto>)null);

        var result = await _controller.GetProducts();
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetProductById_ReturnsOk_WithProduct()
    {
        var product = new GetProductDto { Id = Guid.NewGuid(), Name = "Product1" };
        _mockProductService.Setup(s => s.GetProductById(product.Id)).ReturnsAsync(product);

        var result = await _controller.GetProductById(product.Id);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedProduct = Assert.IsType<GetProductDto>(okResult.Value);
        Assert.Equal(product.Id, returnedProduct.Id);
    }

    [Fact]
    public async Task GetProductById_ReturnsNotFound_WhenProductNotExists()
    {
        _mockProductService.Setup(s => s.GetProductById(It.IsAny<Guid>())).ReturnsAsync((GetProductDto)null);

        var result = await _controller.GetProductById(Guid.NewGuid());
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task CreateProduct_ReturnsOk_WithNewProductId()
    {
        var productDto = new ProductDto { Name = "New Product", Description = "Description Product", Price = 10};
        var newProductId = Guid.NewGuid();
        _mockProductService.Setup(s => s.CreateProduct(productDto)).ReturnsAsync(newProductId);

        var result = await _controller.CreateProduct(productDto);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(newProductId, okResult.Value);
    }

    [Fact]
    public async Task CreateProduct_ReturnsBadRequest_WhenRequestIsNull()
    {
        var result = await _controller.CreateProduct(null);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsNoContent()
    {
        var productDto = new ProductDto { Name = "Updated Product" };
        var productId = Guid.NewGuid();

        var result = await _controller.UpdateProduct(productId, productDto);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsBadRequest_WhenRequestIsNull()
    {
        var result = await _controller.UpdateProduct(Guid.NewGuid(), null);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNoContent()
    {
        var productId = Guid.NewGuid();

        var result = await _controller.DeleteProduct(productId);
        Assert.IsType<NoContentResult>(result);
    }
}
