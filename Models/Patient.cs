namespace randevu_kayit.Models
{
    public class Patient
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }

        // Navigation property
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public string? Username { get; set; }
        public string? Password { get; set; }

        // Yapıcı metod
        public Patient(string fullName, DateTime dateOfBirth, string gender, string username, string password)
        {
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Username = username;
            Password = password;
        }

        // Parametresiz constructor (opsiyonel, EF için gerekebilir)
        public Patient() { }
    }
}
