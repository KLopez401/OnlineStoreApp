using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineStoreApp.Application.DTO.Customers;
using OnlineStoreApp.Application.Interface.Services;
using OnlineStoreApp.Presentation.Controllers;

namespace OnlineStoreApp.Test.Controllers;

public class CustomerApiControllerTest
{
    private readonly Mock<ICustomerService> _mockService;
    private readonly CustomerApiController _controller;

    public CustomerApiControllerTest()
    {
        _mockService = new Mock<ICustomerService>();
        _controller = new CustomerApiController(_mockService.Object);
    }

    [Fact]
    public async Task GetCustomers_ReturnsOkResult_WithCustomerList()
    {
        var customers = new List<GetCustomerDto> { new GetCustomerDto { Id = Guid.NewGuid(), FirstName = "John", LastName = "Doe" } };
        _mockService.Setup(s => s.GetCustomers()).ReturnsAsync(customers);

        var result = await _controller.GetCustomers();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(customers, okResult.Value);
    }

    [Fact]
    public async Task GetCustomers_ReturnsNotFound_WhenNoCustomers()
    {
        _mockService.Setup(s => s.GetCustomers()).ReturnsAsync((List<GetCustomerDto>)null);

        var result = await _controller.GetCustomers();

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetCustomerById_ReturnsOkResult_WhenCustomerExists()
    {
        var customer = new GetCustomerDto { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Doe", Email = "janedoe@gmail.com" };
        _mockService.Setup(s => s.GetCustomerById(customer.Id)).ReturnsAsync(customer);

        var result = await _controller.GetCustomers(customer.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(customer, okResult.Value);
    }

    [Fact]
    public async Task GetCustomerById_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        _mockService.Setup(s => s.GetCustomerById(It.IsAny<Guid>())).ReturnsAsync((GetCustomerDto)null);

        var result = await _controller.GetCustomers(Guid.NewGuid());
 
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task CreateCustomer_ReturnsOk_WithNewCustomerId()
    {
        var request = new CustomerDto { FirstName = "New", LastName = "Customer" };
        var customerId = Guid.NewGuid();
        _mockService.Setup(s => s.CreateCustomer(request)).ReturnsAsync(customerId);

        var result = await _controller.CreateCustomer(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(customerId, okResult.Value);
    }

    [Fact]
    public async Task CreateCustomer_ReturnsBadRequest_WhenRequestIsNull()
    {
        var result = await _controller.CreateCustomer(null);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateCustomer_ReturnsNoContent_WhenSuccessful()
    {
        var id = Guid.NewGuid();
        var request = new CustomerDto { FirstName = "Updated", LastName = "Customer" };

        var result = await _controller.UpdateCustomer(id, request);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateCustomer_ReturnsBadRequest_WhenRequestIsNull()
    {
        var id = Guid.NewGuid();

        var result = await _controller.UpdateCustomer(id, null);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeleteCustomer_ReturnsNoContent_WhenSuccessful()
    {
        var id = Guid.NewGuid();
          
        var result = await _controller.DeleteCustomer(id);

        Assert.IsType<NoContentResult>(result);
    }
}
