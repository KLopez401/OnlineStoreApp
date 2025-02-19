using Moq;
using OnlineStoreApp.Application.Application.DTO.Products;
using OnlineStoreApp.Application.DTO.CustomerPurchases;
using OnlineStoreApp.Application.Interface.Repository;
using OnlineStoreApp.Application.Services;
using OnlineStoreApp.Domain.Entities;
using System.Text.Json;

namespace OnlineStoreApp.Test.Services;
public class CustomerPurchaseServiceTests
{
    private readonly Mock<ICustomerPurchaseRepository> _mockRepository;
    private readonly CustomerPurchaseService _service;

    public CustomerPurchaseServiceTests()
    {
        _mockRepository = new Mock<ICustomerPurchaseRepository>();
        _service = new CustomerPurchaseService(_mockRepository.Object);
    }

    [Fact]
    public async Task CreateCustomerPurchase_ShouldReturnValidGuid()
    {
        var dto = new CustomerPurchaseDto
        {
            CustomerId = Guid.NewGuid(),
            Products = new List<ProductInfoDto>
            {
                new ProductInfoDto { Name = "Item1", Price = 10 },
                new ProductInfoDto { Name = "Item2", Price = 15 }
            },
            Total = 25
        };

        _mockRepository
            .Setup(repo => repo.AddAsync(It.IsAny<CustomerPurchases>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateCustomerPurchase(dto);

        Assert.NotEqual(Guid.Empty, result);
    }

    [Fact]
    public async Task CreateCustomerPurchase_ShouldHandleNullProducts()
    {
        var dto = new CustomerPurchaseDto
        {
            CustomerId = Guid.NewGuid(),
            Products = null,
            Total = 0
        };

        _mockRepository
            .Setup(repo => repo.AddAsync(It.IsAny<CustomerPurchases>()))
            .Returns(Task.CompletedTask);

        var result = await _service.CreateCustomerPurchase(dto);

        Assert.NotEqual(Guid.Empty, result);
    }

    [Fact]
    public async Task GetCustomerPurchaseById_ShouldReturnValidPurchase()
    {
        var id = Guid.NewGuid();
        var expectedPurchase = new CustomerPurchases
        {
            Id = id,
            CustomerId = Guid.NewGuid(),
            Products = JsonSerializer.Serialize(new List<ProductInfoDto>
            {
                new ProductInfoDto { Name = "Item1", Price = 10 }
            }),
            Total = 10,
            PurchaseDate = DateTime.Now,
            ReceiptReference = "1234567",
            IsDeleted = false
        };

        _mockRepository
            .Setup(repo => repo.FindActiveRecordByIdAsync(id))
            .ReturnsAsync(expectedPurchase);

        var result = await _service.GetCustomerPurchaseById(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Single(result.Products);
        Assert.Equal("Item1", result.Products[0].Name);
        Assert.Equal(10, result.Products[0].Price);
    }

    [Fact]
    public async Task GetCustomerPurchases_ShouldReturnEmptyListIfNoPurchases()
    {
        var purchases = new List<CustomerPurchases>();

        _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(purchases);

        var result = await _service.GetCustomerPurchases();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task DeleteCustomerPurchase_ShouldMarkPurchaseAsDeleted()
    {
        var id = Guid.NewGuid();
        var purchase = new CustomerPurchases { Id = id, CustomerId = Guid.NewGuid(), IsDeleted = false };

        _mockRepository.Setup(repo => repo.FindActiveRecordByIdAsync(id)).ReturnsAsync(purchase);
        _mockRepository.Setup(repo => repo.UpdateAsync(purchase)).Returns(Task.CompletedTask);

        await _service.DeleteCustomerPurchase(id);

        Assert.True(purchase.IsDeleted);
    }

    [Fact]
    public async Task DeleteCustomerPurchase_ShouldThrowExceptionIfNotFound()
    {
        var id = Guid.NewGuid();

        _mockRepository.Setup(repo => repo.FindActiveRecordByIdAsync(id)).ReturnsAsync((CustomerPurchases)null);

        await Assert.ThrowsAsync<Exception>(() => _service.DeleteCustomerPurchase(id));
    }
}