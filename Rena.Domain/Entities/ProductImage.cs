using Rena.Domain.Common;
using Rena.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ProductImage : BaseEntity
{
    public string ImageUrl { get; set; } = string.Empty;
    public int SortOrder { get; set; } = 0;

    // Foreign key
    public Guid ProductId { get; set; }

    // Navigation property
    public virtual Product Product { get; set; } = null!;
}
