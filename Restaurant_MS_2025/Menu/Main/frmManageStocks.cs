

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmManageStocks : Form
    {
        UnitOfWork unitOfWork;
        int pageNo = 1;
        IPagedList<StockVM> StocksList;
        bool isDateChanged = false;
        DateTime ActionTime;
        public frmManageStocks()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }
        private void frmManageStocks_Load(object sender, EventArgs e)
        {
            this.ActionTime = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
            dtpFrom.Value = dtpTo.Value = this.ActionTime;
            this.WindowState = FormWindowState.Maximized;
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnAddNewStock, btnPrint, btnExcel });
            LoadSuppliers();
            LoadStocks();
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {

            if (keyData == (Keys.Control | Keys.P))
            {
                btnPrint.PerformClick();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.T))
            {
                btnAddNewStock.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void LoadSuppliers()
        {
            cmbSuppliers.SelectedIndexChanged -= cmbSuppliers_SelectedIndexChanged;
            List<Supplier> suppliers = unitOfWork.SupplierRepository.GetAll().ToList();
            Supplier Select = new Supplier();
            Select.Name = "Select";
            Select.SupplierID = 0;
            suppliers.Insert(0, Select);
            cmbSuppliers.DataSource = suppliers;
            cmbSuppliers.ValueMember = "SupplierId";
            cmbSuppliers.DisplayMember = "Name";
            cmbSuppliers.SelectedIndexChanged += cmbSuppliers_SelectedIndexChanged;
        }

        private void LoadStocks()
        {
            flowPanel.Controls.Clear();
            if (cmbSuppliers.SelectedIndex > 0)
            {
                StocksList = unitOfWork.StockRepository.GetStocks(Convert.ToInt64(cmbSuppliers.SelectedValue), pageNo, SharedVariables.PageSize);
            }
            else if (txtSearchByName.Text.Trim() != "")
            {
                StocksList = unitOfWork.StockRepository.GetStocks(dtpFrom.Value, dtpTo.Value, txtSearchByName.Text.Trim().ToLower(), pageNo, SharedVariables.PageSize);
            }
            else if (isDateChanged)
            {
                //StocksList = unitOfWork.StockRepository.GetStocks(pageNo, SharedVariables.PageSize);
                StocksList = unitOfWork.StockRepository.GetStocks(dtpFrom.Value, dtpTo.Value, pageNo, SharedVariables.PageSize);
            }
            else
            {
                StocksList = unitOfWork.StockRepository.GetStocks(pageNo, SharedVariables.PageSize);
            }
            //int ActiveItemsCount = 0;
            foreach (StockVM s in StocksList.Items)
            {
                //ActiveItemsCount = s.StockItems.Where(i => i.IsActive && i.Quantity > 0).Count();
                //if (ActiveItemsCount > 0)
                //{
                this.flowPanel.SuspendLayout();
                BuildPanel(s);
                this.flowPanel.ResumeLayout(true);
                //}
            }
            btnLastPage.Enabled = btnNext.Enabled = StocksList.HasNextPage ? true : false;
            btnFirstPage.Enabled = btnPrevious.Enabled = StocksList.HasPreviousPage ? true : false;
            SharedFunctions.ShowPageNo(lblPageNo, pageNo, StocksList.PageCount);
        }

        private void BuildPanel(StockVM s)
        {
            int ConsumptionsCount = 0;
            foreach (StockItemVM si in s.StockItems)
            {
                if (si.StockConsumed > 0)
                {
                    ConsumptionsCount += 1;
                }
            }
            Panel pnlHeader = new Panel();

            // 
            // lblSupplier
            // 
            Label lblSupplier = new Label();
            lblSupplier.AutoSize = true;
            lblSupplier.Location = new System.Drawing.Point(14, 15);
            lblSupplier.Name = "lblSupplier";
            lblSupplier.Size = new System.Drawing.Size(35, 13);
            lblSupplier.TabIndex = 1;
            lblSupplier.Text = s.Supplier == null ? "N/A" : s.Supplier.Name + " => " + Math.Round(s.DocumentNo, 0).ToString();
            // 
            // lblDate
            // 
            Label lblDate = new Label();
            lblDate.AutoSize = true;
            lblDate.Location = new System.Drawing.Point(523, 15);
            lblDate.Name = "lblDate";
            lblDate.Size = new System.Drawing.Size(35, 13);
            lblDate.TabIndex = 1;
            lblDate.Text = s.CreatedAt.ToString();

            // 
            // lblAddedBy
            // 
            Label lblAddedBy = new Label();
            lblAddedBy.AutoSize = true;
            lblAddedBy.Location = new System.Drawing.Point(1205 - 500, 15);
            lblAddedBy.Name = "lblAddedBy";
            lblAddedBy.Size = new System.Drawing.Size(35, 13);
            lblAddedBy.TabIndex = 1;
            lblAddedBy.Text = "Added By: " + s.UserName;

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
            btnPrint.Tag = s.StockId;
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
            btnDelete.Tag = s.StockId;

            if (ConsumptionsCount > 0 || s.IsAutoInsertedStock)
            {
                btnDelete.Enabled = false;
            }
            else
            {
                btnDelete.Enabled = true;
            }
            //btnDelete.Enabled = ConsumptionsCount > 0 ? false : true;
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
            btnEdit.Tag = s.StockId;
            //if (ConsumptionsCount == s.StockItems.Count || s.IsAutoInsertedStock)
            //{
            //    btnEdit.Enabled = false;
            //}
            //else
            //{
            //    btnEdit.Enabled = true;
            //}

            //btnEdit.Enabled = ConsumptionsCount == s.StockItems.Count ? false : true;
            btnEdit.Click += new EventHandler(btnEdit_Click);

            pnlHeader.BackColor = Color.FromArgb(241, 241, 241);
            pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            pnlHeader.Location = new System.Drawing.Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new System.Drawing.Size(1258, 39);
            pnlHeader.TabIndex = 0;
            pnlHeader.Controls.Add(btnEdit);
            pnlHeader.Controls.Add(btnDelete);
            pnlHeader.Controls.Add(btnPrint);
            pnlHeader.Controls.Add(lblDate);
            pnlHeader.Controls.Add(lblAddedBy);
            pnlHeader.Controls.Add(lblSupplier);


            lblDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top)));
            btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // grdStockItems
            // 
            DataGridView grdStockItem = new DataGridView();
            GenerateDatagridView(grdStockItem);
            SharedFunctions.SetGridStyle(grdStockItem);
            //grdStockItem.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //grdStockItem.ColumnHeadersHeight = 35;
            //grdStockItem.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            //grdStockItem.EnableHeadersVisualStyles = false;
            //grdStockItem.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            //grdStockItem.RowTemplate.Height = 35;
            //grdStockItem.RowHeadersVisible = false;
            //grdStockItem.BackgroundColor = Color.White;
            grdStockItem.Dock = System.Windows.Forms.DockStyle.Fill;
            grdStockItem.Location = new System.Drawing.Point(0, 39);
            grdStockItem.Name = "grdStockItems";
            grdStockItem.Size = new System.Drawing.Size(1258, 201);
            grdStockItem.TabIndex = 1;
            grdStockItem.AllowUserToAddRows = false;
            grdStockItem.AllowUserToDeleteRows = false;
            //grdStockItem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            foreach (StockItemVM i in s.StockItems)
            {
                if (i.IsActive && i.Quantity > 0)
                {
                    i.Quantity = i.Unit == 0 ? i.Quantity / i.ConversionUnit : i.Quantity;
                    i.BonusQuantity = i.Unit == 0 ? i.BonusQuantity / i.ConversionUnit : i.BonusQuantity;
                    i.UnitCost = i.Unit == 0 ? i.UnitCost * i.Item.ConversionUnit : i.UnitCost;
                    i.RetailPrice = i.Unit == 0 ? i.RetailPrice * i.Item.ConversionUnit : i.RetailPrice;
                    grdStockItem.Rows.Add(
                        i.Item.ItemId,
                        i.Item.ItemName,
                        i.UnitString,
                        i.Item.ConversionUnit,
                        i.Quantity,
                        i.BonusQuantity,
                        i.UnitCost,
                        i.TotalCost,
                        i.RetailPrice,
                        i.Batch.BatchName,
                        i.Discount,
                        i.SalesTax,
                        i.NetValue,
                        i.CreatedAt
                        );
                }
            }
            // 
            // pnlDetail
            // 
            Panel pnlDetail = new Panel();
            pnlDetail.Tag = s.StockId;

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
            DataGridViewTextBoxColumn colUnit;
            DataGridViewTextBoxColumn colConvUnit;
            DataGridViewTextBoxColumn colQuantity;
            DataGridViewTextBoxColumn colBonusQuantity;
            DataGridViewTextBoxColumn colUnitCost;
            DataGridViewTextBoxColumn colToalCost;
            DataGridViewTextBoxColumn colRetailPrice;
            DataGridViewTextBoxColumn colBatch;
            DataGridViewTextBoxColumn colDiscount;
            DataGridViewTextBoxColumn colSalesTax;
            DataGridViewTextBoxColumn colNetValue;
            DataGridViewTextBoxColumn colCreatedAt;

            colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colConvUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBonusQuantity = new DataGridViewTextBoxColumn();
            colUnitCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colToalCost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colRetailPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDiscount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colSalesTax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colNetValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colCreatedAt = new System.Windows.Forms.DataGridViewTextBoxColumn();

            SourceGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            colItemId,
            colItemName,
            colUnit,
            colConvUnit,
            colQuantity,
            colBonusQuantity,
            colUnitCost,
            colToalCost,
            colRetailPrice,
            colBatch,
            colDiscount,
            colSalesTax,
            colNetValue,
            colCreatedAt});
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
            colUnit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colUnit.HeaderText = "Unit";
            colUnit.Width = 70;
            colUnit.Name = "colUnit";
            colUnit.ReadOnly = true;

            // 
            // colConvUnit
            // 
            colConvUnit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colConvUnit.MinimumWidth = 40;
            colConvUnit.HeaderText = "Conv.Unit";
            colConvUnit.Name = "colConvUnit";
            colConvUnit.ReadOnly = true;
            // 
            // colQuantity
            // 
            //colQuantity.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            colQuantity.Width = 60;
            colQuantity.MinimumWidth = 40;
            colQuantity.HeaderText = "Quantity";
            colQuantity.Name = "colQuantity";
            colQuantity.ReadOnly = true;
            // 
            // colBonusQuantity
            //
            colBonusQuantity.Width = 60;
            colBonusQuantity.MinimumWidth = 40;
            colBonusQuantity.HeaderText = "Bonus Quantity";
            colBonusQuantity.Name = "colBonusQuantity";
            colBonusQuantity.ReadOnly = true;
            // 
            // colUnitCost
            // 
            colUnitCost.Width = 70;
            colUnitCost.MinimumWidth = 40;
            colUnitCost.HeaderText = "UnitCost";
            colUnitCost.Name = "colUnitCost";
            colUnitCost.ReadOnly = true;
            // 
            // colToalCost
            // 
            colToalCost.Width = 70;
            colToalCost.MinimumWidth = 40;
            colToalCost.HeaderText = "Total Cost";
            colToalCost.Name = "colToalCost";
            colToalCost.ReadOnly = true;
            // 
            // colRetailPrice
            // 
            colRetailPrice.Width = 70;
            colRetailPrice.MinimumWidth = 40;
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
            colDiscount.Width = 70;
            colDiscount.MinimumWidth = 40;
            colDiscount.HeaderText = "Discount";
            colDiscount.Name = "colDiscount";
            colDiscount.ReadOnly = true;
            // 
            // colSalesTax
            // 
            colSalesTax.Width = 70;
            colSalesTax.MinimumWidth = 40;
            colSalesTax.HeaderText = "Sales Tax";
            colSalesTax.Name = "colSalesTax";
            colSalesTax.ReadOnly = true;
            // 
            // colNetValue
            // 
            colNetValue.Width = 70;
            colNetValue.MinimumWidth = 40;
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
            Form f = SharedFunctions.OpenForm(new btn(Stockid, 0), this.MdiParent);
            //frmAddStock f = new frmAddStock(Stockid, 0);
            //f.MdiParent = this.MdiParent;
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
            this.ActionTime = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
            dtpFrom.Value = dtpTo.Value = this.ActionTime;
            pageNo = 1;
            isDateChanged = false;
            unitOfWork = new UnitOfWork();
            LoadStocks();
            LoadSuppliers();
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

        private void frmAddNewStock_Closed(object sender, FormClosedEventArgs e)
        {
            //cmbRangeFilter.SelectedIndex = 0;
            txtSearchByName.Text = "";
            cmbSuppliers.SelectedIndex = 0;
            LoadStocks();
        }
        private void btnPrint_Click_1(object sender, EventArgs e)
        {
            List<StocksReportVM> StocksList = this.GetReportData();
            //StocksViewer v = new StocksViewer(this.dtpFrom.Value, this.dtpTo.Value, StocksList);
            //v.Show();
        }

        private List<StocksReportVM> GetReportData()
        {
            try
            {
                if (cmbSuppliers.SelectedIndex > 0)
                {
                    return unitOfWork.StockRepository.GetStocks_Print(dtpFrom.Value, dtpTo.Value, Convert.ToInt64(cmbSuppliers.SelectedValue));
                }
                else if (txtSearchByName.Text.Trim() != "")
                {
                    return unitOfWork.StockRepository.GetStocks_Print(dtpFrom.Value, dtpTo.Value, txtSearchByName.Text.Trim().ToLower());
                }
                else if (isDateChanged)
                {
                    return unitOfWork.StockRepository.GetStocks_Print(dtpFrom.Value, dtpTo.Value, true);
                }
                else
                {
                    return unitOfWork.StockRepository.GetStocks_Print(dtpFrom.Value, dtpTo.Value, false);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An error occurred while fetching report data, Please try again.", ex.Message, "Failed.");
                return null;
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            this.ActionTime = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
            try
            {
                //List<Stock> stocks = unitOfWork.StockRepository.GetActiveStocksParentOnly();
                //foreach (var s in stocks)
                //{
                //    s.SupplierName = s.Supplier == null ? "N/A" : s.Supplier.Name;
                //}
                //List<StockItemVM> StockItems = unitOfWork.StockRepository.GetActiveStockItems();
                //foreach (var i in StockItems)
                //{
                //    i.StockId = i.StockId;
                //    i.ItemName = i.ItemName;
                //}
                //StocksRpt rpt = new StocksRpt();
                //rpt.Database.Tables[0].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                //rpt.Database.Tables[1].SetDataSource(this.GetReportData());
                //rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                //rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                //rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                //rpt.SetParameterValue("FromDate", this.ActionTime);
                //rpt.SetParameterValue("ToDate", this.ActionTime);
                //if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                //    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                //    MessageBox.Show("Items Date Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        private void cmbRangeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "today")
            //{
            //    dtpFrom.Value = DateTime.Today;
            //    dtpTo.Value = DateTime.Today;
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "yesterday")
            //{
            //    dtpFrom.Value = DateTime.Today.AddDays(-1);
            //    dtpTo.Value = DateTime.Today.AddDays(-1);
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "last 7 days")
            //{
            //    dtpFrom.Value = DateTime.Today.AddDays(-6);
            //    dtpTo.Value = DateTime.Today;
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "last 30 days")
            //{
            //    dtpFrom.Value = DateTime.Today.AddDays(-29);
            //    dtpTo.Value = DateTime.Today;
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "this month")
            //{
            //    dtpFrom.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            //    dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "last month")
            //{
            //    DateTime LastMonth = DateTime.Today.AddMonths(-1);
            //    dtpFrom.Value = new DateTime(LastMonth.Year, LastMonth.Month, 1);
            //    dtpTo.Value = dtpFrom.Value.AddMonths(1).AddDays(-1);
            //    return;
            //}
            //if (cmbRangeFilter.GetItemText(cmbRangeFilter.SelectedItem).ToString().ToLower() == "custom range")
            //{
            //    dtpFrom.Value = DateTime.Today;
            //    dtpTo.Value = DateTime.Today;
            //    return;
            //}
        }

        private void btnApplyFilter_Click(object sender, EventArgs e)
        {

        }

        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSuppliers.SelectedIndex >= 0)
            {
                //cmbRangeFilter.SelectedIndex = 0;
                txtSearchByName.Text = "";
                pageNo = 1;
                LoadStocks();
            }
            flowPanel_SizeChanged(null, null);
        }

        private void txtSearchByName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cmbSuppliers.SelectedIndex = 0;
                pageNo = 1;
                LoadStocks();
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            isDateChanged = true;
            pageNo = 1;
            LoadStocks();
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
            pageNo = StocksList.PageCount;
            LoadStocks();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            isDateChanged = true;
            pageNo = 1;
            LoadStocks();
        }

        private void flowPanel_Paint(object sender, PaintEventArgs e)
        {

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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

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
                    if (_pageNo <= 0 || _pageNo > StocksList.PageCount)
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

        private void txtSearchByName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}