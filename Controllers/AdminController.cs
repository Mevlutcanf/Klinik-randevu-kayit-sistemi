using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using randevu_kayit.Data;
using randevu_kayit.Models;

[Authorize(Roles = "Admin")]  // Admin rolüyle giriş yapmış kullanıcılar erişebilsin
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Doktorları listeleme
    public IActionResult Doctors()
    {
        var doctors = _context.Doctors.ToList();
        return View(doctors);
    }

    // Hastaları listeleme
    public IActionResult Patients()
    {
        var patients = _context.Patients.ToList();
        return View(patients);
    }

    // Departmanları listeleme
    public IActionResult Departments()
    {
        var departments = _context.Departments.ToList();
        return View(departments);
    }

    // Diğer admin işlemleri burada yapılabilir
}
