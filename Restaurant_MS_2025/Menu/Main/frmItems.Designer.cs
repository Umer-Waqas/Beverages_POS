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
            label1 = new Label();
            txtItemName = new TextBox();
            btnAddItem = new Button();
            label2 = new Label();
            txtBarcode = new TextBox();
            label3 = new Label();
            label4 = new Label();
            btnAddSupplier = new Button();
            txtUnit = new TextBox();
            label5 = new Label();
            numReOrderingLevel = new NumericUpDown();
            label6 = new Label();
            label7 = new Label();
            numOpeningStock = new NumericUpDown();
            txtGenericName = new TextBox();
            label8 = new Label();
            txtUnitCostPrice = new TextBox();
            label9 = new Label();
            ErrItemName = new Label();
            ErrBarcode = new Label();
            ErrSupplier = new Label();
            ErrUnit = new Label();
            ErrReOrderingLevel = new Label();
            ErrOpeningStock = new Label();
            ErrChemicalName = new Label();
            ErrUnitCostPrice = new Label();
            btnClear = new Button();
            ErrMessage = new Label();
            lstSuppliers = new ListBox();
            lblInvlalidUnitCost = new Label();
            pnlOpeningStock = new Panel();
            btnCancel = new Button();
            lblInvalidRetailPrice = new Label();
            errRetailPrice = new Label();
            txtRetailPrice = new TextBox();
            label13 = new Label();
            cmbCategory = new ComboBox();
            label11 = new Label();
            btnAddCategory = new Button();
            errCategory = new Label();
            numConvUnit = new NumericUpDown();
            errConvUnit = new Label();
            label14 = new Label();
            label12 = new Label();
            cmbManufacturer = new ComboBox();
            cmbRacks = new ComboBox();
            btnAddRack = new Button();
            btnAddManf = new Button();
            chkIsRawItem = new CheckBox();
            errManf = new Label();
            cmbSubCategory = new ComboBox();
            label16 = new Label();
            btnAddSubCategory = new Button();
            chkCheckStockOnSale = new CheckBox();
            cmbItemType = new ComboBox();
            errItemType = new Label();
            label15 = new Label();
            ((ISupportInitialize)numReOrderingLevel).BeginInit();
            ((ISupportInitialize)numOpeningStock).BeginInit();
            pnlOpeningStock.SuspendLayout();
            ((ISupportInitialize)numConvUnit).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Tai Le", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(40, 166);
            label1.Name = "label1";
            label1.Size = new Size(108, 25);
            label1.TabIndex = 0;
            label1.Text = "Item Name";
            // 
            // txtItemName
            // 
            txtItemName.Location = new Point(286, 161);
            txtItemName.MaxLength = 599;
            txtItemName.Name = "txtItemName";
            txtItemName.Size = new Size(248, 25);
            txtItemName.TabIndex = 1;
            // 
            // btnAddItem
            // 
            btnAddItem.Location = new Point(421, 561);
            btnAddItem.Name = "btnAddItem";
            btnAddItem.Size = new Size(174, 51);
            btnAddItem.TabIndex = 16;
            btnAddItem.Text = "Add";
            btnAddItem.UseVisualStyleBackColor = true;
            btnAddItem.Click += btnAddItem_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Tai Le", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(801, 75);
            label2.Name = "label2";
            label2.Size = new Size(65, 25);
            label2.TabIndex = 4;
            label2.Text = "Rack#";
            // 
            // txtBarcode
            // 
            txtBarcode.Location = new Point(1011, 109);
            txtBarcode.MaxLength = 599;
            txtBarcode.Name = "txtBarcode";
            txtBarcode.Size = new Size(246, 25);
            txtBarcode.TabIndex = 3;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Tai Le", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(801, 116);
            label3.Name = "label3";
            label3.Size = new Size(83, 25);
            label3.TabIndex = 6;
            label3.Text = "Barcode";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Tai Le", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(801, 154);
            label4.Name = "label4";
            label4.Size = new Size(93, 25);
            label4.TabIndex = 8;
            label4.Text = "Suppliers";
            // 
            // btnAddSupplier
            // 
            btnAddSupplier.Location = new Point(1283, 135);
            btnAddSupplier.Name = "btnAddSupplier";
            btnAddSupplier.Size = new Size(32, 25);
            btnAddSupplier.TabIndex = 5;
            btnAddSupplier.Text = "+";
            btnAddSupplier.UseVisualStyleBackColor = true;
            btnAddSupplier.Click += btnAddSupplier_Click;
            // 
            // txtUnit
            // 
            txtUnit.Location = new Point(286, 232);
            txtUnit.Name = "txtUnit";
            txtUnit.Size = new Size(248, 25);
            txtUnit.TabIndex = 8;
            txtUnit.Text = "Pack";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Tai Le", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(38, 232);
            label5.Name = "label5";
            label5.Size = new Size(205, 25);
            label5.TabIndex = 11;
            label5.Text = "Unit(Strips, Boxes etc.)";
            // 
            // numReOrderingLevel
            // 
            numReOrderingLevel.Location = new Point(292, 313);
            numReOrderingLevel.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numReOrderingLevel.Name = "numReOrderingLevel";
            numReOrderingLevel.Size = new Size(138, 25);
            numReOrderingLevel.TabIndex = 10;
            numReOrderingLevel.ThousandsSeparator = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft Tai Le", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(40, 314);
            label6.Name = "label6";
            label6.Size = new Size(170, 25);
            label6.TabIndex = 14;
            label6.Text = "Re-Ordering Level";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(10, -4);
            label7.Name = "label7";
            label7.Size = new Size(142, 25);
            label7.TabIndex = 16;
            label7.Text = "Opening Stock";
            // 
            // numOpeningStock
            // 
            numOpeningStock.Location = new Point(242, -4);
            numOpeningStock.Name = "numOpeningStock";
            numOpeningStock.Size = new Size(113, 33);
            numOpeningStock.TabIndex = 12;
            // 
            // txtGenericName
            // 
            txtGenericName.Location = new Point(744, 696);
            txtGenericName.Name = "txtGenericName";
            txtGenericName.Size = new Size(246, 25);
            txtGenericName.TabIndex = 13;
            txtGenericName.Visible = false;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(615, 699);
            label8.Name = "label8";
            label8.Size = new Size(94, 18);
            label8.TabIndex = 17;
            label8.Text = "Generic Name";
            label8.Visible = false;
            // 
            // txtUnitCostPrice
            // 
            txtUnitCostPrice.Location = new Point(292, 443);
            txtUnitCostPrice.Name = "txtUnitCostPrice";
            txtUnitCostPrice.Size = new Size(246, 25);
            txtUnitCostPrice.TabIndex = 14;
            txtUnitCostPrice.KeyPress += txtUnitCostPrice_KeyPress;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Microsoft Tai Le", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.Location = new Point(42, 438);
            label9.Name = "label9";
            label9.Size = new Size(140, 25);
            label9.TabIndex = 19;
            label9.Text = "Unit Cost Price";
            // 
            // ErrItemName
            // 
            ErrItemName.AutoSize = true;
            ErrItemName.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ErrItemName.ForeColor = Color.Red;
            ErrItemName.Location = new Point(533, 165);
            ErrItemName.Name = "ErrItemName";
            ErrItemName.Size = new Size(24, 29);
            ErrItemName.TabIndex = 24;
            ErrItemName.Text = "*";
            ErrItemName.Visible = false;
            // 
            // ErrBarcode
            // 
            ErrBarcode.AutoSize = true;
            ErrBarcode.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ErrBarcode.ForeColor = Color.Red;
            ErrBarcode.Location = new Point(1260, 109);
            ErrBarcode.Name = "ErrBarcode";
            ErrBarcode.Size = new Size(24, 29);
            ErrBarcode.TabIndex = 26;
            ErrBarcode.Text = "*";
            ErrBarcode.Visible = false;
            // 
            // ErrSupplier
            // 
            ErrSupplier.AutoSize = true;
            ErrSupplier.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ErrSupplier.ForeColor = Color.Red;
            ErrSupplier.Location = new Point(1260, 154);
            ErrSupplier.Name = "ErrSupplier";
            ErrSupplier.Size = new Size(24, 29);
            ErrSupplier.TabIndex = 27;
            ErrSupplier.Text = "*";
            ErrSupplier.Visible = false;
            // 
            // ErrUnit
            // 
            ErrUnit.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ErrUnit.ForeColor = Color.Red;
            ErrUnit.Location = new Point(533, 234);
            ErrUnit.Name = "ErrUnit";
            ErrUnit.Size = new Size(14, 13);
            ErrUnit.TabIndex = 28;
            ErrUnit.Text = "*";
            ErrUnit.TextAlign = ContentAlignment.MiddleLeft;
            ErrUnit.Visible = false;
            // 
            // ErrReOrderingLevel
            // 
            ErrReOrderingLevel.AutoSize = true;
            ErrReOrderingLevel.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ErrReOrderingLevel.ForeColor = Color.Red;
            ErrReOrderingLevel.Location = new Point(431, 314);
            ErrReOrderingLevel.Name = "ErrReOrderingLevel";
            ErrReOrderingLevel.Size = new Size(24, 29);
            ErrReOrderingLevel.TabIndex = 29;
            ErrReOrderingLevel.Text = "*";
            ErrReOrderingLevel.Visible = false;
            // 
            // ErrOpeningStock
            // 
            ErrOpeningStock.AutoSize = true;
            ErrOpeningStock.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ErrOpeningStock.ForeColor = Color.Red;
            ErrOpeningStock.Location = new Point(252, 4);
            ErrOpeningStock.Name = "ErrOpeningStock";
            ErrOpeningStock.Size = new Size(24, 29);
            ErrOpeningStock.TabIndex = 30;
            ErrOpeningStock.Text = "*";
            ErrOpeningStock.Visible = false;
            // 
            // ErrChemicalName
            // 
            ErrChemicalName.AutoSize = true;
            ErrChemicalName.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ErrChemicalName.ForeColor = Color.Red;
            ErrChemicalName.Location = new Point(750, 642);
            ErrChemicalName.Name = "ErrChemicalName";
            ErrChemicalName.Size = new Size(24, 29);
            ErrChemicalName.TabIndex = 31;
            ErrChemicalName.Text = "*";
            ErrChemicalName.Visible = false;
            // 
            // ErrUnitCostPrice
            // 
            ErrUnitCostPrice.AutoSize = true;
            ErrUnitCostPrice.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ErrUnitCostPrice.ForeColor = Color.Red;
            ErrUnitCostPrice.Location = new Point(541, 440);
            ErrUnitCostPrice.Name = "ErrUnitCostPrice";
            ErrUnitCostPrice.Size = new Size(24, 29);
            ErrUnitCostPrice.TabIndex = 32;
            ErrUnitCostPrice.Text = "*";
            ErrUnitCostPrice.Visible = false;
            // 
            // btnClear
            // 
            btnClear.Location = new Point(630, 561);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(174, 51);
            btnClear.TabIndex = 17;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // ErrMessage
            // 
            ErrMessage.AutoSize = true;
            ErrMessage.Font = new Font("Microsoft Sans Serif", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ErrMessage.ForeColor = Color.Coral;
            ErrMessage.Location = new Point(43, 10);
            ErrMessage.Name = "ErrMessage";
            ErrMessage.Size = new Size(330, 18);
            ErrMessage.TabIndex = 34;
            ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            ErrMessage.Visible = false;
            // 
            // lstSuppliers
            // 
            lstSuppliers.FormattingEnabled = true;
            lstSuppliers.ItemHeight = 18;
            lstSuppliers.Location = new Point(1011, 135);
            lstSuppliers.Name = "lstSuppliers";
            lstSuppliers.SelectionMode = SelectionMode.MultiSimple;
            lstSuppliers.Size = new Size(246, 130);
            lstSuppliers.TabIndex = 4;
            // 
            // lblInvlalidUnitCost
            // 
            lblInvlalidUnitCost.AutoSize = true;
            lblInvlalidUnitCost.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblInvlalidUnitCost.ForeColor = Color.Red;
            lblInvlalidUnitCost.Location = new Point(563, 443);
            lblInvlalidUnitCost.Name = "lblInvlalidUnitCost";
            lblInvlalidUnitCost.Size = new Size(102, 17);
            lblInvlalidUnitCost.TabIndex = 73;
            lblInvlalidUnitCost.Text = "Invalid Number";
            lblInvlalidUnitCost.Visible = false;
            // 
            // pnlOpeningStock
            // 
            pnlOpeningStock.Controls.Add(numOpeningStock);
            pnlOpeningStock.Controls.Add(label7);
            pnlOpeningStock.Controls.Add(ErrOpeningStock);
            pnlOpeningStock.Font = new Font("Microsoft Tai Le", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            pnlOpeningStock.Location = new Point(32, 392);
            pnlOpeningStock.Name = "pnlOpeningStock";
            pnlOpeningStock.Size = new Size(508, 30);
            pnlOpeningStock.TabIndex = 74;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(837, 561);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(174, 51);
            btnCancel.TabIndex = 18;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // lblInvalidRetailPrice
            // 
            lblInvalidRetailPrice.AutoSize = true;
            lblInvalidRetailPrice.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblInvalidRetailPrice.ForeColor = Color.Red;
            lblInvalidRetailPrice.Location = new Point(563, 351);
            lblInvalidRetailPrice.Name = "lblInvalidRetailPrice";
            lblInvalidRetailPrice.Size = new Size(102, 17);
            lblInvalidRetailPrice.TabIndex = 78;
            lblInvalidRetailPrice.Text = "Invalid Number";
            lblInvalidRetailPrice.Visible = false;
            // 
            // errRetailPrice
            // 
            errRetailPrice.AutoSize = true;
            errRetailPrice.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            errRetailPrice.ForeColor = Color.Red;
            errRetailPrice.Location = new Point(543, 350);
            errRetailPrice.Name = "errRetailPrice";
            errRetailPrice.Size = new Size(24, 29);
            errRetailPrice.TabIndex = 77;
            errRetailPrice.Text = "*";
            errRetailPrice.Visible = false;
            // 
            // txtRetailPrice
            // 
            txtRetailPrice.Location = new Point(289, 351);
            txtRetailPrice.Name = "txtRetailPrice";
            txtRetailPrice.Size = new Size(246, 25);
            txtRetailPrice.TabIndex = 11;
            txtRetailPrice.KeyPress += txtRetailPrice_KeyPress;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Microsoft Tai Le", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label13.Location = new Point(40, 351);
            label13.Name = "label13";
            label13.Size = new Size(108, 25);
            label13.TabIndex = 76;
            label13.Text = "Retail Price";
            // 
            // cmbCategory
            // 
            cmbCategory.FormattingEnabled = true;
            cmbCategory.Location = new Point(286, 118);
            cmbCategory.Name = "cmbCategory";
            cmbCategory.Size = new Size(248, 26);
            cmbCategory.TabIndex = 6;
            cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Microsoft Tai Le", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label11.Location = new Point(41, 116);
            label11.Name = "label11";
            label11.Size = new Size(92, 25);
            label11.TabIndex = 11;
            label11.Text = "Category";
            // 
            // btnAddCategory
            // 
            btnAddCategory.Location = new Point(560, 115);
            btnAddCategory.Name = "btnAddCategory";
            btnAddCategory.Size = new Size(32, 24);
            btnAddCategory.TabIndex = 7;
            btnAddCategory.Text = "+";
            btnAddCategory.UseVisualStyleBackColor = true;
            btnAddCategory.Click += btnAddCategory_Click;
            // 
            // errCategory
            // 
            errCategory.AutoSize = true;
            errCategory.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            errCategory.ForeColor = Color.Red;
            errCategory.Location = new Point(533, 118);
            errCategory.Name = "errCategory";
            errCategory.Size = new Size(24, 29);
            errCategory.TabIndex = 28;
            errCategory.Text = "*";
            errCategory.Visible = false;
            // 
            // numConvUnit
            // 
            numConvUnit.Location = new Point(291, 272);
            numConvUnit.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            numConvUnit.Name = "numConvUnit";
            numConvUnit.Size = new Size(138, 25);
            numConvUnit.TabIndex = 9;
            numConvUnit.ThousandsSeparator = true;
            numConvUnit.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // errConvUnit
            // 
            errConvUnit.AutoSize = true;
            errConvUnit.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            errConvUnit.ForeColor = Color.Red;
            errConvUnit.Location = new Point(431, 273);
            errConvUnit.Name = "errConvUnit";
            errConvUnit.Size = new Size(24, 29);
            errConvUnit.TabIndex = 29;
            errConvUnit.Text = "*";
            errConvUnit.Visible = false;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Microsoft Tai Le", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label14.Location = new Point(40, 279);
            label14.Name = "label14";
            label14.Size = new Size(152, 25);
            label14.TabIndex = 11;
            label14.Text = "Conversion Unit";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Microsoft Tai Le", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label12.Location = new Point(801, 289);
            label12.Name = "label12";
            label12.Size = new Size(129, 25);
            label12.TabIndex = 11;
            label12.Text = "Manufacturer";
            // 
            // cmbManufacturer
            // 
            cmbManufacturer.FormattingEnabled = true;
            cmbManufacturer.Location = new Point(1011, 286);
            cmbManufacturer.Name = "cmbManufacturer";
            cmbManufacturer.Size = new Size(248, 26);
            cmbManufacturer.TabIndex = 6;
            // 
            // cmbRacks
            // 
            cmbRacks.FormattingEnabled = true;
            cmbRacks.Location = new Point(1009, 72);
            cmbRacks.Name = "cmbRacks";
            cmbRacks.Size = new Size(248, 26);
            cmbRacks.TabIndex = 2;
            // 
            // btnAddRack
            // 
            btnAddRack.Location = new Point(1283, 69);
            btnAddRack.Name = "btnAddRack";
            btnAddRack.Size = new Size(32, 25);
            btnAddRack.TabIndex = 5;
            btnAddRack.Text = "+";
            btnAddRack.UseVisualStyleBackColor = true;
            btnAddRack.Click += btnAddRack_Click;
            // 
            // btnAddManf
            // 
            btnAddManf.Location = new Point(1281, 288);
            btnAddManf.Name = "btnAddManf";
            btnAddManf.Size = new Size(32, 24);
            btnAddManf.TabIndex = 7;
            btnAddManf.Text = "+";
            btnAddManf.UseVisualStyleBackColor = true;
            btnAddManf.Click += btnAddManf_Click;
            // 
            // chkIsRawItem
            // 
            chkIsRawItem.AutoSize = true;
            chkIsRawItem.Location = new Point(622, 646);
            chkIsRawItem.Name = "chkIsRawItem";
            chkIsRawItem.Size = new Size(87, 22);
            chkIsRawItem.TabIndex = 79;
            chkIsRawItem.Text = "Raw Item";
            chkIsRawItem.UseVisualStyleBackColor = true;
            chkIsRawItem.Visible = false;
            chkIsRawItem.CheckedChanged += chkIsRawItem_CheckedChanged;
            // 
            // errManf
            // 
            errManf.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            errManf.ForeColor = Color.Red;
            errManf.Location = new Point(1260, 289);
            errManf.Name = "errManf";
            errManf.Size = new Size(16, 17);
            errManf.TabIndex = 50;
            errManf.Text = "*";
            errManf.Visible = false;
            errManf.Click += label15_Click;
            // 
            // cmbSubCategory
            // 
            cmbSubCategory.FormattingEnabled = true;
            cmbSubCategory.Location = new Point(1011, 338);
            cmbSubCategory.Name = "cmbSubCategory";
            cmbSubCategory.Size = new Size(248, 26);
            cmbSubCategory.TabIndex = 80;
            cmbSubCategory.Visible = false;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Microsoft Tai Le", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label16.Location = new Point(801, 341);
            label16.Name = "label16";
            label16.Size = new Size(131, 25);
            label16.TabIndex = 82;
            label16.Text = "Sub Category";
            label16.Visible = false;
            // 
            // btnAddSubCategory
            // 
            btnAddSubCategory.Location = new Point(1283, 335);
            btnAddSubCategory.Name = "btnAddSubCategory";
            btnAddSubCategory.Size = new Size(32, 24);
            btnAddSubCategory.TabIndex = 81;
            btnAddSubCategory.Text = "+";
            btnAddSubCategory.UseVisualStyleBackColor = true;
            btnAddSubCategory.Visible = false;
            btnAddSubCategory.Click += btnAddSubCategory_Click;
            // 
            // chkCheckStockOnSale
            // 
            chkCheckStockOnSale.AutoSize = true;
            chkCheckStockOnSale.Location = new Point(622, 674);
            chkCheckStockOnSale.Name = "chkCheckStockOnSale";
            chkCheckStockOnSale.Size = new Size(152, 22);
            chkCheckStockOnSale.TabIndex = 83;
            chkCheckStockOnSale.Text = "Check Stock on Sale";
            chkCheckStockOnSale.UseVisualStyleBackColor = true;
            chkCheckStockOnSale.Visible = false;
            // 
            // cmbItemType
            // 
            cmbItemType.FormattingEnabled = true;
            cmbItemType.Location = new Point(285, 72);
            cmbItemType.Name = "cmbItemType";
            cmbItemType.Size = new Size(248, 26);
            cmbItemType.TabIndex = 84;
            // 
            // errItemType
            // 
            errItemType.AutoSize = true;
            errItemType.Font = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold, GraphicsUnit.Point, 0);
            errItemType.ForeColor = Color.Red;
            errItemType.Location = new Point(532, 72);
            errItemType.Name = "errItemType";
            errItemType.Size = new Size(24, 29);
            errItemType.TabIndex = 87;
            errItemType.Text = "*";
            errItemType.Visible = false;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Microsoft Tai Le", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label15.Location = new Point(40, 70);
            label15.Name = "label15";
            label15.Size = new Size(98, 25);
            label15.TabIndex = 86;
            label15.Text = "Item Type";
            // 
            // frmItems
            // 
            AcceptButton = btnAddItem;
            AutoScaleDimensions = new SizeF(8F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.White;
            CancelButton = btnCancel;
            ClientSize = new Size(1351, 784);
            Controls.Add(cmbItemType);
            Controls.Add(errItemType);
            Controls.Add(label15);
            Controls.Add(chkCheckStockOnSale);
            Controls.Add(cmbSubCategory);
            Controls.Add(label16);
            Controls.Add(btnAddSubCategory);
            Controls.Add(chkIsRawItem);
            Controls.Add(cmbRacks);
            Controls.Add(cmbManufacturer);
            Controls.Add(cmbCategory);
            Controls.Add(lblInvalidRetailPrice);
            Controls.Add(errRetailPrice);
            Controls.Add(txtRetailPrice);
            Controls.Add(label13);
            Controls.Add(pnlOpeningStock);
            Controls.Add(lblInvlalidUnitCost);
            Controls.Add(lstSuppliers);
            Controls.Add(ErrMessage);
            Controls.Add(ErrUnitCostPrice);
            Controls.Add(ErrChemicalName);
            Controls.Add(errConvUnit);
            Controls.Add(ErrReOrderingLevel);
            Controls.Add(errCategory);
            Controls.Add(ErrUnit);
            Controls.Add(ErrSupplier);
            Controls.Add(ErrBarcode);
            Controls.Add(ErrItemName);
            Controls.Add(txtUnitCostPrice);
            Controls.Add(label9);
            Controls.Add(txtGenericName);
            Controls.Add(label8);
            Controls.Add(label6);
            Controls.Add(numConvUnit);
            Controls.Add(numReOrderingLevel);
            Controls.Add(label12);
            Controls.Add(txtUnit);
            Controls.Add(label11);
            Controls.Add(label14);
            Controls.Add(label5);
            Controls.Add(btnAddManf);
            Controls.Add(btnAddCategory);
            Controls.Add(btnAddRack);
            Controls.Add(btnAddSupplier);
            Controls.Add(label4);
            Controls.Add(txtBarcode);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(btnCancel);
            Controls.Add(btnClear);
            Controls.Add(btnAddItem);
            Controls.Add(txtItemName);
            Controls.Add(label1);
            Controls.Add(errManf);
            Font = new Font("Microsoft Tai Le", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MinimumSize = new Size(565, 602);
            Name = "frmItems";
            Text = "Add New Item";
            Load += frmItems_Load;
            ((ISupportInitialize)numReOrderingLevel).EndInit();
            ((ISupportInitialize)numOpeningStock).EndInit();
            pnlOpeningStock.ResumeLayout(false);
            pnlOpeningStock.PerformLayout();
            ((ISupportInitialize)numConvUnit).EndInit();
            ResumeLayout(false);
            PerformLayout();

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