// Controllers/AccountController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using randevu_kayit.Models;
using randevu_kayit.ViewModels;

namespace randevu_kayit.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
                    return View(model);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    AdSoyad = $"{model.Ad} {model.Soyad}",
                    TCKimlikNo = model.TCKimlikNo,
                    DogumTarihi = model.DogumTarihi,
                    PhoneNumber = model.PhoneNumber,
                    KanGrubu = model.KanGrubu,
                    IsPatient = true
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Patient");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToLocal(returnUrl);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var nameParts = user.AdSoyad.Split(' ', 2);
            var model = new ProfileViewModel
            {
                Ad = nameParts[0],
                Soyad = nameParts.Length > 1 ? nameParts[1] : "",
                Email = user.Email!,
                TCKimlikNo = user.TCKimlikNo ?? "",
                DogumTarihi = user.DogumTarihi ?? DateTime.Now,
                PhoneNumber = user.PhoneNumber ?? "",
                KanGrubu = user.KanGrubu,
                Uzmanlik = user.Uzmanlik,
                DepartmentId = user.DepartmentId
            };

            if (User.IsInRole("Doctor"))
            {
                ViewBag.Departments = await _context.Departments.ToListAsync();
            }

            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (User.IsInRole("Doctor"))
                {
                    ViewBag.Departments = await _context.Departments.ToListAsync();
                }
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.AdSoyad = $"{model.Ad} {model.Soyad}";
            user.TCKimlikNo = model.TCKimlikNo;
            user.DogumTarihi = model.DogumTarihi;
            user.PhoneNumber = model.PhoneNumber;
            user.KanGrubu = model.KanGrubu;

            if (User.IsInRole("Doctor"))
            {
                user.Uzmanlik = model.Uzmanlik;
                user.DepartmentId = model.DepartmentId;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!string.IsNullOrEmpty(model.CurrentPassword) && !string.IsNullOrEmpty(model.NewPassword))
            {
                var passwordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
                if (!passwordResult.Succeeded)
                {
                    foreach (var error in passwordResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    if (User.IsInRole("Doctor"))
                    {
                        ViewBag.Departments = await _context.Departments.ToListAsync();
                    }
                    return View(model);
                }
            }

            if (result.Succeeded)
            {
                TempData["StatusMessage"] = "Profiliniz başarıyla güncellendi.";
                return RedirectToAction(nameof(Profile));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            if (User.IsInRole("Doctor"))
            {
                ViewBag.Departments = await _context.Departments.ToListAsync();
            }
            return View(model);
        }

        private IActionResult RedirectToLocal(string? returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
