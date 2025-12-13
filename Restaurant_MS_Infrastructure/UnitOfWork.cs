

using Restaurant_MS_Infrastructure.Repository;

namespace Restaurant_MS_Infrastructure
{
    public class UnitOfWork : IDisposable
    {
        private bool disposed = false;
        AppDbContext cxt = new AppDbContext();
        //AppDbContext cxt;

        //public UnitOfWork()
        //{

        //}

        private ItemsRepository _ItemRspository;
        private SupplierRepository _SupplierRepository;
        private PurchaseOrderRepository _PurchaseOrderRepository;
        private PurchaseOrderItemsRepository _PurchaseOrderItemsRepository;
        private BatchRepository _BatchRepository;
        private StockRepository _StockRepository;
        private PatientRepository _PatientRepository;
        private TagsRepository _TagsRepository;
        private InvoiceRepository _InvoiceRepository;
        private ChequeInfoRepository _ChequeInfoRepository;
        private StockConsumptionRepository _StockConsumptionRepository;
        private AdjustmentRepository _AdjustmentRepository;
        private UserRepository _UserRepository;
        private UserRoleRepository _UserRoleRepository;
        private RightRepository _RightRepository;
        private GeneralRepository _GeneralRepository;
        private AdminInvoiceSettingRepository _AdminInvoiceSettingRepository;
        private AdminProcedureInvoiceSettingRepository _AdminProcedureInvoiceSettingRepository;
        private AdminPractiseSettingRepository _AdminPractiseSettingRepository;
        private AdminPrintFormatSettingRepository _AdminPrintFormatSettingRepository;
        private DepratmentRepository _DepratmentRepository;
        private SubDepartmentRepository _SubDepartmentRepository;
        private LowStockMailRepository _LowStockMailRepository;
        private CategoryRepository _CategoryRepository;
        private ExpenseRepository _ExpenseRepository;
        private ExpenseCategoryRepository _ExpenseCategoryRepository;
        private ManufacturerRepository _ManufacturerRepository;
        private HwDataRepository _HwDataRepository;
        private TemplateRepository _TemplateRepository;
        private StockAuditRepository _StockAuditRepository;
        private RackRepository _RackRepository;
        private AdminPharmacySettingRepository _AdminPharmacySettingRepository;
        private FlatDiscountRepository _FlatDiscountRepository;
        private MissedsalesRepository _MissedsalesRepository;
        private AdminShiftMasterSettingRepository _AdminShiftMasterSettingRepository;
        private AdminShiftsSettingRepository _AdminShiftsSettingRepository;
        private POSCashReceiveRepository _POSCashReceiveRepository;
        private POSCashSkimmedRepository _POSCashSkimmedRepository;
        private LoginHistoryRepository _LoginHistoryRepository;
        private POSClosingRepository _POSClosingRepository;
        private AccTransactionsRepository _AccTransactionsRepository;
        private StoreClosingRepository _StoreClosingRepository;
        private SeatingTableRepository _SeatingTableRepository;
        private EmployeeRepository _EmployeeRepository;
        private ItemTypeRepository _ItemTypeRepository;
        private RecipeRepository _RecipeRepository;
        private AppSettingsRepository _AppSettingsRepository;



        public ItemsRepository ItemRspository
        {
            get
            {
                if (_ItemRspository == null)
                {
                    _ItemRspository = new ItemsRepository(cxt);
                }
                return _ItemRspository;
            }
        }

        public SupplierRepository SupplierRepository
        {
            get
            {
                if (_SupplierRepository == null)
                {
                    _SupplierRepository = new SupplierRepository(cxt);
                }
                return _SupplierRepository;
            }
        }
        public PurchaseOrderRepository PurchaseOrderRepository
        {
            get
            {
                if (_PurchaseOrderRepository == null)
                {
                    _PurchaseOrderRepository = new PurchaseOrderRepository(cxt);
                }
                return _PurchaseOrderRepository;
            }
        }
        public PurchaseOrderItemsRepository PurchaseOrderItemsRepository
        {
            get
            {
                if (_PurchaseOrderItemsRepository == null)
                {
                    _PurchaseOrderItemsRepository = new PurchaseOrderItemsRepository(cxt);
                }
                return _PurchaseOrderItemsRepository;
            }
        }
        public BatchRepository BatchRepository
        {
            get
            {
                if (_BatchRepository == null)
                {
                    _BatchRepository = new BatchRepository(cxt);
                }
                return _BatchRepository;
            }
        }

