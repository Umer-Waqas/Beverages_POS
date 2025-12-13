

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmExpense : Form
    {
        UnitOfWork unitOfWork;
        int PageNo = 1;
        long ExpenseEditId = 0;
        long filterCategoryIdValue = 0;
        DateTime filterFromDateValue = DateTime.Now;
        DateTime filterToDateValue = DateTime.Now;

        public frmExpense()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }

        public frmExpense(long ExpenseEditId)
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
            this.ExpenseEditId = ExpenseEditId;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cmbExpCategory.SelectedValue = 0;
            txtDescription.Text = "";
            txtAmount.Text = "";
            this.ExpenseEditId = 0;
            this.dtpExpDate.Value = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
        }

        private void FillObject(Expense objExp)
        {
            var expDate = dtpExpDate.Value;
            objExp.ExpenseCategoryId = Convert.ToInt64(cmbExpCategory.SelectedValue);
            objExp.description = txtDescription.Text.Trim();
            double exp = 0; double.TryParse(txtAmount.Text.Trim(), out exp);
            objExp.Amount = exp;
            objExp.Date = expDate;
            if (this.ExpenseEditId <= 0)
            {
                objExp.IsNew = true;
                objExp.CreatedAt = DateTime.Now;
                objExp.IsActive = true;
            }
            objExp.UpdatedAt = DateTime.Now;
        }

        private bool IsValidInput()
        {
            bool ErrFound = false;
            if (Convert.ToInt64(cmbExpCategory.SelectedValue) <= 0)
            {
                ErrExpCategory.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrExpCategory.Visible = false;
            }
            //if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            //{
            //    ErrDesc.Visible = true;
            //    ErrFound = true;
            //}
            //else
            //{
            //    ErrDesc.Visible = false;
            //}
            double expAmt = 0; double.TryParse(txtAmount.Text.Trim(), out expAmt);
            if (expAmt <= 0)
            {
                errAmt.Visible = true;
                ErrFound = true;
            }
            else
            {
                errAmt.Visible = false;
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

        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    if (this.ExpenseEditId > 0)
                    {
                        this.btnAddSupplier.Text = "Update";
                        UpdateExpense();
                    }
                    else
                    {
                        InsertExpense();
                    }
                }
                loadExpenses();
            }
            catch (Exception ex)
            {
                unitOfWork = new UnitOfWork();
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

        private void InsertExpense()
        {
            Expense objExp = new Expense();
            FillObject(objExp);
            using (unitOfWork = new UnitOfWork())
            {
                objExp.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                unitOfWork.ExpenseRepository.Insert(objExp);
                unitOfWork.Save();
            }
            this.btnClear.PerformClick();
            MessageBox.Show("Expense Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateExpense()
        {
            using (unitOfWork = new UnitOfWork())
            {
                Expense objExp = unitOfWork.ExpenseRepository.GetById(this.ExpenseEditId);
                FillObject(objExp);
                if (objExp.IsSynced)
                {
                    objExp.IsNew = false;
                    objExp.IsUpdate = true;
                    objExp.IsSynced = false;
                }
                objExp.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                unitOfWork.ExpenseRepository.Update(objExp);
                unitOfWork.Save();
                this.btnClear.PerformClick();
            }
            MessageBox.Show("Expense Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmAddSupplier_Load(object sender, EventArgs e)
        {
            grdExpenses.Columns["colId"].Visible = false;
            grdExpenses.Columns["colExpCategoryId"].Visible = false;
            grdExpenses.Columns["colExpDate"].Visible = false;

            dtpExpDate.Value = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
            if (SharedFunctions.CheckDayClosed())
            {
                this.BeginInvoke(new MethodInvoker(Close));
                return;
            }

            loadExpCategories();
            this.WindowState = FormWindowState.Maximized;
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddSupplier, btnAddExpCategory, btnClear });
            loadExpenses();
            //try
            //{
            //    if (this.ExpenseEditId > 0)
            //    {
            //        pnlSupplierItems.Visible = true;
            //        LoadSupplier();
            //        btnAddSupplier.Text = "Update";
            //        this.Text = "Update Supplier";
            //    }
            //    else
            //    {
            //        this.Size = new System.Drawing.Size(415, 450);
            //        this.Text = "Add Supplier";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            //}
        }

        private void loadExpenses()
        {
            grdExpenses.Rows.Clear();
            if (filterFromDateValue > filterToDateValue)
            {
                MessageBox.Show("Start date can't be greater that to date.", "Invalid Date Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<Expense> exps = new List<Expense>();
            using (unitOfWork = new UnitOfWork())
            {
                exps = (List<Expense>)unitOfWork.ExpenseRepository.getExpenses(filterCategoryIdValue, filterFromDateValue, filterToDateValue);
            }
            foreach (var r in exps)
            {
                grdExpenses.Rows.Add(r.ExpenseId, r.ExpenseCategoryId, r.ExpenseCategory.Title, r.Amount, r.description, r.Date, r.Date.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
            }
            lblTotal.Text = (exps.Sum(e => (double?)e.Amount) ?? 0).ToString();
        }

        private void loadExpCategories()
        {
            List<SelectListVM> exps = new List<SelectListVM>();
            List<SelectListVM> expsCatFilter = new List<SelectListVM>();
            using (unitOfWork = new UnitOfWork())
            {
                exps = unitOfWork.ExpenseCategoryRepository.GetSelectList();
            }
            SharedFunctions.SetComboDataSource(exps, cmbExpCategory, "Select Expense Category");
            expsCatFilter = new List<SelectListVM>(exps);
            filterCategory.SelectedIndexChanged -= filterCategory_SelectedIndexChanged;
            SharedFunctions.SetComboDataSource(expsCatFilter, filterCategory, "Select Expense Category");
            filterCategory.SelectedIndexChanged += filterCategory_SelectedIndexChanged;
        }

        private void LoadSupplier()
        {
            //SupplierVM vm = new SupplierVM();
            //using (unitOfWork = new UnitOfWork())
            //{
            //    vm = unitOfWork.SupplierRepository.GetSupplierWithItems(this.ExpenseEditId, PageNo, SharedVariables.PageSize);
            //}
            //chkIsHoSupplier.Checked = vm.IsHOSupplier.HasValue ? vm.IsHOSupplier.Value : false;
            //txtSuppName.Text = vm.Name;
            //txtPhone.Text = vm.Phone;
            //txtDescription.Text = vm.Address;
            //txtPrimContName.Text = vm.PrimaryContactPersonName;
            //txtPrimaryContPh.Text = vm.PrimaryContactPersonPhone;
            //numExpense.Value = (decimal)vm.OpeningBalance;
            //foreach (Item i in vm.Items)
            //{
            //    grdSupplierItems.Rows.Add(i.ItemName, i.IsNarcotic ? "Narcotic" : "Drug");
            //}
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            //frmAddSupplierShortCuts f = new frmAddSupplierShortCuts();
            //f.ShowDialog();
        }

        private void btnAddExpCategory_Click(object sender, EventArgs e)
        {
            frmExpenseCategory f = new frmExpenseCategory();
            f.ShowDialog();
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void filterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.filterCategoryIdValue = Convert.ToInt64(filterCategory.SelectedValue);
            loadExpenses();
        }

        private void filterFromDate_ValueChanged(object sender, EventArgs e)
        {
            this.filterFromDateValue = filterFromDate.Value;
            loadExpenses();
        }

        private void filterToDate_ValueChanged(object sender, EventArgs e)
        {
            this.filterToDateValue = filterToDate.Value;
            loadExpenses();
        }

        private void grdExpenses_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine("row entered");
        }

        private void grdExpenses_DoubleClick(object sender, EventArgs e)
        {
        }

        private void grdExpenses_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

        }

        private void grdExpenses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                var row = grdExpenses.Rows[e.RowIndex];
                if (e.ColumnIndex == grdExpenses.Columns["colEdit"].Index)
                {
                    this.ExpenseEditId = Convert.ToInt64(row.Cells["colId"].Value);
                    this.txtDescription.Text = row.Cells["colDesc"].Value.ToString();
                    this.dtpExpDate.Value = Convert.ToDateTime(row.Cells["colExpDate"].Value);
                    cmbExpCategory.SelectedValue = Convert.ToInt64(row.Cells["colExpCategoryId"].Value);
                    txtAmount.Text = row.Cells["colTotalExpense"].Value.ToString();
                }

                if (e.ColumnIndex == grdExpenses.Columns["colDelete"].Index)
                {
                    var confirm = MessageBox.Show("Are you sure you want to delete expense?", "Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (confirm == System.Windows.Forms.DialogResult.Yes)
                    {
                        var delId = Convert.ToInt64(row.Cells["colId"].Value);
                        bool res = false;
                        using (unitOfWork = new UnitOfWork())
                        {
                            res = unitOfWork.ExpenseRepository.DeleteExpense(delId);
                        }
                        if (res)
                        {
                            loadExpenses();
                            //MessageBox.Show("Expense deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("An error occurred while deleting expense, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error Occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}