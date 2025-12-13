

using Restaurant_MS_UI.App.MenuBar;

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmPurchaseOrder : Form
    {
        private UnitOfWork unitOfWork;
        //List<PurchaseOrderItem> PurchaseOrderItemsList = new List<PurchaseOrderItem>();
        PurchaseOrder LoadedPurchaseOrder;
        private bool IsOldOrderLoaded = false;
        long PurchaseOrderId = 0;
        int EditingRowIndex = -1;
        BackgroundWorker bgWItemsLoader = new BackgroundWorker();
        List<Item> Items = new List<Item>();
        private bool IsFormLoading = false;
        private bool IsLoadingLowStocks = false;
        private bool IsLoadingPrvSoldItems = false;
        DateTime ActionTime;

        Item LoadedItemData;

        int SelectedItemId = 0;
        string SelectedItemName = "";
        public frmPurchaseOrder()
        {

            InitializeComponent();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }
        public frmPurchaseOrder(long PurchaseOrderId)
        {
            InitializeComponent();
            this.PurchaseOrderId = PurchaseOrderId;
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }

        private void uC_SearchItems1_OnItemSelectionChanged()
        {
            try
            {
                this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;
                if (this.SelectedItemId > 0)
                {
                    btnAddItem.PerformClick();
                }
                //this.SelectedItemName = this.uC_SearchItems1.SelectedItemName;
                //if (!this.SelectedItemId > 0)
                //{
                //    btnClear.perf
                //    LoadItemData(this.SelectedItemId);
                //}
                //else
                //{
                //    btnClear.PerformClick();
                //    return;
                //}
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while loading selected item data", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }            //MessageBox.Show(this.SelectedItemId + " | " + this.SelectedItemName);
        }
        private void LoadItemData(int ItemId)
        {
            using (unitOfWork = new UnitOfWork())
            {
                LoadedItemData = unitOfWork.ItemRspository.GetById(ItemId);
                double avQty = unitOfWork.ItemRspository.GetItemAvailableQty(ItemId);

                cmbUnit.Items.Add(LoadedItemData.Unit);
                cmbUnit.Items.Add("Units");
                cmbUnit.SelectedIndexChanged -= cmbUnit_SelectedIndexChanged;
                cmbUnit.SelectedIndex = 0;
                cmbUnit.SelectedIndexChanged += cmbUnit_SelectedIndexChanged;
                if (cmbUnit.SelectedIndex == 0)
                {
                    //numAvailableQty.Value = (decimal)Math.Floor((double)avQty / LoadedItemData.ConversionUnit);
                    numAvailableQty.Value = (decimal)(avQty / LoadedItemData.ConversionUnit);
                }
                else
                {
                    numAvailableQty.Value = (decimal)avQty;
                }
                numConversionUnit.Value = LoadedItemData.ConversionUnit;
                numUnitCost.Value = (decimal)LoadedItemData.UnitCostPrice;
            }
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
            if (keyData == (Keys.Alt | Keys.D))
            {
                btnClear.PerformClick();
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
                    //cmbSelectBatch.Focus();
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
                    //cmbSelectBatch.Focus();
                }
                return true;
            }
            #endregion grid short cuts
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            //frmAddSupplier fSup = new frmAddSupplier();
            //Form f = SharedFunctions.IsFormOpen(fSup.Name);
            //if (f == null)
            //{
            //    fSup.MdiParent = this.MdiParent;
            //    fSup.Show();
            //    fSup.FormClosed += new FormClosedEventHandler(frmAddSupplier_Closed);
            //}
            //else
            //{
            //    f.Focus();
            //}
        }
        private void frmAddSupplier_Closed(object sender, FormClosedEventArgs e)
        {
            LoadSuppliers();
        }
        private void bgWItemsLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                uC_SearchItems1.Enabled = false;
                //cmbItems.Enabled = false;                
            });

            //if (SharedVariables.POSItems == null)
            //{
            using (unitOfWork = new UnitOfWork())
            {
                Items = unitOfWork.ItemRspository.GetActiveItems().ToList();
            }
            //}
        }
        private void bgWItemsLoader_RunWokrerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //cmbItems.DataSource = Items;
            //cmbItems.ValueMember = "ItemId";
            //cmbItems.DisplayMember = "ItemName";
            //cmbItems.SelectedIndex = -1;
            //cmbItems.Enabled = true;
            uC_SearchItems1.Enabled = true;
        }
        private void LoadSuppliers()
        {
            cmbSuppliers.SelectedIndexChanged -= cmbSuppliers_SelectedIndexChanged;
            List<Supplier> suppliers = new List<Supplier>();
            using (unitOfWork = new UnitOfWork())
            {
                suppliers = unitOfWork.SupplierRepository.GetActiveSuppliers();
            }
            suppliers.Insert(0, new Supplier { SupplierID = 0, Name = "Select Supplier" });
            cmbSuppliers.DataSource = suppliers;
            cmbSuppliers.ValueMember = "SupplierID";
            cmbSuppliers.DisplayMember = "Name";
            cmbSuppliers.SelectedIndexChanged += cmbSuppliers_SelectedIndexChanged;
            cmbSuppliers.SelectedIndex = 0;
            //if (cmbSuppliers.Items.Count > 0)
            //{
            //    cmbSuppliers.SelectedIndex = 0;
            //    cmbSuppliers_SelectedIndexChanged(null, null);
            //}
        }
        private void setNumericUD_toEmpty()
        {
            numQuantity.ResetText();
            numUnitCost.ResetText();
        }
        private void LoadItems()
        {
            this.bgWItemsLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWItemsLoader_DoWork);
            this.bgWItemsLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWItemsLoader_RunWokrerCompleted);
            this.bgWItemsLoader.RunWorkerAsync();
            //Thread worker = new Thread(() => { LoadItems_Async(); });
            //worker.Name = "ItemsLoader";
            //worker.IsBackground = true;
            //worker.Start(); 
        }
        private void frmPurchaseOrder_Load(object sender, EventArgs e)
        {
            try
            {
                if (SharedFunctions.CheckDayClosed())
                {
                    this.BeginInvoke(new MethodInvoker(Close));
                    return;
                }
                dtpPurchaseOrderDate.Value = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                SharedFunctions.SetGridStyle(grdItems);
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnSave, btnClearAll, btnLoadLSI, btnSupAI, btnSupLSI, btnLoadPrvSoldItems });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnAddItem, btnClear, btnAddSupplier });
                LoadSuppliers();
                //LoadItems(); // this is using background worker
                if (this.PurchaseOrderId > 0)
                {
                    // PurchaseOrderItemsList.Clear();
                    LoadOrderByOrderId(this.PurchaseOrderId);
                    IsOldOrderLoaded = true;
                    txtGrandTotal.Text = GetGrandTotals().ToString();
                }
                else
                {
                    ShowNewPurchaseOrderNumber();
                }
                setNumericUD_toEmpty();
                grdItems.CellValueChanged += grdItems_CellValueChanged;

            }
            catch (Exception ex)
            {
                if (SharedVariables.ShowActualExceptionMessages)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(SharedVariables.GeneralErrMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //private void LoadItems()
        //{
        //    List<Item> itemsList = unitOfWork.ItemRspository.GetActiveItems();
        //    cmbItems.DataSource = itemsList;
        //    cmbItems.ValueMember = "ItemId";
        //    cmbItems.DisplayMember = "ItemName";
        //}
        private void LoadOrderByOrderId(long PoId)
        {
            this.IsFormLoading = true;
            LoadedPurchaseOrder = new PurchaseOrder();
            using (unitOfWork = new UnitOfWork())
            {
                LoadedPurchaseOrder = unitOfWork.PurchaseOrderRepository.GetPurchaseOrderById(PoId);
            }
            txtPurchaseOrderNo.Text = LoadedPurchaseOrder.PurchaseOrderNo.ToString();
            if (LoadedPurchaseOrder.Supplier != null)
            {
                cmbSuppliers.SelectedValue = LoadedPurchaseOrder.Supplier.SupplierID;
            }
            else
            {
                cmbSuppliers.SelectedIndex = -1;
            }

            txtTotalAmount.Text = LoadedPurchaseOrder.TotalAmount.ToString();
            foreach (PurchaseOrderItem poi in LoadedPurchaseOrder.PurchaseOrderItems)
            {
                if (poi.IsActive)
                {
                    AddItemToGrid(poi, (decimal)LoadedPurchaseOrder.PurchaseOrderNo);
                }
            }
            this.IsFormLoading = false;
        }

        private void ShowNewPurchaseOrderNumber()
        {
            using (unitOfWork = new UnitOfWork())
            {
                txtPurchaseOrderNo.Text = unitOfWork.PurchaseOrderRepository.GetNewPurchaseOrderNumber().ToString();
            }
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdItems.Rows.Count > 0)
                {
                    DialogResult rs = MessageBox.Show("There are Some Un-Saved Items In Grid, are you Sure you Want to clear", "Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (rs == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                cmbSuppliers.SelectedIndex = -1;
                grdItems.Rows.Clear();
                dtpPurchaseOrderDate.Value = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
                IsOldOrderLoaded = false;
                this.PurchaseOrderId = 0;
                LoadedPurchaseOrder = null;
                txtGrandTotal.Text = 0.ToString();
                Clear();
                ShowNewPurchaseOrderNumber();
                this.IsLoadingLowStocks = false;
                this.IsLoadingPrvSoldItems = false;
            }
            catch (Exception ex)
            {
                if (SharedVariables.ShowActualExceptionMessages)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(SharedVariables.GeneralErrMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            //long SupplierID = Convert.ToInt64(cmbSuppliers.SelectedValue);
            //cmbItems.DataSource = null;
            //if (SupplierID >= 0)
            //{
            //    IEnumerable<Item> SupplierItems `= unitOfWork.ItemRspository.GetItemsBySupplierID(SupplierID);
            //    cmbItems.DataSource = SupplierItems;
            //    cmbItems.ValueMember = "ItemId";
            //    cmbItems.DisplayMember = "ItemName";
            //}
            //if (cmbItems.Items.Count > 0)
            //{
            //    cmbItems.SelectedIndex = 0;
            //}
        }
        private void Clear()
        {
            numQuantity.ValueChanged -= numQuantity_ValueChanged;
            numUnitCost.ValueChanged -= numUnitCost_ValueChanged;
            //cmbItems.SelectedIndex = -1;
            cmbUnit.SelectedIndex = -1;
            cmbUnit.Items.Clear();
            numConversionUnit.Value = 0;
            numQuantity.Value = 0;
            numUnitCost.Value = 0;
            numAvailableQty.Value = 0;
            numAvailableQty.ResetText();
            setNumericUD_toEmpty();
            txtTotalAmount.Text = "";
            //cmbItems.Enabled = true;
            uC_SearchItems1.Enabled = true;
            if (EditingRowIndex > -1)
            {
                grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
            }
            EditingRowIndex = -1;
            numQuantity.ValueChanged += numQuantity_ValueChanged;
            numUnitCost.ValueChanged += numUnitCost_ValueChanged;
            this.uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
            this.uC_SearchItems1.SetText = "";
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            this.SelectedItemId = 0;
            LoadedItemData = new Item();
            this.uC_SearchItems1.SetFocus();
            //PurchaseOrderItemsList.Clear();
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                //if (IsValidInput())
                //{
                //    PurchaseOrderItem objPItem = new PurchaseOrderItem();
                //    FillObject(objPItem);
                //    if (EditingRowIndex >= 0)
                //    {
                //        UpdateRow(objPItem);
                //    }
                //    else
                //    {
                //        AddItemToGrid(objPItem);
                //    }
                //    Clear();
                //}
                if (this.SelectedItemId > 0)
                {
                    grdItems.Rows.Add(0, this.SelectedItemId);
                }
                else
                {
                    MessageBox.Show("Please select an item before proceeding.", "Select Item", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                if (SharedVariables.ShowActualExceptionMessages)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show(SharedVariables.GeneralErrMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            this.uC_SearchItems1.SetFocus();
        }
        private void UpdateRow(PurchaseOrderItem objPItem)
        {
            DataGridViewRow r = grdItems.Rows[EditingRowIndex];
            r.Cells["colUnit"].Value = cmbUnit.GetItemText(cmbUnit.SelectedItem);
            //r.Cells["colConvUnit"].Value = objPItem.ConversionUnit;
            r.Cells["colQuantity"].Value = objPItem.Quantity;
            r.Cells["colUnitCost"].Value = objPItem.UnitCost;
            r.Cells["colTotalAmount"].Value = objPItem.TotalAmount;
            r.DefaultCellStyle.BackColor = Color.White;
        }
        private void AddItemToGrid(PurchaseOrderItem objPItem)
        {
            if (ItemAlreadyExists(objPItem.ItemId)) return;
            //object[] objPurchaseItem = { objPItem.PurchaseOrderItemId, objPItem.ItemId, objPItem.ItemName, objPItem.UnitString, objPItem.ConversionUnit, objPItem.Quantity, objPItem.UnitCost, objPItem.TotalAmount };
            //grdItems.Rows.Add(objPurchaseItem);
            //PurchaseOrderItemsList.Add(objPItem);
        }
        private void AddItemToGrid(PurchaseOrderItem objItem, decimal PurchaseOrderNo)
        {
            PurchaseOrderItem objPItem = new PurchaseOrderItem();
            //FillObject(objPItem, objItem, PurchaseOrderNo);
            if (ItemAlreadyExists(objItem.Item.ItemId)) return;
            //objItem.UnitString = objItem.Unit == 0 ? objItem.Item.Unit : "Units";
            objItem.Quantity = objPItem.Unit == 0 ? objItem.Quantity / objItem.Item.ConversionUnit : objItem.Quantity;
            objItem.UnitCost = objPItem.Unit == 0 ? objItem.UnitCost * objItem.Item.ConversionUnit : objItem.UnitCost;

            //object[] objPurchaseItem = { objItem.PurchaseOrderItemId, objItem.Item.ItemId, objItem.Item.ItemName, objItem.UnitString, objItem.Item.ConversionUnit, 0, objItem.Quantity, objItem.UnitCost, objItem.TotalAmount };
            //grdItems.Rows.Add(objPurchaseItem);
            ///PurchaseOrderItemsList.Add(objPItem);
        }
        private bool ItemAlreadyExists(long ItemId)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == ItemId)
                {
                    MessageBox.Show("Selected Item is Already Added, Please Laod that one", "Item Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    grdItems.Rows[r.Index].Selected = true;
                    return true;
                }
            }
            return false;
        }
        private void FillObject(PurchaseOrderItem objPItem)
        {
            objPItem.PurchaseOrderItemId = Convert.ToInt64(txtPurchaseOrderItemId.Text);
            objPItem.PurchaseOrderNo = decimal.Parse(txtPurchaseOrderNo.Text);
            objPItem.Item = new Item();
            objPItem.ItemId = this.SelectedItemId;// Convert.ToInt32(cmbItems.SelectedValue);
            //objPItem.ItemName = this.SelectedItemName; //cmbItems.GetItemText(cmbItems.SelectedItem);\
            //objPItem.UnitString = cmbUnit.GetItemText(cmbUnit.SelectedItem);
            //objPItem.ConversionUnit = (int)numConversionUnit.Value;
            objPItem.Quantity = (int)numQuantity.Value;
            objPItem.UnitCost = (double)numUnitCost.Value;
            objPItem.TotalAmount = double.Parse(txtTotalAmount.Text);
            //objPItem.SupplierId = Convert.ToInt64(cmbSuppliers.SelectedValue);
        }
        private void FillObject(PurchaseOrderItem objPItem, PurchaseOrderItem objLoadedItem, decimal PurchaseOrderNo)
        {
            objPItem.PurchaseOrderItemId = objLoadedItem.PurchaseOrderItemId;
            objPItem.PurchaseOrderNo = PurchaseOrderNo;
            objPItem.Item = new Item();
            objPItem.ItemId = objLoadedItem.Item.ItemId;
            //objPItem.ItemName = objLoadedItem.Item.ItemName;
            objPItem.Quantity = objLoadedItem.Quantity;
            objPItem.UnitCost = objLoadedItem.UnitCost;
            objPItem.TotalAmount = objLoadedItem.TotalAmount;
        }
        private bool IsValidInput()
        {
            bool ErrFound = false;
            //if (cmbSuppliers.SelectedIndex < 0)
            //{
            //    ErrSupplier.Visible = true;
            //    ErrFound = true;
            //}
            //else
            //{
            //    ErrSupplier.Visible = false;
            //}
            if (this.SelectedItemId <= 0)//(cmbItems.SelectedIndex < 0)
            {
                ErrItem.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrItem.Visible = false;
            }
            if (this.cmbUnit.SelectedIndex < 0)//(cmbItems.SelectedIndex < 0)
            {
                errUnit.Visible = true;
                ErrFound = true;
            }
            else
            {
                errUnit.Visible = false;
            }
            if (numQuantity.Value <= 0)
            {
                ErrQuantity.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrQuantity.Visible = false;
            }

            if (numConversionUnit.Value <= 0)
            {
                errConvUnit.Visible = true;
                ErrFound = true;
            }
            else
            {
                errConvUnit.Visible = false;
            }
            //if (numUnitCost.Value <= 0)
            //{
            //    ErrUnitCost.Visible = true;
            //    ErrFound = true;
            //}
            //else
            //{
            //    ErrUnitCost.Visible = false;
            //}
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
        private void numUnitCost_ValueChanged(object sender, EventArgs e)
        {
            calculateTotal();
        }
        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {
            calculateTotal();
        }
        private void btnClearForm_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.ActionTime = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
            if (grdItems.Rows.Count <= 0)
            {
                MessageBox.Show("Please Add Some Purchase Items", "Empty Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (IsOldOrderLoaded)
            {
                UpdatePurchaseOrder(this.PurchaseOrderId);
            }
            else
            {
                SaveNewPurchaseOrder();
            }
        }
        private void SaveNewPurchaseOrder()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    PurchaseOrder objPOrder = new PurchaseOrder();
                    objPOrder.PurchaseOrderNo = decimal.Parse(txtPurchaseOrderNo.Text);
                    objPOrder.OrderDate = dtpPurchaseOrderDate.Value;
                    objPOrder.Supplier = unitOfWork.SupplierRepository.GetById(Convert.ToInt64(cmbSuppliers.SelectedValue));
                    objPOrder.PurchaseOrderItems = new List<PurchaseOrderItem>();
                    objPOrder.IsActive = true;
                    objPOrder.IsNew = true;
                    objPOrder.CreatedAt = this.ActionTime;
                    objPOrder.UpdatedAt = ActionTime;
                    objPOrder.TotalAmount = decimal.Parse(txtGrandTotal.Text);
                    objPOrder.UserId = SharedVariables.LoggedInUser.UserId;

                    double qty = 0;
                    foreach (DataGridViewRow r in grdItems.Rows)
                    {
                        qty = 0;
                        PurchaseOrderItem objPItem = new PurchaseOrderItem();
                        Item objItem = unitOfWork.ItemRspository.GetById(Convert.ToInt32(r.Cells["colItemId"].Value));
                        objPItem.Item = objItem;
                        double.TryParse(r.Cells["colQuantity"].Value.ToString(), out qty);
                        objPItem.Quantity = qty;
                        objPItem.Unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
                        //objPItem.ConversionUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
                        //objPItem.Quantity = objPItem.Unit == 0 ? objPItem.Quantity * objPItem.ConversionUnit : objPItem.Quantity; // to save quantity always in broken form.
                        objPItem.UnitCost = Convert.ToDouble(r.Cells["colUnitCost"].Value);
                        //objPItem.UnitCost = objPItem.Unit == 0 ? objPItem.UnitCost / objPItem.ConversionUnit : objPItem.UnitCost; // to save quantity always in broken form.
                        objPItem.TotalAmount = Convert.ToDouble(r.Cells["colTotalAmount"].Value);
                        objPItem.PurchaseOrderNo = (decimal)objPOrder.PurchaseOrderNo;
                        objPItem.IsActive = true;
                        objPItem.IsNew = true;
                        objPItem.CreatedAt = DateTime.Now;
                        objPItem.UpdatedAt = DateTime.Now;
                        objPItem.UserId = SharedVariables.LoggedInUser.UserId;
                        objPOrder.PurchaseOrderItems.Add(objPItem);
                    }
                    //cxt.PurchaseOrders.Add(objPOrder);
                    //cxt.SaveChanges();
                    unitOfWork.PurchaseOrderRepository.Insert(objPOrder);
                    unitOfWork.Save();
                    MessageBox.Show("Purchase Order Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void UpdatePurchaseOrder(long PurchaseOrderId)
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    PurchaseOrder objPOrder = unitOfWork.PurchaseOrderRepository.GetPurchaseOrderById(PurchaseOrderId);
                    objPOrder.OrderDate = dtpPurchaseOrderDate.Value;
                    objPOrder.Supplier = unitOfWork.SupplierRepository.GetById(Convert.ToInt64(cmbSuppliers.SelectedValue));
                    objPOrder.TotalAmount = decimal.Parse(txtGrandTotal.Text);
                    if (objPOrder.IsSynced)
                    {
                        objPOrder.IsNew = false;
                        objPOrder.IsUpdate = true;
                        objPOrder.IsSynced = false;
                    }
                    objPOrder.UpdatedAt = DateTime.Now;
                    objPOrder.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    long PurchaseOrderItemId = -1;
                    bool IsRowDeleted = false;
                    double qty = 0;
                    foreach (DataGridViewRow r in grdItems.Rows)
                    {
                        qty = 0;
                        PurchaseOrderItemId = Convert.ToInt64(r.Cells["colPurchaseOrderItemId"].Value);
                        PurchaseOrderItemId = PurchaseOrderItemId == 0 ? -1 : PurchaseOrderItemId; // because default value of PurchaseOrderItemId  is 0
                        IsRowDeleted = r.Visible ? false : true;
                        PurchaseOrderItem objPItem = new PurchaseOrderItem();
                        Item objItem = unitOfWork.ItemRspository.GetById(Convert.ToInt32(r.Cells["colItemId"].Value));
                        objPItem.Item = objItem;

                        double.TryParse(r.Cells["colQuantity"].Value.ToString(), out qty);
                        objPItem.Quantity = qty;

                        objPItem.Unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
                        //objPItem.ConversionUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
                        //objPItem.Quantity = objPItem.Unit == 0 ? objPItem.Quantity * objPItem.ConversionUnit : objPItem.Quantity; // to save quantity always in broken form.

                        objPItem.UnitCost = Convert.ToDouble(r.Cells["colUnitCost"].Value);
                        //objPItem.UnitCost = objPItem.Unit == 0 ? objPItem.UnitCost / objPItem.ConversionUnit : objPItem.UnitCost; // to save quantity always in broken form.
                        objPItem.TotalAmount = Convert.ToDouble(r.Cells["colTotalAmount"].Value);
                        objPItem.PurchaseOrderNo = (decimal)objPOrder.PurchaseOrderNo;
                        objPItem.IsNew = true;
                        objPItem.IsActive = true;
                        objPItem.CreatedAt = DateTime.Now;
                        objPItem.UpdatedAt = DateTime.Now;
                        if (PurchaseOrderItemId > 0)
                        {
                            foreach (PurchaseOrderItem i in objPOrder.PurchaseOrderItems)
                            {
                                if (i.PurchaseOrderItemId == PurchaseOrderItemId)
                                {
                                    i.Item = objPItem.Item;
                                    i.Quantity = objPItem.Quantity;
                                    i.UnitCost = objPItem.UnitCost;
                                    i.TotalAmount = objPItem.TotalAmount;
                                    i.IsUpdate = true;
                                    i.IsSynced = false;
                                    i.UpdatedAt = DateTime.Now;
                                    if (IsRowDeleted)
                                    {
                                        i.IsActive = false;
                                    }
                                    unitOfWork.GetDbContext().Entry(i).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                }
                            }
                        }
                        else
                        {
                            objPOrder.PurchaseOrderItems.Add(objPItem);
                        }
                    }

                    unitOfWork.PurchaseOrderRepository.Update(objPOrder);
                    unitOfWork.Save();
                }
                MessageBox.Show("Purchase Order Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            DataGridViewRowCancelEventArgs ev = new DataGridViewRowCancelEventArgs(grdItems.Rows[e.RowIndex]);
            if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
            {
                grdItems_UserDeletingRow(grdItems, ev);

                long PurchaseOrderItemId = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colPurchaseORderItemID"].Value);
                if (e.RowIndex == EditingRowIndex) { Clear(); EditingRowIndex = -1; }
                else if (e.RowIndex < EditingRowIndex) { EditingRowIndex--; }
                if (PurchaseOrderItemId > 0)
                {
                    grdItems.Rows[e.RowIndex].Visible = false;
                }
                else
                {
                    grdItems.Rows.RemoveAt(e.RowIndex);
                }
                return;
            }
            if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
            {
                Clear();
                numQuantity.ValueChanged -= numQuantity_ValueChanged;
                numUnitCost.ValueChanged -= numUnitCost_ValueChanged;
                FillItemInfo(grdItems.Rows[e.RowIndex]);
                EditingRowIndex = e.RowIndex;
                this.btnAddItem.Text = "Update Item";
                grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = SharedVariables.EditingColor;
                //cmbItems.Enabled = false;  
                uC_SearchItems1.Enabled = false;
                numQuantity.ValueChanged += numQuantity_ValueChanged;
                numUnitCost.ValueChanged += numUnitCost_ValueChanged;
                return;
            }
        }
        private void FillItemInfo(DataGridViewRow rItem)
        {
            //cmbSuppliers.SelectedValue = Convert.ToInt64(rItem.Cells["colSupplierId"].Value.ToString());
            txtPurchaseOrderItemId.Text = rItem.Cells["colPurchaseOrderItemId"].Value.ToString();
            //cmbItems.SelectedValue = Convert.ToInt32(rItem.Cells["colItemID"].Value.ToString());
            this.SelectedItemId = Convert.ToInt32(rItem.Cells["colItemID"].Value.ToString());
            using (unitOfWork = new UnitOfWork())
            {
                string unit = unitOfWork.ItemRspository.GetUnitByItemId(this.SelectedItemId);
                cmbUnit.Items.Add(unit);
                cmbUnit.Items.Add("Units");
            }
            cmbUnit.SelectedIndex = rItem.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
            numConversionUnit.Value = Convert.ToInt32(rItem.Cells["colConvUnit"].Value);
            this.uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
            this.uC_SearchItems1.SetText = this.SelectedItemName = rItem.Cells["colItemName"].Value.ToString();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;

            numQuantity.Value = Convert.ToInt32(rItem.Cells["colQuantity"].Value);
            numUnitCost.Value = Convert.ToDecimal(rItem.Cells["colUnitCost"].Value);
            txtTotalAmount.Text = rItem.Cells["colTotalAmount"].Value.ToString();
        }

        //private void grdItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        //{
        //    decimal RowTotal = Convert.ToDecimal(grdItems.Rows[e.RowIndex].Cells["colTotalAmount"].Value);
        //    decimal Grandtotal = 0;
        //    decimal.TryParse(txtGrandTotal.Text, out Grandtotal);
        //    txtGrandTotal.Text = (Grandtotal + RowTotal).ToString();
        //}

        private void grdItems_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            decimal RowTotal = Convert.ToDecimal(e.Row.Cells["colTotalAmount"].Value);
            decimal Grandtotal = 0;
            decimal.TryParse(txtGrandTotal.Text, out Grandtotal);
            txtGrandTotal.Text = (Grandtotal - RowTotal).ToString();
        }

        private void calculateTotal()
        {
            double TotalCost = 0;
            int Quantity = (int)numQuantity.Value;
            double UnitCost = (double)numUnitCost.Value;
            TotalCost = Quantity * UnitCost;
            double GrandTotals = GetGrandTotals();
            txtTotalAmount.Text = TotalCost.ToString();
            txtGrandTotal.Text = (TotalCost + GrandTotals).ToString();
        }

        private double GetGrandTotals()
        {
            double TotalAmount = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (r.Index != EditingRowIndex)
                {
                    TotalAmount += Convert.ToDouble(r.Cells["coltotalAmount"].Value);
                }
            }
            return TotalAmount;
        }

        private void cmbItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void numQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void numUnitCost_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmPurchaseOrderShortCuts f = new ShortCutDialogs.frmPurchaseOrderShortCuts();
            f.ShowDialog();
        }

        private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.SelectedItemId > 0)
                {
                    double avQty = 0;
                    using (unitOfWork = new UnitOfWork())
                    {
                        avQty = unitOfWork.ItemRspository.GetItemAvailableQty(this.SelectedItemId);
                    }
                    if (cmbUnit.SelectedIndex == 0)
                    {
                        numAvailableQty.Value = (decimal)Math.Floor((double)avQty / LoadedItemData.ConversionUnit);
                    }
                    else
                    {
                        numAvailableQty.Value = (decimal)avQty;
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void btnLoadLSI_Click(object sender, EventArgs e)
        {
            try
            {
                List<ItemsVM> lst = new List<ItemsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    lst = unitOfWork.ItemRspository.LoadLowStockItems();
                }

                this.IsLoadingLowStocks = true;
                grdItems.Rows.Clear();
                foreach (ItemsVM i in lst)
                {
                    if (i.AvailableStock < i.ReorderingLevel)
                    {
                        grdItems.Rows.Add(0, i.ItemId, i.ItemName, i.Unit, i.ConversionUnit, i.AvailableStock, 0, i.UnitCost, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                this.IsLoadingLowStocks = false;
                MessageBox.Show("An error occurred while loading low stock items.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                this.IsLoadingLowStocks = false;
            }
        }

        private void grdItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                DataGridViewComboBoxCell dgCmbUnit;
                DataGridViewRow row = grdItems.Rows[e.RowIndex];
                if (this.IsLoadingLowStocks)
                {
                    dgCmbUnit = (DataGridViewComboBoxCell)grdItems.Rows[e.RowIndex].Cells["colUnit"];
                    string unit = grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString();
                    dgCmbUnit.Items.Add(unit);
                    dgCmbUnit.Items.Add("Units");
                    row.Cells["colUnit"].Value = unit;
                }
                else if (this.IsLoadingPrvSoldItems)
                {
                    dgCmbUnit = (DataGridViewComboBoxCell)grdItems.Rows[e.RowIndex].Cells["colUnit"];
                    string unit = grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString();
                    dgCmbUnit.Items.Add(unit);
                    dgCmbUnit.Items.Add("Units");
                    row.Cells["colUnit"].Value = "Units";

                }
                else if (this.IsFormLoading)
                {
                    double avQty = 0;
                    using (unitOfWork = new UnitOfWork())
                    {
                        int ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                        avQty = unitOfWork.ItemRspository.GetItemAvailableQty(ItemId);
                    }
                    row.Cells["colAvQty"].Value = avQty;
                    dgCmbUnit = (DataGridViewComboBoxCell)grdItems.Rows[e.RowIndex].Cells["colUnit"];
                    string unit = grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString();
                    dgCmbUnit.Items.Add(unit);
                    dgCmbUnit.Items.Add("Units");
                    row.Cells["colUnit"].Value = unit;
                }
                else
                {
                    row = grdItems.Rows[e.RowIndex];
                    int ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                    ItemsVM itemData;
                    using (unitOfWork = new UnitOfWork())
                    {
                        itemData = unitOfWork.ItemRspository.GetItemWithAvQty(ItemId);
                    }
                    dgCmbUnit = (DataGridViewComboBoxCell)grdItems.Rows[e.RowIndex].Cells["colUnit"];

                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    row.Cells["colItemName"].Value = itemData.ItemName;
                    row.Cells["colConvUnit"].Value = itemData.ConversionUnit;
                    row.Cells["colAvQty"].Value = (int)Math.Floor((double)itemData.AvailableStock / itemData.ConversionUnit);
                    row.Cells["colQuantity"].Value = 0;
                    row.Cells["colUnitCost"].Value = itemData.UnitCost * itemData.ConversionUnit;
                    row.Cells["colTotalAmount"].Value = 0;

                    string unit = itemData.Unit;
                    dgCmbUnit.Items.Add(unit);
                    dgCmbUnit.Items.Add("Units");
                    row.Cells["colUnit"].Value = dgCmbUnit.Items[0];
                    grdItems.CellValueChanged += grdItems_CellValueChanged;
                }
                PopulateTotals();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void PopulateTotals()
        {
            double tCost = 0;
            double TotalCost = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                double.TryParse(r.Cells["colTotalAmount"].Value.ToString(), out tCost);
                TotalCost += tCost;
            }
            txtGrandTotal.Text = TotalCost.ToString();
        }

        private void grdItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0) return;
            int qty = 0;
            double uCost = 0;
            try
            {
                DataGridViewRow row = grdItems.Rows[e.RowIndex];
                grdItems.CellValueChanged -= grdItems_CellValueChanged;
                if (e.ColumnIndex == grdItems.Columns["colQuantity"].Index || e.ColumnIndex == grdItems.Columns["colUnitCost"].Index)
                {
                    int.TryParse(row.Cells["colQuantity"].Value.ToString(), out qty);
                    double.TryParse(row.Cells["colUnitCost"].Value.ToString(), out uCost);
                    row.Cells["colTotalAmount"].Value = qty * uCost;

                }
                else if (e.ColumnIndex == grdItems.Columns["colUnit"].Index)
                {

                    ItemsVM itemData = new ItemsVM();
                    int Qty = Convert.ToInt32(row.Cells["colQuantity"].Value);
                    int ItemId = Convert.ToInt32(row.Cells["colItemId"].Value);
                    using (unitOfWork = new UnitOfWork())
                    {
                        itemData = unitOfWork.ItemRspository.GetItemWithAvQty(ItemId);
                    }
                    string unit = row.Cells["colUnit"].Value.ToString();

                    grdItems.CellValueChanged -= grdItems_CellValueChanged;
                    if (!unit.ToLower().Equals("units"))
                    {
                        itemData.AvailableStock = (int)Math.Floor((double)itemData.AvailableStock / itemData.ConversionUnit);
                        itemData.UnitCost = itemData.UnitCost * itemData.ConversionUnit;
                    }
                    row.Cells["colAvQty"].Value = itemData.AvailableStock;
                    row.Cells["colUnitCost"].Value = itemData.UnitCost;
                    row.Cells["colTotalAmount"].Value = itemData.UnitCost;
                }
                PopulateTotals();
                grdItems.CellValueChanged += grdItems_CellValueChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Calculcation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void grdItems_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colUnit"].Index)
            {
                grdItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void grdItems_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }


        // load low stock items for selected supplier
        private void btnSupLSI_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbSuppliers.SelectedIndex <= 0)
                {
                    MessageBox.Show("Please select supplier before proceeding.", "Select Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                List<ItemsVM> lst = new List<ItemsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    lst = unitOfWork.ItemRspository.LoadSup_LowStockItems(Convert.ToInt32(cmbSuppliers.SelectedValue));
                }
                this.IsLoadingLowStocks = true; // this variable is being used in Rows_added event.
                grdItems.Rows.Clear();
                foreach (ItemsVM i in lst)
                {
                    if (i.AvailableStock < i.ReorderingLevel)
                    {
                        grdItems.Rows.Add(0, i.ItemId, i.ItemName, i.Unit, i.ConversionUnit, i.AvailableStock, 0, i.UnitCost, 0);
                    }
                }
                this.IsLoadingLowStocks = false;
            }
            catch (Exception)
            {
                this.IsLoadingLowStocks = false;
                MessageBox.Show("An error occurred while loading low stock items.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        // load all items for selected supplier
        private void btnSupAI_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbSuppliers.SelectedIndex <= 0)
                {
                    MessageBox.Show("Please select supplier before proceeding.", "Select Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                List<ItemsVM> lst = new List<ItemsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    lst = unitOfWork.ItemRspository.LoadSupItems(Convert.ToInt32(cmbSuppliers.SelectedValue));
                }
                grdItems.Rows.Clear();
                this.IsLoadingLowStocks = true; // this variable is being used in Rows_added event.
                foreach (ItemsVM i in lst)
                {
                    grdItems.Rows.Add(0, i.ItemId, i.ItemName, i.Unit, i.ConversionUnit, i.AvailableStock, 0, i.UnitCost, 0);
                }
                this.IsLoadingLowStocks = false;

            }
            catch (Exception)
            {
                this.IsLoadingLowStocks = false;
                MessageBox.Show("An error occurred while loading suppier items.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLoadPrvSoldItems_Click(object sender, EventArgs e)
        {
            try
            {
                this.IsLoadingPrvSoldItems = true; // this is needed to makes "Units" slected by default on this action
                DlgAddStockFilter f = new DlgAddStockFilter();
                f.StartPosition = FormStartPosition.Manual;
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
                List<StockAddFilterVM> lst = f.SelectedItems;
                foreach (var i in lst)
                {
                    grdItems.Rows.Add(0, i.ItemId, i.ItemName, i.Unit, i.ConversionUnit, i.AvailableStock, i.Quantity, i.UnitCost, 0);

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

        private void grdItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index)
            {
                e.Control.KeyPress += new KeyPressEventHandler(SharedFunctions.isValidNumericKey);
            }
        }
    }
}