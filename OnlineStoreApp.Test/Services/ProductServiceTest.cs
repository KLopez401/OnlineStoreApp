using Moq;
using OnlineStoreApp.Application.DTO.Products;
using OnlineStoreApp.Application.Interface.Repository;
using OnlineStoreApp.Application.Services;
using OnlineStoreApp.Domain.Entities;

namespace OnlineStoreApp.Test.Services;

public class ProductServiceTest
{
    private readonly Mock<IProductRepository> _mockRepo;
    private readonly ProductService _productService;

    public ProductServiceTest()
    {
        _mockRepo = new Mock<IProductRepository>();
        _productService = new ProductService(_mockRepo.Object);
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnNewProductId()
    {
        var dto = new ProductDto
        {
            Name = "Test Product",
            Description = "Test Description",
            Price = 100
        };

        _mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Products>())).Returns(Task.CompletedTask);

        var result = await _productService.CreateProduct(dto);

        Assert.NotEqual(Guid.Empty, result);
    }

    [Fact]
    public async Task DeleteProduct_ShouldMarkProductAsDeleted()
    {
        var productId = Guid.NewGuid();
        var product = new Products { Id = productId, IsDeleted = false };

        _mockRepo.Setup(repo => repo.FindActiveRecordByIdAsync(productId))
                 .ReturnsAsync(product);
        _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Products>())).Returns(Task.CompletedTask);

        await _productService.DeleteProduct(productId);

        Assert.True(product.IsDeleted);
    }

    [Fact]
    public async Task DeleteProduct_ShouldThrowException_WhenProductNotFound()
    {
        var productId = Guid.NewGuid();
        _mockRepo.Setup(repo => repo.FindActiveRecordByIdAsync(productId)).ReturnsAsync((Products)null);

        await Assert.ThrowsAsync<Exception>(() => _productService.DeleteProduct(productId));
    }

    [Fact]
    public async Task GetProductById_ShouldReturnProductDto()
    {
        var productId = Guid.NewGuid();
        var product = new Products
        {
            Id = productId,
            Name = "Test Product",
            Description = "Test Description",
            Price = 100,
            DateAdded = DateTime.Now
        };

        _mockRepo.Setup(repo => repo.FindActiveRecordByIdAsync(productId)).ReturnsAsync(product);

        var result = await _productService.GetProductById(productId);

        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
        Assert.Equal("Test Product", result.Name);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnEmptyDto_WhenProductNotFound()
    {
        var productId = Guid.NewGuid();
        _mockRepo.Setup(repo => repo.FindActiveRecordByIdAsync(productId)).ReturnsAsync((Products)null);

        var result = await _productService.GetProductById(productId);

        Assert.NotNull(result);
        Assert.Equal(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnActiveProducts()
    {
        var products = new List<Products>
        {
            new Products { Id = Guid.NewGuid(), Name = "Product 1", Description = "Desc 1", Price = 100, DateAdded = DateTime.Now, IsDeleted = false },
            new Products { Id = Guid.NewGuid(), Name = "Product 2", Description = "Desc 2", Price = 200, DateAdded = DateTime.Now.AddDays(-1), IsDeleted = false },
            new Products { Id = Guid.NewGuid(), Name = "Deleted Product", Description = "Desc 3", Price = 300, DateAdded = DateTime.Now.AddDays(-2), IsDeleted = true }
        };

        _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

        var result = await _productService.GetProducts();

        Assert.Equal(2, result.Count());
        Assert.DoesNotContain(result, p => p.Name == "Deleted Product");
    }

    [Fact]
    public async Task UpdateProduct_ShouldModifyExistingProduct()
    {
        var productId = Guid.NewGuid();
        var product = new Products
        {
            Id = productId,
            Name = "Old Name",
            Description = "Old Description",
            Price = 50
        };

        var updateDto = new ProductDto
        {
            Name = "New Name",
            Description = "New Description",
            Price = 150
        };

        _mockRepo.Setup(repo => repo.FindActiveRecordByIdAsync(productId)).ReturnsAsync(product);
        _mockRepo.Setup(repo => repo.UpdateAsync(It.IsAny<Products>())).Returns(Task.CompletedTask);

        await _productService.UpdateProduct(productId, updateDto);

        Assert.Equal("New Name", product.Name);
        Assert.Equal("New Description", product.Description);
        Assert.Equal(150, product.Price);
    }

    [Fact]
    public async Task UpdateProduct_ShouldThrowException_WhenProductNotFound()
    {
        var productId = Guid.NewGuid();
        var updateDto = new ProductDto
        {
            Name = "New Name",
            Description = "New Description",
            Price = 150
        };

        _mockRepo.Setup(repo => repo.FindActiveRecordByIdAsync(productId)).ReturnsAsync((Products)null);

        await Assert.ThrowsAsync<Exception>(() => _productService.UpdateProduct(productId, updateDto));
    }
}