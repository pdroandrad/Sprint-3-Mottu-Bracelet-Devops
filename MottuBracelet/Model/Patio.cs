using System.ComponentModel.DataAnnotations;

namespace MottuBracelet.Model
{
    public class Patio
    {
        public int Id { get; set; }

        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        public int CapacidadeMaxima { get; set; }

        [Required]
        public string AdministradorResponsavel { get; set; } = string.Empty;

        [Required]
        public Endereco Endereco { get; set; } = new Endereco();

        // Relacionamentos
        public ICollection<Moto> Motos { get; set; } = new List<Moto>();
        public ICollection<Dispositivo> Dispositivos { get; set; } = new List<Dispositivo>();
    }
}
