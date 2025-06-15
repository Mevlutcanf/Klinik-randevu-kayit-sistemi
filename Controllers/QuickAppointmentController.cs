using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using randevu_kayit.Models;

namespace randevu_kayit.Controllers
{
    public class QuickAppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuickAppointmentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Create", "Randevu");
            }

            var departments = await _context.Departments
                .OrderBy(d => d.Name)
                .ToListAsync();
            ViewBag.Departments = departments;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _context.Departments.OrderBy(d => d.Name).ToListAsync();
            ViewBag.Departments = departments;
            return View();
        }        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuickAppointment appointment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // AJAX request için JSON response                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        var errors = ModelState.Where(x => x.Value?.Errors?.Count > 0)
                            .Select(x => new { Field = x.Key, Errors = x.Value?.Errors?.Select(e => e.ErrorMessage) ?? new List<string>() })
                            .ToList();
                        return Json(new { success = false, message = "Form bilgileri eksik veya hatalı.", errors = errors });
                    }
                    
                    var departments = await _context.Departments.OrderBy(d => d.Name).ToListAsync();
                    ViewBag.Departments = departments;
                    return View(appointment);
                }

                // Check for appointment conflicts
                var hasConflict = await _context.QuickAppointments
                    .AnyAsync(a => a.DoktorId == appointment.DoktorId &&
                                  a.TarihSaat == appointment.TarihSaat &&
                                  a.Durum != QuickAppointmentStatus.Reddedildi);

                if (hasConflict)
                {
                    // AJAX request için JSON response
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Seçilen tarih ve saatte doktor müsait değil." });
                    }
                    
                    ModelState.AddModelError("", "Seçilen tarih ve saatte doktor müsait değil.");
                    var departments = await _context.Departments.OrderBy(d => d.Name).ToListAsync();
                    ViewBag.Departments = departments;
                    return View(appointment);
                }                // Set default values
                appointment.OlusturmaTarihi = DateTime.Now;
                if (appointment.Durum == 0)
                {
                    appointment.Durum = QuickAppointmentStatus.Bekliyor;
                }

                // Set DepartmentId based on the selected doctor's department
                if (!string.IsNullOrEmpty(appointment.DoktorId))
                {
                    var doctor = await _userManager.FindByIdAsync(appointment.DoktorId);
                    if (doctor != null && doctor.DepartmentId.HasValue)
                    {
                        appointment.DepartmentId = doctor.DepartmentId.Value;
                    }
                }

                _context.QuickAppointments.Add(appointment);
                await _context.SaveChangesAsync();

                // AJAX request için JSON response
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { 
                        success = true, 
                        message = "Randevunuz başarıyla oluşturuldu.", 
                        appointmentId = appointment.Id 
                    });
                }

                return RedirectToAction(nameof(Confirmation), new { id = appointment.Id });
            }            catch (Exception ex)
            {
                // Log the error with inner exception details
                Console.WriteLine($"Error creating appointment: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner exception stack trace: {ex.InnerException.StackTrace}");
                }
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // Get detailed error message
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                
                // AJAX request için JSON response
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { 
                        success = false, 
                        message = $"Randevu oluşturulurken bir hata oluştu: {errorMessage}" 
                    });
                }
                
                ModelState.AddModelError("", $"Randevu oluşturulurken bir hata oluştu: {errorMessage}");
                var departments = await _context.Departments.OrderBy(d => d.Name).ToListAsync();
                ViewBag.Departments = departments;
                return View(appointment);
            }
        }[HttpGet]
        public async Task<IActionResult> GetDoctorsByDepartment(int departmentId)
        {
            try
            {
                var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
                var doctorsInDepartment = doctors
                    .Where(d => d.DepartmentId == departmentId)
                    .Select(d => new { 
                        id = d.Id, 
                        adSoyad = d.AdSoyad ?? "İsimsiz Doktor",
                        uzmanlik = d.Uzmanlik ?? "Uzman Doktor"
                    })
                    .ToList();

                return Json(doctorsInDepartment);
            }
            catch
            {
                // Log the error (add proper logging here)
                return Json(new List<object>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableHours(string doctorId, DateTime date)
        {
            // Get doctor's schedule
            var schedule = await _context.DoctorSchedules
                .FirstOrDefaultAsync(s => s.DoktorId == doctorId && s.Tarih.Date == date.Date);

            if (schedule == null)
                return Json(new List<object>());

            // Get booked appointments
            var bookedHours = await _context.QuickAppointments
                .Where(a => a.DoktorId == doctorId && 
                           a.TarihSaat.Date == date.Date &&
                           a.Durum != QuickAppointmentStatus.Reddedildi)
                .Select(a => new { hour = a.TarihSaat.Hour, minute = a.TarihSaat.Minute })
                .ToListAsync();

            // Generate available time slots
            var availableHours = new List<object>();
            var startTime = schedule.BaslangicSaat;
            var endTime = schedule.BitisSaat;

            for (var time = startTime; time < endTime; time = time.Add(TimeSpan.FromMinutes(30)))
            {
                if (!bookedHours.Any(h => h.hour == time.Hours && h.minute == time.Minutes))
                {
                    availableHours.Add(new
                    {
                        hours = time.Hours,
                        minutes = time.Minutes
                    });
                }
            }

            return Json(availableHours);
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Accept(int id)
        {
            var appointment = await _context.QuickAppointments.FindAsync(id);
            if (appointment == null)
                return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || appointment.DoktorId != currentUser.Id)
                return Forbid();

            // Hızlı randevuyu normal randevuya çevir
            var randevu = new Randevu
            {
                DoktorId = appointment.DoktorId,
                HastaId = null, // Hasta henüz kayıtlı değil
                RandevuTarihi = appointment.TarihSaat,
                Sikayet = appointment.Sikayet,
                Durum = RandevuDurumu.Aktif,
                OlusturmaTarihi = DateTime.Now
            };

            _context.Randevular.Add(randevu);
            appointment.Durum = QuickAppointmentStatus.Onaylandi;
            await _context.SaveChangesAsync();

            // TODO: SMS/Email bildirimi gönder

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Reject(int id)
        {
            var appointment = await _context.QuickAppointments.FindAsync(id);
            if (appointment == null)
                return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || appointment.DoktorId != currentUser.Id)
                return Forbid();

            appointment.Durum = QuickAppointmentStatus.Reddedildi;
            await _context.SaveChangesAsync();

            // TODO: SMS/Email bildirimi gönder

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Confirmation(int id)
        {
            var appointment = await _context.QuickAppointments
                .Include(a => a.Doktor)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (appointment == null)
                return NotFound();

            return View(appointment);
        }
    }
} 