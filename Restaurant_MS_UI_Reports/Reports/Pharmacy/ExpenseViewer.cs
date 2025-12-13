using GK.Shared.Core;
using Restaurant_MS_Core.ViewModels;
using GK.Shared.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GK.Shared.Repository;

using Pharmacy.Core.ViewModels;

namespace Pharmacy.UI.Reports.Pharmacy
{
    public partial class ExpenseViewer : Form
    {
        int Orientation = 0;
        DateTime DateFrom;
        DateTime DateTo;
        UnitOfWork unitOfWork;

        long filterCategoryIdValue = 0;
        DateTime filterFromDateValue = DateTime.Now;
        DateTime filterToDateValue = DateTime.Now;
        public ExpenseViewer()
        {
            InitializeComponent();
        }

        public ExpenseViewer(int Orientation, DateTime DateFrom, DateTime DateTo)
        {
            InitializeComponent();
            this.Orientation = Orientation;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
        }


        private void loadExpenses()
        {
            try
            {
                printReport(type: 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DeadStocksViewer_Load(object sender, EventArgs e)
        {
            loadExpCategories();
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint });
            SharedFunctions.SetSmallButtonsStyle(new[] { btnView });
        }
        private void loadExpCategories()
        {
            List<SelectListVM> exps = new List<SelectListVM>();
            List<SelectListVM> expsCatFilter = new List<SelectListVM>();
            using (unitOfWork = new UnitOfWork())
            {
                exps = unitOfWork.ExpenseCategoryRepository.GetSelectList();
            }
            expsCatFilter = new List<SelectListVM>(exps);
            cmbExpCategory.SelectedIndexChanged -= filterCategory_SelectedIndexChanged;
            SharedFunctions.SetComboDataSource(expsCatFilter, cmbExpCategory, "Select Expense Category");
            cmbExpCategory.SelectedIndexChanged += filterCategory_SelectedIndexChanged;
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            this.filterFromDateValue = filterFromDate.Value;
            this.filterToDateValue = filterToDate.Value;
            this.filterCategoryIdValue = Convert.ToInt64(cmbExpCategory.SelectedValue);
            this.loadExpenses();
        }

        private void filterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.filterCategoryIdValue = Convert.ToInt64(cmbExpCategory.SelectedValue);
        }

        private List<ExpenseVM> getExpensesData()
        {
            List<ExpenseVM> vm = new List<ExpenseVM>();
            List<Expens> exps = new List<Expens>();
            using (unitOfWork = new UnitOfWork())
            {
                exps = (List<Expens>)unitOfWork.ExpenseRepository.getExpenses(filterCategoryIdValue, filterFromDateValue, filterToDateValue);

                foreach (var e in exps)
                {
                    ExpenseVM obj = new ExpenseVM();
                    obj.ExpenseId = e.ExpenseId;
                    obj.ExpenseCategory = e.ExpenseCategory.Description;
                    obj.Amount = e.Amount.Value;
                    obj.description = e.description;
                    obj.Date = e.Date;
                    obj.CreatedAt = e.CreatedAt;
                    vm.Add(obj);
                }
            }
            return vm;
        }

        /// <summary>
        /// type = 0 => view
        /// type = 1 => excel
        /// type = 2 => pdf
        /// </summary>
        /// <param name="type"></param>
        private void printReport(int type)
        {
            if (filterFromDateValue.Date > filterToDateValue.Date)
            {
                MessageBox.Show("Start date can't be greater that to date.", "Invalid Date Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Reports.Pharmacy.Expense_Portrait r = new Expense_Portrait();
            r.Database.Tables[1].SetDataSource(this.getExpensesData());
            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
            {
                r.Database.Tables[0].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
            }
            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
            r.SetParameterValue("DateFrom", this.DateFrom);
            r.SetParameterValue("DateTo", this.DateTo);
            r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

            if (type == 0)
            {
                crystalReportViewer1.ReportSource = r;
                crystalReportViewer1.Show();
            }
            else if (type == 1)
            {
                if (saveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    r.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, saveExcel.FileName);
                    MessageBox.Show("Expenses Data Exported Successfully." + Environment.NewLine + "Destination:" + saveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                printReport(type: 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
