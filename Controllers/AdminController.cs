using System.Net.Http;
using System.Text.Json;
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

        [HttpGet]
        public async Task<IActionResult> Voluntarios()
        {
            var voluntarios = await _httpClient.GetFromJsonAsync<List<VoluntarioReadDto>>("/api/Voluntario");
            return View(voluntarios);
        }

        [HttpGet]
        public async Task<IActionResult> Doadores()
        {
            var doadores = await _httpClient.GetFromJsonAsync<List<DoadorReadDto>>("/api/Doador");
            return View(doadores);
        }

        [HttpGet]
        public async Task<IActionResult> Eventos()
        {
            var eventos = await _httpClient.GetFromJsonAsync<List<EventoComContagemDto>>("/api/Evento/com-contagem");
            return View(eventos ?? new List<EventoComContagemDto>());
        }


        [HttpGet]
        public IActionResult CadastrarEvento()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarEvento(EventoCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var response = await _httpClient.PostAsJsonAsync("/api/Evento", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensagem"] = "Evento cadastrado com sucesso!";
                return RedirectToAction("Eventos");
            }

            ModelState.AddModelError(string.Empty, "Erro ao cadastrar evento.");
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> EditarEvento(int id)
        {
            var evento = await _httpClient.GetFromJsonAsync<EventoUpdateDto>($"/api/Evento/{id}");
            if (evento == null) return NotFound();
            return View(evento);
        }

        [HttpPost]
        public async Task<IActionResult> EditarEvento(EventoUpdateDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Evento/{dto.Id}", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensagem"] = "Evento atualizado com sucesso!";
                return RedirectToAction("Eventos");
            }

            ModelState.AddModelError(string.Empty, "Erro ao atualizar evento.");
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirEvento(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Evento/{id}");

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensagem"] = "Evento excluído com sucesso!";
            }
            else
            {
                TempData["Mensagem"] = "Erro ao excluir evento.";
            }

            return RedirectToAction("Eventos");
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

        [HttpGet]
        public async Task<IActionResult> EditarProduto(int id)
        {
            var produto = await _httpClient.GetFromJsonAsync<ProdutoInsumoReadDto>($"/api/ProdutoInsumo/{id}");
            if (produto == null)
                return NotFound();

            var dto = new ProdutoInsumoUpdateDto
            {
                Id = produto.Id,
                Nome = produto.Nome,
                QuantidadeDisponivel = produto.QuantidadeDisponivel,
                UnidadeMedida = produto.UnidadeMedida,
                Categoria = produto.Categoria
            };

            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> EditarProduto(ProdutoInsumoUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var response = await _httpClient.PutAsJsonAsync($"/api/ProdutoInsumo/{dto.Id}", dto);
            if (response.IsSuccessStatusCode)
            {
                TempData["Mensagem"] = "Produto atualizado com sucesso!";
                return RedirectToAction("Produtos");
            }

            ModelState.AddModelError(string.Empty, "Erro ao atualizar produto.");
            return View(dto);
        }

        [HttpPost]
        public async Task<IActionResult> ExcluirProduto(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/ProdutoInsumo/{id}");
            if (response.IsSuccessStatusCode)
            {
                TempData["Mensagem"] = "Produto excluído com sucesso!";
            }
            else
            {
                TempData["Mensagem"] = "Erro ao excluir o produto.";
            }

            return RedirectToAction("Produtos");
        }

        [HttpGet]
        public async Task<IActionResult> Relatorios(int page = 1)
        {
            var response = await _httpClient.GetAsync($"/api/Relatorio/paginado?page={page}&pageSize=5");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Mensagem"] = "Erro ao carregar relatórios.";
                return View(new RelatorioPaginadoViewModel());
            }

            var json = await response.Content.ReadAsStringAsync();

            var resultado = JsonSerializer.Deserialize<RelatorioPaginadoViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(resultado);
        }


        [HttpGet]
        public IActionResult CadastrarRelatorio()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarRelatorio(RelatorioCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var response = await _httpClient.PostAsJsonAsync("/api/Relatorio", dto);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensagem"] = "Relatório gerado com sucesso!";
                return RedirectToAction("Relatorios");
            }

            ModelState.AddModelError(string.Empty, "Erro ao gerar relatório.");
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> VisualizarRelatorio(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Relatorio/{id}/pdf");

            if (!response.IsSuccessStatusCode)
            {
                TempData["Mensagem"] = "Erro ao carregar o relatório em PDF.";
                return RedirectToAction("Relatorios");
            }

            var content = await response.Content.ReadAsByteArrayAsync();
            return File(content, "application/pdf");
        }

        [HttpGet]
        public async Task<IActionResult> DownloadRelatorio(int id)
        {
            var response = await _httpClient.GetAsync($"/api/Relatorio/{id}/pdf");
            if (!response.IsSuccessStatusCode)
                return NotFound();

            var bytes = await response.Content.ReadAsByteArrayAsync();
            return File(bytes, "application/pdf", $"relatorio_{id}.pdf");
        }

        [HttpGet]
        public async Task<IActionResult> PontoColeta()
        {
            var response = await _httpClient.GetFromJsonAsync<List<PontoColetaReadDto>>("/api/PontoColeta");
            return View(response);
        }

        public IActionResult CadastrarPontoColeta()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarPontoColeta(PontoColetaCreateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var response = await _httpClient.PostAsJsonAsync("/api/PontoColeta", dto);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("PontoColeta");

            TempData["Erro"] = "Erro ao cadastrar ponto de coleta.";
            return View(dto);
        }

        public async Task<IActionResult> EditarPontoColeta(int id)
        {
            var ponto = await _httpClient.GetFromJsonAsync<PontoColetaReadDto>($"/api/PontoColeta/{id}");
            if (ponto == null) return NotFound();

            var updateDto = new PontoColetaUpdateDto
            {
                NomeLocal = ponto.NomeLocal,
                Endereco = ponto.Endereco
            };

            return View(updateDto);
        }

        [HttpPost]
        public async Task<IActionResult> EditarPontoColeta(PontoColetaUpdateDto dto)
        {
            if (!ModelState.IsValid) return View(dto);

            var response = await _httpClient.PutAsJsonAsync($"/api/PontoColeta/{dto.Id}", dto);
            if (response.IsSuccessStatusCode)
                return RedirectToAction("PontoColeta");

            TempData["Erro"] = "Erro ao editar ponto de coleta.";
            return View(dto);
        }

        public async Task<IActionResult> ExcluirPontoColeta(int id)
        {
            var response = await _httpClient.DeleteAsync($"/api/PontoColeta/{id}");
            return RedirectToAction("PontoColeta");
        }
    }
}
