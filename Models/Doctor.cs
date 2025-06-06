namespace randevu_kayit.Models
{
    public class Doctor
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Specialty { get; set; }
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public string? Username { get; set; }
        public string? Password { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public Doctor(string name, string specialty, string username, string password, Department department)
        {
            Name = name;
            Specialty = specialty;
            Username = username;
            Password = password;
            Department = department;
        }

        // Parametresiz constructor
        public Doctor() { }
    }
}
