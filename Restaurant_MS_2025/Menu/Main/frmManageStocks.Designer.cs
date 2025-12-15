namespace Restaurant_MS_UI.Menu.Main
{
    partial class frmManageStocks
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(frmManageStocks));
            flowPanel = new FlowLayoutPanel();
            panel3 = new Panel();
            dataGridView1 = new DataGridView();
            colItemId = new DataGridViewTextBoxColumn();
            colItemName = new DataGridViewTextBoxColumn();
            colQuantity = new DataGridViewTextBoxColumn();
            colUnitCost = new DataGridViewTextBoxColumn();
            colToalCost = new DataGridViewTextBoxColumn();
            colRetailPrice = new DataGridViewTextBoxColumn();
            colBatch = new DataGridViewTextBoxColumn();
            colDiscount = new DataGridViewTextBoxColumn();
            colSalesTax = new DataGridViewTextBoxColumn();
            colNetValue = new DataGridViewTextBoxColumn();
            colCreatedAt = new DataGridViewTextBoxColumn();
            btnRefresh = new Button();
            btnAddNewStock = new Button();
            btnPrint = new Button();
            btnExcel = new Button();
            dlgSaveExcel = new SaveFileDialog();
            lblTo = new Label();
            lblFrom = new Label();
            dtpTo = new DateTimePicker();
            dtpFrom = new DateTimePicker();
            txtSearchByName = new TextBox();
            cmbSuppliers = new ComboBox();
            label2 = new Label();
            label3 = new Label();
            pbInfo = new PictureBox();
            panel2 = new Panel();
            panel1 = new Panel();
            txtGotoPage = new TextBox();
            label6 = new Label();
            btnLastPage = new Button();
            btnFirstPage = new Button();
            btnNext = new Button();
            lblPageNo = new Label();
            btnPrevious = new Button();
            label5 = new Label();
            label4 = new Label();
            label1 = new Label();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn11 = new DataGridViewTextBoxColumn();
            flowPanel.SuspendLayout();
            panel3.SuspendLayout();
            ((ISupportInitialize)dataGridView1).BeginInit();
            ((ISupportInitialize)pbInfo).BeginInit();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // flowPanel
            // 
            flowPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flowPanel.AutoScroll = true;
            flowPanel.BackColor = Color.White;
            flowPanel.Controls.Add(panel3);
            flowPanel.Location = new Point(3, 28);
            flowPanel.Name = "flowPanel";
            flowPanel.Size = new Size(1497, 443);
            flowPanel.TabIndex = 9;
            flowPanel.SizeChanged += flowPanel_SizeChanged;
            flowPanel.ControlAdded += flowPanel_ControlAdded;
            flowPanel.Paint += flowPanel_Paint;
            // 
            // panel3
            // 
            panel3.Controls.Add(dataGridView1);
            panel3.Location = new Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(1264, 213);
            panel3.TabIndex = 13;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = Color.White;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersHeight = 35;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { colItemId, colItemName, colQuantity, colUnitCost, colToalCost, colRetailPrice, colBatch, colDiscount, colSalesTax, colNetValue, colCreatedAt });
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.Location = new Point(3, 51);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 35;
            dataGridView1.Size = new Size(1257, 162);
            dataGridView1.TabIndex = 12;
            dataGridView1.Visible = false;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // colItemId
            // 
            colItemId.HeaderText = "ItemId";
            colItemId.MinimumWidth = 6;
            colItemId.Name = "colItemId";
            colItemId.ReadOnly = true;
            colItemId.Width = 125;
            // 
            // colItemName
            // 
            colItemName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colItemName.HeaderText = "Item";
            colItemName.MinimumWidth = 6;
            colItemName.Name = "colItemName";
            colItemName.ReadOnly = true;
            // 
            // colQuantity
            // 
            colQuantity.HeaderText = "Quantity";
            colQuantity.MinimumWidth = 6;
            colQuantity.Name = "colQuantity";
            colQuantity.ReadOnly = true;
            colQuantity.Width = 125;
            // 
            // colUnitCost
            // 
            colUnitCost.HeaderText = "UnitCost";
            colUnitCost.MinimumWidth = 6;
            colUnitCost.Name = "colUnitCost";
            colUnitCost.ReadOnly = true;
            colUnitCost.Width = 125;
            // 
            // colToalCost
            // 
            colToalCost.HeaderText = "Total Cost";
            colToalCost.MinimumWidth = 6;
            colToalCost.Name = "colToalCost";
            colToalCost.ReadOnly = true;
            colToalCost.Width = 125;
            // 
            // colRetailPrice
            // 
            colRetailPrice.HeaderText = "Retail Price";
            colRetailPrice.MinimumWidth = 6;
            colRetailPrice.Name = "colRetailPrice";
            colRetailPrice.ReadOnly = true;
            colRetailPrice.Width = 125;
            // 
            // colBatch
            // 
            colBatch.HeaderText = "Batch";
            colBatch.MinimumWidth = 6;
            colBatch.Name = "colBatch";
            colBatch.ReadOnly = true;
            colBatch.Width = 125;
            // 
            // colDiscount
            // 
            colDiscount.HeaderText = "Discount";
            colDiscount.MinimumWidth = 6;
            colDiscount.Name = "colDiscount";
            colDiscount.ReadOnly = true;
            colDiscount.Width = 125;
            // 
            // colSalesTax
            // 
            colSalesTax.HeaderText = "Sales Tax";
            colSalesTax.MinimumWidth = 6;
            colSalesTax.Name = "colSalesTax";
            colSalesTax.ReadOnly = true;
            colSalesTax.Width = 125;
            // 
            // colNetValue
            // 
            colNetValue.HeaderText = "Net Value";
            colNetValue.MinimumWidth = 6;
            colNetValue.Name = "colNetValue";
            colNetValue.ReadOnly = true;
            colNetValue.Width = 125;
            // 
            // colCreatedAt
            // 
            colCreatedAt.HeaderText = "Create At";
            colCreatedAt.MinimumWidth = 6;
            colCreatedAt.Name = "colCreatedAt";
            colCreatedAt.ReadOnly = true;
            colCreatedAt.Width = 125;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnRefresh.Image = (Image)resources.GetObject("btnRefresh.Image");
            btnRefresh.Location = new Point(1376, 5);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(111, 42);
            btnRefresh.TabIndex = 8;
            btnRefresh.Text = "Refresh";
            btnRefresh.TextImageRelation = TextImageRelation.TextBeforeImage;
            btnRefresh.UseVisualStyleBackColor = true;
            btnRefresh.Click += btnRefresh_Click;
            // 
            // btnAddNewStock
            // 
            btnAddNewStock.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnAddNewStock.Location = new Point(1239, 5);
            btnAddNewStock.Name = "btnAddNewStock";
            btnAddNewStock.Size = new Size(131, 42);
            btnAddNewStock.TabIndex = 7;
            btnAddNewStock.Text = "Add New Stock";
            btnAddNewStock.UseVisualStyleBackColor = true;
            btnAddNewStock.Click += btnAddNewStock_Click;
            // 
            // btnPrint
            // 
            btnPrint.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnPrint.Location = new Point(1176, 5);
            btnPrint.Name = "btnPrint";
            btnPrint.Size = new Size(57, 42);
            btnPrint.TabIndex = 6;
            btnPrint.Text = "Print";
            btnPrint.UseVisualStyleBackColor = true;
            btnPrint.Visible = false;
            btnPrint.Click += btnPrint_Click_1;
            // 
            // btnExcel
            // 
            btnExcel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnExcel.Location = new Point(1113, 5);
            btnExcel.Name = "btnExcel";
            btnExcel.Size = new Size(57, 42);
            btnExcel.TabIndex = 5;
            btnExcel.Text = "Excel";
            btnExcel.UseVisualStyleBackColor = true;
            btnExcel.Visible = false;
            btnExcel.Click += btnExcel_Click;
            // 
            // dlgSaveExcel
            // 
            dlgSaveExcel.FileName = "StocksList";
            dlgSaveExcel.Filter = "Excel|*.xls";
            // 
            // lblTo
            // 
            lblTo.AutoSize = true;
            lblTo.Location = new Point(212, 60);
            lblTo.Name = "lblTo";
            lblTo.Size = new Size(23, 18);
            lblTo.TabIndex = 21;
            lblTo.Text = "To";
            // 
            // lblFrom
            // 
            lblFrom.AutoSize = true;
            lblFrom.Location = new Point(6, 60);
            lblFrom.Name = "lblFrom";
            lblFrom.Size = new Size(40, 18);
            lblFrom.TabIndex = 18;
            lblFrom.Text = "From";
            // 
            // dtpTo
            // 
            dtpTo.Location = new Point(215, 78);
            dtpTo.Name = "dtpTo";
            dtpTo.Size = new Size(200, 25);
            dtpTo.TabIndex = 2;
            dtpTo.ValueChanged += dtpTo_ValueChanged;
            // 
            // dtpFrom
            // 
            dtpFrom.Location = new Point(9, 78);
            dtpFrom.Name = "dtpFrom";
            dtpFrom.Size = new Size(200, 25);
            dtpFrom.TabIndex = 1;
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
            // 
            // txtSearchByName
            // 
            txtSearchByName.Location = new Point(428, 78);
            txtSearchByName.Name = "txtSearchByName";
            txtSearchByName.Size = new Size(200, 25);
            txtSearchByName.TabIndex = 3;
            txtSearchByName.TextChanged += txtSearchByName_TextChanged;
            txtSearchByName.KeyDown += txtSearchByName_KeyDown;
            // 
            // cmbSuppliers
            // 
            cmbSuppliers.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbSuppliers.FormattingEnabled = true;
            cmbSuppliers.Location = new Point(634, 76);
            cmbSuppliers.Name = "cmbSuppliers";
            cmbSuppliers.Size = new Size(200, 26);
            cmbSuppliers.TabIndex = 4;
            cmbSuppliers.SelectedIndexChanged += cmbSuppliers_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(428, 60);
            label2.Name = "label2";
            label2.Size = new Size(139, 18);
            label2.TabIndex = 18;
            label2.Text = "Search By Item Name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(634, 60);
            label3.Name = "label3";
            label3.Size = new Size(120, 18);
            label3.TabIndex = 18;
            label3.Text = "Search By Supplier";
            // 
            // pbInfo
            // 
            pbInfo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            pbInfo.Cursor = Cursors.Hand;
            pbInfo.Image = (Image)resources.GetObject("pbInfo.Image");
            pbInfo.Location = new Point(1501, 34);
            pbInfo.Name = "pbInfo";
            pbInfo.Size = new Size(29, 30);
            pbInfo.SizeMode = PictureBoxSizeMode.CenterImage;
            pbInfo.TabIndex = 100;
            pbInfo.TabStop = false;
            pbInfo.Click += pbInfo_Click;
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = Color.FromArgb(246, 246, 246);
            panel2.Controls.Add(panel1);
            panel2.Controls.Add(flowPanel);
            panel2.Location = new Point(-1, 118);
            panel2.Name = "panel2";
            panel2.Size = new Size(1500, 512);
            panel2.TabIndex = 101;
            panel2.Paint += panel2_Paint;
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Bottom;
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(txtGotoPage);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(btnLastPage);
            panel1.Controls.Add(btnFirstPage);
            panel1.Controls.Add(btnNext);
            panel1.Controls.Add(lblPageNo);
            panel1.Controls.Add(btnPrevious);
            panel1.Location = new Point(492, 477);
            panel1.Name = "panel1";
            panel1.Size = new Size(514, 31);
            panel1.TabIndex = 16;
            // 
            // txtGotoPage
            // 
            txtGotoPage.Location = new Point(433, 4);
            txtGotoPage.Name = "txtGotoPage";
            txtGotoPage.Size = new Size(75, 25);
            txtGotoPage.TabIndex = 1;
            txtGotoPage.KeyDown += txtGotoPage_KeyDown;
            txtGotoPage.KeyPress += txtGotoPage_KeyPress;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(373, 8);
            label6.Name = "label6";
            label6.Size = new Size(75, 18);
            label6.TabIndex = 0;
            label6.Text = "go to page";
            // 
            // btnLastPage
            // 
            btnLastPage.BackColor = Color.White;
            btnLastPage.FlatAppearance.BorderSize = 0;
            btnLastPage.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 152, 210);
            btnLastPage.FlatStyle = FlatStyle.Flat;
            btnLastPage.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLastPage.ForeColor = Color.Black;
            btnLastPage.Location = new Point(333, 2);
            btnLastPage.Name = "btnLastPage";
            btnLastPage.Size = new Size(35, 25);
            btnLastPage.TabIndex = 11;
            btnLastPage.Text = ">>";
            btnLastPage.UseVisualStyleBackColor = false;
            btnLastPage.Click += btnLastPage_Click;
            // 
            // btnFirstPage
            // 
            btnFirstPage.BackColor = Color.White;
            btnFirstPage.FlatAppearance.BorderSize = 0;
            btnFirstPage.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 152, 210);
            btnFirstPage.FlatStyle = FlatStyle.Flat;
            btnFirstPage.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnFirstPage.ForeColor = Color.Black;
            btnFirstPage.Location = new Point(26, 2);
            btnFirstPage.Name = "btnFirstPage";
            btnFirstPage.Size = new Size(33, 25);
            btnFirstPage.TabIndex = 14;
            btnFirstPage.Text = "<<";
            btnFirstPage.UseVisualStyleBackColor = false;
            btnFirstPage.Click += btnFirstPage_Click;
            // 
            // btnNext
            // 
            btnNext.BackColor = Color.White;
            btnNext.Enabled = false;
            btnNext.FlatAppearance.BorderSize = 0;
            btnNext.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 152, 210);
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNext.ForeColor = Color.Black;
            btnNext.Location = new Point(302, 2);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(27, 25);
            btnNext.TabIndex = 10;
            btnNext.Text = "> ";
            btnNext.UseVisualStyleBackColor = false;
            btnNext.Click += btnNext_Click;
            // 
            // lblPageNo
            // 
            lblPageNo.BackColor = Color.White;
            lblPageNo.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPageNo.ForeColor = Color.Black;
            lblPageNo.Location = new Point(97, 3);
            lblPageNo.MinimumSize = new Size(0, 22);
            lblPageNo.Name = "lblPageNo";
            lblPageNo.Size = new Size(199, 22);
            lblPageNo.TabIndex = 7;
            lblPageNo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnPrevious
            // 
            btnPrevious.BackColor = Color.White;
            btnPrevious.Enabled = false;
            btnPrevious.FlatAppearance.BorderSize = 0;
            btnPrevious.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 152, 210);
            btnPrevious.FlatStyle = FlatStyle.Flat;
            btnPrevious.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPrevious.ForeColor = Color.Black;
            btnPrevious.Location = new Point(63, 2);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(28, 25);
            btnPrevious.TabIndex = 13;
            btnPrevious.Text = "<";
            btnPrevious.UseVisualStyleBackColor = false;
            btnPrevious.Click += btnPrevious_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(51, 14);
            label5.Name = "label5";
            label5.Size = new Size(19, 20);
            label5.TabIndex = 103;
            label5.Text = ">";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.ForeColor = Color.FromArgb(0, 166, 90);
            label4.Location = new Point(64, 13);
            label4.Name = "label4";
            label4.Size = new Size(124, 20);
            label4.TabIndex = 104;
            label4.Text = "Manage Stocks";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(15, 14);
            label1.Name = "label1";
            label1.Size = new Size(44, 20);
            label1.TabIndex = 105;
            label1.Text = "POS";
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "ItemId";
            dataGridViewTextBoxColumn1.MinimumWidth = 6;
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            dataGridViewTextBoxColumn1.ReadOnly = true;
            dataGridViewTextBoxColumn1.Width = 125;
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewTextBoxColumn2.HeaderText = "Item";
            dataGridViewTextBoxColumn2.MinimumWidth = 6;
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Quantity";
            dataGridViewTextBoxColumn3.MinimumWidth = 6;
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            dataGridViewTextBoxColumn3.Width = 125;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "UnitCost";
            dataGridViewTextBoxColumn4.MinimumWidth = 6;
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            dataGridViewTextBoxColumn4.Width = 125;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "Total Cost";
            dataGridViewTextBoxColumn5.MinimumWidth = 6;
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            dataGridViewTextBoxColumn5.ReadOnly = true;
            dataGridViewTextBoxColumn5.Width = 125;
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "Retail Price";
            dataGridViewTextBoxColumn6.MinimumWidth = 6;
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            dataGridViewTextBoxColumn6.ReadOnly = true;
            dataGridViewTextBoxColumn6.Width = 125;
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "Batch";
            dataGridViewTextBoxColumn7.MinimumWidth = 6;
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.ReadOnly = true;
            dataGridViewTextBoxColumn7.Width = 125;
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewTextBoxColumn8.HeaderText = "Discount";
            dataGridViewTextBoxColumn8.MinimumWidth = 6;
            dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            dataGridViewTextBoxColumn8.ReadOnly = true;
            dataGridViewTextBoxColumn8.Width = 125;
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewTextBoxColumn9.HeaderText = "Sales Tax";
            dataGridViewTextBoxColumn9.MinimumWidth = 6;
            dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            dataGridViewTextBoxColumn9.ReadOnly = true;
            dataGridViewTextBoxColumn9.Width = 125;
            // 
            // dataGridViewTextBoxColumn10
            // 
            dataGridViewTextBoxColumn10.HeaderText = "Net Value";
            dataGridViewTextBoxColumn10.MinimumWidth = 6;
            dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            dataGridViewTextBoxColumn10.ReadOnly = true;
            dataGridViewTextBoxColumn10.Width = 125;
            // 
            // dataGridViewTextBoxColumn11
            // 
            dataGridViewTextBoxColumn11.HeaderText = "Create At";
            dataGridViewTextBoxColumn11.MinimumWidth = 6;
            dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            dataGridViewTextBoxColumn11.ReadOnly = true;
            dataGridViewTextBoxColumn11.Width = 125;
            // 
            // frmManageStocks
            // 
            AutoScaleDimensions = new SizeF(8F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1498, 629);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label1);
            Controls.Add(panel2);
            Controls.Add(pbInfo);
            Controls.Add(cmbSuppliers);
            Controls.Add(txtSearchByName);
            Controls.Add(lblTo);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(lblFrom);
            Controls.Add(dtpTo);
            Controls.Add(dtpFrom);
            Controls.Add(btnExcel);
            Controls.Add(btnPrint);
            Controls.Add(btnAddNewStock);
            Controls.Add(btnRefresh);
            Font = new Font("Microsoft Tai Le", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "frmManageStocks";
            Text = "Manage Stock";
            Load += frmManageStocks_Load;
            flowPanel.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ((ISupportInitialize)dataGridView1).EndInit();
            ((ISupportInitialize)pbInfo).EndInit();
            panel2.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowPanel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnAddNewStock;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExcel;
        private System.Windows.Forms.SaveFileDialog dlgSaveExcel;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.TextBox txtSearchByName;
        private System.Windows.Forms.ComboBox cmbSuppliers;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pbInfo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        //private DevComponents.DotNetBar.Controls.Line line1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtGotoPage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblPageNo;
        private System.Windows.Forms.Button btnPrevious;
        private Panel panel3;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn colItemId;
        private DataGridViewTextBoxColumn colItemName;
        private DataGridViewTextBoxColumn colQuantity;
        private DataGridViewTextBoxColumn colUnitCost;
        private DataGridViewTextBoxColumn colToalCost;
        private DataGridViewTextBoxColumn colRetailPrice;
        private DataGridViewTextBoxColumn colBatch;
        private DataGridViewTextBoxColumn colDiscount;
        private DataGridViewTextBoxColumn colSalesTax;
        private DataGridViewTextBoxColumn colNetValue;
        private DataGridViewTextBoxColumn colCreatedAt;
    }
}