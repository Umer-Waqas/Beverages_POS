
namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmStockAdjustments : Form
    {
        enum FilterType { Default = 1, Date = 2, Item = 3 };
        FilterType filter = FilterType.Default;
        UnitOfWork unitOfWork;
        int pageNo = 1;
        IPagedList<Adjustment> AdjustmentList;

        int SelectedItemId = 0;
        public frmStockAdjustments()
        {
            InitializeComponent();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        }

        private void frmManageStocks_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnAddNewAdjustment });
            LoadAdjustments();
            flowPanel_SizeChanged(null, null);
        }
        private void uC_SearchItems1_OnItemSelectionChanged()
        {
            this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;
            if (this.SelectedItemId > 0)
            {
                filter = FilterType.Item;
                LoadAdjustments();
            }
        }
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.F5))
            {
                btnRefresh.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.Shift | Keys.J))
            {
                btnAddNewAdjustment.PerformClick();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void LoadAdjustments()
        {
            flowPanel.Controls.Clear();
            using (unitOfWork = new UnitOfWork())
            {
                if (filter == FilterType.Date)
                {
                    AdjustmentList = unitOfWork.AdjustmentRepository.GetAdjustments(dtpFrom.Value, dtpTo.Value, pageNo, SharedVariables.PageSize, Enums.AdjustmentType.Adjustment);
                }
                else if (filter == FilterType.Item)
                {
                    AdjustmentList = unitOfWork.AdjustmentRepository.GetAdjustments(this.SelectedItemId, this.pageNo, SharedVariables.PageSize, Enums.AdjustmentType.Adjustment);
                }
                else if (filter == FilterType.Default)
                {
                    AdjustmentList = unitOfWork.AdjustmentRepository.GetAdjustments(pageNo, SharedVariables.PageSize, Enums.AdjustmentType.Adjustment);
                }
            }
            foreach (Adjustment a in AdjustmentList.Items)
            {
                BuildPanel(a);
            }
            btnNext.Enabled = AdjustmentList.HasNextPage ? true : false;
            btnPrevious.Enabled = AdjustmentList.HasPreviousPage ? true : false;
            SharedFunctions.ShowPageNo(lblPageNo, pageNo, AdjustmentList.PageCount);
            this.SelectedItemId = 0;
        }
        private void LoadAdjustments(DateTime FromDate, DateTime DateTo)
        {
            flowPanel.Controls.Clear();
            using (unitOfWork = new UnitOfWork())
            {
                AdjustmentList = unitOfWork.AdjustmentRepository.GetAdjustments(pageNo, SharedVariables.PageSize, Enums.AdjustmentType.Adjustment);
            }
            flowPanel.SuspendLayout();
            foreach (Adjustment a in AdjustmentList.Items)
            {
                BuildPanel(a);
            }
            flowPanel.ResumeLayout();
            btnNext.Enabled = AdjustmentList.HasNextPage ? true : false;
            btnPrevious.Enabled = AdjustmentList.HasPreviousPage ? true : false;
            SharedFunctions.ShowPageNo(lblPageNo, pageNo, AdjustmentList.PageCount);
        }

        private void BuildPanel(Adjustment a)
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
            lblDate.Location = new System.Drawing.Point(9, 15);
            lblDate.Name = "lblDate";
            lblDate.Size = new System.Drawing.Size(35, 13);
            lblDate.TabIndex = 1;
            lblDate.Text = a.CreatedAt.ToString();
            // 
            // 
            // lblAddedBy
            // 
            Label lblAddedBy = new Label();
            lblAddedBy.AutoSize = true;
            lblAddedBy.Location = new System.Drawing.Point(1205 - 500, 15);
            lblAddedBy.Name = "lblAddedBy";
            lblAddedBy.Size = new System.Drawing.Size(35, 13);
            lblAddedBy.TabIndex = 1;
            lblAddedBy.Text =  "Added By: " + a.User.UserName;
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
            btnPrint.Tag = a.AdjustmentId;
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
            btnDelete.Tag = a.AdjustmentId;
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
            btnEdit.Tag = a.AdjustmentId;
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
            lblDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
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
            //grdItem.ColumnHeadersHeight = 35;
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
            foreach (AdjustmentItem i in a.AdjustmentItems)
            {
                if (i.IsActive)
                {
                    i.UnitString = i.Unit == 0 ? i.Item.Unit : "Units";
                    i.Quantity = i.Unit == 0 ? i.Quantity / i.Item.ConversionUnit : i.Quantity;
                    grdItem.Rows.Add(i.Item.ItemId, i.Item.ItemName, i.Batch.BatchName, i.UnitString, i.Quantity, i.Reason, i.CreatedAt);
                }
            }
            SharedFunctions.SetGridStyle(grdItem);
            // 
            // pnlDetail
            // 
            Panel pnlDetail = new Panel();
            pnlDetail.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            pnlDetail.Controls.Add(grdItem);
            pnlDetail.Controls.Add(pnlHeader);
            pnlDetail.Location = new System.Drawing.Point(3, 3);
            pnlDetail.Name = "pnlDetail";
            pnlDetail.Size = new System.Drawing.Size(1258, 240);
            pnlDetail.TabIndex = 0;

            flowPanel.Controls.Add(pnlDetail);
        }
        private void GenerateDatagridView(DataGridView SourceGrid)
        {
            DataGridViewTextBoxColumn colItemId;
            DataGridViewTextBoxColumn colItemName;
            DataGridViewTextBoxColumn colBatch;
            DataGridViewTextBoxColumn colUnit;
            DataGridViewTextBoxColumn colAdjustment;
            DataGridViewTextBoxColumn colReason;
            DataGridViewTextBoxColumn colDate;

            colItemId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colBatch = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colUnit = new DataGridViewTextBoxColumn();
            colAdjustment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colReason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();

            SourceGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            colItemId,
            colItemName,
            colBatch,
            colUnit,
            colAdjustment,
            colReason,
            colDate
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
            colAdjustment.HeaderText = "Adjustment";
            colAdjustment.Name = "colAdjustment";
            colAdjustment.ReadOnly = true;

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
            // colReason
            //
            colReason.HeaderText = "Reason";
            colReason.Name = "colReason";
            colReason.ReadOnly = true;
            //
            // colAdjustmentDate
            //
            colDate.HeaderText = "Adjustment Date";
            colDate.Name = "colDate";
            colDate.ReadOnly = true;
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
            long Adjustment = (long)((Button)sender).Tag;
            Form f = SharedFunctions.OpenForm(new frmStockAdjustment(Adjustment), this.MdiParent);
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
                long Adjustment = (long)((Button)sender).Tag;
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.AdjustmentRepository.SetInactive(Adjustment);
                }
                MessageBox.Show("Adjustment Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnRefresh.PerformClick();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            long Adjustment = (long)((Button)sender).Tag;
            //Reports.AdjustmentsViewer v = new Reports.AdjustmentsViewer(Adjustment);
            //v.Show();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            pageNo++;
            LoadAdjustments();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            pageNo--;
            LoadAdjustments();
        }
        private void Clear()
        {
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            dtpTo.ValueChanged -= dtpTo_ValueChanged;
            dtpTo.Value = dtpFrom.Value = DateTime.Now;
            filter = FilterType.Default;
            pageNo = 1;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnAddNewAdjustment_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmStockAdjustment(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void ToggleEvents(bool Register)
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
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            //ToggleEvents(false);
            Clear();
            LoadAdjustments();
            flowPanel_SizeChanged(null, null);
            //ToggleEvents(true);
        }

        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            LoadAdjustments();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            pageNo = AdjustmentList.PageCount;
            LoadAdjustments();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            pageNo = 1;
            LoadAdjustments();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            pageNo = 1;
            LoadAdjustments();
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmStockAdjustmentsShortCuts f = new ShortCutDialogs.frmStockAdjustmentsShortCuts();
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
                    if (_pageNo <= 0 || _pageNo > AdjustmentList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.pageNo = _pageNo;
                    LoadAdjustments();
                }
            }
        }

        private void flowPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}