        public StockRepository StockRepository
        {
            get
            {
                if (_StockRepository == null)
                {
                    _StockRepository = new StockRepository(cxt);
                }
                return _StockRepository;
            }
        }
        public PatientRepository PatientRepository
        {
            get
            {
                if (_PatientRepository == null)
                {
                    _PatientRepository = new PatientRepository(cxt);
                }
                return _PatientRepository;
            }
        }
        public TagsRepository TagsRepository
        {
            get
            {
                if (_TagsRepository == null)
                {
                    _TagsRepository = new TagsRepository(cxt);
                }
                return _TagsRepository;
            }
        }

        public InvoiceRepository InvoiceRepository
        {
            get
            {
                if (_InvoiceRepository == null)
                {
                    _InvoiceRepository = new InvoiceRepository(cxt);
                }
                return _InvoiceRepository;
            }
        }

        public ChequeInfoRepository ChequeInfoRepository
        {
            get
            {
                if (_ChequeInfoRepository == null)
                {
                    _ChequeInfoRepository = new ChequeInfoRepository(cxt);
                }
                return _ChequeInfoRepository;
            }
        }
        public AdjustmentRepository AdjustmentRepository
        {
            get
            {
                if (_AdjustmentRepository == null)
                {
                    _AdjustmentRepository = new AdjustmentRepository(cxt);
                }
                return _AdjustmentRepository;
            }
        }

        public UserRepository UserRepository
        {
            get
            {
                if (_UserRepository == null)
                {
                    _UserRepository = new UserRepository(cxt);
                }
                return _UserRepository;
            }
        }

        public UserRoleRepository UserRoleRepository
        {
            get
            {
                if (_UserRoleRepository == null)
                {
                    _UserRoleRepository = new UserRoleRepository(cxt);
                }
                return _UserRoleRepository;
            }
        }
        public RightRepository RightRepository
        {
            get
            {
                if (_RightRepository == null)
                {
                    _RightRepository = new RightRepository(cxt);
                }
                return _RightRepository;
            }
        }

        public GeneralRepository GeneralRepository
        {
            get
            {
                if (_GeneralRepository == null)
                {
                    _GeneralRepository = new GeneralRepository(cxt);
                }
                return _GeneralRepository;
            }
        }

        public AdminInvoiceSettingRepository AdminInvoiceSettingRepository
        {
            get
            {
                if (_AdminInvoiceSettingRepository == null)
                {
                    _AdminInvoiceSettingRepository = new AdminInvoiceSettingRepository(cxt);
                }
                return _AdminInvoiceSettingRepository;
            }
        }


        public StockConsumptionRepository StockConsumptionRepository
        {
            get
            {
                if (_StockConsumptionRepository == null)
                {
                    _StockConsumptionRepository = new StockConsumptionRepository(cxt);
                }
                return _StockConsumptionRepository;
            }
        }

        public AdminPractiseSettingRepository AdminPractiseSettingRepository
        {
            get
            {
                if (_AdminPractiseSettingRepository == null)
                {
                    _AdminPractiseSettingRepository = new AdminPractiseSettingRepository(cxt);
                }
                return _AdminPractiseSettingRepository;
            }
        }
        public AdminProcedureInvoiceSettingRepository AdminProcedureInvoiceSettingRepository
        {
            get
            {
                if (_AdminProcedureInvoiceSettingRepository == null)
                {
                    _AdminProcedureInvoiceSettingRepository = new AdminProcedureInvoiceSettingRepository(cxt);
                }
                return _AdminProcedureInvoiceSettingRepository;
            }
        }

