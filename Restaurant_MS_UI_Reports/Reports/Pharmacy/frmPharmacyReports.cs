using GK.Shared.Core;
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
    public partial class frmPharmacyReports : Form
    {
        public frmPharmacyReports()
        {
            InitializeComponent();
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new TransactionUI(), this.MdiParent);
        }

        private void btnNarcotic_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new NarcoticDrugsUI(), this.MdiParent);
        }

        private void btnItemWiseSale_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new ItemWiseSaleUI(), this.MdiParent);
        }

        private void btnItemWiseRevenue_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new ItemWiseRevenueUI(), this.MdiParent);
        }

        private void btnLowStocks_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new LowStockUI(), this.MdiParent);
        }

        private void btnIncomeStatement_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new IncomeStatementUI(), this.MdiParent);
        }

        private void btnPurchaseReports_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new PurchaseReportsUI(), this.MdiParent);
        }

        private void btnDiscounts_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new DiscountsUI(), this.MdiParent);
        }

        private void btnProfitStatement_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new ProfiStatementUI(), this.MdiParent);
        }

        private void btnDeadStock_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new DeadStockUI(), this.MdiParent);
        }

        private void btnExpiry_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new ExpiryUI(), this.MdiParent);
        }

        private void btnStockStatistics_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new StockStatisticsUI(), this.MdiParent);
        }
        private void btnItemWisePurchase_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new ItemWisePurchaseUI(), this.MdiParent);
        }

        private void btnItemsList_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new ItemsListViewer(), this.MdiParent);
        }

        private void btnTodayCollection_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new DailyCollectionViewer(), this.MdiParent);
        }

        private void btnDailySummaryRpt_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new DailySummaryReportViewer(), this.MdiParent);
        }

        private void btnStoreItemStockValue_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new StockValueViewer(), this.MdiParent);
        }

        private void btnExpenses_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new ExpenseViewer(), this.MdiParent);
        }
    }
}