using System;
using System.ComponentModel.DataAnnotations;

namespace SisONGFront.Dtos
{
    public class NotificacaoCreateDto
    {
        public string TipoUsuario { get; set; }  // "Voluntario" ou "Doador"
        public string Tipo { get; set; }         // Tipo da notificação
        public string Mensagem { get; set; }
        public DateTime DataEnvio { get; set; } = DateTime.Now;
    }
}
