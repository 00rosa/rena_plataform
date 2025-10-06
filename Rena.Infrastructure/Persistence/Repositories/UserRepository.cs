using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::Rena.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Rena.Domain.Entities;
using Rena.Domain.Interfaces;
using Rena.Infrastructure.Data;

namespace Rena.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .Where(u => u.IsActive)
            .OrderBy(u => u.Name)
            .ToListAsync();
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
    }

    public void Delete(User user)
    {
        // Soft delete
        user.IsActive = false;
        _context.Users.Update(user);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email && u.IsActive);
    }

    public async Task<bool> ValidateCredentialsAsync(string email, string password)
    {
        // En una implementación real, aquí verificarías el hash de la contraseña
        var user = await GetByEmailAsync(email);
        return user != null; // Simplificado - en producción usar hashing
    }
}