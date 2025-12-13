using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Restaurant_MS_Core.Entities;

namespace Restaurant_MS_Infrastructure.Database;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccTransaction> AccTransactions { get; set; }

    public virtual DbSet<Adjustment> Adjustments { get; set; }

    public virtual DbSet<AdjustmentItem> AdjustmentItems { get; set; }

    public virtual DbSet<AdminInvoiceSetting> AdminInvoiceSettings { get; set; }

    public virtual DbSet<AdminPharmacySetting> AdminPharmacySettings { get; set; }

    public virtual DbSet<AdminPractiseSetting> AdminPractiseSettings { get; set; }

    public virtual DbSet<AdminPrintFormatSetting> AdminPrintFormatSettings { get; set; }

    public virtual DbSet<AdminProcedureInvoiceSetting> AdminProcedureInvoiceSettings { get; set; }

    public virtual DbSet<AdminShiftMasterSetting> AdminShiftMasterSettings { get; set; }

    public virtual DbSet<AdminShiftSetting> AdminShiftSettings { get; set; }

    public virtual DbSet<Batch> Batches { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ChequeInfo> ChequeInfoes { get; set; }

    public virtual DbSet<Deal> Deals { get; set; }

    public virtual DbSet<DealItem> DealItems { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DiscountItem> DiscountItems { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<ExpenseCategory> ExpenseCategories { get; set; }

    public virtual DbSet<FlatDiscount> FlatDiscounts { get; set; }

    public virtual DbSet<HwData> HwDatas { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }

    public virtual DbSet<InvoicePayment> InvoicePayments { get; set; }

    public virtual DbSet<Item> Items { get; set; }

    public virtual DbSet<ItemType> ItemTypes { get; set; }

    public virtual DbSet<LoginHistory> LoginHistories { get; set; }

    public virtual DbSet<LowStockMail> LowStockMails { get; set; }

    public virtual DbSet<Manufacturer> Manufacturers { get; set; }

    public virtual DbSet<MissedSale> MissedSales { get; set; }

    public virtual DbSet<POSClosing> POSClosings { get; set; }

    public virtual DbSet<POSReceivedCash> POSReceivedCashes { get; set; }

    public virtual DbSet<POSSkimmedCash> POSSkimmedCashes { get; set; }

    public virtual DbSet<Patient> Patients { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

    public virtual DbSet<Rack> Racks { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<RecipeItem> RecipeItems { get; set; }

    public virtual DbSet<Right> Rights { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<StockAudit> StockAudits { get; set; }

    public virtual DbSet<StockAuditDetail> StockAuditDetails { get; set; }

    public virtual DbSet<StockConsumption> StockConsumptions { get; set; }

    public virtual DbSet<StockConsumptionItem> StockConsumptionItems { get; set; }

    public virtual DbSet<StockItem> StockItems { get; set; }

    public virtual DbSet<StoreClosing> StoreClosings { get; set; }

    public virtual DbSet<SubDepartment> SubDepartments { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<Template> Templates { get; set; }

    public virtual DbSet<TemplateItem> TemplateItems { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPermission> UserPermissions { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<campaign> campaigns { get; set; }

    public virtual DbSet<Appsettings> AppSettings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-BLPH810\\SQLEXPRESS;Database=BevPOS;TrustServerCertificate=True;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccTransaction>(entity =>
        {
            entity.HasKey(e => e.AccTransactionId);

            entity.HasOne(d => d.User)
            .WithMany(p => p.AccTransactions)
            .HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Adjustment>(entity =>
        {
            entity.HasKey(e => e.AdjustmentId);
            entity.HasOne(d => d.User).WithMany(p => p.Adjustments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        });

        modelBuilder.Entity<AdjustmentItem>(entity =>
        {
            entity.HasKey(e => e.AdjustmentItemId);

            entity.HasOne(d => d.Adjustment)
            .WithMany(p => p.AdjustmentItems)
            .HasForeignKey(d => d.AdjustmentId)
            .OnDelete(DeleteBehavior.ClientSetNull);


            entity.HasOne(d => d.Batch).WithMany(p => p.AdjustmentItems)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            entity.HasOne(d => d.Item).WithMany(p => p.AdjustmentItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.AdjustmentItems)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<AdminInvoiceSetting>(entity =>
        {
            entity.HasKey(e => e.AdminInvoiceSettingId);
            entity.Property(e => e.A4_cols_format).HasDefaultValue(1);
        });

        modelBuilder.Entity<AdminPharmacySetting>(entity =>
        {
            entity.HasKey(e => e.AdminPharmacySettingId);



        });

        modelBuilder.Entity<AdminPractiseSetting>(entity =>
        {
            entity.HasKey(e => e.AdminPractiseSettingId);

            entity.Property(e => e.Backgroundpath).HasMaxLength(2000);
            entity.Property(e => e.Enable_FBR)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FBR_AccessCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FBR_Enabled).HasMaxLength(100);
            entity.Property(e => e.FBR_POSID).HasMaxLength(50);
            entity.Property(e => e.PracticeType)
                .HasMaxLength(50)
                .HasDefaultValue("POS");
        });

        modelBuilder.Entity<AdminPrintFormatSetting>(entity =>
        {
            entity.HasKey(e => e.AdminPrintFormatSettingId);
        });

        modelBuilder.Entity<AdminProcedureInvoiceSetting>(entity =>
        {
            entity.HasKey(e => e.AdminProcedureInvoiceSettingId);
        });

        modelBuilder.Entity<AdminShiftMasterSetting>(entity =>
        {
            entity.HasKey(e => e.AdminShiftMasterSettingId);
        });

        modelBuilder.Entity<AdminShiftSetting>(entity =>
        {
            entity.HasKey(e => e.AdminShiftSettingId);

            entity.HasOne(d => d.User).WithMany(p => p.AdminShiftSettings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Batch>(entity =>
        {
            entity.HasKey(e => e.BatchId);

            entity.HasOne(d => d.User).WithMany(p => p.Batches)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId);

            entity.HasOne(d => d.User).WithMany(p => p.Categories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ChequeInfo>(entity =>
        {
            entity.HasKey(e => e.ChequeInfoId);

            entity.HasOne(d => d.User).WithMany(p => p.ChequeInfos)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Deal>(entity =>
        {
            entity.HasKey(e => e.DealId);
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.Item).WithMany(p => p.Deals)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DealItem>(entity =>
        {
            entity.HasKey(e => e.DealItemId);
            entity.Property(e => e.Unit).HasMaxLength(50);

            entity.Property(e => e.Quantity).HasPrecision
            (18, 6);

            entity.HasOne(d => d.Deal).WithMany(p => p.DealItems)
                .HasForeignKey(d => d.DealId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Item).WithMany(p => p.DealItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId);

            entity.HasOne(d => d.User).WithMany(p => p.Departments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<DiscountItem>(entity =>
        {
            entity.HasKey(e => e.DiscountItemId);

            entity.HasOne(d => d.FlatDiscount)
            .WithMany(p => p.DiscountItems)
                .HasForeignKey(d => d.FlatDiscountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.FlatDiscount)
            .WithMany(p => p.DiscountItems)
                .HasForeignKey(d => d.FlatDiscountId);

            entity.HasOne(d => d.Item).WithMany(p => p.DiscountItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.DiscountItems)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId);
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId);

            entity.HasOne(d => d.ExpenseCategory).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.ExpenseCategoryId);

            entity.HasOne(d => d.Stock).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.StockId);

            entity.HasOne(d => d.Supplier).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.SupplierId);

            entity.HasOne(d => d.User).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ExpenseCategory>(entity =>
        {
            entity.HasKey(e => e.ExpenseCategoryId);

            entity.HasOne(d => d.User).WithMany(p => p.ExpenseCategories)
                .HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<FlatDiscount>(entity =>
        {
            entity.HasKey(e => e.FlatDiscountId);

            entity.HasOne(d => d.User).WithMany(p => p.FlatDiscounts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<HwData>(entity =>
        {
            entity.HasKey(e => e.HwDataId);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {

            entity.Property(e => e.FBR_InvoiceNo)
    .HasMaxLength(200)
    .IsUnicode(false);
            entity.Property(e => e.OrderStatus).HasDefaultValue(1);
            entity.Property(e => e.OrderType).HasDefaultValue(1);
            entity.Property(e => e.PaymentMethod).HasDefaultValue(1);
            entity.Property(e => e.PaymentStatus).HasDefaultValue(1);

            entity.HasOne(d => d.Employee).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.EmployeeId);

            entity.HasOne(d => d.Patient).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.PatientId);

            entity.HasOne(d => d.StockConsumption).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.StockConsumptionId);

            entity.HasOne(d => d.User).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasKey(e => e.InvoiceItemId);

            entity.HasOne(d => d.Batch).WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Category).WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => d.CategoryId);

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => (Invoice?)d.Invoice).WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => d.Invoice_InvoiceId);

            entity.HasOne(d => d.Item).WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<InvoicePayment>(entity =>
        {
            entity.HasKey(e => e.InvoicePaymentId);

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoicePayments)
                .HasForeignKey(d => d.InvoiceId);

            entity.HasOne(d => d.User).WithMany(p => p.InvoicePayments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(e => e.ItemId);
            entity.Property(e => e.Barcode).HasMaxLength(600);
            entity.Property(e => e.ItemName).HasMaxLength(600);
            entity.Property(e => e.ItemTypeId).HasDefaultValue(1);

            entity.HasOne(d => d.Category).WithMany(p => p.ItemCategories)
                .HasForeignKey(d => d.CategoryId);

            entity.HasOne(d => d.ItemType).WithMany(p => p.Items)
                .HasForeignKey(d => d.ItemTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Manufacturer).WithMany(p => p.Items)
                .HasForeignKey(d => d.ManufacturerId);

            entity.HasOne(d => d.Rack).WithMany(p => p.Items)
                .HasForeignKey(d => d.RackId);

            entity.HasOne(d => d.SubCategory).WithMany(p => p.ItemSubCategories)
                .HasForeignKey(d => d.SubCategoryId);

            entity.HasOne(d => d.User).WithMany(p => p.Items)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ItemType>(entity =>
        {
            entity.HasKey(e => e.ItemTypeId);

            entity.HasOne(d => d.User).WithMany(p => p.ItemTypes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<LoginHistory>(entity =>
        {
            entity.HasKey(e => e.LoginHistoryId);

            entity.HasOne(d => d.User).WithMany(p => p.LoginHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<LowStockMail>(entity =>
        {
            entity.HasKey(e => e.LowStockMailId);
        });

        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(e => e.ManufacturerId);

            entity.HasOne(d => d.User).WithMany(p => p.Manufacturers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<MissedSale>(entity =>
        {
            entity.HasKey(e => e.MissedSaleId);

            entity.HasOne(d => d.Item).WithMany(p => p.MissedSales)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.User).WithMany(p => p.MissedSales)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<POSClosing>(entity =>
        {
            entity.HasKey(e => e.POSClosingId);

            entity.HasOne(d => d.User).WithMany(p => p.POSClosings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<POSReceivedCash>(entity =>
        {
            entity.HasKey(e => e.POSReceivedCashId);

            entity.HasOne(d => d.User).WithMany(p => p.POSReceivedCashes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<POSSkimmedCash>(entity =>
        {
            entity.HasKey(e => e.POSSkimmedCashId);

            entity.HasOne(d => d.User).WithMany(p => p.POSSkimmedCashes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(e => e.PatientId);

            entity.HasOne(d => d.User).WithMany(p => p.Patients)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.PurchaseOrderId);

            entity.Property(e => e.PurchaseOrderNo).HasPrecision(18, 0);
            entity.Property(e => e.TotalAmount).HasPrecision(18, 2);

            entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.SupplierId);

            entity.HasOne(d => d.User).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PurchaseOrderItem>(entity =>
        {
            entity.HasKey(e => e.PurchaseOrderItemId);

            entity.HasOne(d => d.Item).WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.Property(e => e.PurchaseOrderNo).HasPrecision(18, 0);

            entity.HasOne(d => d.PurchaseOrder)
            .WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.PurchaseOrder)
            .WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(d => d.PurchaseOrderId);

            entity.HasOne(d => d.User).WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Rack>(entity =>
        {
            entity.HasKey(e => e.RackId);

            entity.HasOne(d => d.User).WithMany(p => p.Racks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.RecipeId);
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.Item).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<RecipeItem>(entity =>
        {
            entity.HasKey(e => e.RecipeItemId);
            entity.Property(e => e.Unit).HasMaxLength(50);

            entity.Property(e => e.Quantity).HasPrecision(18, 2);


            entity.HasOne(d => d.Ingredient).WithMany(p => p.RecipeItems)
                .HasForeignKey(d => d.IngredientId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Recipe).WithMany(p => p.RecipeItems)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });



        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.StockId);

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.PurchaseOrderId);

            entity.Property(e => e.DocumentNo).HasPrecision(18, 0);

            entity.HasOne(d => d.Supplier).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.SupplierId);

            entity.HasOne(d => d.User).WithMany(p => p.Stocks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<StockAudit>(entity =>
        {
            entity.HasKey(e => e.StockAuditId);

            entity.HasOne(d => d.User).WithMany(p => p.StockAudits)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<StockAuditDetail>(entity =>
        {
            entity.HasKey(e => e.StockAuditDetailId);

            entity.HasOne(d => d.Item).WithMany(p => p.StockAuditDetails)
                .HasForeignKey(d => d.ItemId);

            entity.HasOne(d => d.StockAudit).WithMany(p => p.StockAuditDetails)
                .HasForeignKey(d => d.StockAuditId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.StockAudit)
            .WithMany(p => p.StockAuditDetails)
                .HasForeignKey(d => d.StockAuditId);

            entity.HasOne(d => d.User).WithMany(p => p.StockAuditDetails)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<StockConsumption>(entity =>
        {
            entity.HasKey(e => e.StockConsumptionId);

            entity.HasOne(d => d.User).WithMany(p => p.StockConsumptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<StockConsumptionItem>(entity =>
        {
            entity.HasKey(e => e.StockConsumptionItemId);

            entity.HasOne(d => d.Batch).WithMany(p => p.StockConsumptionItems)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Item).WithMany(p => p.StockConsumptionItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.StockConsumption).WithMany(p => p.StockConsumptionItems)
                .HasForeignKey(d => d.StockConsumptionId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.StockConsumption).WithMany(p => p.StockConsumptionItems)
                .HasForeignKey(d => d.StockConsumptionId);

            entity.HasOne(d => d.User).WithMany(p => p.StockConsumptionItems)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<StockItem>((Action<Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<StockItem>>)(entity =>
        {
            entity.HasKey(e => e.StockItemId);

            entity.HasOne(d => d.Batch).WithMany(p => p.StockItems)
                .HasForeignKey(d => d.BatchId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Item).WithMany(p => p.StockItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.StockConsumptionItem).WithMany(p => p.StockItems)
                .HasForeignKey(d => d.StockConsumptionItemId);

            entity.HasOne((Expression<Func<StockItem, Stock?>>?)(d => d.Stock)).WithMany(p => p.StockItems)
                .HasForeignKey(d => d.StockId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne((Expression<Func<StockItem, Stock?>>?)(d => (Stock?)d.Stock)).WithMany(p => p.StockItems)
                .HasForeignKey(d => d.StockId);

            entity.HasOne(d => d.User).WithMany(p => p.StockItems)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        }));

        modelBuilder.Entity<StoreClosing>(entity =>
        {
            entity.HasKey(e => e.StoreClosingId);

            entity.HasOne(d => d.User).WithMany(p => p.StoreClosings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<SubDepartment>(entity =>
        {
            entity.HasKey(e => e.SubDepartmentId);

            entity.HasOne(d => d.Department).WithMany(p => p.SubDepartments)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Department).WithMany(p => p.SubDepartments)
                .HasForeignKey(d => d.DepartmentId);

            entity.HasOne(d => d.User).WithMany(p => p.SubDepartments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierID);
            entity.Property(e => e.Name).HasMaxLength(600);

            entity.HasOne(d => d.User).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.TagId);
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.HasKey(e => e.TemplateId);

            entity.HasOne(d => d.User).WithMany(p => p.Templates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<TemplateItem>(entity =>
        {
            entity.HasKey(e => e.TemplateItemId);

            entity.HasOne(d => d.Item).WithMany(p => p.TemplateItems)
                .HasForeignKey(d => d.ItemId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Template).WithMany(p => p.TemplateItems)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Template)
            .WithMany(p => p.TemplateItems)
                .HasForeignKey(d => d.TemplateId);

            entity.HasOne(d => d.User).WithMany(p => p.TemplateItems)
                .HasForeignKey(d => d.UserId);
        });

        // Configure User -> UserRoles (many-to-many)
        modelBuilder.Entity<User>()
            .HasMany(u => u.UserRoles)           // User has many UserRoles
            .WithMany(ur => ur.Users)            // UserRole has many Users
            .UsingEntity<Dictionary<string, object>>(
                "UserUserRole",                   // Join table name
                j => j.HasOne<UserRole>().WithMany().HasForeignKey("UserRoleId"),
                j => j.HasOne<User>().WithMany().HasForeignKey("UserId")
            );

        modelBuilder.Entity<User>()
            .HasOne(d => d.AdminShiftSetting).WithMany(p => p.Users)
            .HasForeignKey(d => d.AdminShiftSettingId);

        //modelBuilder.Entity<UserRole>()
        //.HasKey(ur => new { ur.UserId, ur.RoleId });

        // Configure User -> UserRole relationship
        //modelBuilder.Entity<UserRole>()
        //.HasOne(ur => ur.User)
        //.WithMany(u => u.UserRoles)
        //.HasForeignKey(ur => ur.UserId)
        //.OnDelete(DeleteBehavior.Cascade);

        //// Configure Role -> UserRole relationship
        //modelBuilder.Entity<UserRole>()
        //    .HasOne(ur => ur.Role)
        //    .WithMany(r => r.UserRoles)
        //    .HasForeignKey(ur => ur.RoleId)
        //    .OnDelete(DeleteBehavior.Cascade);



        //modelBuilder.Entity<User>(entity =>
        //{
        //    entity.HasKey(e => e.UserId);
        //    entity.Property(e => e.Email).HasMaxLength(600);


        //    entity.HasMany(u => u.UserRoles)
        //    .WithOne()
        //    .OnDelete(DeleteBehavior.ClientSetNull);

        //    entity.HasOne(d => d.AdminShiftSetting).WithMany(p => p.Users)
        //        .HasForeignKey(d => d.AdminShiftSettingId);
        //});

        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.HasKey(e => e.UserPermissionId);

            entity.HasOne(d => d.User).WithMany(p => p.UserPermissions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        // modelBuilder.Entity<User>()
        //.HasMany(u => u.UserRoles)
        //.WithMany(r => r.Users);

        modelBuilder.Entity<campaign>(entity =>
        {
            entity.HasKey(e => e.CampaignId);
            entity.ToTable("campaign");

            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.SMS).HasMaxLength(3000);
            entity.Property(e => e.Status).HasMaxLength(200);
            entity.Property(e => e.Title).HasMaxLength(200);
        });

        modelBuilder.Entity<Appsettings>(entity =>
        {
            entity.HasKey(e => e.Id);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
