using OnlineStoreApp.Application.Application.DTO.Helper.Pagination.Response;
using OnlineStoreApp.Application.DTO.Customers;
using OnlineStoreApp.Application.Interface.Repository;
using OnlineStoreApp.Application.Interface.Services;
using OnlineStoreApp.Domain.Entities;

namespace OnlineStoreApp.Application.Services;

public class CustomerService : ICustomerService
{
    private ICustomerRepository _customerRepository;
    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    public async Task<Guid> CreateCustomer(CustomerDto dto)
    {
        try
        {
            var id = Guid.NewGuid();

            var newCustomer = new Customers
            {
                Id = id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                DateAdded = DateTime.Now,
                IsDeleted = false
            };

            await _customerRepository.AddAsync(newCustomer);

            return newCustomer.Id;
        }
        catch
        {
            throw;
        }
    }

    public async Task DeleteCustomer(Guid id)
    {
        try
        {
            var customer = await _customerRepository.FindActiveRecordByIdAsync(id);

            if (customer == null)
            {
                throw new Exception("Customer not found");
            }

            customer.IsDeleted = true;

            await _customerRepository.UpdateAsync(customer);
        }
        catch
        {
            throw;
        }
    }

    public async Task<GetCustomerDto> GetCustomerById(Guid id)
    {
        try
        {
            var customer = await _customerRepository.FindActiveRecordByIdAsync(id);

            var getCustomerDto = new GetCustomerDto();
            if (customer != null)
            {
                getCustomerDto = new GetCustomerDto
                {
                    Id = customer.Id,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Email = customer.Email,
                    Phone = customer.Phone,
                    DateAdded = customer.DateAdded
                };
            }

            return getCustomerDto;
        }
        catch
        {
            throw;
        }
    }

    public async Task<IEnumerable<GetCustomerDto>> GetCustomers()
    {
        try
        {
            var customers = await _customerRepository.GetAllAsync();
            customers = customers.Where(x => x.IsDeleted == false);

            var getCustomerDto = customers.Select(x => new GetCustomerDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                Phone = x.Phone,
                DateAdded = x.DateAdded
            }).ToList()
              .OrderByDescending(x => x.DateAdded);

            return getCustomerDto;
        }
        catch
        {
            throw;
        }
    }

    public async Task UpdateCustomer(Guid id, CustomerDto dto)
    {
        try
        {
            var customer = await _customerRepository.FindActiveRecordByIdAsync(id);

            if (customer == null)
            {
                throw new Exception("Customer not found");
            }

            customer.FirstName = dto.FirstName;
            customer.LastName = dto.LastName;
            customer.Email = dto.Email;
            customer.Phone = dto.Phone;

            await _customerRepository.UpdateAsync(customer);
        
        }
        catch
        {
            throw;
        }
    }

    #region HelperMethod
    private IEnumerable<Customers> GetPaginatedCustomers(IEnumerable<Customers> customers, int page, int pageSize)
    {
        return customers = customers.Skip((page - 1) * pageSize)
                  .Take(pageSize)
                  .ToList();
    }
    #endregion
}
