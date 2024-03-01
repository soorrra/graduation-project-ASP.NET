using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petsitter.Data;
using Petsitter.Models;
using Petsitter.Repositories;
using Petsitter.ViewModels;

namespace Petsitter.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly PetsitterContext _db;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly SitterRepos sitterRepos;
        private readonly CustomerRepo customerRepo;
        private readonly PetRepo petRepo;
        private readonly AdminRepo adminRepo;
        

        public AdminController(ILogger<AdminController> logger, PetsitterContext db, IWebHostEnvironment webHost, ApplicationDbContext context )
        {
            _logger            = logger;
            _db                = db;
            webHostEnvironment = webHost;
            _context           = context;

            sitterRepos  = new SitterRepos(_db, webHostEnvironment);
            customerRepo = new CustomerRepo(_db, webHostEnvironment);
            petRepo      = new PetRepo(_db, webHostEnvironment);
            adminRepo    = new AdminRepo(_db, webHostEnvironment, _context);

        }

        public IActionResult AdminDashboard()
        {
            var allUsers = adminRepo.GetAllUsers();

            return View(allUsers);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
