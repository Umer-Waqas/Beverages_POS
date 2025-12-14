using Restaurant_MS_Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Database.Seed
{
    public static class RolesSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.UserRoles.Any())
            {
                var userRoles = new List<UserRole>
                {
                    new UserRole
                    {
                        UserRoleId = 1,
                        Description = "Admin",
                        IsAdmin = true,  // 1 = true
                        CreatedAt = new DateTime(2021, 12, 10, 6, 15, 43, 727),
                        UpdatedAt = new DateTime(2021, 12, 10, 6, 15, 43, 727),
                        SyncedAt = null,
                        IsActive = true,    // 1 = true
                        IsNew = true,       // 1 = true
                        IsUpdate = false,   // 0 = false
                        IsSynced = false,   // 0 = false
                        UserId = 1
                    },
                    new UserRole
                    {
                        UserRoleId = 2,
                        Description = "Accountant",
                        IsAdmin = false,  // 0 = false
                        CreatedAt = new DateTime(2021, 12, 10, 6, 15, 43, 727),
                        UpdatedAt = new DateTime(2021, 12, 10, 6, 15, 43, 727),
                        SyncedAt = null,
                        IsActive = true,    // 1 = true
                        IsNew = false,      // 0 = false
                        IsUpdate = false,   // 0 = false
                        IsSynced = false,   // 0 = false
                        UserId = 1
                    }
                };


                var connection = context.Database.GetDbConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT UserRoles ON");

                try
                {
                    context.UserRoles.AddRange(userRoles);
                    context.SaveChanges();
                    Console.WriteLine("Users seeded successfully!");
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT UserRoles OFF");
                }
                Console.WriteLine("UserRoles seeded successfully!");
            }
            else
            {
                Console.WriteLine("UserRoles already exist. Skipping.");
            }
        }
    }
}