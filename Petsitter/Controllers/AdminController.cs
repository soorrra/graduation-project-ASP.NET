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
        private ILogger<AdminController> _logger;
        private PetsitterContext _db;
        private ApplicationDbContext _context;
        private IWebHostEnvironment webHostEnvironment;
        private readonly SitterRepos sitterRepos;
        private readonly CustomerRepo customerRepo;
        private readonly PetRepo petRepo;
        private AdminRepo adminRepo;


        public AdminController(ILogger<AdminController> logger, PetsitterContext context, IWebHostEnvironment webHost)
        {
            _logger = logger;
            _db = context;
            webHostEnvironment = webHost;
           

            sitterRepos = new SitterRepos(_db, webHostEnvironment);
            customerRepo = new CustomerRepo(_db, webHostEnvironment);
            petRepo = new PetRepo(_db, webHostEnvironment);
            adminRepo = new AdminRepo(_db, webHostEnvironment);
            
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
        public IActionResult DeleteUser(int id)
        {
            // Call a method in AdminRepo to delete the user and related records
            adminRepo.DeleteUserAndRelatedRecords(id);

            return RedirectToAction("AdminDashboard", "Admin");
        }
    }
}