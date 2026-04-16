using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.Services;
using TaskManagerApp.ViewModels;

namespace TaskManagerApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        // GET: /Account/Login
        public IActionResult Login() => View();

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = await _userService.LoginAsync(model.Email, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Email o contraseña incorrectos");
                return View(model);
            }

            // Guardar sesión del usuario
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.FullName);

            return RedirectToAction("Index", "Tasks");
        }

        // GET: /Account/Register
        public IActionResult Register() => View();

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var success = await _userService.RegisterAsync(
                model.FullName, model.Email, model.Password);

            if (!success)
            {
                ModelState.AddModelError("", "Ese email ya está registrado");
                return View(model);
            }

            return RedirectToAction("Login");
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
