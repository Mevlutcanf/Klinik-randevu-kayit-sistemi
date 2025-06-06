using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace randevu_kayit.Models
{
    public class DoctorSchedule
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Doktor seçimi zorunludur.")]
        public string DoktorId { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Tarih zorunludur.")]
        public DateTime Tarih { get; set; }
        
        [Required(ErrorMessage = "Başlangıç saati zorunludur.")]
        public TimeSpan BaslangicSaat { get; set; }
        
        [Required(ErrorMessage = "Bitiş saati zorunludur.")]
        public TimeSpan BitisSaat { get; set; }
        
        public bool IsActive { get; set; } = true;

        [Required(ErrorMessage = "Neden belirtilmelidir.")]
        public string Reason { get; set; } = "Normal Çalışma Saati";

        [ForeignKey("DoktorId")]
        public virtual ApplicationUser Doktor { get; set; } = null!;
    }

    public class DoctorLeave
    {
        public int Id { get; set; }
        
        [Required]
        public string DoktorId { get; set; }
        
        [Required]
        public DateTime StartDate { get; set; }
        
        [Required]
        public DateTime EndDate { get; set; }
        
        [Required]
        [StringLength(500)]
        public string Reason { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("DoktorId")]
        public virtual ApplicationUser Doktor { get; set; }
    }
} 