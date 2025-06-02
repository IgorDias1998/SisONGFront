using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using SisONGFront.Dtos;

namespace SisONGFront.Models;
public class NotificacaoFormViewModel
{
    [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
    public string TipoUsuario { get; set; }

    [Required(ErrorMessage = "O tipo de notificação é obrigatório.")]
    public string Tipo { get; set; }

    [Required(ErrorMessage = "A mensagem é obrigatória.")]
    public string Mensagem { get; set; }

    public List<SelectListItem> TiposUsuario { get; set; }
    public List<SelectListItem> TiposNotificacao { get; set; }
}
