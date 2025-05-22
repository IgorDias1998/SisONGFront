using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SisONGFront.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
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
    }
}
