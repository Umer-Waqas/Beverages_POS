using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Database.Seed
{
    public static class AdminPrintFormatSettingsSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.AdminPrintFormatSettings.Any())
            {
                var printFormatSetting = new AdminPrintFormatSetting
                {
                    AdminPrintFormatSettingId = 1,
                    HeaderFooterPref = 1,    // Integer value
                    PatientDetailsPref = 1   // Integer value
                };


                var connection = context.Database.GetDbConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT AdminPrintFormatSettings ON");

                try
                {
                    context.AdminPrintFormatSettings.Add(printFormatSetting);
                    context.SaveChanges();
                    Console.WriteLine("AdminPrintFormatSettings seeded successfully!");
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT AdminPrintFormatSettings OFF");
                }
                Console.WriteLine("AdminPrintFormatSettings seeded successfully!");


                
                Console.WriteLine("AdminPrintFormatSettings seeded successfully!");
            }
        }
    }
}