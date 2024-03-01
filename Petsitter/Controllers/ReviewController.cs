using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
//using Petsitter.Data.Services;
using Petsitter.Models;

namespace Petsitter.Controllers
{
    public class ReviewController : Controller
    {

        private readonly PetsitterContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public ReviewController(PetsitterContext db, IWebHostEnvironment webHost)
        {
            _db = db;
            _webHostEnvironment = webHost;
        }

        public IActionResult Index()
        {
            return View();
        }

        

    }










}
