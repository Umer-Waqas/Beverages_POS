

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class btn : Form
    {
        private long StockAddItemId = 0;
        UnitOfWork unitOfWork;
        private long StockEditID = 0;
        int EditingRowIndex = -1;
        long PurchaseOrderId = 0;
        List<Supplier> Suppliers = new List<Supplier>();
        BackgroundWorker bgWItemsLoader = new BackgroundWorker();
        BackgroundWorker bgWSuppliersLoader = new BackgroundWorker();
        List<Item> Items = new List<Item>();
        long SelectedItemId = 0;
        string SelectedItemName = "";
        private bool isStockLoading = false;
        private bool IsLoadingPrvSoldItems = false;
        public DateTime? ActionTime = null;

        private long AddedStockId = 0;
        private decimal AddedDocNo = 0;
        private long AddedSupplierId = 0;

        public btn()
        {
            InitializeComponent();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }
        public btn(long StockAdd_ItemID)
        {
            InitializeComponent();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            this.StockAddItemId = StockAdd_ItemID;
        }
        public btn(long StockEditID, long PurchaseOrderId)
        {
            InitializeComponent();
            this.StockEditID = StockEditID;
            this.PurchaseOrderId = PurchaseOrderId;
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            this.uC_SearchItems1.BringToFront();
        }
        private void uC_SearchItems1_OnItemSelectionChanged()
        {
            this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;
            this.SelectedItemName = this.uC_SearchItems1.SelectedItemName;
            //if (this.SelectedItemId > 0)
            //{
            //    LoadRates();
            //}
            if (this.SelectedItemId > 0)
            {
                btnAddItem.PerformClick();
            }
            //this.uC_SearchItems1.BringToFront();
            //MessageBox.Show(this.SelectedItemId + " | " + this.SelectedItemName);
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.A))
            {
                btnAddItem.PerformClick();
                return true;
            }
            if (keyData == (Keys.Alt | Keys.I))
            {
                //cmbItems.Focus();
                this.uC_SearchItems1.Focus();
                return true;
            }
            if (keyData == (Keys.Alt | Keys.D))
            {
                btnClear.PerformClick();
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
            if (keyData == (Keys.F10))
            {
                resetSearchFocus();
                return true;
            }
            if (keyData == (Keys.Alt | Keys.E))
            {
                if (grdItems.SelectedRows.Count > 0)
                {
                    int colIndex = grdItems.Columns["colEdit"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                }
                return true;
            }
            if (keyData == (Keys.Alt | Keys.R))
            {
                if (grdItems.SelectedRows.Count > 0)
                {
                    int colIndex = grdItems.Columns["colRemove"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                }
                return true;
            }

            if (keyData == (Keys.Control | Keys.S))
            {
                btnSave.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.D))
            {
                btnClearAll.PerformClick();
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
                }
                return true;
            }

            if (keyData == (Keys.Alt | Keys.R))
            {
                {
                    int colIndex = grdItems.Columns["colRemove"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                }
                return true;
            }
            #endregion grid short cuts
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void setNumericUD_toEmpty()
        {
            numUnitCost.ResetText();
            numQty.ResetText();
            numRetailPrice.ResetText();
            numDiscount.ResetText();
            numSalesTax.ResetText();
        }
        private void UpdateUsersAndSuppliers()
        {
            Supplier Select = new Supplier();
            Select.Name = "Select";
            Select.SupplierID = 0;
            Suppliers.Insert(0, Select);
            cmbSuppliers.DataSource = Suppliers;
            cmbSuppliers.ValueMember = "SupplierId";
            cmbSuppliers.DisplayMember = "Name";
            cmbSuppliers.Enabled = true;
            lblLoadingSuppliers.Visible = false;
        }
        private async void loadSuppliers()
        {
            List<SelectListVM> Suppliers = new List<SelectListVM>();
            using (unitOfWork = new UnitOfWork())
            {
                Suppliers = await unitOfWork.SupplierRepository.GetSelectList();
                SharedFunctions.SetComboDataSource(Suppliers, cmbSuppliers, "Select Supplier");
            }
        }
        private void frmAddStock_Load(object sender, EventArgs e)
        {
            if (SharedFunctions.CheckDayClosed())
            {
                this.BeginInvoke(new MethodInvoker(Close));
                return;
            }
            dtpSupplierInvoiceDate.Value = dtpDate.Value = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
            this.WindowState = FormWindowState.Maximized;
            //chkBarcodePref.Checked = Properties.Settings.Default.UseBarcodeSearch;
            //chkSearchItemPref.Checked = !Properties.Settings.Default.UseBarcodeSearch;
            grdItems.Columns["colExpiry"].Visible = grdItems.Columns["colBatch"].Visible = SharedVariables.AdminPharmacySetting.AllowBatchEntryOnAddStock;



            SharedFunctions.SetGridStyle(this.grdItems);
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnClearAll, btnSave, btnMoreOpt, btnMoreOpt, btnPayBill });
            SharedFunctions.SetSmallButtonsStyle(new[] { btnAddSupplier, btnAttachImage, btnAddItem, btnClear, btnSearchItems });
            try
            {
                this.loadSuppliers();
                LoadItemsAndSuppliers();      // calls backGorundWorker : //  also loadsStock in case of edit on RunWorker_Completed
                setNumericUD_toEmpty();
                cmbDiscountType.SelectedIndex = 0;
                cmbSalesTaxType.SelectedIndex = 0;
                //MessageBox.Show(this.cmbSuppliers.Items.Count.ToString());
                // MessageBox.Show(this.cmbSuppliers.Items.Count.ToString());
                //cmbItems.SelectedIndex = -1;
                if (SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo)
                {
                    grdItems.Columns["colBatch"].Visible = false;
                    grdItems.Columns["colExpiry"].Visible = false;
                }
                if (this.StockAddItemId > 0)
                {
                    this.SelectedItemId = this.StockAddItemId;
                    btnAddItem.PerformClick();
                }
                ShowNewDocumentNumber();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                isStockLoading = false;
            }
            //grdItems.Columns["colRemove"].DefaultCellStyle.ForeColor = Color.Red;
            //grdItems.Rows[0].Cells["colRemove"].Value = "X";
            //CustomCode.CalendarColumn col = new CustomCode.CalendarColumn();
            //col.Name = "colExpiryDate";
            //col.HeaderText = "Expiry";
            //grdItems.Columns.Insert(6, col);    

            grdItems.CellValueChanged += grdItems_CellValueChanged;
            resetSearchFocus();
        }
        private void LoadPurchaseOrder()
        {
            AddPOStockVM Po;

            using (unitOfWork = new UnitOfWork())
            {
                Po = unitOfWork.PurchaseOrderRepository.GetPOById(this.PurchaseOrderId);
            }
            if (Po.SupId > 0)
            {
                cmbSuppliers.SelectedValue = Po.SupId;
            }
            isStockLoading = true;
            foreach (ItemsVM i in Po.Items)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ConsumedStock - (i.ExpiredStock - i.ExpiredConsumedStock);
                }
                else
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ConsumedStock - (i.ExpiredStock - i.ExpiredConsumedStock) + i.HoldStock;
                }
                i.Unit = i.UnitValue == 0 ? i.Unit : "Units";
                i.ItemQuantity = i.UnitValue == 0 ? i.ItemQuantity / i.ConversionUnit : i.ItemQuantity;
                i.UnitCost = i.UnitValue == 0 ? i.UnitCost * i.ConversionUnit : i.UnitCost;
                object[] gr = {
                                      0,
                                      i.ItemId,
                                      i.ItemName,
                                      i.Unit,
                                      i.ConversionUnit,
                                      i.AvailableStock, // available quantity
                                      i.ItemQuantity.ToString(),
                                      0, // bonus quantiy
                                      i.UnitCost,
                                      i.ItemQuantity * i.UnitCost,
                                      i.RetailPrice,
                                      "",
                                      "",
                                      0, // bonus discount
                                      0, //discount
                                      "%",
                                      "Lumpsum", //discoutn type
                                      0, // salestax
                                      "%",
                                      "Lumpsum",  // salestax type
                                      i.ItemQuantity * i.UnitCost
                                  };
                grdItems.Rows.Add(gr);

            }
            isStockLoading = false;
            var tpl = GetGrandTotal();
            txtGrandTotalCost.Text = tpl.Item1.ToString();
            txtGrandTotalRetail.Text = tpl.Item2.ToString();
        }
        private void LoadStockForEdit()
        {
            StockVM s;
            using (unitOfWork = new UnitOfWork())
            {
                s = unitOfWork.StockRepository.GetStockById_Show(this.StockEditID);
            }
            txtDocumentNo.Text = s.DocumentNo.ToString();
            if (s.Supplier != null)
            {
                cmbSuppliers.SelectedValue = s.Supplier.SupplierID;
            }
            else
            {
                cmbSuppliers.SelectedIndex = -1;
            }
            txtSupplierInvoiceNo.Text = s.SupplierInvoiceNo;
            dtpDate.Value = (DateTime)s.CreatedAt;
            dtpSupplierInvoiceDate.Value = (DateTime)s.SupplierIvoiceDate;
            isStockLoading = true;
            foreach (StockItemVM i in s.StockItems)
            {
                if (i.Quantity > 0)
                {
                    i.Quantity = i.Unit == 0 ? i.Quantity / i.ConversionUnit : i.Quantity; // convert quantity back and forth depending on unit.
                    i.UnitCost = i.Unit == 0 ? i.UnitCost * i.Item.ConversionUnit : i.UnitCost;
                    i.Discount = i.Unit == 0 ? i.Discount * i.Item.ConversionUnit : i.Discount;
                    i.SalesTax = i.Unit == 0 ? i.SalesTax * i.Item.ConversionUnit : i.SalesTax;
                    i.BonusQuantity = i.Unit == 0 ? i.BonusQuantity / i.ConversionUnit : i.BonusQuantity;
                    i.BonusDiscount = i.Unit == 0 ? i.BonusDiscount * i.ConversionUnit : i.BonusDiscount;
                    string exp = i.Batch.Expiry == null ? "" : i.Batch.Expiry.Value.ToShortDateString();
                    // 0 : available quantity
                    grdItems.Rows.Add(i.StockItemId, i.Item.ItemId, i.Item.ItemName, i.UnitString, i.Item.ConversionUnit, 0, i.Quantity, i.BonusQuantity, i.UnitCost, i.TotalCost, i.RetailPrice, i.Batch.BatchName, exp, i.BonusDiscount, i.Discount, i.PercDiscType == 0 ? "%" : "Value", i.DiscountType == 0 ? "Lumpsum" : "Per Item", i.SalesTax, i.PercSalesTaxType == 0 ? "%" : "Value", i.SalesTaxType == 0 ? "Lumpsum" : "Per Item", i.NetValue, i.StockConsumed, i.DiscountVal, i.SalesTaxVal);
                }
            }
            isStockLoading = false;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colConsumedQty"].Value) > 0)
                {
                    r.DefaultCellStyle.BackColor = SharedVariables.DisabledColor;
                }
            }
            var tpl = GetGrandTotal();
            txtGrandTotalCost.Text = tpl.Item1.ToString();
            txtGrandTotalRetail.Text = tpl.Item2.ToString();
        }
        private void bgWItemsLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                cmbSuppliers.Enabled = false;
                lblLoadingSuppliers.Visible = true;
            });

            //using (unitOfWork = new UnitOfWork())
            //{
            //    //Items = unitOfWork.ItemRspository.GetActiveItems().ToList(); don't load , now these will be loaded at runtime
            //    Suppliers = unitOfWork.SupplierRepository.GetAll().ToList();
            //}
        }
        private void bgWItemsLoader_RunWokrerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                //cmbItems.SelectedIndexChanged -= cmbItems_SelectedIndexChanged;
                //cmbItems.DataSource = Items;
                //cmbItems.ValueMember = "ItemId";
                //cmbItems.DisplayMember = "ItemName";
                //cmbItems.SelectedIndexChanged += cmbItems_SelectedIndexChanged;
                //cmbItems.Enabled = true;
                //txtSearchItems.TextChanged -= txtSearchItems_TextChanged;
                //txtSearchItems.Enabled = true;
                //txtSearchItems.TextChanged += txtSearchItems_TextChanged;

                Suppliers.Insert(0, new Supplier { SupplierID = 0, Name = "Select Supplier" });
                cmbSuppliers.DataSource = Suppliers;
                cmbSuppliers.ValueMember = "SupplierId";
                cmbSuppliers.DisplayMember = "Name";
                cmbSuppliers.Enabled = true;
                lblLoadingSuppliers.Visible = false;

                if (this.StockEditID > 0)
                {
                    LoadStockForEdit();
                }
                else
                {
                    if (this.PurchaseOrderId > 0)
                    {
                        LoadPurchaseOrder();
                    }
                    this.dtpExpiry.MinDate = DateTime.Now;
                    this.dtpExpiry.Format = DateTimePickerFormat.Custom;
                    this.dtpExpiry.CustomFormat = " ";
                    dtpExpiry.Tag = 0;
                    ShowNewDocumentNumber();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading form data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void bgWSupplierssLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                cmbSuppliers.Enabled = false;
                lblLoadingSuppliers.Visible = true;
            });

            using (unitOfWork = new UnitOfWork())
            {
                Suppliers = unitOfWork.SupplierRepository.GetAll().ToList();
            }

        }
        private void bgWSuppliersLoader_RunWokrerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Supplier Select = new Supplier();
            Select.Name = "Select";
            Select.SupplierID = 0;
            Suppliers.Insert(0, Select);
            cmbSuppliers.DataSource = Suppliers;
            cmbSuppliers.ValueMember = "SupplierId";
            cmbSuppliers.DisplayMember = "Name";
            cmbSuppliers.Enabled = true;
            lblLoadingSuppliers.Visible = false;
        }
        private void LoadItemsAndSuppliers()
        {
            //this.bgWItemsLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWItemsLoader_DoWork);
            //this.bgWItemsLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWItemsLoader_RunWokrerCompleted);
            //this.bgWItemsLoader.RunWorkerAsync();
        }
        //private void LoadSuppliers()
        //{
        //    //this.bgWSuppliersLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWSupplierssLoader_DoWork);
        //    //this.bgWSuppliersLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWSuppliersLoader_RunWokrerCompleted);
        //    //this.bgWSuppliersLoader.RunWorkerAsync();  
        //}
        private void ShowNewDocumentNumber()
        {
            using (unitOfWork = new UnitOfWork())
            {
                txtDocumentNo.Text = unitOfWork.StockRepository.GetNewDocumentNumber().ToString();
            }
        }
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) { return; }

                if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
                {
                    double ConsumedQty = 0; double.TryParse(grdItems.Rows[e.RowIndex].Cells["colConsumedQty"].Value.ToString(), out ConsumedQty);
                    if (ConsumedQty > 0)
                    {
                        MessageBox.Show("Item Has Consumed Quantity, it Can't be Updated/Removed.", "Invliad Operation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    if (e.RowIndex == EditingRowIndex) { Clear(); EditingRowIndex = -1; }
                    else if (e.RowIndex < EditingRowIndex) { EditingRowIndex--; }
                    long StockItemId = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colStockItemId"].Value);
                    double nv = 0;
                    double.TryParse(grdItems.Rows[e.RowIndex].Cells["colNetValue"].Value.ToString(), out nv);
                    txtGrandTotalCost.Text = (double.Parse(txtGrandTotalCost.Text) - nv).ToString();
                    if (StockItemId > 0) { grdItems.Rows[e.RowIndex].Visible = false; }
                    else { grdItems.Rows.RemoveAt(e.RowIndex); }
                    //StockItemsList.RemoveAt(e.RowIndex);
                }
                //if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
                //{
                //    Clear();
                //    //int ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                //    FillDataForEdit(grdItems.Rows[e.RowIndex]);
                //    btnAddItem.Text = "Update Item";
                //    grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = SharedVariables.EditingColor;
                //    EditingRowIndex = e.RowIndex;
                //    //cmbItems.Enabled = false;
                //    this.uC_SearchItems1.Enabled = false;
                //    long StockItemId = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colStockItemId"].Value);
                //    //if (StockItemId > 0)
                //    //{
                //    //    numRetailPrice.Enabled = false;
                //    //}
                //    //grdItems.Rows.RemoveAt(e.RowIndex);
                //    // StockItemsList.RemoveAt(e.RowIndex);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FillDataForEdit(DataGridViewRow r)
        {
            txtStockItemId.Text = r.Cells["colStockItemId"].Value.ToString();
            //cmbItems.SelectedValue = r.Cells["colItemId"].Value;
            this.SelectedItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            using (unitOfWork = new UnitOfWork())
            {
                string unit = unitOfWork.ItemRspository.GetUnitByItemId(this.SelectedItemId);
                //cmbUnit.Items.Add(unit);
                //cmbUnit.Items.Add("Units");
            }
            this.SelectedItemName = r.Cells["colItem"].Value.ToString();
            //cmbUnit.SelectedIndex = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
            numConvUnit.Value = Convert.ToInt32(r.Cells["colConvUnit"].Value);
            this.uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
            this.uC_SearchItems1.SetText = r.Cells["colItem"].Value.ToString();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            numQty.Value = Convert.ToInt32(r.Cells["colQuantity"].Value);
            numUnitCost.Value = Convert.ToDecimal(r.Cells["colUnitCost"].Value);
            txtTotalCost.Text = r.Cells["colTotalCost"].Value.ToString();
            numRetailPrice.Value = Convert.ToDecimal(r.Cells["colRetailPrice"].Value);
            txtBatch.Text = string.IsNullOrEmpty(r.Cells["colBatch"].Value.ToString()) ? "" : r.Cells["colBatch"].Value.ToString();
            if (r.Cells["colExpiry"].Value == null) { dtpExpiry.CustomFormat = " "; }
            else
            {
                dtpExpiry.CustomFormat = "dd/MM/yyyy";
                dtpExpiry.Value = Convert.ToDateTime(r.Cells["colExpiry"].Value);
            }
            numDiscount.Value = Convert.ToDecimal(r.Cells["colDiscount"].Value);
            cmbDiscountType.SelectedItem = string.IsNullOrEmpty(r.Cells["colDiscountTypeString"].Value.ToString()) ? "Value" : r.Cells["colDiscountTypeString"].Value.ToString();
            numSalesTax.Value = Convert.ToDecimal(r.Cells["colSalesTax"].Value == DBNull.Value ? 0 : r.Cells["colSalesTax"].Value);
            cmbSalesTaxType.SelectedItem = r.Cells["colSalesTaxTypeString"].Value == null ? "Value" : r.Cells["colSalesTaxTypeString"].Value.ToString();
            txtNetValue.Text = r.Cells["colNetValue"].Value.ToString();
        }
        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            //frmAddSupplier f = new frmAddSupplier();
            //f.Show();
            //f.FormClosed += new FormClosedEventHandler(frmAddSupplier_Closed);
        }
        private void frmAddSupplier_Closed(object sender, FormClosedEventArgs e)
        {
            loadSuppliers();
        }
        private void btnAttachImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Select Invoice Image";
            fd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;";
            fd.CheckFileExists = true;
            fd.CheckPathExists = true;
            fd.Multiselect = false;
            fd.RestoreDirectory = true;
            DialogResult rs = fd.ShowDialog();
            if (rs == DialogResult.OK)
            {
                string selectedFile = fd.FileName;
                lblImagePath.Text = selectedFile;
            }
        }
        private void lblRemoveFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lblImagePath.Text = "No File Choosen";
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                //if (IsValidInput())
                //{
                //StockItem objStockItem = new StockItem();
                //FillObject(objStockItem);
                //if (EditingRowIndex >= 0)
                //{
                //UpdateRow(objStockItem);
                //}
                if (this.SelectedItemId > 0)
                {
                    if (!IsItemAlreadyAdded(this.SelectedItemId))
                    {
                        ItemsVM vm = new ItemsVM();

                        using (unitOfWork = new UnitOfWork())
                        {
                            vm = unitOfWork.ItemRspository.GetItemWithAvQty(this.SelectedItemId);
                        }

                        grdItems.Rows.Add(
                            0,
                            vm.ItemId,
                             vm.ItemName, //objStockItem.ItemName,
                            vm.Unit, //objStockItem.UnitString,
                            vm.ConversionUnit, //objStockItem.ConversionUnit,
                            vm.AvailableStock / vm.ConversionUnit, // available quantity
                            "", //objStockItem.Quantity,
                            "", //bonus quantity
                            vm.UnitCost * vm.ConversionUnit, //objStockItem.UnitCost,
                            0, //objStockItem.TotalCost,
                            vm.RetailPrice * vm.ConversionUnit, //objStockItem.RetailPrice,
                            "", //objStockItem.BatchName,
                            "", //objStockItem.BatchExpiry,
                            0, //objStockItem.Discount,
                            0, // bonus discount
                            "%",//Percent discout type
                            "Lumpsum", //objStockItem.DiscoutnType,
                            0, //objStockItem.SalesTax,
                            "%", // percent Sales Tax Type
                            "Lumpsum", //objStockItem.SalesTaxType,
                            0, //objStockItem.NetValue,
                            0,
                            0, //objStockItem.DiscountValue,
                            0 //objStockItem.SalesTaxValue
                            );
                    }
                    Clear();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            grdItems.CellValueChanged += grdItems_CellValueChanged;
            if (!chkRepeatedScans.Checked)
            {
                grdItems.CurrentCell = grdItems.Rows[grdItems.Rows.Count - 1].Cells["colQuantity"];
                grdItems.BeginEdit(true);
            }
            else
            {
                resetSearchFocus();
            }
        }
        private void UpdateRow(StockItem ObjStockItem)
        {
            DataGridViewRow r = grdItems.Rows[EditingRowIndex];
            r.Cells["colStockItemId"].Value = ObjStockItem.StockItemId;
            r.Cells["colItemId"].Value = ObjStockItem.ItemId;
            r.Cells["colItem"].Value = ObjStockItem.ItemName;

            r.Cells["colUnit"].Value = ObjStockItem.UnitString;
            r.Cells["colConvUnit"].Value = ObjStockItem.ConversionUnit;
            r.Cells["colQuantity"].Value = ObjStockItem.Quantity;
            r.Cells["colUnitCost"].Value = ObjStockItem.UnitCost;
            r.Cells["colTotalCost"].Value = ObjStockItem.TotalCost;
            r.Cells["colRetailPrice"].Value = ObjStockItem.RetailPrice;
            r.Cells["colBatch"].Value = ObjStockItem.BatchName;
            r.Cells["colExpiry"].Value = ObjStockItem.BatchExpiry;
            r.Cells["colDiscount"].Value = ObjStockItem.Discount;
            r.Cells["colDiscountTypeString"].Value = ObjStockItem.DiscoutnTypeString;
            r.Cells["colSalesTax"].Value = ObjStockItem.SalesTax;
            r.Cells["colSalesTaxTypeString"].Value = ObjStockItem.SalesTaxTypeString;
            r.Cells["colNetValue"].Value = ObjStockItem.NetValue;
            r.Cells["colDiscountType"].Value = ObjStockItem.DiscountType;
            r.Cells["colSalesTaxType"].Value = ObjStockItem.SalesTaxType;
            r.Cells["colDiscountValue"].Value = ObjStockItem.DiscountValue;
            r.Cells["colSalesTaxValue"].Value = ObjStockItem.SalesTaxValue;
            r.Cells["colIsRowUpdated"].Value = true;
            r.DefaultCellStyle.BackColor = Color.White;
        }
        private bool IsItemAlreadyAdded(long ItemId)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == ItemId)//Convert.ToInt32(cmbItems.SelectedValue))
                {
                    MessageBox.Show("Selected Item Already Exists", "Alredy Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }
            }

            return false;
        }
        private void Clear()
        {
            //cmbItems.SelectedIndex = -1;
            //cmbItems.Enabled = true;
            this.uC_SearchItems1.Enabled = true;
            numRetailPrice.Enabled = true;
            numQty.Value = 0;
            numUnitCost.Value = 0;
            txtTotalCost.Text = "";
            numRetailPrice.Value = 0;
            txtBatch.Text = "";
            dtpExpiry.CustomFormat = " ";
            dtpExpiry.Tag = 0;
            numDiscount.Value = 0;
            cmbDiscountType.SelectedIndex = 0;
            numSalesTax.Value = 0;
            cmbSalesTaxType.SelectedIndex = 0;
            txtNetValue.Text = "0";
            btnAddItem.Text = "Add Item";
            //cmbUnit.SelectedIndex = -1;
            //cmbUnit.Items.Clear();
            numConvUnit.Value = 0;
            uC_SearchItems1.SetText = ""; // this will reset selectedItemId and SelectedItemName
            if (EditingRowIndex >= 0)
            {
                grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
            }
            EditingRowIndex = -1;
            setNumericUD_toEmpty();
        }
        private bool IsValidInput()
        {

            bool ErrFound = false;
            if (this.SelectedItemId <= 0)//(cmbItems.SelectedIndex < 0)
            {
                ErrSelectItem.Visible = true;
                ErrFound = true;
                ErrMessage.Visible = true;
                this.uC_SearchItems1.Focus();
                return false;
            }
            else
            {
                ErrSelectItem.Visible = false;
            }
            if (numQty.Value <= 0)
            {
                ErrQuantity.Visible = true;
                ErrFound = true;
                ErrMessage.Visible = true;
                numQty.Focus();
                return false;
            }
            else
            {
                ErrQuantity.Visible = false;
            }

            if (numConvUnit.Value <= 0)
            {
                errConvUnit.Visible = true;
                ErrFound = true;
                ErrMessage.Visible = true;
                numConvUnit.Focus();
                return false;
            }
            else
            {
                errConvUnit.Visible = false;
            }

            if (!ErrFound)
            {
                ErrMessage.Visible = false;
                return true;
            }
            else
            {
                ErrMessage.Visible = true;
                return false;
            }
        }
        private void FillObject(StockItem objStockItem)
        {
            objStockItem.StockItemId = int.Parse(txtStockItemId.Text);
            objStockItem.ItemId = this.SelectedItemId;//int.Parse(cmbItems.SelectedValue.ToString());
            objStockItem.ItemName = this.SelectedItemName; //cmbItems.GetItemText(cmbItems.SelectedItem);
            //objStockItem.UnitString = cmbUnit.GetItemText(cmbUnit.SelectedItem);
            objStockItem.ConversionUnit = (int)numConvUnit.Value;
            objStockItem.Quantity = (int)numQty.Value;
            objStockItem.UnitCost = (double)numUnitCost.Value;
            objStockItem.TotalCost = double.Parse(txtTotalCost.Text);
            objStockItem.RetailPrice = (double)numRetailPrice.Value;
            objStockItem.BatchName = txtBatch.Text.Trim() == "" ? "No Batch" : txtBatch.Text.Trim();
            if (dtpExpiry.Tag.ToString().Equals("1"))
            {
                objStockItem.BatchExpiry = dtpExpiry.Value;
            }
            else
            {
                objStockItem.BatchExpiry = null;
            }
            objStockItem.Discount = (double)numDiscount.Value;
            string DiscountTypeString = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem);
            objStockItem.DiscoutnTypeString = DiscountTypeString;
            objStockItem.DiscountType = DiscountTypeString.ToLower() == "lumpsum" ? 0 : 1;
            string SalesTaxTypeString = cmbSalesTaxType.GetItemText(cmbSalesTaxType.SelectedItem);
            objStockItem.SalesTaxTypeString = SalesTaxTypeString;
            objStockItem.SalesTaxType = SalesTaxTypeString.ToLower() == "lumpsum" ? 0 : 1;
            objStockItem.SalesTax = (double)numSalesTax.Value;
            objStockItem.NetValue = double.Parse(txtNetValue.Text);
            objStockItem.CreatedAt = DateTime.Now;
            objStockItem.UpdatedAt = DateTime.Now;
            if (objStockItem.DiscountType == 1)
            {
                objStockItem.DiscountValue = objStockItem.Discount;
            }
            else
            {
                objStockItem.DiscountValue = objStockItem.Discount * objStockItem.Quantity;
            }

            if (objStockItem.SalesTaxType == 1)
            {
                objStockItem.SalesTaxValue = objStockItem.SalesTax;
            }
            else
            {
                objStockItem.SalesTaxValue = objStockItem.SalesTax * objStockItem.Quantity;
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void cmbDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CalculateValues();
        }
        private void cmbSalesTaxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CalculateValues();
        }
        private void numUnitCost_ValueChanged(object sender, EventArgs e)
        {
            //CalculateValues();
        }
        private void numQty_ValueChanged(object sender, EventArgs e)
        {
            //CalculateValues();
        }
        private void numDiscount_ValueChanged(object sender, EventArgs e)
        {
            //CalculateValues();
        }
        private void CalculateValues(DataGridViewRow r)
        {
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            double TotalCost = 0;

            double qty = 0; double.TryParse(r.Cells["colQuantity"].Value.ToString(), out qty); //(int)numQty.Value;
            double Bonqty = 0; double.TryParse(r.Cells["colBonusQty"].Value.ToString(), out Bonqty); //(int)numQty.Value;

            double UnitCost = Convert.ToDouble(r.Cells["colUnitCost"].Value);//(double)numUnitCost.Value;
            double Disc = Convert.ToDouble(r.Cells["colDiscount"].Value);//(double)numDiscount.Value;
            double SalesTax = Convert.ToDouble(r.Cells["colSalesTax"].Value);//(double)numSalesTax.Value;
            TotalCost = qty * UnitCost;
            //TotalCost = TotalCost - ((TotalCost / qty) * Bonqty);
            r.Cells["colTotalCost"].Value = TotalCost;
            //txtTotalCost.Text = TotalCost.ToString();
            string DiscType = r.Cells["colDiscType"].Value.ToString();
            string PercDiscType = r.Cells["colPercDiscType"].Value.ToString();
            string StType = r.Cells["colSaleStaxType"].Value.ToString();
            string PercStType = r.Cells["colPercSaleStaxType"].Value.ToString();
            if (DiscType.ToLower() == "per item")
            {
                Disc = qty * Disc;
                if (PercDiscType == "%")
                {
                    Disc = (Disc / 100) * (qty * UnitCost);
                }
            }
            else
            {
                if (PercDiscType == "%")
                {
                    Disc = (Disc / 100) * (qty * UnitCost);
                }
            }

            if (StType.ToLower() == "per item")
            {
                SalesTax = qty * SalesTax;
                if (PercStType == "%")
                {
                    SalesTax = (SalesTax / 100) * (qty * SalesTax);
                }
            }
            else
            {
                if (PercStType == "%")
                {
                    SalesTax = (SalesTax / 100) * (qty * UnitCost);
                }
            }
            double bonDisc = Bonqty * UnitCost;
            r.Cells["colBonusDiscount"].Value = bonDisc;
            //get discount values for bonus quantity
            double netValue = TotalCost - Disc + SalesTax;
            r.Cells["colNetValue"].Value = netValue;
            r.Cells["colDiscountValue"].Value = Disc;
            r.Cells["colSalesTaxValue"].Value = SalesTax;
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (grdItems.Rows.Count > 0)
            {
                DialogResult rs = MessageBox.Show("There Are Some Un-Saved Items, Are You Sure You Want To Clear It", "Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (rs == DialogResult.OK)
                {
                    Clear();
                    grdItems.Rows.Clear();
                    ///StockItemsList.Clear();
                    this.ShowNewDocumentNumber();
                    dtpSupplierInvoiceDate.Value = dtpDate.Value = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
                    this.cmbSuppliers.SelectedIndex = -1;
                    this.txtSupplierInvoiceNo.Text = "";
                    this.txtGrandTotalCost.Text = "0";
                    this.isStockLoading = false;
                    this.IsLoadingPrvSoldItems = false;
                    this.AddedStockId = 0;
                }
            }
        }

        private void Save()
        {
            this.ActionTime = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
            if (cmbSuppliers.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select supplier before proceeding", "Select Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (grdItems.Rows.Count <= 0)
            {
                MessageBox.Show("Please Add Some Items Before Saving", "Ivalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                double qty = 0;
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    double.TryParse(r.Cells["colQuantity"].Value.ToString(), out qty);
                    if (qty < 1)
                    {
                        MessageBox.Show("Some items have 0 quantity, please check before proceeding.", "Invaliud Quantity", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                if (this.StockEditID > 0)
                {
                    UpdateStock();
                }
                else// this will be used in case of ADD-STOCK from purchase order as well.
                {
                    InsertStock();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }


        private bool validatePrices()
        {
            double retailPrice = 0, costPrice = 0;
            string itemName = "";
            int rowIndex = 0;
            bool priceNotFound = false;
            string message = "";
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                costPrice = 0; retailPrice = 0; itemName = ""; priceNotFound = false;
                double.TryParse(r.Cells["colUnitCost"].Value.ToString(), out costPrice);
                double.TryParse(r.Cells["colRetailPrice"].Value.ToString(), out retailPrice);
                itemName = r.Cells["colItem"].Value.ToString();
                rowIndex = r.Index;
                if (costPrice <= 0 && retailPrice <= 0)
                {
                    message = "Item (" + itemName + ") at row # " + (rowIndex + 1) + " does not conatin cost price and retail price.";
                    priceNotFound = true;
                }
                else if (costPrice <= 0)
                {
                    message = "Item (" + itemName + ") at row # " + (rowIndex + 1) + " does not conatin cost price.";
                    priceNotFound = true;
                }
                else if (retailPrice <= 0)
                {
                    message = "Item (" + itemName + ") at row # " + (rowIndex + 1) + " does not conatin retail price.";
                    priceNotFound = true;
                }
                if (priceNotFound)
                {
                    break;
                }
            }
            if (priceNotFound)
            {
                MessageBox.Show(message, "Price Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }
        private void InsertStock()
        {
            if (!validatePrices()) { return; }
            using (TransactionScope ts = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Stock objStock = new Stock();
                        if (cmbSuppliers.SelectedIndex > 0)
                        {
                            objStock.SupplierId = Convert.ToInt64(cmbSuppliers.SelectedValue); // unitOfWork.SupplierRepository.GetById(cmbSuppliers.SelectedValue);
                        }
                        objStock.SupplierInvoiceNo = txtSupplierInvoiceNo.Text.Trim();
                        if (lblImagePath.Text.ToLower() == "no file choosen")
                            objStock.ImagePath = null;
                        else
                            objStock.ImagePath = lblImagePath.Text;
                        if (objStock.ImagePath != null)
                        {
                            objStock.ImagePath = SharedFunctions.CopyFileToLocal(objStock.ImagePath);
                        }
                        objStock.StockDate = dtpDate.Value;
                        objStock.CreatedAt = this.ActionTime.Value;
                        objStock.UpdatedAt = this.ActionTime.Value;
                        objStock.SupplierIvoiceDate = dtpSupplierInvoiceDate.Value;
                        objStock.DocumentNo = decimal.Parse(txtDocumentNo.Text);
                        objStock.IsNew = true;
                        objStock.IsActive = true;
                        objStock.GrandTotal = double.Parse(txtGrandTotalCost.Text);
                        objStock.GrandTotal_RetailPrice = double.Parse(txtGrandTotalRetail.Text);
                        objStock.TotalPaid = 0;
                        //objStock.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        objStock.UserId = SharedVariables.LoggedInUser.UserId;
                        objStock.PurchaseOrder = unitOfWork.PurchaseOrderRepository.GetById(this.PurchaseOrderId);
                        objStock.IsOpeningStock = false;
                        objStock.StockItems = new List<StockItem>();
                        List<long> itemIds = new List<long>();
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            StockItem objStockItem = new StockItem();
                            FillObject(r, objStockItem);
                            if (!SharedVariables.AdminInvoiceSetting.AllowBelowCostSale && objStockItem.RetailPrice < objStockItem.UnitCost)
                            {
                                //r.DefaultCellStyle.BackColor = Color.GreenYellow;
                                //grdItems.ClearSelection();
                                //grdItems.Rows[r.Index].DefaultCellStyle.BackColor= Color.RosyBrown;
                                MessageBox.Show("Retail price can't be below cost price, you can change setting in under POS invoice setting.", "Invalid Retail Price", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            //update item for retail price
                            Item objItem = unitOfWork.ItemRspository.GetById(objStockItem.Item.ItemId);
                            if (!objItem.IsSyncable)
                            {
                                objItem.IsSyncable = true;
                            }
                            else if (objItem.IsSynced)
                            {
                                objItem.IsNew = false;
                                objItem.IsUpdate = true;
                                objItem.IsSynced = false;
                            }

                            objItem.UpdatedAt = this.ActionTime.Value;
                            objItem.RetailPrice = objStockItem.RetailPrice;
                            objItem.UnitCostPrice = objStockItem.UnitCost;
                            unitOfWork.GetDbContext().Entry(objItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            //unitOfWork.ItemRspository.Update(objItem);
                            //end update item for retail price
                            objStock.StockItems.Add(objStockItem);
                            itemIds.Add(objStockItem.ItemId);
                        }
                        List<MissedSale> msList = unitOfWork.MissedsalesRepository.GetMissedSalesByItemIds(itemIds);
                        foreach (var i in msList)
                        {
                            i.UpdatedAt = this.ActionTime.Value;
                            //i.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                            i.UserId = SharedVariables.LoggedInUser.UserId;
                            i.IsActive = false;
                            //unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                            unitOfWork.MissedsalesRepository.Update(i);
                        }
                        var st = unitOfWork.StockRepository.Insert(objStock);
                        unitOfWork.Save();
                        this.AddedStockId = st.StockId;
                        this.AddedDocNo = st.DocumentNo;
                        this.AddedSupplierId = st.SupplierId.Value;
                        if (this.PurchaseOrderId > 0)
                        {
                            PurchaseOrder po = unitOfWork.PurchaseOrderRepository.GetById(this.PurchaseOrderId);
                            if (po != null)
                            {
                                po.StockId = objStock.StockId;
                                unitOfWork.PurchaseOrderRepository.Update(po);
                                unitOfWork.Save();
                            }
                        }
                    }
                    ts.Complete();
                }
                catch (Exception ex)
                {
                    ts.Dispose();
                    this.AddedStockId = 0;
                    this.AddedDocNo = 0;
                    throw new Exception("Error occurred while inserting stock, please try again.");
                }
            }
            MessageBox.Show("Items Stock Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private void UpdateStock()
        {
            if (!validatePrices()) { return; }
            using (unitOfWork = new UnitOfWork())
            {
                Stock objStock = unitOfWork.StockRepository.GetStockById_Edit(this.StockEditID);
                objStock.Supplier = unitOfWork.SupplierRepository.GetById(cmbSuppliers.SelectedValue);
                objStock.SupplierInvoiceNo = txtSupplierInvoiceNo.Text.Trim();
                if (lblImagePath.Text.ToLower() == "no file choosen")
                    objStock.ImagePath = null;
                else
                    objStock.ImagePath = lblImagePath.Text;
                if (objStock.ImagePath != null)
                {
                    objStock.ImagePath = SharedFunctions.CopyFileToLocal(objStock.ImagePath);
                }
                objStock.UpdatedAt = this.ActionTime.Value;
                if (objStock.IsSynced)
                {
                    objStock.IsNew = false;
                    objStock.IsUpdate = true;
                    objStock.IsSynced = false;
                }
                objStock.SupplierIvoiceDate = dtpSupplierInvoiceDate.Value;
                objStock.GrandTotal = double.Parse(txtGrandTotalCost.Text);
                objStock.GrandTotal_RetailPrice = double.Parse(txtGrandTotalRetail.Text);

                objStock.TotalPaid = 0;
                objStock.UserId = SharedVariables.LoggedInUser.UserId;
                bool isExistingItem = false;
                bool isDeleted = false;
                long StockItemId = -1;
                bool IsRowUpdated = false;
                List<long> itemIds = new List<long>();
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    IsRowUpdated = false;
                    if (r.Cells["colIsRowUpdated"].Value != null)
                    {
                        bool.TryParse(r.Cells["colIsRowUpdated"].Value.ToString(), out IsRowUpdated);
                    }
                    isDeleted = r.Visible ? false : true;
                    //if (!IsRowUpdated && !isDeleted)
                    //{
                    //    continue;
                    //}
                    isExistingItem = false;
                    StockItemId = Convert.ToInt32(r.Cells["colStockItemId"].Value);
                    StockItemId = StockItemId == 0 ? -1 : StockItemId; // because each new item has default value of 0 from stockitemid
                    StockItem objStockItem = new StockItem();
                    objStockItem.StockItemId = StockItemId;
                    FillObject(r, objStockItem);

                    foreach (StockItem i in objStock.StockItems)
                    {
                        if (i.StockItemId == StockItemId && i.StockItemId != -1) // -1 shows newly added item
                        {
                            i.BatchId = objStockItem.BatchId;
                            i.Unit = objStockItem.Unit;
                            i.Quantity = objStockItem.Quantity;
                            i.BonusQuantity = objStockItem.BonusQuantity;
                            i.UnitCost = objStockItem.UnitCost;
                            i.TotalCost = objStockItem.TotalCost;
                            i.RetailPrice = objStockItem.RetailPrice;
                            i.Discount = objStockItem.Discount;
                            i.DiscountType = objStockItem.DiscountType;
                            i.PercDiscType = objStockItem.PercDiscType;
                            i.SalesTax = objStockItem.SalesTax;
                            i.SalesTaxType = objStockItem.SalesTaxType;
                            i.PercSalesTaxType = objStockItem.PercSalesTaxType;
                            i.NetValue = objStockItem.NetValue;
                            i.DiscountValue = objStockItem.DiscountValue;
                            i.SalesTaxValue = objStockItem.SalesTaxValue;
                            i.UpdatedAt = objStockItem.UpdatedAt;
                            i.IsUpdate = true;
                            i.BatchExpiry = objStockItem.BatchExpiry;
                            if (isDeleted)
                            {
                                i.IsActive = false;
                                i.UpdatedAt = this.ActionTime.Value;
                            }
                            i.IsSynced = false;
                            unitOfWork.GetDbContext().Entry(i).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            //update item for retail price
                            Item objItem = unitOfWork.ItemRspository.GetById(objStockItem.ItemId);
                            objItem.IsSynced = false;
                            objItem.IsUpdate = true;
                            objItem.UpdatedAt = this.ActionTime.Value;
                            objItem.RetailPrice = objStockItem.RetailPrice;
                            objItem.UnitCostPrice = objStockItem.UnitCost;
                            unitOfWork.GetDbContext().Entry(objItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            unitOfWork.ItemRspository.Update(objItem);
                            //end update item for retail price
                            isExistingItem = true;
                            itemIds.Add(objStockItem.ItemId);
                            break;
                        }
                    }
                    if (!isExistingItem)
                    {
                        //update item for retail price
                        Item objItem = unitOfWork.ItemRspository.GetById(objStockItem.Item.ItemId);
                        if (!objItem.IsSyncable)
                        {
                            objItem.IsSyncable = true;
                        }
                        else if (objItem.IsSynced)
                        {
                            objItem.IsNew = false;
                            objItem.IsUpdate = true;
                            objItem.IsSynced = false;
                        }

                        objItem.UpdatedAt = this.ActionTime.Value;
                        objItem.RetailPrice = objStockItem.RetailPrice;
                        unitOfWork.GetDbContext().Entry(objItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        unitOfWork.ItemRspository.Update(objItem);
                        //end update item for retail price
                        objStock.StockItems.Add(objStockItem);
                    }
                }
                List<MissedSale> msList = unitOfWork.MissedsalesRepository.GetMissedSalesByItemIds(itemIds);
                foreach (var i in msList)
                {
                    i.UpdatedAt = this.ActionTime.Value;
                    i.UserId = SharedVariables.LoggedInUser.UserId;
                    i.IsActive = false;
                    //unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                    unitOfWork.MissedsalesRepository.Update(i);
                }
                unitOfWork.StockRepository.Update(objStock);
                unitOfWork.Save();
            }
            MessageBox.Show("Items Stock Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private void FillObject(DataGridViewRow r, StockItem obj)
        {
            int ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            obj.ItemId = ItemId;
            obj.BatchName = r.Cells["colBatch"].Value.ToString() == "" ? "No Batch" : r.Cells["colBatch"].Value.ToString();
            obj.Unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
            DateTime? batchExp = null;
            if (!string.IsNullOrEmpty(r.Cells["colExpiry"].Value.ToString()))
            {
                batchExp = Convert.ToDateTime(r.Cells["colExpiry"].Value);
            }
            obj.BatchExpiry = batchExp;
            if (obj.BatchExpiry != null)
            {
                Batch objBatch = unitOfWork.BatchRepository.FindItemBatch(obj.BatchName.ToLower(), ItemId);
                if (objBatch == null)
                {
                    objBatch = new Batch();
                    objBatch.BatchName = obj.BatchName;
                    if (obj.BatchExpiry != null)
                    {
                        objBatch.Expiry = (DateTime)obj.BatchExpiry;
                    }
                    else
                    {
                        objBatch.Expiry = null;
                    }
                    objBatch.CreatedAt = this.ActionTime.Value;
                    objBatch.UpdatedAt = this.ActionTime.Value;
                    objBatch.IsNew = true;
                    objBatch.IsActive = true;
                    objBatch.UserId = SharedVariables.LoggedInUser.UserId;
                    objBatch = unitOfWork.BatchRepository.InsertBatch(objBatch);
                    obj.BatchId = objBatch.BatchId;
                }
                else
                {
                    if (obj.BatchExpiry != null)
                    {
                        objBatch.Expiry = (DateTime)obj.BatchExpiry;
                        objBatch.UpdatedAt = this.ActionTime.Value;
                        objBatch.IsUpdate = true;
                        objBatch.IsSynced = false;
                        unitOfWork.GetDbContext().Entry(objBatch).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                }
            }
            else
            {
                obj.BatchId = 1; // default batch
            }

            if (obj.StockItemId <= 0) // don't get item from db when stock is being updated, because item updation is not allowed
            {
                obj.Item = unitOfWork.ItemRspository.GetById(Convert.ToInt64(r.Cells["colItemId"].Value));
            }
            obj.ConversionUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
            double qty = 0; double.TryParse(r.Cells["colQuantity"].Value.ToString(), out qty);
            obj.Quantity = qty;
            obj.Quantity = obj.Unit == 0 ? obj.Quantity * obj.ConversionUnit : obj.Quantity; // to save quantity always in broken form.
            double bQty = 0; double.TryParse(r.Cells["colBonusQty"].Value.ToString(), out bQty);
            obj.BonusQuantity = bQty;
            obj.BonusQuantity = obj.Unit == 0 ? obj.BonusQuantity * obj.ConversionUnit : obj.BonusQuantity; // to save quantity always in broken form.
            obj.UnitCost = Convert.ToDouble(r.Cells["colUnitCost"].Value);
            obj.UnitCost = obj.Unit == 0 ? obj.UnitCost / obj.ConversionUnit : obj.UnitCost; // to save quantity always in broken form.
            obj.TotalCost = Convert.ToDouble(r.Cells["colTotalCost"].Value);
            obj.RetailPrice = Convert.ToDouble(r.Cells["colRetailPrice"].Value);
            obj.RetailPrice = obj.Unit == 0 ? obj.RetailPrice / obj.ConversionUnit : obj.RetailPrice; // to save quantity always in broken form.
            obj.Discount = Convert.ToDouble(r.Cells["colDiscount"].Value);
            ///obj.Discount = obj.Unit == 0 ? obj.Discount / obj.ConversionUnit : obj.Discount; // to save quantity always in broken form.
            obj.BonusDiscount = Convert.ToDouble(r.Cells["colBonusDiscount"].Value); // no need to break discount
            //obj.BonusDiscount = obj.Unit == 0 ? obj.BonusDiscount / obj.ConversionUnit : obj.BonusDiscount; // to save quantity always in broken form.
            obj.PercDiscType = r.Cells["colPercDiscType"].Value.ToString().ToLower() == "%" ? 0 : 1;
            obj.DiscountType = r.Cells["colDiscType"].Value.ToString().ToLower() == "lumpsum" ? 0 : 1;
            obj.SalesTax = Convert.ToDouble(r.Cells["colSalesTax"].Value);
            obj.SalesTax = obj.Unit == 0 ? obj.SalesTax / obj.ConversionUnit : obj.SalesTax; // to save quantity always in broken form.
            obj.PercSalesTaxType = r.Cells["colPercSalesTaxType"].Value.ToString().ToLower() == "%" ? 0 : 1;
            obj.SalesTaxType = r.Cells["colSalesTaxType"].Value.ToString().ToLower() == "lumpsum" ? 0 : 1;
            obj.NetValue = Convert.ToDouble(r.Cells["colNetValue"].Value);
            obj.CreatedAt = this.ActionTime.Value;
            obj.UpdatedAt = this.ActionTime.Value;
            obj.UserId = SharedVariables.LoggedInUser.UserId;
            obj.IsNew = true;
            obj.IsActive = true;
            obj.DiscountValue = Convert.ToDouble(r.Cells["colDiscountValue"].Value);
            obj.SalesTaxValue = Convert.ToDouble(r.Cells["colSalesTaxValue"].Value);
        }
        private void dtpExpiry_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            dtp.CustomFormat = "dd/MM/yyyy";
            dtp.Tag = 1;
        }
        private void lblClearExpiry_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            dtpExpiry.CustomFormat = " ";
            dtpExpiry.Tag = 0;
        }
        private void numSalesTax_ValueChanged(object sender, EventArgs e)
        {
            //CalculateValues();
        }

        private void GetITtemUnit(DataGridViewRow r, int ItemId)
        {
            using (unitOfWork = new UnitOfWork())
            {
                //numUnitCost.Value = (decimal)unitOfWork.ItemRspository.GetUnitCostByItemId(ItemId);
                Item objItem = unitOfWork.ItemRspository.GetById(ItemId);
                if (objItem == null)
                {
                    MessageBox.Show("No item selected, please try loading item again.", "Item not loaded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string unit = objItem.Unit;
                DataGridViewComboBoxCell dgCmbBatch = (DataGridViewComboBoxCell)r.Cells["colUnit"];
                dgCmbBatch.Items.Add(unit);
                dgCmbBatch.Items.Add("Units");
            }
        }
        private void LoadItemData(DataGridViewRow r)
        {
        }
        private void dtpExpiry_Enter(object sender, EventArgs e)
        {
            dtpExpiry.Select();
            SendKeys.Send("%{DOWN}");
        }
        private void dtpExpiry_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                lblClearExpiry_LinkClicked(null, null);
            }
            else if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void cmbItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void numQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void numRetailPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void txtBatch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void numDiscount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void cmbDiscountType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void numSalesTax_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void cmbSalesTaxType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void grdItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                DataGridViewRow r = grdItems.Rows[e.RowIndex];
                if (this.IsLoadingPrvSoldItems)
                {
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    string unit = r.Cells["colUnit"].Value.ToString();
                    DataGridViewComboBoxCell dgCmbUnit = (DataGridViewComboBoxCell)r.Cells["colUnit"];
                    dgCmbUnit.Items.Add(unit);
                    dgCmbUnit.Items.Add("Units");
                    dgCmbUnit.Value = "Units";
                    //double NetValue = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colNetValue"].Value);
                    //txtGrandTotal.Text = (double.Parse(txtGrandTotal.Text) + NetValue).ToString();
                    grdItems.CellValueChanged += grdItems_CellValueChanged;
                }
                else if (isStockLoading)
                {
                    GetITtemUnit(grdItems.Rows[e.RowIndex], Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value));
                }
                else
                {
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    string unit = r.Cells["colUnit"].Value.ToString();
                    DataGridViewComboBoxCell dgCmbUnit = (DataGridViewComboBoxCell)r.Cells["colUnit"];
                    dgCmbUnit.Items.Add(r.Cells["colUnit"].Value.ToString());
                    dgCmbUnit.Items.Add("Units");
                    dgCmbUnit.Value = unit;
                    double NetValue = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colNetValue"].Value);
                    txtGrandTotalCost.Text = (double.Parse(txtGrandTotalCost.Text) + NetValue).ToString();
                    grdItems.CellValueChanged += grdItems_CellValueChanged;
                }
                var val = r.Cells["colQuantity"].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void grdItems_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            try
            {
                double NetValue = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colNetValue"].Value);
                if (txtGrandTotalCost.Text == "") txtGrandTotalCost.Text = "0";
                txtGrandTotalCost.Text = (double.Parse(txtGrandTotalCost.Text) - NetValue).ToString();
            }
            catch (Exception)
            {
            }
        }
        private void grdItems_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            try
            {
                double NetValue = Convert.ToDouble(e.Row.Cells["colNetValue"].Value);
                if (txtGrandTotalCost.Text == "") txtGrandTotalCost.Text = "0";
                txtGrandTotalCost.Text = (double.Parse(txtGrandTotalCost.Text) - NetValue).ToString();
            }
            catch (Exception)
            {
            }
        }
        private void uC_SearchItems1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }


        private void LoadSupplierItems(bool LoadLowStockItems)
        {
            if (cmbSuppliers.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select supplier before proceeding.", "Select Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int SupId = Convert.ToInt32(cmbSuppliers.SelectedValue);

            grdItems.Rows.Clear();

            try
            {
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                this.isStockLoading = true;
                List<int> missedSaleItems = new List<int>();
                List<ItemsVM> vmList = new List<ItemsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    vmList = unitOfWork.ItemRspository.LoadSupItems(SupId);
                }

                foreach (var vm in vmList)
                {
                    if (LoadLowStockItems)
                    {
                        if (vm.AvailableStock >= vm.ReorderingLevel)
                        {
                            continue;
                        }
                    }

                    grdItems.Rows.Add(
                        0,
                        vm.ItemId,
                         vm.ItemName, //objStockItem.ItemName,
                        vm.Unit, //objStockItem.UnitString,
                        vm.ConversionUnit, //objStockItem.ConversionUnit,
                        vm.AvailableStock / vm.ConversionUnit, // available quantity
                        "", //objStockItem.Quantity,
                        "", //bonus quantity
                        vm.UnitCost * vm.ConversionUnit, //objStockItem.UnitCost,
                        0, //objStockItem.TotalCost,
                        vm.RetailPrice * vm.ConversionUnit, //objStockItem.RetailPrice,
                        "", //objStockItem.BatchName,
                        "", //objStockItem.BatchExpiry,
                        0, //objStockItem.Discount,
                        0, // bonus discount
                        "%",//Percent discout type
                        "Lumpsum", //objStockItem.DiscoutnType,
                        0, //objStockItem.SalesTax,
                        "%", // percent Sales Tax Type
                        "Lumpsum", //objStockItem.SalesTaxType,
                        0, //objStockItem.NetValue,
                        0,
                        0, //objStockItem.DiscountValue,
                        0 //objStockItem.SalesTaxValue
                        );

                    Clear();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally
            {
                this.isStockLoading = true;
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
        }

        private void btnSearchItems_Click(object sender, EventArgs e)
        {
            frmSearchItem f = new frmSearchItem();
            f.OnItemSelected += frmSearchItem_OnItemSelected;
            f.ShowDialog();
        }
        private void frmSearchItem_OnItemSelected(int ItemId, int Quantity)
        {
            try
            {
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                if (!IsItemAlreadyAdded(ItemId))
                {
                    ItemsVM vm = new ItemsVM();
                    using (unitOfWork = new UnitOfWork())
                    {
                        vm = unitOfWork.ItemRspository.GetItemWithAvQty(ItemId);
                    }
                    grdItems.Rows.Add(
                        0,
                        vm.ItemId,
                         vm.ItemName, //objStockItem.ItemName,
                        vm.Unit, //objStockItem.UnitString,
                        vm.ConversionUnit, //objStockItem.ConversionUnit,
                        vm.AvailableStock / vm.ConversionUnit, // available quantity
                        "", //objStockItem.Quantity,
                        "", //bonus quantity
                        vm.UnitCost * vm.ConversionUnit, //objStockItem.UnitCost,
                        0, //objStockItem.TotalCost,
                        vm.RetailPrice * vm.ConversionUnit, //objStockItem.RetailPrice,
                        "", //objStockItem.BatchName,
                        "", //objStockItem.BatchExpiry,
                        0, //objStockItem.Discount,
                        0, // bonus discount
                        "%",//Percent discout type
                        "Lumpsum", //objStockItem.DiscoutnType,
                        0, //objStockItem.SalesTax,
                        "%", // percent Sales Tax Type
                        "Lumpsum", //objStockItem.SalesTaxType,
                        0, //objStockItem.NetValue,
                        0,
                        0, //objStockItem.DiscountValue,
                        0 //objStockItem.SalesTaxValue
                        );
                }
                Clear();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
            finally
            {
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
        }

        private void grdItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            var r = grdItems.Rows[e.RowIndex];
            if (r.Cells["colQuantity"].Value == null) r.Cells["colQuantity"].Value = "";
            if (r.Cells["colBonusQty"].Value == null) r.Cells["colBonusQty"].Value = "";

            if (e.RowIndex < 0) return;
            int ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);

            if (e.ColumnIndex == grdItems.Columns["colQuantity"].Index
                || e.ColumnIndex == grdItems.Columns["colUnitCost"].Index
                || e.ColumnIndex == grdItems.Columns["colRetailPrice"].Index
                || e.ColumnIndex == grdItems.Columns["colDiscount"].Index
                || e.ColumnIndex == grdItems.Columns["colRetailPrice"].Index
                || e.ColumnIndex == grdItems.Columns["colDiscType"].Index
                || e.ColumnIndex == grdItems.Columns["colPercDiscType"].Index
                || e.ColumnIndex == grdItems.Columns["colSalesTax"].Index
                || e.ColumnIndex == grdItems.Columns["colSalesTaxType"].Index
                || e.ColumnIndex == grdItems.Columns["colPercSalesTaxType"].Index
                || e.ColumnIndex == grdItems.Columns["colBonusQty"].Index
                )
            {
                CalculateValues(grdItems.Rows[e.RowIndex]);
                var tpl = GetGrandTotal();
                txtGrandTotalCost.Text = tpl.Item1.ToString();
                txtGrandTotalRetail.Text = tpl.Item2.ToString();
            }
            else if (e.ColumnIndex == grdItems.Columns["colBonusQty"].Index)
            {
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                double bq = 0; double.TryParse(grdItems.Rows[e.RowIndex].Cells["colBonusQty"].Value.ToString(), out bq);
                if (bq > 0)
                {
                    grdItems.Rows[e.RowIndex].Cells["colDiscount"].ReadOnly = true;
                    grdItems.Rows[e.RowIndex].Cells["colDiscount"].Value = 0;
                }
                else
                {
                    grdItems.Rows[e.RowIndex].Cells["colDiscount"].ReadOnly = false;
                }
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }

            else if (e.ColumnIndex == grdItems.Columns["colRetailPrice"].Index || e.ColumnIndex == grdItems.Columns["colUnitCost"].Index)
            {

                double unitCost = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colUnitCost"].Value);
                double retailPrice = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colRetailPrice"].Value);
                if (!SharedVariables.AdminInvoiceSetting.AllowBelowCostSale && retailPrice < unitCost)
                {
                    StockItemDetailRunTimeVM objItem = new StockItemDetailRunTimeVM();
                    using (unitOfWork = new UnitOfWork())
                    {
                        objItem = unitOfWork.ItemRspository.GetItemDetail(ItemId);
                    }
                    if (objItem == null)
                    {
                        MessageBox.Show("No item selected, please try loading item again.", "Item not loaded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    // here re-assigining values from database to reset values to defaults.
                    grdItems.Rows[e.RowIndex].Cells["colUnitCost"].Value = objItem.CostPrice;
                    grdItems.Rows[e.RowIndex].Cells["colRetailPrice"].Value = objItem.RetailPrice;
                    MessageBox.Show("Retail price can't be below cost price, you can change setting in under POS invoice setting.", "Invalid Retail Price", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    CalculateValues(grdItems.Rows[e.RowIndex]);
                    var tpl = GetGrandTotal();
                    txtGrandTotalCost.Text = tpl.Item1.ToString();
                    txtGrandTotalRetail.Text = tpl.Item2.ToString();
                    grdItems.CellValueChanged += grdItems_CellValueChanged;
                    return;
                }
            }
            else if (e.ColumnIndex == grdItems.Columns["colUnit"].Index)
            {
                StockItemDetailRunTimeVM objItem = new StockItemDetailRunTimeVM();
                string unit = grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString();
                using (unitOfWork = new UnitOfWork())
                {
                    objItem = unitOfWork.ItemRspository.GetItemDetail(ItemId);
                }
                if (objItem == null)
                {
                    MessageBox.Show("Failed to load item stock details, please try again.", "Item not loaded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                double avStock = objItem.TotalStock + objItem.AdjustedStock - objItem.ExpiredStock - objItem.ConsumedStock;
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                if (unit.ToLower() != "units")
                {
                    grdItems.Rows[e.RowIndex].Cells["ColItem"].Value = objItem.ItemName;
                    grdItems.Rows[e.RowIndex].Cells["colConvUnit"].Value = objItem.ConversionUnit;
                    grdItems.Rows[e.RowIndex].Cells["colUnitCost"].Value = objItem.CostPrice * objItem.ConversionUnit;
                    grdItems.Rows[e.RowIndex].Cells["colRetailPrice"].Value = objItem.RetailPrice * objItem.ConversionUnit;
                    grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value = (int)Math.Floor((double)avStock / objItem.ConversionUnit);
                }
                else
                {
                    grdItems.Rows[e.RowIndex].Cells["ColItem"].Value = objItem.ItemName;
                    grdItems.Rows[e.RowIndex].Cells["colConvUnit"].Value = objItem.ConversionUnit;
                    grdItems.Rows[e.RowIndex].Cells["colUnitCost"].Value = objItem.CostPrice;
                    grdItems.Rows[e.RowIndex].Cells["colRetailPrice"].Value = objItem.RetailPrice;
                    grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value = avStock;
                }
                CalculateValues(grdItems.Rows[e.RowIndex]);
                var tpl = GetGrandTotal();
                txtGrandTotalCost.Text = tpl.Item1.ToString();
                txtGrandTotalRetail.Text = tpl.Item2.ToString();
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }
        private Tuple<double, double> GetGrandTotal()
        {
            double gt = 0;
            double gt_retail = 0;
            double qty = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                qty = 0;
                gt += Convert.ToDouble(r.Cells["colNetValue"].Value);
                double.TryParse(r.Cells["colQuantity"].Value.ToString(), out qty);
                gt_retail += (qty * Convert.ToDouble(r.Cells["colRetailPrice"].Value));
            }
            return new Tuple<double, double>(gt, gt_retail);
        }
        private void grdItems_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colDiscType"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colSalesTaxType"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colPercDiscType"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colPercSalesTaxType"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colUnit"].Index
                )
            {
                grdItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void grdItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colDiscount"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colSalesTax"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colUnitCost"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colRetailPrice"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colBonusQty"].Index
                )
            {
                e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress); // to allow onyl integer input here in the datagridview
            }
            //else
            //{
            //    e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
            //}

        }
        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsControl(e.KeyChar)
            //        && !char.IsNumber(e.KeyChar))
            //{
            //    e.Handled = true;
            //}

            //// only allow one decimal point
            //if (e.KeyChar == '.'
            //    && (sender as TextBox).Text.IndexOf('.') > -1)
            //{
            //    e.Handled = true;
            //}
            //if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            //{
            //    e.Handled = true;
            //}


            //if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void grdItems_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == grdItems.Columns["colExpiry"].Index)
            {
                dlgExpiry d = new dlgExpiry();
                int x = grdItems.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).X + 231 + 20; //231 : left panel width, 20 : extra width to fill the left gap
                int y = grdItems.Location.Y;//grdItems.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false).Y + ;// +grdItems.ColumnHeadersHeight + 48 + 10; //48 : top panel height : 10 : extra height
                d.StartPosition = FormStartPosition.Manual;
                d.Location = new Point(x, y);
                d.ShowDialog();
                DateTime? exp = d.selectedDate;
                if (exp != null)
                {
                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    grdItems.Rows[e.RowIndex].Cells["colExpiry"].Value = exp.Value.ToShortDateString();
                    grdItems.CellValueChanged += grdItems_CellValueChanged;
                }
            }
        }

        private void grdItems_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        private void btnLoadMissedSales_Click(object sender, EventArgs e)
        {

        }
        private void btnLPrvSoldItems_Click(object sender, EventArgs e)
        {

        }

        private void btnMoreOpt_Click(object sender, EventArgs e)
        {
            frmAddStock_MoreOpts f = new frmAddStock_MoreOpts();
            f.StartPosition = FormStartPosition.Manual;
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
            cmbSuppliers.SelectedValue = f.selectedSupplierId;
            // check value in shared varibale
            switch (f.LoadStockAction)
            {
                case SharedVariables.LoadStockAction.AllItemsBySupplier: AllItemsBySupplier(); break;
                case SharedVariables.LoadStockAction.LowStockItemsBySupplier: LowStockItemsBySupplier(); break;
                case SharedVariables.LoadStockAction.PrevSoldItemsBySupplier: PrevSoldItemsBySupplier(); break;
                case SharedVariables.LoadStockAction.MissedSaleItemsBySupplier: MissedSaleItemsBySupplier(); break;
                case SharedVariables.LoadStockAction.AllLowStockItems: AllLowStockItems(); break;
                case SharedVariables.LoadStockAction.AllPreviouslySoldItems: AllPreviouslySoldItems(); break;
                case SharedVariables.LoadStockAction.AllMissedSaleItems: AllMissedSaleItems(); break;
            }
            f.Dispose();
        }

        private void AllItemsBySupplier()
        {
            if (cmbSuppliers.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select supplier before proceeding.", "Select Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int SupId = Convert.ToInt32(cmbSuppliers.SelectedValue);

            grdItems.Rows.Clear();

            try
            {
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                this.isStockLoading = true;
                List<int> missedSaleItems = new List<int>();
                List<ItemsVM> vmList = new List<ItemsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    vmList = unitOfWork.ItemRspository.LoadSupItems(SupId);
                }

                foreach (var vm in vmList)
                {
                    grdItems.Rows.Add(
                        0,
                        vm.ItemId,
                         vm.ItemName, //objStockItem.ItemName,
                        vm.Unit, //objStockItem.UnitString,
                        vm.ConversionUnit, //objStockItem.ConversionUnit,
                        vm.AvailableStock / vm.ConversionUnit, // available quantity
                        "", //objStockItem.Quantity,
                        "", //bonus quantity
                        vm.UnitCost * vm.ConversionUnit, //objStockItem.UnitCost,
                        0, //objStockItem.TotalCost,
                        vm.RetailPrice * vm.ConversionUnit, //objStockItem.RetailPrice,
                        "", //objStockItem.BatchName,
                        "", //objStockItem.BatchExpiry,
                        0, //objStockItem.Discount,
                        0, // bonus discount
                        "%",//Percent discout type
                        "Lumpsum", //objStockItem.DiscoutnType,
                        0, //objStockItem.SalesTax,
                        "%", // percent Sales Tax Type
                        "Lumpsum", //objStockItem.SalesTaxType,
                        0, //objStockItem.NetValue,
                        0,
                        0, //objStockItem.DiscountValue,
                        0 //objStockItem.SalesTaxValue
                        );

                    Clear();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally
            {
                this.isStockLoading = true;
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
        }
        private void LowStockItemsBySupplier()
        {
            if (cmbSuppliers.SelectedIndex <= 0)
            {
                MessageBox.Show("Please select supplier before proceeding.", "Select Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int SupId = Convert.ToInt32(cmbSuppliers.SelectedValue);

            grdItems.Rows.Clear();

            try
            {
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                this.isStockLoading = true;
                List<int> missedSaleItems = new List<int>();
                List<ItemsVM> vmList = new List<ItemsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    vmList = unitOfWork.ItemRspository.LoadSupItems(SupId);
                }

                foreach (var vm in vmList)
                {

                    if (vm.AvailableStock >= vm.ReorderingLevel)
                    {
                        continue;
                    }

                    grdItems.Rows.Add(
                        0,
                        vm.ItemId,
                         vm.ItemName, //objStockItem.ItemName,
                        vm.Unit, //objStockItem.UnitString,
                        vm.ConversionUnit, //objStockItem.ConversionUnit,
                        vm.AvailableStock / vm.ConversionUnit, // available quantity
                        "", //objStockItem.Quantity,
                        "", //bonus quantity
                        vm.UnitCost * vm.ConversionUnit, //objStockItem.UnitCost,
                        0, //objStockItem.TotalCost,
                        vm.RetailPrice * vm.ConversionUnit, //objStockItem.RetailPrice,
                        "", //objStockItem.BatchName,
                        "", //objStockItem.BatchExpiry,
                        0, //objStockItem.Discount,
                        0, // bonus discount
                        "%",//Percent discout type
                        "Lumpsum", //objStockItem.DiscoutnType,
                        0, //objStockItem.SalesTax,
                        "%", // percent Sales Tax Type
                        "Lumpsum", //objStockItem.SalesTaxType,
                        0, //objStockItem.NetValue,
                        0,
                        0, //objStockItem.DiscountValue,
                        0 //objStockItem.SalesTaxValue
                        );

                    Clear();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally
            {
                this.isStockLoading = true;
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
        }
        private void PrevSoldItemsBySupplier()
        {
            try
            {
                this.IsLoadingPrvSoldItems = true; // this is needed to makes "Units" slected by default on this action
                DlgAddStockFilter f = new DlgAddStockFilter();
                f.StartPosition = FormStartPosition.Manual;
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                List<StockAddFilterVM> lst = f.SelectedItems;

                foreach (var vm in lst)
                {
                    grdItems.Rows.Add(
                        0,
                        vm.ItemId,
                        vm.ItemName, //objStockItem.ItemName,
                        vm.Unit, //objStockItem.UnitString,
                        vm.ConversionUnit, //objStockItem.ConversionUnit,
                        vm.AvailableStock / vm.ConversionUnit, // available quantity
                        vm.Quantity, //objStockItem.Quantity,
                        "", //bonus quantity
                        vm.UnitCost * vm.ConversionUnit, //objStockItem.UnitCost,
                        0, //objStockItem.TotalCost,
                        vm.RetailPrice * vm.ConversionUnit, //objStockItem.RetailPrice,
                        "", //objStockItem.BatchName,
                        "", //objStockItem.BatchExpiry,
                        0, //objStockItem.Discount,
                        0, // bonus discount
                        "%",//Percent discout type
                        "Lumpsum", //objStockItem.DiscoutnType,
                        0, //objStockItem.SalesTax,
                        "%", // percent Sales Tax Type
                        "Lumpsum", //objStockItem.SalesTaxType,
                        0, //objStockItem.NetValue,
                        0,
                        0, //objStockItem.DiscountValue,
                        0 //objStockItem.SalesTaxValue
                        );
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
            finally
            {
                this.IsLoadingPrvSoldItems = false;
            }
        }
        private void MissedSaleItemsBySupplier()
        {
            MessageBox.Show("This method is not implemented yet, please wait for upcomming updates.", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void AllLowStockItems()
        {
            MessageBox.Show("This method is not implemented yet, please wait for upcomming updates.", "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void AllPreviouslySoldItems()
        {
            try
            {
                this.IsLoadingPrvSoldItems = true; // this is needed to makes "Units" slected by default on this action
                DlgAddStockFilter f = new DlgAddStockFilter();
                f.StartPosition = FormStartPosition.Manual;
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                List<StockAddFilterVM> lst = f.SelectedItems;

                foreach (var vm in lst)
                {
                    grdItems.Rows.Add(
                        0,
                        vm.ItemId,
                        vm.ItemName, //objStockItem.ItemName,
                        vm.Unit, //objStockItem.UnitString,
                        vm.ConversionUnit, //objStockItem.ConversionUnit,
                        vm.AvailableStock / vm.ConversionUnit, // available quantity
                        vm.Quantity, //objStockItem.Quantity,
                        "", //bonus quantity
                        vm.UnitCost * vm.ConversionUnit, //objStockItem.UnitCost,
                        0, //objStockItem.TotalCost,
                        vm.RetailPrice * vm.ConversionUnit, //objStockItem.RetailPrice,
                        "", //objStockItem.BatchName,
                        "", //objStockItem.BatchExpiry,
                        0, //objStockItem.Discount,
                        0, // bonus discount
                        "%",//Percent discout type
                        "Lumpsum", //objStockItem.DiscoutnType,
                        0, //objStockItem.SalesTax,
                        "%", // percent Sales Tax Type
                        "Lumpsum", //objStockItem.SalesTaxType,
                        0, //objStockItem.NetValue,
                        0,
                        0, //objStockItem.DiscountValue,
                        0 //objStockItem.SalesTaxValue
                        );
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
            finally
            {
                this.IsLoadingPrvSoldItems = false;
            }
        }
        private void AllMissedSaleItems()
        {
            try
            {
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                this.isStockLoading = true;
                List<long> missedSaleItems = new List<long>();
                List<ItemsVM> vmList = new List<ItemsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    missedSaleItems = unitOfWork.MissedsalesRepository.GetMissedSalesItemsIds();
                    vmList = unitOfWork.ItemRspository.GetMissedSalesItems(missedSaleItems);
                }

                foreach (var vm in vmList)
                {
                    grdItems.Rows.Add(
                        0,
                        vm.ItemId,
                         vm.ItemName, //objStockItem.ItemName,
                        vm.Unit, //objStockItem.UnitString,
                        vm.ConversionUnit, //objStockItem.ConversionUnit,
                        vm.AvailableStock / vm.ConversionUnit, // available quantity
                        "", //objStockItem.Quantity,
                        "", //bonus quantity
                        vm.UnitCost * vm.ConversionUnit, //objStockItem.UnitCost,
                        0, //objStockItem.TotalCost,
                        vm.RetailPrice * vm.ConversionUnit, //objStockItem.RetailPrice,
                        "", //objStockItem.BatchName,
                        "", //objStockItem.BatchExpiry,
                        0, //objStockItem.Discount,
                        0, // bonus discount
                        "%",//Percent discout type
                        "Lumpsum", //objStockItem.DiscoutnType,
                        0, //objStockItem.SalesTax,
                        "%", // percent Sales Tax Type
                        "Lumpsum", //objStockItem.SalesTaxType,
                        0, //objStockItem.NetValue,
                        0,
                        0, //objStockItem.DiscountValue,
                        0 //objStockItem.SalesTaxValue
                        );
                }
                Clear();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally
            {
                this.isStockLoading = true;
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
        }

        private void btnSupLowStockItems_Click(object sender, EventArgs e)
        {

        }

        private void btlLoadSupProds_Click(object sender, EventArgs e)
        {

        }

        private void frmAddStock_Shown(object sender, EventArgs e)
        {
            if (this.StockEditID > 0)
            {
                btnPayBill.Visible = false;
                LoadStockForEdit();
            }
            else if (this.PurchaseOrderId > 0)
            {
                LoadPurchaseOrder();
            }
        }

        private void chkBarcodePref_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBarcodePref.Checked)
            {
                chkSearchItemPref.Checked = false;
            }
            //Properties.Settings.Default.UseBarcodeSearch = chkBarcodePref.Checked;
            //Properties.Settings.Default.Save();
            resetSearchFocus();
        }

        private void chkSearchItemPref_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSearchItemPref.Checked)
            {
                chkBarcodePref.Checked = false;
            }
            //Properties.Settings.Default.UseBarcodeSearch = chkBarcodePref.Checked;
            //Properties.Settings.Default.Save();
            resetSearchFocus();
        }
        private void resetSearchFocus()
        {
            if (chkSearchItemPref.Checked)
            {
                //this.uC_SearchItems1.Focus();
                this.uC_SearchItems1.SetText = "";
                this.uC_SearchItems1.SetFocus();
            }
            else
            {
                //txtBarcode.Text = "";
                //txtBarcode.Focus();
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                using (unitOfWork = new UnitOfWork())
                {
                    this.SelectedItemId = (int)unitOfWork.ItemRspository.getItemIdByBarcode("txtBarcode.Text.Trim().ToLower()");
                    if (this.SelectedItemId > 0)
                    {
                        btnAddItem.PerformClick();
                        this.SelectedItemId = 0;
                    }
                    else
                    {
                        //txtBarcode.Text = "";
                        this.SelectedItemId = 0;
                        MessageBox.Show("No item found with given barcode.", "Item Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void btnPayBill_Click(object sender, EventArgs e)
        {
            this.Save();
            if (this.AddedStockId > 0)
            {
                frmStockPayment f = new frmStockPayment(this.AddedStockId, this.AddedDocNo, this.AddedSupplierId);
                f.ShowDialog();
            }
        }

        private void line1_Click(object sender, EventArgs e)
        {

        }
    }
}