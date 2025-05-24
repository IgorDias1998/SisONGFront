namespace SisONGFront.Dtos
{
    public class DoadorReadDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Cpf { get; set; }
        public string Endereco { get; set; }

        public string Tipo { get; set; } = "Doador";
    }
}
