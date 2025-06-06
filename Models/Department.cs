using System.ComponentModel.DataAnnotations;

namespace randevu_kayit.Models
{
    public class Department
    {
        public int Id { get; set; }  // Bölüm kimliği

        [Required]
        [StringLength(100)]
        [Display(Name = "Bölüm Adı")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [StringLength(500)]
        [Display(Name = "Görsel URL")]
        public string? ImageUrl { get; set; }

        // İlişkiler
        public virtual ICollection<ApplicationUser> Doctors { get; set; } = new List<ApplicationUser>();
    }
}
