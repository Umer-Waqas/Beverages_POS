namespace Restaurant_MS_UI.Menu.Main
{
    partial class frmExpense
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmExpense));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtDescription = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnAddSupplier = new System.Windows.Forms.Button();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.ErrExpCategory = new System.Windows.Forms.Label();
            this.errAmt = new System.Windows.Forms.Label();
            this.pnlSupplierItems = new System.Windows.Forms.Panel();
            this.filterToDate = new System.Windows.Forms.DateTimePicker();
            this.filterFromDate = new System.Windows.Forms.DateTimePicker();
            this.filterCategory = new System.Windows.Forms.ComboBox();
            this.grdExpenses = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExpCategoryId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExpenseCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalExpense = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExpDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colExpDateDisplay = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.label9 = new System.Windows.Forms.Label();
            this.pbInfo = new System.Windows.Forms.PictureBox();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.btnAddExpCategory = new System.Windows.Forms.Button();
            this.ErrDesc = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbExpCategory = new System.Windows.Forms.ComboBox();
            this.dtpExpDate = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTotal = new System.Windows.Forms.Label();
            this.pnlSupplierItems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdExpenses)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(154, 193);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(333, 103);
            this.txtDescription.TabIndex = 3;
            this.txtDescription.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 196);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 18);
            this.label2.TabIndex = 5;
            this.label2.Text = "Expense Description";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(79, 319);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 18);
            this.label8.TabIndex = 14;
            this.label8.Text = "Amount";
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnClear.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(400, 379);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(119, 36);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnAddSupplier
            // 
            this.btnAddSupplier.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnAddSupplier.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnAddSupplier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddSupplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSupplier.ForeColor = System.Drawing.Color.White;
            this.btnAddSupplier.Location = new System.Drawing.Point(273, 379);
            this.btnAddSupplier.Name = "btnAddSupplier";
            this.btnAddSupplier.Size = new System.Drawing.Size(119, 36);
            this.btnAddSupplier.TabIndex = 5;
            this.btnAddSupplier.Text = "Save";
            this.btnAddSupplier.UseVisualStyleBackColor = false;
            this.btnAddSupplier.Click += new System.EventHandler(this.btnAddSupplier_Click);
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Coral;
            this.ErrMessage.Location = new System.Drawing.Point(24, 10);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(330, 18);
            this.ErrMessage.TabIndex = 35;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // ErrExpCategory
            // 
            this.ErrExpCategory.AutoSize = true;
            this.ErrExpCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrExpCategory.ForeColor = System.Drawing.Color.Red;
            this.ErrExpCategory.Location = new System.Drawing.Point(493, 146);
            this.ErrExpCategory.Name = "ErrExpCategory";
            this.ErrExpCategory.Size = new System.Drawing.Size(24, 29);
            this.ErrExpCategory.TabIndex = 36;
            this.ErrExpCategory.Text = "*";
            this.ErrExpCategory.Visible = false;
            // 
            // errAmt
            // 
            this.errAmt.AutoSize = true;
            this.errAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errAmt.ForeColor = System.Drawing.Color.Red;
            this.errAmt.Location = new System.Drawing.Point(375, 316);
            this.errAmt.Name = "errAmt";
            this.errAmt.Size = new System.Drawing.Size(24, 29);
            this.errAmt.TabIndex = 41;
            this.errAmt.Text = "*";
            this.errAmt.Visible = false;
            // 
            // pnlSupplierItems
            // 
            this.pnlSupplierItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSupplierItems.Controls.Add(this.filterToDate);
            this.pnlSupplierItems.Controls.Add(this.filterFromDate);
            this.pnlSupplierItems.Controls.Add(this.filterCategory);
            this.pnlSupplierItems.Controls.Add(this.grdExpenses);
            this.pnlSupplierItems.Controls.Add(this.label9);
            this.pnlSupplierItems.Location = new System.Drawing.Point(525, 81);
            this.pnlSupplierItems.Name = "pnlSupplierItems";
            this.pnlSupplierItems.Size = new System.Drawing.Size(882, 484);
            this.pnlSupplierItems.TabIndex = 42;
            // 
            // filterToDate
            // 
            this.filterToDate.Location = new System.Drawing.Point(557, 9);
            this.filterToDate.Name = "filterToDate";
            this.filterToDate.Size = new System.Drawing.Size(237, 25);
            this.filterToDate.TabIndex = 9;
            this.filterToDate.ValueChanged += new System.EventHandler(this.filterToDate_ValueChanged);
            // 
            // filterFromDate
            // 
            this.filterFromDate.Location = new System.Drawing.Point(312, 10);
            this.filterFromDate.Name = "filterFromDate";
            this.filterFromDate.Size = new System.Drawing.Size(237, 25);
            this.filterFromDate.TabIndex = 8;
            this.filterFromDate.ValueChanged += new System.EventHandler(this.filterFromDate_ValueChanged);
            // 
            // filterCategory
            // 
            this.filterCategory.FormattingEnabled = true;
            this.filterCategory.Location = new System.Drawing.Point(6, 9);
            this.filterCategory.Name = "filterCategory";
            this.filterCategory.Size = new System.Drawing.Size(230, 26);
            this.filterCategory.TabIndex = 7;
            this.filterCategory.SelectedIndexChanged += new System.EventHandler(this.filterCategory_SelectedIndexChanged);
            // 
            // grdExpenses
            // 
            this.grdExpenses.AllowUserToAddRows = false;
            this.grdExpenses.AllowUserToDeleteRows = false;
            this.grdExpenses.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdExpenses.BackgroundColor = System.Drawing.Color.White;
            this.grdExpenses.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdExpenses.ColumnHeadersHeight = 34;
            this.grdExpenses.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colExpCategoryId,
            this.colExpenseCategory,
            this.colTotalExpense,
            this.colDesc,
            this.colExpDate,
            this.colExpDateDisplay,
            this.colEdit,
            this.colDelete});
            this.grdExpenses.EnableHeadersVisualStyles = false;
            this.grdExpenses.Location = new System.Drawing.Point(6, 78);
            this.grdExpenses.Name = "grdExpenses";
            this.grdExpenses.RowHeadersVisible = false;
            this.grdExpenses.RowTemplate.Height = 35;
            this.grdExpenses.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdExpenses.Size = new System.Drawing.Size(873, 403);
            this.grdExpenses.TabIndex = 10;
            this.grdExpenses.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdExpenses_CellContentClick);
            this.grdExpenses.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdExpenses_CellDoubleClick);
            this.grdExpenses.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdExpenses_RowEnter);
            this.grdExpenses.DoubleClick += new System.EventHandler(this.grdExpenses_DoubleClick);
            // 
            // colId
            // 
            this.colId.HeaderText = "Id";
            this.colId.Name = "colId";
            this.colId.ReadOnly = true;
            // 
            // colExpCategoryId
            // 
            this.colExpCategoryId.HeaderText = "ExpeCatId";
            this.colExpCategoryId.Name = "colExpCategoryId";
            this.colExpCategoryId.ReadOnly = true;
            // 
            // colExpenseCategory
            // 
            this.colExpenseCategory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colExpenseCategory.HeaderText = "Expense Category";
            this.colExpenseCategory.MinimumWidth = 100;
            this.colExpenseCategory.Name = "colExpenseCategory";
            this.colExpenseCategory.ReadOnly = true;
            // 
            // colTotalExpense
            // 
            this.colTotalExpense.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = null;
            this.colTotalExpense.DefaultCellStyle = dataGridViewCellStyle3;
            this.colTotalExpense.HeaderText = "Total Expense";
            this.colTotalExpense.Name = "colTotalExpense";
            this.colTotalExpense.ReadOnly = true;
            this.colTotalExpense.Width = 70;
            // 
            // colDesc
            // 
            this.colDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDesc.HeaderText = "Description";
            this.colDesc.Name = "colDesc";
            this.colDesc.ReadOnly = true;
            // 
            // colExpDate
            // 
            this.colExpDate.HeaderText = "Expense Date Hidden";
            this.colExpDate.Name = "colExpDate";
            this.colExpDate.ReadOnly = true;
            // 
            // colExpDateDisplay
            // 
            this.colExpDateDisplay.HeaderText = "Expense Date";
            this.colExpDateDisplay.Name = "colExpDateDisplay";
            this.colExpDateDisplay.ReadOnly = true;
            this.colExpDateDisplay.Width = 200;
            // 
            // colEdit
            // 
            this.colEdit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colEdit.HeaderText = "Edit";
            this.colEdit.Name = "colEdit";
            this.colEdit.Text = "Edit";
            this.colEdit.UseColumnTextForButtonValue = true;
            this.colEdit.Width = 50;
            // 
            // colDelete
            // 
            this.colDelete.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colDelete.HeaderText = "Delete";
            this.colDelete.MinimumWidth = 50;
            this.colDelete.Name = "colDelete";
            this.colDelete.Text = "Delete";
            this.colDelete.UseColumnTextForButtonValue = true;
            this.colDelete.Width = 50;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(3, 41);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(878, 37);
            this.label9.TabIndex = 2;
            this.label9.Text = "Expenses Detail";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbInfo
            // 
            this.pbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbInfo.Image = ((System.Drawing.Image)(resources.GetObject("pbInfo.Image")));
            this.pbInfo.Location = new System.Drawing.Point(1408, 0);
            this.pbInfo.Name = "pbInfo";
            this.pbInfo.Size = new System.Drawing.Size(29, 30);
            this.pbInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbInfo.TabIndex = 100;
            this.pbInfo.TabStop = false;
            this.pbInfo.Click += new System.EventHandler(this.pbInfo_Click);
            // 
            // txtAmount
            // 
            this.txtAmount.Location = new System.Drawing.Point(154, 316);
            this.txtAmount.MaxLength = 12;
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(215, 25);
            this.txtAmount.TabIndex = 4;
            this.txtAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtAmount_KeyPress);
            // 
            // btnAddExpCategory
            // 
            this.btnAddExpCategory.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnAddExpCategory.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnAddExpCategory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddExpCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddExpCategory.ForeColor = System.Drawing.Color.White;
            this.btnAddExpCategory.Location = new System.Drawing.Point(456, 145);
            this.btnAddExpCategory.Name = "btnAddExpCategory";
            this.btnAddExpCategory.Size = new System.Drawing.Size(35, 27);
            this.btnAddExpCategory.TabIndex = 2;
            this.btnAddExpCategory.Text = "+";
            this.btnAddExpCategory.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAddExpCategory.UseVisualStyleBackColor = false;
            this.btnAddExpCategory.Click += new System.EventHandler(this.btnAddExpCategory_Click);
            // 
            // ErrDesc
            // 
            this.ErrDesc.AutoSize = true;
            this.ErrDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrDesc.ForeColor = System.Drawing.Color.Red;
            this.ErrDesc.Location = new System.Drawing.Point(493, 196);
            this.ErrDesc.Name = "ErrDesc";
            this.ErrDesc.Size = new System.Drawing.Size(24, 29);
            this.ErrDesc.TabIndex = 38;
            this.ErrDesc.Text = "*";
            this.ErrDesc.Visible = false;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("Microsoft Tai Le", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(525, 568);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 34);
            this.label3.TabIndex = 105;
            this.label3.Text = "Total :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Expense Category";
            // 
            // cmbExpCategory
            // 
            this.cmbExpCategory.FormattingEnabled = true;
            this.cmbExpCategory.Location = new System.Drawing.Point(154, 146);
            this.cmbExpCategory.Name = "cmbExpCategory";
            this.cmbExpCategory.Size = new System.Drawing.Size(296, 26);
            this.cmbExpCategory.TabIndex = 1;
            // 
            // dtpExpDate
            // 
            this.dtpExpDate.Location = new System.Drawing.Point(154, 103);
            this.dtpExpDate.Name = "dtpExpDate";
            this.dtpExpDate.Size = new System.Drawing.Size(333, 25);
            this.dtpExpDate.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 108);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 18);
            this.label4.TabIndex = 110;
            this.label4.Text = "Expense Date";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(196)))), ((int)(((byte)(244)))));
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(210)))));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(1274, 626);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(151, 52);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.HeaderText = "Item Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.HeaderText = "Item Type";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn3.HeaderText = "CreatedAt";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 20;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "N0";
            dataGridViewCellStyle4.NullValue = null;
            this.dataGridViewTextBoxColumn4.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewTextBoxColumn4.HeaderText = "CreatedAt";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.HeaderText = "Description";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "CreatedAt";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Expense Date";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 200;
            // 
            // lblTotal
            // 
            this.lblTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Microsoft Tai Le", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.Location = new System.Drawing.Point(683, 568);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(30, 34);
            this.lblTotal.TabIndex = 111;
            this.lblTotal.Text = "0";
            // 
            // frmExpense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1437, 690);
            this.Controls.Add(this.lblTotal);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dtpExpDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnAddExpCategory);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.cmbExpCategory);
            this.Controls.Add(this.pbInfo);
            this.Controls.Add(this.pnlSupplierItems);
            this.Controls.Add(this.errAmt);
            this.Controls.Add(this.ErrDesc);
            this.Controls.Add(this.ErrExpCategory);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnAddSupplier);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmExpense";
            this.Text = "Expense";
            this.Load += new System.EventHandler(this.frmAddSupplier_Load);
            this.pnlSupplierItems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdExpenses)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox txtDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnAddSupplier;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.Label ErrExpCategory;
        private System.Windows.Forms.Label errAmt;
        private System.Windows.Forms.Panel pnlSupplierItems;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridView grdExpenses;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.PictureBox pbInfo;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Button btnAddExpCategory;
        private System.Windows.Forms.Label ErrDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox filterCategory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbExpCategory;
        private System.Windows.Forms.DateTimePicker filterToDate;
        private System.Windows.Forms.DateTimePicker filterFromDate;
        private System.Windows.Forms.DateTimePicker dtpExpDate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExpCategoryId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExpenseCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTotalExpense;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDesc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExpDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExpDateDisplay;
        private System.Windows.Forms.DataGridViewButtonColumn colEdit;
        private System.Windows.Forms.DataGridViewButtonColumn colDelete;
        private System.Windows.Forms.Label lblTotal;
    }
}