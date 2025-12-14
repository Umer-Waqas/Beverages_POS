using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Database.Seed
{
    public class ItemTypeSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.ItemTypes.Any())
            {
                var itemTypes = new List<ItemType>
                    {
                        new ItemType
                        {
                            ItemTypeId = 1,
                            Name = "Sale Item",
                            CreatedAt = DateTime.Parse("2025-12-01 15:43:05.050"),
                            UpdatedAt = DateTime.Parse("2025-12-01 15:43:05.050"),
                            SyncedAt = null,
                            IsActive = true,    // 1 = true
                            IsNew = true,       // 1 = true
                            IsUpdate = false,   // 0 = false
                            IsSynced = false,   // 0 = false
                            UserId = 1
                        },
                        new ItemType
                        {
                            ItemTypeId = 3,
                            Name = "Baked Item",
                            CreatedAt = DateTime.Parse("2025-12-01 15:43:05.050"),
                            UpdatedAt = DateTime.Parse("2025-12-01 15:43:05.050"),
                            SyncedAt = null,
                            IsActive = true,    // 1 = true
                            IsNew = true,       // 1 = true
                            IsUpdate = false,   // 0 = false
                            IsSynced = false,   // 0 = false
                            UserId = 1
                        },
                        new ItemType
                        {
                            ItemTypeId = 4,
                            Name = "Deal Item",
                            CreatedAt = DateTime.Parse("2025-12-01 15:43:05.050"),
                            UpdatedAt = DateTime.Parse("2025-12-01 15:43:05.050"),
                            SyncedAt = null,
                            IsActive = true,    // 1 = true
                            IsNew = true,       // 1 = true
                            IsUpdate = false,   // 0 = false
                            IsSynced = false,   // 0 = false
                            UserId = 1
                        },
                        new ItemType
                        {
                            ItemTypeId = 5,
                            Name = "Raw Item",
                            CreatedAt = DateTime.Parse("2025-12-01 15:43:05.050"),
                            UpdatedAt = DateTime.Parse("2025-12-01 15:43:05.050"),
                            SyncedAt = null,
                            IsActive = true,    // 1 = true
                            IsNew = true,       // 1 = true
                            IsUpdate = false,   // 0 = false
                            IsSynced = false,   // 0 = false
                            UserId = 1
                        }
                    };

                var connection = context.Database.GetDbConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ItemTypes ON");

                try
                {
                    context.ItemTypes.AddRange(itemTypes);
                    context.SaveChanges();
                    Console.WriteLine("ItemTypes seeded successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error seeding ItemTypes: {ex.Message}");
                }
                finally
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT ItemTypes OFF");
                }
            }
            else
            {
                Console.WriteLine("ItemTypes already exist. Skipping.");
            }

        }
    }
}
