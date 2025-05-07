namespace randevu_kayit.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Specialty { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

        // Yeni eklediğimiz alan
        public string Username { get; set; }
        public string Password { get; set; }

        // DepartmentId ile ilişkili alan
        public int? DepartmentId { get; set; } // Nullable çünkü doktor her zaman bir departmana ait olmayabilir
        public Department Department { get; set; } // Department tablosu ile ilişkiyi belirtiyoruz
    }
}
