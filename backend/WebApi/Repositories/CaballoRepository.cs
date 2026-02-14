using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class CaballoRepository : ICaballoRepository
    {
        private readonly NC07WebAppContext _context;

        public CaballoRepository(NC07WebAppContext context)
        {
            _context = context;
        }

        public async Task<Caballo?> GetByIdAsync(long id)
        {
            return await _context.Caballos.FindAsync(id);
        }
        public async Task<IEnumerable<Caballo>> GetAllAsync()
        {
            return await _context.Caballos.ToListAsync();
        }

        public async Task<IEnumerable<Caballo>> FindAsync(System.Linq.Expressions.Expression<Func<Caballo, bool>> predicate)
        {
            return await _context.Caballos.Where(predicate).ToListAsync();
        }

        public async Task<Caballo?> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<Caballo, bool>> predicate)
        {
            return await _context.Caballos.FirstOrDefaultAsync(predicate);
        }

        public async Task<Caballo> AddAsync(Caballo entity)
        {
         //   entity.CreatedAt = DateTime.Now;
         //   entity.ModifiedAt = DateTime.Now;
            await _context.Caballos.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Caballo entity)
        {
          //  entity.ModifiedAt = DateTime.Now;
            _context.Caballos.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Caballo entity)
        {
            _context.Caballos.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(System.Linq.Expressions.Expression<Func<Caballo, bool>> predicate)
        {
            return await _context.Caballos.AnyAsync(predicate);
        }

    }
}
