using System;
using System.Collections.Generic;

namespace Restaurant_MS_Core.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int? AdminShiftSettingId { get; set; }

    public bool CanGiveDiscount { get; set; }

    public double DiscLimit { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? SyncedAt { get; set; }

    public bool IsActive { get; set; }

    public bool IsNew { get; set; }

    public bool IsUpdate { get; set; }

    public bool IsSynced { get; set; }

    public int CreatedbyId { get; set; }

    public virtual ICollection<AccTransaction> AccTransactions { get; set; } = new List<AccTransaction>();

    public virtual ICollection<AdjustmentItem> AdjustmentItems { get; set; } = new List<AdjustmentItem>();

    public virtual ICollection<Adjustment> Adjustments { get; set; } = new List<Adjustment>();

    public virtual AdminShiftSetting? AdminShiftSetting { get; set; }

    public virtual ICollection<AdminShiftSetting> AdminShiftSettings { get; set; } = new List<AdminShiftSetting>();

    public virtual ICollection<Batch> Batches { get; set; } = new List<Batch>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<ChequeInfo> ChequeInfos { get; set; } = new List<ChequeInfo>();

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual ICollection<DiscountItem> DiscountItems { get; set; } = new List<DiscountItem>();

    public virtual ICollection<ExpenseCategory> ExpenseCategories { get; set; } = new List<ExpenseCategory>();

    public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public virtual ICollection<FlatDiscount> FlatDiscounts { get; set; } = new List<FlatDiscount>();

    public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

    public virtual ICollection<InvoicePayment> InvoicePayments { get; set; } = new List<InvoicePayment>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<ItemType> ItemTypes { get; set; } = new List<ItemType>();

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();

    public virtual ICollection<LoginHistory> LoginHistories { get; set; } = new List<LoginHistory>();

    public virtual ICollection<Manufacturer> Manufacturers { get; set; } = new List<Manufacturer>();

    public virtual ICollection<MissedSale> MissedSales { get; set; } = new List<MissedSale>();

    public virtual ICollection<POSClosing> POSClosings { get; set; } = new List<POSClosing>();

    public virtual ICollection<POSReceivedCash> POSReceivedCashes { get; set; } = new List<POSReceivedCash>();

    public virtual ICollection<POSSkimmedCash> POSSkimmedCashes { get; set; } = new List<POSSkimmedCash>();

    public virtual ICollection<Patient> Patients { get; set; } = new List<Patient>();

    public virtual ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();

    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<Rack> Racks { get; set; } = new List<Rack>();

    public virtual ICollection<StockAuditDetail> StockAuditDetails { get; set; } = new List<StockAuditDetail>();

    public virtual ICollection<StockAudit> StockAudits { get; set; } = new List<StockAudit>();

    public virtual ICollection<StockConsumptionItem> StockConsumptionItems { get; set; } = new List<StockConsumptionItem>();

    public virtual ICollection<StockConsumption> StockConsumptions { get; set; } = new List<StockConsumption>();

    public virtual ICollection<StockItem> StockItems { get; set; } = new List<StockItem>();

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();

    public virtual ICollection<StoreClosing> StoreClosings { get; set; } = new List<StoreClosing>();

    public virtual ICollection<SubDepartment> SubDepartments { get; set; } = new List<SubDepartment>();

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

    public virtual ICollection<TemplateItem> TemplateItems { get; set; } = new List<TemplateItem>();

    public virtual ICollection<Template> Templates { get; set; } = new List<Template>();

    public virtual ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public virtual ICollection<Right> Rights { get; set; } = new List<Right>();
}
