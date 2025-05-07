using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using randevu_kayit.Data;

var builder = WebApplication.CreateBuilder(args);

// Veritabanı bağlantısı
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔒 Cookie tabanlı kimlik doğrulama
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Giriş yapılmamışsa yönlendirilecek sayfa
        options.AccessDeniedPath = "/Account/AccessDenied"; // Yetkisiz erişim varsa yönlendirilecek sayfa
    });

// MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Error handling vs.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// 📌 Authentication ve Authorization sırasına dikkat!
app.UseAuthentication(); // önce auth kontrolü
app.UseAuthorization();  // sonra yetkilendirme

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
