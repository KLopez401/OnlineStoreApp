using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Application.Interface.Repository;
using OnlineStoreApp.Domain.Entities;
using OnlineStoreApp.Infrastructure.Context;

namespace OnlineStoreApp.Infrastructure.Repositories;

public class CustomerPurchaseRepository : ICustomerPurchaseRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerPurchaseRepository(ApplicationDbContext context) => _context = context;

    public async Task<IEnumerable<CustomerPurchases>> GetAllAsync()
    {
        return await _context.CustomerPurchases.ToListAsync();
    }

    public async Task<CustomerPurchases?> GetByIdAsync(Guid id)
    {
        return await _context.CustomerPurchases.FindAsync(id);
    }

    public async Task<CustomerPurchases?> FindActiveRecordByIdAsync(Guid id)
    {
        return await _context.CustomerPurchases.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task AddAsync(CustomerPurchases purchases)
    {
        _context.CustomerPurchases.Add(purchases);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(CustomerPurchases purchases)
    {
        _context.CustomerPurchases.Update(purchases);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var purchase = await _context.CustomerPurchases.FindAsync(id);
        if (purchase != null)
        {
            _context.CustomerPurchases.Remove(purchase);
            await _context.SaveChangesAsync();
        }
    }
}
