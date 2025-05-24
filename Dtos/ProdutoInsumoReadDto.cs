namespace SisONGFront.Dtos
{
    public class ProdutoInsumoReadDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int QuantidadeDisponivel { get; set; }
        public string UnidadeMedida { get; set; }
        public string Categoria { get; set; }
    }
}
