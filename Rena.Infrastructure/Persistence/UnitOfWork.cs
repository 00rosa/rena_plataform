using Rena.Domain.Interfaces;
using Rena.Infrastructure.Data;
using Rena.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Rena.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;
    private IDbContextTransaction _transaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        
        // Inicializar repositorios
        Products = new ProductRepository(_context);
        Users = new UserRepository(_context);
        Categories = new CategoryRepository(_context);
        //Messages = new MessageRepository(_context);
    }

    public IProductRepository Products { get; }
    public IUserRepository Users { get; }
    public ICategoryRepository Categories { get; }
    //public IMessageRepository Messages { get; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            await _transaction.CommitAsync();
        }
        catch
        {
            await RollbackTransactionAsync();
            throw;
        }
        finally
        {
            _transaction?.Dispose();
        }
    }

    public async Task RollbackTransactionAsync()
    {
        await _transaction?.RollbackAsync();
        _transaction?.Dispose();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
    }
}