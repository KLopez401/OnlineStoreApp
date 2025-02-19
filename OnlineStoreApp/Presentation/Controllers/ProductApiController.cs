using Microsoft.AspNetCore.Mvc;
using OnlineStoreApp.Application.Application.DTO.Helper.Pagination.Response;
using OnlineStoreApp.Application.DTO.Products;
using OnlineStoreApp.Application.Interface.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OnlineStoreApp.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductApiController : ControllerBase
{
    private IProductService _productService;
    public ProductApiController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<GetProductDto>), 200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> GetProducts()
    {
        var result = await _productService.GetProducts();

        if (result == null)
        {
            return NotFound("No available products.");
        }

        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetProductDto), 200)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var result = await _productService.GetProductById(id);

        if (result == null)
        {
            return NotFound("No available products.");
        }

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), 200)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> CreateProduct([FromBody] ProductDto request)
    {
        if (request == null)
        {
            return BadRequest("Request should not be empty.");
        }

        var result = await _productService.CreateProduct(request);

        return Ok(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductDto request)
    {
        if (request == null)
        {
            return BadRequest("Request should not be empty.");
        }

        await _productService.UpdateProduct(id, request);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(Error), 400)]
    [ProducesResponseType(typeof(Error), 500)]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        await _productService.DeleteProduct(id);

        return NoContent();
    }
}
