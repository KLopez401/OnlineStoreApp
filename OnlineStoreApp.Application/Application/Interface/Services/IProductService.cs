using OnlineStoreApp.Application.Application.DTO.Helper.Pagination.Response;
using OnlineStoreApp.Application.DTO.Products;

namespace OnlineStoreApp.Application.Interface.Services;

public interface IProductService
{
    Task<IEnumerable<GetProductDto>> GetProducts();
    Task<GetProductDto> GetProductById(Guid id);
    Task<Guid> CreateProduct(ProductDto dto);
    Task UpdateProduct(Guid id, ProductDto dto);
    Task DeleteProduct(Guid id);
}
