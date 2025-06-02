using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SisONGFront.Dtos;
using SisONGFront.Models;

namespace SisONGFront.Controllers
{
    public class VoluntarioController : Controller
    {
        private readonly HttpClient _httpClient;

        public VoluntarioController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SisONGApi");
        }

        public async Task<IActionResult> Index()
        {
            var voluntarioIdClaim = User.FindFirst("UsuarioId")?.Value;

            if (string.IsNullOrEmpty(voluntarioIdClaim))
                return RedirectToAction("Index", "Login");

            int voluntarioId = int.Parse(voluntarioIdClaim);

            // Pega todos os eventos
            var eventosResponse = await _httpClient.GetFromJsonAsync<List<EventoDto>>("/api/Evento");
            var participacoesResponse = await _httpClient.GetFromJsonAsync<List<EventoVoluntarioDto>>($"/api/EventoVoluntario/voluntario/{voluntarioId}");

            var eventosParticipadosIds = participacoesResponse.Select(p => p.EventoId).ToList();
            var eventosParticipados = eventosResponse.Where(e => eventosParticipadosIds.Contains(e.Id)).ToList();
            var eventosFuturos = eventosResponse.Where(e => e.DataHora > DateTime.Now).ToList();

            var model = new PainelVoluntarioViewModel
            {
                EventosFuturos = eventosFuturos,
                HistoricoParticipacao = eventosParticipados
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult CadastrarVoluntario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarVoluntario(VoluntarioCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var response = await _httpClient.PostAsJsonAsync("/api/Voluntario", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["MensagemSucesso"] = "Voluntário cadastrado com sucesso!";
                return RedirectToAction("Index", "Login");
            }

            ModelState.AddModelError("", "Erro ao cadastrar voluntário.");
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Participar(int eventoId)
        {
            var voluntarioIdClaim = User.FindFirst("UsuarioId")?.Value;
            if (string.IsNullOrEmpty(voluntarioIdClaim))
                return RedirectToAction("Index", "Login");

            int voluntarioId = int.Parse(voluntarioIdClaim);

            var payload = new
            {
                EventoId = eventoId,
                VoluntarioId = voluntarioId
            };

            var response = await _httpClient.PostAsJsonAsync("/api/EventoVoluntario", payload);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Erro"] = "Erro ao registrar participação.";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CancelarParticipacao(int eventoId)
        {
            var voluntarioIdClaim = User.FindFirst("UsuarioId")?.Value;
            if (string.IsNullOrEmpty(voluntarioIdClaim))
                return RedirectToAction("Index", "Login");

            int voluntarioId = int.Parse(voluntarioIdClaim);

            // Faz a requisição DELETE para cancelar a participação
            var response = await _httpClient.DeleteAsync($"/api/EventoVoluntario/{eventoId}/{voluntarioId}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Erro"] = "Erro ao cancelar a participação.";
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Painel()
        {
            var usuarioId = User.FindFirst("UsuarioId")?.Value; // Método que pega o ID do usuário logado
            var notificacoes = new List<NotificacaoDto>();

            var resposta = await _httpClient.GetAsync($"/api/Notificacao/usuario/{usuarioId}");
            if (resposta.IsSuccessStatusCode)
            {
                var json = await resposta.Content.ReadAsStringAsync();
                notificacoes = JsonSerializer.Deserialize<List<NotificacaoDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            var model = new PainelVoluntarioViewModel
            {
                Notificacoes = notificacoes,
                // outros dados do painel...
            };

            return View(model);
        }

    }
}
