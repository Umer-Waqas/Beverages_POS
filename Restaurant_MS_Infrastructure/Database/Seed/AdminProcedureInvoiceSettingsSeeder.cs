using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Database.Seed
{
    public static class AdminProcedureInvoiceSettingsSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.AdminProcedureInvoiceSettings.Any())
            {
                var procedureInvoiceSetting = new AdminProcedureInvoiceSetting
                {
                    AdminProcedureInvoiceSettingId = 1,
                    PrintPageSize = 1,
                    MarginTop = 0,
                    MarginRight = 0,
                    MarginBottom = 0,
                    MarginLeft = 0,
                    FontSize = 11,
                    PageType = 1,
                    PageOrientation = 1,
                    ShowUserName = false,
                    ShowLogoWaterMark = false,
                    HideRateAndQuantityInPrintFormat = false,
                    ShowGrandtotalsInWords = false,
                    GrandTotalsOfInvoiceAsPaymentByDefault = false,
                    HidePaymentsAndDuesFromInvoiceAndPmntReceipts = false,
                    PrintPractiseName = false,
                    InvoiceNote = string.Empty  // Empty string instead of null
                };


                var connection = context.Database.GetDbConnection();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT AdminProcedureInvoiceSettings ON");

                try
                {
                    context.AdminProcedureInvoiceSettings.Add(procedureInvoiceSetting);
                    context.SaveChanges();
                    Console.WriteLine("AdminProcedureInvoiceSettings seeded successfully!");
                }
                catch (Exception ex)
                { }
                finally
                {
                    context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT AdminProcedureInvoiceSettings OFF");
                }
                Console.WriteLine("AdminProcedureInvoiceSettings seeded successfully!");
            }
        }
    }
}