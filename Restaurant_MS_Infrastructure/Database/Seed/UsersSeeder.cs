using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Restaurant_MS_Infrastructure.Database.Seed
{
    public static class UsersSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Users.Any())
            {
                var users = new List<User>
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
                        CreatedbyId = 0
                    },
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
                        CreatedbyId = 0
                    }
                };

                var connection = context.Database.GetDbConnection();
                var wasClosed = connection.State == ConnectionState.Closed;

                if (wasClosed)
                {
                    connection.Open();
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Users ON");

                //using var transaction = connection.BeginTransaction();
                //context.Database.UseTransaction(transaction);

                // Step 1: Turn ON IDENTITY_INSERT using the SAME connection
                //using (var cmd = connection.CreateCommand())
                //{
                //    cmd.Transaction = transaction;
                //    cmd.CommandText = "SET IDENTITY_INSERT Users ON";
                //    cmd.ExecuteNonQuery();
                //    Console.WriteLine("IDENTITY_INSERT ON executed successfully.");
                //}

                try
                {
                    context.Users.AddRange(users);
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

                Console.WriteLine("UserRoles seeded successfully!");
            }
            else
            {
                Console.WriteLine("UserRoles already exist. Skipping.");
            }
        }
    }
}
