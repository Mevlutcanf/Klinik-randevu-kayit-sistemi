using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using randevu_kayit.Models;

namespace randevu_kayit.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DoctorController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Dashboard - Shows today's appointments and quick stats
        public async Task<IActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var today = DateTime.Today;
            var appointments = await _context.Randevular
                .Include(r => r.Hasta)
                .Where(r => r.DoktorId == currentUser.Id &&
                           r.RandevuTarihi.Date == today &&
                           r.Durum == RandevuDurumu.Aktif)
                .OrderBy(r => r.RandevuTarihi)
                .ToListAsync();

            // Quick stats
            var totalAppointments = await _context.Randevular
                .Where(r => r.DoktorId == currentUser.Id && r.Durum != RandevuDurumu.Iptal)
                .CountAsync();

            var uniquePatients = await _context.Randevular
                .Where(r => r.DoktorId == currentUser.Id && r.Durum != RandevuDurumu.Iptal)
                .Select(r => r.HastaId)
                .Distinct()
                .CountAsync();

            ViewBag.TotalAppointments = totalAppointments;
            ViewBag.UniquePatients = uniquePatients;
            ViewBag.TodaysAppointmentCount = appointments.Count;

            return View(appointments);
        }

        // Schedule Management - Doctor's working hours
        public async Task<IActionResult> Schedule()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var schedules = await _context.DoctorSchedules
                .Where(s => s.DoktorId == currentUser.Id && s.IsActive)
                .OrderBy(s => s.Tarih)
                .ToListAsync();

            var leaves = await _context.DoctorLeaves
                .Where(dl => dl.DoktorId == currentUser.Id && dl.EndDate >= DateTime.Today)
                .OrderBy(dl => dl.StartDate)
                .ToListAsync();

            ViewBag.Leaves = leaves;

            return View(schedules);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSchedule(DateTime tarih, TimeSpan baslangicSaat, TimeSpan bitisSaat)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var schedule = new DoctorSchedule
            {
                DoktorId = currentUser.Id,
                Tarih = tarih,
                BaslangicSaat = baslangicSaat,
                BitisSaat = bitisSaat,
                Reason = "Normal Çalışma Saati"
            };

            _context.DoctorSchedules.Add(schedule);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Schedule));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSchedule([FromBody] List<DoctorSchedule> schedules)
        {
            var currentDoctor = await _userManager.GetUserAsync(User);
            if (currentDoctor == null) return Challenge();

            // Deactivate existing schedules
            var existingSchedules = await _context.DoctorSchedules
                .Where(ds => ds.DoktorId == currentDoctor.Id)
                .ToListAsync();

            foreach (var schedule in existingSchedules)
            {
                schedule.IsActive = false;
            }

            // Add new schedules
            foreach (var schedule in schedules)
            {
                schedule.DoktorId = currentDoctor.Id;
                schedule.IsActive = true;
                _context.DoctorSchedules.Add(schedule);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddLeave(DoctorLeave leave)
        {
            var currentDoctor = await _userManager.GetUserAsync(User);
            if (currentDoctor == null) return Challenge();

            // Check for overlapping leaves
            var hasOverlap = await _context.DoctorLeaves
                .AnyAsync(dl => dl.DoktorId == currentDoctor.Id &&
                               dl.StartDate <= leave.EndDate &&
                               dl.EndDate >= leave.StartDate);

            if (hasOverlap)
            {
                return Json(new { success = false, message = "Bu tarih aralığında zaten izin bulunmaktadır." });
            }

            leave.DoktorId = currentDoctor.Id;
            _context.DoctorLeaves.Add(leave);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLeave(int id)
        {
            var currentDoctor = await _userManager.GetUserAsync(User);
            if (currentDoctor == null) return Challenge();

            var leave = await _context.DoctorLeaves
                .FirstOrDefaultAsync(dl => dl.Id == id && dl.DoktorId == currentDoctor.Id);

            if (leave == null)
                return NotFound();

            _context.DoctorLeaves.Remove(leave);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        // Appointment History with filtering
        public async Task<IActionResult> AppointmentHistory(DateTime? startDate, DateTime? endDate)
        {
            var currentDoctor = await _userManager.GetUserAsync(User);
            if (currentDoctor == null) return Challenge();

            var query = _context.Randevular
                .Include(r => r.Hasta)
                .Where(r => r.DoktorId == currentDoctor.Id);

            if (startDate.HasValue)
                query = query.Where(r => r.RandevuTarihi.Date >= startDate.Value.Date);
            
            if (endDate.HasValue)
                query = query.Where(r => r.RandevuTarihi.Date <= endDate.Value.Date);

            var appointments = await query
                .OrderByDescending(r => r.RandevuTarihi)
                .ToListAsync();

            return View(appointments);
        }

        // Profile Management
        public async Task<IActionResult> Profile()
        {
            var currentDoctor = await _userManager.GetUserAsync(User);
            if (currentDoctor == null) return Challenge();

            return View(currentDoctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(string uzmanlik, string about)
        {
            var currentDoctor = await _userManager.GetUserAsync(User);
            if (currentDoctor == null) return Challenge();

            currentDoctor.Uzmanlik = uzmanlik;
            currentDoctor.About = about;

            await _userManager.UpdateAsync(currentDoctor);
            
            TempData["Message"] = "Profiliniz başarıyla güncellendi.";
            return RedirectToAction(nameof(Profile));
        }

        // Add notes to appointment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNote(int appointmentId, string note)
        {
            var currentDoctor = await _userManager.GetUserAsync(User);
            if (currentDoctor == null) return Challenge();

            var appointment = await _context.Randevular
                .FirstOrDefaultAsync(r => r.Id == appointmentId && r.DoktorId == currentDoctor.Id);

            if (appointment == null)
                return NotFound();

            appointment.DoktorNotu = note;
            appointment.GuncellemeTarihi = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AppointmentHistory));
        }

        // Appointments view - Shows all appointments for the doctor
        public async Task<IActionResult> Appointments(string? status = null)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var query = _context.Randevular
                .Include(r => r.Hasta)
                .Where(r => r.DoktorId == currentUser.Id);

            if (!string.IsNullOrEmpty(status))
            {
                var durumEnum = Enum.Parse<RandevuDurumu>(status);
                query = query.Where(r => r.Durum == durumEnum);
            }

            var appointments = await query
                .OrderByDescending(r => r.RandevuTarihi)
                .ToListAsync();

            ViewBag.Status = status;
            return View(appointments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAppointmentStatus(int id, RandevuDurumu status)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var appointment = await _context.Randevular.FindAsync(id);
            if (appointment == null || appointment.DoktorId != currentUser.Id)
                return NotFound();

            appointment.Durum = status;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Appointments));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAppointmentNote(int id, string note)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var appointment = await _context.Randevular.FindAsync(id);
            if (appointment == null || appointment.DoktorId != currentUser.Id)
                return NotFound();

            appointment.DoktorNotu = note;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Appointments));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var schedule = await _context.DoctorSchedules.FindAsync(id);
            if (schedule == null || schedule.DoktorId != currentUser.Id)
                return NotFound();

            _context.DoctorSchedules.Remove(schedule);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Schedule));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddMedicalHistory(int appointmentId, MedicalHistory history)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var appointment = await _context.Randevular
                .Include(r => r.Hasta)
                .FirstOrDefaultAsync(r => r.Id == appointmentId && r.DoktorId == currentUser.Id);

            if (appointment == null)
                return NotFound();

            history.DoktorId = currentUser.Id;
            history.HastaId = appointment.HastaId;
            history.TarihSaat = appointment.RandevuTarihi;

            _context.MedicalHistories.Add(history);
            await _context.SaveChangesAsync();

            // Randevuyu tamamlandı olarak işaretle
            appointment.Durum = RandevuDurumu.Tamamlandi;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(AppointmentHistory));
        }

        public async Task<IActionResult> PatientHistory(string hastaId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var histories = await _context.MedicalHistories
                .Include(h => h.Hasta)
                .Where(h => h.HastaId == hastaId && h.DoktorId == currentUser.Id)
                .OrderByDescending(h => h.TarihSaat)
                .ToListAsync();

            var patient = await _userManager.FindByIdAsync(hastaId);
            if (patient == null) return NotFound();

            ViewBag.Patient = patient;
            return View(histories);
        }

        [AllowAnonymous]
        public async Task<IActionResult> List()
        {
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            var doctorsWithDepartments = await _context.Users
                .Include(u => u.Department)
                .Where(u => doctors.Select(d => d.Id).Contains(u.Id))
                .OrderBy(u => u.Department.Name)
                .ThenBy(u => u.AdSoyad)
                .ToListAsync();

            return View(doctorsWithDepartments);
        }

        public async Task<IActionResult> AppointmentDetails(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var appointment = await _context.Randevular
                .Include(a => a.Hasta)
                .FirstOrDefaultAsync(a => a.Id == id && a.DoktorId == currentUser.Id);

            if (appointment == null)
                return NotFound();

            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAppointmentDetails(int id, string sikayet, string tani, string tedavi, string recete, string doktorNotu, string durum)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var appointment = await _context.Randevular
                .Include(a => a.Hasta)
                .FirstOrDefaultAsync(a => a.Id == id && a.DoktorId == currentUser.Id);

            if (appointment == null)
                return NotFound();

            // Update appointment details
            appointment.Sikayet = sikayet;
            appointment.Tani = tani;
            appointment.Tedavi = tedavi;
            appointment.Recete = recete;
            appointment.DoktorNotu = doktorNotu;
            appointment.GuncellemeTarihi = DateTime.Now;
            appointment.Durum = durum == "Tamamlandı" ? RandevuDurumu.Tamamlandi :
                              durum == "İptal" ? RandevuDurumu.Iptal :
                              RandevuDurumu.Aktif;

            try
            {
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Randevu detayları başarıyla güncellendi.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Randevu detayları güncellenirken bir hata oluştu.";
            }

            return RedirectToAction(nameof(AppointmentDetails), new { id = appointment.Id });
        }
    }
} 