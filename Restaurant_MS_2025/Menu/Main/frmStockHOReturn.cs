

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmStockHOReturn : Form
    {
        UnitOfWork unitOfWork;
        private long AdjustmentId = 0;
        List<BatchStockVM> BatchStockList;
        int EditingRowIndex = -1;
        BackgroundWorker bgWItemsLoader = new BackgroundWorker();
        List<Item> Items = new List<Item>();
        int SelectedItemId = 0;
        string SelectedItemName = "";
        public DateTime? ActionTime = null;
        public frmStockHOReturn()
        {
            InitializeComponent();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }
        public frmStockHOReturn(long AdjustmentId)
        {
            InitializeComponent();
            this.AdjustmentId = AdjustmentId;
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }

        private void uC_SearchItems1_OnItemSelectionChanged()
        {
            this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;
            this.SelectedItemName = this.uC_SearchItems1.SelectedItemName;
            if (this.SelectedItemId > 0)
            {
                this.btnAddItem.PerformClick();
                //LoadItemData(this.SelectedItemId);
                //GetItemBacthes(this.SelectedItemId);
            }
            //MessageBox.Show(this.SelectedItemId + " | " + this.SelectedItemName);
        }
        private void LoadItemData(int ItemId)
        {
            using (unitOfWork = new UnitOfWork())
            {
                Item item = unitOfWork.ItemRspository.GetById(ItemId);
            }
        }
        private void frmAddStock_Load(object sender, EventArgs e)
        {
            try
            {
                if (SharedFunctions.CheckDayClosed())
                {
                    this.BeginInvoke(new MethodInvoker(Close));
                    return;
                }

                grdItems.CellValueChanged += grdItems_CellValueChanged;
                if (this.AdjustmentId > 0)
                {
                    LoadAdjustment();
                    //grdItems.Columns["colEdit"].Visible = false;
                }

                SharedFunctions.SetGridStyle(grdItems);
                SharedFunctions.SetSmallButtonsStyle(new[] { btnAddItem, btnClear });
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnSave, btnClearAll });
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
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
        private void LoadAdjustment()
        {
            Adjustment a = new Adjustment();
            using (unitOfWork = new UnitOfWork())
            {
                a = unitOfWork.AdjustmentRepository.GetAdjustmentById(this.AdjustmentId);
            }
            int count = 0;
            foreach (AdjustmentItem i in a.AdjustmentItems)
            {
                if (i.IsActive)
                {
                    //i.UnitString = i.Unit == 0 ? i.Item.Unit : "Units";
                    i.Quantity = i.Unit == 0 ? i.Quantity / i.Item.ConversionUnit : i.Quantity;
                    string at = ((Enums.ItemAdjustmentType)i.AdjustmentType).ToString();
                    grdItems.Rows.Add(i.AdjustmentItemId, i.Item.ItemId, i.Item.ItemName, i.Batch.BatchId, i.Batch.BatchName, "i.UnitString", i.Item.ConversionUnit, i.Quantity * -1, i.RetailPrice, i.TotalRetailPrice, at, i.Reason);
                    grdItems.Rows[count].DefaultCellStyle.BackColor = Color.LightGray;
                    count++;
                }
            }
        }
        private void bgWItemsLoader_DoWork(object sender, DoWorkEventArgs e)
        {


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
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            long AdjItemId = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colAdjustmentItemId"].Value);
            if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
            {

                if (AdjItemId > 0)
                {
                    DialogResult rs = MessageBox.Show("Are you Sure You Want to Delete This Adjustment?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs == DialogResult.OK)
                    {
                        if (e.RowIndex == EditingRowIndex) { Clear(); EditingRowIndex = -1; }
                        else if (e.RowIndex < EditingRowIndex) { EditingRowIndex--; }
                        grdItems.Rows[e.RowIndex].Visible = false;
                    }
                }
                else
                {
                    if (e.RowIndex == EditingRowIndex) { Clear(); EditingRowIndex = -1; }
                    else if (e.RowIndex < EditingRowIndex) { EditingRowIndex--; }
                    grdItems.Rows.RemoveAt(e.RowIndex);
                }
                //StockItemsList.RemoveAt(e.RowIndex);
            }
            if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
            {

                //if (AdjItemId > 0)
                //{
                //    MessageBox.Show("You Can't Edit This Item", "Inavalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
                Clear();
                FillForm(grdItems.Rows[e.RowIndex]);
                btnAddItem.Text = "Update Item";
                if (EditingRowIndex >= 0)
                {
                    grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
                }
                grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = SharedVariables.EditingColor;
                EditingRowIndex = e.RowIndex;

                //Edit(grdItems.Rows[e.RowIndex]);
                //grdItems.Rows.RemoveAt(e.RowIndex);
                // StockItemsList.RemoveAt(e.RowIndex);
            }
        }

        private void GetItemBacthes(int ItemId)
        {
            if (ItemId > 0)
            {
                GetItemBatches(ItemId);
            }

        }
        private void FillForm(DataGridViewRow r)
        {
            //cmbItems.SelectedValue = Convert.ToInt32(r.Cells["colItemId"].Value);
            this.SelectedItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            this.SelectedItemName = r.Cells["colItem"].Value.ToString();
            uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
            uC_SearchItems1.SetText = this.SelectedItemName;
            uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            LoadItemData(this.SelectedItemId);
            GetItemBacthes(this.SelectedItemId);
            //cmbConsumtionType.SelectedIndex = cmbConsumtionType.FindStringExact(r.Cells["colConsumptionType"].Value.ToString());
            var val = (int)(Enums.ItemAdjustmentType)r.Cells["colAdjustmentType"].Value;
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
            }
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsItemAlreadyAdded())
                {
                    return;
                }

                if (!isValidQuantityEntered())
                {
                    MessageBox.Show("Adjustment Can't be Less Than Available Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }
                grdItems.Rows.Add(
                    0 // adjustmentitemid
                    , this.SelectedItemId
                    , "" // itemname
                    , 0 // batchid
                    , "" // batchname
                    , "" //UnitName
                    , 0 // conversion unit
                    , 0.1 // quantity
                    , 0 // retail price
                    , 0 // total
                    , "Exceed" // adjustmentType string
                    , "Exceeded" // reason
                    );
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally { this.uC_SearchItems1.SetFocus(); }
        }

        private bool IsItemAlreadyAdded()
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == this.SelectedItemId// Convert.ToInt32(cmbItems.SelectedValue)
                                                                                      //&& Convert.ToInt64(r.Cells["colBatchId"].Value) == Convert.ToInt64(cmbBatch.SelectedValue)
                    )
                {
                    MessageBox.Show("Selected Item Already Exists", "Alredy Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                }
            }

            return false;
        }
        private void Clear()
        {
            if (EditingRowIndex > -1)
            {
                grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
            }
            btnAddItem.Text = "Add Item";
            this.SelectedItemId = 0;
            this.SelectedItemName = "";
            uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
            uC_SearchItems1.SetText = "";
            this.uC_SearchItems1.SetFocus();
            this.uC_SearchItems1.SetFocus();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            if (grdItems.Rows.Count > 1)
            {
                DialogResult rs = MessageBox.Show("There Are Some Un-Saved Items, Are You Sure You Want To Clear It", "Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (rs == DialogResult.OK)
                {
                    ClearAll();
                }
            }
        }

        private void ClearAll()
        {
            Clear();
            this.ActionTime = null;
            grdItems.Rows.Clear();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            this.ActionTime = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
            try
            {
                if (grdItems.Rows.Count <= 0)
                {
                    MessageBox.Show("Can't Save Empty Adjustment, Please Add Some items", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (this.AdjustmentId > 0)
                {
                    UpdateAdjustment();
                    return;
                }
                InsertAdjustment();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Errror");
            }
        }

        private void InsertAdjustment()
        {
            using (unitOfWork = new UnitOfWork())
            {
                Adjustment objAdjustment = new Adjustment();
                objAdjustment.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                objAdjustment.AdjustmentType = (int)Enums.AdjustmentType.HOReturn;
                objAdjustment.CreatedAt = ActionTime.Value;
                objAdjustment.UpdatedAt = ActionTime.Value;
                objAdjustment.AdjustmentItems = new List<AdjustmentItem>();
                objAdjustment.IsNew = true;
                objAdjustment.IsActive = true;
                objAdjustment.GrandTotalRetailPrice = 0;
                objAdjustment.GrandTotalCostPrice = 0;
                objAdjustment.UserId = SharedVariables.LoggedInUser.UserId;
                double qty = 0;
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    qty = 0;
                    AdjustmentItem objItem = new AdjustmentItem();
                    long itemId = Convert.ToInt64(r.Cells["colItemId"].Value);
                    long batchId = Convert.ToInt64(r.Cells["colbatchid"].Value);
                    Item inventoryItem = unitOfWork.ItemRspository.GetById(itemId);
                    objItem.ItemId = itemId;
                    objItem.BatchId = 1;
                    objItem.Unit = r.Cells["colUnit"].Value.ToString().Equals("Units") ? 1 : 0;
                    double.TryParse(r.Cells["colQuantity"].Value.ToString(), out qty);
                    objItem.Quantity = qty;
                    objItem.Quantity = (objItem.Unit == 0 ? objItem.Quantity * inventoryItem.ConversionUnit : objItem.Quantity) * -1;
                    objItem.CostPrice = inventoryItem.UnitCostPrice;
                    objItem.RetailPrice = inventoryItem.RetailPrice;
                    objItem.TotalRetailPrice = objItem.Quantity * inventoryItem.RetailPrice;
                    objItem.TotalCostPrice = objItem.Quantity * inventoryItem.UnitCostPrice;

                    objAdjustment.GrandTotalCostPrice += objItem.TotalCostPrice;
                    objAdjustment.GrandTotalRetailPrice += objItem.TotalRetailPrice;

                    objItem.Reason = r.Cells["colReason"].Value == null ? "" : r.Cells["colReason"].Value.ToString();
                    Enums.ItemAdjustmentType itemAdjustmentType;
                    Enum.TryParse(r.Cells["colAdjustmentType"].Value.ToString(), out itemAdjustmentType);
                    objItem.AdjustmentType = (int)itemAdjustmentType;
                    objItem.CreatedAt = ActionTime.Value;
                    objItem.UpdatedAt = ActionTime.Value;
                    objItem.IsActive = true;
                    objItem.IsNew = true;
                    objItem.UserId = SharedVariables.LoggedInUser.UserId;

                    objAdjustment.AdjustmentItems.Add(objItem);
                }
                unitOfWork.AdjustmentRepository.Insert(objAdjustment);
                unitOfWork.Save();
            }
            MessageBox.Show("Return Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private void UpdateAdjustment()
        {
            using (unitOfWork = new UnitOfWork())
            {
                Adjustment objAdjustment = unitOfWork.AdjustmentRepository.GetAdjustmentById(this.AdjustmentId);
                objAdjustment.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                objAdjustment.GrandTotalRetailPrice = 0;
                objAdjustment.GrandTotalCostPrice = 0;
                if (objAdjustment.IsSynced)
                {
                    objAdjustment.IsNew = false;
                    objAdjustment.IsUpdate = true;
                    objAdjustment.IsSynced = false;
                }

                objAdjustment.UpdatedAt = ActionTime.Value;
                long AdjItemId = 0;
                int ItemId = 0;
                double qty = 0;
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    qty = 0;
                    AdjItemId = Convert.ToInt64(r.Cells["colAdjustmentItemId"].Value);
                    ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                    if (AdjItemId == 0)
                    {
                        AdjustmentItem objItem = new AdjustmentItem();
                        Item inventoryItem = unitOfWork.ItemRspository.GetById(ItemId);
                        //objItem.Batch = unitOfWork.BatchRepository.GetById(Convert.ToInt64(r.Cells["colbatchid"].Value));
                        objItem.BatchId = 1;
                        objItem.ItemId = ItemId;

                        objItem.Unit = r.Cells["colUnit"].Value.ToString().Equals("Units") ? 1 : 0;
                        double.TryParse(r.Cells["colQuantity"].Value.ToString(), out qty);
                        objItem.Quantity = qty;
                        objItem.Quantity = (objItem.Unit == 0 ? objItem.Quantity * inventoryItem.ConversionUnit : objItem.Quantity) * -1;

                        objItem.CostPrice = inventoryItem.UnitCostPrice;
                        objItem.RetailPrice = inventoryItem.RetailPrice;
                        objItem.TotalRetailPrice = objItem.Quantity * inventoryItem.RetailPrice;
                        objItem.TotalCostPrice = objItem.Quantity * inventoryItem.UnitCostPrice;

                        objAdjustment.GrandTotalCostPrice += objItem.TotalCostPrice;
                        objAdjustment.GrandTotalRetailPrice += objItem.TotalRetailPrice;

                        Enums.ItemAdjustmentType itemAdjustmentType;
                        Enum.TryParse(r.Cells["colAdjustmentType"].Value.ToString(), out itemAdjustmentType);
                        objItem.AdjustmentType = (int)itemAdjustmentType;
                        objItem.Reason = r.Cells["colReason"].Value.ToString();

                        objItem.CreatedAt = ActionTime.Value;
                        objItem.UpdatedAt = ActionTime.Value;
                        objItem.IsNew = true;
                        objItem.IsActive = true;
                        objItem.UserId = SharedVariables.LoggedInUser.UserId;

                        ////following 2 lines of code were added due to default items issue handeling.
                        //objItem.Item.IsSyncable = true;
                        //unitOfWork.GetDbContext().Entry(objItem.Item).State = System.Data.Entity.EntityState.Modified;

                        objAdjustment.AdjustmentItems.Add(objItem);
                    }
                    else
                    {
                        if (!r.Visible)
                        {
                            AdjustmentItem ai = objAdjustment.AdjustmentItems.Where(i => i.AdjustmentItemId == AdjItemId).FirstOrDefault();
                            ai.IsActive = false;
                            ai.IsUpdate = true;
                            ai.IsSynced = false;
                            ai.UpdatedAt = ActionTime.Value;
                            unitOfWork.GetDbContext().Entry(ai).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        }
                        else
                        {
                            var adjItem = objAdjustment.AdjustmentItems.FirstOrDefault(i => i.AdjustmentItemId == AdjItemId);
                            if (adjItem != null)
                            {
                                adjItem.ItemId = ItemId;
                                //adjItem.BatchId = 1;

                                adjItem.Unit = r.Cells["colUnit"].Value.ToString().Equals("Units") ? 1 : 0;
                                double.TryParse(r.Cells["colQuantity"].Value.ToString(), out qty);
                                adjItem.Quantity = qty;
                                adjItem.Quantity = (adjItem.Unit == 0 ? adjItem.Quantity * adjItem.Item.ConversionUnit : adjItem.Quantity) * -1;

                                adjItem.TotalRetailPrice = adjItem.Quantity * adjItem.RetailPrice;
                                adjItem.TotalCostPrice = adjItem.Quantity * adjItem.CostPrice;

                                objAdjustment.GrandTotalCostPrice += adjItem.TotalCostPrice;
                                objAdjustment.GrandTotalRetailPrice += adjItem.TotalRetailPrice;

                                Enums.ItemAdjustmentType itemAdjustmentType;
                                Enum.TryParse(r.Cells["colAdjustmentType"].Value.ToString(), out itemAdjustmentType);
                                adjItem.AdjustmentType = (int)itemAdjustmentType;
                                adjItem.UpdatedAt = ActionTime.Value;
                                if (adjItem.IsSynced)
                                {
                                    adjItem.IsUpdate = true;
                                    adjItem.IsSynced = false;
                                }
                                unitOfWork.GetDbContext().Entry(adjItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                //following 2 lines of code were added due to default items issue handeling.
                                //objItem.Item.IsSyncable = true;
                                //unitOfWork.GetDbContext().Entry(objItem.Item).State = System.Data.Entity.EntityState.Modified;
                            }
                        }
                    }
                    AdjItemId = 0;
                    ItemId = 0;
                }
                unitOfWork.AdjustmentRepository.Update(objAdjustment);
                unitOfWork.Save();
            }
            MessageBox.Show("Return Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private void cmbSelectBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void GetItemBatches(int ItemId)
        {
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
            //cmbBatch.DataSource = FinalResult;
            //cmbBatch.ValueMember = "BatchId";
            //cmbBatch.DisplayMember = "BatchName";
        }

        private bool isValidQuantityEntered()
        {
            //double AvailableStock = BatchStockList.Where(x => x.BatchId == Convert.ToInt32(cmbBatch.SelectedValue)).FirstOrDefault().AvailableStock;
            //if (numQty.Value < 0 && Math.Abs(numQty.Value) > (decimal)AvailableStock)
            //{
            //    //ToolTip tt = new ToolTip();
            //    //numQty.Value = 1;
            //    //tt.Show("Please Enter Valid Quantity", numQty, 2000);
            //    //numQty.Select(0, 1);
            //    return false;
            //}
            return true;
        }

        private void grdItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            newRowAdded(sender, e);
        }

        private void newRowAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            var r = grdItems.Rows[e.RowIndex];
            long ItemId = Convert.ToInt64(r.Cells["colItemID"].Value);

            Item itemData = new Item();
            double qty = 0; double.TryParse(r.Cells["colQuantity"].Value.ToString(), out qty);
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            Item objItem = new Item();
            ItemDetailVM itemDetail = new ItemDetailVM();
            FlatDiscount objDiscount = new FlatDiscount();
            using (unitOfWork = new UnitOfWork())
            {
                itemData = unitOfWork.ItemRspository.GetById(ItemId);
                //itemDetail = unitOfWork.ItemRspository.GetItemDetailWithBatchStock(ItemId, SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);
                //BatchStockList = itemDetail.BatchStockList.ToList();
            }
            //List<BatchStockVM> FinalResult = new List<BatchStockVM>();


            //foreach (BatchStockVM i in BatchStockList)
            //{
            //    i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock);
            //    if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold) // if not hold then add holded stock in total available stock
            //    {
            //        i.AvailableStock += i.HoldStock;
            //    }
            //    i.BatchName = i.BatchName + " [ " + i.AvailableStock + " ]";
            //    if (i.AvailableStock > 0)
            //    {
            //        FinalResult.Add(i);
            //    }
            //}

            //if (!SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo)
            //{
            //    DataGridViewComboBoxCell dgCmbBatch = (DataGridViewComboBoxCell)r.Cells["colBatch"];
            //    dgCmbBatch.DataSource = FinalResult;
            //    dgCmbBatch.ValueMember = "BatchId";
            //    dgCmbBatch.DisplayMember = "BatchName";

            //    if (FinalResult.Count > 0)
            //    {
            //        r.Cells["colBatchId"].Value = FinalResult[0].BatchId;
            //        r.Cells["colBatch"].Value = FinalResult[0].BatchName;
            //        if (SharedVariables.AdminPharmacySetting.IsItemDefUnitOnPOS)
            //        {
            //            avQty = FinalResult[0].AvailableStock / itemDetail.Item.ConversionUnit;
            //            Rate = FinalResult[0].RetailPrice * itemDetail.Item.ConversionUnit;
            //            unitCost = FinalResult[0].CostPrice * itemDetail.Item.ConversionUnit;
            //        }
            //        else
            //        {
            //            Rate = FinalResult[0].RetailPrice;
            //            avQty = FinalResult[0].AvailableStock;
            //            unitCost = FinalResult[0].CostPrice;
            //        }
            //    }
            //}
            //else
            //{
            //    double aq = FinalResult.Sum(f => f.AvailableStock);
            //    if (SharedVariables.AdminPharmacySetting.IsItemDefUnitOnPOS)
            //    {
            //        avQty = (aq / itemDetail.Item.ConversionUnit);
            //    }
            //    else
            //    {
            //        avQty = aq;
            //    }
            //}

            //r.Cells["colAvailableQty"].Value = avQty;
            //if (avQty < qty)
            //{
            //    if (!SharedVariables.AdminPharmacySetting.AllowNegCons)
            //    {
            //        MessageBox.Show("Availabe quantuity is less than enterd quantity. quantity will be set to available quantity automatically", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        r.Cells["colQuantity"].Value = avQty;
            //    }
            //    else
            //    {
            //        r.Cells["colQuantity"].Value = qty;
            //        //r.DefaultCellStyle.SelectionBackColor = Color.Red;
            //        //r.DefaultCellStyle.BackColor = Color.Red;
            //    }
            //}
            //else
            //{
            //    r.Cells["colQuantity"].Value = qty;
            //}
            r.Cells["colItem"].Value = itemData.ItemName;
            r.Cells["colConvUnit"].Value = itemData.ConversionUnit;
            r.Cells["colRetailPrice"].Value = itemData.RetailPrice;

            DataGridViewComboBoxCell dgCmbUnit = (DataGridViewComboBoxCell)r.Cells["colUnit"];
            string unit = itemData.Unit;
            dgCmbUnit.Items.Add(unit);
            dgCmbUnit.Items.Add("Units");

            if (SharedVariables.AdminPharmacySetting.IsItemDefUnitOnPOS)
            {
                r.Cells["colUnit"].Value = dgCmbUnit.Items[0];
            }
            else
            {
                r.Cells["colUnit"].Value = dgCmbUnit.Items[1];
            }

            r.Cells["colConvUnit"].Value = itemData.ConversionUnit;
            CalculateItemTotals(r);
            CalculateTotals();
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }

        private void CalculateItemTotals(DataGridViewRow r)
        {
            double retPrice = 0; double.TryParse(r.Cells["colRetailPrice"].Value == null ? "0" : r.Cells["colRetailPrice"].Value.ToString(), out retPrice);
            double qty = 0; double.TryParse(r.Cells["colQuantity"].Value != null ? r.Cells["colQuantity"].Value.ToString() : "0", out qty);
            string unit = r.Cells["colUnit"].Value.ToString();
            int convUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
            qty = unit == "Units" ? qty : qty / convUnit;
            var total = qty * retPrice;
            r.Cells["colTotal"].Value = total;
        }

        private void CalculateTotals()
        {
            double total = 0;
            double rowTotal = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                rowTotal = 0;
                double.TryParse(r.Cells["colTotal"].Value == null ? "0" : r.Cells["colTotal"].Value.ToString(), out rowTotal);
                total += rowTotal;
            }
            txtTotalReturn.Text = total.ToString();
        }

        private void grdItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == grdItems.Columns["colQuantity"].Index)
            {
                CalculateItemTotals(grdItems.Rows[e.RowIndex]);
                CalculateTotals();
            }
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }

        private void grdItems_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colAdjustmentType"].Index)
            {
                grdItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void grdItems_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void grdItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

        }
    }
}