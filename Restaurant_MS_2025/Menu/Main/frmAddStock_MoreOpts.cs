using GK.Shared.Core;
using Restaurant_MS_Core.Entities;
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

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmAddStock_MoreOpts : Form
    {
        UnitOfWork unitOfWork;
        public SharedVariables.LoadStockAction LoadStockAction;
        public long selectedSupplierId = 0;
        public frmAddStock_MoreOpts()
        {
            InitializeComponent();
        }

        private void frmAssStock_MoreOpts_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnSup_AI, btnSup_LS, btnSup_MS, btnSup_PSI, btnAll_LS, btnAll_MS, btnAll_PSI, btnCancel });
            
        }

        private async void loadSuppliers()
        {
            List<SelectListVM> Suppliers = new List<SelectListVM>();
            using (unitOfWork = new UnitOfWork())
            {
                Suppliers = await unitOfWork.SupplierRepository.GetSelectList();
                SharedFunctions.SetComboDataSource(Suppliers, cmbSuppliers, "Select Supplier");
            }
        }

        private bool IsSupplierSelected()
        {
            if (Convert.ToInt32(cmbSuppliers.SelectedValue) <= 0)
            {
                MessageBox.Show("Please select supplier before proceeding", "Select Supplier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnSup_AI_Click(object sender, EventArgs e)
        {
            if (!IsSupplierSelected()) return;
            this.LoadStockAction = SharedVariables.LoadStockAction.AllItemsBySupplier;
            this.selectedSupplierId = Convert.ToInt32(cmbSuppliers.SelectedValue);
            this.Close();
        }
        private void btnSup_LS_Click(object sender, EventArgs e)
        {
            if (!IsSupplierSelected()) return;
            this.LoadStockAction = SharedVariables.LoadStockAction.LowStockItemsBySupplier;
            this.selectedSupplierId = Convert.ToInt32(cmbSuppliers.SelectedValue);
            this.Close();
        }
        private void btnSup_PSI_Click(object sender, EventArgs e)
        {
            if (!IsSupplierSelected()) return;
            this.LoadStockAction = SharedVariables.LoadStockAction.PrevSoldItemsBySupplier;
            this.selectedSupplierId = Convert.ToInt32(cmbSuppliers.SelectedValue);
            this.Close();
        }

        private void btnSup_MS_Click(object sender, EventArgs e)
        {
            if (!IsSupplierSelected()) return;
            this.LoadStockAction = SharedVariables.LoadStockAction.MissedSaleItemsBySupplier;
            this.selectedSupplierId = Convert.ToInt32(cmbSuppliers.SelectedValue);
            this.Close();
        }

        private void btnAll_LS_Click(object sender, EventArgs e)
        {
            this.LoadStockAction = SharedVariables.LoadStockAction.AllLowStockItems;
            this.Close();
        }

        private void btnAll_PSI_Click(object sender, EventArgs e)
        {
            this.LoadStockAction = SharedVariables.LoadStockAction.AllPreviouslySoldItems;
            this.Close();
        }

        private void btnAll_MS_Click(object sender, EventArgs e)
        {
            this.LoadStockAction = SharedVariables.LoadStockAction.AllMissedSaleItems;
            this.Close();
        }

        private void frmAddStock_MoreOpts_Shown(object sender, EventArgs e)
        {
            loadSuppliers();
        }
    }
}