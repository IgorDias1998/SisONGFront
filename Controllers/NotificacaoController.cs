using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SisONGFront.Dtos;
using SisONGFront.Models;

public class NotificacaoController : Controller
{
    private readonly HttpClient _httpClient;

    public NotificacaoController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("SisONGApi");
    }

    [HttpGet]
    public IActionResult Enviar()
    {
        return View(new NotificacaoGrupoDto());
    }

    [HttpPost]
    public async Task<IActionResult> Enviar(NotificacaoGrupoDto dto)
    {
        if (!ModelState.IsValid)
            return View(dto);

        dto.DataEnvio = DateTime.Now;

        var json = JsonSerializer.Serialize(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("https://localhost:7090/api/Notificacao/enviar", content);

        if (!response.IsSuccessStatusCode)
        {
            ModelState.AddModelError("", "Erro ao enviar a notificação.");
            return View(dto);
        }

        TempData["Sucesso"] = "Notificações enviadas com sucesso!";
        return RedirectToAction("Enviar");
    }


    private async Task<List<SelectListItem>> ObterUsuariosAsync()
    {
        var resposta = await _httpClient.GetAsync("/api/Usuario");
        if (!resposta.IsSuccessStatusCode)
            return new List<SelectListItem>();

        var json = await resposta.Content.ReadAsStringAsync();
        var usuarios = JsonSerializer.Deserialize<List<UsuarioDto>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return usuarios.Select(u => new SelectListItem
        {
            Value = u.Id.ToString(),
            Text = u.Nome
        }).ToList();
    }

    [HttpGet]
    public async Task<IActionResult> BuscarUsuariosPorTipo(string tipo)
    {
        var resposta = await _httpClient.GetAsync($"/api/Usuario?tipo={tipo}");
        if (!resposta.IsSuccessStatusCode)
            return BadRequest();

        var json = await resposta.Content.ReadAsStringAsync();
        var usuarios = JsonSerializer.Deserialize<List<UsuarioDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var itens = usuarios.Select(u => new SelectListItem
        {
            Value = u.Id.ToString(),
            Text = u.Nome
        }).ToList();

        return Json(itens);
    }

}
