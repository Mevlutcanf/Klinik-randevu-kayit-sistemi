using Microsoft.AspNetCore.Identity;
using randevu_kayit.Models;
using Microsoft.EntityFrameworkCore;

namespace randevu_kayit.Data
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Rolleri oluştur
            string[] roleNames = { "Admin", "Doctor", "Patient" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // Admin kullanıcısını oluştur
            var adminUser = await userManager.FindByEmailAsync("admin@klinik.com");
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@klinik.com",
                    Email = "admin@klinik.com",
                    AdSoyad = "Admin User",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Doktorları oluştur
            var departments = new List<string>
            {
                "Dahiliye",
                "Kardiyoloji",
                "Göz Hastalıkları",
                "Ortopedi",
                "Nöroloji",
                "Psikiyatri",
                "Dermatoloji",
                "Kulak Burun Boğaz",
                "Üroloji",
                "Genel Cerrahi",
                "Kadın Hastalıkları ve Doğum",
                "Çocuk Sağlığı ve Hastalıkları",
                "Fizik Tedavi ve Rehabilitasyon",
                "Göğüs Hastalıkları",
                "Endokrinoloji"
            };

            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Bölümleri oluştur
            foreach (var deptName in departments)
            {
                if (!await dbContext.Departments.AnyAsync(d => d.Name == deptName))
                {
                    dbContext.Departments.Add(new Department { Name = deptName });
                }
            }
            await dbContext.SaveChangesAsync();

            var doctors = new List<(string Email, string AdSoyad, string Bolum, string Uzmanlik)>
            {
                ("dr.ahmet@klinik.com", "Ahmet Yılmaz", "Dahiliye", "İç Hastalıkları Uzmanı"),
                ("dr.mehmet@klinik.com", "Mehmet Öz", "Kardiyoloji", "Kardiyoloji Uzmanı"),
                ("dr.ayse@klinik.com", "Ayşe Demir", "Göz Hastalıkları", "Göz Hastalıkları Uzmanı"),
                ("dr.fatma@klinik.com", "Fatma Şahin", "Ortopedi", "Ortopedi ve Travmatoloji Uzmanı"),
                ("dr.ali@klinik.com", "Ali Kaya", "Nöroloji", "Nöroloji Uzmanı"),
                ("dr.zeynep@klinik.com", "Zeynep Yıldız", "Psikiyatri", "Psikiyatri Uzmanı"),
                ("dr.mustafa@klinik.com", "Mustafa Aydın", "Dermatoloji", "Dermatoloji Uzmanı"),
                ("dr.esra@klinik.com", "Esra Çelik", "Kulak Burun Boğaz", "KBB Uzmanı"),
                ("dr.kemal@klinik.com", "Kemal Özkan", "Üroloji", "Üroloji Uzmanı"),
                ("dr.selin@klinik.com", "Selin Arslan", "Genel Cerrahi", "Genel Cerrahi Uzmanı"),
                ("dr.burak@klinik.com", "Burak Yılmaz", "Kadın Hastalıkları ve Doğum", "Kadın Hastalıkları ve Doğum Uzmanı"),
                ("dr.deniz@klinik.com", "Deniz Kara", "Çocuk Sağlığı ve Hastalıkları", "Çocuk Sağlığı ve Hastalıkları Uzmanı"),
                ("dr.canan@klinik.com", "Canan Güneş", "Fizik Tedavi ve Rehabilitasyon", "Fizik Tedavi ve Rehabilitasyon Uzmanı"),
                ("dr.emre@klinik.com", "Emre Yıldırım", "Göğüs Hastalıkları", "Göğüs Hastalıkları Uzmanı"),
                ("dr.pinar@klinik.com", "Pınar Aktaş", "Endokrinoloji", "Endokrinoloji Uzmanı")
            };

            foreach (var doctor in doctors)
            {
                var existingDoctor = await userManager.FindByEmailAsync(doctor.Email);
                if (existingDoctor == null)
                {
                    var department = await dbContext.Departments.FirstOrDefaultAsync(d => d.Name == doctor.Bolum);
                    var newDoctor = new ApplicationUser
                    {
                        UserName = doctor.Email,
                        Email = doctor.Email,
                        AdSoyad = doctor.AdSoyad,
                        Department = department,
                        Uzmanlik = doctor.Uzmanlik,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newDoctor, "Doctor123!");
                    await userManager.AddToRoleAsync(newDoctor, "Doctor");
                }
            }
        }
    }
} 