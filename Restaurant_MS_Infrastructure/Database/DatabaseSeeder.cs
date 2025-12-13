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
                AdminPractiseSettingsSeeder.Seed(context);

                //Add another seeder just above this line

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
