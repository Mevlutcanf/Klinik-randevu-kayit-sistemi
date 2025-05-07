namespace randevu_kayit.Models
{
    public class Department
    {
        public int Id { get; set; }  // Bölüm kimliği
        public string? Name { get; set; }  // Bölüm adı

        // İlişkiler
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>(); // Bölümde çalışan doktorlar
    }
}
