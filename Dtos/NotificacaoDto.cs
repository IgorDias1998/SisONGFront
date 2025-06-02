namespace SisONGFront.Dtos
{
    public class NotificacaoDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }

        public string Tipo { get; set; } = string.Empty;

        public string Mensagem { get; set; } = string.Empty;

        public DateTime DataEnvio { get; set; }
    }
}
