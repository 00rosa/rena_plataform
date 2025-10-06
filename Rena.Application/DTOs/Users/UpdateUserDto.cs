using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Rena.Application.DTOs.Users;

public class UpdateUserDto
{
    [StringLength(100, MinimumLength = 2)]
    public string? Name { get; set; }

    [Phone]
    public string? Phone { get; set; }

    public string? AvatarUrl { get; set; }
}