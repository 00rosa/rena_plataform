using AutoMapper;
using Rena.Application.DTOs.Products;
using Rena.Application.DTOs.Users;
using Rena.Application.DTOs.Categories;
using Rena.Domain.Entities;

namespace Rena.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        // User Mappings
        CreateMap<User, UserDto>();
        CreateMap<RegisterDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()); // Se maneja en el servicio

        // Product Mappings
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.Images.OrderBy(i => i.SortOrder).Select(i => i.ImageUrl)));

        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.Images, opt => opt.Ignore()) // Se maneja en el servicio
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => Domain.Enums.ProductStatus.Available));

        CreateMap<UpdateProductDto, Product>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        // Category Mappings
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}