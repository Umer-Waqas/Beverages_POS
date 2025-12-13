using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Database.Seed
{
    public static class UsersSeeder
    {
        public static void Seed(AppDbContext context)
        {
            //if (!context.Users.Any())
            //{
            //    var adminRole = context.Roles.First(r => r.Name == "Admin");
            //    var userRole = context.Roles.First(r => r.Name == "User");

            //    var users = new List<User>
            //{
            //    new()
            //    {
            //        Username = "admin",
            //        Email = "admin@example.com",
            //        PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            //        RoleId = adminRole.Id,
            //        IsActive = true,
            //        CreatedAt = DateTime.UtcNow
            //    },
            //    new()
            //    {
            //        Username = "user",
            //        Email = "user@example.com",
            //        PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"),
            //        RoleId = userRole.Id,
            //        IsActive = true,
            //        CreatedAt = DateTime.UtcNow
            //    }
            //};

            //    context.Users.AddRange(users);
            //    context.SaveChanges();
            //}
        }
    }
}
