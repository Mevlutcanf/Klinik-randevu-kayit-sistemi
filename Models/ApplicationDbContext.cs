using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace randevu_kayit.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Randevu> Randevular { get; set; }
        public DbSet<QuickAppointment> QuickAppointments { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<DoctorLeave> DoctorLeaves { get; set; }
        public DbSet<MedicalHistory> MedicalHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure column types for SQL Server
            modelBuilder.Entity<IdentityRole>().Property(r => r.Id).HasMaxLength(450);
            modelBuilder.Entity<IdentityRole>().Property(r => r.Name).HasMaxLength(256);
            modelBuilder.Entity<IdentityRole>().Property(r => r.NormalizedName).HasMaxLength(256);
            modelBuilder.Entity<IdentityRole>().Property(r => r.ConcurrencyStamp).HasMaxLength(450);

            modelBuilder.Entity<ApplicationUser>().Property(u => u.Id).HasMaxLength(450);
            modelBuilder.Entity<ApplicationUser>().Property(u => u.UserName).HasMaxLength(256);
            modelBuilder.Entity<ApplicationUser>().Property(u => u.NormalizedUserName).HasMaxLength(256);
            modelBuilder.Entity<ApplicationUser>().Property(u => u.Email).HasMaxLength(256);
            modelBuilder.Entity<ApplicationUser>().Property(u => u.NormalizedEmail).HasMaxLength(256);
            modelBuilder.Entity<ApplicationUser>().Property(u => u.PasswordHash).HasMaxLength(450);
            modelBuilder.Entity<ApplicationUser>().Property(u => u.SecurityStamp).HasMaxLength(450);
            modelBuilder.Entity<ApplicationUser>().Property(u => u.ConcurrencyStamp).HasMaxLength(450);
            modelBuilder.Entity<ApplicationUser>().Property(u => u.PhoneNumber).HasMaxLength(50);

            // Randevu ilişkileri
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Hasta)
                .WithMany(p => p.HastaRandevulari)
                .HasForeignKey(r => r.HastaId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Doktor)
                .WithMany(d => d.DoktorRandevulari)
                .HasForeignKey(r => r.DoktorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Doktor-Bölüm ilişkisi
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(d => d.Department)
                .WithMany(d => d.Doctors)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            // Doktor çalışma saatleri ilişkisi
            modelBuilder.Entity<DoctorSchedule>()
                .HasOne(ds => ds.Doktor)
                .WithMany()
                .HasForeignKey(ds => ds.DoktorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Doktor izin günleri ilişkisi
            modelBuilder.Entity<DoctorLeave>()
                .HasOne(dl => dl.Doktor)
                .WithMany()
                .HasForeignKey(dl => dl.DoktorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Varsayılan bölümler
            modelBuilder.Entity<Department>().HasData(
                new Department { Id = 1, Name = "Dahiliye", Description = "İç Hastalıkları" },
                new Department { Id = 2, Name = "Kardiyoloji", Description = "Kalp ve Damar Hastalıkları" },
                new Department { Id = 3, Name = "Nöroloji", Description = "Sinir Sistemi Hastalıkları" },
                new Department { Id = 4, Name = "Ortopedi", Description = "Kas ve İskelet Sistemi" },
                new Department { Id = 5, Name = "Göz Hastalıkları", Description = "Göz ve Görme Bozuklukları" }
            );
        }
    }
} 