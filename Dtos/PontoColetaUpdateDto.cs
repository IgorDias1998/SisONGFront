using System.ComponentModel.DataAnnotations;

namespace SisONGFront.Dtos
{
    public class PontoColetaUpdateDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string NomeLocal { get; set; }

        [Required]
        [StringLength(200)]
        public string Endereco { get; set; }
    }
}
