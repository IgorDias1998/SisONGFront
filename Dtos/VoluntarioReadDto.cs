namespace SisONGFront.Dtos
{
    public class VoluntarioReadDto
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Telefone { get; set; }

        public string Habilidades { get; set; }

        public string Disponibilidade { get; set; }

        public string HistoricoParticipacao { get; set; }

        public string Tipo { get; set; } = "Voluntario";
    }
}
