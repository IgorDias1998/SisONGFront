using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SisONGFront.Dtos;

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
        public async Task<IActionResult> RealizarDoacao()
        {
            var dto = new DoacaoCreateDto { Data = DateTime.Now };
            await ObterPontosColetaAsync(dto);
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> RealizarDoacao(DoacaoCreateDto dto)
        {
            await ObterPontosColetaAsync(dto);

            if (!ModelState.IsValid)
                return View(dto);

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

        // Método helper para buscar pontos
        private async Task ObterPontosColetaAsync(DoacaoCreateDto dto)
        {
            var resposta = await _httpClient.GetAsync("/api/PontoColeta");
            if (!resposta.IsSuccessStatusCode)
            {
                dto.PontosColeta = new List<SelectListItem>();
                dto.PontosColetaExibir = new List<PontoColetaReadDto>();
                return;
            }

            var json = await resposta.Content.ReadAsStringAsync();
            var pontos = JsonSerializer.Deserialize<List<PontoColetaReadDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            dto.PontosColeta = pontos.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.NomeLocal
            }).ToList();

            dto.PontosColetaExibir = pontos.Select(p => new PontoColetaReadDto
            {
                Id = p.Id,
                NomeLocal = p.NomeLocal,
                Endereco = p.Endereco
            }).ToList();
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
