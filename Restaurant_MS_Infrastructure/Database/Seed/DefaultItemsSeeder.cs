using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Database.Seed
{
    public static class DefaultItemsSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Items.Any())
            {
                var item = new Item
                {
                    ItemId = 1,
                    ItemName = "default",
                    IsRawItem = null,
                    Barcode = "default",
                    RackId = null,
                    Unit = "",  // Empty string
                    ConversionUnit = 0,
                    ReOrderingLevel = 0,
                    RetailPrice = 0,
                    OpeningStock = 0,
                    ChecmicalName = "",  // Empty string (note typo in column name)
                    UnitCostPrice = 0,
                    IsNarcotic = false,  // 0 = false
                    MappedCategoryId = 0,
                    MappedProductId = 0,
                    IsDefault = true,    // 1 = true
                    IsDefaultLoaded = false,  // 0 = false
                    IsSyncable = false,  // 0 = false
                    CategoryId = null,
                    SubCategoryId = null,
                    ManufacturerId = null,
                    CurrentQuantity = 0,
                    CurrentRetailPrice = 0,
                    CurrentCostPrice = 0,
                    CreatedAt = DateTime.Parse("2021-12-10 06:15:43.800"),  // Approximated from "15:43.8"
                    UpdatedAt = DateTime.Parse("2021-12-10 06:15:43.800"),  // Approximated from "15:43.8"
                    SyncedAt = null,
                    IsActive = true,     // 1 = true
                    IsNew = false,       // 0 = false
                    IsUpdate = false,    // 0 = false
                    IsSynced = false,    // 0 = false
                    UserId = 1,
                    CheckStockOnSale = null,
                    ItemTypeId = 1
                };
                context.Items.Add(item);
                Console.WriteLine("Items seeded successfully!");
            }
            else
            {
                Console.WriteLine("Items already exist. Skipping.");
            }
        }
    }
}