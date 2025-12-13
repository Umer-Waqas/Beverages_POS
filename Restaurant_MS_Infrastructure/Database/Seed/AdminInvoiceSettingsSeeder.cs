using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure.Database.Seed
{
    public static class AdminInvoiceSettingsSeeder
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.AdminInvoiceSettings.Any())
            {
                var invoiceSetting = new AdminInvoiceSetting
                {
                    AdminInvoiceSettingId = 1,
                    PrintPageSize = 2,
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
                    GrandTotalsOfInvoiceAsPaymentByDefault = true,
                    HideDuesAndAdvanceFromInvoiceAndPmntReceipts = true,
                    PrintPractiseName = true,
                    ShowPharmaPhone = true,
                    ShowPharmaAddress = true,
                    InvoiceNote = "*No return/refund for delivered or baked items.  *Minimum delivery order 600 within 5 KM range from point, extra amount will be charged for order with distance more than 5KM  *For complains & suggestions call at(0321-4880099)    FOOD BENCH IS COMMITED TO DELIVER HYGIENIC, FAST AND FRESH FOODS TO OUR CUSTOMERS.",
                    IsOptionalBatchNo = false,
                    AllowBelowCostSale = false,
                    IsAskLoginOnInvSave = false,
                    PrinterName = "Microsoft Print to PDF",
                    ShowGender = false,
                    ShowPhone = false,
                    ShowMR = false,
                    ShowEmail = false,
                    ShowDOB = false,
                    ShowAge = false,
                    ShowAddress = false,
                    ShowBonusQty = false,
                    ShowSalesTax = true,
                    InvoiceLayout = 1,
                    A4_cols_format = 1
                };

                context.AdminInvoiceSettings.Add(invoiceSetting);

                Console.WriteLine("AdminInvoiceSettings seeded successfully!");
            }
            else
            {
                // Update existing record if needed
                //var existingSetting = context.AdminInvoiceSettings
                //    .FirstOrDefault(a => a.AdminInvoiceSettingId == 1);

                //if (existingSetting != null)
                //{
                //    // Optionally update specific fields if needed
                //    // For example, if you want to ensure certain defaults:
                //    if (string.IsNullOrEmpty(existingSetting.PrinterName))
                //    {
                //        existingSetting.PrinterName = "Microsoft Print to PDF";
                //        context.SaveChanges();
                //        Console.WriteLine("AdminInvoiceSettings updated!");
                //    }
                //}
            }
        }
    }
}