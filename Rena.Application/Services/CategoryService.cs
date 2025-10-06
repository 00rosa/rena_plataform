using AutoMapper;
using Rena.Application.DTOs.Categories;
using Rena.Application.Interfaces;
using Rena.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Rena.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CategoryService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        return category == null ? null : _mapper.Map<CategoryDto>(category);
    }

    public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        return _mapper.Map<IEnumerable<CategoryDto>>(categories);
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
    {
        // Verificar si el nombre ya existe
        if (await _unitOfWork.Categories.NameExistsAsync(createCategoryDto.Name))
        {
            throw new InvalidOperationException("Ya existe una categoría con ese nombre.");
        }

        var category = _mapper.Map<Domain.Entities.Category>(createCategoryDto);

        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Categoría creada: {Name}", createCategoryDto.Name);

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<CategoryDto> UpdateCategoryAsync(Guid id, UpdateCategoryDto updateCategoryDto)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null)
        {
            throw new KeyNotFoundException("Categoría no encontrada.");
        }

        _mapper.Map(updateCategoryDto, category);
        _unitOfWork.Categories.Update(category);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CategoryDto>(category);
    }

    public async Task<bool> DeleteCategoryAsync(Guid id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null)
        {
            return false;
        }

        _unitOfWork.Categories.Delete(category);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}