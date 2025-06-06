using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using randevu_kayit.Models;

namespace randevu_kayit.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DepartmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Departmanları listeleme
        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .Include(d => d.Doctors)
                .ToListAsync();
            return View(departments);
        }

        // Departman ekleme formu
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // Departman düzenleme
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound();
            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Department department)
        {
            if (id != department.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
    }
}
