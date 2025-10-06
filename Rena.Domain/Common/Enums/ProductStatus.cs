using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rena.Domain.Enums;

public enum ProductStatus
{
    Available = 1,  // Disponible
    Reserved = 2,   // Reservado
    Sold = 3,       // Vendido
    Rented = 4,     // Rentado
    Inactive = 5    // Inactivo
}
