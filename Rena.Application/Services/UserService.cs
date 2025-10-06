using AutoMapper;
using Rena.Application.DTOs.Users;
using Rena.Application.Interfaces;
using Rena.Domain.Entities;
using Rena.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Rena.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UserService> _logger;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UserService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserDto?> GetUserByIdAsync(Guid id)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> RegisterUserAsync(RegisterDto registerDto)
    {
        // Verificar si el email ya existe
        if (await _unitOfWork.Users.EmailExistsAsync(registerDto.Email))
        {
            throw new InvalidOperationException("El email ya está registrado.");
        }

        // Crear usuario
        var user = _mapper.Map<User>(registerDto);

        // En producción, aquí hashearías la contraseña
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password);

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Usuario registrado: {Email}", registerDto.Email);

        return _mapper.Map<UserDto>(user);
    }

    public async Task<(bool success, UserDto? user, string token)> LoginAsync(LoginDto loginDto)
    {
        var user = await _unitOfWork.Users.GetByEmailAsync(loginDto.Email);

        if (user == null)
        {
            return (false, null, string.Empty);
        }

        // En producción, verificar hash de contraseña
        bool passwordValid = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash);

        if (!passwordValid)
        {
            return (false, null, string.Empty);
        }

        // Actualizar último login
        user.LastLogin = DateTime.UtcNow;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        // En producción, generar JWT token real
        var token = GenerateJwtToken(user);

        return (true, _mapper.Map<UserDto>(user), token);
    }

    public async Task<UserDto> UpdateUserProfileAsync(Guid id, UpdateUserDto updateUserDto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("Usuario no encontrado.");
        }

        _mapper.Map(updateUserDto, user);
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<UserDto>(user);
    }

    public async Task<bool> ChangePasswordAsync(Guid userId, ChangePasswordDto changePasswordDto)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        // Verificar contraseña actual
        bool currentPasswordValid = BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, user.PasswordHash);
        if (!currentPasswordValid)
        {
            return false;
        }

        // Actualizar contraseña
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        var user = await _unitOfWork.Users.GetByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        _unitOfWork.Users.Delete(user);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    private string GenerateJwtToken(User user)
    {
        // En producción, implementar generación real de JWT
        // Por ahora retornar un token simulado
        return $"simulated-jwt-token-for-{user.Id}";
    }
}