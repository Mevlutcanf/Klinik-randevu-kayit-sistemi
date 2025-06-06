using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using randevu_kayit.Models;
using randevu_kayit.ViewModels;
using System.Linq;

namespace randevu_kayit.Controllers
{
    [Authorize]
    public class RandevuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RandevuController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var appointments = await _context.Randevular
                .Include(r => r.Doktor)
                .ThenInclude(d => d.Department)
                .Where(r => r.HastaId == currentUser.Id)
                .OrderByDescending(r => r.RandevuTarihi)
                .ToListAsync();

            return View(appointments);
        }

        public async Task<IActionResult> Create()
        {
            // Get all departments
            var departments = await _context.Departments
                .OrderBy(d => d.Name)
                .ToListAsync();
            ViewBag.Bolumler = new SelectList(departments, "Name", "Name");

            // Get all doctors with their departments for initial load
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            var doctorsQuery = from d in doctors
                             join dept in _context.Departments
                             on d.DepartmentId equals dept.Id
                             select new
                             {
                                 Id = d.Id,
                                 AdSoyad = d.AdSoyad,
                                 Bolum = dept.Name,
                                 Uzmanlik = d.Uzmanlik
                             };

            ViewBag.Doktorlar = new SelectList(doctorsQuery, "Id", "AdSoyad");

            var model = new RandevuViewModel
            {
                Bolum = string.Empty,
                DoktorId = string.Empty,
                RandevuTarihi = DateTime.Now,
                Sikayet = string.Empty
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RandevuViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var departments = await _context.Departments.ToListAsync();
                ViewBag.Bolumler = new SelectList(departments, "Name", "Name");
                
                var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
                ViewBag.Doktorlar = doctors
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id,
                        Text = d.AdSoyad,
                        Group = new SelectListGroup { Name = d.Department?.Name ?? "Diğer" }
                    })
                    .ToList();
                
                return View(model);
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            // Randevu çakışması kontrolü
            var existingAppointment = await _context.Randevular
                .AnyAsync(r => r.DoktorId == model.DoktorId && 
                              r.RandevuTarihi == model.RandevuTarihi &&
                              r.Durum != RandevuDurumu.Iptal);

            if (existingAppointment)
            {
                ModelState.AddModelError("", "Seçilen tarih ve saatte doktor müsait değil.");
                var departments = await _context.Departments.ToListAsync();
                ViewBag.Bolumler = new SelectList(departments, "Name", "Name");
                
                var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
                ViewBag.Doktorlar = doctors
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id,
                        Text = d.AdSoyad,
                        Group = new SelectListGroup { Name = d.Department?.Name ?? "Diğer" }
                    })
                    .ToList();
                
                return View(model);
            }

            var randevu = new Randevu
            {
                HastaId = currentUser.Id,
                DoktorId = model.DoktorId,
                RandevuTarihi = model.RandevuTarihi,
                Sikayet = model.Sikayet,
                Durum = RandevuDurumu.Aktif,
                OlusturmaTarihi = DateTime.Now
            };

            _context.Randevular.Add(randevu);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Randevunuz başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var appointment = await _context.Randevular.FindAsync(id);
            if (appointment == null || appointment.HastaId != currentUser.Id)
                return NotFound();

            if (appointment.Durum != RandevuDurumu.Aktif)
                return BadRequest("Bu randevu iptal edilemez.");

            appointment.Durum = RandevuDurumu.Iptal;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetDoctorsByDepartment(string department)
        {
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            var departmentInfo = await _context.Departments
                .FirstOrDefaultAsync(d => d.Name == department);

            if (departmentInfo == null)
            {
                return Json(new List<object>());
            }

            var departmentDoctors = doctors
                .Where(d => d.DepartmentId == departmentInfo.Id)
                .Select(d => new
                {
                    id = d.Id,
                    adSoyad = d.AdSoyad,
                    uzmanlik = d.Uzmanlik,
                    bolum = department
                })
                .OrderBy(d => d.adSoyad)
                .ToList();

            return Json(departmentDoctors);
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableHours(string doktorId, DateTime date)
        {
            var bookedHours = await _context.Randevular
                .Where(r => r.DoktorId == doktorId && 
                           r.RandevuTarihi.Date == date.Date &&
                           r.Durum != RandevuDurumu.Iptal)
                .Select(r => new { hour = r.RandevuTarihi.Hour, minute = r.RandevuTarihi.Minute })
                .ToListAsync();

            var availableHours = new List<object>();
            for (int hour = 9; hour <= 17; hour++)
            {
                if (!bookedHours.Any(h => h.hour == hour && h.minute == 0))
                {
                    availableHours.Add(new { hours = hour, minutes = 0 });
                }
                if (hour != 17 && !bookedHours.Any(h => h.hour == hour && h.minute == 30))
                {
                    availableHours.Add(new { hours = hour, minutes = 30 });
                }
            }

            return Json(availableHours);
        }
    }
} 