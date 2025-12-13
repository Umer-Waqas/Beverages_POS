
//using ExcelDataReader;
using System.Data.OleDb;

namespace Restaurant_MS_UI.App
{
    public partial class frmWaiting : Form
    {
        UnitOfWork unitOfWork;
        public frmWaiting()
        {
            InitializeComponent();
        }

        private void frmWaiting_Load(object sender, EventArgs e)
        {
        }

        private void bgDataLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Comp_Timer = new System.Timers.Timer();
            //Comp_Timer.Enabled = true;
            //Comp_Timer.Interval = 3000;
            //Comp_Timer.Elapsed += new System.Timers.ElapsedEventHandler(Comp_Timer_Elapsed);
            //Comp_Timer.Start();
            //Thread.Sleep(3000);

            //pbLoading.Visible = false;
            //lblStatus.Text = "Done!";
            //pbCompleted.Visible = true;
            //pbCompleted.BringToFront();
            //pnlDisplay.Invalidate();
            //panel1.Invalidate();

            this.Invoke(new Action(() =>
             {
                 pbLoading.Visible = false;
                 lblStatus.Text = "Done!";
                 pbCompleted.Visible = true;
                 pbCompleted.BringToFront();
                 pnlDisplay.Invalidate();
                 this.Invalidate();
                 lblStatus.Refresh();
                 pnlDisplay.Refresh();
                 Thread.Sleep(3000);
                 this.Close();
             }));
        }

