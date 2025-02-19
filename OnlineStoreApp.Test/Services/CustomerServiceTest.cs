using Moq;
using OnlineStoreApp.Application.DTO.Customers;
using OnlineStoreApp.Application.Interface.Repository;
using OnlineStoreApp.Application.Services;
using OnlineStoreApp.Domain.Entities;

namespace OnlineStoreApp.Test.Services;

public class CustomerServiceTest
{
    private readonly Mock<ICustomerRepository> _customerRepositoryMock;
    private readonly CustomerService _customerService;

    public CustomerServiceTest()
    {
        _customerRepositoryMock = new Mock<ICustomerRepository>();
        _customerService = new CustomerService(_customerRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateCustomer_ShouldReturnNewCustomerId()
    {
        var customerDto = new CustomerDto
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            Phone = "1234567890"
        };

        _customerRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Customers>()))
            .Returns(Task.CompletedTask);

        var result = await _customerService.CreateCustomer(customerDto);

        Assert.NotEqual(Guid.Empty, result);
    }

    [Fact]
    public async Task DeleteCustomer_WhenCustomerExists_ShouldMarkAsDeleted()
    {
        var customerId = Guid.NewGuid();
        var customer = new Customers { Id = customerId, IsDeleted = false };
        _customerRepositoryMock.Setup(repo => repo.FindActiveRecordByIdAsync(customerId))
            .ReturnsAsync(customer);
        _customerRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Customers>()))
            .Returns(Task.CompletedTask);

        await _customerService.DeleteCustomer(customerId);

        Assert.True(customer.IsDeleted);
    }

    [Fact]
    public async Task DeleteCustomer_WhenCustomerDoesNotExist_ShouldThrowException()
    {
        var customerId = Guid.NewGuid();
        _customerRepositoryMock.Setup(repo => repo.FindActiveRecordByIdAsync(customerId))
            .ReturnsAsync((Customers)null);

        await Assert.ThrowsAsync<Exception>(() => _customerService.DeleteCustomer(customerId));
    }

    [Fact]
    public async Task GetCustomerById_WhenCustomerExists_ShouldReturnCustomerDto()
    {
        var customerId = Guid.NewGuid();
        var customer = new Customers
        {
            Id = customerId,
            FirstName = "Jane",
            LastName = "Doe",
            Email = "jane.doe@example.com",
            Phone = "9876543210",
            DateAdded = DateTime.Now
        };

        _customerRepositoryMock.Setup(repo => repo.FindActiveRecordByIdAsync(customerId))
            .ReturnsAsync(customer);

        var result = await _customerService.GetCustomerById(customerId);

        Assert.NotNull(result);
        Assert.Equal(customer.FirstName, result.FirstName);
    }

    [Fact]
    public async Task GetCustomers_ShouldReturnNonDeletedCustomers()
    {
        var customers = new List<Customers>
        {
            new Customers { Id = Guid.NewGuid(), FirstName = "Alice", LastName = "Smith", Email = "alice@example.com", Phone = "1111111111", DateAdded = DateTime.Now, IsDeleted = false },
            new Customers { Id = Guid.NewGuid(), FirstName = "Bob", LastName = "Brown", Email = "bob@example.com", Phone = "2222222222", DateAdded = DateTime.Now, IsDeleted = true }
        };

        _customerRepositoryMock.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(customers);

        var result = await _customerService.GetCustomers();

        Assert.Single(result);
        Assert.Contains(result, c => c.FirstName == "Alice");
    }

    [Fact]
    public async Task UpdateCustomer_WhenCustomerExists_ShouldUpdateCustomer()
    {
        var customerId = Guid.NewGuid();
        var customer = new Customers { Id = customerId, FirstName = "Old", LastName = "Name" };
        var updatedDto = new CustomerDto { FirstName = "New", LastName = "Name" };

        _customerRepositoryMock.Setup(repo => repo.FindActiveRecordByIdAsync(customerId))
            .ReturnsAsync(customer);
        _customerRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<Customers>()))
            .Returns(Task.CompletedTask);

        await _customerService.UpdateCustomer(customerId, updatedDto);

        Assert.Equal("New", customer.FirstName);
    }

    [Fact]
    public async Task UpdateCustomer_WhenCustomerDoesNotExist_ShouldThrowException()
    {
        var customerId = Guid.NewGuid();
        var updatedDto = new CustomerDto { FirstName = "New", LastName = "Name" };

        _customerRepositoryMock.Setup(repo => repo.FindActiveRecordByIdAsync(customerId))
            .ReturnsAsync((Customers)null);

        await Assert.ThrowsAsync<Exception>(() => _customerService.UpdateCustomer(customerId, updatedDto));
    }
}
