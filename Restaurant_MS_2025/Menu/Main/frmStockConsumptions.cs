

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmStockConsumptions : Form
    {
        enum FilterType { Default = 1, Date = 2, Item = 3, User = 4 };
        FilterType filter = FilterType.Default;
        UnitOfWork unitOfWork;
        int pageNo = 1;
        bool IsFilterApplied = false;
        IPagedList<StockConsumptionVM> ConsumptionsList;
        int SelectedItemId = 0;
        public frmStockConsumptions()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }
        private void toggleEvents(bool Register)
        {
            if (Register)
            {
                dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
                dtpTo.ValueChanged -= dtpTo_ValueChanged;
            }
            else
            {
                dtpFrom.ValueChanged += dtpFrom_ValueChanged;
                dtpTo.ValueChanged += dtpTo_ValueChanged;
            }
        }
        private void frmManageStocks_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnAddNewConsumption });
            LoadConsumptions();
            LoadUsers();            
            flowPanel_SizeChanged(null, null);
        }
        private void LoadUsers()
        {
            try
            {
                List<UserSelectVM> us = new List<UserSelectVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    us = unitOfWork.UserRepository.GetAllActiveUsers();
                }
                UserSelectVM v = new UserSelectVM
                {
                    UserId = 0,
                    UserName = "Select All"
                };
                us.Insert(0, v);
                cmbUsers.SelectedIndexChanged -= cmbUsers_SelectedIndexChanged;
                cmbUsers.DataSource = us;
                cmbUsers.DisplayMember = "UserName";
                cmbUsers.ValueMember = "UserId";
                cmbUsers.SelectedIndexChanged += cmbUsers_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading users data. Please try again.", "User Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void uC_SearchItems1_OnItemSelectionChanged()
        {
            this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;
            if (this.SelectedItemId > 0)
            {
                filter = FilterType.Item;
                LoadConsumptions();
            }
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.F5))
            {
                btnRefresh.PerformClick();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.N))
            {
                btnAddNewConsumption.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void LoadConsumptions()
        {
            flowPanel.Controls.Clear();
            using (unitOfWork = new UnitOfWork())
            {
                if (filter == FilterType.Date)
                {
                    ConsumptionsList = unitOfWork.StockConsumptionRepository.GetStockConsumptions(dtpFrom.Value, dtpTo.Value, pageNo, SharedVariables.PageSize);
                }
                else if (filter == FilterType.Item)
                {
                    ConsumptionsList = unitOfWork.StockConsumptionRepository.GetStockConsumptions(this.SelectedItemId, this.pageNo, SharedVariables.PageSize);
                }
                else if (filter == FilterType.User)
                {
                    ConsumptionsList = unitOfWork.StockConsumptionRepository.GetStockConsumptions(dtpFrom.Value, dtpTo.Value, Convert.ToInt32(cmbUsers.SelectedValue), pageNo, SharedVariables.PageSize);
                }
                else
                {
                    ConsumptionsList = unitOfWork.StockConsumptionRepository.GetStockConsumptions(pageNo, SharedVariables.PageSize);
                }
            }
            foreach (StockConsumptionVM a in ConsumptionsList.Items)
            {
                BuildPanel(a);
            }
            btnLastPage.Enabled = btnNext.Enabled = ConsumptionsList.HasNextPage ? true : false;
            btnFirstPage.Enabled = btnPrevious.Enabled = ConsumptionsList.HasPreviousPage ? true : false;
            SharedFunctions.ShowPageNo(lblPageNo, pageNo, ConsumptionsList.PageCount);
        }

        private void BuildPanel(StockConsumptionVM a)
        {
            Panel pnlHeader = new Panel();

            // 
            // lblSupplier
            // 
            //Label lblSupplier = new Label();
            //lblSupplier.AutoSize = true;
            //lblSupplier.Location = new System.Drawing.Point(14, 15);
            //lblSupplier.Name = "lblSupplier";
            //lblSupplier.Size = new System.Drawing.Size(35, 13);
            //lblSupplier.TabIndex = 1;
            //lblSupplier.Text = s.Supplier == null ? "N/A" : s.Supplier.Name;
            // 
            // lblDate
            // 
            Label lblDate = new Label();
            lblDate.AutoSize = true;
            lblDate.Location = new System.Drawing.Point(85, 15);
            lblDate.Name = "lblDate";
            lblDate.Size = new System.Drawing.Size(35, 13);
            lblDate.TabIndex = 1;
            lblDate.Text = a.CreatedAt.ToString();
            // 
            // lblAddedBy
            // 
            Label lblAddedBy = new Label();
            lblAddedBy.AutoSize = true;
            lblAddedBy.Location = new System.Drawing.Point(1205 - 500, 15);
            lblAddedBy.Name = "lblAddedBy";
            lblAddedBy.Size = new System.Drawing.Size(35, 13);
            lblAddedBy.TabIndex = 1;
            lblAddedBy.Text = "Added By: " + a.UserName;
            // 
            // btnPrint
            // 
            Button btnPrint = new Button();
            btnPrint.Location = new System.Drawing.Point(1205, 0); // 1205
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new System.Drawing.Size(51, 39);
            btnPrint.TabIndex = 1;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Tag = a.StockConsumptionId;
            btnPrint.Click += new EventHandler(btnPrint_Click);
            // 
            // btnDelete
            // 
            Button btnDelete = new Button();
            btnDelete.Location = new System.Drawing.Point(1152, 0); // 1205
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(51, 39);
            btnDelete.TabIndex = 1;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Tag = a.StockConsumptionId;
            btnDelete.Click += new EventHandler(btnDelete_Click);

            // 
            // btnEdit
            //
            Button btnEdit = new Button();
            btnEdit.Location = new System.Drawing.Point(1095, 0); // 1152
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(51, 39);
            btnEdit.TabIndex = 1;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Tag = a.StockConsumptionId;
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
            //pnlHeader.Controls.Add(lblSupplier);
            lblDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top)));
            btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            // 
            // grdStockItems
            // 
            DataGridView grdItem = new DataGridView();
            GenerateDatagridView(grdItem);
            SharedFunctions.SetGridStyle(grdItem);
            //grdItem.EnableHeadersVisualStyles = false;
            //grdItem.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            //grdItem.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            grdItem.ColumnHeadersHeight = 35;
            //grdItem.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            //grdItem.RowTemplate.Height = 35;
            //grdItem.RowHeadersVisible = false;
            //grdItem.BackgroundColor = Color.White;
            grdItem.Dock = System.Windows.Forms.DockStyle.Fill;
            grdItem.Location = new System.Drawing.Point(0, 39);
            grdItem.Name = "grdStockItems";
            grdItem.Size = new System.Drawing.Size(1258, 201);
            grdItem.TabIndex = 1;
            grdItem.AllowUserToAddRows = false;
            grdItem.AllowUserToDeleteRows = false;
            //grdItem.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            foreach (StockConsumptionItemVM i in a.StockConsumptionsList)
            {
                //colItemId;
                //colItemName;
                //colConsumedQuantity
                //colBatchId;
                //colBatch;
                //colConsumptionType;
                //colUser;
                //colComment;
                //colTotalCost;
                //colCreatedAt;
                if (i.IsActive)
                {
                    if (i.ConsumptionTypeString.ToLower() == "sales")
                    {
                        btnDelete.Enabled = false;
                        btnEdit.Enabled = false;
                    }
                    grdItem.Rows.Add(i.ItemId, i.ItemName, i.BatchName, i.UnitString, i.ConversionUnit, i.Quantity, i.ConsumptionTypeString, i.User.UserName, i.Comment, i.TotalCost, i.CreatedAt);
                }
            }

            // 
            // pnlDetail
            // 
            Panel pnlDetail = new Panel();

            pnlDetail.Controls.Add(grdItem);
            pnlDetail.Controls.Add(pnlHeader);
            pnlDetail.Location = new System.Drawing.Point(3, 3);
            pnlDetail.Name = "pnlDetail";
            pnlDetail.Size = new System.Drawing.Size(1258, 240);
            pnlDetail.TabIndex = 0;
            flowPanel.Controls.Add(pnlDetail);
            pnlDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
           | System.Windows.Forms.AnchorStyles.Right)));
        }
        private void GenerateDatagridView(DataGridView SourceGrid)
        {
            DataGridViewTextBoxColumn colItemId;
            DataGridViewTextBoxColumn colItemName;
            DataGridViewTextBoxColumn colConsumedQuantity;
            //            DataGridViewTextBoxColumn colBatchId;
            DataGridViewTextBoxColumn colBatch;

            DataGridViewTextBoxColumn colUnit;
            DataGridViewTextBoxColumn colConvUnit;

            DataGridViewTextBoxColumn colConsumptionType;
            DataGridViewTextBoxColumn colUser;
            DataGridViewTextBoxColumn colComment;
            DataGridViewTextBoxColumn colTotalCost;
            DataGridViewTextBoxColumn colCreatedAt;

            colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colConsumedQuantity = new DataGridViewTextBoxColumn();
            //colBatchId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBatch = new System.Windows.Forms.DataGridViewTextBoxColumn();

            colUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colConvUnit = new System.Windows.Forms.DataGridViewTextBoxColumn();

            colConsumptionType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colComment = new DataGridViewTextBoxColumn();
            colTotalCost = new DataGridViewTextBoxColumn();
            colCreatedAt = new DataGridViewTextBoxColumn();

            SourceGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            colItemId,
            colItemName,
            //colBatchId,
            colBatch,
            colUnit,
            colConvUnit,
            colConsumedQuantity,
            colConsumptionType,
            colUser,
            colComment,
            colTotalCost,
            colCreatedAt
            });
            grdITems.Location = new System.Drawing.Point(15, 28);
            grdITems.Name = "grdITems";
            grdITems.Size = new System.Drawing.Size(1258, 150);
            grdITems.TabIndex = 12;
            grdITems.Visible = false;
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
            // colQuantity
            // 
            colConsumedQuantity.HeaderText = "Consumed Quantity";
            colConsumedQuantity.Name = "colConsumedQuantity";
            colConsumedQuantity.ReadOnly = true;
            // 
            // colBatchId
            // 
            //colBatchId.HeaderText = "Batch Id";
            //colBatchId.Name = "colBatchId";
            //colBatchId.ReadOnly = true;
            // 
            // colBatch
            // 
            colBatch.HeaderText = "Batch";
            colBatch.Name = "colBatch";
            colBatch.ReadOnly = true;
            // 
            // colUnit
            // 
            colUnit.HeaderText = "Unit";
            colUnit.Name = "colUnit";
            colUnit.ReadOnly = true;
            // 
            // colConvUnit
            // 
            colConvUnit.HeaderText = "Conv. Unit";
            colConvUnit.Name = "colConvUnit";
            colConvUnit.ReadOnly = true;

            //
            // colReason
            //
            colConsumptionType.HeaderText = "Consumption type";
            colConsumptionType.Name = "colConsumptionType";
            colConsumptionType.ReadOnly = true;
            //
            // colAdjustmentDate
            //
            colUser.HeaderText = "Added By";
            colUser.Name = "colUser";
            colUser.ReadOnly = true;
            //
            // colComment
            //
            colComment.HeaderText = "Comment";
            colComment.Name = "colComment";
            colComment.ReadOnly = true;
            //
            // colTotalCost
            //
            colTotalCost.HeaderText = "Total Cost";
            colTotalCost.Name = "colTotalCost";
            colTotalCost.ReadOnly = true;
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
            long ConsumptionId = (long)((Button)sender).Tag;
            Form f = SharedFunctions.OpenForm(new frmStockConsumption(ConsumptionId), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(frmStockAdjustment_Closed);
        }

        private void frmStockAdjustment_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult rs = MessageBox.Show("Are You Sure, You Want to Delete This Adjustment", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                if (rs == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }
                long ConsumptionId = (long)((Button)sender).Tag;
                using (unitOfWork = new UnitOfWork())
                {
                    StockConsumption sc = unitOfWork.StockConsumptionRepository.GetStockConumptionById_(ConsumptionId);
                    sc.IsActive = false;
                    sc.IsSynced = false;
                    sc.IsUpdate = true;
                    sc.UpdatedAt = DateTime.Now;
                    sc.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    foreach (StockConsumptionItem i in sc.StockConsumptionItems)
                    {
                        i.IsActive = false;
                        i.UpdatedAt = DateTime.Now;
                        i.IsSynced = false;
                        i.IsUpdate = true;
                    }
                    unitOfWork.StockConsumptionRepository.Update(sc);
                    unitOfWork.Save();
                }
                MessageBox.Show("Consumption Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnRefresh.PerformClick();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            long ConsumptionId = (long)((Button)sender).Tag;
            //Reports.StockConsumptionViewer v = new Reports.StockConsumptionViewer(ConsumptionId);
            //v.Show();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            pageNo++;
            LoadConsumptions();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            pageNo--;
            LoadConsumptions();
        }
        private void Clear()
        {
            dtpTo.Value = dtpFrom.Value = DateTime.Now;
            pageNo = 1;
        }

        private void btnAddNewAdjustment_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmStockConsumption(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                toggleEvents(false);
                unitOfWork = new UnitOfWork();
                Clear();
                filter = FilterType.Default;
                LoadConsumptions();
                flowPanel_SizeChanged(null, null);
                toggleEvents(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading data, please try again after reloading form.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadConsumptions();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            pageNo = ConsumptionsList.PageCount;
            LoadConsumptions();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            pageNo = 1;
            LoadConsumptions();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            pageNo = 1;
            LoadConsumptions();
        }

        private void flowPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            Panel AddedPanel = (Panel)(e.Control);
            AddedPanel.Width = flowPanel.Width - 23;
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
                    if (_pageNo <= 0 || _pageNo > ConsumptionsList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.pageNo = _pageNo;
                    LoadConsumptions();
                }
            }
        }

        private void cmbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            uC_SearchItems1.SetText = "";
            if (cmbUsers.SelectedIndex == 0)
            {
                filter = FilterType.Default;
            }
            else
            {
                filter = FilterType.User;
            }
            LoadConsumptions();
        }
    }
}