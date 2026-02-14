namespace WebApi.DTOs.Jinetes
{
    public class JineteDto
    {
        public long Id { get; set; }
        public string Nombre { get; set; } = null!;
        public decimal AlturaCm { get; set; }
        public decimal PesoKg { get; set; }
        public decimal LargoPierna { get; set; }
        public decimal AnchoCadera { get; set; }
        public string Nivel { get; set; } = null!;
        public string Disciplina { get; set; } = null!;
        public long UserId { get; set; }
    }
}
