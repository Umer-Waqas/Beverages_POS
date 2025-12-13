using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Database.Seed
{
    public static class HwDatasSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.HwDatas.Any())
            {
                var hwData = new HwData
                {
                    SystemExpiry = "1LDm3PwxE5xJX7g/+A/p4sgWYgeC9TST",
                    IsSyncManual = false   // 0 = false
                };
                context.HwDatas.Add(hwData);
                Console.WriteLine("HwData seeded successfully!");
            }
            else
            {
                Console.WriteLine("HwData already exists. Skipping.");
            }
        }
    }
}