using WebApi.DTOs.Caballos;
using WebApi.DTOs.User;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services
{
    public class CaballoService : ICaballoService
    {
        private readonly ICaballoRepository _caballoRepository;
        public CaballoService(ICaballoRepository caballoRepository)
        {
            _caballoRepository = caballoRepository;
        }
        public async Task<IEnumerable<CaballoDto>> GetAllAsync()
        {
            var caballos = await _caballoRepository.GetAllAsync();
            return caballos.Select(MapToDto);
        }

        public async Task<CaballoDto?> GetByIdAsync(long id)
        {
            var caballos = await _caballoRepository.GetByIdAsync(id);
            return caballos == null ? null : MapToDto(caballos);
        }

        
        private static CaballoDto MapToDto(Caballo caballo)
        {
            return new CaballoDto
            {
                Id = caballo.Id,
                Nombre = caballo.Nombre,
                Raza = caballo.Raza,
                AlturaCm = caballo.AlturaCm,
                TipoTorso = caballo.TipoTorso,
                Anchura = caballo.Anchura,
                Edad = caballo.Edad,
                Musculatura = caballo.Musculatura,
                UserId = caballo.UserId,
            };
        }
    }
}
