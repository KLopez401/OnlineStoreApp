using Microsoft.AspNetCore.Mvc;
using OnlineStoreApp.Application.Application.DTO.Helper.Pagination.Response;
using OnlineStoreApp.Application.DTO.Customers;
using OnlineStoreApp.Application.Interface.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OnlineStoreApp.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomerApiController : ControllerBase
{
    private ICustomerService _customerService;
    public CustomerApiController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetCustomerDto>), 200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> GetCustomers()
    {
        var result = await _customerService.GetCustomers();

        if (result == null)
        {
            return NotFound("No available customer.");
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetCustomerDto), 200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> GetCustomers(Guid id)
    {
        var result = await _customerService.GetCustomerById(id);

        if (result == null)
        {
            return NotFound("No available customer.");
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), 200)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto request)
    {
        if (request == null)
        {
            return BadRequest("Request should not be empty.");
        }

        var result = await _customerService.CreateCustomer(request);

        return Ok(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] CustomerDto request)
    {
        if (request == null)
        {
            return BadRequest("Request should not be empty.");
        }

        await _customerService.UpdateCustomer(id, request);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> DeleteCustomer(Guid id)
    {
        await _customerService.DeleteCustomer(id);

        return NoContent();
    }
}
