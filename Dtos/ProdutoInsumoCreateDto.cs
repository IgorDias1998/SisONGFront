namespace SisONGFront.Dtos
{
    public class ProdutoInsumoCreateDto
    {
        public string Nome { get; set; }
        public int QuantidadeDisponivel { get; set; }
        public string UnidadeMedida { get; set; }
        public string Categoria { get; set; }
    }
}
