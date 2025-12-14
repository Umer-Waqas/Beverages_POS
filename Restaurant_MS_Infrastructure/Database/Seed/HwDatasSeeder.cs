using Restaurant_MS_Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
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
                    HwDataId = 1,
                    SystemExpiry = "1LDm3PwxE5xJX7g/+A/p4sgWYgeC9TST",
                    IsSyncManual = false   // 0 = false
                };

                var connection = context.Database.GetDbConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HwDatas ON");

                try
                {
                    context.HwDatas.Add(hwData);
                    context.SaveChanges();
                    Console.WriteLine("HwDatas seeded successfully!");
                }
                catch (Exception ex)
                { }
                finally
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT HwDatas OFF");
                }

                Console.WriteLine("HwData seeded successfully!");
            }
            else
            {
                Console.WriteLine("HwData already exists. Skipping.");
            }
        }
    }
}