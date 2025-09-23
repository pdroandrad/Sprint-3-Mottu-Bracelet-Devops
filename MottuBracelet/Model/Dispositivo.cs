using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MottuBracelet.Model
{
    public class Dispositivo
    {
        public int Id { get; set; }

        [Required]
        public string StatusDispositivo { get; set; } = string.Empty;

        public int? MotoId { get; set; }
        [JsonIgnore]
        public Moto? Moto { get; set; }

        public int? PatioId { get; set; }
        [JsonIgnore]
        public Patio? Patio { get; set; }
    }
}
