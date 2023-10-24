using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    public class AuthController : Controller
    {

        private readonly IAuthService _authService;

        public AuthController(IAuth)

        public IActionResult Index()
        {
            return View();
        }
    }
}
