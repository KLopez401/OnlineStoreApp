using OnlineStoreApp.Application.Application.DTO.Helper.Pagination.Response;
using OnlineStoreApp.Application.DTO.Customers;

namespace OnlineStoreApp.Application.Interface.Services;

public interface ICustomerService
{
    Task<IEnumerable<GetCustomerDto>> GetCustomers();
    Task<GetCustomerDto> GetCustomerById(Guid id);
    Task<Guid> CreateCustomer(CustomerDto dto);
    Task UpdateCustomer(Guid id, CustomerDto dto);
    Task DeleteCustomer(Guid id);
}
