using System.ComponentModel.DataAnnotations;

namespace randevu_kayit.ViewModels
{
    public class RandevuViewModel
    {
        [Required(ErrorMessage = "Bölüm seçimi zorunludur")]
        [Display(Name = "Bölüm")]
        public required string Bolum { get; set; }

        [Required(ErrorMessage = "Doktor seçimi zorunludur")]
        [Display(Name = "Doktor")]
        public required string DoktorId { get; set; }

        [Required(ErrorMessage = "Randevu tarihi zorunludur")]
        [Display(Name = "Randevu Tarihi")]
        public DateTime RandevuTarihi { get; set; }

        [Required(ErrorMessage = "Şikayet açıklaması zorunludur")]
        [Display(Name = "Şikayetiniz")]
        [StringLength(500, ErrorMessage = "Şikayet açıklaması en fazla 500 karakter olabilir")]
        public required string Sikayet { get; set; }
    }
} 