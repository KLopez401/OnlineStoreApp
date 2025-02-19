using OnlineStoreApp.Application.Application.DTO.Helper.Pagination.Response;
using OnlineStoreApp.Application.DTO.CustomerPurchases;

namespace OnlineStoreApp.Application.Interface.Services;

public interface ICustomerPurchaseService
{
    Task<IEnumerable<GetCustomerPurchaseDto>> GetCustomerPurchases();
    Task<GetCustomerPurchaseDto> GetCustomerPurchaseById(Guid id);
    Task<Guid> CreateCustomerPurchase(CustomerPurchaseDto dto);
    Task DeleteCustomerPurchase(Guid id);
}
