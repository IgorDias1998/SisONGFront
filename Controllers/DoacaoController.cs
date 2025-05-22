using Microsoft.AspNetCore.Mvc;
using SisONGFront.Dtos;
using System.Net.Http.Json;
using System.Security.Claims;

namespace SisONGFront.Controllers
{
    public class DoacaoController : Controller
    {
        private readonly HttpClient _httpClient;

        public DoacaoController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SisONGApi");
        }

        [HttpGet]
        public IActionResult RealizarDoacao()
        {
            return View(new DoacaoCreateDto { Data = DateTime.Now });
        }

        [HttpPost]
        public async Task<IActionResult> RealizarDoacao(DoacaoCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            // Pega o ID do doador logado
            var doadorIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId")?.Value;
            if (doadorIdClaim == null)
            {
                ModelState.AddModelError("", "Usuário não autenticado.");
                return View(dto);
            }

            dto.DoadorId = int.Parse(doadorIdClaim);

            var response = await _httpClient.PostAsJsonAsync("/api/Doacao", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["MensagemSucesso"] = "Doação registrada com sucesso!";
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Erro ao registrar doação.");
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Historico()
        {
            var doadorIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UsuarioId")?.Value;

            if (doadorIdClaim == null)
            {
                TempData["MensagemErro"] = "Usuário não autenticado.";
                return RedirectToAction("Index", "Home");
            }

            int doadorId = int.Parse(doadorIdClaim);

            var response = await _httpClient.GetAsync($"/api/Doacao/doador/{doadorId}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["MensagemErro"] = "Erro ao carregar doações.";
                return View(new List<DoacaoReadDto>());
            }

            var doacoes = await response.Content.ReadFromJsonAsync<List<DoacaoReadDto>>();
            return View(doacoes);
        }
    }
}
