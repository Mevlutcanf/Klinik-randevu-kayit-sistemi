// public class PatientController : Controller
// {
//     private readonly ApplicationDbContext _context;

//     public PatientController(ApplicationDbContext context)
//     {
//         _context = context;
//     }

//     // Hastaları listeleme
//     public IActionResult Index()
//     {
//         var patients = _context.Patients.ToList();
//         return View(patients);
//     }

//     // Hasta ekleme formu
//     public IActionResult Create()
//     {
//         return View();
//     }

//     [HttpPost]
//     public IActionResult Create(Patient patient)
//     {
//         if (ModelState.IsValid)
//         {
//             _context.Patients.Add(patient);
//             _context.SaveChanges();
//             return RedirectToAction(nameof(Index));
//         }
//         return View(patient);
//     }

//     // Hasta düzenleme
//     public IActionResult Edit(int id)
//     {
//         var patient = _context.Patients.Find(id);
//         if (patient == null) return NotFound();
//         return View(patient);
//     }

//     [HttpPost]
//     public IActionResult Edit(int id, Patient patient)
//     {
//         if (id != patient.Id) return NotFound();

//         if (ModelState.IsValid)
//         {
//             _context.Update(patient);
//             _context.SaveChanges();
//             return RedirectToAction(nameof(Index));
//         }
//         return View(patient);
//     }
// }
