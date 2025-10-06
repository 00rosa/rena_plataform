using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Rena.Domain.Enums;

namespace Rena.Application.DTOs.Products;

public class UpdateProductDto
{
    [Required]
    public ProductStatus NewStatus { get; set; }

    [StringLength(200, MinimumLength = 5)]
    public string? Title { get; set; }

    [StringLength(2000, MinimumLength = 10)]
    public string? Description { get; set; }

    [Range(0.01, 100000)]
    public decimal? Price { get; set; }

    public Guid? CategoryId { get; set; }

    [StringLength(20)]
    public string? Size { get; set; }

    public ProductCondition? Condition { get; set; }

    public bool? ForSale { get; set; }

    public bool? ForRent { get; set; }

    public ProductStatus? Status { get; set; }

    public List<string>? ImageUrls { get; set; }
}