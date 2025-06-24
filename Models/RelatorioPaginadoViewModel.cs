using SisONGFront.Dtos;

namespace SisONGFront.Models
{
    public class RelatorioPaginadoViewModel
    {
        public int Total { get; set; }
        public int PaginaAtual { get; set; }
        public int TotalPaginas { get; set; }
        public List<RelatorioReadDto> Itens { get; set; } = new();
    }
}