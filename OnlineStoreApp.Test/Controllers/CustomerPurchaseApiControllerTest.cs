using Microsoft.AspNetCore.Mvc;
using Moq;
using OnlineStoreApp.Application.DTO.CustomerPurchases;
using OnlineStoreApp.Application.Interface.Services;
using OnlineStoreApp.Presentation.Controllers;

namespace OnlineStoreApp.Test.Controllers;

public class CustomerPurchaseApiControllerTest
{
    private readonly Mock<ICustomerPurchaseService> _mockService;
    private readonly CustomerPurchaseApiController _controller;

    public CustomerPurchaseApiControllerTest()
    {
        _mockService = new Mock<ICustomerPurchaseService>();
        _controller = new CustomerPurchaseApiController(_mockService.Object);
    }

    [Fact]
    public async Task GetPurchases_ReturnsOkResult_WithListOfPurchases()
    {
        var purchases = new List<GetCustomerPurchaseDto> { new GetCustomerPurchaseDto() };
        _mockService.Setup(service => service.GetCustomerPurchases()).ReturnsAsync(purchases);

        var result = await _controller.GetPurchases();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(purchases, okResult.Value);
    }

    [Fact]
    public async Task GetPurchases_ReturnsNotFound_WhenNoPurchasesAvailable()
    {
        _mockService.Setup(service => service.GetCustomerPurchases()).ReturnsAsync((List<GetCustomerPurchaseDto>)null);

        var result = await _controller.GetPurchases();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetCustomerById_ReturnsOk_WhenCustomerExists()
    {
        var purchase = new GetCustomerPurchaseDto();
        var id = Guid.NewGuid();
        _mockService.Setup(service => service.GetCustomerPurchaseById(id)).ReturnsAsync(purchase);

        var result = await _controller.GetCustomerById(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(purchase, okResult.Value);
    }

    [Fact]
    public async Task GetCustomerById_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        var id = Guid.NewGuid();
        _mockService.Setup(service => service.GetCustomerPurchaseById(id)).ReturnsAsync((GetCustomerPurchaseDto)null);

        var result = await _controller.GetCustomerById(id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task CreateCustomer_ReturnsOk_WithGuid()
    {
        var request = new CustomerPurchaseDto { CustomerId = Guid.NewGuid() };
        var expectedId = Guid.NewGuid();
        _mockService.Setup(service => service.CreateCustomerPurchase(request)).ReturnsAsync(expectedId);

        var result = await _controller.CreateCustomer(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(expectedId, okResult.Value);
    }

    [Fact]
    public async Task CreateCustomer_ReturnsBadRequest_WhenRequestIsNull()
    {
        var result = await _controller.CreateCustomer(null);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task CreateCustomer_ReturnsBadRequest_WhenCustomerIdIsEmpty()
    {
        var request = new CustomerPurchaseDto { CustomerId = Guid.Empty };

        var result = await _controller.CreateCustomer(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNoContent()
    {
        var id = Guid.NewGuid();
        _mockService.Setup(service => service.DeleteCustomerPurchase(id)).Returns(Task.CompletedTask);

        var result = await _controller.DeleteProduct(id);

        var noContentResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(204, noContentResult.StatusCode);
    }
}
