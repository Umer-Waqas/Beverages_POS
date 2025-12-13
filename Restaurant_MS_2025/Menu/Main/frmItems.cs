
namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmItems : Form
    {
        private UnitOfWork unitOfWork;
        public long selectedCategoryId = 0;
        List<Supplier> LinkedSuppliers = new List<Supplier>();
        DateTime actionTime;
        private int ItemId = 0;
        public frmItems()
        {
            InitializeComponent();
        }

        public frmItems(int ItemId)
        {
            InitializeComponent();
            this.ItemId = ItemId;
        }
        private void frmItems_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddItem, btnCancel, btnClear, btnAddCategory, btnAddManf });
            SharedFunctions.SetSmallButtonsStyle(new[] { btnAddSupplier, btnAddRack, btnAddSubCategory });
            using (unitOfWork = new UnitOfWork())
            {
                LoadItemTypes(unitOfWork);
                LoadCategories(unitOfWork);
                LoadManufacturers(unitOfWork);
                LoadRacks(unitOfWork);
                if (this.ItemId > 0)
                {
                    lstSuppliers.SelectedIndex = -1;
                    LoadItemData(this.ItemId, unitOfWork);
                }
            }

            if (this.ItemId > 0)
            {

                pnlOpeningStock.Visible = false;
                btnAddItem.Text = "Update";
            }
        }

        private void LoadRacks(UnitOfWork _unitOfWork)
        {
            //List<> rs = _unitOfWork.RackRepository.GetAllActiveRacks();
            //Rack r = new Rack();
            //r.RackId = 0;
            //r.Name = "Select Rack";
            //rs.Insert(0, r);
            //cmbRacks.DataSource = rs;
            //cmbRacks.DisplayMember = "Name";
            //cmbRacks.ValueMember = "RackId";
        }

        private void LoadItemTypes(UnitOfWork _unitOfWork)
        {
            List<SelectListVM> rs = _unitOfWork.ItemTypeRepository.GetItemTypesSelectList();
            SelectListVM r = new SelectListVM();
            r.Value = 0;
            r.Text = "Select Item Type";
            rs.Insert(0, r);
            cmbItemType.DataSource = rs;
            cmbItemType.DisplayMember = "Text";
            cmbItemType.ValueMember = "Value";
        }

        private void LoadCategories(UnitOfWork p_unitOfWork)
        {
            try
            {
                List<SelectListVM> cts = new List<SelectListVM>();
                cts = p_unitOfWork.CategoryRepository.GetCategoriesSelectList();

                SelectListVM c = new SelectListVM();
                c.Value = 0;
                c.Text = "Select Category";
                cts.Insert(0, c);
                cmbCategory.DataSource = cts.ToList();
                cmbCategory.ValueMember = "Value";
                cmbCategory.DisplayMember = "Text";


            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading categories, please try again.", "Categories not laoded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void LoadSubCategories(UnitOfWork p_unitOfWork, long ParentId)
        {
            try
            {
                List<Category> cts = new List<Category>();
                cts = p_unitOfWork.CategoryRepository.GetAllActiveSubCategories(ParentId);
                Category c = new Category();
                c.CategoryId = 0;
                c.Name = "Select Sub Category";
                cts.Insert(0, c);
                cmbSubCategory.DataSource = cts;
                cmbSubCategory.ValueMember = "CategoryId";
                cmbSubCategory.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading sub categories, please try again.", "Sub Categories not laoded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void LoadManufacturers(UnitOfWork p_unitOfWork)
        {
            try
            {
                List<Manufacturer> cts = new List<Manufacturer>();
                cts = p_unitOfWork.ManufacturerRepository.GetAllActiveManufacturers();

                Manufacturer c = new Manufacturer();
                c.ManufacturerId = 0;
                c.Name = "Select Manufacturer";
                cts.Insert(0, c);
                cmbManufacturer.DataSource = cts;
                cmbManufacturer.ValueMember = "ManufacturerId";
                cmbManufacturer.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading manufacturers, please try again.", "Manufacturers not laoded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void LoadItemData(int ItemId, UnitOfWork p_unitOfWork)
        {
            try
            {
                Item LoadedItem = new Item();
                LoadedItem = p_unitOfWork.ItemRspository.GetItemData_ByITemId(ItemId);
                txtItemName.Text = LoadedItem.ItemName;
                if (LoadedItem.Rack != null)
                {
                    cmbRacks.SelectedValue = LoadedItem.Rack.RackId;
                }
                else
                {
                    cmbRacks.SelectedValue = 0;
                }
                txtBarcode.Text = LoadedItem.Barcode;

                List<Supplier> objSuppliers = LoadedItem.Suppliers.ToList();
                foreach (Supplier s2 in objSuppliers)
                {
                    LinkedSuppliers.Add(s2);
                    lstSuppliers.SelectedIndex = lstSuppliers.FindStringExact(s2.Name);
                }
                cmbCategory.SelectedValue = LoadedItem.Category == null ? 0 : LoadedItem.CategoryId;
                //cmbSubCategory.SelectedValue = LoadedItem.Category1 == null ? 0 : LoadedItem.SubCategoryId;
                cmbManufacturer.SelectedValue = LoadedItem.Manufacturer == null ? 0 : LoadedItem.ManufacturerId;
                txtUnit.Text = LoadedItem.Unit.ToString();
                numConvUnit.Value = LoadedItem.ConversionUnit;
                numReOrderingLevel.Value = LoadedItem.ReOrderingLevel;
                txtRetailPrice.Text = LoadedItem.RetailPrice.ToString();
                numOpeningStock.Value = LoadedItem.OpeningStock;
                txtUnitCostPrice.Text = LoadedItem.UnitCostPrice.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading item data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Clear()
        {
            //if (ItemId > 0)
            //{
            //    DialogResult rs = MessageBox.Show("You Were About to Update Product. If You Clear, New Product Will be Saved, STILL YOU WANT TO CLEAR??", "Please Make Sure", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            //    if (rs != DialogResult.Yes)
            //    {
            //        return;
            //    }
            //}

            btnAddItem.Text = "Add";
            ItemId = 0;
            txtItemName.Text = "";
            cmbRacks.SelectedIndex = 0;
            txtBarcode.Text = "";
            lstSuppliers.SelectedIndex = -1;
            cmbCategory.SelectedIndex = 0;
            cmbManufacturer.SelectedIndex = 0;
            txtUnit.Text = "";
            numConvUnit.Value = 1;
            numReOrderingLevel.Value = 0;
            numOpeningStock.Value = 0;
            txtGenericName.Text = "";
            txtUnitCostPrice.Text = "";

            ErrMessage.Visible = false;
            ErrItemName.Visible = false;
            //            ErrRackNo.Visible = false;
            ErrBarcode.Visible = false;
            ErrSupplier.Visible = false;
            ErrUnit.Visible = false;
            ErrReOrderingLevel.Visible = false;
            ErrOpeningStock.Visible = false;
            ErrChemicalName.Visible = false;
            ErrUnitCostPrice.Visible = false;
            chkCheckStockOnSale.Checked = false;
            chkIsRawItem.Checked = false;
        }
        private bool IsValidInput()
        {
            bool ErrFound = false;
            if (cmbItemType.SelectedIndex <= 0)
            {
                errItemType.Visible = true;
                ErrFound = true;
            }
            else
            {
                errItemType.Visible = false;
            }

            if (string.IsNullOrEmpty(txtItemName.Text.Trim()))
            {
                ErrItemName.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrItemName.Visible = false;
            }
            if (string.IsNullOrEmpty(txtUnit.Text.Trim()))
            {
                ErrUnit.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrUnit.Visible = false;
            }
            if (numConvUnit.Value < 1)
            {
                errConvUnit.Visible = true;
                ErrFound = true;
            }
            else
            {
                errConvUnit.Visible = false;
            }
            //if (string.IsNullOrEmpty(txtRackNo.Text.Trim()))
            //{
            //    ErrRackNo.Visible = true;
            //    ErrFound = true;
            //}
            //else
            //{
            //    ErrRackNo.Visible = false;
            //}
            //if (string.IsNullOrEmpty(txtBarcode.Text.Trim()))
            //{
            //    ErrBarcode.Visible = true;
            //    ErrFound = true;
            //}
            //else
            //{
            //    ErrBarcode.Visible = false;
            //}
            if (txtUnitCostPrice.Text.Trim() == "")
            {
                txtUnitCostPrice.Text = "0";
            }
            else
            {
                double unitCostPrice = 0;
                bool success = double.TryParse(txtUnitCostPrice.Text, out unitCostPrice);
                if (!success)
                {
                    lblInvlalidUnitCost.Visible = true;
                    ErrFound = true;
                }
                else
                {
                    lblInvlalidUnitCost.Visible = false;
                }
            }
            if (txtRetailPrice.Text.Trim() == "")
            {
                txtRetailPrice.Text = "0";
            }
            else
            {
                double rp = 0;
                bool success = double.TryParse(txtRetailPrice.Text, out rp);
                if (!success)
                {
                    lblInvalidRetailPrice.Visible = true;
                    ErrFound = true;
                }
                else
                {
                    lblInvalidRetailPrice.Visible = false;
                }
            }

            if (cmbCategory.SelectedIndex <= 0)
            {
                errCategory.Visible = true;
                ErrFound = true;
            }
            else
            {
                errCategory.Visible = false;
            }

            //if (cmbManufacturer.SelectedIndex <= 0)
            //{
            //    errManf.Visible = true;
            //    ErrFound = true;
            //}
            //else
            //{
            //    errManf.Visible = false;
            //}

            if (!ErrFound)
            {
                ErrMessage.Visible = false;
                return true;
            }
            else
            {
                ErrMessage.Visible = true;
                return false;
            }
        }
        private void FillObject(Item objItem)
        {
            objItem.ItemTypeId = Convert.ToInt32(cmbItemType.SelectedValue);
            objItem.ItemName = txtItemName.Text;
            objItem.IsRawItem = chkIsRawItem.Checked;
            objItem.CheckStockOnSale = chkCheckStockOnSale.Checked;
            objItem.Barcode = txtBarcode.Text;
            objItem.Rack = unitOfWork.RackRepository.GetById(Convert.ToInt32(cmbRacks.SelectedValue));
            objItem.Unit = txtUnit.Text.Trim();
            int convUnit = Convert.ToInt32(numConvUnit.Value);
            if (this.ItemId > 0 && convUnit != objItem.ConversionUnit)
            {
                DialogResult res = MessageBox.Show("New conversion unit would not apply on old stocks, consumptions, invoices, adjustments, audits." + Environment.NewLine + " Are you sure you want to update conversion unit?", "Please Make Sure", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    objItem.ConversionUnit = convUnit;
                }
            }
            else
            {
                objItem.ConversionUnit = convUnit;
            }
            objItem.ReOrderingLevel = (int)numReOrderingLevel.Value;
            objItem.RetailPrice = double.Parse(txtRetailPrice.Text);
            objItem.OpeningStock = (int)numOpeningStock.Value;
            objItem.UnitCostPrice = double.Parse(txtUnitCostPrice.Text);
            if (this.ItemId <= 0)
            {
                objItem.IsActive = true;
                objItem.IsNew = true;
                objItem.CreatedAt = DateTime.Now;
                objItem.IsSyncable = true;
            }
            objItem.UpdatedAt = DateTime.Now;
            objItem.CategoryId = Convert.ToInt64(cmbCategory.SelectedValue);
            long subCategoryId = Convert.ToInt64(cmbSubCategory.SelectedValue);
            if (subCategoryId > 0)
            {
                objItem.SubCategoryId = subCategoryId;
            }
            if (cmbManufacturer.SelectedIndex > 0)
            {
                objItem.ManufacturerId = Convert.ToInt64(cmbManufacturer.SelectedValue);
            }
        }
        private void LoadSuppliers(UnitOfWork p_unitOfWork)
        {
            List<Supplier> suppliers = new List<Supplier>();
            suppliers = unitOfWork.SupplierRepository.GetAll().ToList();
            lstSuppliers.DataSource = suppliers;
            lstSuppliers.ValueMember = "SupplierID";
            lstSuppliers.DisplayMember = "Name";
        }

        private void RefreshBulkItems(UnitOfWork _unitOfwork)
        {
            //SharedVariables.BulkLoadedItemsList = _unitOfwork.ItemRspository.GetBulkAllActiveItems();
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    if (this.ItemId > 0)
                    {
                        UpdateItem();
                        return;
                    }
                    InsertItem();
                }
            }
            catch (Exception ex)
            {
                //if (ex.InnerException != null)
                //{
                //    if (ex.InnerException.InnerException.Message.Contains("IX_ItemName"))
                //    {
                //        MessageBox.Show("Item with Given Name Already Exists, Please Specify Another Name", "Item Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        return;
                //    }
                //    else if (ex.InnerException.InnerException.Message.Contains("IX_Barcode"))
                //    {
                //        MessageBox.Show("Item with Given Barcode Already Exists, Please Specify Another Barcode", "Barcode Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //        return;
                //    }
                //}
                // MessageBox.Show(ex.InnerException.InnerException.Message, "Inner exception details");
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void InsertItem()
        {
            using (unitOfWork = new UnitOfWork())
            {
                Item objItem = new Item();
                FillObject(objItem);
                int result = -1;
                result = unitOfWork.ItemRspository.ItemAlreadyExists(objItem.ItemName, objItem.Barcode);
                if (result == 1)
                {
                    MessageBox.Show("Item With Given Name Already Exists, Please Specify Another Name.", "Item Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (result == 2)
                {
                    MessageBox.Show("Item With Given Barcode Already Added, Please Specify Another Barcode.", "Barcode Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                objItem.Suppliers = new List<Supplier>();
                objItem.CategoryId = Convert.ToInt64(cmbCategory.SelectedValue);
                objItem.UserId = SharedVariables.LoggedInUser.UserId;
                foreach (Supplier supp in lstSuppliers.SelectedItems)
                {
                    Supplier objSupplier = unitOfWork.SupplierRepository.GetById(supp.SupplierID);
                    objItem.Suppliers.Add(objSupplier);
                }
                unitOfWork.ItemRspository.Insert(objItem);
                if (objItem.OpeningStock > 0)
                {
                    MakeOpeningStockEntry(objItem);
                }
                unitOfWork.Save();
                MessageBox.Show("Item Saved Successfully", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //RefreshBulkItems(unitOfWork);
                SharedVariables.ReloadItemsRequired = true;
            }
            this.Close();
        }

        private void UpdateItem()
        {
            using (unitOfWork = new UnitOfWork())
            {
                Item obj = new Item();
                obj = unitOfWork.ItemRspository.GetItemData_ByITemId(this.ItemId);
                FillObject(obj);

                if (!obj.IsSyncable)
                {
                    obj.IsSyncable = true;
                }
                else if (obj.IsSynced)
                {
                    obj.IsNew = false;
                    obj.IsUpdate = true;
                    obj.IsSynced = false;
                }

                int result = -1;

                result = unitOfWork.ItemRspository.ItemAlreadyExists(obj.ItemId, obj.ItemName, obj.Barcode);
                obj.Category = unitOfWork.CategoryRepository.GetById(obj.CategoryId);
                obj.Manufacturer = unitOfWork.ManufacturerRepository.GetById(obj.ManufacturerId);
                obj.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);

                if (result == 1)
                {
                    MessageBox.Show("Item With Given Name Already Exists, Please Specify Another Name.", "Item Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (result == 2)
                {
                    MessageBox.Show("Item With Given Barcode Already Added, Please Specify Another Barcode.", "Barcode Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                foreach (Supplier s in LinkedSuppliers)
                {
                    obj.Suppliers.Remove(s);
                }
                obj.Suppliers = new List<Supplier>();
                foreach (Supplier supp in lstSuppliers.SelectedItems)
                {
                    Supplier objSupplier = unitOfWork.SupplierRepository.GetById(supp.SupplierID);
                    obj.Suppliers.Add(objSupplier);
                }
                unitOfWork.ItemRspository.Update(obj);
                unitOfWork.Save();
                MessageBox.Show("Item Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SharedVariables.ReloadItemsRequired = true;
                //RefreshBulkItems(unitOfWork);
            }
            this.Close();
        }
        private void MakeOpeningStockEntry(Item objItem)
        {
            actionTime = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
            Stock objStock = new Stock();
            objStock.Supplier = null;
            objStock.SupplierInvoiceNo = "";
            objStock.ImagePath = null;
            objStock.StockDate = actionTime;
            objStock.CreatedAt = actionTime;
            objStock.UpdatedAt = actionTime;
            objStock.IsActive = true;
            objStock.IsNew = true;
            objStock.SupplierIvoiceDate = actionTime;
            objStock.DocumentNo = unitOfWork.StockRepository.GetNewDocumentNumber();
            objStock.UserId = SharedVariables.LoggedInUser.UserId;
            objStock.IsOpeningStock = true;
            objStock.StockItems = new List<StockItem>();
            StockItem objStockItem = new StockItem();

            objStockItem.BatchId = 1;
            objStockItem.Quantity = objItem.OpeningStock;
            objStockItem.UnitCost = objItem.UnitCostPrice;
            objStock.GrandTotal = objStockItem.TotalCost = objStockItem.NetValue = objStockItem.Quantity * objStockItem.UnitCost;
            objStockItem.RetailPrice = objItem.RetailPrice;
            objStockItem.Item = objItem;// unitOfWork.ItemRspository.GetById(ItemId);
            objStockItem.CreatedAt = actionTime;
            objStockItem.UpdatedAt = actionTime;
            objStockItem.IsActive = true;
            objStockItem.UserId = SharedVariables.LoggedInUser.UserId;
            objStock.StockItems.Add(objStockItem);
            unitOfWork.StockRepository.Insert(objStock);
        }
        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            //frmAddSupplier fSup = new frmAddSupplier();
            //Form f = SharedFunctions.IsFormOpen(fSup.Name);
            //if (f == null)
            //{
            //    fSup.MdiParent = this.MdiParent;
            //    fSup.Show();
            //    fSup.FormClosed += new FormClosedEventHandler(frmAddSupplier_Closed);
            //}
            //else
            //{
            //    f.Focus();
            //}
        }
        private void frmAddSupplier_Closed(object sender, FormClosedEventArgs e)
        {
            using (unitOfWork = new UnitOfWork())
            {
                LoadSuppliers(unitOfWork);
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void txtUnitCostPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtRetailPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            frmAddCategory f = new frmAddCategory();
            f.FormClosed += new FormClosedEventHandler(frmAddCategory_Closed);
            f.ShowDialog();
        }

        private void frmAddCategory_Closed(object sender, FormClosedEventArgs e)
        {
            using (unitOfWork = new UnitOfWork())
            {
                LoadCategories(unitOfWork);
            }
        }

        private void frmAddSubCategory_Closed(object sender, FormClosedEventArgs e)
        {
            using (unitOfWork = new UnitOfWork())
            {
                LoadSubCategories(unitOfWork, this.selectedCategoryId);
            }
        }

        private void btnAddRack_Click(object sender, EventArgs e)
        {
            frmAddRack f = new frmAddRack();
            f.ShowDialog();
            using (unitOfWork = new UnitOfWork())
            {
                LoadRacks(unitOfWork);
            }
        }

        private void btnAddManf_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmAddManufacturer(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(frmManf_Closed);
        }

        private void frmManf_Closed(object sender, FormClosedEventArgs e)
        {
            using (unitOfWork = new UnitOfWork())
            {
                LoadManufacturers(unitOfWork);
            }
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void btnAddSubCategory_Click(object sender, EventArgs e)
        {
            frmAddSubCategory f = new frmAddSubCategory();
            f.FormClosed += new FormClosedEventHandler(frmAddSubCategory_Closed);
            f.ShowDialog();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCategory.SelectedIndex > 0)
            {
                this.selectedCategoryId = Convert.ToInt64(cmbCategory.SelectedValue);
                using (unitOfWork = new UnitOfWork())
                {
                    this.LoadSubCategories(unitOfWork, this.selectedCategoryId);
                }
            }
        }

        private void chkIsRawItem_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}