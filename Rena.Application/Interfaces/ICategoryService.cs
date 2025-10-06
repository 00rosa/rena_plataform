using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rena.Application.DTOs.Categories;

namespace Rena.Application.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto?> GetCategoryByIdAsync(Guid id);
    Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto);
    Task<CategoryDto> UpdateCategoryAsync(Guid id, UpdateCategoryDto updateCategoryDto);
    Task<bool> DeleteCategoryAsync(Guid id);
}