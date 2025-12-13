using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class AdminShiftMasterSetting
{
    public int AdminShiftMasterSettingId { get; set; }

    public bool ShiftsEnabled { get; set; }

    public bool EnforceLogout { get; set; }
}
