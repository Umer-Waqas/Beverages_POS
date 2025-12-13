using System;
using System.Collections.Generic;
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

                context.AdminPrintFormatSettings.Add(printFormatSetting);
                Console.WriteLine("AdminPrintFormatSettings seeded successfully!");
            }
        }
    }
}