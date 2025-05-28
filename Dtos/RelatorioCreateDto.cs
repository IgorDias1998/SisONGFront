namespace SisONGFront.Dtos
{
    public class RelatorioCreateDto
    {
        public string Tipo { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
