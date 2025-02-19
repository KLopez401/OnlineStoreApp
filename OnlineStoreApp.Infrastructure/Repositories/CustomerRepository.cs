using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Application.Interface.Repository;
using OnlineStoreApp.Domain.Entities;
using OnlineStoreApp.Infrastructure.Context;

namespace OnlineStoreApp.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;
    public CustomerRepository(ApplicationDbContext context) => _context = context;
    public async Task AddAsync(Customers entity)
    {
        _context.Customers.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Customers>> GetAllAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    public async Task<Customers?> FindActiveRecordByIdAsync(Guid id)
    {
        return await _context.Customers.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Customers?> GetByIdAsync(Guid id)
    {
        return await _context.Customers.FindAsync(id);
    }

    public async Task UpdateAsync(Customers entity)
    {
        _context.Customers.Update(entity);
        await _context.SaveChangesAsync();
    }
}
