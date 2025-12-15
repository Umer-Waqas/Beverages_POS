
namespace Restaurant_MS_UI.Menu.Suppliers
{
    public partial class frmAddSupplier : Form
    {
        long SupplierEditId = 0;
        public frmAddSupplier()
        {
            InitializeComponent();
        }


        public frmAddSupplier(long SupplierEditId)
        {
            InitializeComponent();
            this.SupplierEditId = SupplierEditId;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSuppName.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            txtPrimContName.Text = "";
            txtPrimaryContPh.Text = "";

            ErrSuppName.Visible = false;
            ErrPhoneNo.Visible = false;
        }

        private void FillObject(Supplier objSupplier)
        {
            objSupplier.IsHoSupplier = chkIsHoSupplier.Checked;
            objSupplier.Name = txtSuppName.Text;
            objSupplier.Phone = txtPhone.Text;
            objSupplier.Address = txtAddress.Text;
            objSupplier.PrimaryContactPersonName = txtPrimContName.Text;
            objSupplier.PrimaryContactPersonPhone = txtPrimaryContPh.Text;
            if (this.SupplierEditId <= 0)
            {
                objSupplier.IsNew = true;
                objSupplier.CreatedAt = DateTime.Now;
                objSupplier.IsActive = true;
            }
            objSupplier.UpdatedAt = DateTime.Now;
        }

        private bool IsValidInput()
        {
            bool ErrFound = false;
            if (string.IsNullOrEmpty(txtSuppName.Text.Trim()))
            {
                ErrSuppName.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrSuppName.Visible = false;
            }
            if (string.IsNullOrEmpty(txtPhone.Text.Trim()))
            {
                ErrPhoneNo.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrPhoneNo.Visible = false;
            }
            //if (string.IsNullOrEmpty(txtAddress.Text.Trim()))
            //{
            //    ErrAddress.Visible = true;
            //    ErrFound = true;
            //}
            //else
            //{
            //    ErrAddress.Visible = false;
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
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    if (this.SupplierEditId > 0)
                    {
                        UpdateSupplier();
                    }
                    else
                    {
                        InsertSupplier();
                    }
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("IX_Name"))
                    {
                        MessageBox.Show("Supplier with Given Name Already Exists, Please Specify Another Name", "Supplier Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                // MessageBox.Show(ex.InnerException.InnerException.Message, "Inner exception details");
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void InsertSupplier()
        {
            Supplier objSupplier = new Supplier();
            FillObject(objSupplier);
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                objSupplier.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                unitOfWork.SupplierRepository.Insert(objSupplier);
                unitOfWork.Save();
            }
            MessageBox.Show("Supplier Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateSupplier()
        {
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                Supplier objSupplier = unitOfWork.SupplierRepository.GetById(this.SupplierEditId);
                FillObject(objSupplier);
                if (objSupplier.IsSynced)
                {
                    objSupplier.IsNew = false;
                    objSupplier.IsUpdate = true;
                    objSupplier.IsSynced = false;
                }
                objSupplier.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                unitOfWork.SupplierRepository.Update(objSupplier);
                unitOfWork.Save();
            }
            MessageBox.Show("Supplier Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmAddSupplier1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnSave, btnClear });
            try
            {
                if (this.SupplierEditId > 0)
                {
                    grdSupplierItems.Visible = true;
                    LoadSupplier();
                    this.Text = "Update Supplier";
                }
                else
                {
                    this.Size = new System.Drawing.Size(415, 450);
                    this.Text = "Add Supplier";
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void LoadSupplier()
        {
            SupplierVM vm = new SupplierVM();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                vm = unitOfWork.SupplierRepository.GetSupplierWithItems(this.SupplierEditId, 1, SharedVariables.PageSize);
            }
            chkIsHoSupplier.Checked = vm.IsHOSupplier.HasValue ? vm.IsHOSupplier.Value : false;
            txtSuppName.Text = vm.Name;
            txtPhone.Text = vm.Phone;
            txtAddress.Text = vm.Address;
            txtPrimContName.Text = vm.PrimaryContactPersonName;
            txtPrimaryContPh.Text = vm.PrimaryContactPersonPhone;
            foreach (Item i in vm.Items)
            {
                grdSupplierItems.Rows.Add(i.ItemName);
            }
        }
    }
}
