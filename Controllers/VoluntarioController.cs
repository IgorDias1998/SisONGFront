using System.Linq;
using System.Net.Http.Json;
using System.Security.Claims;
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


    }
}
