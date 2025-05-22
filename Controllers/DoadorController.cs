using Microsoft.AspNetCore.Mvc;
using SisONGFront.Dtos;
using System.Net.Http.Json;

namespace SisONGFront.Controllers
{
    public class DoadorController : Controller
    {
        private readonly HttpClient _httpClient;

        public DoadorController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SisONGApi");
        }

        [HttpGet]
        public IActionResult CadastrarDoador()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarDoador(DoadorCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var response = await _httpClient.PostAsJsonAsync("/api/Doador", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["MensagemSucesso"] = "Doador cadastrado com sucesso!";
                return RedirectToAction("Index", "Login");
            }

            ModelState.AddModelError("", "Erro ao cadastrar doador.");
            return View(dto);
        }
    }
}
