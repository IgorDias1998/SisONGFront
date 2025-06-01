using System.ComponentModel.DataAnnotations;

namespace SisONGFront.Dtos
{
    public class PontoColetaCreateDto
    {
        [Required]
        [StringLength(100)]
        public string NomeLocal { get; set; }

        [Required]
        [StringLength(200)]
        public string Endereco { get; set; }
    }
}
