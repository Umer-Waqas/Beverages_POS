namespace Restaurant_MS_UI.Menu.Suppliers
{
    partial class frmAddSupplier
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
            this.txtSuppName = new System.Windows.Forms.TextBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.txtPrimContName = new System.Windows.Forms.TextBox();
            this.txtPrimaryContPh = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.ErrSuppName = new System.Windows.Forms.Label();
            this.ErrPhoneNo = new System.Windows.Forms.Label();
            this.chkIsHoSupplier = new System.Windows.Forms.CheckBox();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.grdSupplierItems = new System.Windows.Forms.DataGridView();
            this.label9 = new System.Windows.Forms.Label();
            this.colItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.grdSupplierItems)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSuppName
            // 
            this.txtSuppName.Location = new System.Drawing.Point(199, 73);
            this.txtSuppName.Name = "txtSuppName";
            this.txtSuppName.Size = new System.Drawing.Size(289, 22);
            this.txtSuppName.TabIndex = 0;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(199, 123);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(289, 22);
            this.txtPhone.TabIndex = 0;
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(199, 166);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(289, 123);
            this.txtAddress.TabIndex = 0;
            // 
            // txtPrimContName
            // 
            this.txtPrimContName.Location = new System.Drawing.Point(199, 344);
            this.txtPrimContName.Name = "txtPrimContName";
            this.txtPrimContName.Size = new System.Drawing.Size(289, 22);
            this.txtPrimContName.TabIndex = 0;
            // 
            // txtPrimaryContPh
            // 
            this.txtPrimaryContPh.Location = new System.Drawing.Point(199, 390);
            this.txtPrimaryContPh.Name = "txtPrimaryContPh";
            this.txtPrimaryContPh.Size = new System.Drawing.Size(289, 22);
            this.txtPrimaryContPh.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Supplier Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Phone";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 166);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Address";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 344);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "Primary Contact Person";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 395);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(153, 17);
            this.label5.TabIndex = 1;
            this.label5.Text = "Primary Contact Phone";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(394, 435);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(94, 30);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(296, 435);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(94, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ErrSuppName
            // 
            this.ErrSuppName.AutoSize = true;
            this.ErrSuppName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrSuppName.ForeColor = System.Drawing.Color.Red;
            this.ErrSuppName.Location = new System.Drawing.Point(494, 73);
            this.ErrSuppName.Name = "ErrSuppName";
            this.ErrSuppName.Size = new System.Drawing.Size(24, 29);
            this.ErrSuppName.TabIndex = 39;
            this.ErrSuppName.Text = "*";
            this.ErrSuppName.Visible = false;
            // 
            // ErrPhoneNo
            // 
            this.ErrPhoneNo.AutoSize = true;
            this.ErrPhoneNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrPhoneNo.ForeColor = System.Drawing.Color.Red;
            this.ErrPhoneNo.Location = new System.Drawing.Point(494, 123);
            this.ErrPhoneNo.Name = "ErrPhoneNo";
            this.ErrPhoneNo.Size = new System.Drawing.Size(24, 29);
            this.ErrPhoneNo.TabIndex = 39;
            this.ErrPhoneNo.Text = "*";
            this.ErrPhoneNo.Visible = false;
            // 
            // chkIsHoSupplier
            // 
            this.chkIsHoSupplier.AutoSize = true;
            this.chkIsHoSupplier.Location = new System.Drawing.Point(390, 46);
            this.chkIsHoSupplier.Name = "chkIsHoSupplier";
            this.chkIsHoSupplier.Size = new System.Drawing.Size(98, 21);
            this.chkIsHoSupplier.TabIndex = 40;
            this.chkIsHoSupplier.Text = "checkBox1";
            this.chkIsHoSupplier.UseVisualStyleBackColor = true;
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Coral;
            this.ErrMessage.Location = new System.Drawing.Point(22, 9);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(330, 18);
            this.ErrMessage.TabIndex = 41;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // grdSupplierItems
            // 
            this.grdSupplierItems.AllowUserToAddRows = false;
            this.grdSupplierItems.AllowUserToDeleteRows = false;
            this.grdSupplierItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdSupplierItems.BackgroundColor = System.Drawing.Color.White;
            this.grdSupplierItems.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.grdSupplierItems.ColumnHeadersHeight = 34;
            this.grdSupplierItems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colItemName});
            this.grdSupplierItems.EnableHeadersVisualStyles = false;
            this.grdSupplierItems.Location = new System.Drawing.Point(605, 92);
            this.grdSupplierItems.Name = "grdSupplierItems";
            this.grdSupplierItems.RowHeadersVisible = false;
            this.grdSupplierItems.RowTemplate.Height = 35;
            this.grdSupplierItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdSupplierItems.Size = new System.Drawing.Size(878, 382);
            this.grdSupplierItems.TabIndex = 43;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(241)))), ((int)(((byte)(241)))));
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(602, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(883, 37);
            this.label9.TabIndex = 42;
            this.label9.Text = "Expenses Detail";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // colItemName
            // 
            this.colItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colItemName.HeaderText = "Item Name";
            this.colItemName.Name = "colItemName";
            this.colItemName.ReadOnly = true;
            // 
            // frmAddSupplier1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1495, 506);
            this.Controls.Add(this.grdSupplierItems);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.chkIsHoSupplier);
            this.Controls.Add(this.ErrPhoneNo);
            this.Controls.Add(this.ErrSuppName);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPrimaryContPh);
            this.Controls.Add(this.txtPrimContName);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.txtSuppName);
            this.Name = "frmAddSupplier1";
            this.Text = "Add Supplier";
            this.Load += new System.EventHandler(this.frmAddSupplier1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdSupplierItems)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSuppName;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.TextBox txtPrimContName;
        private System.Windows.Forms.TextBox txtPrimaryContPh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label ErrSuppName;
        private System.Windows.Forms.Label ErrPhoneNo;
        private System.Windows.Forms.CheckBox chkIsHoSupplier;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.DataGridView grdSupplierItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemName;
        private System.Windows.Forms.Label label9;
    }
}