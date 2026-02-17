using WebApi.DTOs.Caballos;
using WebApi.Models;

namespace WebApi.Services
{
    public interface ICaballoService
    {
        Task<IEnumerable<CaballoDto>> GetAllAsync();
        Task<CaballoDto?> GetByIdAsync(long id);
    }
}
