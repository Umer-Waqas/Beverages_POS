using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Database.Seed
{
    public static class AdminShiftSettingsSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.AdminShiftSettings.Any())
            {
                var shiftSettings = new List<AdminShiftSetting>
            {
                new AdminShiftSetting
                {
                    AdminShiftSettingId = 1,
                    Name = "Morning",
                    Code = 1,
                    StartTime = new DateTime(2021, 1, 15, 0, 0, 0, 0),
                    EndTime = new DateTime(2021, 1, 15, 0, 0, 0, 0),
                    CreatedAt = new DateTime(2021, 12, 10, 6, 15, 43, 903),
                    UpdatedAt = new DateTime(2021, 12, 10, 6, 15, 43, 903),
                    SyncedAt = null,
                    IsActive = true,    // 1 = true
                    IsNew = true,       // 1 = true
                    IsUpdate = false,   // 0 = false
                    IsSynced = false,   // 0 = false
                    UserId = 1
                },
                new AdminShiftSetting
                {
                    AdminShiftSettingId = 2,
                    Name = "Afternoon",
                    Code = 2,
                    StartTime = new DateTime(2021, 1, 15, 0, 0, 0, 0),
                    EndTime = new DateTime(2021, 1, 15, 0, 0, 0, 0),
                    CreatedAt = new DateTime(2021, 12, 10, 6, 15, 43, 920),
                    UpdatedAt = new DateTime(2021, 12, 10, 6, 15, 43, 920),
                    SyncedAt = null,
                    IsActive = true,    // 1 = true
                    IsNew = true,       // 1 = true
                    IsUpdate = false,   // 0 = false
                    IsSynced = false,   // 0 = false
                    UserId = 1
                },
                new AdminShiftSetting
                {
                    AdminShiftSettingId = 3,
                    Name = "Evening",
                    Code = 3,
                    StartTime = new DateTime(2021, 1, 15, 0, 0, 0, 0),
                    EndTime = new DateTime(2021, 1, 15, 0, 0, 0, 0),
                    CreatedAt = new DateTime(2021, 12, 10, 6, 15, 43, 920),
                    UpdatedAt = new DateTime(2021, 12, 10, 6, 15, 43, 920),
                    SyncedAt = null,
                    IsActive = true,    // 1 = true
                    IsNew = true,       // 1 = true
                    IsUpdate = false,   // 0 = false
                    IsSynced = false,   // 0 = false
                    UserId = 1
                },
                new AdminShiftSetting
                {
                    AdminShiftSettingId = 4,
                    Name = "Night",
                    Code = 4,
                    StartTime = new DateTime(2021, 1, 15, 0, 0, 0, 0),
                    EndTime = new DateTime(2021, 1, 15, 0, 0, 0, 0),
                    CreatedAt = new DateTime(2021, 12, 10, 6, 15, 43, 923),
                    UpdatedAt = new DateTime(2021, 12, 10, 6, 15, 43, 923),
                    SyncedAt = null,
                    IsActive = true,    // 1 = true
                    IsNew = true,       // 1 = true
                    IsUpdate = false,   // 0 = false
                    IsSynced = false,   // 0 = false
                    UserId = 1
                }
            };

                context.AdminShiftSettings.AddRange(shiftSettings);
                Console.WriteLine("AdminShiftSettings seeded successfully!");
            }
            else
            {
                Console.WriteLine("AdminShiftSettings already exist. Skipping.");
            }

        }
    }
}