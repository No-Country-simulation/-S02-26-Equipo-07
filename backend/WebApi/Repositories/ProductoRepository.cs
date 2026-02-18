using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly NC07WebAppContext _context;

        public ProductoRepository(NC07WebAppContext context)
        {
            _context = context;
        }

        public async Task<Producto?> GetByIdAsync(long id)
        {
            return await _context.Productos.FindAsync(id);
        }

        public async Task<IEnumerable<Producto>> GetAllAsync()
        {
            return await _context.Productos.Include(p => p.CategoriaNavigation).ToListAsync();
        }

        public async Task<IEnumerable<Producto>> FindAsync(System.Linq.Expressions.Expression<Func<Producto, bool>> predicate)
        {
            return await _context.Productos.Where(predicate).Include(p => p.CategoriaNavigation).ToListAsync();
        }

        public async Task<Producto?> FirstOrDefaultAsync(System.Linq.Expressions.Expression<Func<Producto, bool>> predicate)
        {
            return await _context.Productos.Include(p => p.CategoriaNavigation).FirstOrDefaultAsync(predicate);
        }

        public async Task<Producto> AddAsync(Producto entity)
        {
            await _context.Productos.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Producto entity)
        {
            _context.Productos.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Producto entity)
        {
            _context.Productos.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(System.Linq.Expressions.Expression<Func<Producto, bool>> predicate)
        {
            return await _context.Productos.AnyAsync(predicate);
        }
    }
}