        private void Comp_Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    pbLoading.Visible = false;
                    lblStatus.Text = "Done!";
                    pbCompleted.Visible = true;
                    pbCompleted.BringToFront();
                    pnlDisplay.Invalidate();
                    this.Close();
                }));

            }
            else
            {
                pbLoading.Visible = false;
                lblStatus.Text = "Done!";
                pbCompleted.Visible = true;
                pbCompleted.BringToFront();
                pnlDisplay.Invalidate();
                this.Close();
            }
        }

        private void bgDataLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            //Thread.Sleep(5000);
            //return;
            try
            {
                bool proceedToLoadData = false;
                using (unitOfWork = new UnitOfWork())
                {
                    if (unitOfWork.SupplierRepository.SuppliersExist() && unitOfWork.ItemRspository.ItemsCount() > 1)
                    {
                        MessageBox.Show("Data already loaded", "Data exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else if (!unitOfWork.SupplierRepository.SuppliersExist() || unitOfWork.ItemRspository.ItemsCount() == 1)
                    {
                        proceedToLoadData = true;
                    }
                }
                if (proceedToLoadData)
                {
                    string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    string path = "";

                    path = Path.Combine(baseDir, "Data", "HS_ItemsList.xlsx");
                    DataTable dt = ReadExcel(path, ".xlsx");
                    LoadItemsSheet(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private int GetPriaceId()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    return unitOfWork.AdminPractiseSettingRepository.GetPracticeId();
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public DataTable ReadExcel(string path, string extension)
        {
            DataTableCollection dtcl;
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
            {
                //using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                //{
                //    //DataSet ds = reader.AsDataSet(new ExcelDataSetConfiguration
                //    //{
                //    //    ConfigureDataTable = (_) => new ExcelDataTableConfiguration { UseHeaderRow = true }
                //    //});
                //    //dtcl = ds.Tables;
                //    //return dtcl["Sheet1"];
                //}
            }
            return new DataTable();
        }
        public DataTable ReadExcel_UsingIterop(string filePath, string fileExt)
        {
            string conn = string.Empty;
            DataTable dtexcel = new DataTable();
            if (fileExt.CompareTo(".xls") == 0)//compare the extension of the file
            {
                //conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties='Excel 8.0;HRD=Yes;IMEX=2';";//for below excel 2007
                conn = @"provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + filePath + "';Extended Properties= Excel 8.0;";//for below excel 2007
            }

            else
                conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1';";//for above excel 2007
            //using (OleDbConnection con = new OleDbConnection(conn))
            //{
            //    try
            //    {
            //        //OleDbDataAdapter oleAdpt = new OleDbDataAdapter(@"select * from [CourseList$]", con);//here we read data from sheet1
            //        OleDbDataAdapter oleAdpt = new OleDbDataAdapter(@"select * from [Sheet1$]", con);//here we read data from sheet1

            //        //oleAdpt.TableMappings.Add("Table", "TestTable");
            //        //con.Open();
            //        //DataTable schemaTable = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null });

            //        //foreach (DataRow dataRow in schemaTable.Rows)
            //        //{
            //        //    string tableName = dataRow["TABLE_NAME"].ToString();	// gets the sheet name from each schema entry
            //        //    if (!tableName.EndsWith("_"))
            //        //    {
            //        //        // some code to display the name
            //        //    }
            //        //}
            //        oleAdpt.Fill(dtexcel);//fill excel data into dataTable
            //        con.Close();
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message.ToString());
            //    }
            //}
            return dtexcel;
        }
        private void LoadItemsSheet(DataTable dtSource)
        {
            loadSuppliers(dtSource);
            loadCategories(dtSource);
            loadItems(dtSource);
            //MessageBox.Show("Data loaded successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void MakeOpeningStockEntry(Item objItem, int DocNo)
        {
            Stock objStock = new Stock();
            objStock.DocumentNo = DocNo;
            objStock.Supplier = null;
            objStock.SupplierInvoiceNo = "";
            objStock.SupplierIvoiceDate = DateTime.Now;
            objStock.IsAutoInsertedStock = false; // if need to sync stocks, then set this to false : else set this variable to true.
            objStock.GrandTotal = objItem.OpeningStock * objItem.UnitCostPrice;
            objStock.TotalPaid = 0;
            objStock.SupplierInvoiceNo = "";
            objStock.ImagePath = null;
            objStock.StockDate = DateTime.Now;
            objStock.CreatedAt = DateTime.Now;
            objStock.UpdatedAt = DateTime.Now;
            objStock.IsActive = true;
            objStock.IsNew = true;
            objStock.IsUpdate = false;
            objStock.IsSynced = false;
            objStock.DocumentNo = DocNo;
            objStock.UserId = SharedVariables.LoggedInUser.UserId;
            objStock.StockItems = new List<StockItem>();
            StockItem objStockItem = new StockItem();

            objStockItem.Batch = new Batch();
            objStockItem.Batch.BatchName = "Opening Stock";
            objStockItem.Batch.Expiry = null;
            objStockItem.Batch.CreatedAt = DateTime.Now;
            objStockItem.Batch.UpdatedAt = DateTime.Now;
            objStockItem.Batch.IsNew = true;
            objStockItem.Batch.IsActive = true;
            objStockItem.Batch.IsSynced = false;
            objStockItem.Batch.IsUpdate = false;
            objStockItem.Unit = 0; // "For Stock in units"
            objStockItem.Quantity = objItem.OpeningStock;
            objStockItem.BonusQuantity = 0;

            objStockItem.UnitCost = objItem.UnitCostPrice;
            objStockItem.TotalCost = objStockItem.UnitCost * objStockItem.Quantity;
            objStockItem.NetValue = objStockItem.TotalCost;
            objStockItem.RetailPrice = objItem.RetailPrice;
            objStockItem.BonusDiscount = 0;

            objStockItem.Discount = 0;
            objStockItem.DiscountType = 0;
            objStockItem.PercDiscType = 0;
            objStockItem.DiscountValue = 0;

            objStockItem.SalesTax = 0;
            objStockItem.SalesTaxType = 0;
            objStockItem.PercSalesTaxType = 0;
            objStockItem.SalesTaxValue = 0;

            objStockItem.Item = objItem;// unitOfWork.ItemRspository.GetById(ItemId);
            objStockItem.CreatedAt = DateTime.Now;
            objStockItem.UpdatedAt = DateTime.Now;
            objStockItem.IsActive = true;
            objStockItem.IsNew = true;
            objStockItem.IsSynced = false;
            objStockItem.IsUpdate = false;
            objStock.StockItems.Add(objStockItem);
            unitOfWork.StockRepository.Insert(objStock);
        }
        private void loadSuppliers(DataTable dtSource)
        {
            string suppName = "";
            using (unitOfWork = new UnitOfWork())
            {
                if (unitOfWork.SupplierRepository.Any())
                {
                    return;
                }

                List<Supplier> sl = new List<Supplier>();
                foreach (DataRow r in dtSource.Rows)
                {
                    suppName = "";
                    suppName = r["Supplier"].ToString().Trim();
                    if (string.IsNullOrEmpty(suppName))
                    {
                        continue;
                    }
                    Supplier objSupplier = sl.Where(sp => sp.Name.ToLower().Equals(suppName.ToLower())).FirstOrDefault();
                    if (objSupplier == null)
                    {
                        objSupplier =
                        new Supplier
                        {
                            Name = suppName,
                            Phone = "",
                            Address = "",
                            PrimaryContactPersonName = "",
                            PrimaryContactPersonPhone = "",
                            OpeningBalance = 0,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            IsActive = true,
                            IsNew = true,
                            IsUpdate = false,
                            IsSynced = false,
                            UserId = SharedVariables.LoggedInUser.UserId
                        };
                        sl.Add(objSupplier);
                    }
                }

                unitOfWork.SupplierRepository.InsertRange(sl);
                unitOfWork.Save();
            }
        }
        private void loadCategories(DataTable dtSource)
        {
            string catName = "";
            using (unitOfWork = new UnitOfWork())
            {
                if (unitOfWork.CategoryRepository.Any())
                {
                    return;
                }

                List<Category> cl = new List<Category>();
                foreach (DataRow r in dtSource.Rows)
                {
                    catName = "";
                    catName = r["Category"].ToString().Trim();
                    if (string.IsNullOrEmpty(catName))
                    {
                        continue;
                    }
                    Category objCat = cl.Where(sp => sp.Name.ToLower().Equals(catName.ToLower())).FirstOrDefault();
                    if (objCat == null)
                    {
                        objCat =
                        new Category
                        {
                            Name = catName,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            IsActive = true,
                            IsNew = true,
                            IsUpdate = false,
                            IsSynced = false,
                            UserId = SharedVariables.LoggedInUser.UserId
                        };
                        cl.Add(objCat);
                    }
                }
                unitOfWork.CategoryRepository.InsertRange(cl);
                unitOfWork.Save();
            }
        }
        private void loadItems(DataTable dtSource)
        {
            int Quantity = 0, convUnit = 1;
            long catId = 0;
            double retailPrice = 0, costPrice = 0;
            string barcode = "", category = "", suppName = "";

            List<Item> il = new List<Item>();
            using (unitOfWork = new UnitOfWork())
            {
                if (unitOfWork.ItemRspository.ExistsAny())
                {
                    return;
                }
                foreach (DataRow r in dtSource.Rows)
                {
                    suppName = "";
                    Quantity = 0;
                    convUnit = 1;
                    category = "";

                    Item objItem = new Item();
                    objItem.ItemName = r["ItemName"].ToString().Trim();
                    if (string.IsNullOrEmpty(objItem.ItemName))
                    {
                        continue;
                    }
                    Item itemFound = il.Where(i => i.ItemName.ToLower().Equals(objItem.ItemName.ToLower())).FirstOrDefault();


                    if (itemFound == null)
                    {
                        suppName = r["Supplier"].ToString().Trim();
                        int.TryParse(r["Quantity"].ToString(), out Quantity);
                        double.TryParse(r["CostPrice"].ToString(), out costPrice);
                        double.TryParse(r["RetailPrice"].ToString(), out retailPrice);
                        barcode = r["Barcode"].ToString();
                        category = r["Category"].ToString().Trim();

                        //objItem.RackNo = "";
                        objItem.Barcode = barcode;
                        objItem.Unit = "Pack";
                        objItem.ConversionUnit = convUnit;
                        objItem.ReOrderingLevel = 0;
                        objItem.OpeningStock = Quantity;
                        objItem.UnitCostPrice = costPrice;
                        objItem.RetailPrice = retailPrice;
                        objItem.IsActive = true;
                        objItem.IsNew = true;
                        objItem.IsUpdate = false;
                        objItem.IsSynced = false;
                        objItem.IsSyncable = true;
                        objItem.CreatedAt = DateTime.Now;
                        objItem.UpdatedAt = DateTime.Now;
                        objItem.Suppliers = new List<Supplier>();
                        Supplier objSupp = unitOfWork.SupplierRepository.GetSupplierByName(suppName.ToLower());
                        if (objSupp != null)
                        {
                            objItem.Suppliers.Add(objSupp);
                        }

                        catId = unitOfWork.CategoryRepository.GetCategoryIdByName(category.ToLower());
                        if (catId > 0)
                        {
                            objItem.CategoryId = catId;
                        }
                        objItem.UserId = SharedVariables.LoggedInUser.UserId;
                        il.Add(objItem);
                    }
                }
                int docNo = 100001;
                foreach (var i in il)
                {
                    if (i.OpeningStock > 0)
                    {
                        MakeOpeningStockEntry(i, docNo);
                        docNo += 1;
                    }
                }
                unitOfWork.ItemRspository.InsertRange(il);
                unitOfWork.Save();
            }
        }
        private void frmWaiting_Shown(object sender, EventArgs e)
        {
            Thread.Sleep(3000);
            bgDataLoader = new BackgroundWorker();
            this.bgDataLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgDataLoader_RunWorkerCompleted);
            this.bgDataLoader.DoWork += new DoWorkEventHandler(bgDataLoader_DoWork);
            this.bgDataLoader.WorkerReportsProgress = true;
            bgDataLoader.RunWorkerAsync();
        }

        private void pnlDisplay_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}