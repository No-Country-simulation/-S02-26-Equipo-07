using WebApi.DTOs.Caballos;
using WebApi.DTOs.Jinetes;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IJineteService
    {
        Task<IEnumerable<JineteDto>> GetAllAsync();
        Task<JineteDto?> GetByIdAsync(long id);
    }
}
