using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SisONGFront.Dtos;

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
            var response = await _httpClient.PostAsJsonAsync("/api/Doador", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["MensagemSucesso"] = "Doador cadastrado com sucesso!";
                return RedirectToAction("Index", "Login");
            }

            else
            {
                var erroStr = await response.Content.ReadAsStringAsync();
                string mensagemErro;

                try
                {
                    var erroObj = JsonSerializer.Deserialize<Dictionary<string, string>>(erroStr);
                    mensagemErro = erroObj?.GetValueOrDefault("mensagem") ?? "Erro ao cadastrar doador.";
                }
                catch (JsonException)
                {
                    mensagemErro = erroStr;
                }

                ModelState.AddModelError("Email", mensagemErro);
                return View(dto);
            }
        }
    }
}
