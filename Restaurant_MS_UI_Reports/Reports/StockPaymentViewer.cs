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

namespace Pharmacy.UI.Reports
{
    public partial class StockPaymentViewer : Form
    {
        UnitOfWork unitOfWork;
        private long StockId;
        public StockPaymentViewer()
        {
            InitializeComponent();
        }

        public StockPaymentViewer(long StockId)
        {
            InitializeComponent();
            this.StockId = StockId;
        }

        private void AdjustmentsViewer_Load(object sender, EventArgs e)
        {
            StockPaymentWithItemDetails stock = new StockPaymentWithItemDetails();
            using (unitOfWork = new UnitOfWork())
            {
                stock = unitOfWork.StockRepository.GetStockPaymentPrint(this.StockId);
            }

            StockPaymentRpt r = new StockPaymentRpt();

            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
            {
                r.Database.Tables[0].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
            }
            else
            {
                r.Database.Tables[0].SetDataSource(new DataTable());
            }
            r.Database.Tables[1].SetDataSource(stock.Items);

            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
            r.SetParameterValue("DocumentNum", stock.DocumentNo);
            r.SetParameterValue("SupplierName", stock.SupplierName);
            r.SetParameterValue("SupplierInvoiceNo", stock.SupplierInvoiceNo);
            r.SetParameterValue("StockDate", stock.StockDate);
            r.SetParameterValue("GrandTotal", stock.GrandTotal);
            r.SetParameterValue("Paid", stock.TotalPaid);
            // r.SetParameterValue("ToDate", this.DateTo);
            crystalReportViewer1.ReportSource = r;
            crystalReportViewer1.Show();
        }
    }
}
