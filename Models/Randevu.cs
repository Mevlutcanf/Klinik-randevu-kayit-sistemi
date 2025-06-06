using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace randevu_kayit.Models
{
    public enum RandevuDurumu
    {
        [Display(Name = "Aktif")]
        Aktif = 0,
        
        [Display(Name = "Tamamlandı")]
        Tamamlandi = 1,
        
        [Display(Name = "İptal")]
        Iptal = 2
    }

    public class Randevu
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Hasta seçimi zorunludur.")]
        [Display(Name = "Hasta")]
        public string? HastaId { get; set; }

        [ForeignKey("HastaId")]
        public virtual ApplicationUser? Hasta { get; set; }

        [Required(ErrorMessage = "Doktor seçimi zorunludur.")]
        [Display(Name = "Doktor")]
        public string? DoktorId { get; set; }

        [ForeignKey("DoktorId")]
        public virtual ApplicationUser? Doktor { get; set; }

        [Required(ErrorMessage = "Randevu tarihi zorunludur.")]
        [Display(Name = "Randevu Tarihi")]
        [DataType(DataType.DateTime)]
        public DateTime RandevuTarihi { get; set; }

        [Required(ErrorMessage = "Randevu durumu zorunludur.")]
        [Display(Name = "Durum")]
        public RandevuDurumu Durum { get; set; } = RandevuDurumu.Aktif;

        [Display(Name = "Şikayet")]
        public string? Sikayet { get; set; }

        [Display(Name = "Doktor Notu")]
        public string? DoktorNotu { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        [Display(Name = "Güncelleme Tarihi")]
        public DateTime? GuncellemeTarihi { get; set; }

        [Display(Name = "Tanı")]
        public string? Tani { get; set; }

        [Display(Name = "Tedavi")]
        public string? Tedavi { get; set; }

        [Display(Name = "Reçete")]
        public string? Recete { get; set; }

        [Display(Name = "Laboratuvar Sonuçları")]
        public string? LaboratuvarSonuclari { get; set; }

        [Display(Name = "Radyoloji Sonuçları")]
        public string? RadyolojiSonuclari { get; set; }
    }
} 