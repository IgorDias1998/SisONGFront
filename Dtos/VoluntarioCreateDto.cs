using System.ComponentModel.DataAnnotations;

namespace SisONGFront.Dtos
{
    public class VoluntarioCreateDto
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }

        public string Telefone { get; set; }

        [Required]
        public string Habilidades { get; set; }

        [Required]
        public string Disponibilidade { get; set; }

        public string HistoricoParticipacao { get; set; } = "0";
    }
}