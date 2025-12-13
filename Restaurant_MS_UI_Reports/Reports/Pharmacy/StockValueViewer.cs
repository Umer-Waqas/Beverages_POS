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

namespace Pharmacy.UI.Reports.Pharmacy
{
    public partial class StockValueViewer : Form
    {
        public StockValueViewer()
        {
            InitializeComponent();
        }

        private void StockValueViewer_Load(object sender, EventArgs e)
        {
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                cmbCategory.SelectedIndexChanged -= cmbCategory_SelectedIndexChanged;

                List<SelectListVM> cats = new List<SelectListVM>();
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    cats = unitOfWork.CategoryRepository.GetCategoriesSelectList();
                }
                SharedFunctions.SetComboDataSource(cats, cmbCategory, "Select Category");
                cmbCategory.SelectedIndexChanged += cmbCategory_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSubCategories(long ParentCategoryId)
        {
            try
            {
                cmbSubCategory.SelectedIndexChanged -= cmbSubCategory_SelectedIndexChanged;

                List<SelectListVM> cats = new List<SelectListVM>();
                using (UnitOfWork unitOfWork = new UnitOfWork())
                {
                    cats = unitOfWork.CategoryRepository.GetSubCategoriesSelectList(ParentCategoryId);
                }
                SharedFunctions.SetComboDataSource(cats, cmbSubCategory, "Select Sub Category");
                cmbSubCategory.SelectedIndexChanged += cmbSubCategory_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loadReport()
        {
            long? categoryId = null;
            long? subCategoryId = null;

            if (Convert.ToInt64(cmbCategory.SelectedValue) > 0)
            {
                categoryId = Convert.ToInt64(cmbCategory.SelectedValue);
            }
            if (Convert.ToInt64(cmbSubCategory.SelectedValue) > 0)
            {
                subCategoryId = Convert.ToInt64(cmbSubCategory.SelectedValue);
            }

            List<StockValueRptVM> data = new List<StockValueRptVM>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                data = unitOfWork.ItemRspository.GetItemsWiseStockValue(categoryId, subCategoryId);
            }
            StockValueRpt r = new StockValueRpt();
            r.Database.Tables["dtItemsStockValueReport"].SetDataSource(data);
            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
            {
                r.Database.Tables["dtLogo"].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
            }
            else
            {
                r.Database.Tables["dtLogo"].SetDataSource(new DataTable());
            }
            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
            crystalReportViewer1.ReportSource = r;
            crystalReportViewer1.Show();
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                long categoryId = Convert.ToInt64(cmbCategory.SelectedValue);
                //this.LoadSubCategories(categoryId);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                loadReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}