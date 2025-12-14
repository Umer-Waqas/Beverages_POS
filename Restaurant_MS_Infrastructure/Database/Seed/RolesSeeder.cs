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
                var adminRole = new List<UserRole>
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
                        //UserId = 1,
                        Users = new List<User>()
                    }
                };

                var accountantRole = new List<UserRole>
                {
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
                        //UserId = 2,
                        Users = new List<User>()

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
                    context.UserRoles.AddRange(adminRole);
                    context.UserRoles.AddRange(accountantRole);

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

                var adminUsers = new List<User>
                {
                    new User
                    {
                        UserId = 1,
                        UserName = "gigakeys",
                        Password = "admin1122",
                        Phone = "3222222222",
                        Email = "admin@gigakeyssol.pk",
                        AdminShiftSettingId = null,
                        CanGiveDiscount = true,  // 1 = true
                        DiscLimit = 100,
                        CreatedAt = DateTime.Parse("2021-12-10 06:15:43.800"),  // Approximated from "15:43.8"
                        UpdatedAt = DateTime.Parse("2021-12-10 06:15:43.800"),  // Approximated from "15:43.8"
                        SyncedAt = null,
                        IsActive = true,    // 1 = true
                        IsNew = true,       // 1 = true
                        IsUpdate = false,   // 0 = false
                        IsSynced = false,   // 0 = false
                        CreatedbyId = 0,
                        UserRoles = adminRole
                    }
                };

                var accountantUsers = new List<User>
                {
                   new User
                    {
                        UserId = 2,
                        UserName = "cashier",
                        Password = "cashier@00",
                        Phone = "",  // Empty string
                        Email = "cashier@gigakeyssol.pk",
                        AdminShiftSettingId = null,
                        CanGiveDiscount = false,  // 0 = false
                        DiscLimit = 0,
                        CreatedAt = DateTime.Parse("2021-12-10 06:17:40.400"),  // Approximated from "17:40.4"
                        UpdatedAt = DateTime.Parse("2021-12-10 06:17:40.400"),  // Approximated from "17:40.4"
                        SyncedAt = null,
                        IsActive = true,    // 1 = true
                        IsNew = true,       // 1 = true
                        IsUpdate = false,   // 0 = false
                        IsSynced = false,   // 0 = false
                        CreatedbyId = 0,
                        UserRoles = accountantRole
                    }
                };

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Users ON");

                try
                {
                    context.Users.AddRange(adminUsers);
                    context.Users.AddRange(accountantUsers);
                    context.SaveChanges();
                    Console.WriteLine("Users seeded successfully!");
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Users OFF");
                }
                Console.WriteLine("Users seeded successfully!");
            }
            else
            {
                Console.WriteLine("UserRoles already exist. Skipping.");
            }
        }
    }
}