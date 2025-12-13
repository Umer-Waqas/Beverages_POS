using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GK.Shared.Repository;
using Restaurant_MS_Core.Entities;
using Restaurant_MS_Core.ViewModels;
using GK.Shared.Repository;
using GK.Shared.Core;

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmStockAdjustment : Form
    {
        UnitOfWork unitOfWork;
        private long AdjustmentId = 0;
        List<BatchStockVM> BatchStockList;
        int EditingRowIndex = -1;
        BackgroundWorker bgWItemsLoader = new BackgroundWorker();
        List<Item> Items = new List<Item>();
        int SelectedItemId = 0;
        string SelectedItemName = "";

        DateTime ActionTime;

        public frmStockAdjustment()
        {
            InitializeComponent();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }
        public frmStockAdjustment(long AdjustmentId)
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
                LoadItemData(this.SelectedItemId);
                GetItemBacthes(this.SelectedItemId);
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
        private void frmAddStock_Load(object sender, EventArgs e)
        {
            try
            {
                if (SharedFunctions.CheckDayClosed())
                {
                    this.BeginInvoke(new MethodInvoker(Close));
                    return;
                }
                if (this.AdjustmentId > 0)
                {
                    LoadAdjustment();
                    grdItems.Columns["colEdit"].Visible = false;
                }
                //cmbItems.SelectedIndexChanged -= cmbItems_SelectedIndexChanged;
                LoadItems();
                //cmbItems.SelectedIndex = -1;
                //cmbItems.SelectedIndexChanged += cmbItems_SelectedIndexChanged;
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
            if (keyData == (Keys.Alt | Keys.I))
            {
                //cmbItems.Focus();
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
                    i.UnitString = i.Unit == 0 ? i.Item.Unit : "Units";
                    i.Quantity = i.Unit == 0 ? i.Quantity / i.Item.ConversionUnit : i.Quantity;
                    grdItems.Rows.Add(i.AdjustmentItemId, i.Item.ItemId, i.Item.ItemName, i.Batch.BatchId, i.Batch.BatchName, i.UnitString, i.Item.ConversionUnit, i.Quantity, i.RetailPrice, i.CostPrice, i.TotalRetailPrice, i.TotalCostPrice, i.Reason);
                    grdItems.Rows[count].DefaultCellStyle.BackColor = Color.LightGray;
                    count++;
                }
            }
        }
        private void bgWItemsLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate ()
            {
                //cmbItems.Enabled = false;
                lblLoadingItems.Visible = true;
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
            //cmbItems.SelectedIndexChanged -= cmbItems_SelectedIndexChanged;
            //cmbItems.DataSource = Items;
            //cmbItems.ValueMember = "ItemId";
            //cmbItems.DisplayMember = "ItemName";
            //cmbItems.SelectedIndexChanged += cmbItems_SelectedIndexChanged;
            //cmbItems.SelectedIndex = -1;
            //cmbItems.Enabled = true;
            lblLoadingItems.Visible = false;
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

                if (AdjItemId > 0)
                {
                    MessageBox.Show("You Can't Edit This Item", "Inavalid Operation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Clear();
                FillForm(grdItems.Rows[e.RowIndex]);
                btnAddItem.Text = "Update Item";
                //cmbItems.Enabled = false;
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
                cmbBatch.DataSource = null;
                cmbBatch.SelectedIndex = -1;

                cmbBatch.SelectedIndexChanged -= cmbSelectBatch_SelectedIndexChanged;
                GetItemBatches(ItemId);
                cmbBatch.SelectedIndexChanged += cmbSelectBatch_SelectedIndexChanged;
                if (cmbBatch.Items.Count > 0)
                {
                    cmbBatch.SelectedIndex = 0;
                    cmbSelectBatch_SelectedIndexChanged(null, null);
                }
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
            cmbBatch.SelectedValue = Convert.ToInt32(r.Cells["colBatchId"].Value);
            //cmbConsumtionType.SelectedIndex = cmbConsumtionType.FindStringExact(r.Cells["colConsumptionType"].Value.ToString());
            txtReason.Text = r.Cells["colReason"].Value.ToString();
            numQty.Value = Convert.ToInt32(r.Cells["colQuantity"].Value);
        }
        public void Edit(DataGridViewRow r)
        {
            //cmbItems.SelectedValue = Convert.ToInt32(r.Cells["colItemId"].Value);
            cmbBatch.SelectedValue = Convert.ToInt32(r.Cells["colBatchId"].Value);
            numQty.Value = Convert.ToDecimal(r.Cells["colQuantity"].Value);
            txtReason.Text = r.Cells["colReason"].Value.ToString();
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
                if (IsValidInput())
                {
                    AdjustmentItem objAdjItem = new AdjustmentItem();
                    FillObject(objAdjItem);
                    if (this.EditingRowIndex >= 0)
                    {
                        if (!isValidQuantityEntered())
                        {
                            MessageBox.Show("Adjustment Can't be Less Than Available Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            numQty.Focus();
                            numQty.Select(0, numQty.Value.ToString().Length);
                            return;
                        }
                        UpdateRow(objAdjItem);
                    }
                    else
                    {
                        if (IsItemAlreadyAdded())
                        {
                            return;
                        }

                        if (!isValidQuantityEntered())
                        {
                            MessageBox.Show("Adjustment Can't be Less Than Available Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            numQty.Focus();
                            numQty.Select(0, numQty.Value.ToString().Length);
                            return;
                        }
                        grdItems.Rows.Add(0, objAdjItem.ItemId, objAdjItem.ItemName, objAdjItem.BatchId, objAdjItem.BatchName, objAdjItem.UnitString, objAdjItem.ConversionUnit, objAdjItem.Quantity, objAdjItem.RetailPrice, objAdjItem.CostPrice, objAdjItem.TotalRetailPrice, objAdjItem.TotalCostPrice, objAdjItem.Reason);
                    }
                    Clear();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            finally { this.uC_SearchItems1.SetFocus(); }
        }
        private void UpdateRow(AdjustmentItem objAdjustment)
        {
            DataGridViewRow r = grdItems.Rows[EditingRowIndex];
            r.Cells["colBatchId"].Value = objAdjustment.BatchId;
            r.Cells["colBatch"].Value = objAdjustment.BatchName;

            r.Cells["colUnit"].Value = objAdjustment.UnitString;
            r.Cells["colConvUnit"].Value = objAdjustment.ConversionUnit;

            r.Cells["colQuantity"].Value = objAdjustment.Quantity;

            r.Cells["colRetailPrice"].Value = objAdjustment.RetailPrice;
            r.Cells["colCostPrice"].Value = objAdjustment.CostPrice;
            r.Cells["colTotalRetailPrice"].Value = objAdjustment.TotalRetailPrice;
            r.Cells["colTotalCostPrice"].Value = objAdjustment.TotalCostPrice;

            r.Cells["colReason"].Value = objAdjustment.Reason;
            grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
        }
        private void FillObject(AdjustmentItem objAdjItem)
        {
            Tuple<double, double> itemPrices;
            using (unitOfWork = new UnitOfWork())
            {
                itemPrices = unitOfWork.ItemRspository.GetItemPrices(this.SelectedItemId);
            }
            objAdjItem.ItemId = this.SelectedItemId; //Convert.ToInt32(cmbItems.SelectedValue);
            objAdjItem.ItemName = this.SelectedItemName; //cmbItems.GetItemText(cmbItems.SelectedItem);
            objAdjItem.UnitString = cmbUnit.GetItemText(cmbUnit.SelectedItem);
            objAdjItem.Unit = cmbUnit.GetItemText(cmbUnit.SelectedItem).ToString() == "Units" ? 1 : 0;
            objAdjItem.ConversionUnit = (int)numConvUnit.Value;
            objAdjItem.Quantity = Convert.ToDouble(numQty.Value);

            objAdjItem.RetailPrice = itemPrices.Item1;
            objAdjItem.CostPrice = itemPrices.Item2;
            objAdjItem.TotalRetailPrice = objAdjItem.Unit == 0 ? objAdjItem.RetailPrice * objAdjItem.ConversionUnit * objAdjItem.Quantity : objAdjItem.Quantity * objAdjItem.RetailPrice;
            objAdjItem.TotalCostPrice = objAdjItem.Unit == 0 ? objAdjItem.CostPrice * objAdjItem.ConversionUnit * objAdjItem.Quantity : objAdjItem.Quantity * objAdjItem.CostPrice;
            objAdjItem.BatchId = Convert.ToInt32(cmbBatch.SelectedValue);
            objAdjItem.BatchName = cmbBatch.GetItemText(cmbBatch.SelectedItem);
            objAdjItem.Reason = txtReason.Text.Trim();
        }
        private bool IsItemAlreadyAdded()
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == this.SelectedItemId// Convert.ToInt32(cmbItems.SelectedValue)
                    && Convert.ToInt64(r.Cells["colBatchId"].Value) == Convert.ToInt64(cmbBatch.SelectedValue)
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
            //cmbItems.Enabled = true;
            btnAddItem.Text = "Add Item";
            this.SelectedItemId = 0;
            this.SelectedItemName = "";
            uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
            uC_SearchItems1.SetText = "";
            uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            cmbUnit.Items.Clear();
            cmbUnit.SelectedIndex = -1;
            numConvUnit.Value = 1;
            cmbBatch.DataSource = null;
            EditingRowIndex = -1;
            //cmbItems.SelectedIndex = -1;
            numQty.Value = 1;
            txtReason.Text = "";
            this.uC_SearchItems1.SetFocus();
            this.uC_SearchItems1.SetFocus();
        }
        private bool IsValidInput()
        {

            bool ErrFound = false;
            if (this.SelectedItemId <= 0) // (cmbItems.SelectedIndex < 0)
            {
                ErrSelectItem.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrSelectItem.Visible = false;
            }
            if (cmbBatch.SelectedIndex < 0)
            {
                errSelectBatch.Visible = true;
                ErrFound = true;
            }
            else
            {
                errSelectBatch.Visible = false;
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
                objAdjustment.UserId = SharedVariables.LoggedInUser.UserId;
                objAdjustment.CreatedAt = this.ActionTime;
                objAdjustment.UpdatedAt = this.ActionTime;
                objAdjustment.AdjustmentItems = new List<AdjustmentItem>();
                objAdjustment.IsNew = true;
                objAdjustment.AdjustmentType = (int)Enums.AdjustmentType.Adjustment;
                objAdjustment.IsActive = true;
                objAdjustment.GrandTotalRetailPrice = 0;
                objAdjustment.GrandTotalCostPrice = 0;
                objAdjustment.UserId = SharedVariables.LoggedInUser.UserId;
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    AdjustmentItem objItem = new AdjustmentItem();
                    objItem.Item = unitOfWork.ItemRspository.GetById(Convert.ToInt32(r.Cells["colItemId"].Value));
                    objItem.Batch = unitOfWork.BatchRepository.GetById(Convert.ToInt64(r.Cells["colbatchid"].Value));
                    objItem.Unit = r.Cells["colUnit"].Value.ToString().Equals("Units") ? 1 : 0;
                    objItem.Quantity = Convert.ToDouble(r.Cells["colQuantity"].Value);
                    objItem.Quantity = objItem.Unit == 0 ? objItem.Quantity * objItem.Item.ConversionUnit : objItem.Quantity;

                    objItem.RetailPrice = Convert.ToDouble(r.Cells["colRetailPrice"].Value);
                    objItem.CostPrice = Convert.ToDouble(r.Cells["colCostPrice"].Value);
                    objItem.AdjustmentType = (int)Enums.ItemAdjustmentType.Adjustment;
                    objAdjustment.GrandTotalRetailPrice += objItem.TotalRetailPrice = Convert.ToDouble(r.Cells["colTotalRetailPrice"].Value);
                    objAdjustment.GrandTotalCostPrice += objItem.TotalCostPrice = Convert.ToDouble(r.Cells["colTotalCostPrice"].Value);

                    objItem.Reason = r.Cells["colReason"].Value.ToString();
                    objItem.CreatedAt = this.ActionTime;
                    objItem.UpdatedAt = this.ActionTime;
                    objItem.IsActive = true;
                    objItem.IsNew = true;
                    objItem.Item.IsSyncable = true;
                    objItem.UserId = SharedVariables.LoggedInUser.UserId;
                    unitOfWork.GetDbContext().Entry(objItem.Item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                    objAdjustment.AdjustmentItems.Add(objItem);
                }
                unitOfWork.AdjustmentRepository.Insert(objAdjustment);
                unitOfWork.Save();
            }
            MessageBox.Show("Adjustment Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //ClearAll();
            this.Close();
        }
        private void UpdateAdjustment()
        {
            using (unitOfWork = new UnitOfWork())
            {
                Adjustment objAdjustment = unitOfWork.AdjustmentRepository.GetAdjustmentById(this.AdjustmentId);
                objAdjustment.UserId = SharedVariables.LoggedInUser.UserId;

                if (objAdjustment.IsSynced)
                {
                    objAdjustment.IsNew = false;
                    objAdjustment.IsUpdate = true;
                    objAdjustment.IsSynced = false;
                }

                objAdjustment.UpdatedAt = this.ActionTime;
                long AdjItemId = 0;
                int ItemId = 0;
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    AdjItemId = Convert.ToInt64(r.Cells["colAdjustmentItemId"].Value);
                    ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                    if (AdjItemId == 0)
                    {
                        AdjustmentItem objItem = new AdjustmentItem();
                        objItem.Item = unitOfWork.ItemRspository.GetById(ItemId);
                        objItem.Batch = unitOfWork.BatchRepository.GetById(Convert.ToInt64(r.Cells["colbatchid"].Value));

                        objItem.Unit = r.Cells["colUnit"].Value.ToString().Equals("Units") ? 1 : 0;
                        objItem.Quantity = Convert.ToDouble(r.Cells["colQuantity"].Value);
                        objItem.Quantity = objItem.Unit == 0 ? objItem.Quantity * objItem.Item.ConversionUnit : objItem.Quantity;

                        objItem.RetailPrice = Convert.ToDouble(r.Cells["colRetailPrice"].Value);
                        objItem.CostPrice = Convert.ToDouble(r.Cells["colCostPrice"].Value);
                        objAdjustment.GrandTotalRetailPrice += objItem.TotalRetailPrice = Convert.ToDouble(r.Cells["colTotalRetailPrice"].Value);
                        objAdjustment.GrandTotalCostPrice += objItem.TotalCostPrice = Convert.ToDouble(r.Cells["colTotalCostPrice"].Value);
                        objItem.AdjustmentType = (int)Enums.ItemAdjustmentType.Adjustment;
                        objItem.Reason = r.Cells["colReason"].Value.ToString();
                        objItem.CreatedAt = this.ActionTime;
                        objItem.UpdatedAt = this.ActionTime;
                        objItem.IsNew = true;
                        objItem.IsActive = true;
                        objItem.UserId = SharedVariables.LoggedInUser.UserId;

                        //following 2 lines of code were added due to default items issue handeling.
                        objItem.Item.IsSyncable = true;
                        unitOfWork.GetDbContext().Entry(objItem.Item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

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
                            ai.UpdatedAt = this.ActionTime;
                            unitOfWork.GetDbContext().Entry(ai).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        }
                    }
                    AdjItemId = 0;
                    ItemId = 0;
                }
                unitOfWork.AdjustmentRepository.Update(objAdjustment);
                unitOfWork.Save();
            }
            MessageBox.Show("Adjustment Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // ClearAll();
            this.Close();
        }
        private void cmbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbBatch.DataSource = null;
            cmbBatch.SelectedIndex = -1;
            if (/*cmbItems.SelectedIndex*/ 0 >= 0)
            {
                cmbBatch.SelectedIndexChanged -= cmbSelectBatch_SelectedIndexChanged;
                GetItemBatches(Convert.ToInt32("cmbItems.SelectedValue"));
                cmbBatch.SelectedIndexChanged += cmbSelectBatch_SelectedIndexChanged;
            }
            if (cmbBatch.Items.Count > 0)
            {
                cmbBatch.SelectedIndex = 0;
                cmbSelectBatch_SelectedIndexChanged(null, null);
            }
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
            cmbBatch.DataSource = FinalResult;
            cmbBatch.ValueMember = "BatchId";
            cmbBatch.DisplayMember = "BatchName";
        }

        private void numQty_ValueChanged(object sender, EventArgs e)
        {
            if (cmbBatch.SelectedIndex >= 0)
            {
                if (numQty.Value < 0)
                {
                    //if (isValidQuantityEntered())
                    //{
                    //    numQty.BackColor = SystemColors.Window;
                    //}
                    //else
                    //{
                    //    numQty.BackColor = Color.Coral;
                    //}
                }
            }
        }

        private bool isValidQuantityEntered()
        {
            double AvailableStock = BatchStockList.Where(x => x.BatchId == Convert.ToInt32(cmbBatch.SelectedValue)).FirstOrDefault().AvailableStock;
            if (numQty.Value < 0 && Math.Abs(numQty.Value) > (decimal)AvailableStock)
            {
                //ToolTip tt = new ToolTip();
                //numQty.Value = 1;
                //tt.Show("Please Enter Valid Quantity", numQty, 2000);
                //numQty.Select(0, 1);
                return false;
            }
            return true;
        }
    }
}