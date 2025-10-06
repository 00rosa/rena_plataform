using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rena.Application.DTOs.Users;
using Rena.Domain.Enums;

namespace Rena.Application.DTOs.Products;

public class ProductDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Size { get; set; } = string.Empty;
    public ProductCondition Condition { get; set; }
    public ProductStatus Status { get; set; }
    public bool ForSale { get; set; }
    public bool ForRent { get; set; }
    public DateTime CreatedAt { get; set; }

    // Relaciones
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;

    // Imágenes
    public List<string> ImageUrls { get; set; } = new();
}