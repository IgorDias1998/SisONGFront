using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SisONGFront.Dtos;
using SisONGFront.Models;
using QRCoder;

namespace SisONGFront.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient _httpClient;
        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
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

        public IActionResult PublicHome()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Redireciona baseado no perfil
                if (User.IsInRole("Voluntario"))
                    return RedirectToAction("Index", "Home");
                if (User.IsInRole("Doador"))
                    return RedirectToAction("Index", "Home");
                if (User.IsInRole("Administrador"))
                    return RedirectToAction("Index", "Home");
            }

            return View();
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

        [HttpGet]
        public async Task<IActionResult> PaginaAnonima()
        {
            decimal total = 0;

            try
            {
                var response = await _httpClient.GetAsync("/api/Doacao/total");

                if (response.IsSuccessStatusCode)
                {
                    total = await response.Content.ReadFromJsonAsync<decimal>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter total de doações.");
            }

            var viewModel = new DoacaoAnonimaViewModel
            {
                Doacao = new DoacaoCreateDto { Tipo = "Financeira", Data = DateTime.Now },
                TotalDoacoes = total
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> PaginaAnonima(DoacaoAnonimaViewModel viewModel)
        {
            var doacaoDto = new DoacaoCreateDto
            {
                DoadorId = 0,
                Tipo = viewModel.Doacao.Tipo,
                Valor = viewModel.Doacao.Valor,
                Data = viewModel.Doacao.Data
            };

            var response = await _httpClient.PostAsJsonAsync("/api/Doacao", doacaoDto);

            if (response.IsSuccessStatusCode)
            {
                // Simular um payload de Pix (pode ser substituído por algo real depois)
                string payloadPix = $"Pagamento Pix\nValor: R$ {doacaoDto.Valor:F2}\nChave: contato@sisong.org";

                // Gerar QR Code
                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(payloadPix, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new Base64QRCode(qrCodeData);
                string qrCodeImage = qrCode.GetGraphic(10);

                TempData["QRCodeBase64"] = qrCodeImage;

                return RedirectToAction("QrCodePix", new { valor = doacaoDto.Valor });
            }

            decimal total = 0;
            try
            {
                var totalResponse = await _httpClient.GetAsync("/api/Doacao/total");
                if (totalResponse.IsSuccessStatusCode)
                {
                    total = await totalResponse.Content.ReadFromJsonAsync<decimal>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter total de doações.");
            }

            viewModel.TotalDoacoes = total;

            return View(viewModel);
        }
        public IActionResult QrCodePix(decimal valor)
        {
            ViewBag.Valor = valor;
            ViewBag.QRCode = TempData["QRCodeBase64"]?.ToString();
            return View();
        }
    }
}
