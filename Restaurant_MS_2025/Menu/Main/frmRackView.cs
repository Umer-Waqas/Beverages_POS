

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmRackView : Form
    {
        UnitOfWork unitOfWork;
        int pageNo = 1;
        IPagedList<System.Linq.IGrouping<Rack, Item>> racksList;
        bool isDateChanged = false;
        DateTime ActionTime;
        public frmRackView()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }
        private void frmManageStocks_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnManageRacks });
            LoadSuppliers();
            LoadStocks();
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {

            if (keyData == (Keys.Control | Keys.P))
            {
                //btnPrint.PerformClick();
                return true;
            }



            if (keyData == (Keys.Control | Keys.Shift | Keys.T))
            {
                btnManageRacks.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void LoadSuppliers()
        {

            List<Supplier> suppliers = unitOfWork.SupplierRepository.GetAll().ToList();
            Supplier Select = new Supplier();
            Select.Name = "Select";
            Select.SupplierID = 0;
            suppliers.Insert(0, Select);
        }

        private void LoadStocks()
        {
            try
            {
                flowPanel.Controls.Clear();
                racksList = unitOfWork.RackRepository.getRackView(pageNo, SharedVariables.PageSize);
                //int ActiveItemsCount = 0;
                foreach (var g in racksList.Items)
                {
                    if (g.Key != null)
                    {
                        BuildPanel(g);
                    }
                }
                btnLastPage.Enabled = btnNext.Enabled = racksList.HasNextPage ? true : false;
                btnFirstPage.Enabled = btnPrevious.Enabled = racksList.HasPreviousPage ? true : false;
                SharedFunctions.ShowPageNo(lblPageNo, pageNo, racksList.PageCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading loading form", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BuildPanel(System.Linq.IGrouping<Rack, Item> group)
        {
            Panel pnlHeader = new Panel();
            // 
            // lblSupplier
            // 
            Label lblRack = new Label();
            lblRack.AutoSize = true;
            lblRack.Location = new System.Drawing.Point(14, 15);
            lblRack.Name = "lblSupplier";
            lblRack.Size = new System.Drawing.Size(35, 13);
            lblRack.TabIndex = 1;
            lblRack.Text = group.Key.Name;

            // 
            // btnPrint
            // 
            Button btnPrint = new Button();
            btnPrint.Location = new System.Drawing.Point(1205, 0);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new System.Drawing.Size(51, 39);
            btnPrint.TabIndex = 1;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Tag = 0;
            btnPrint.Click += new EventHandler(btnPrint_Click);
            // 
            // btnDelete
            // 
            Button btnDelete = new Button();
            btnDelete.Location = new System.Drawing.Point(1152, 0);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(51, 39);
            btnDelete.TabIndex = 1;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Tag = 0;
            btnDelete.Enabled = true;//ConsumptionsCount > 0 ? false : true;
            btnDelete.Click += new EventHandler(btnDelete_Click);
            // 
            // btnEdit
            //
            Button btnEdit = new Button();
            btnEdit.Location = new System.Drawing.Point(1099, 0);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(51, 39);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Tag = 0;
            btnEdit.Enabled = true;// ConsumptionsCount == s.StockItems.Count ? false : true;
            btnEdit.Click += new EventHandler(btnEdit_Click);

            pnlHeader.BackColor = Color.FromArgb(241, 241, 241);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1258, 39);
            pnlHeader.TabIndex = 0;
            //pnlHeader.Controls.Add(btnEdit);
            //pnlHeader.Controls.Add(btnDelete);
            //pnlHeader.Controls.Add(btnPrint);
            //pnlHeader.Controls.Add(lblDate);
            pnlHeader.Controls.Add(lblRack);
            //lblDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top)));
            //btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // grdStockItems
            // 
            DataGridView grdStockItem = new DataGridView();
            GenerateDatagridView(grdStockItem);
            grdStockItem.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grdStockItem.ColumnHeadersHeight = 35;
            grdStockItem.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            grdStockItem.EnableHeadersVisualStyles = false;
            grdStockItem.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grdStockItem.RowTemplate.Height = 35;
            grdStockItem.RowHeadersVisible = false;
            grdStockItem.BackgroundColor = Color.White;
            grdStockItem.Dock = System.Windows.Forms.DockStyle.Fill;
            grdStockItem.Location = new System.Drawing.Point(0, 39);
            grdStockItem.Name = "grdStockItems";
            grdStockItem.Size = new System.Drawing.Size(1258, 201);
            grdStockItem.TabIndex = 1;
            grdStockItem.AllowUserToAddRows = false;
            grdStockItem.AllowUserToDeleteRows = false;
            grdStockItem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            SharedFunctions.SetGridStyle(grdStockItem);
            foreach (Item i in group)
            {
                if (i.IsActive)
                {
                    grdStockItem.Rows.Add(i.ItemId, i.ItemName);
                }
            }
            // 
            // pnlDetail
            // 
            Panel pnlDetail = new Panel();
            pnlDetail.Tag = group.Key.RackId;

            pnlDetail.Controls.Add(grdStockItem);
            pnlDetail.Controls.Add(pnlHeader);
            pnlDetail.Location = new System.Drawing.Point(3, 3);
            pnlDetail.Name = "pnlDetail";
            pnlDetail.Size = new System.Drawing.Size(1258, 240);
            pnlDetail.TabIndex = 0;
            flowPanel.Controls.Add(pnlDetail);
            pnlDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            flowPanel.Refresh();
            SharedFunctions.SetGridStyle(grdStockItem);
        }
        private void GenerateDatagridView(DataGridView SourceGrid)
        {
            DataGridViewTextBoxColumn colItemId;
            DataGridViewTextBoxColumn colItemName;
            //DataGridViewTextBoxColumn colUnit;
            //DataGridViewTextBoxColumn colConvUnit;
            //DataGridViewTextBoxColumn colQuantity;
            //DataGridViewTextBoxColumn colUnitCost;
            //DataGridViewTextBoxColumn colToalCost;
            //DataGridViewTextBoxColumn colRetailPrice;
            //DataGridViewTextBoxColumn colBatch;
            //DataGridViewTextBoxColumn colDiscount;
            //DataGridViewTextBoxColumn colSalesTax;
            //DataGridViewTextBoxColumn colNetValue;
            //DataGridViewTextBoxColumn colCreatedAt;

            colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colConvUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colUnitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colToalCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colRetailPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colBatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colSalesTax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colNetValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            //colCreatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();

            SourceGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            colItemId,
            colItemName,
            //colUnit,
            //colConvUnit,
            //colQuantity,
            //colUnitCost,
            //colToalCost,
            //colRetailPrice,
            //colBatch,
            //colDiscount,
            //colSalesTax,
            //colNetValue,
            //colCreatedAt
            });
            dataGridView1.Location = new System.Drawing.Point(15, 28);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new System.Drawing.Size(1258, 150);
            dataGridView1.TabIndex = 12;
            dataGridView1.Visible = false;
            // 
            // colItemId
            // 
            colItemId.HeaderText = "ItemId";
            colItemId.Name = "colItemId";
            colItemId.ReadOnly = true;
            colItemId.Visible = false;
            // 
            // colItemName
            // 
            colItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colItemName.HeaderText = "Item";
            colItemName.Name = "colItemName";
            colItemName.ReadOnly = true;

            // 
            // colUnit
            // 
            //colUnit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            //colUnit.HeaderText = "Unit";
            //colUnit.Width = 70;
            //colUnit.Name = "colUnit";
            //colUnit.ReadOnly = true;

            // 
            // colConvUnit
            // 
            //colConvUnit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            //colConvUnit.Width = 40;
            //colConvUnit.HeaderText = "Conv.Unit";
            //colConvUnit.Name = "colConvUnit";
            //colConvUnit.ReadOnly = true;
            // 
            // colQuantity
            // 
            colQuantity.HeaderText = "Quantity";
            colQuantity.Name = "colQuantity";
            colQuantity.ReadOnly = true;
            // 
            // colUnitCost
            // 
            colUnitCost.HeaderText = "UnitCost";
            colUnitCost.Name = "colUnitCost";
            colUnitCost.ReadOnly = true;
            // 
            // colToalCost
            // 
            colToalCost.HeaderText = "Total Cost";
            colToalCost.Name = "colToalCost";
            colToalCost.ReadOnly = true;
            // 
            // colRetailPrice
            // 
            colRetailPrice.HeaderText = "Retail Price";
            colRetailPrice.Name = "colRetailPrice";
            colRetailPrice.ReadOnly = true;
            // 
            // colBatch
            // 
            colBatch.HeaderText = "Batch";
            colBatch.Name = "colBatch";
            colBatch.ReadOnly = true;
            // 
            // colDiscount
            // 
            colDiscount.HeaderText = "Discount";
            colDiscount.Name = "colDiscount";
            colDiscount.ReadOnly = true;
            // 
            // colSalesTax
            // 
            colSalesTax.HeaderText = "Sales Tax";
            colSalesTax.Name = "colSalesTax";
            colSalesTax.ReadOnly = true;
            // 
            // colNetValue
            // 
            colNetValue.HeaderText = "Net Value";
            colNetValue.Name = "colNetValue";
            colNetValue.ReadOnly = true;
            // 
            // colCreatedAt
            // 
            colCreatedAt.HeaderText = "Create At";
            colCreatedAt.Name = "colCreatedAt";
            colCreatedAt.ReadOnly = true;
        }
        private void flowPanel_SizeChanged(object sender, EventArgs e)
        {
            foreach (Panel p in flowPanel.Controls)
            {
                p.Width = flowPanel.Width - 23;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!SharedFunctions.IsActionallowed("Edit Stock") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            long Stockid = (long)((Button)sender).Tag;
            btn f = new btn(Stockid, 0);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
            f.Show();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.ActionTime = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
            DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete This Record", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (rs == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            long Stockid = (long)((Button)sender).Tag;
            unitOfWork.StockRepository.SetInactive(Stockid, this.ActionTime);
            foreach (Panel p in flowPanel.Controls)
            {
                if (Convert.ToInt64(p.Tag) == Stockid)
                {
                    flowPanel.Controls.Remove(p);
                }
            }
            btnRefresh.PerformClick();
            //flowPanel_SizeChanged(null, null);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            long Stockid = (long)((Button)sender).Tag;
            //StockDetailViewer v = new StockDetailViewer(Stockid);
            //v.Show();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            pageNo++;
            LoadStocks();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            pageNo--;
            LoadStocks();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            isDateChanged = false;
            unitOfWork = new UnitOfWork();
            LoadStocks();
        }

        private void btnAddNewStock_Click(object sender, EventArgs e)
        {
            //if (!SharedFunctions.IsActionallowed("Add Stock") && !SharedVariables.LoggedInUser.UserRoles.Any(r=>r.Description.Equals("Super Admin")))
            //{
            //    MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}
            Form f = SharedFunctions.OpenForm(new btn(), this.MdiParent);
            //f.FormClosed += new FormClosedEventHandler(frmAddNewStock_Closed);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadStocks();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            pageNo = racksList.PageCount;
            LoadStocks();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            isDateChanged = true;
            pageNo = 1;
            LoadStocks();
        }

        private void flowPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            Panel AddedPanel = (Panel)e.Control;
            AddedPanel.Width = flowPanel.Width - 23;
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmManageStockShortCuts f = new ShortCutDialogs.frmManageStockShortCuts();
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
                    if (_pageNo <= 0 || _pageNo > racksList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.pageNo = _pageNo;
                    LoadStocks();
                }
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnManageRacks_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmManageRacks(), this.MdiParent);
        }
    }
}