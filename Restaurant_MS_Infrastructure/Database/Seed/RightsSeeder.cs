using Restaurant_MS_Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Database.Seed
{
    public static class RightsSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Rights.Any())
            {


                var rights = new List<Right>
            {
                new Right {RightId = 1, Description = "Create UserRoles" },
                new Right {RightId = 2, Description = "Edit UserRoles" },
                new Right {RightId = 3, Description = "Delete UserRoles" },
                new Right {RightId = 4, Description = "Edit Payment/Invoice Date" },
                new Right {RightId = 5, Description = "View Financial Reports" },
                new Right {RightId = 6, Description = "Delete Patients" },
                new Right {RightId = 7, Description = "Edit Invoice" },
                new Right {RightId = 8, Description = "Refund Payment" },
                new Right {RightId = 9, Description = "Delete Invoice" },
                new Right {RightId = 10, Description = "Add Stock" },
                new Right {RightId = 11, Description = "Edit Stock" },
                new Right {RightId = 12, Description = "View Pharmacy Report" },
                new Right {RightId = 13, Description = "Edit Item" },
                new Right {RightId = 14, Description = "Add Item" },
                new Right {RightId = 15, Description = "Delete Item" },
                new Right {RightId = 16, Description = "Edit Invoice Retail Price" }
            };

                var connection = context.Database.GetDbConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Rights ON");

                try
                {
                    context.Rights.AddRange(rights);
                    context.SaveChanges();
                    Console.WriteLine("Rights seeded successfully!");
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Rights OFF");
                }
            }
            else
            {
                Console.WriteLine("Rights already exist. Skipping.");
            }
        }
    }
}