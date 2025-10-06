using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rena.Application.DTOs.Users;

namespace Rena.Application.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetUserByIdAsync(Guid id);
    Task<UserDto> RegisterUserAsync(RegisterDto registerDto);
    Task<(bool success, UserDto? user, string token)> LoginAsync(LoginDto loginDto);
    Task<UserDto> UpdateUserProfileAsync(Guid id, UpdateUserDto updateUserDto);
    Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto);
    Task<bool> DeleteUserAsync(Guid userId);
}