        public AdminPrintFormatSettingRepository AdminPrintFormatSettingRepository
        {
            get
            {
                if (_AdminPrintFormatSettingRepository == null)
                {
                    _AdminPrintFormatSettingRepository = new AdminPrintFormatSettingRepository(cxt);
                }
                return _AdminPrintFormatSettingRepository;
            }
        }



        public DepratmentRepository DepratmentRepository
        {
            get
            {
                if (_DepratmentRepository == null)
                {
                    _DepratmentRepository = new DepratmentRepository(cxt);
                }
                return _DepratmentRepository;
            }
        }

        public SubDepartmentRepository SubDepartmentRepository
        {
            get
            {
                if (_SubDepartmentRepository == null)
                {
                    _SubDepartmentRepository = new SubDepartmentRepository(cxt);
                }
                return _SubDepartmentRepository;
            }
        }

        public LowStockMailRepository LowStockMailRepository
        {
            get
            {
                if (_LowStockMailRepository == null)
                {
                    _LowStockMailRepository = new LowStockMailRepository(cxt);
                }
                return _LowStockMailRepository;
            }
        }


        public CategoryRepository CategoryRepository
        {
            get
            {
                if (_CategoryRepository == null)
                {
                    _CategoryRepository = new CategoryRepository(cxt);
                }
                return _CategoryRepository;
            }
        }

        public ExpenseRepository ExpenseRepository
        {
            get
            {
                if (_ExpenseRepository == null)
                {
                    _ExpenseRepository = new ExpenseRepository(cxt);
                }
                return _ExpenseRepository;
            }

        }


        public ExpenseCategoryRepository ExpenseCategoryRepository
        {
            get
            {
                if (_ExpenseCategoryRepository == null)
                {
                    _ExpenseCategoryRepository = new ExpenseCategoryRepository(cxt);
                }
                return _ExpenseCategoryRepository;
            }

        }

        public ManufacturerRepository ManufacturerRepository
        {
            get
            {
                if (_ManufacturerRepository == null)
                {
                    _ManufacturerRepository = new ManufacturerRepository(cxt);
                }
                return _ManufacturerRepository;
            }
        }


        public HwDataRepository HwDataRepository
        {
            get
            {
                if (_HwDataRepository == null)
                {
                    _HwDataRepository = new HwDataRepository(cxt);
                }
                return _HwDataRepository;
            }
        }

        public TemplateRepository TemplateRepository
        {
            get
            {
                if (_TemplateRepository == null)
                {
                    _TemplateRepository = new TemplateRepository(cxt);
                }
                return _TemplateRepository;
            }
        }

        public StockAuditRepository StockAuditRepository
        {
            get
            {
                if (_StockAuditRepository == null)
                {
                    _StockAuditRepository = new StockAuditRepository(cxt);
                }
                return _StockAuditRepository;
            }
        }

        public RackRepository RackRepository
        {
            get
            {
                if (_RackRepository == null)
                {
                    _RackRepository = new RackRepository(cxt);
                }
                return _RackRepository;
            }
        }


        public AdminPharmacySettingRepository AdminPharmacySettingRepository
        {
            get
            {
                if (_AdminPharmacySettingRepository == null)
                {
                    _AdminPharmacySettingRepository = new AdminPharmacySettingRepository(cxt);
                }
                return _AdminPharmacySettingRepository;
            }
        }
        public void SetItemSyncable(int ItemId)
        {
            var r = cxt.Items.Where(i => i.ItemId == ItemId).FirstOrDefault();
            if (r != null)
            {
                r.IsSyncable = true;
            }
        }

        public FlatDiscountRepository FlatDiscountRepository
        {
            get
            {
                if (_FlatDiscountRepository == null)
                {
                    _FlatDiscountRepository = new FlatDiscountRepository(cxt);
                }
                return _FlatDiscountRepository;
            }
        }

