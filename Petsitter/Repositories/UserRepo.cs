using Petsitter.Data;
using Petsitter.Models;
using Petsitter.ViewModels;

namespace Petsitter.Repositories
{
    public class UserRepo
    {
        ApplicationDbContext _context;

        public UserRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<UserVM> All()
        {
            IEnumerable<UserVM> users =
            _context.Users.Select(u => new UserVM() { Email = u.Email });

            return users;
        }
    }
}