using randevu_kayit.Models;
using System.Collections.Generic;

namespace randevu_kayit.ViewModels
{
    public class AdminDashboardViewModel
    {
        public List<ApplicationUser> TotalDoctors { get; set; } = new List<ApplicationUser>();
        public List<ApplicationUser> TotalPatients { get; set; } = new List<ApplicationUser>();
        public int TotalAppointments { get; set; }
        public int PendingQuickAppointments { get; set; }
        public List<QuickAppointment> QuickAppointments { get; set; } = new List<QuickAppointment>();
        public List<Randevu> RecentAppointments { get; set; } = new List<Randevu>();
    }

    public class UserManagementViewModel
    {
        public required ApplicationUser User { get; set; }
        public required IList<string> Roles { get; set; }
    }
} 