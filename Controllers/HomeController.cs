using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using randevu_kayit.Models;
using randevu_kayit.Data;

namespace randevu_kayit.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(
        ILogger<HomeController> logger,
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var departments = await _context.Departments.ToListAsync();
        var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
        
        // Debug logging
        _logger.LogInformation($"Departments count: {departments.Count}");
        _logger.LogInformation($"Doctors count: {doctors.Count}");
        
        ViewBag.Departments = departments;
        ViewBag.Doctors = doctors;
        
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> TestDatabase()
    {
        try
        {
            var departments = await _context.Departments.ToListAsync();
            var doctors = await _userManager.GetUsersInRoleAsync("Doctor");
            
            return Json(new
            {
                Success = true,
                DepartmentCount = departments.Count,
                Departments = departments.Select(d => new { d.Id, d.Name }).ToList(),
                DoctorCount = doctors.Count,
                Doctors = doctors.Take(5).Select(d => new { 
                    d.Id, 
                    d.AdSoyad, 
                    d.DepartmentId,
                    d.Uzmanlik
                }).ToList()
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                Success = false,
                Error = ex.Message,
                StackTrace = ex.StackTrace
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> SeedDatabase()
    {
        try
        {
            var serviceProvider = HttpContext.RequestServices;
            await DbSeeder.SeedRolesAndAdminAsync(serviceProvider);
            
            return Json(new { success = true, message = "Database seeded successfully" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message, stackTrace = ex.StackTrace });
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
