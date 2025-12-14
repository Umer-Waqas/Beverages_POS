using Restaurant_MS_Core.Entities;
using Restaurant_MS_Infrastructure.Database.Seed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Database
{
    public class DatabaseSeeder
    {
        public static void Seed(AppDbContext context)
        {
            try
            {
                context.Database.EnsureCreated();
                // Run seeders in order (respect foreign key dependencies)

                UsersSeeder.Seed(context);
                RolesSeeder.Seed(context);
                RightsSeeder.Seed(context);
                AdminInvoiceSettingsSeeder.Seed(context);
                AdminPharmacySettingsSeeder.Seed(context);
                AdminPractiseSettingsSeeder.Seed(context);
                AdminPrintFormatSettingsSeeder.Seed(context);
                AdminProcedureInvoiceSettingsSeeder.Seed(context);
                AdminShiftMasterSettingsSeeder.Seed(context);
                AdminShiftSettingsSeeder.Seed(context);
                ItemTypeSeeder.Seed(context);
                DefaultItemsSeeder.Seed(context);
                HwDatasSeeder.Seed(context);

                Console.WriteLine("Database seeding completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding database: {ex.Message}");
                throw;
            }
        }
    }
}
