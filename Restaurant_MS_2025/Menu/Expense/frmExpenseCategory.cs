using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmExpenseCategory : Form
    {
        UnitOfWork unitOfWork;
        int PageNo = 1;
        long ExpCategoryEditId = 0;
        public frmExpenseCategory()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }

        public frmExpenseCategory(long ExpCategoryEditId)
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
            this.ExpCategoryEditId = ExpCategoryEditId;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCategory.Text = "";
            txtDescription.Text = "";
            btnAdd.Text = "Add";
        }

        private void FillObject(ExpenseCategory objExp)
        {
            objExp.Title = txtCategory.Text.Trim();
            objExp.Description = txtDescription.Text.Trim();
            objExp.DisplayInDropDown = true;
            if (this.ExpCategoryEditId <= 0)
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
            if (string.IsNullOrEmpty(txtCategory.Text.Trim()))
            {
                ErrCategory.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrCategory.Visible = false;
            }
            if (string.IsNullOrEmpty(txtDescription.Text.Trim()))
            {
                ErrDescription.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrDescription.Visible = false;
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

        private void InsertCategory()
        {
            ExpenseCategory objExp = new ExpenseCategory();
            FillObject(objExp);
            using (unitOfWork = new UnitOfWork())
            {
                objExp.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                if (unitOfWork.ExpenseCategoryRepository.alreadyExists_Insert(objExp.Title))
                {
                    MessageBox.Show("Category with this name already exists. Please search and use existing category instead of creating new one.", "Category Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                unitOfWork.ExpenseCategoryRepository.Insert(objExp);
                unitOfWork.Save();
            }
            MessageBox.Show("Expense Category Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void UpdateCategory()
        {
            using (unitOfWork = new UnitOfWork())
            {
                ExpenseCategory objExp = unitOfWork.ExpenseCategoryRepository.GetById(this.ExpCategoryEditId);
                FillObject(objExp);
                if (unitOfWork.ExpenseCategoryRepository.alreadyExists_Update(objExp.Title, objExp.ExpenseCategoryId))
                {
                    MessageBox.Show("Category with this name already exists. Please search and use existing category instead of creating new one.", "Category Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (objExp.IsSynced)
                {
                    objExp.IsNew = false;
                    objExp.IsUpdate = true;
                    objExp.IsSynced = false;
                }
                objExp.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                unitOfWork.ExpenseCategoryRepository.Update(objExp);
                unitOfWork.Save();
            }
            MessageBox.Show("Expense Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmAddSupplier_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnAdd, btnClear });
            //try
            //{
            //    if (this.ExpCategoryEditId > 0)
            //    {
            //        pnlSupplierItems.Visible = true;
            //        LoadSupplier();
            //        btnAdd.Text = "Update";
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

        private void LoadExpCategory()
        {
            //SupplierVM vm = new SupplierVM();
            //using (unitOfWork = new UnitOfWork())
            //{
            //    vm = unitOfWork.SupplierRepository.GetSupplierWithItems(this.SupplierEditId, PageNo, SharedVariables.PageSize);
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

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!IsValidInput()) return;
            if (this.ExpCategoryEditId > 0)
            {
                UpdateCategory();
            }
            else
            {
                InsertCategory();
            }
        }
    }
}