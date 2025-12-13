

using Pharmacy.Core.ViewModels;
namespace Restaurant_MS_Infrastructure.Repository
{
    public class UserRoleRepository : Repository<UserRole>
    {
        AppDbContext cxt;
        public UserRoleRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }
        public List<UserRole> GetAllRolesWithUsersAndRights(bool IsSuperAdminLoggedIn)
        {
            if (IsSuperAdminLoggedIn)
            {
                return cxt.UserRoles
                    .Where(r => r.IsActive)
                    .Include(r => r.Users.Select(u => u.Rights))
                    .OrderByDescending(r => r.UserRoleId).Where(r => r.Users.Any(u => u.IsActive)).ToList();
            }
            else
            {
                return cxt.UserRoles
                    .Where(r => r.IsActive)
                    .Where(r => !r.IsAdmin)
                    .Include(r => r.Users.Select(u => u.Rights))
                    .OrderByDescending(r => r.UserRoleId).Where(r => r.Users.Any(u => u.IsActive)).ToList();
            }
        }

        public List<getUsersVM> getAllUsers()
        {
            return cxt.Users
                   .Where(u => u.IsActive)
                   .Select(u => new getUsersVM
                   {
                       userId = u.UserId,
                       userName = u.UserName,
                       email = u.Email,
                       phone = u.Phone,
                       //UserRole = u.UserRoles1.FirstOrDefault()
                   })
                   .OrderByDescending(u => u.userId)
                   .ToList();
        }

        public List<UserRole> GetNonAdminUserRoles()
        {
            return cxt.UserRoles.Where(r => !r.IsAdmin).ToList();
        }

        public List<UserRole> GetAdminOnlyUserRole()
        {
            return cxt.UserRoles.Where(r => r.IsAdmin).ToList();
        }

        public List<UserRole> GetActiveUserRoles()
        {
            return cxt.UserRoles.Where(r => r.IsActive).ToList();
        }
    }
}