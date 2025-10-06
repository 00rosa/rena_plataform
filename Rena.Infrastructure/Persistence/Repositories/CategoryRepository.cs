using Microsoft.EntityFrameworkCore;
using Rena.Domain.Entities;
using Rena.Domain.Interfaces;
using Rena.Infrastructure.Data;

namespace Rena.Infrastructure.Persistence.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Category?> GetByIdAsync(Guid id)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task AddAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
    }

    public void Update(Category category)
    {
        _context.Categories.Update(category);
    }

    public void Delete(Category category)
    {
        // Soft delete
        category.IsActive = false;
        _context.Categories.Update(category);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Categories
            .AnyAsync(c => c.Id == id && c.IsActive);
    }

    public async Task<bool> NameExistsAsync(string name)
    {
        return await _context.Categories
            .AnyAsync(c => c.Name == name && c.IsActive);
    }
}