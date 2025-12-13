using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class AdminPharmacySetting
{
    public int AdminPharmacySettingId { get; set; }

    public bool IsItemConumptionFifo { get; set; }

    public bool AllowNegCons { get; set; }

    public bool IsPharmacyTemps { get; set; }

    public bool IsHolsStockOnInvoiceHold { get; set; }

    public bool IsUseNewestStockPrice { get; set; }

    public bool IsItemDefUnitOnPOS { get; set; }

    public int ExpiryPeriod { get; set; }

    public int ExpiryPeriodUnit { get; set; }

    public bool ShowRackNoInPOS { get; set; }

    public bool UseDafaultStoreClosingSetting { get; set; }

    public bool EnforceDayClose { get; set; }

    public DateTime? DayOpenTime { get; set; }

    public DateTime? DayCloseTime { get; set; }

    public bool EnableDayClose { get; set; }

    public bool ShowBatchNoOnPOS { get; set; }

    public bool AllowBatchEntryOnAddStock { get; set; }

    public bool EnableFBRIntegration { get; set; }

    public int? OrderWaningAlertTime { get; set; }
}
