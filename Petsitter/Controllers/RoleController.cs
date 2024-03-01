using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Petsitter.Data;
using Petsitter.Repositories;

namespace Petsitter.Controllers
{
    //This should be uncommented later on, for now keep as it is in order to view user's role in deplevement mode
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        ApplicationDbContext _context;

        public RoleController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Role
        public ActionResult Index()
        {
            RoleRepo roleRepo = new RoleRepo(_context);
            return View(roleRepo.GetAllRoles());
        }
    }

}