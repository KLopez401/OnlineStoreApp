using Microsoft.EntityFrameworkCore;
using OnlineStoreApp.Application.Interface.Repository;
using OnlineStoreApp.Domain.Entities;
using OnlineStoreApp.Infrastructure.Context;

namespace OnlineStoreApp.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    public ProductRepository(ApplicationDbContext context) => _context = context;
    public async Task AddAsync(Products entity)
    {
        _context.Products.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<IEnumerable<Products>> GetAllAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Products?> GetByIdAsync(Guid id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Products?> FindActiveRecordByIdAsync(Guid id)
    {
        return await _context.Products.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task UpdateAsync(Products entity)
    {
        _context.Products.Update(entity);
        await _context.SaveChangesAsync();
    }
}
