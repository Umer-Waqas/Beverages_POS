using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class AdminProcedureInvoiceSetting
{
    public int AdminProcedureInvoiceSettingId { get; set; }

    public int PrintPageSize { get; set; }

    public int MarginTop { get; set; }

    public int MarginRight { get; set; }

    public int MarginBottom { get; set; }

    public int MarginLeft { get; set; }

    public int FontSize { get; set; }

    public int PageType { get; set; }

    public int PageOrientation { get; set; }

    public bool ShowUserName { get; set; }

    public bool ShowLogoWaterMark { get; set; }

    public bool HideRateAndQuantityInPrintFormat { get; set; }

    public bool ShowGrandtotalsInWords { get; set; }

    public bool GrandTotalsOfInvoiceAsPaymentByDefault { get; set; }

    public bool HidePaymentsAndDuesFromInvoiceAndPmntReceipts { get; set; }

    public bool PrintPractiseName { get; set; }

    public string? InvoiceNote { get; set; }
}
