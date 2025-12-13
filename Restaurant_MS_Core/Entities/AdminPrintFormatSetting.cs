using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class AdminPrintFormatSetting
{
    public int AdminPrintFormatSettingId { get; set; }

    public int HeaderFooterPref { get; set; }

    public int PatientDetailsPref { get; set; }
}
