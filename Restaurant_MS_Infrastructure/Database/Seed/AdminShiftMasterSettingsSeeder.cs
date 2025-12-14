using System;
using System.Collections.Generic;
using System.Data;
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


                var connection = context.Database.GetDbConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT AdminShiftMasterSettings ON");

                try
                {
                    context.AdminShiftMasterSettings.Add(shiftSetting);
                    context.SaveChanges();
                    Console.WriteLine("AdminShiftMasterSettings seeded successfully!");
                }
                catch (Exception ex)
                { }
                finally
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT AdminShiftMasterSettings OFF");
                }

                
                Console.WriteLine("AdminShiftMasterSettings seeded successfully!");
            }
            else
            {
                Console.WriteLine("AdminShiftMasterSettings already exist. Skipping.");
            }

        }
    }
}