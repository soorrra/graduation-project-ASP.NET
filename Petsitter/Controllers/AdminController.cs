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
        
        /// same bro same

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
            AdminRepo adminRep = new AdminRepo(_db, webHostEnvironment);
            Tuple<string, int>  deleteUserRecord = adminRep.DeleteUserRecord(id);
            var deleteMessage = deleteUserRecord.Item1;

            return RedirectToAction("AdminDashboard","Admin", 
            new { message = deleteMessage });
        }

        public IActionResult AddPetType()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddPetType(PetType petType)
        {
            AdminRepo adminRep = new AdminRepo(_db, webHostEnvironment);

            if (!ModelState.IsValid)
            {
                return View(petType);
            }

            adminRep.AddPetType(petType);

            return RedirectToAction("AddPetType");
        }

        public IActionResult AddNews()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddNews(NewsVM newsVM)
        {
            AdminRepo adminRep = new AdminRepo(_db, webHostEnvironment);
            adminRep.AddNews(newsVM);

            // After adding the news, redirect to a success page or display a success message
            TempData["SuccessMessage"] = "News published successfully!";
            return RedirectToAction("AddNews"); // or RedirectToAction("NewsList");
        }
    }

}
