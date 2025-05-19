using System.Text.Json.Serialization;

namespace SisONGFront.Dtos
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("tipo")]
        public string Perfil { get; set; } = string.Empty;

        // Campos de Voluntário
        public string? Habilidades { get; set; }
        public string? Disponibilidade { get; set; }
        public string? HistoricoParticipacao { get; set; }

        // Campos de Doador
        public string? Cpf { get; set; }
        public string? Endereco { get; set; }

        // Campos de Administrador
        public string? Cargo { get; set; }
    }
}
