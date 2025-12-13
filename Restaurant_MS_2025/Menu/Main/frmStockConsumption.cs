

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmStockConsumption : Form
    {
        UnitOfWork unitOfWork;
        //List<StockConsumption> StockConsumptionList = new List<StockConsumption>();
        List<BatchStockVM> BatchStockList = new List<BatchStockVM>();
        DataTable dtStockConsumtption = new DataTable();
        int EditingRowIndex = -1;
        long StockConumptionId = 0;
        //List<StockConsumptionItem> DeletedConsumptions = new List<StockConsumptionItem>();
        List<int> DeletedConsumptions = new List<int>();
        BackgroundWorker bgWItemsLoader = new BackgroundWorker();
        List<Item> Items = new List<Item>();
        public delegate void GetSelectedItem(int ItemId);
        int SelectedItemId = 0;
        string SelectedItemName = "";
        public DateTime? ActionTime = null;
        public frmStockConsumption()
        {
            InitializeComponent();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }
        public frmStockConsumption(long StockConumptionId)
        {
            InitializeComponent();
            this.StockConumptionId = StockConumptionId;
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }
        private void uC_SearchItems1_OnItemSelectionChanged()
        {
            this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;
            this.SelectedItemName = this.uC_SearchItems1.SelectedItemName;
            cmbSelectBatch.SelectedIndex = -1;
            if (this.SelectedItemId > 0) // (cmbSelectItems.SelectedIndex >= 0)
            {
                LoadItemData(this.SelectedItemId);
                GetItemBatches(this.SelectedItemId);
            }
            //MessageBox.Show(this.SelectedItemId + " | " + this.SelectedItemName);
        }

        private void LoadItemData(int ItemId)
        {
            using (unitOfWork = new UnitOfWork())
            {
                Item item = unitOfWork.ItemRspository.GetById(ItemId);
                cmbUnit.Items.Add(item.Unit);
                cmbUnit.Items.Add("Units");
                cmbUnit.SelectedIndex = 0;
                numConvUnit.Value = item.ConversionUnit;
            }
        }
        private void LoadStockConsumption()
        {
            StockConsumptionVM sc;
            using (unitOfWork = new UnitOfWork())
            {
                sc = unitOfWork.StockConsumptionRepository.GetStockConsumptionById(this.StockConumptionId);
            }

            foreach (StockConsumptionItemVM v in sc.StockConsumptionsList)
            {
                if (v.IsActive)
                {
                    if (v.ConsumptionType == 1) { v.ConsumptionTypeString = "Services"; }
                    else if (v.ConsumptionType == 2) { v.ConsumptionTypeString = "Sales"; }
                    else if (v.ConsumptionType == 3) { v.ConsumptionTypeString = "Damages"; }
                    else if (v.ConsumptionType == 4) { v.ConsumptionTypeString = "Returned"; }
                    grdItems.Rows.Add(v.StockConsumptionItemId, v.Item.ItemId, v.Item.ItemName, v.Batch.BatchId, v.Batch.BatchName, v.UnitString, v.Item.ConversionUnit, v.Quantity, v.ConsumptionType, v.ConsumptionTypeString, v.Comment);
                }
            }
        }
        private void frmConsumeStock_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnSave, btnClearAll });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnAddItem, btnClear });
                SharedFunctions.GenerateStockConsumptionTable(dtStockConsumtption);

                if (SharedFunctions.CheckDayClosed())
                {
                    this.BeginInvoke(new MethodInvoker(Close));
                    return;
                }
                //cmbSelectItems.SelectedIndexChanged -= cmbSelectItems_SelectedIndexChanged;
                LoadItems();
                if (this.StockConumptionId > 0)
                {
                    LoadStockConsumption();
                    grdItems.Columns["colEdit"].Visible = false;
                }
                //cmbSelectItems.SelectedIndexChanged += cmbSelectItems_SelectedIndexChanged;
                //cmbSelectItems_SelectedIndexChanged(null, null);
                cmbConsumtionType.SelectedIndex = 0;
                SharedFunctions.SetGridStyle(grdItems);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.I))
            {
                //cmbSelectItems.Focus();
                return true;
            }
            if (keyData == (Keys.Alt | Keys.A))
            {
                btnAddItem.PerformClick();
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
                    cmbSelectBatch.Focus();
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
                    cmbSelectBatch.Focus();
                }
                return true;
            }
            #endregion grid short cuts
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void bgWItemsLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                //cmbSelectItems.Enabled = false;
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
            //cmbSelectItems.SelectedIndexChanged -= cmbSelectItems_SelectedIndexChanged;
            //cmbSelectItems.DataSource = Items;
            //cmbSelectItems.ValueMember = "ItemId";
            //cmbSelectItems.DisplayMember = "ItemName";
            //cmbSelectItems.SelectedIndexChanged += cmbSelectItems_SelectedIndexChanged;
            //cmbSelectItems.SelectedIndex = -1;
            //cmbSelectItems.Enabled = true;
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
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    StockConsumptionItem objConsumption = new StockConsumptionItem();
                    FillObject(objConsumption);
                    if (this.EditingRowIndex >= 0)
                    {
                        if (!isValidQuantityEntered())
                        {
                            MessageBox.Show("Consumed Quantity Can't be Greater Than Available Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            numQuantity.Focus();
                            numQuantity.Select(0, numQuantity.Value.ToString().Length);
                            return;
                        }
                        UpdateRow(objConsumption);
                    }
                    else
                    {
                        int ItemId = this.SelectedItemId; //Convert.ToInt32(cmbSelectItems.SelectedValue);
                        int ConsumtionType = cmbConsumtionType.SelectedIndex;

                        if (ItemAlreadyAdded(ItemId))
                        {
                            MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        if (!isValidQuantityEntered())
                        {
                            MessageBox.Show("Consumed Quantity Can't be Greater Than Available Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                            numQuantity.Focus();
                            numQuantity.Select(0, numQuantity.Value.ToString().Length);
                            return;
                        }
                        AddItemtoGridAndList(objConsumption);
                    }
                    Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while adding item to grid, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                this.uC_SearchItems1.SetFocus();
            }
        }
        private void UpdateRow(StockConsumptionItem objConsumption)
        {
            DataGridViewRow r = grdItems.Rows[EditingRowIndex];
            r.Cells["colBatchId"].Value = objConsumption.BatchId;
            //r.Cells["colBatch"].Value = objConsumption.BatchName;
            //r.Cells["colUnit"].Value = objConsumption.UnitString;
            //r.Cells["colConvUnit"].Value = objConsumption.ConversionUnit;
            r.Cells["colQuantity"].Value = objConsumption.Quantity;
            r.Cells["colConsumptiontypeId"].Value = objConsumption.ConsumptionType;
            //r.Cells["colConsumptionTypeString"].Value = objConsumption.ConsumptionTypeString;
            r.Cells["colcomment"].Value = objConsumption.Comment;
            grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
        }
        private void AddItemtoGridAndList(StockConsumptionItem objConsumption)
        {
            //dtStockConsumtption.Rows.Add(objConsumption.Quantity, objConsumption.ConsumptionType, objConsumption.Comment, objConsumption.BatchId == 0 ? null : objConsumption.BatchId,objConsumption.ItemId, null, objConsumption.Date);            
            grdItems.Rows.Add(objConsumption.StockConsumptionItemId, objConsumption.ItemId, objConsumption.Item.ItemName, objConsumption.BatchId, objConsumption.Batch.BatchName, objConsumption.UnitString, objConsumption.ConversionUnit, objConsumption.Quantity, objConsumption.ConsumptionType, objConsumption.ConsumptionTypeString, objConsumption.Comment);
        }
        private void FillObject(StockConsumptionItem objConsumption)
        {
            objConsumption.StockConsumptionItemId = 0;
            objConsumption.ItemId = this.SelectedItemId; // Convert.ToInt32(cmbSelectItems.SelectedValue);
            //objConsumption.ItemName = this.SelectedItemName;// cmbSelectItems.GetItemText(cmbSelectItems.SelectedItem);
            objConsumption.BatchId = Convert.ToInt32(cmbSelectBatch.SelectedValue);
            //objConsumption.BatchName = cmbSelectBatch.GetItemText(cmbSelectBatch.SelectedItem);
            //objConsumption.UnitString = cmbUnit.GetItemText(cmbUnit.SelectedItem);
            //objConsumption.ConversionUnit = (int)numConvUnit.Value;
            objConsumption.Quantity = (double)numQuantity.Value;
            SharedVariables.ConsumptionType enumConType;
            Enum.TryParse(cmbConsumtionType.GetItemText(cmbConsumtionType.SelectedItem), out enumConType);
            objConsumption.ConsumptionType = (int)enumConType;
            //objConsumption.ConsumptionTypeString = enumConType.ToString();
            objConsumption.Comment = txtComment.Text.ToString();
            objConsumption.CreatedAt = DateTime.Now;
            objConsumption.IsActive = true;
            objConsumption.CreatedAt = DateTime.Now;
            objConsumption.UpdatedAt = DateTime.Now;
            objConsumption.IsNew = true;
        }
        private void FillObject(DataGridViewRow r, StockConsumptionItem objConsumption)
        {
            objConsumption.ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            objConsumption.BatchId = Convert.ToInt32(r.Cells["colBatchId"].Value);
            objConsumption.Unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
            objConsumption.Quantity = Convert.ToDouble(r.Cells["colQuantity"].Value);
            //objConsumption.ConversionUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
            //objConsumption.Quantity = objConsumption.Unit == 0 ? objConsumption.Quantity * objConsumption.ConversionUnit : objConsumption.Quantity;
            objConsumption.ConsumptionType = Convert.ToInt32(r.Cells["colConsumptionTypeId"].Value);
            objConsumption.Comment = r.Cells["colComment"].Value.ToString();
            objConsumption.CreatedAt = this.ActionTime.Value;
            objConsumption.UpdatedAt = this.ActionTime.Value;
            objConsumption.IsNew = true;
            objConsumption.IsActive = true;
        }
        private bool IsValidInput()
        {
            ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            bool ErrFound = false;
            if (this.SelectedItemId <= 0) //(Convert.ToInt32(cmbSelectItems.SelectedValue) <= 0)
            {
                ErrSelectItem.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrSelectItem.Visible = false;
            }
            if (Convert.ToInt32(cmbSelectBatch.SelectedValue) <= 0)
            {
                ErrSelectBatch.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrSelectBatch.Visible = false;
            }
            if (Convert.ToDouble(numQuantity.Value) <= 0)
            {
                ErrQuantity.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrQuantity.Visible = false;
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

        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            long StockConItemId = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colStockConsumptionItemId"].Value);
            if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
            {
                Clear();
                FillForm(grdItems.Rows[e.RowIndex]);
                btnAddItem.Text = "Update Item";
                uC_SearchItems1.Enabled = false;
                if (EditingRowIndex >= 0)
                {
                    grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = SharedVariables.EditingColor;
                EditingRowIndex = e.RowIndex;
            }
            if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
            {
                if (StockConItemId > 0)
                {
                    StockConsumptionItem i = new StockConsumptionItem();
                    int conItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colStockConsumptionItemId"].Value);
                    //i.ConsumptionType = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colConsumptionTypeId"].Value);
                    //i.ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                    //i.BatchId = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value);
                    //i.Quantity = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value);
                    DeletedConsumptions.Add(conItemId);
                }
                if (e.RowIndex == EditingRowIndex) { Clear(); EditingRowIndex = -1; }
                else if (e.RowIndex < EditingRowIndex) { EditingRowIndex--; }
                //dtStockConsumtption.Rows.RemoveAt(e.RowIndex);
                //StockConsumptionList.RemoveAt(e.RowIndex);
                grdItems.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void FillForm(DataGridViewRow r)
        {
            //cmbSelectItems.SelectedValue = Convert.ToInt32(r.Cells["colItemId"].Value);
            this.SelectedItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            this.SelectedItemName = r.Cells["colItemName"].Value.ToString();
            uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
            uC_SearchItems1.SetText = this.SelectedItemName;
            uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            GetItemBatches(this.SelectedItemId);
            cmbSelectBatch.SelectedValue = Convert.ToInt32(r.Cells["colBatchId"].Value);
            LoadItemData(this.SelectedItemId);
            cmbUnit.SelectedIndex = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
            //cmbConsumtionType.SelectedIndex = cmbConsumtionType.FindStringExact(r.Cells["colConsumptionType"].Value.ToString());
            cmbConsumtionType.SelectedItem = r.Cells["colConsumptionTypeString"].Value.ToString();
            numQuantity.Value = Convert.ToDecimal(r.Cells["colQuantity"].Value);
            txtComment.Text = r.Cells["colComment"].Value == null ? "" : r.Cells["colComment"].Value.ToString();
        }

        private bool ItemAlreadyAdded(int ItemId, long BatchId, int ConsumptionType)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == ItemId
                    && Convert.ToInt64(r.Cells["colBatchId"].Value) == BatchId
                    && Convert.ToInt32(r.Cells["colConsumptionTypeId"].Value) == ConsumptionType)
                {
                    return true;
                }
            }
            return false;
        }

        private bool ItemAlreadyAdded(int ItemId)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                SharedVariables.ConsumptionType enumConType;
                Enum.TryParse(cmbConsumtionType.GetItemText(cmbConsumtionType.SelectedItem), out enumConType);
                int ConsumptionType = (int)enumConType;

                if (Convert.ToInt32(r.Cells["colItemId"].Value) == ItemId
                    && Convert.ToInt64(r.Cells["colBatchId"].Value) == Convert.ToInt64(cmbSelectBatch.SelectedValue)
                    && Convert.ToInt32(r.Cells["colConsumptionTypeId"].Value) == ConsumptionType)
                {
                    return true;
                }
            }
            return false;
        }

        private void cmbSelectItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSelectBatch.SelectedIndex = -1;
            //if (cmbSelectItems.SelectedIndex >= 0)
            //{
            //    GetItemBatches(Convert.ToInt32(cmbSelectItems.SelectedValue));
            //}
        }
        private void GetItemBatches(int ItemId)
        {
            cmbSelectBatch.SelectedIndexChanged -= cmbSelectBatch_SelectedIndexChanged;
            using (unitOfWork = new UnitOfWork())
            {
                BatchStockList = unitOfWork.ItemRspository.GetBatchStockByItemId(ItemId);
            }
            List<BatchStockVM> FinalResult = new List<BatchStockVM>();
            foreach (BatchStockVM i in BatchStockList)
            {
                i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock);
                i.BatchName = i.BatchName + " | Available Stock : " + i.AvailableStock;
                if (i.AvailableStock > 0)
                {
                    FinalResult.Add(i);
                }
            }
            cmbSelectBatch.DataSource = FinalResult;
            cmbSelectBatch.ValueMember = "BatchId";
            cmbSelectBatch.DisplayMember = "BatchName";
            cmbSelectBatch.SelectedIndexChanged += cmbSelectBatch_SelectedIndexChanged;
            cmbSelectBatch.SelectedIndex = -1;
            if (this.cmbSelectBatch.Items.Count > 0)
            {
                cmbSelectBatch.SelectedIndex = 0;
            }
        }

        //private bool isValidQuantityEntered()
        //{
        //    int AvailableStock = BatchStockList.Where(x => x.BatchId == Convert.ToInt32(cmbSelectBatch.SelectedValue)).FirstOrDefault().AvailableStock;
        //    if (numQuantity.Value > AvailableStock)
        //    {
        //        ToolTip tt = new ToolTip();
        //        numQuantity.Value = 1;
        //        tt.Show("Please Enter Valid Quantity", numQuantity, 800);
        //        numQuantity.Select(0,1);
        //        return false;
        //    }
        //    return true;
        //}
        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {
            isValidQuantityEntered();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            numQuantity.ValueChanged -= numQuantity_ValueChanged;
            //cmbSelectItems.Enabled = true;
            this.btnAddItem.Text = "Add Item";
            this.SelectedItemId = 0; //cmbSelectItems.SelectedIndex = -1;
            this.SelectedItemName = "";
            uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
            uC_SearchItems1.SetText = "";
            uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            cmbSelectBatch.SelectedIndex = -1;
            cmbUnit.Items.Clear();
            cmbUnit.SelectedIndex = -1;
            txtAvlQty.Text = "";
            numQuantity.Value = 1;
            cmbConsumtionType.SelectedIndex = 0;
            txtComment.Text = "";
            dtStockConsumtption.Rows.Clear();
            if (EditingRowIndex > -1)
            {
                grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
            }
            EditingRowIndex = -1;
            numQuantity.ValueChanged += numQuantity_ValueChanged;
            this.cmbSelectBatch.DataSource = null;
            uC_SearchItems1.Enabled = true;
            this.uC_SearchItems1.SetFocus();
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (grdItems.Rows.Count > 0)
            {
                DialogResult rs = MessageBox.Show("There are Some Unsaved Items in Grid, Are You Sure You Want To Clear", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (rs == DialogResult.OK)
                {
                    grdItems.Rows.Clear();
                    this.StockConumptionId = 0;
                    DeletedConsumptions = new List<int>();
                    Clear();
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.ActionTime = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
                if (grdItems.Rows.Count <= 0)
                {
                    MessageBox.Show("Please Add Some Items to Grid.", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                if (this.StockConumptionId > 0)
                {
                    UpdateConsumption();
                }
                else
                {
                    AddConsumption();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void AddConsumption()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    StockConsumption objMaster = new StockConsumption();
                    objMaster.StockConsumptionItems = new List<StockConsumptionItem>();
                    objMaster.CreatedAt = this.ActionTime.Value;
                    objMaster.UpdatedAt = this.ActionTime.Value;
                    objMaster.IsActive = true;
                    objMaster.IsNew = true;
                    objMaster.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    //StockConsumption.BulkInsert(dtStockConsumtption);                
                    foreach (DataGridViewRow r in grdItems.Rows)
                    {
                        StockConsumptionItem s = new StockConsumptionItem();
                        FillObject(r, s);
                        s.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        s.Item = unitOfWork.ItemRspository.GetById(s.ItemId);
                        s.Batch = unitOfWork.BatchRepository.GetById(s.BatchId);

                        //following 2 lines of code were added due to default items issue handeling.
                        s.Item.IsSyncable = true;
                        unitOfWork.GetDbContext().Entry(s.Item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                        objMaster.StockConsumptionItems.Add(s);
                    }
                    unitOfWork.StockConsumptionRepository.Insert(objMaster);
                    unitOfWork.Save();
                }
                MessageBox.Show("Stock Consumption Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void UpdateConsumption()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    User currentUser = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    StockConsumption objMaster = unitOfWork.StockConsumptionRepository.GetStockConumptionById_(this.StockConumptionId);
                    objMaster.User = currentUser;

                    if (objMaster.IsSynced)
                    {
                        objMaster.IsNew = false;
                        objMaster.IsUpdate = true;
                        objMaster.IsSynced = false;
                    }

                    objMaster.UpdatedAt = this.ActionTime.Value;
                    //StockConsumption.BulkInsert(dtStockConsumtption);                
                    long StockConItemId = 0;
                    foreach (DataGridViewRow r in grdItems.Rows)
                    {
                        StockConItemId = Convert.ToInt64(r.Cells["colStockConsumptionItemId"].Value);
                        if (StockConItemId == 0) // new item added
                        {
                            StockConsumptionItem s = new StockConsumptionItem();
                            FillObject(r, s);
                            s.User = currentUser;
                            s.Item = unitOfWork.ItemRspository.GetById(s.ItemId);
                            s.Batch = unitOfWork.BatchRepository.GetById(s.BatchId);

                            //following 2 lines of code were added due to default items issue handeling.
                            s.Item.IsSyncable = true;
                            unitOfWork.GetDbContext().Entry(s.Item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                            objMaster.StockConsumptionItems.Add(s);
                        }
                    }

                    foreach (StockConsumptionItem i in objMaster.StockConsumptionItems)
                    {
                        foreach (int deleted in DeletedConsumptions)
                        {
                            //if ( deleted.ItemId == i.Item.ItemId && deleted.ConsumptionType == i.ConsumptionType && deleted.BatchId == i.Batch.BatchId)
                            if (i.StockConsumptionItemId == deleted)
                            {
                                i.IsActive = false;
                                if (i.IsSynced)
                                {
                                    i.IsNew = false;
                                    i.IsUpdate = true;
                                    i.IsSynced = false;
                                }
                                i.UpdatedAt = this.ActionTime.Value;
                                //unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                    }
                    unitOfWork.StockConsumptionRepository.Update(objMaster);
                    unitOfWork.Save();
                }
                MessageBox.Show("Stock Consumption Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void cmbSelectBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbSelectBatch.SelectedIndex >= 0)
                {
                    BatchStockVM r = BatchStockList.Where(b => b.BatchId == Convert.ToInt64(cmbSelectBatch.SelectedValue)).FirstOrDefault();
                    if (r != null)
                    {
                        double avlQty = (r.TotalStock + r.AdjustedStock - r.ConsumedStock - r.ExpiredStock);
                        int convUnit = (int)numConvUnit.Value;
                        if (cmbUnit.SelectedIndex == 0)
                        {
                            //avlQty = Convert.ToInt32(Math.Floor((double)(avlQty / convUnit)));
                            avlQty = Math.Round(avlQty / convUnit, 3);
                        }
                        txtAvlQty.Text = avlQty.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private bool isValidQuantityEntered()
        {
            double AvailableStock = BatchStockList.Where(x => x.BatchId == Convert.ToInt32(cmbSelectBatch.SelectedValue)).FirstOrDefault().AvailableStock;
            if (numQuantity.Value > (decimal)AvailableStock)
            {
                //ToolTip tt = new ToolTip();
                //numQuantity.Value = 1;
                //ErrMessage.Text = "Alert : Consumed Quantity Can't be Greater Than Available Quantity.";
                //ErrMessage.Visible = true;
                ////tt.Show("Please Enter Valid Quantity", numQuantity, 800);
                //numQuantity.Select(0, 1);
                return false;
            }
            else
            {
                ErrMessage.Visible = false;
            }
            return true;
        }

        private void cmbSelectItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSelectBatch_SelectedIndexChanged(null, null);
        }
    }
}