using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rena.Domain.Entities;

namespace Rena.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task<IEnumerable<Product>> GetByUserIdAsync(Guid userId);
    Task<IEnumerable<Product>> GetByCategoryAsync(Guid categoryId);
    Task<IEnumerable<Product>> SearchAsync(string searchTerm);
    Task<IEnumerable<Product>> GetAvailableProductsAsync();
    Task AddAsync(Product product);
    void Update(Product product);
    void Delete(Product product);
    Task<bool> ExistsAsync(Guid id);
}

