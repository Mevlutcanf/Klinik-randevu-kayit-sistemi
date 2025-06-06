using System.ComponentModel.DataAnnotations;

namespace randevu_kayit.ViewModels
{
    public class BulkRoleAssignmentModel
    {
        public List<string> UserIds { get; set; } = new List<string>();
        public string RoleName { get; set; } = string.Empty;
    }

    public class DeleteUserModel
    {
        public string UserId { get; set; } = string.Empty;
    }

    public class EditUserModel
    {
        public string Id { get; set; } = string.Empty;
        
        [Required]
        [Display(Name = "Ad Soyad")]
        public string AdSoyad { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;
        
        [Display(Name = "Telefon")]
        public string? PhoneNumber { get; set; }
        
        [Display(Name = "Rol")]
        public string Role { get; set; } = string.Empty;
        
        [Display(Name = "Aktif")]
        public bool IsActive { get; set; } = true;
    }

    public class ViewUserModel
    {
        public string Id { get; set; } = string.Empty;
        public string AdSoyad { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }

    public class SystemSettingsModel
    {
        [Required]
        [Display(Name = "Site Adı")]
        public string SiteName { get; set; } = string.Empty;
        
        [Display(Name = "Site Açıklaması")]
        public string SiteDescription { get; set; } = string.Empty;
        
        [Display(Name = "Maintenance Modu")]
        public bool MaintenanceMode { get; set; }
        
        [Display(Name = "Kayıt Açık")]
        public bool RegistrationEnabled { get; set; } = true;
        
        [Display(Name = "E-posta Bildirimleri")]
        public bool EmailNotifications { get; set; } = true;
        
        [Display(Name = "SMS Bildirimleri")]
        public bool SmsNotifications { get; set; }
        
        [Display(Name = "Maksimum Günlük Randevu")]
        public int MaxDailyAppointments { get; set; } = 50;
        
        [Display(Name = "Randevu İptal Süresi (Saat)")]
        public int CancellationHours { get; set; } = 2;
    }

    public class UpdateStatusRequest
    {
        public string Status { get; set; } = string.Empty;
        public string? Note { get; set; }
    }

    public class DashboardStats
    {
        public int TotalDoctors { get; set; }
        public int TotalPatients { get; set; }
        public int TotalAppointments { get; set; }
        public int ActiveAppointments { get; set; }
        public int TodaysAppointments { get; set; }
    }
}
