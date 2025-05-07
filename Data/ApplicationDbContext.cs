using Microsoft.EntityFrameworkCore;
using randevu_kayit.Models;

namespace randevu_kayit.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Department> Departments { get; set; }
         public DbSet<Admin> Admins { get; set; } 
    }
}
