namespace SisONGFront.Dtos
{
    public class EventoUpdateDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime? DataHora { get; set; }
        public string Local { get; set; }
        public string Status { get; set; }
    }
}
