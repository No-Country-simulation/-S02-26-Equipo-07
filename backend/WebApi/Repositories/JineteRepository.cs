using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class JineteRepository : IJineteRepository
    {
        private readonly NC07WebAppContext _context;

        public JineteRepository(NC07WebAppContext context)
        {
            _context = context;
        }

        public async Task<Jinete?> GetByIdAsync(long id)
        {
            return await _context.Jinetes.FindAsync(id);
        }
        public async Task<IEnumerable<Jinete>> GetAllAsync()
        {
            return await _context.Jinetes.ToListAsync();
        }

        public async Task<IEnumerable<Jinete>> FindAsync(System.Linq.Expressions.Expression<Func<Jinete, bool>> predicate)
        {
            return await _context.Jinetes.Where(predicate).ToListAsync();
        }

        public async Task<Jinete?> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<Jinete, bool>> predicate)
        {
            return await _context.Jinetes.FirstOrDefaultAsync(predicate);
        }

        public async Task<Jinete> AddAsync(Jinete entity)
        {
            await _context.Jinetes.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Jinete entity)
        {
            _context.Jinetes.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Jinete entity)
        {
            _context.Jinetes.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(System.Linq.Expressions.Expression<Func<Jinete, bool>> predicate)
        {
            return await _context.Jinetes.AnyAsync(predicate);
        }
    }
}
