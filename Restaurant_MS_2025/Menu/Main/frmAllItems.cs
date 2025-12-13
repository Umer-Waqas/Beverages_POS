



namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmAllItems : Form
    {
        enum FilterType { Default = 1, Generic = 2, Date = 3, Category = 4, Supplier = 5, Manufacturer = 6, Unit = 7 }
        FilterType filter = FilterType.Default;

        int PageNo = 1;
        bool IsFilterApplied = false;
        static AppDbContext cxt = new AppDbContext();
        ItemsRepository repItems = new ItemsRepository(cxt);
        UnitOfWork unitOfWork;

        IPagedList<ItemsVM> itemsList;
        public frmAllItems()
        {
            InitializeComponent();
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (!SharedFunctions.IsActionallowed("Add Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Form f = SharedFunctions.OpenForm(new frmItems(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefreshForm.PerformClick();
        }
        private void btnAddMultipleItems_Click(object sender, EventArgs e)
        {
            if (!SharedFunctions.IsActionallowed("Add Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //Form f = SharedFunctions.OpenForm(new frmMultipleItems(), this.MdiParent);
            //f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }
        private void btnAddStock_Click(object sender, EventArgs e)
        {
            if (!SharedFunctions.IsActionallowed("Add Stock") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Form f = SharedFunctions.OpenForm(new btn(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }
        private void frmAllItems_Load(object sender, EventArgs e)
        {
            if (SharedVariables.AdminPharmacySetting.ShowRackNoInPOS)
            {
                grdItems.Columns["colRackNo"].Visible = true;
            }
            if (!SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                grdItems.Columns["btnGBarcode"].Visible = false;
            }
            // default items are being loaded on frm activation event.
            SharedFunctions.SetGridStyle(grdItems);
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnEditDetails, btnAddMultipleItems, btnAddItem, btnAddStock, btnRefreshForm, btnExcel, btnPrint, btnGenBarcodes, btnAddDummyStock });
            this.WindowState = FormWindowState.Maximized;
        }

        private void LoadFilters()
        {
            try
            {
                cmbSupplier.SelectedIndexChanged -= cmbSupplier_SelectedIndexChanged;
                cmbCategory.SelectedIndexChanged -= cmbCategory_SelectedIndexChanged;
                cmbManuf.SelectedIndexChanged -= cmbManuf_SelectedIndexChanged;
                cmbStockUnit.SelectedIndexChanged -= cmbStockUnit_SelectedIndexChanged;
                cmbStockUnit.SelectedIndex = 0;
                using (unitOfWork = new UnitOfWork())
                {
                    loadSuppliers();
                    loadCategories();
                    loadManufacturers();
                }
                cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
                cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
                cmbManuf.SelectedIndexChanged += cmbManuf_SelectedIndexChanged;
                cmbStockUnit.SelectedIndexChanged += cmbStockUnit_SelectedIndexChanged;
                cmbStockUnit.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading some filters data, please try reloading current form", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void loadSuppliers()
        {
            List<Supplier> sups = new List<Supplier>();
            sups = unitOfWork.SupplierRepository.GetActiveSuppliers();
            sups.Insert(0, new Supplier() { SupplierID = 0, Name = "Select Supplier" });
            cmbSupplier.DataSource = sups;
            cmbSupplier.ValueMember = "SupplierId";
            cmbSupplier.DisplayMember = "Name";

        }

        private void loadCategories()
        {
            List<Category> cats = new List<Category>();
            cats = unitOfWork.CategoryRepository.GetAllActiveCategories();
            cats.Insert(0, new Category { CategoryId = 0, Name = "Select Category" });
            cmbCategory.DataSource = cats;
            cmbCategory.ValueMember = "CategoryId";
            cmbCategory.DisplayMember = "Name";
        }

        private void loadManufacturers()
        {
            List<Manufacturer> manfs = new List<Manufacturer>();
            manfs = unitOfWork.ManufacturerRepository.GetAllActiveManufacturers();
            manfs.Insert(0, new Manufacturer { ManufacturerId = 0, Name = "Select Manufacturer" });
            cmbManuf.DataSource = manfs;
            cmbManuf.ValueMember = "ManufacturerId";
            cmbManuf.DisplayMember = "Name";
        }


        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {

            if (keyData == (Keys.Control | Keys.P))
            {
                btnPrint.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.E))
            {
                btnExcel.PerformClick();
                return true;
            }
            if (keyData == (Keys.F5))
            {
                btnRefreshForm.PerformClick();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.M))
            {
                btnAddMultipleItems.PerformClick();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.I))
            {
                btnAddItem.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.Shift | Keys.T))
            {
                btnAddStock.PerformClick();
                return true;
            }

            if (keyData == (Keys.Alt | Keys.G))
            {
                grdItems.Focus();
                if (grdItems.Rows.Count >= 1)
                {
                    grdItems.Rows[0].Selected = true;
                }
                return true;
            }

            #region grid short cuts
            if (keyData == (Keys.Alt | Keys.E))
            {
                if (grdItems.SelectedRows.Count > 0)
                {
                    int colIndex = grdItems.Columns["colEdit"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                    //cmbSelectBatch.Focus();
                }
                return true;
            }

            if (keyData == (Keys.Alt | Keys.D))
            {
                {
                    int colIndex = grdItems.Columns["colDelete"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                    //cmbSelectBatch.Focus();
                }
                return true;
            }

            if (keyData == (Keys.Alt | Keys.V))
            {
                {
                    int colIndex = grdItems.Columns["colView"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                    //cmbSelectBatch.Focus();
                }
                return true;
            }
            #endregion grid short cuts
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void LoadItems()
        {
            try
            {
                grdItems.Rows.Clear();
                using (unitOfWork = new UnitOfWork())
                {

                    string FilterString = txtNameFilter.Text.Trim();
                    if (filter == FilterType.Generic)
                    {
                        itemsList = unitOfWork.ItemRspository.GetItemsWithStockDataByItemNameFilter(FilterString.ToLower(), PageNo, SharedVariables.PageSize);
                    }
                    else if (filter == FilterType.Date)
                    {
                        itemsList = unitOfWork.ItemRspository.GetItemsWithStockDataByStockDate(dtpStockDate.Value, PageNo, SharedVariables.PageSize);
                    }
                    else if (filter == FilterType.Manufacturer)
                    {
                        itemsList = unitOfWork.ItemRspository.GetItemsWithStockDataByManuf(Convert.ToInt32(cmbManuf.SelectedValue), PageNo, SharedVariables.PageSize);
                    }
                    else if (filter == FilterType.Supplier)
                    {
                        itemsList = unitOfWork.ItemRspository.GetItemsWithStockDataBySupp(Convert.ToInt32(cmbSupplier.SelectedValue), PageNo, SharedVariables.PageSize);
                    }
                    else if (filter == FilterType.Category)
                    {
                        itemsList = unitOfWork.ItemRspository.GetItemsWithStockDataByCat(Convert.ToInt32(cmbCategory.SelectedValue), PageNo, SharedVariables.PageSize);
                    }
                    else
                    {
                        itemsList = unitOfWork.ItemRspository.GetItemsWithStockData(PageNo, SharedVariables.PageSize);
                    }
                }
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, itemsList.PageCount);
                btnFirstPage.Enabled = btnPrevious.Enabled = itemsList.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = itemsList.HasNextPage;
                //AppendSuppliers(itemsList);
                string Low = "";
                foreach (ItemsVM item in itemsList.Items)
                {
                    foreach (Supplier s in item.SuppliersList)
                    {
                        item.Suppliers += s.Name + ", ";
                    }
                    if (!string.IsNullOrEmpty(item.Suppliers))
                    {
                        item.Suppliers = item.Suppliers.Trim().TrimEnd(',');
                    }
                    Low = "";
                    if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                    {
                        item.AvailableStock = item.TotalStock - (item.ExpiredStock - item.ExpiredConsumedStock) - item.ConsumedStock + item.AdjustedStock;
                    }
                    else
                    {
                        item.AvailableStock = item.TotalStock - (item.ExpiredStock - item.ExpiredConsumedStock) - item.ConsumedStock + item.AdjustedStock + item.HoldStock;
                    }
                    item.TotalStock = item.TotalStock + item.AdjustedStock;
                    if (item.ReorderingLevel > 0)
                    {
                        Low = item.AvailableStock <= item.ReorderingLevel ? "LOW" : "";
                    }
                    string Expired = item.ExpiredStock > 0 ? "EXPIRED" : "";
                    item.ItemType = item.IsNarcotic ? "Narcotic" : "Drug";
                    if (cmbStockUnit.SelectedIndex == 0)
                    {
                        //item.TotalStock = Math.Floor((double)(item.TotalStock / item.ConversionUnit));
                        //item.AvailableStock = Convert.ToInt32(Math.Floor((double)(item.AvailableStock / item.ConversionUnit)));
                        //item.ExpiredStock = Convert.ToInt32(Math.Floor((double)(item.ExpiredStock / item.ConversionUnit)));

                        item.TotalStock = item.TotalStock / item.ConversionUnit;
                        item.AvailableStock = item.AvailableStock / item.ConversionUnit;
                        item.ExpiredStock = item.ExpiredStock / item.ConversionUnit;

                    }
                    else
                    {
                        item.Unit = "Units";
                    }
                    grdItems.Rows.Add(item.ItemId, item.ItemName, item.Barcode, item.RackNo, item.Suppliers, item.Manufacturer == null ? "" : item.Manufacturer.Name, item.ReorderingLevel, item.TotalStock, item.AvailableStock, Low, item.ExpiredStock, Expired, item.Unit, item.ConversionUnit, item.ConsumedStock);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void AppendSuppliers(List<ItemsVM> itemsList)
        {
            List<Item_SuppliersVM> itemSupList = repItems.GetSuppliersByItemIds(itemsList);
            foreach (ItemsVM i in itemsList)
            {
                foreach (Item_SuppliersVM ii in itemSupList)
                {
                    if (i.ItemId == ii.ItemId)
                    {
                        i.Suppliers += ii.SupplierName.ToString() + ",";
                    }
                }
                string suppliers = i.Suppliers.Length > 0 ? i.Suppliers.TrimEnd(',') : "";
                i.Suppliers = suppliers;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadItems();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadItems();
        }
        //private void btnExcel_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ItemssRpt rpt = new ItemssRpt();
        //        rpt.Database.Tables[0].SetDataSource(this.GetReportData());
        //        if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
        //        {
        //            rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
        //        }

        //        rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
        //        rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
        //        rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
        //        if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
        //            MessageBox.Show("Items Date Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
        //    }
        //}
        //private void btnPrint_Click(object sender, EventArgs e)
        //{
        //    ItemsViewer f = new ItemsViewer(GetReportData());
        //    f.Show();
        //}

        private List<ItemsVM> GetReportData()
        {
            try
            {
                List<ItemsVM> Items = new List<ItemsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    if (IsFilterApplied)
                    {
                        string FilterString = txtNameFilter.Text.Trim();
                        if (FilterString != "")
                        {
                            Items = unitOfWork.ItemRspository.GetItemsWithStockDataByItemNameFilter(FilterString.ToLower());
                        }
                        else
                        {
                            Items = unitOfWork.ItemRspository.GetItemsWithStockDataByStockDate(dtpStockDate.Value);
                        }
                    }
                    else
                    {
                        Items = unitOfWork.ItemRspository.GetItemsWithStockData();
                    }
                }
                string suppliers = "";
                foreach (ItemsVM i in Items)
                {
                    suppliers = "";
                    i.AvailableStock = i.TotalStock - i.ExpiredStock + i.AdjustedStock - i.ConsumedStock;
                    i.TotalStock = i.TotalStock + i.AdjustedStock;
                    foreach (var s in i.SuppliersList)
                    {
                        suppliers += s.Name + ", ";
                    }
                    suppliers = suppliers.TrimEnd(',');
                    i.Suppliers = suppliers;
                }
                return Items;
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while fetching report data, please try again.", ex.Message, "Failed.");
                return null;
            }
        }


        private void dtpStockDate_ValueChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            PageNo = 1;
            LoadItems();
        }


        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            dtpStockDate.ValueChanged -= dtpStockDate_ValueChanged;
            dtpStockDate.Value = DateTime.Now;
            txtNameFilter.Text = "";
            filter = FilterType.Default;
            LoadItems();
            PageNo = 1;
            dtpStockDate.ValueChanged += dtpStockDate_ValueChanged;
        }
        private void txtNameFilter_KeyDown(object sender, KeyEventArgs e)
        {
            dtpStockDate.ValueChanged -= dtpStockDate_ValueChanged;
            cmbCategory.SelectedIndexChanged -= cmbCategory_SelectedIndexChanged;
            cmbSupplier.SelectedIndexChanged -= cmbSupplier_SelectedIndexChanged;
            cmbManuf.SelectedIndexChanged -= cmbManuf_SelectedIndexChanged;

            dtpStockDate.Value = DateTime.Today;
            cmbCategory.SelectedIndex = 0;
            cmbSupplier.SelectedIndex = 0;
            cmbManuf.SelectedIndex = 0;

            dtpStockDate.ValueChanged += dtpStockDate_ValueChanged;
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
            cmbManuf.SelectedIndexChanged += cmbManuf_SelectedIndexChanged;


            if (e.KeyData == Keys.Enter)
            {
                if (txtNameFilter.Text.Trim() != "")
                {
                    filter = FilterType.Generic;
                    PageNo = 1;
                    LoadItems();
                }
            }
        }

        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    int ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                    if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
                    {
                        if (!SharedFunctions.IsActionallowed("Edit Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                        {
                            MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        frmItems f = new frmItems(ItemId);
                        f.MdiParent = this.MdiParent;
                        f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                        f.Show();
                        return;
                    }
                    if (e.ColumnIndex == grdItems.Columns["colDelete"].Index)
                    {
                        if (!SharedFunctions.IsActionallowed("Delete Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                        {
                            MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        int ConsumedQty = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colConsumedStock"].Value);
                        if (ConsumedQty > 0)
                        {
                            MessageBox.Show("Item Has Consumed Quantity, it Can't be Deleted", "Invalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete This Item?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs == System.Windows.Forms.DialogResult.OK)
                        {
                            //Item i = unitOfWork.ItemRspository.GetById(ItemId);
                            //i.IsActive = false;
                            //unitOfWork.ItemRspository.Update(i);
                            using (unitOfWork = new UnitOfWork())
                            {
                                unitOfWork.ItemRspository.SetItemDataInactive(ItemId);
                                unitOfWork.Save();
                            }
                            grdItems.Rows.RemoveAt(e.RowIndex);
                            MessageBox.Show("Item Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            PageNo = 1;
                            btnRefreshForm.PerformClick();
                        }
                        return;
                    }
                    if (e.ColumnIndex == grdItems.Columns["colView"].Index)
                    {
                        frmItemInfo f = new frmItemInfo(ItemId);
                        f.MdiParent = this.MdiParent;
                        f.Show();
                    }

                    if (e.ColumnIndex == grdItems.Columns["btnGBarCode"].Index)
                    {
                        try
                        {
                            //string barcode = SharedFunctions.GetTimestamp2(DateTime.Now);
                            using (unitOfWork = new UnitOfWork())
                            {
                                Item obj = unitOfWork.ItemRspository.GetById(ItemId);
                                obj.Barcode = ItemId.ToString("D5");
                                unitOfWork.ItemRspository.Update(obj);
                                unitOfWork.Save();
                                MessageBox.Show("Barcode generated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("An error occurred while generating barcodes, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = itemsList.PageCount;
            LoadItems();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadItems();
        }

        private void frmAllItems_Activated(object sender, EventArgs e)
        {
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmAllItemsShortCuts f = new ShortCutDialogs.frmAllItemsShortCuts();
            f.ShowDialog();
        }

        private void txtGotoPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > itemsList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    PageNo = _pageNo;
                    LoadItems();
                }
            }
        }

        private void cmbStockUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadItems();
        }

        private void btnEditDetails_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmEditDetails(), this.MdiParent);
        }

        private void cmbSupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtNameFilter.KeyDown -= txtNameFilter_KeyDown;
            cmbCategory.SelectedIndexChanged -= cmbCategory_SelectedIndexChanged;
            cmbManuf.SelectedIndexChanged -= cmbManuf_SelectedIndexChanged;
            dtpStockDate.ValueChanged -= dtpStockDate_ValueChanged;

            txtNameFilter.Text = "";
            cmbCategory.SelectedIndex = 0;
            cmbManuf.SelectedIndex = 0;
            dtpStockDate.Value = DateTime.Today;

            txtNameFilter.KeyDown += txtNameFilter_KeyDown;
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            cmbManuf.SelectedIndexChanged += cmbManuf_SelectedIndexChanged;
            dtpStockDate.ValueChanged += dtpStockDate_ValueChanged;

            PageNo = 1;
            if (cmbSupplier.SelectedIndex > 0)
            {
                this.filter = FilterType.Supplier;
            }
            else
            {
                this.filter = FilterType.Default; ;
            }
            LoadItems();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtpStockDate.ValueChanged -= dtpStockDate_ValueChanged;
            txtNameFilter.KeyDown -= txtNameFilter_KeyDown;
            cmbSupplier.SelectedIndexChanged -= cmbSupplier_SelectedIndexChanged;
            cmbManuf.SelectedIndexChanged -= cmbManuf_SelectedIndexChanged;

            dtpStockDate.Value = DateTime.Today;
            txtNameFilter.Text = "";
            cmbSupplier.SelectedIndex = 0;
            cmbManuf.SelectedIndex = 0;

            dtpStockDate.ValueChanged += dtpStockDate_ValueChanged;
            txtNameFilter.KeyDown += txtNameFilter_KeyDown;
            cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
            cmbManuf.SelectedIndexChanged += cmbManuf_SelectedIndexChanged;


            PageNo = 1;
            if (cmbCategory.SelectedIndex > 0)
            {
                this.filter = FilterType.Category;
            }
            else
            {
                this.filter = FilterType.Default;
            }
            LoadItems();

        }

        private void cmbManuf_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtpStockDate.ValueChanged -= dtpStockDate_ValueChanged;
            txtNameFilter.KeyDown -= txtNameFilter_KeyDown;
            cmbCategory.SelectedIndexChanged -= cmbCategory_SelectedIndexChanged;
            cmbSupplier.SelectedIndexChanged -= cmbSupplier_SelectedIndexChanged;

            dtpStockDate.Value = DateTime.Today;
            txtNameFilter.Text = "";
            cmbCategory.SelectedIndex = 0;
            cmbSupplier.SelectedIndex = 0;

            dtpStockDate.ValueChanged += dtpStockDate_ValueChanged;
            txtNameFilter.KeyDown += txtNameFilter_KeyDown;
            cmbSupplier.SelectedIndexChanged += cmbSupplier_SelectedIndexChanged;
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;

            PageNo = 1;
            if (cmbManuf.SelectedIndex > 0)
            {
                this.filter = FilterType.Manufacturer;
            }
            else
            {
                this.filter = FilterType.Default;
            }
            LoadItems();
        }

        private void frmAllItems_Shown(object sender, EventArgs e)
        {
            LoadFilters();
            LoadItems();
        }

        private void btnGenBarcodes_Click(object sender, EventArgs e)
        {
            if (!SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                List<long> items = new List<long>();
                using (unitOfWork = new UnitOfWork())
                {
                    items = unitOfWork.ItemRspository.GetItemsHavingNoBarcodes();

                    foreach (var i in items)
                    {
                        Item obj = unitOfWork.ItemRspository.GetById(i);
                        obj.Barcode = i.ToString("D5");
                        unitOfWork.ItemRspository.Update(obj);
                    }
                    unitOfWork.Save();
                }
                MessageBox.Show("Barcodes generated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while generating barciodes, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddDummyStock_Click(object sender, EventArgs e)
        {
            if (!SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            try
            {
                List<Item> items = new List<Item>();
                using (unitOfWork = new UnitOfWork())
                {
                    items = unitOfWork.ItemRspository.GetActiveItems();

                }
                int docNo = 100900;
                foreach (var i in items)
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        i.OpeningStock = 200;
                        AddDummStockEntry(i, docNo);
                        docNo += 1;
                        unitOfWork.Save();
                    }
                }
                MessageBox.Show("Dummy stock saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while inserting dummy stock.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddDummStockEntry(Item objItem, int DocNo)
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

            objStockItem.ItemId = objItem.ItemId;// unitOfWork.ItemRspository.GetById(ItemId);
            objStockItem.CreatedAt = DateTime.Now;
            objStockItem.UpdatedAt = DateTime.Now;
            objStockItem.IsActive = true;
            objStockItem.IsNew = true;
            objStockItem.IsSynced = false;
            objStockItem.IsUpdate = false;
            objStock.StockItems.Add(objStockItem);
            unitOfWork.StockRepository.Insert(objStock);
        }
    }
}