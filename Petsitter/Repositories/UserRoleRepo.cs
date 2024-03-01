using Microsoft.AspNetCore.Identity;
using Petsitter.Models;
using Petsitter.ViewModels;

namespace Petsitter.Repositories
{
    public class UserRoleRepo
    {
        IServiceProvider serviceProvider;
        PetsitterContext _context;

        public UserRoleRepo(IServiceProvider serviceProvider, PetsitterContext context)
        {
            this.serviceProvider = serviceProvider;
            _context = context;
        }

        // Assign a role to a user.
        public async Task<bool> AddUserRole(string email, string roleName)
        {
            var UserManager = serviceProvider
                .GetRequiredService<UserManager<IdentityUser>>();
            var user = await UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                await UserManager.AddToRoleAsync(user, roleName);
                User updatedUser = _context.Users.Where(u => u.Email == email).FirstOrDefault();
                updatedUser.UserType = roleName;

            }
            return true;
        }

        // Remove role from a user.
        public async Task<bool> RemoveUserRole(string email, string roleName)
        {
            var UserManager = serviceProvider
                .GetRequiredService<UserManager<IdentityUser>>();
            var user = await UserManager.FindByEmailAsync(email);
            if (user != null)
            {
                await UserManager.RemoveFromRoleAsync(user, roleName);
            }
            return true;
        }

        // Get all roles of a specific user.
        public async Task<IEnumerable<RoleVM>> GetUserRoles(string email)
        {
            var UserManager = serviceProvider
                .GetRequiredService<UserManager<IdentityUser>>();
            var user = await UserManager.FindByEmailAsync(email);
            var roles = await UserManager.GetRolesAsync(user);
            List<RoleVM> roleVMObjects = new List<RoleVM>();
            foreach (var item in roles)
            {
                roleVMObjects.Add(new RoleVM() { Id = item, RoleName = item });
            }
            return roleVMObjects;
        }
    }

}