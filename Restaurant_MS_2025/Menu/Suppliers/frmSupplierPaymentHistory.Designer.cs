namespace Restaurant_MS_UI.Menu.Suppliers
{
    partial class frmSupplierPaymentHistory
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
            this.grdPayments = new System.Windows.Forms.DataGridView();
            this.colExpenseId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colstockIdPm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDocumentNoPm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDatePm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescriptionPm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaymentModePm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPaymentPm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEdit = new System.Windows.Forms.DataGridViewButtonColumn();
            this.colDelete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDocumentNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblSupplierName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grdPayments)).BeginInit();
            this.SuspendLayout();
            // 
            // grdPayments
            // 
            this.grdPayments.AllowUserToAddRows = false;
            this.grdPayments.AllowUserToDeleteRows = false;
            this.grdPayments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdPayments.BackgroundColor = System.Drawing.Color.White;
            this.grdPayments.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdPayments.ColumnHeadersHeight = 35;
            this.grdPayments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colExpenseId,
            this.colstockIdPm,
            this.colDocumentNoPm,
            this.colDatePm,
            this.colDescriptionPm,
            this.colPaymentModePm,
            this.colPaymentPm,
            this.colEdit,
            this.colDelete});
            this.grdPayments.EnableHeadersVisualStyles = false;
            this.grdPayments.Location = new System.Drawing.Point(11, 178);
            this.grdPayments.Name = "grdPayments";
            this.grdPayments.RowHeadersVisible = false;
            this.grdPayments.Size = new System.Drawing.Size(1354, 488);
            this.grdPayments.TabIndex = 2;
            this.grdPayments.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdPayments_CellContentClick);
            // 
            // colExpenseId
            // 
            this.colExpenseId.HeaderText = "Expense Id";
            this.colExpenseId.Name = "colExpenseId";
            this.colExpenseId.ReadOnly = true;
            // 
            // colstockIdPm
            // 
            this.colstockIdPm.HeaderText = "SOTCKID";
            this.colstockIdPm.Name = "colstockIdPm";
            this.colstockIdPm.ReadOnly = true;
            this.colstockIdPm.Visible = false;
            // 
            // colDocumentNoPm
            // 
            this.colDocumentNoPm.HeaderText = "DOCUMENT NO";
            this.colDocumentNoPm.Name = "colDocumentNoPm";
            this.colDocumentNoPm.ReadOnly = true;
            // 
            // colDatePm
            // 
            this.colDatePm.HeaderText = "DATE";
            this.colDatePm.Name = "colDatePm";
            this.colDatePm.ReadOnly = true;
            // 
            // colDescriptionPm
            // 
            this.colDescriptionPm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDescriptionPm.HeaderText = "DESCRIPTION";
            this.colDescriptionPm.Name = "colDescriptionPm";
            this.colDescriptionPm.ReadOnly = true;
            // 
            // colPaymentModePm
            // 
            this.colPaymentModePm.HeaderText = "PAYMENT MODE";
            this.colPaymentModePm.Name = "colPaymentModePm";
            this.colPaymentModePm.ReadOnly = true;
            // 
            // colPaymentPm
            // 
            this.colPaymentPm.HeaderText = "AMOUNT PAID";
            this.colPaymentPm.Name = "colPaymentPm";
            this.colPaymentPm.ReadOnly = true;
            // 
            // colEdit
            // 
            this.colEdit.HeaderText = "Edit";
            this.colEdit.Name = "colEdit";
            this.colEdit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colEdit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.colEdit.Text = "Edit";
            this.colEdit.UseColumnTextForButtonValue = true;
            // 
            // colDelete
            // 
            this.colDelete.HeaderText = "Delete";
            this.colDelete.Name = "colDelete";
            this.colDelete.Text = "Delete";
            this.colDelete.UseColumnTextForButtonValue = true;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpFromDate.Location = new System.Drawing.Point(11, 116);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(347, 30);
            this.dtpFromDate.TabIndex = 3;
            // 
            // dtpToDate
            // 
            this.dtpToDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpToDate.Location = new System.Drawing.Point(364, 116);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(353, 30);
            this.dtpToDate.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "From Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(369, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 25);
            this.label2.TabIndex = 5;
            this.label2.Text = "To Date";
            // 
            // txtDocumentNo
            // 
            this.txtDocumentNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDocumentNo.Location = new System.Drawing.Point(723, 116);
            this.txtDocumentNo.Name = "txtDocumentNo";
            this.txtDocumentNo.Size = new System.Drawing.Size(211, 30);
            this.txtDocumentNo.TabIndex = 6;
            this.txtDocumentNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtDocumentNo_KeyDown);
            this.txtDocumentNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDocumentNo_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(718, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(131, 25);
            this.label3.TabIndex = 7;
            this.label3.Text = "Document No";
            // 
            // lblSupplierName
            // 
            this.lblSupplierName.AutoSize = true;
            this.lblSupplierName.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblSupplierName.Font = new System.Drawing.Font("Segoe UI Semibold", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSupplierName.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.lblSupplierName.Location = new System.Drawing.Point(31, 9);
            this.lblSupplierName.Name = "lblSupplierName";
            this.lblSupplierName.Size = new System.Drawing.Size(145, 37);
            this.lblSupplierName.TabIndex = 8;
            this.lblSupplierName.Text = "From Date";
            // 
            // frmSupplierPaymentHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1378, 679);
            this.Controls.Add(this.lblSupplierName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtDocumentNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dtpToDate);
            this.Controls.Add(this.dtpFromDate);
            this.Controls.Add(this.grdPayments);
            this.Name = "frmSupplierPaymentHistory";
            this.Text = "Supplier Payment History";
            this.Load += new System.EventHandler(this.frmSupplierPaymentHistory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdPayments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdPayments;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDocumentNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colExpenseId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colstockIdPm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDocumentNoPm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDatePm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescriptionPm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaymentModePm;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPaymentPm;
        private System.Windows.Forms.DataGridViewButtonColumn colEdit;
        private System.Windows.Forms.DataGridViewButtonColumn colDelete;
        private System.Windows.Forms.Label lblSupplierName;
    }
}