namespace WebApi.DTOs.Caballos
{
    public class CaballoDto
    {
        public long Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Raza { get; set; } = null!;
        public decimal AlturaCm { get; set; }
        public string TipoTorso { get; set; } = null!;
        public decimal Anchura { get; set; }
        public short Edad { get; set; }
        public string Musculatura { get; set; } = null!;
        public long UserId { get; set; }
    }
}
