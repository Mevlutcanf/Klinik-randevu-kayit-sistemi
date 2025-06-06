using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using randevu_kayit.Models;

namespace randevu_kayit.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PatientController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> MedicalHistory()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var histories = await _context.MedicalHistories
                .Include(h => h.Doktor)
                .Where(h => h.HastaId == currentUser.Id)
                .OrderByDescending(h => h.TarihSaat)
                .ToListAsync();

            return View(histories);
        }

        public async Task<IActionResult> Appointments()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var appointments = await _context.Randevular
                .Include(r => r.Doktor)
                .Where(r => r.HastaId == currentUser.Id)
                .OrderByDescending(r => r.RandevuTarihi)
                .ToListAsync();

            return View(appointments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var appointment = await _context.Randevular
                .FirstOrDefaultAsync(r => r.Id == id && r.HastaId == currentUser.Id);

            if (appointment == null)
                return NotFound();            if (appointment.RandevuTarihi <= DateTime.Now.AddHours(2))
            {
                TempData["ErrorMessage"] = "Randevu tarihinden en az 2 saat önce iptal edilmelidir.";
                return RedirectToAction(nameof(Appointments));
            }

            appointment.Durum = RandevuDurumu.Iptal;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Randevunuz başarıyla iptal edildi.";
            return RedirectToAction(nameof(Appointments));
        }
    }
} 