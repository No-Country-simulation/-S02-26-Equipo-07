using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly NC07WebAppContext _context;

    public UserRepository(NC07WebAppContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<IEnumerable<User>> FindAsync(System.Linq.Expressions.Expression<Func<User, bool>> predicate)
    {
        return await _context.Users.Where(predicate).ToListAsync();
    }

    public async Task<User?> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<User, bool>> predicate)
    {
        return await _context.Users.FirstOrDefaultAsync(predicate);
    }

    public async Task<User> AddAsync(User entity)
    {
        entity.CreatedAt = DateTime.Now;
        entity.ModifiedAt = DateTime.Now;
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(User entity)
    {
        entity.ModifiedAt = DateTime.Now;
        _context.Users.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(User entity)
    {
        _context.Users.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(System.Linq.Expressions.Expression<Func<User, bool>> predicate)
    {
        return await _context.Users.AnyAsync(predicate);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }
}
