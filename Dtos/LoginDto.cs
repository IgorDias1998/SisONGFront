using System.ComponentModel.DataAnnotations;

namespace SisONGFront.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "O Email é obrigatório.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Senha é obrigatória.")]
        public string Senha { get; set; } = string.Empty;
    }
}
