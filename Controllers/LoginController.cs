using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SisONGFront.Dtos;
using System.Net.Http.Json;


namespace SisONG.MVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly HttpClient _httpClient;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("SisONGApi");
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Index(LoginDto loginDto)
        {
            if (!ModelState.IsValid) return View(loginDto);

            var response = await _httpClient.PostAsJsonAsync("usuario/login", loginDto);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Email ou senha inválidos.");
                return View();
            }

            var usuario = await response.Content.ReadFromJsonAsync<UsuarioDto>();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario!.Nome),
                new Claim(ClaimTypes.Email, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.Perfil), // Usa o Tipo para Role
                new Claim("UsuarioId", usuario.Id.ToString())
            };

            // Adiciona claims extras do voluntário, se existir
            if (usuario.Perfil == "Voluntario")
            {
                if (!string.IsNullOrEmpty(usuario.Habilidades))
                    claims.Add(new Claim("Habilidades", usuario.Habilidades));
                if (!string.IsNullOrEmpty(usuario.Disponibilidade))
                    claims.Add(new Claim("Disponibilidade", usuario.Disponibilidade));
                if (!string.IsNullOrEmpty(usuario.HistoricoParticipacao))
                    claims.Add(new Claim("HistoricoParticipacao", usuario.HistoricoParticipacao));
            }
            else if (usuario.Perfil == "Doador")
            {
                if (!string.IsNullOrEmpty(usuario.Cpf))
                    claims.Add(new Claim("Cpf", usuario.Cpf));
                if (!string.IsNullOrEmpty(usuario.Endereco))
                    claims.Add(new Claim("Endereco", usuario.Endereco));
            }
            else if (usuario.Perfil == "Administrador")
            {
                if (!string.IsNullOrEmpty(usuario.Cargo))
                    claims.Add(new Claim("Cargo", usuario.Cargo));
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        public IActionResult AcessoNegado() => View();
    }
}