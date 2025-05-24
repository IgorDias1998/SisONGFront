using SisONGFront.Dtos;

namespace SisONGFront.Models
{
    public class ProdutoViewModel
    {
        public ProdutoInsumoCreateDto NovoProduto { get; set; } = new();
        public List<ProdutoInsumoReadDto> Produtos { get; set; } = new();
    }
}
