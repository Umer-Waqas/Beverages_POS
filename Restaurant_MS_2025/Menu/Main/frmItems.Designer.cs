namespace Restaurant_MS_UI.Menu.Main
{
    partial class frmItems
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.btnAddItem = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnAddSupplier = new System.Windows.Forms.Button();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numReOrderingLevel = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numOpeningStock = new System.Windows.Forms.NumericUpDown();
            this.txtGenericName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtUnitCostPrice = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ErrItemName = new System.Windows.Forms.Label();
            this.ErrBarcode = new System.Windows.Forms.Label();
            this.ErrSupplier = new System.Windows.Forms.Label();
            this.ErrUnit = new System.Windows.Forms.Label();
            this.ErrReOrderingLevel = new System.Windows.Forms.Label();
            this.ErrOpeningStock = new System.Windows.Forms.Label();
            this.ErrChemicalName = new System.Windows.Forms.Label();
            this.ErrUnitCostPrice = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.ErrMessage = new System.Windows.Forms.Label();
            this.lstSuppliers = new System.Windows.Forms.ListBox();
            this.lblInvlalidUnitCost = new System.Windows.Forms.Label();
            this.pnlOpeningStock = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblInvalidRetailPrice = new System.Windows.Forms.Label();
            this.errRetailPrice = new System.Windows.Forms.Label();
            this.txtRetailPrice = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnAddCategory = new System.Windows.Forms.Button();
            this.errCategory = new System.Windows.Forms.Label();
            this.numConvUnit = new System.Windows.Forms.NumericUpDown();
            this.errConvUnit = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbManufacturer = new System.Windows.Forms.ComboBox();
            this.cmbRacks = new System.Windows.Forms.ComboBox();
            this.btnAddRack = new System.Windows.Forms.Button();
            this.btnAddManf = new System.Windows.Forms.Button();
            this.chkIsRawItem = new System.Windows.Forms.CheckBox();
            this.errManf = new System.Windows.Forms.Label();
            this.cmbSubCategory = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.btnAddSubCategory = new System.Windows.Forms.Button();
            this.chkCheckStockOnSale = new System.Windows.Forms.CheckBox();
            this.cmbItemType = new System.Windows.Forms.ComboBox();
            this.errItemType = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numReOrderingLevel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOpeningStock)).BeginInit();
            this.pnlOpeningStock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numConvUnit)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(40, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Item Name";
            // 
            // txtItemName
            // 
            this.txtItemName.Location = new System.Drawing.Point(286, 161);
            this.txtItemName.MaxLength = 599;
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.Size = new System.Drawing.Size(248, 25);
            this.txtItemName.TabIndex = 1;
            // 
            // btnAddItem
            // 
            this.btnAddItem.Location = new System.Drawing.Point(421, 561);
            this.btnAddItem.Name = "btnAddItem";
            this.btnAddItem.Size = new System.Drawing.Size(174, 51);
            this.btnAddItem.TabIndex = 16;
            this.btnAddItem.Text = "Add";
            this.btnAddItem.UseVisualStyleBackColor = true;
            this.btnAddItem.Click += new System.EventHandler(this.btnAddItem_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(801, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 25);
            this.label2.TabIndex = 4;
            this.label2.Text = "Rack#";
            // 
            // txtBarcode
            // 
            this.txtBarcode.Location = new System.Drawing.Point(1011, 109);
            this.txtBarcode.MaxLength = 599;
            this.txtBarcode.Name = "txtBarcode";
            this.txtBarcode.Size = new System.Drawing.Size(246, 25);
            this.txtBarcode.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(801, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 25);
            this.label3.TabIndex = 6;
            this.label3.Text = "Barcode";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(801, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 25);
            this.label4.TabIndex = 8;
            this.label4.Text = "Suppliers";
            // 
            // btnAddSupplier
            // 
            this.btnAddSupplier.Location = new System.Drawing.Point(1283, 135);
            this.btnAddSupplier.Name = "btnAddSupplier";
            this.btnAddSupplier.Size = new System.Drawing.Size(32, 25);
            this.btnAddSupplier.TabIndex = 5;
            this.btnAddSupplier.Text = "+";
            this.btnAddSupplier.UseVisualStyleBackColor = true;
            this.btnAddSupplier.Click += new System.EventHandler(this.btnAddSupplier_Click);
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(286, 232);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.Size = new System.Drawing.Size(248, 25);
            this.txtUnit.TabIndex = 8;
            this.txtUnit.Text = "Pack";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(38, 232);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(205, 25);
            this.label5.TabIndex = 11;
            this.label5.Text = "Unit(Strips, Boxes etc.)";
            // 
            // numReOrderingLevel
            // 
            this.numReOrderingLevel.Location = new System.Drawing.Point(292, 313);
            this.numReOrderingLevel.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numReOrderingLevel.Name = "numReOrderingLevel";
            this.numReOrderingLevel.Size = new System.Drawing.Size(138, 25);
            this.numReOrderingLevel.TabIndex = 10;
            this.numReOrderingLevel.ThousandsSeparator = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(40, 314);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(170, 25);
            this.label6.TabIndex = 14;
            this.label6.Text = "Re-Ordering Level";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, -4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(142, 25);
            this.label7.TabIndex = 16;
            this.label7.Text = "Opening Stock";
            // 
            // numOpeningStock
            // 
            this.numOpeningStock.Location = new System.Drawing.Point(242, -4);
            this.numOpeningStock.Name = "numOpeningStock";
            this.numOpeningStock.Size = new System.Drawing.Size(113, 33);
            this.numOpeningStock.TabIndex = 12;
            // 
            // txtGenericName
            // 
            this.txtGenericName.Location = new System.Drawing.Point(744, 696);
            this.txtGenericName.Name = "txtGenericName";
            this.txtGenericName.Size = new System.Drawing.Size(246, 25);
            this.txtGenericName.TabIndex = 13;
            this.txtGenericName.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(615, 699);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(94, 18);
            this.label8.TabIndex = 17;
            this.label8.Text = "Generic Name";
            this.label8.Visible = false;
            // 
            // txtUnitCostPrice
            // 
            this.txtUnitCostPrice.Location = new System.Drawing.Point(292, 443);
            this.txtUnitCostPrice.Name = "txtUnitCostPrice";
            this.txtUnitCostPrice.Size = new System.Drawing.Size(246, 25);
            this.txtUnitCostPrice.TabIndex = 14;
            this.txtUnitCostPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUnitCostPrice_KeyPress);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(42, 438);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(140, 25);
            this.label9.TabIndex = 19;
            this.label9.Text = "Unit Cost Price";
            // 
            // ErrItemName
            // 
            this.ErrItemName.AutoSize = true;
            this.ErrItemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrItemName.ForeColor = System.Drawing.Color.Red;
            this.ErrItemName.Location = new System.Drawing.Point(533, 165);
            this.ErrItemName.Name = "ErrItemName";
            this.ErrItemName.Size = new System.Drawing.Size(24, 29);
            this.ErrItemName.TabIndex = 24;
            this.ErrItemName.Text = "*";
            this.ErrItemName.Visible = false;
            // 
            // ErrBarcode
            // 
            this.ErrBarcode.AutoSize = true;
            this.ErrBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrBarcode.ForeColor = System.Drawing.Color.Red;
            this.ErrBarcode.Location = new System.Drawing.Point(1260, 109);
            this.ErrBarcode.Name = "ErrBarcode";
            this.ErrBarcode.Size = new System.Drawing.Size(24, 29);
            this.ErrBarcode.TabIndex = 26;
            this.ErrBarcode.Text = "*";
            this.ErrBarcode.Visible = false;
            // 
            // ErrSupplier
            // 
            this.ErrSupplier.AutoSize = true;
            this.ErrSupplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrSupplier.ForeColor = System.Drawing.Color.Red;
            this.ErrSupplier.Location = new System.Drawing.Point(1260, 154);
            this.ErrSupplier.Name = "ErrSupplier";
            this.ErrSupplier.Size = new System.Drawing.Size(24, 29);
            this.ErrSupplier.TabIndex = 27;
            this.ErrSupplier.Text = "*";
            this.ErrSupplier.Visible = false;
            // 
            // ErrUnit
            // 
            this.ErrUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrUnit.ForeColor = System.Drawing.Color.Red;
            this.ErrUnit.Location = new System.Drawing.Point(533, 234);
            this.ErrUnit.Name = "ErrUnit";
            this.ErrUnit.Size = new System.Drawing.Size(14, 13);
            this.ErrUnit.TabIndex = 28;
            this.ErrUnit.Text = "*";
            this.ErrUnit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.ErrUnit.Visible = false;
            // 
            // ErrReOrderingLevel
            // 
            this.ErrReOrderingLevel.AutoSize = true;
            this.ErrReOrderingLevel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrReOrderingLevel.ForeColor = System.Drawing.Color.Red;
            this.ErrReOrderingLevel.Location = new System.Drawing.Point(431, 314);
            this.ErrReOrderingLevel.Name = "ErrReOrderingLevel";
            this.ErrReOrderingLevel.Size = new System.Drawing.Size(24, 29);
            this.ErrReOrderingLevel.TabIndex = 29;
            this.ErrReOrderingLevel.Text = "*";
            this.ErrReOrderingLevel.Visible = false;
            // 
            // ErrOpeningStock
            // 
            this.ErrOpeningStock.AutoSize = true;
            this.ErrOpeningStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrOpeningStock.ForeColor = System.Drawing.Color.Red;
            this.ErrOpeningStock.Location = new System.Drawing.Point(252, 4);
            this.ErrOpeningStock.Name = "ErrOpeningStock";
            this.ErrOpeningStock.Size = new System.Drawing.Size(24, 29);
            this.ErrOpeningStock.TabIndex = 30;
            this.ErrOpeningStock.Text = "*";
            this.ErrOpeningStock.Visible = false;
            // 
            // ErrChemicalName
            // 
            this.ErrChemicalName.AutoSize = true;
            this.ErrChemicalName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrChemicalName.ForeColor = System.Drawing.Color.Red;
            this.ErrChemicalName.Location = new System.Drawing.Point(750, 642);
            this.ErrChemicalName.Name = "ErrChemicalName";
            this.ErrChemicalName.Size = new System.Drawing.Size(24, 29);
            this.ErrChemicalName.TabIndex = 31;
            this.ErrChemicalName.Text = "*";
            this.ErrChemicalName.Visible = false;
            // 
            // ErrUnitCostPrice
            // 
            this.ErrUnitCostPrice.AutoSize = true;
            this.ErrUnitCostPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrUnitCostPrice.ForeColor = System.Drawing.Color.Red;
            this.ErrUnitCostPrice.Location = new System.Drawing.Point(541, 440);
            this.ErrUnitCostPrice.Name = "ErrUnitCostPrice";
            this.ErrUnitCostPrice.Size = new System.Drawing.Size(24, 29);
            this.ErrUnitCostPrice.TabIndex = 32;
            this.ErrUnitCostPrice.Text = "*";
            this.ErrUnitCostPrice.Visible = false;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(630, 561);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(174, 51);
            this.btnClear.TabIndex = 17;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // ErrMessage
            // 
            this.ErrMessage.AutoSize = true;
            this.ErrMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrMessage.ForeColor = System.Drawing.Color.Coral;
            this.ErrMessage.Location = new System.Drawing.Point(43, 10);
            this.ErrMessage.Name = "ErrMessage";
            this.ErrMessage.Size = new System.Drawing.Size(330, 18);
            this.ErrMessage.TabIndex = 34;
            this.ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            this.ErrMessage.Visible = false;
            // 
            // lstSuppliers
            // 
            this.lstSuppliers.FormattingEnabled = true;
            this.lstSuppliers.ItemHeight = 18;
            this.lstSuppliers.Location = new System.Drawing.Point(1011, 135);
            this.lstSuppliers.Name = "lstSuppliers";
            this.lstSuppliers.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstSuppliers.Size = new System.Drawing.Size(246, 130);
            this.lstSuppliers.TabIndex = 4;
            // 
            // lblInvlalidUnitCost
            // 
            this.lblInvlalidUnitCost.AutoSize = true;
            this.lblInvlalidUnitCost.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvlalidUnitCost.ForeColor = System.Drawing.Color.Red;
            this.lblInvlalidUnitCost.Location = new System.Drawing.Point(563, 443);
            this.lblInvlalidUnitCost.Name = "lblInvlalidUnitCost";
            this.lblInvlalidUnitCost.Size = new System.Drawing.Size(102, 17);
            this.lblInvlalidUnitCost.TabIndex = 73;
            this.lblInvlalidUnitCost.Text = "Invalid Number";
            this.lblInvlalidUnitCost.Visible = false;
            // 
            // pnlOpeningStock
            // 
            this.pnlOpeningStock.Controls.Add(this.numOpeningStock);
            this.pnlOpeningStock.Controls.Add(this.label7);
            this.pnlOpeningStock.Controls.Add(this.ErrOpeningStock);
            this.pnlOpeningStock.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlOpeningStock.Location = new System.Drawing.Point(32, 392);
            this.pnlOpeningStock.Name = "pnlOpeningStock";
            this.pnlOpeningStock.Size = new System.Drawing.Size(508, 30);
            this.pnlOpeningStock.TabIndex = 74;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(837, 561);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(174, 51);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblInvalidRetailPrice
            // 
            this.lblInvalidRetailPrice.AutoSize = true;
            this.lblInvalidRetailPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvalidRetailPrice.ForeColor = System.Drawing.Color.Red;
            this.lblInvalidRetailPrice.Location = new System.Drawing.Point(563, 351);
            this.lblInvalidRetailPrice.Name = "lblInvalidRetailPrice";
            this.lblInvalidRetailPrice.Size = new System.Drawing.Size(102, 17);
            this.lblInvalidRetailPrice.TabIndex = 78;
            this.lblInvalidRetailPrice.Text = "Invalid Number";
            this.lblInvalidRetailPrice.Visible = false;
            // 
            // errRetailPrice
            // 
            this.errRetailPrice.AutoSize = true;
            this.errRetailPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errRetailPrice.ForeColor = System.Drawing.Color.Red;
            this.errRetailPrice.Location = new System.Drawing.Point(543, 350);
            this.errRetailPrice.Name = "errRetailPrice";
            this.errRetailPrice.Size = new System.Drawing.Size(24, 29);
            this.errRetailPrice.TabIndex = 77;
            this.errRetailPrice.Text = "*";
            this.errRetailPrice.Visible = false;
            // 
            // txtRetailPrice
            // 
            this.txtRetailPrice.Location = new System.Drawing.Point(289, 351);
            this.txtRetailPrice.Name = "txtRetailPrice";
            this.txtRetailPrice.Size = new System.Drawing.Size(246, 25);
            this.txtRetailPrice.TabIndex = 11;
            this.txtRetailPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtRetailPrice_KeyPress);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(40, 351);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(108, 25);
            this.label13.TabIndex = 76;
            this.label13.Text = "Retail Price";
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(286, 118);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(248, 26);
            this.cmbCategory.TabIndex = 6;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(41, 116);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(92, 25);
            this.label11.TabIndex = 11;
            this.label11.Text = "Category";
            // 
            // btnAddCategory
            // 
            this.btnAddCategory.Location = new System.Drawing.Point(560, 115);
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.Size = new System.Drawing.Size(32, 24);
            this.btnAddCategory.TabIndex = 7;
            this.btnAddCategory.Text = "+";
            this.btnAddCategory.UseVisualStyleBackColor = true;
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
            // 
            // errCategory
            // 
            this.errCategory.AutoSize = true;
            this.errCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errCategory.ForeColor = System.Drawing.Color.Red;
            this.errCategory.Location = new System.Drawing.Point(533, 118);
            this.errCategory.Name = "errCategory";
            this.errCategory.Size = new System.Drawing.Size(24, 29);
            this.errCategory.TabIndex = 28;
            this.errCategory.Text = "*";
            this.errCategory.Visible = false;
            // 
            // numConvUnit
            // 
            this.numConvUnit.Location = new System.Drawing.Point(291, 272);
            this.numConvUnit.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numConvUnit.Name = "numConvUnit";
            this.numConvUnit.Size = new System.Drawing.Size(138, 25);
            this.numConvUnit.TabIndex = 9;
            this.numConvUnit.ThousandsSeparator = true;
            this.numConvUnit.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // errConvUnit
            // 
            this.errConvUnit.AutoSize = true;
            this.errConvUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errConvUnit.ForeColor = System.Drawing.Color.Red;
            this.errConvUnit.Location = new System.Drawing.Point(431, 273);
            this.errConvUnit.Name = "errConvUnit";
            this.errConvUnit.Size = new System.Drawing.Size(24, 29);
            this.errConvUnit.TabIndex = 29;
            this.errConvUnit.Text = "*";
            this.errConvUnit.Visible = false;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(40, 279);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(152, 25);
            this.label14.TabIndex = 11;
            this.label14.Text = "Conversion Unit";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(801, 289);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(129, 25);
            this.label12.TabIndex = 11;
            this.label12.Text = "Manufacturer";
            // 
            // cmbManufacturer
            // 
            this.cmbManufacturer.FormattingEnabled = true;
            this.cmbManufacturer.Location = new System.Drawing.Point(1011, 286);
            this.cmbManufacturer.Name = "cmbManufacturer";
            this.cmbManufacturer.Size = new System.Drawing.Size(248, 26);
            this.cmbManufacturer.TabIndex = 6;
            // 
            // cmbRacks
            // 
            this.cmbRacks.FormattingEnabled = true;
            this.cmbRacks.Location = new System.Drawing.Point(1009, 72);
            this.cmbRacks.Name = "cmbRacks";
            this.cmbRacks.Size = new System.Drawing.Size(248, 26);
            this.cmbRacks.TabIndex = 2;
            // 
            // btnAddRack
            // 
            this.btnAddRack.Location = new System.Drawing.Point(1283, 69);
            this.btnAddRack.Name = "btnAddRack";
            this.btnAddRack.Size = new System.Drawing.Size(32, 25);
            this.btnAddRack.TabIndex = 5;
            this.btnAddRack.Text = "+";
            this.btnAddRack.UseVisualStyleBackColor = true;
            this.btnAddRack.Click += new System.EventHandler(this.btnAddRack_Click);
            // 
            // btnAddManf
            // 
            this.btnAddManf.Location = new System.Drawing.Point(1281, 288);
            this.btnAddManf.Name = "btnAddManf";
            this.btnAddManf.Size = new System.Drawing.Size(32, 24);
            this.btnAddManf.TabIndex = 7;
            this.btnAddManf.Text = "+";
            this.btnAddManf.UseVisualStyleBackColor = true;
            this.btnAddManf.Click += new System.EventHandler(this.btnAddManf_Click);
            // 
            // chkIsRawItem
            // 
            this.chkIsRawItem.AutoSize = true;
            this.chkIsRawItem.Location = new System.Drawing.Point(622, 646);
            this.chkIsRawItem.Name = "chkIsRawItem";
            this.chkIsRawItem.Size = new System.Drawing.Size(87, 22);
            this.chkIsRawItem.TabIndex = 79;
            this.chkIsRawItem.Text = "Raw Item";
            this.chkIsRawItem.UseVisualStyleBackColor = true;
            this.chkIsRawItem.Visible = false;
            this.chkIsRawItem.CheckedChanged += new System.EventHandler(this.chkIsRawItem_CheckedChanged);
            // 
            // errManf
            // 
            this.errManf.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errManf.ForeColor = System.Drawing.Color.Red;
            this.errManf.Location = new System.Drawing.Point(1260, 289);
            this.errManf.Name = "errManf";
            this.errManf.Size = new System.Drawing.Size(16, 17);
            this.errManf.TabIndex = 50;
            this.errManf.Text = "*";
            this.errManf.Visible = false;
            this.errManf.Click += new System.EventHandler(this.label15_Click);
            // 
            // cmbSubCategory
            // 
            this.cmbSubCategory.FormattingEnabled = true;
            this.cmbSubCategory.Location = new System.Drawing.Point(1011, 338);
            this.cmbSubCategory.Name = "cmbSubCategory";
            this.cmbSubCategory.Size = new System.Drawing.Size(248, 26);
            this.cmbSubCategory.TabIndex = 80;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(801, 341);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(131, 25);
            this.label16.TabIndex = 82;
            this.label16.Text = "Sub Category";
            // 
            // btnAddSubCategory
            // 
            this.btnAddSubCategory.Location = new System.Drawing.Point(1283, 335);
            this.btnAddSubCategory.Name = "btnAddSubCategory";
            this.btnAddSubCategory.Size = new System.Drawing.Size(32, 24);
            this.btnAddSubCategory.TabIndex = 81;
            this.btnAddSubCategory.Text = "+";
            this.btnAddSubCategory.UseVisualStyleBackColor = true;
            this.btnAddSubCategory.Click += new System.EventHandler(this.btnAddSubCategory_Click);
            // 
            // chkCheckStockOnSale
            // 
            this.chkCheckStockOnSale.AutoSize = true;
            this.chkCheckStockOnSale.Location = new System.Drawing.Point(622, 674);
            this.chkCheckStockOnSale.Name = "chkCheckStockOnSale";
            this.chkCheckStockOnSale.Size = new System.Drawing.Size(152, 22);
            this.chkCheckStockOnSale.TabIndex = 83;
            this.chkCheckStockOnSale.Text = "Check Stock on Sale";
            this.chkCheckStockOnSale.UseVisualStyleBackColor = true;
            this.chkCheckStockOnSale.Visible = false;
            // 
            // cmbItemType
            // 
            this.cmbItemType.FormattingEnabled = true;
            this.cmbItemType.Location = new System.Drawing.Point(285, 72);
            this.cmbItemType.Name = "cmbItemType";
            this.cmbItemType.Size = new System.Drawing.Size(248, 26);
            this.cmbItemType.TabIndex = 84;
            // 
            // errItemType
            // 
            this.errItemType.AutoSize = true;
            this.errItemType.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.errItemType.ForeColor = System.Drawing.Color.Red;
            this.errItemType.Location = new System.Drawing.Point(532, 72);
            this.errItemType.Name = "errItemType";
            this.errItemType.Size = new System.Drawing.Size(24, 29);
            this.errItemType.TabIndex = 87;
            this.errItemType.Text = "*";
            this.errItemType.Visible = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Tai Le", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(40, 70);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(98, 25);
            this.label15.TabIndex = 86;
            this.label15.Text = "Item Type";
            // 
            // frmItems
            // 
            this.AcceptButton = this.btnAddItem;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1351, 784);
            this.Controls.Add(this.cmbItemType);
            this.Controls.Add(this.errItemType);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.chkCheckStockOnSale);
            this.Controls.Add(this.cmbSubCategory);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.btnAddSubCategory);
            this.Controls.Add(this.chkIsRawItem);
            this.Controls.Add(this.cmbRacks);
            this.Controls.Add(this.cmbManufacturer);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.lblInvalidRetailPrice);
            this.Controls.Add(this.errRetailPrice);
            this.Controls.Add(this.txtRetailPrice);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.pnlOpeningStock);
            this.Controls.Add(this.lblInvlalidUnitCost);
            this.Controls.Add(this.lstSuppliers);
            this.Controls.Add(this.ErrMessage);
            this.Controls.Add(this.ErrUnitCostPrice);
            this.Controls.Add(this.ErrChemicalName);
            this.Controls.Add(this.errConvUnit);
            this.Controls.Add(this.ErrReOrderingLevel);
            this.Controls.Add(this.errCategory);
            this.Controls.Add(this.ErrUnit);
            this.Controls.Add(this.ErrSupplier);
            this.Controls.Add(this.ErrBarcode);
            this.Controls.Add(this.ErrItemName);
            this.Controls.Add(this.txtUnitCostPrice);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtGenericName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numConvUnit);
            this.Controls.Add(this.numReOrderingLevel);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtUnit);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnAddManf);
            this.Controls.Add(this.btnAddCategory);
            this.Controls.Add(this.btnAddRack);
            this.Controls.Add(this.btnAddSupplier);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtBarcode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnAddItem);
            this.Controls.Add(this.txtItemName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.errManf);
            this.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(565, 602);
            this.Name = "frmItems";
            this.Text = "Add New Item";
            this.Load += new System.EventHandler(this.frmItems_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numReOrderingLevel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOpeningStock)).EndInit();
            this.pnlOpeningStock.ResumeLayout(false);
            this.pnlOpeningStock.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numConvUnit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.Button btnAddItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnAddSupplier;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numReOrderingLevel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numOpeningStock;
        private System.Windows.Forms.TextBox txtGenericName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtUnitCostPrice;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label ErrItemName;
        private System.Windows.Forms.Label ErrBarcode;
        private System.Windows.Forms.Label ErrSupplier;
        private System.Windows.Forms.Label ErrUnit;
        private System.Windows.Forms.Label ErrReOrderingLevel;
        private System.Windows.Forms.Label ErrOpeningStock;
        private System.Windows.Forms.Label ErrChemicalName;
        private System.Windows.Forms.Label ErrUnitCostPrice;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Label ErrMessage;
        private System.Windows.Forms.ListBox lstSuppliers;
        private System.Windows.Forms.Label lblInvlalidUnitCost;
        private System.Windows.Forms.Panel pnlOpeningStock;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblInvalidRetailPrice;
        private System.Windows.Forms.Label errRetailPrice;
        private System.Windows.Forms.TextBox txtRetailPrice;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnAddCategory;
        private System.Windows.Forms.Label errCategory;
        private System.Windows.Forms.NumericUpDown numConvUnit;
        private System.Windows.Forms.Label errConvUnit;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cmbManufacturer;
        private System.Windows.Forms.ComboBox cmbRacks;
        private System.Windows.Forms.Button btnAddRack;
        private System.Windows.Forms.Button btnAddManf;
        private System.Windows.Forms.CheckBox chkIsRawItem;
        private System.Windows.Forms.Label errManf;
        private System.Windows.Forms.ComboBox cmbSubCategory;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnAddSubCategory;
        private System.Windows.Forms.CheckBox chkCheckStockOnSale;
        private System.Windows.Forms.ComboBox cmbItemType;
        private System.Windows.Forms.Label errItemType;
        private System.Windows.Forms.Label label15;
    }
}