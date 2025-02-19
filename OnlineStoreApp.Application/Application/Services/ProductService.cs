using OnlineStoreApp.Application.Application.DTO.Helper.Pagination.Response;
using OnlineStoreApp.Application.DTO.Products;
using OnlineStoreApp.Application.Interface.Repository;
using OnlineStoreApp.Application.Interface.Services;
using OnlineStoreApp.Domain.Entities;

namespace OnlineStoreApp.Application.Services;

public class ProductService : IProductService
{
    private IProductRepository _productRepository;
    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<Guid> CreateProduct(ProductDto dto)
    {
        try
        {
            var id = Guid.NewGuid();
            var product = new Products
            {
                Id = id,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                DateAdded = DateTime.Now,
                IsDeleted = false
            };

            await _productRepository.AddAsync(product);

            return product.Id;
        }
        catch
        {
            throw;
        }
    }

    public async Task DeleteProduct(Guid id)
    {
        try
        {
            var product = await _productRepository.FindActiveRecordByIdAsync(id);

            if (product == null)
            {
                throw new Exception("Product not found");
            }

            product.IsDeleted = true;
            await _productRepository.UpdateAsync(product);
        }
        catch
        {
            throw;
        }
    }

    public async Task<GetProductDto> GetProductById(Guid id)
    {
        try
        {
            var product = await _productRepository.FindActiveRecordByIdAsync(id);

            var getProductDto = new GetProductDto();
            if (product != null)
            {
                getProductDto = new GetProductDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    DateAdded = product.DateAdded
                };
            }
            
            return getProductDto;
        }
        catch
        {
           throw;
        }
    }

    public async Task<IEnumerable<GetProductDto>> GetProducts()
    {
        try
        {
            var products = await _productRepository.GetAllAsync();
            products = products.Where(x => x.IsDeleted == false);
            
            var getProductDto = products.Select(x => new GetProductDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                DateAdded = x.DateAdded
            }).ToList()
            .OrderByDescending(x => x.DateAdded);

            return getProductDto;
        }
        catch
        {
            throw;
        }
    }

    public async Task UpdateProduct(Guid id, ProductDto dto)
    {
        try
        {
            var product = await _productRepository.FindActiveRecordByIdAsync(id);

            if (product == null)
            {
                throw new Exception("Product not found");
            }

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            await _productRepository.UpdateAsync(product);
        }
        catch
        {
            throw;
        }
    }

    #region HelperMethod
    private IEnumerable<Products> GetPaginatedProducts(IEnumerable<Products> products, int page, int pageSize)
    {
        return products = products.Skip((page - 1) * pageSize)
                  .Take(pageSize)
                  .ToList();
    }
    #endregion
}
