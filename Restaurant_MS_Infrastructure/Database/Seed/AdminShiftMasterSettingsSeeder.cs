using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Database.Seed
{
    public static class AdminShiftMasterSettingsSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.AdminShiftMasterSettings.Any())
            {
                var shiftSetting = new AdminShiftMasterSetting
                {
                    AdminShiftMasterSettingId = 1,
                    ShiftsEnabled = false,   // 0 = false
                    EnforceLogout = false    // 0 = false
                };
                context.AdminShiftMasterSettings.Add(shiftSetting);
                Console.WriteLine("AdminShiftMasterSettings seeded successfully!");
            }
            else
            {
                Console.WriteLine("AdminShiftMasterSettings already exist. Skipping.");
            }

        }
    }
}