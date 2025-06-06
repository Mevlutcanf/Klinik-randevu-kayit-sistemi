using System.ComponentModel.DataAnnotations;

namespace randevu_kayit.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [Display(Name = "E-posta")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public required string Password { get; set; }

        [Display(Name = "Beni hatırla")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ad zorunludur")]
        [Display(Name = "Ad")]
        public required string Ad { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur")]
        [Display(Name = "Soyad")]
        public required string Soyad { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [Display(Name = "E-posta")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "TC Kimlik No zorunludur")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 haneli olmalıdır")]
        [Display(Name = "TC Kimlik No")]
        public required string TCKimlikNo { get; set; }

        [Required(ErrorMessage = "Doğum tarihi zorunludur")]
        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        public DateTime DogumTarihi { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [Display(Name = "Telefon")]
        public required string PhoneNumber { get; set; }

        [Display(Name = "Kan Grubu")]
        public string? KanGrubu { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur")]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar")]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
        public required string ConfirmPassword { get; set; }
    }

    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Ad zorunludur")]
        [Display(Name = "Ad")]
        public required string Ad { get; set; }

        [Required(ErrorMessage = "Soyad zorunludur")]
        [Display(Name = "Soyad")]
        public required string Soyad { get; set; }

        [Required(ErrorMessage = "E-posta adresi zorunludur")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz")]
        [Display(Name = "E-posta")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "TC Kimlik No zorunludur")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "TC Kimlik No 11 haneli olmalıdır")]
        [Display(Name = "TC Kimlik No")]
        public required string TCKimlikNo { get; set; }

        [Required(ErrorMessage = "Doğum tarihi zorunludur")]
        [Display(Name = "Doğum Tarihi")]
        [DataType(DataType.Date)]
        public DateTime DogumTarihi { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur")]
        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz")]
        [Display(Name = "Telefon")]
        public required string PhoneNumber { get; set; }

        [Display(Name = "Kan Grubu")]
        public string? KanGrubu { get; set; }

        [Display(Name = "Uzmanlık")]
        public string? Uzmanlik { get; set; }

        [Display(Name = "Bölüm")]
        public int? DepartmentId { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mevcut Şifre")]
        public string? CurrentPassword { get; set; }

        [StringLength(100, ErrorMessage = "{0} en az {2} karakter uzunluğunda olmalıdır.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre")]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre Tekrar")]
        [Compare("NewPassword", ErrorMessage = "Şifreler eşleşmiyor")]
        public string? ConfirmNewPassword { get; set; }
    }
} 