using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Rena.Domain.Enums;

namespace Rena.Application.DTOs.Products;

public class CreateProductDto
{
    [Required]
    [StringLength(200, MinimumLength = 5)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Range(0.01, 100000)]
    public decimal Price { get; set; }

    [Required]
    public Guid CategoryId { get; set; }

    [Required]
    [StringLength(20)]
    public string Size { get; set; } = string.Empty;

    [Required]
    public ProductCondition Condition { get; set; }

    [Required]
    public bool ForSale { get; set; } = true;

    public bool ForRent { get; set; } = false;

    public List<string> ImageUrls { get; set; } = new();
}