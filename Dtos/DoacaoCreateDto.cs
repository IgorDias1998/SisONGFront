using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SisONGFront.Dtos
{
    public class DoacaoCreateDto
    {
        [Required]
        public int DoadorId { get; set; }

        [Required]
        public string Tipo { get; set; }

        public decimal? Valor { get; set; }

        public string? Item { get; set; }

        public int? Quantidade { get; set; }

        public DateTime? Data { get; set; }

        //Ponto coleta
        public int? PontoColetaId { get; set; }

        public List<SelectListItem> PontosColeta { get; set; } = new();
        public List<PontoColetaReadDto> PontosColetaExibir { get; set; } = new();
    }
}
