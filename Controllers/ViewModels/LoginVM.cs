using System.ComponentModel.DataAnnotations;


namespace randevu_kayit.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Kullanıcı adı zorunlu.")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Şifre zorunlu.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
