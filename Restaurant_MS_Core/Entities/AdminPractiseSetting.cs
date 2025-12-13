using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class AdminPractiseSetting
{
    public long AdminPractiseSettingId { get; set; }

    public string? Name { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? LogoPath { get; set; }

    public int PracticeId { get; set; }

    public bool UserDeletedDefaultData { get; set; }

    public string? Backgroundpath { get; set; }

    public string? FBR_POSID { get; set; }

    public string? FBR_AccessCode { get; set; }

    public string? Enable_FBR { get; set; }

    public string? FBR_Enabled { get; set; }

    public string PracticeType { get; set; } = null!;
}
