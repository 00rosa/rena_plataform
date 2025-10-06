using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rena.Application.DTOs.Products;
using Rena.Domain.Enums;

namespace Rena.Application.Interfaces;

public interface IProductService
{
    Task<ProductDto?> GetProductByIdAsync(Guid id);
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<IEnumerable<ProductDto>> GetProductsByUserAsync(Guid userId);
    Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm, Guid? categoryId = null);
    Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto, Guid userId);
    Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductDto updateProductDto, Guid userId);
    Task<bool> DeleteProductAsync(Guid id, Guid userId);
    //Task<bool> ToggleProductStatusAsync(Guid id, Guid userId, Domain.Enums.ProductStatus newStatus);
    // Agregar este método a la interfaz existente
    Task<bool> ToggleProductStatusAsync(Guid id, Guid userId, ProductStatus newStatus);
}