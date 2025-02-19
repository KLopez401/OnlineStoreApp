using Microsoft.AspNetCore.Mvc;
using OnlineStoreApp.Application.Application.DTO.Helper.Pagination.Response;
using OnlineStoreApp.Application.DTO.CustomerPurchases;
using OnlineStoreApp.Application.Interface.Services;
using OnlineStoreApp.Application.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OnlineStoreApp.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerPurchaseApiController : ControllerBase
{
    private ICustomerPurchaseService _purchaseService;
    public CustomerPurchaseApiController(ICustomerPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetCustomerPurchaseDto>), 200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> GetPurchases()
    {
        var result = await _purchaseService.GetCustomerPurchases();

        if (result == null)
        {
            return NotFound("No available customer order.");
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetCustomerPurchaseDto), 200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> GetCustomerById(Guid id)
    {
        var result = await _purchaseService.GetCustomerPurchaseById(id);

        if (result == null)
        {
            return NotFound("No available customer order.");
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), 200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerPurchaseDto request)
    {
        if (request == null)
        {
            return BadRequest("Request should not be empty.");
        }

        if (request.CustomerId == Guid.Empty)
        {
            return BadRequest("Customer should not be empty.");
        }

        var result = await _purchaseService.CreateCustomerPurchase(request);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        await _purchaseService.DeleteCustomerPurchase(id);

        return NoContent();
    }
}
