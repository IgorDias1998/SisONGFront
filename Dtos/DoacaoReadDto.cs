namespace SisONGFront.Dtos
{
    public class DoacaoReadDto
    {
        public int Id { get; set; }
        public int DoadorId { get; set; }
        public string Tipo { get; set; }
        public decimal? Valor { get; set; }
        public string? Item { get; set; }
        public int? Quantidade { get; set; }
        public DateTime? Data { get; set; }
    }
}
