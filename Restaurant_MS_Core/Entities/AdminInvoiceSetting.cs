using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class AdminInvoiceSetting
{
    public int AdminInvoiceSettingId { get; set; }

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

    public bool HideDuesAndAdvanceFromInvoiceAndPmntReceipts { get; set; }

    public bool PrintPractiseName { get; set; }

    public bool ShowPharmaPhone { get; set; }

    public bool ShowPharmaAddress { get; set; }

    public string? InvoiceNote { get; set; }

    public bool IsOptionalBatchNo { get; set; }

    public bool AllowBelowCostSale { get; set; }

    public bool IsAskLoginOnInvSave { get; set; }

    public string? PrinterName { get; set; }

    public bool ShowGender { get; set; }

    public bool ShowPhone { get; set; }

    public bool ShowMR { get; set; }

    public bool ShowEmail { get; set; }

    public bool ShowDOB { get; set; }

    public bool ShowAge { get; set; }

    public bool ShowAddress { get; set; }

    public bool ShowBonusQty { get; set; }

    public bool ShowSalesTax { get; set; }

    public int InvoiceLayout { get; set; }

    public int A4_cols_format { get; set; }
}
