using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization;
using Org.BouncyCastle.Cms;
using Petsitter.Data;
using Petsitter.Models;
using Petsitter.Repositories;
using Petsitter.ViewModels;
using System.Diagnostics;
using System.Drawing.Printing;

namespace Petsitter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PetsitterContext _db;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<HomeController> _localizer;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webHost, PetsitterContext context, IConfiguration configuration, IStringLocalizer<HomeController> localizer)
        {
            _logger = logger;
            _webHostEnvironment = webHost;
            _db = context;
            _configuration = configuration;
            _localizer = localizer;
        }


        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.Now.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }


        public IActionResult Index()
        {
            //var sendgridApiKey = _configuration["SendGrid:ApiKey"];
            var connectionString = _configuration["ConnectionStrings:DefaultConnection"];

            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            //return View();

            ReviewRepo rRepo = new ReviewRepo(_db);
            var top3Sitter = rRepo.GetTop3SitterVMs().ToList();

            return View(top3Sitter);
        }


        //public IActionResult TopPetsitter()
        //{
        //    ReviewRepo rRepo = new ReviewRepo(_db);
        //    var top3Sitter = rRepo.GetTop3SitterVMs().ToList();

        //    return View(top3Sitter);
        //}

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult NoPermission()
        {
            return View();
        }

        [Authorize]
        public IActionResult NoBookedPet()
        {
            return View();
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}