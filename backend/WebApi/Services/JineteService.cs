using WebApi.DTOs.Jinetes;
using WebApi.DTOs.User;
using WebApi.Models;
using WebApi.Repositories;

namespace WebApi.Services
{
    public class JineteService : IJineteService
    {
        private readonly IJineteRepository _jineteRepository;

        public JineteService(IJineteRepository jineteRepository)
        {
            _jineteRepository = jineteRepository;
        }
        public async Task<IEnumerable<JineteDto>> GetAllAsync()
        {
            var jinetes = await _jineteRepository.GetAllAsync();
            return jinetes.Select(MapToDto);
        }

        public async Task<JineteDto?> GetByIdAsync(long id)
        {
            var jinetes = await _jineteRepository.GetByIdAsync(id);
            return jinetes == null ? null : MapToDto(jinetes);
        }

        private static JineteDto MapToDto(Jinete jinete)
        {
            return new JineteDto
            {
                Id = jinete.Id,
                Nombre = jinete.Nombre,
                AlturaCm = jinete.AlturaCm,
                PesoKg = jinete.PesoKg,
                LargoPierna = jinete.LargoPierna,
                AnchoCadera = jinete.AnchoCadera,
                Nivel = jinete.Nivel,
                Disciplina = jinete.Disciplina,
                UserId = jinete.UserId,
            };
        }
    }
}
