namespace MottuBracelet.DTO
{
    public class PatioHateoasDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int CapacidadeMaxima { get; set; }
        public string AdministradorResponsavel { get; set; } = string.Empty;
        public EnderecoDto Endereco { get; set; } = new EnderecoDto();
        public List<LinkDto> Links { get; set; } = new();
    }
}
