using Restaurant_MS_Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Database.Seed
{
    public class BatchSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Batches.Any())
            {
                var batch =
                        new Batch
                        {
                            BatchId = 1,
                            BatchName = "Opening Stock",
                            Expiry = null,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            SyncedAt = null,
                            IsActive = true,    // 1 = true
                            IsNew = true,       // 1 = true
                            IsUpdate = false,   // 0 = false
                            IsSynced = false,   // 0 = false
                            UserId = 1
                        };

                var connection = context.Database.GetDbConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Batches ON");

                try
                {
                    context.Batches.Add(batch);
                    context.SaveChanges();
                    Console.WriteLine("Batches seeded successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error seeding ItemTypes: {ex.Message}");
                }
                finally
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Batches OFF");
                }
            }
            else
            {
                Console.WriteLine("Batches already exist. Skipping.");
            }

        }
    }
}
