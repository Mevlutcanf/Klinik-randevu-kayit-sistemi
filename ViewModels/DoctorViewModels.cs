using randevu_kayit.Models;

namespace randevu_kayit.ViewModels
{
    public class DoctorDashboardViewModel
    {
        public List<Randevu> TodaysAppointments { get; set; } = new List<Randevu>();
        public List<Randevu> UpcomingAppointments { get; set; } = new List<Randevu>();
        public List<Randevu> CompletedAppointments { get; set; } = new List<Randevu>();
    }
} 