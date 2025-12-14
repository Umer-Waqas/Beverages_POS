using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Database.Seed
{
    public static class AdminPractiseSettingsSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.AdminPractiseSettings.Any())
            {
                var practiseSetting = new AdminPractiseSetting
                {
                    AdminPractiseSettingId = 1,
                    Name = "FOOD BENCH",
                    Phone = "0309-6763 166 / 0318-4012 662",
                    Address = "CANTT VIEW, OPP WARIS TOWN PHASE-1, PAF/47 PULL LINK ROAD, SARGODHA",
                    LogoPath = @"d:\programming\giga keys solutions\gk_restaurant\gk_restaurant\pharmacy.ui\bin\debug\images\202511301803138075.jpg",
                    PracticeId = -1,
                    UserDeletedDefaultData = false,  // 0 = false
                    Backgroundpath = "",             // Empty string for NULL/empty
                    FBR_POSID = null,                // NULL
                    FBR_AccessCode = null,           // NULL
                    Enable_FBR = null,               // NULL
                    FBR_Enabled = "McjjWRFrGes=",    // Encrypted string
                    PracticeType = "POS"
                };


                var connection = context.Database.GetDbConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT AdminPractiseSettings ON");

                try
                {
                    context.AdminPractiseSettings.Add(practiseSetting);
                    context.SaveChanges();
                    Console.WriteLine("AdminPractiseSettings seeded successfully!");
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT AdminPractiseSettings OFF");
                }
                Console.WriteLine("AdminPractiseSettings seeded successfully!");
            }
            else
            {
                Console.WriteLine("AdminPractiseSettings already exist. Skipping.");
            }

        }
    }
}
