using Microsoft.EntityFrameworkCore;
using Rena.Domain.Entities;
using Rena.Domain.Interfaces;
using Rena.Infrastructure.Data;

namespace Rena.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.User)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.User)
            .Include(p => p.Images)
            .Where(p => p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.UserId == userId && p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.User)
            .Include(p => p.Images)
            .Where(p => p.CategoryId == categoryId && p.IsActive)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> SearchAsync(string searchTerm)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.User)
            .Include(p => p.Images)
            .Where(p => p.IsActive &&
                   (p.Title.Contains(searchTerm) ||
                    p.Description.Contains(searchTerm) ||
                    p.Category.Name.Contains(searchTerm)))
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAvailableProductsAsync()
    {
        return await _context.Products
            .Include(p => p.Category)
            .Include(p => p.User)
            .Include(p => p.Images)
            .Where(p => p.IsActive && p.Status == Domain.Enums.ProductStatus.Available)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public void Update(Product product)
    {
        _context.Products.Update(product);
    }

    public void Delete(Product product)
    {
        // Soft delete
        product.IsActive = false;
        _context.Products.Update(product);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Products
            .AnyAsync(p => p.Id == id && p.IsActive);
    }
}