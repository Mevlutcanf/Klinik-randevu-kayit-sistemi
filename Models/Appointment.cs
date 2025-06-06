namespace randevu_kayit.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = "Bekliyor"; // Bekliyor, Tamamlandı, İptal

        public string? ChiefComplaint { get; set; }
        public string? Diagnosis { get; set; }
        public string? Treatment { get; set; }
        public string? Prescription { get; set; }
        public string? DoctorNotes { get; set; }
        public string? FollowUpInstructions { get; set; }
        public DateTime? LastUpdated { get; set; }

        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; } = null!;

        public int DoctorId { get; set; }
        public virtual Doctor Doctor { get; set; } = null!;
    }
}
