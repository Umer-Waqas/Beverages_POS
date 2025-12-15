using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GK.Shared.Core;
using GK.Shared.Core.ViewModels;
using GK.Shared.Repository;
using Pharmacy.Core.EntityModel;

namespace Restaurant_MS_UI.Menu.Suppliers
{
    public partial class frmSupplierInfo : Form
    {
        private UnitOfWork unitOfWork;
        long SupplierId = 0;
        public frmSupplierInfo()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }

        public frmSupplierInfo(long SupplierId)
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
            this.SupplierId = SupplierId;
        }
        private void frmSupplierInfo_Load(object sender, EventArgs e)
        {
            LoadSupplierData();
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnViewPaymentHistory });
            SharedFunctions.SetGridStyle(grdBills);
            SharedFunctions.SetGridStyle(grdItems);
        }

        private void LoadSupplierData()
        {
            SupplierDetailsVM sd = unitOfWork.SupplierRepository.GetSupplierWithDataDetails(this.SupplierId);
            lblName.Text = sd.Supplier.Name;
            lblPhone.Text = sd.Supplier.Phone;
            lblAddress.Text = sd.Supplier.Address;
            string PaymentMode = "Cash";
            grdItems.Rows.Clear();
            foreach (Item i in sd.Items)
            {
                if (i.IsActive)
                {
                    grdItems.Rows.Add(i.ItemId, i.ItemName, i.CreatedAt);
                }
            }
            string itemsString = "";
            double NetValue = 0;
            foreach (StockVM s in sd.Stocks)
            {
                itemsString = "";
                NetValue = 0;
                foreach (StockItemVM si in s.StockItems)
                {
                    itemsString += si.Item.ItemName + ", ";
                    NetValue += si.TotalCost;
                }
                if (!string.IsNullOrEmpty(itemsString))
                {
                    itemsString = itemsString.Trim().TrimEnd(',');
                }
                grdBills.Rows.Add(s.StockId, Math.Round(s.DocumentNo, 0), s.CreatedAt, s.SupplierInvoiceNo, s.SupplierIvoiceDate, s.TotalAmount, s.TotalAmount - s.TotalPaid);
            }

        }

        private void grdBills_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == grdBills.Columns["colPayment"].Index)
            {
                if (UISharedFunctions.CheckDayClosed()) { return; }
                frmStockPayments f = new frmStockPayments(this.SupplierId);
                f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                f.Show();
            }
            if (e.ColumnIndex == grdBills.Columns["colPaymentHistory"].Index)
            {
                var docNum = Convert.ToDecimal(grdBills.Rows[e.RowIndex].Cells["colDocumentNoBills"].Value);
                frmSupplierPaymentHistory f = new frmSupplierPaymentHistory (this.SupplierId, docNum);
                f.FormClosed += new FormClosedEventHandler (frmSupplierPaymentHistory_closed);
                f.Show();
            }
        }

        private void frmSupplierPaymentHistory_closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }

        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }

        private void grdPayments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            grdItems.Rows.Clear();
            grdBills.Rows.Clear();
            LoadSupplierData();
        }

        private void btnViewPaymentHistory_Click(object sender, EventArgs e)
        {
            frmSupplierPaymentHistory f = new frmSupplierPaymentHistory(this.SupplierId);
            f.Show();
        }
    }
}
