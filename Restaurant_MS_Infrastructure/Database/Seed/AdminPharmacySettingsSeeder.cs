using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Database.Seed
{
    public static class AdminPharmacySettingsSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.AdminPharmacySettings.Any())
            {
                var pharmacySetting = new AdminPharmacySetting
                {
                    AdminPharmacySettingId = 1,
                    IsItemConumptionFifo = true,           // 1 = true
                    AllowNegCons = true,                   // 1 = true
                    IsPharmacyTemps = false,               // 0 = false
                    IsHolsStockOnInvoiceHold = false,      // 0 = false
                    IsUseNewestStockPrice = false,         // 0 = false
                    IsItemDefUnitOnPOS = true,             // 1 = true
                    ExpiryPeriod = 0,
                    ExpiryPeriodUnit = 0,
                    ShowRackNoInPOS = false,               // 0 = false
                    UseDafaultStoreClosingSetting = false, // 0 = false
                    EnforceDayClose = false,               // 0 = false
                    DayOpenTime = new DateTime(2025, 01, 01, 6, 0, 0),   // 06:00:00
                    DayCloseTime = new DateTime(2025, 01, 01, 2, 0, 0),  // 02:00:00 (next day)
                    EnableDayClose = false,                // 0 = false
                    ShowBatchNoOnPOS = false,              // 0 = false
                    AllowBatchEntryOnAddStock = false,     // 0 = false
                    EnableFBRIntegration = false,          // 0 = false
                    OrderWaningAlertTime = 1               // 1 = true (assuming boolean)
                };
                context.AdminPharmacySettings.Add(pharmacySetting);
                //context.SaveChanges();
                Console.WriteLine("AdminPharmacySettings seeded successfully!");
            }
            else
            {
                Console.WriteLine("AdminPharmacySettings already exist. Skipping.");
            }

        }
    }
}