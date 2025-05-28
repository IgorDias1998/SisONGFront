using System.ComponentModel.DataAnnotations;

namespace SisONGFront.Dtos
{
    public class RelatorioUpdateDto
    {
        [Required(ErrorMessage = "O tipo do relatório é obrigatório.")]
        public string Tipo { get; set; }
    }
}
