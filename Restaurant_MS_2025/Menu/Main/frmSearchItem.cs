

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmSearchItem : Form
    {
        UnitOfWork unitOfWork;
        public delegate void ItemSelected(int ItemId, int Quantity);
        public ItemSelected OnItemSelected;
        public int SelectedItemId = 0;
        DataView dataView = new DataView();
        public frmSearchItem()
        {
            InitializeComponent();
        }

        private void frmSearchItem_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetGridStyle(grdItems);
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnClose, btnRefresh });
                LoadItemsGrid();
                btnRefresh.Visible = SharedVariables.ReloadItemsRequired;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading items data, please try restart appliaction.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadItemsGrid()
        {
            if (SharedVariables.BulkLoadedItemsDataView != null)
            {
                SharedVariables.BulkLoadedItemsDataView.RowFilter = "";
                grdItems.DataSource = SharedVariables.BulkLoadedItemsDataView;
                grdItems.Columns["ItemId"].Visible = false;
                grdItems.Columns["Item Name"].MinimumWidth = 100;
                grdItems.Columns["Item Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                //grdItems.Columns["Generic Name"].MinimumWidth = 100;
                //grdItems.Columns["Generic Name"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                grdItems.Columns["Retail Price"].MinimumWidth = 60;
                //grdItems.Columns["Cost Price"].MinimumWidth = 60;
                grdItems.Columns["Retail Price"].Width = 60;
                //grdItems.Columns["Cost Price"].Width = 60;
                foreach (DataGridViewColumn col in grdItems.Columns)
                {
                    if (col.Name != "Quantity")
                        col.ReadOnly = true;
                }
                DataGridViewButtonColumn c = new DataGridViewButtonColumn();
                c.Name = "colLoad";
                c.Text = "Load";
                c.HeaderText = "Load";
                c.UseColumnTextForButtonValue = true;
                c.Width = 60;
                grdItems.Columns.Add(c);
                DataGridViewButtonColumn c2 = new DataGridViewButtonColumn();
                c2.Name = "colMissedSale";
                c2.Text = "Missed Sale";
                c2.HeaderText = "Missed Sale";
                c2.UseColumnTextForButtonValue = true;
                c2.Width = 110;
                grdItems.Columns.Add(c2);
            }
            else
            {
                MessageBox.Show("No items could be found, please press reresh button, to latest items", "Items Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                int ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["ItemId"].Value);
                int Quantity = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["Quantity"].Value);
                if (e.ColumnIndex == grdItems.Columns["colLoad"].Index)
                {
                    //this.SelectedItemId = ItemId;
                    this.OnItemSelected(ItemId, Quantity);
                }
                if (e.ColumnIndex == grdItems.Columns["colMissedSale"].Index)
                {
                    try
                    {

                        using (unitOfWork = new UnitOfWork())
                        {
                            if (!unitOfWork.MissedsalesRepository.ItemMissedSaleExists(ItemId))
                            {
                                MissedSale ms = new MissedSale();
                                ms.CreatedAt = DateTime.Now;
                                ms.UpdatedAt = DateTime.Now;
                                ms.IsActive = true;
                                ms.IsSynced = false;
                                ms.IsNew = true;
                                ms.IsUpdate = false;
                                ms.Item = unitOfWork.ItemRspository.GetById(ItemId);
                                ms.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                                unitOfWork.MissedsalesRepository.Insert(ms);
                                unitOfWork.Save();
                            }
                        }
                        MessageBox.Show("Recorded as missed sale.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while recording item as missed sale.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (!this.chkHold.Checked)
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading item, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //grdItems.Rows.Clear();
            try
            {
                //var res = SharedVariables.BulkLoadedItems.Where(i => i.ItemName.ToLower().StartsWith(txtSearch.Text.Trim()));
                //grdItems.Rows.Clear();
                //foreach (BulkItemsVM i in res)
                //{
                //    grdItems.Rows.Add(i.ItemId, i.ItemName, i.RetailPrice);
                //}
                //SharedVariables.BulkLoadedItemsDataView.RowFilter = "[Item Name] Like '" + txtSearch.Text.Trim() + "*' OR [Generic Name] Like '" + txtSearch.Text.Trim() + "*' " + "OR [Manufacturer] Like'" + txtSearch.Text.Trim() + "*' " + "OR [Barcode] ='" + txtSearch.Text.Trim() + "'";
                SharedVariables.BulkLoadedItemsDataView.RowFilter = "[Item Name] Like '*" + /*txtSearch.Text.Trim()*/  "this code was cmommented, check this part" + "*'";
                //foreach(var r in dataView)
                //{
                //    grdItems.Rows.Add(r["ItemId"], r["ItemName"], r[""]);
                //}
                grdItems.DataSource = SharedVariables.BulkLoadedItemsDataView;
                //grdItems.DataSource = dataView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while performing search, please try again.", "Search Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        bool itemsLoaded = false;

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.I) || keyData == (Keys.Shift | Keys.OemQuestion))
            {
                //txtSearch.Focus();
                //cmbSelectItems.Focus();
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
                //txtSearch.Text = "";
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                grdItems.CurrentCell = grdItems.Rows[0].Cells["Quantity"];
                grdItems.BeginEdit(true);
            }
            //try
            //{
            //    if (e.KeyCode == Keys.Enter)
            //    {
            //        if (itemsLoaded && txtSearch.Text.Trim() == "")
            //        {
            //            return;
            //        }
            //        grdItems.Rows.Clear();
            //        if (txtSearch.Text.Trim() == "")
            //        {
            //            foreach (BulkItemsVM i in SharedVariables.BulkLoadedItems)
            //            {
            //                grdItems.Rows.Add(i.ItemId, i.ItemName, i.RetailPrice);
            //            }
            //            itemsLoaded = true;
            //        }
            //        else
            //        {
            //            var res = SharedVariables.BulkLoadedItems.Where(i => i.ItemName.ToLower().Contains(txtSearch.Text.Trim()));
            //            foreach (BulkItemsVM i in res)
            //            {
            //                grdItems.Rows.Add(i.ItemId, i.ItemName, i.RetailPrice);
            //            }
            //            itemsLoaded = false;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("An error occurred while performing search, please try again.", "Search Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            grdItems.DataSource = null;
            grdItems.Columns.Clear();
            DataTable dt = new DataTable();
            dt.Columns.Add("ItemId", typeof(int));
            dt.Columns.Add("Item Name", typeof(string));
            dt.Columns.Add("Generic Name", typeof(string));
            dt.Columns.Add("Manufacturer", typeof(string));
            dt.Columns.Add("Retail Price", typeof(double));

            List<BulkItemsVM> bulkList = new List<BulkItemsVM>();
            using (unitOfWork = new UnitOfWork())
            {
                bulkList = unitOfWork.ItemRspository.GetBulkAllActiveItems();
            }
            foreach (var i in bulkList)
            {
                dt.Rows.Add(i.ItemId, i.ItemName, i.GenericName, i.Manufacturer, i.RetailPrice);
                SharedVariables.BulkLoadedItemsDataView = new DataView(dt);
            }
            SharedVariables.BulkLoadedItemsList = bulkList;
            LoadItemsGrid();
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmItemSearchShortCuts f = new ShortCutDialogs.frmItemSearchShortCuts();
            f.ShowDialog();
        }

        private void grdItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            int ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["ItemId"].Value);
            int Quantity = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["Quantity"].Value);
            this.OnItemSelected(ItemId, Quantity);
            if (!this.chkHold.Checked)
            {
                this.Close();
            }
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

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }

           
            //if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            //{
            //    e.Handled = true;
            //}
        }
        private void grdItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Control_KeyPress);
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["Quantity"].Index)
            {
                e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress); // to allow onyl integer input here in the datagridview               
            }
        }
    }
}