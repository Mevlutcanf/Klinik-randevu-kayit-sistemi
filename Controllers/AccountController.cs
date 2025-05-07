using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using randevu_kayit.Data;
using System.Security.Claims;

namespace randevu_kayit.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password, string role)
        {
            if (role == "Doctor")
            {
                var doctor = _context.Doctors.FirstOrDefault(x => x.Username == username && x.Password == password);
                if (doctor != null)
                {
                    await SignInUser(username, role);
                    return RedirectToAction("Index", "Doctor");
                }
            }
            else if (role == "Patient")
            {
                var patient = _context.Patients.FirstOrDefault(x => x.Username == username && x.Password == password);
                if (patient != null)
                {
                    await SignInUser(username, role);
                    return RedirectToAction("Index", "Patient");
                }
            }
            else if (role == "Admin")
            {
                if (username == "admin" && password == "admin123")
                {
                    await SignInUser(username, role);
                    return RedirectToAction("Index", "Admin");
                }
            }

            ModelState.AddModelError("", "Geçersiz bilgiler");
            return View();
        }

        private async Task SignInUser(string username, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }
    }
}
