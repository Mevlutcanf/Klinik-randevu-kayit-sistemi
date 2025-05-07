namespace randevu_kayit.Models
{
    public class Patient
    {
        public int Id { get; set; }  // Hasta kimliği
        public string? FullName { get; set; }  // Hasta adı
        public DateTime DateOfBirth { get; set; }  // Doğum tarihi
        public string? Gender { get; set; }  // Cinsiyet

        // İlişkiler
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
         public string Username { get; set; } // <-- BUNU EKLE
    public string Password { get; set; } // <-- BUNU EKLE
    }
}
