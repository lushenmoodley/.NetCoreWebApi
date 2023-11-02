using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MagicVilla_Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto obj = new();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>  Login(LoginRequestDto obj)
        {
            APIResponse response=await _authService.LoginAsync<APIResponse>(obj);   
            if(response!=null && response.IsSuccess)
            {
                LoginResponseDTO model=JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(model.Token);

                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

                identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(x => x.Type == "unique_name").Value));
                identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(x=>x.Type=="role").Value));

                var principle = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);

                HttpContext.Session.SetString(SD.SessionToken,model.Token);
                return RedirectToAction("Index","Home");
            }
            else
            {
                TempData["error"] = response.ErrorMessages.FirstOrDefault();
                
                return View(obj);
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterationRequestDTO obj = new();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterationRequestDTO obj)
        {

            APIResponse result=await _authService.RegisterAsync<APIResponse>(obj);

            if (result != null && result.IsSuccess)
            {
                return RedirectToAction("Login");
            }

            if (result.ErrorMessages.Count>0)
            {
                TempData["error"] = result.ErrorMessages.First();
            }

           


            return View();
        }

        public async Task<IActionResult> Logout(RegisterationRequestDTO obj)
        {

            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionToken,"");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}

