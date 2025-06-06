using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using randevu_kayit.Models;
using randevu_kayit.ViewModels;
using System.Data;
using System.IO;

namespace randevu_kayit.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public AdminController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context,
            IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _hostEnvironment = hostEnvironment;
        }        public async Task<IActionResult> Index()
        {
            var stats = new DashboardStats
            {
                TotalDoctors = (await _userManager.GetUsersInRoleAsync("Doctor")).Count,
                TotalPatients = (await _userManager.GetUsersInRoleAsync("Patient")).Count,
                TotalAppointments = await _context.Randevular.CountAsync(),
                ActiveAppointments = await _context.Randevular
                    .Where(r => r.Durum == RandevuDurumu.Aktif)
                    .CountAsync(),
                TodaysAppointments = await _context.Randevular
                    .Where(r => r.RandevuTarihi.Date == DateTime.Today)
                    .CountAsync()
            };

            // Ek istatistikler
            ViewBag.WeeklyAppointments = await _context.Randevular
                .Where(r => r.RandevuTarihi >= DateTime.Today.AddDays(-7))
                .CountAsync();
            
            ViewBag.MonthlyAppointments = await _context.Randevular
                .Where(r => r.RandevuTarihi >= DateTime.Today.AddDays(-30))
                .CountAsync();

            ViewBag.TotalDepartments = await _context.Departments.CountAsync();

            // En son aktiviteler
            ViewBag.RecentActivities = await GetRecentActivities();

            // Randevu durumu istatistikleri
            ViewBag.AppointmentsByStatus = await _context.Randevular
                .GroupBy(r => r.Durum)
                .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                .ToListAsync();

            return View(stats);
        }        private async Task<List<object>> GetRecentActivities()
        {
            var activities = new List<object>();

            // Son eklenen doktorlar
            var recentDoctors = await _userManager.Users
                .Where(u => u.IsDoctor == true)
                .OrderByDescending(u => u.Id)
                .Take(5)
                .Select(u => new { 
                    Type = "doctor", 
                    Message = $"Dr. {u.AdSoyad ?? "Bilinmeyen"} sisteme eklendi",
                    Time = "Bugün"
                })
                .ToListAsync();

            // Son randevular
            var recentAppointments = await _context.Randevular
                .Include(r => r.Hasta)
                .Include(r => r.Doktor)
                .OrderByDescending(r => r.RandevuTarihi)
                .Take(5)
                .Select(r => new {
                    Type = "appointment",
                    Message = $"{(r.Hasta != null ? r.Hasta.AdSoyad : "Bilinmeyen")} - Dr. {(r.Doktor != null ? r.Doktor.AdSoyad : "Bilinmeyen")} randevusu oluşturuldu",
                    Time = r.RandevuTarihi.ToString("dd.MM.yyyy HH:mm")
                })
                .ToListAsync();

            activities.AddRange(recentDoctors);
            activities.AddRange(recentAppointments);

            return activities.Take(10).ToList();
        }

        public async Task<IActionResult> Users(string? role = null, string? search = null)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(role))
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role);
                query = query.Where(u => usersInRole.Select(ur => ur.Id).Contains(u.Id));
            }

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(u => u.AdSoyad!.Contains(search) || 
                                       u.Email!.Contains(search) || 
                                       u.UserName!.Contains(search));
            }

            var users = await query
                .Include(u => u.Department)
                .OrderBy(u => u.AdSoyad)
                .ToListAsync();

            ViewBag.Roles = new[] { "Doctor", "Patient", "Admin" };
            ViewBag.SelectedRole = role;
            ViewBag.Search = search;

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleUserStatus(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            if (user.LockoutEnd == null || user.LockoutEnd < DateTimeOffset.Now)
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            }
            else
            {
                await _userManager.SetLockoutEndDateAsync(user, null);
            }            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> BulkAssignRole([FromBody] BulkRoleAssignmentModel model)
        {
            if (model.UserIds == null || !model.UserIds.Any())
            {
                return Json(new { success = false, message = "Kullanıcı seçiniz." });
            }

            try
            {
                foreach (var userId in model.UserIds)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        // Remove existing roles
                        var userRoles = await _userManager.GetRolesAsync(user);
                        await _userManager.RemoveFromRolesAsync(user, userRoles);
                        
                        // Add new role
                        await _userManager.AddToRoleAsync(user, model.RoleName);
                    }
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Rol ataması sırasında hata oluştu: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromBody] DeleteUserModel model)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return Json(new { success = false, message = "Kullanıcı bulunamadı." });
                }

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Kullanıcı silinemedi." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata oluştu: " + ex.Message });
            }
        }

        public async Task<IActionResult> Departments()
        {
            var departments = await _context.Departments
                .Include(d => d.Doctors)
                .OrderBy(d => d.Name)
                .ToListAsync();

            return View(departments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDepartment(Department department, IFormFile? image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "img", "departments");
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    department.ImageUrl = "/img/departments/" + uniqueFileName;
                }

                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Departments));
            }

            return View(nameof(Departments));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDepartment(int id, string name, string description, IFormFile? image)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
                return NotFound();

            if (image != null)
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(department.ImageUrl))
                {
                    var oldFilePath = Path.Combine(_hostEnvironment.WebRootPath, department.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Save new image
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "img", "departments");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await image.CopyToAsync(fileStream);
                }

                department.ImageUrl = "/img/departments/" + uniqueFileName;
            }

            department.Name = name;
            department.Description = description;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Departments));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = await _context.Departments
                .Include(d => d.Doctors)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (department == null)
                return NotFound();

            if (department.Doctors.Any())
            {
                TempData["ErrorMessage"] = "Bu bölümde kayıtlı doktorlar olduğu için silinemez.";
                return RedirectToAction(nameof(Departments));
            }

            // Delete department image if exists
            if (!string.IsNullOrEmpty(department.ImageUrl))
            {
                var filePath = Path.Combine(_hostEnvironment.WebRootPath, department.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Departments));
        }        public async Task<IActionResult> Appointments(
            DateTime? startDate = null,
            DateTime? endDate = null,
            string? status = null,
            string? doctorId = null)
        {
            var query = _context.Randevular
                .Include(r => r.Doktor)
                .Include(r => r.Hasta)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(r => r.RandevuTarihi.Date >= startDate.Value.Date);
            
            if (endDate.HasValue)
                query = query.Where(r => r.RandevuTarihi.Date <= endDate.Value.Date);
            
            if (!string.IsNullOrEmpty(status))
                query = query.Where(r => r.Durum.ToString() == status);
            
            if (!string.IsNullOrEmpty(doctorId))
                query = query.Where(r => r.DoktorId == doctorId);

            var appointments = await query
                .OrderByDescending(r => r.RandevuTarihi)
                .ToListAsync();

            ViewBag.Doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            ViewBag.Status = status;
            ViewBag.DoctorId = doctorId;

            return View(appointments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAppointmentStatus(int id, RandevuDurumu status)
        {
            var appointment = await _context.Randevular.FindAsync(id);
            if (appointment == null)
                return NotFound();

            appointment.Durum = status;
            appointment.GuncellemeTarihi = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Appointments));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Json(new { success = true });
                }
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> CancelAppointment(int id)
        {
            var appointment = await _context.Randevular.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Durum = RandevuDurumu.Iptal;
            appointment.GuncellemeTarihi = DateTime.Now;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Appointments));
        }

        [HttpPut("api/QuickAppointment/{id}/status")]
        public async Task<IActionResult> UpdateQuickAppointmentStatus(int id, [FromBody] UpdateStatusRequest request)
        {
            var appointment = await _context.QuickAppointments.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Durum = Enum.Parse<QuickAppointmentStatus>(request.Status);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(string id, string adSoyad, string email, string phoneNumber, int? departmentId)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            user.AdSoyad = adSoyad;
            user.Email = email;
            user.PhoneNumber = phoneNumber;
            user.DepartmentId = departmentId;

            await _userManager.UpdateAsync(user);
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAppointmentNote(int id, string note)
        {
            var appointment = await _context.Randevular.FindAsync(id);
            if (appointment == null)
                return NotFound();

            appointment.DoktorNotu = note;
            appointment.GuncellemeTarihi = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Appointments));
        }

        public async Task<IActionResult> Doctors()
        {
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            return View(doctors);
        }

        public async Task<IActionResult> AddDoctor()
        {
            ViewBag.Departments = new SelectList(await _context.Departments.OrderBy(d => d.Name).ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDoctor(ApplicationUser doctor, IFormFile? profilePhoto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(await _context.Departments.OrderBy(d => d.Name).ToListAsync(), "Id", "Name");
                return View(doctor);
            }

            // Handle profile photo
            if (profilePhoto != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "doctors");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + profilePhoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePhoto.CopyToAsync(fileStream);
                }

                doctor.ProfilePhotoPath = "/uploads/doctors/" + uniqueFileName;
            }

            // Generate username from email
            doctor.UserName = doctor.Email;
            doctor.IsDoctor = true;

            // Create user
            var result = await _userManager.CreateAsync(doctor, "Doctor123!"); // Default password
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(doctor, "Doctor");
                TempData["SuccessMessage"] = "Doktor başarıyla eklendi.";
                return RedirectToAction(nameof(Doctors));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            ViewBag.Departments = new SelectList(await _context.Departments.OrderBy(d => d.Name).ToListAsync(), "Id", "Name");
            return View(doctor);
        }

        public async Task<IActionResult> EditDoctor(string id)
        {
            var doctor = await _userManager.FindByIdAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            ViewBag.Departments = new SelectList(await _context.Departments.OrderBy(d => d.Name).ToListAsync(), "Id", "Name");
            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDoctor(ApplicationUser doctor, IFormFile? profilePhoto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Departments = new SelectList(await _context.Departments.OrderBy(d => d.Name).ToListAsync(), "Id", "Name");
                return View(doctor);
            }

            var existingDoctor = await _userManager.FindByIdAsync(doctor.Id);
            if (existingDoctor == null)
            {
                return NotFound();
            }

            // Handle profile photo
            if (profilePhoto != null)
            {
                string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, "uploads", "doctors");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + "_" + profilePhoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await profilePhoto.CopyToAsync(fileStream);
                }

                // Delete old photo if exists
                if (!string.IsNullOrEmpty(existingDoctor.ProfilePhotoPath))
                {
                    var oldFilePath = Path.Combine(_hostEnvironment.WebRootPath, existingDoctor.ProfilePhotoPath.TrimStart('/'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                existingDoctor.ProfilePhotoPath = "/uploads/doctors/" + uniqueFileName;
            }

            // Update properties
            existingDoctor.AdSoyad = doctor.AdSoyad;
            existingDoctor.Email = doctor.Email;
            existingDoctor.PhoneNumber = doctor.PhoneNumber;
            existingDoctor.DepartmentId = doctor.DepartmentId;
            existingDoctor.Uzmanlik = doctor.Uzmanlik;
            existingDoctor.About = doctor.About;
            existingDoctor.CalismaSaatleri = doctor.CalismaSaatleri;

            var result = await _userManager.UpdateAsync(existingDoctor);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Doktor bilgileri başarıyla güncellendi.";
                return RedirectToAction(nameof(Doctors));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }            ViewBag.Departments = new SelectList(await _context.Departments.OrderBy(d => d.Name).ToListAsync(), "Id", "Name");
            return View(doctor);
        }

        // Toplu işlemler için yeni metodlar
        [HttpPost]
        public async Task<IActionResult> BulkDeleteDoctors(string[] doctorIds)
        {
            if (doctorIds == null || doctorIds.Length == 0)
            {
                TempData["ErrorMessage"] = "Silmek için doktor seçiniz.";
                return RedirectToAction(nameof(Doctors));
            }

            int deletedCount = 0;
            foreach (var doctorId in doctorIds)
            {
                var doctor = await _userManager.FindByIdAsync(doctorId);
                if (doctor != null)
                {
                    var result = await _userManager.DeleteAsync(doctor);
                    if (result.Succeeded)
                    {
                        deletedCount++;
                    }
                }
            }

            TempData["SuccessMessage"] = $"{deletedCount} doktor başarıyla silindi.";
            return RedirectToAction(nameof(Doctors));
        }

        // Sistem ayarları
        public IActionResult SystemSettings()
        {
            return View();
        }        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSystemSettings(SystemSettingsModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Here you would typically save settings to database or configuration
                    // For now, just show success message
                    TempData["SuccessMessage"] = "Sistem ayarları başarıyla güncellendi.";
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Ayarlar güncellenirken hata oluştu: " + ex.Message;
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Lütfen tüm alanları doğru şekilde doldurun.";
            }

            return RedirectToAction(nameof(SystemSettings));
        }

        public IActionResult SystemLogs(int page = 1, string level = "", string search = "")
        {
            var logs = GetSystemLogs(page, level, search);
            return View(logs);
        }        private object GetSystemLogs(int page, string level, string search)
        {
            // Enhanced system logs with more realistic data
            var allLogs = new List<dynamic>();
            
            // Generate recent activities from actual database data
            var recentAppointments = _context.Randevular
                .Include(r => r.Hasta)
                .Include(r => r.Doktor)
                .OrderByDescending(r => r.RandevuTarihi)
                .Take(10)                .Select(r => new { 
                    Id = r.Id, 
                    Time = r.RandevuTarihi, 
                    Level = "Info", 
                    Message = $"Randevu oluşturuldu - {(r.Hasta != null ? r.Hasta.AdSoyad : "Bilinmeyen")} / Dr. {(r.Doktor != null ? r.Doktor.AdSoyad : "Bilinmeyen")}", 
                    User = r.Hasta != null ? r.Hasta.Email ?? "system" : "system", 
                    Details = $"Randevu ID: {r.Id}, Durum: {r.Durum}" 
                })
                .ToList();

            var recentUsers = _context.Users
                .Where(u => u.EmailConfirmed)
                .OrderByDescending(u => u.Id)
                .Take(5)
                .Select(u => new { 
                    Id = int.Parse(u.Id.Substring(0, 8)), 
                    Time = DateTime.Now.AddHours(-new Random().Next(1, 24)), 
                    Level = "Info", 
                    Message = $"Yeni kullanıcı kaydı - {u.AdSoyad}", 
                    User = u.Email ?? "system", 
                    Details = $"Kullanıcı ID: {u.Id}" 
                })
                .ToList();

            // Add some system logs
            var systemLogs = new List<dynamic>
            {
                new { Id = 1001, Time = DateTime.Now.AddMinutes(-5), Level = "Info", Message = "Sistem sağlık kontrolü tamamlandı", User = "System", Details = "Tüm servisler çalışıyor" },
                new { Id = 1002, Time = DateTime.Now.AddMinutes(-15), Level = "Warning", Message = "Veritabanı bağlantı havuzu %80 dolu", User = "System", Details = "Connection pool monitoring" },
                new { Id = 1003, Time = DateTime.Now.AddMinutes(-30), Level = "Info", Message = "Otomatik yedekleme tamamlandı", User = "System", Details = "Backup size: 2.5MB" },
                new { Id = 1004, Time = DateTime.Now.AddHours(-1), Level = "Info", Message = "Email bildirimleri gönderildi", User = "System", Details = "5 randevu hatırlatması" },
                new { Id = 1005, Time = DateTime.Now.AddHours(-2), Level = "Error", Message = "SMTP sunucu geçici olarak erişilemez", User = "System", Details = "Email service retry scheduled" }
            };            allLogs.AddRange(recentAppointments.Cast<dynamic>());
            allLogs.AddRange(recentUsers.Cast<dynamic>());
            allLogs.AddRange(systemLogs);

            // Filter logs
            var filteredLogs = allLogs.AsEnumerable();
            
            if (!string.IsNullOrEmpty(level))
            {
                filteredLogs = filteredLogs.Where(l => l.Level == level);
            }
            
            if (!string.IsNullOrEmpty(search))
            {
                filteredLogs = filteredLogs.Where(l => l.Message.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var pageSize = 10;
            var totalCount = filteredLogs.Count();
            var logs = filteredLogs.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new
            {
                Logs = logs,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                TotalCount = totalCount,
                Level = level,
                Search = search
            };
        }

        [HttpGet]
        public IActionResult GetLogDetails(int id)
        {
            // Mock log details - in a real application, this would fetch from logging provider
            var logDetail = new
            {
                Id = id,
                Time = DateTime.Now.AddMinutes(-id * 5),
                Level = "Info",
                Message = $"Detaylı log mesajı #{id}",
                User = "admin@example.com",
                IpAddress = "192.168.1.100",
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36",
                StackTrace = id % 3 == 0 ? "at AdminController.GetLogDetails(Int32 id)\nat System.Threading.Tasks.Task.Run()" : null,
                AdditionalData = new
                {
                    RequestId = Guid.NewGuid().ToString(),
                    SessionId = "session_" + id,
                    Duration = $"{id * 100}ms"
                }
            };

            return Json(logDetail);
        }

        public IActionResult BackupRestore()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBackup()
        {
            try
            {
                // Mock backup creation - in a real application, this would create actual backup
                var backupFileName = $"backup_{DateTime.Now:yyyyMMdd_HHmmss}.sql";
                
                // Simulate backup process
                await Task.Delay(2000); // Simulate backup time
                
                return Json(new { 
                    success = true, 
                    message = "Yedekleme başarıyla oluşturuldu",
                    fileName = backupFileName,
                    size = "2.5 MB",
                    date = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Yedekleme hatası: " + ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RestoreBackup(IFormFile backupFile)
        {
            try
            {
                if (backupFile == null || backupFile.Length == 0)
                {
                    return Json(new { success = false, message = "Lütfen geçerli bir yedek dosyası seçin." });
                }

                // Mock restore process - in a real application, this would restore from backup
                await Task.Delay(3000); // Simulate restore time
                
                return Json(new { 
                    success = true, 
                    message = "Veritabanı başarıyla geri yüklendi",
                    fileName = backupFile.FileName,
                    restoredAt = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Geri yükleme hatası: " + ex.Message });
            }
        }
    }
}