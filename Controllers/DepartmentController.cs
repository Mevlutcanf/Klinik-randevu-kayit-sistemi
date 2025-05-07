// public class DepartmentController : Controller
// {
//     private readonly ApplicationDbContext _context;

//     public DepartmentController(ApplicationDbContext context)
//     {
//         _context = context;
//     }

//     // Departmanları listeleme
//     public IActionResult Index()
//     {
//         var departments = _context.Departments.ToList();
//         return View(departments);
//     }

//     // Departman ekleme formu
//     public IActionResult Create()
//     {
//         return View();
//     }

//     [HttpPost]
//     public IActionResult Create(Department department)
//     {
//         if (ModelState.IsValid)
//         {
//             _context.Departments.Add(department);
//             _context.SaveChanges();
//             return RedirectToAction(nameof(Index));
//         }
//         return View(department);
//     }

//     // Departman düzenleme
//     public IActionResult Edit(int id)
//     {
//         var department = _context.Departments.Find(id);
//         if (department == null) return NotFound();
//         return View(department);
//     }

//     [HttpPost]
//     public IActionResult Edit(int id, Department department)
//     {
//         if (id != department.Id) return NotFound();

//         if (ModelState.IsValid)
//         {
//             _context.Update(department);
//             _context.SaveChanges();
//             return RedirectToAction(nameof(Index));
//         }
//         return View(department);
//     }
// }
