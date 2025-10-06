using Rena.Domain.Common;
using Rena.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rena.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Size { get; set; } = string.Empty;
        public ProductCondition Condition { get; set; }
        public ProductStatus Status { get; set; } = ProductStatus.Available;
        public bool ForSale { get; set; } = true;
        public bool ForRent { get; set; } = false;

        // Foreign keys
        public Guid CategoryId { get; set; }
        public Guid UserId { get; set; }

        // Navigation properties
        public virtual Category Category { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}

