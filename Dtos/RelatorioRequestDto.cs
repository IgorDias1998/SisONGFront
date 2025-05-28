using System.ComponentModel.DataAnnotations;

namespace SisONGFront.Dtos
{
    public class RelatorioRequestDto
    {
        [Required(ErrorMessage = "O tipo do relatório é obrigatório.")]
        public string Tipo { get; set; }  // Ex: "financeiro", "voluntariado", "eventos"

        [Display(Name = "Data Início")]
        public DateTime? DataInicio { get; set; }

        [Display(Name = "Data Fim")]
        public DateTime? DataFim { get; set; }
    }
}
