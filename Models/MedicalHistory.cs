using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace randevu_kayit.Models
{
    public class MedicalHistory
    {
        public int Id { get; set; }

        [Required]
        public string HastaId { get; set; }

        [ForeignKey("HastaId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public ApplicationUser Hasta { get; set; }

        [Required]
        public string DoktorId { get; set; }

        [ForeignKey("DoktorId")]
        [DeleteBehavior(DeleteBehavior.NoAction)]
        public ApplicationUser Doktor { get; set; }

        [Required]
        public DateTime TarihSaat { get; set; }

        [Required]
        public string Tani { get; set; }

        public string? Tedavi { get; set; }

        public string? YazilanIlaclar { get; set; }

        public string? Notlar { get; set; }

        public string? LabSonuclari { get; set; }

        public DateTime? SonrakiRandevuTarihi { get; set; }

        [Required]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        public DateTime? GuncellemeTarihi { get; set; }
    }
} 