using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly NC07WebAppContext _context;

        public CategoriaRepository(NC07WebAppContext context)
        {
            _context = context;
        }

        public async Task<Categorium?> GetByIdAsync(long id)
        {
            return await _context.Categoria.FindAsync(id);
        }

        public async Task<IEnumerable<Categorium>> GetAllAsync()
        {
            return await _context.Categoria.ToListAsync();
        }

        public async Task<IEnumerable<Categorium>> FindAsync(System.Linq.Expressions.Expression<Func<Categorium, bool>> predicate)
        {
            return await _context.Categoria.Where(predicate).ToListAsync();
        }

        public async Task<Categorium?> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<Categorium, bool>> predicate)
        {
            return await _context.Categoria.FirstOrDefaultAsync(predicate);
        }

        public async Task<Categorium> AddAsync(Categorium entity)
        {
            await _context.Categoria.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Categorium entity)
        {
            _context.Categoria.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Categorium entity)
        {
            _context.Categoria.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(System.Linq.Expressions.Expression<Func<Categorium, bool>> predicate)
        {
            return await _context.Categoria.AnyAsync(predicate);
        }
    }
}