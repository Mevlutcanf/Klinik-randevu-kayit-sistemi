// Models/ApplicationUser.cs
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace randevu_kayit.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Ad Soyad zorunludur.")]
        [Display(Name = "Ad Soyad")]
        public string AdSoyad { get; set; } = string.Empty;

        // Common Properties
        [Display(Name = "Doğum Tarihi")]
        public DateTime? DogumTarihi { get; set; }

        [Display(Name = "TC Kimlik No")]
        [StringLength(11)]
        public string? TCKimlikNo { get; set; }

        [Display(Name = "Cinsiyet")]
        public string? Cinsiyet { get; set; }

        [Display(Name = "Adres")]
        public string? Adres { get; set; }

        [Display(Name = "Kan Grubu")]
        public string? KanGrubu { get; set; }

        // Doctor Specific Properties
        [Display(Name = "Uzmanlık")]
        public string? Uzmanlik { get; set; }

        [Display(Name = "Diploma No")]
        public string? DiplomaNo { get; set; }

        [Display(Name = "Mezun Olduğu Üniversite")]
        public string? MezunOlduguUniversite { get; set; }

        [Display(Name = "Mezuniyet Yılı")]
        public int? MezuniyetYili { get; set; }

        [Display(Name = "Çalışma Saatleri")]
        public string? CalismaSaatleri { get; set; }

        [Display(Name = "Departman")]
        public int? DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }

        [Display(Name = "Hakkında")]
        public string? About { get; set; }

        [Display(Name = "Deneyim (Yıl)")]
        public int? DeneyimYili { get; set; }

        // Patient Specific Properties
        [Display(Name = "Acil Durumda Aranacak Kişi")]
        public string? AcilDurumKisisi { get; set; }

        [Display(Name = "Acil Durum Telefonu")]
        public string? AcilDurumTelefonu { get; set; }

        [Display(Name = "Kronik Hastalıklar")]
        public string? KronikHastaliklar { get; set; }

        [Display(Name = "Alerjiler")]
        public string? Alerjiler { get; set; }

        [Display(Name = "Kullandığı İlaçlar")]
        public string? KullanilanIlaclar { get; set; }

        [Display(Name = "Sigara Kullanımı")]
        public bool? SigaraKullanimi { get; set; }

        [Display(Name = "Alkol Kullanımı")]
        public bool? AlkolKullanimi { get; set; }

        [Display(Name = "Boy (cm)")]
        public int? Boy { get; set; }

        [Display(Name = "Kilo (kg)")]
        public int? Kilo { get; set; }

        [Display(Name = "Profil Fotoğrafı")]
        public string? ProfilePhotoPath { get; set; }

        // Role indicators
        public bool IsDoctor { get; set; }
        public bool IsPatient { get; set; }

        // Navigation properties
        public virtual ICollection<Randevu> HastaRandevulari { get; set; } = new List<Randevu>();
        public virtual ICollection<Randevu> DoktorRandevulari { get; set; } = new List<Randevu>();
        public virtual ICollection<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();
        public virtual ICollection<DoctorLeave> Leaves { get; set; } = new List<DoctorLeave>();
    }
}
