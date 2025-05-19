using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SisONGFront.Dtos;
using SisONGFront.Models;

namespace SisONGFront.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Login");
            }

            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Claim -> {claim.Type}: {claim.Value}");
            }

            var usuario = new UsuarioDto
            {
                Id = int.Parse(User.FindFirst("UsuarioId")?.Value ?? "0"),
                Nome = User.Identity?.Name ?? "",
                Email = User.FindFirst(ClaimTypes.Email)?.Value ?? "",
                Perfil = User.FindFirst(ClaimTypes.Role)?.Value ?? "",

                Habilidades = User.FindFirst("Habilidades")?.Value,
                Disponibilidade = User.FindFirst("Disponibilidade")?.Value,
                HistoricoParticipacao = User.FindFirst("HistoricoParticipacao")?.Value,
                Cpf = User.FindFirst("Cpf")?.Value,
                Endereco = User.FindFirst("Endereco")?.Value,
                Cargo = User.FindFirst("Cargo")?.Value
            };

            return View(usuario);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
