using AutoMapper;
using Rena.Application.DTOs.Products;
using Rena.Application.Interfaces;
using Rena.Domain.Entities;
using Rena.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Rena.Domain.Enums;

namespace Rena.Application.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductService> _logger;

    public ProductService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<ProductService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDto?> GetProductByIdAsync(Guid id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        return product == null ? null : _mapper.Map<ProductDto>(product);
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await _unitOfWork.Products.GetAvailableProductsAsync();
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductDto>> GetProductsByUserAsync(Guid userId)
    {
        var products = await _unitOfWork.Products.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<IEnumerable<ProductDto>> SearchProductsAsync(string searchTerm, Guid? categoryId = null)
    {
        IEnumerable<Domain.Entities.Product> products;

        if (categoryId.HasValue)
        {
            products = await _unitOfWork.Products.GetByCategoryAsync(categoryId.Value);
            products = products.Where(p =>
                p.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }
        else
        {
            products = await _unitOfWork.Products.SearchAsync(searchTerm);
        }

        return _mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto> CreateProductAsync(CreateProductDto createProductDto, Guid userId)
    {
        // Verificar que el usuario existe
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
        {
            throw new KeyNotFoundException("Usuario no encontrado.");
        }

        // Verificar que la categoría existe
        var category = await _unitOfWork.Categories.GetByIdAsync(createProductDto.CategoryId);
        if (category == null)
        {
            throw new KeyNotFoundException("Categoría no encontrada.");
        }

        // Crear producto
        var product = _mapper.Map<Domain.Entities.Product>(createProductDto);
        product.UserId = userId;

        // Agregar imágenes
        if (createProductDto.ImageUrls != null && createProductDto.ImageUrls.Any())
        {
            for (int i = 0; i < createProductDto.ImageUrls.Count; i++)
            {
                product.Images.Add(new ProductImage
                {
                    ImageUrl = createProductDto.ImageUrls[i],
                    SortOrder = i
                });
            }
        }

        await _unitOfWork.Products.AddAsync(product);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Producto creado: {Title} por usuario {UserId}", createProductDto.Title, userId);

        // Retornar el producto con datos completos
        var createdProduct = await _unitOfWork.Products.GetByIdAsync(product.Id);
        return _mapper.Map<ProductDto>(createdProduct!);
    }

    public async Task<ProductDto> UpdateProductAsync(Guid id, UpdateProductDto updateProductDto, Guid userId)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null)
        {
            throw new KeyNotFoundException("Producto no encontrado.");
        }

        // Verificar que el usuario es el propietario
        if (product.UserId != userId)
        {
            throw new UnauthorizedAccessException("No tienes permisos para editar este producto.");
        }

        _mapper.Map(updateProductDto, product);

        // Actualizar imágenes si se proporcionan
        if (updateProductDto.ImageUrls != null)
        {
            product.Images.Clear();
            for (int i = 0; i < updateProductDto.ImageUrls.Count; i++)
            {
                product.Images.Add(new ProductImage
                {
                    ImageUrl = updateProductDto.ImageUrls[i],
                    SortOrder = i
                });
            }
        }

        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();

        // Retornar producto actualizado
        var updatedProduct = await _unitOfWork.Products.GetByIdAsync(id);
        return _mapper.Map<ProductDto>(updatedProduct!);
    }

    public async Task<bool> DeleteProductAsync(Guid id, Guid userId)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null || product.UserId != userId)
        {
            return false;
        }

        _unitOfWork.Products.Delete(product);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    //public async Task<bool> ToggleProductStatusAsync(Guid id, Guid userId, Domain.Enums.ProductStatus newStatus)
    //{
    //    var product = await _unitOfWork.Products.GetByIdAsync(id);
    //    if (product == null || product.UserId != userId)
    //    {
    //        return false;
    //    }

    //    product.Status = newStatus;
    //    _unitOfWork.Products.Update(product);
    //    await _unitOfWork.SaveChangesAsync();

    //    return true;
    //}
    public async Task<bool> ToggleProductStatusAsync(Guid id, Guid userId, ProductStatus newStatus)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if (product == null || product.UserId != userId)
        {
            return false;
        }

        product.Status = newStatus;
        _unitOfWork.Products.Update(product);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}