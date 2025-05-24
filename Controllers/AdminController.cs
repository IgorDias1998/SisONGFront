using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SisONGFront.Dtos;
using SisONGFront.Models;

namespace SisONGFront.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SisONGApi");
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var voluntarios = await _httpClient.GetFromJsonAsync<List<VoluntarioReadDto>>("/api/Voluntario");
            var doadores = await _httpClient.GetFromJsonAsync<List<DoadorReadDto>>("/api/Doador");
            var eventos = await _httpClient.GetFromJsonAsync<List<EventoReadDto>>("/api/Evento");
            var produtos = await _httpClient.GetFromJsonAsync<List<ProdutoInsumoReadDto>>("/api/ProdutoInsumo");

            ViewBag.VoluntariosCount = voluntarios?.Count ?? 0;
            ViewBag.DoadoresCount = doadores?.Count ?? 0;
            ViewBag.EventosCount = eventos?.Count ?? 0;
            ViewBag.ProdutosCount = produtos?.Count ?? 0;

            return View();
        }

        public IActionResult Voluntarios()
        {
            return View();
        }

        public IActionResult Doadores()
        {
            return View();
        }

        public IActionResult Eventos()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Produtos()
        {
            var produtos = await _httpClient.GetFromJsonAsync<List<ProdutoInsumoReadDto>>("/api/ProdutoInsumo");
            return View(produtos ?? new List<ProdutoInsumoReadDto>());
        }

        [HttpGet]
        public IActionResult CadastrarProduto()
        {
            return View(new ProdutoInsumoCreateDto());
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarProduto(ProdutoInsumoCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var response = await _httpClient.PostAsJsonAsync("/api/ProdutoInsumo", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensagem"] = "Produto cadastrado com sucesso!";
                return RedirectToAction("Produtos");
            }

            ModelState.AddModelError(string.Empty, "Erro ao cadastrar produto.");
            return View(dto);
        }
    }
}