        public MissedsalesRepository MissedsalesRepository
        {
            get
            {
                if (_MissedsalesRepository == null)
                {
                    _MissedsalesRepository = new MissedsalesRepository(cxt);
                }
                return _MissedsalesRepository;
            }
        }


        public AdminShiftMasterSettingRepository AdminShiftMasterSettingRepository
        {
            get
            {
                if (_AdminShiftMasterSettingRepository == null)
                {
                    _AdminShiftMasterSettingRepository = new AdminShiftMasterSettingRepository(cxt);
                }
                return _AdminShiftMasterSettingRepository;
            }
        }


        public AdminShiftsSettingRepository AdminShiftsSettingRepository
        {
            get
            {
                if (_AdminShiftsSettingRepository == null)
                {
                    _AdminShiftsSettingRepository = new AdminShiftsSettingRepository(cxt);
                }
                return _AdminShiftsSettingRepository;
            }
        }


        public POSCashReceiveRepository POSCashReceiveRepository
        {
            get
            {
                if (_POSCashReceiveRepository == null)
                {
                    _POSCashReceiveRepository = new POSCashReceiveRepository(cxt);
                }
                return _POSCashReceiveRepository;
            }
        }


        public LoginHistoryRepository LoginHistoryRepository
        {
            get
            {
                if (_LoginHistoryRepository == null)
                {
                    _LoginHistoryRepository = new LoginHistoryRepository(cxt);
                }
                return _LoginHistoryRepository;
            }
        }

        public POSCashSkimmedRepository POSCashSkimmedRepository
        {
            get
            {
                if (_POSCashSkimmedRepository == null)
                {
                    _POSCashSkimmedRepository = new POSCashSkimmedRepository(cxt);
                }
                return _POSCashSkimmedRepository;
            }
        }


        public POSClosingRepository POSClosingRepository
        {
            get
            {
                if (_POSClosingRepository == null)
                {
                    _POSClosingRepository = new POSClosingRepository(cxt);
                }
                return _POSClosingRepository;
            }
        }


        public AccTransactionsRepository AccTransactionsRepository
        {
            get
            {
                if (_AccTransactionsRepository == null)
                {
                    _AccTransactionsRepository = new AccTransactionsRepository(cxt);
                }
                return _AccTransactionsRepository;
            }
        }


        public StoreClosingRepository StoreClosingRepository
        {
            get
            {
                if (_StoreClosingRepository == null)
                {
                    _StoreClosingRepository = new StoreClosingRepository(cxt);
                }
                return _StoreClosingRepository;
            }
        }

        public SeatingTableRepository SeatingTableRspository
        {
            get
            {
                if (_SeatingTableRepository == null)
                {
                    _SeatingTableRepository = new SeatingTableRepository(cxt);
                }
                return _SeatingTableRepository;
            }
        }
        public EmployeeRepository EmployeeRepository
        {
            get
            {
                if (_EmployeeRepository == null)
                {
                    _EmployeeRepository = new EmployeeRepository(cxt);
                }
                return _EmployeeRepository;
            }
        }

        public ItemTypeRepository ItemTypeRepository
        {
            get
            {
                if (_ItemTypeRepository == null)
                {
                    _ItemTypeRepository = new ItemTypeRepository(cxt);
                }
                return _ItemTypeRepository;
            }
        }

        public RecipeRepository RecipeRepository
        {
            get
            {
                if (_RecipeRepository == null)
                {
                    _RecipeRepository = new RecipeRepository(cxt);
                }
                return _RecipeRepository;
            }
        }



        public AppSettingsRepository AppSettingsRepository
        {
            get
            {
                if (_AppSettingsRepository == null)
                {
                    _AppSettingsRepository = new AppSettingsRepository(cxt);
                }
                return _AppSettingsRepository;
            }
        }

        public void Save()
        {
            cxt.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    cxt.Dispose();
                }
            }
            this.disposed = true;
        }

        public AppDbContext GetDbContext()
        {
            return this.cxt;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
