using SisONGFront.Dtos;

namespace SisONGFront.Models
{
    public class PainelVoluntarioViewModel
    {
        public List<EventoDto> EventosFuturos { get; set; } = new();
        public List<EventoDto> HistoricoParticipacao { get; set; } = new();
        public List<NotificacaoDto> Notificacoes { get; set; } = new();
    }
}
