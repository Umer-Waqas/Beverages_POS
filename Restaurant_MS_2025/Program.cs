using Microsoft.EntityFrameworkCore;
using Restaurant_MS_UI;
using Restaurant_MS_UI.App;

namespace Restaurant_MS_2025
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            SetupDatabase();

            Application.Run(new SplashForm());
        }

        private static void SetupDatabase()
        {
            try
            {
                using var context = new AppDbContext();

                // 1. Apply pending migrations (creates/updates database schema)
                context.Database.Migrate();

                // 2. Run seed data
                DatabaseSeeder.Seed(context);

                Console.WriteLine("Database setup completed successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database setup failed: {ex.Message}",
                              "Error",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Error);
                // Optional: Log to file
                File.AppendAllText("startup.log", $"{DateTime.Now}: {ex}\n");
            }
        }

    }
}