using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace randevu_kayit.Models
{
    public enum QuickAppointmentStatus
    {
        [Display(Name = "Bekliyor")]
        Bekliyor = 0,
        
        [Display(Name = "Onaylandı")]
        Onaylandi = 1,
        
        [Display(Name = "Reddedildi")]
        Reddedildi = 2
    }

    public class QuickAppointment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [Display(Name = "Ad Soyad")]
        [StringLength(100)]
        public string AdSoyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Display(Name = "Telefon")]
        [Phone]
        public string Telefon { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "TC Kimlik No zorunludur.")]
        [StringLength(11)]
        [Display(Name = "TC Kimlik No")]
        public string TCKimlikNo { get; set; } = string.Empty;        [Required(ErrorMessage = "Doktor seçimi zorunludur.")]
        public string DoktorId { get; set; } = string.Empty;

        [Display(Name = "Bölüm")]
        public int? DepartmentId { get; set; }

        [Required(ErrorMessage = "Randevu tarihi zorunludur.")]
        [Display(Name = "Randevu Tarihi")]
        public DateTime TarihSaat { get; set; }

        [Required(ErrorMessage = "Şikayet belirtilmelidir.")]
        [Display(Name = "Şikayet")]
        public string Sikayet { get; set; } = string.Empty;

        [Display(Name = "Durum")]
        public QuickAppointmentStatus Durum { get; set; } = QuickAppointmentStatus.Bekliyor;        [Display(Name = "Oluşturma Tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        [ForeignKey("DoktorId")]
        public virtual ApplicationUser? Doktor { get; set; }
    }
}
