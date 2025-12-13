using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class Patient
{
    public long PatientId { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Phone2 { get; set; }

    public string? Phone3 { get; set; }

    public int? AgeYears { get; set; }

    public int? AgeMonths { get; set; }

    public int? AgeDays { get; set; }

    public string? CountryCode1 { get; set; }

    public string? CountryCode2 { get; set; }

    public string? CountryCode3 { get; set; }

    public string? Address { get; set; }

    public string? ReferredBy { get; set; }

    public string? ImagePath { get; set; }

    public string? Gender { get; set; }

    public int Status { get; set; }

    public int SMSPreferrence { get; set; }

    public double Dues { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Tag> Tags { get; set; } = new List<Tag>();
}
