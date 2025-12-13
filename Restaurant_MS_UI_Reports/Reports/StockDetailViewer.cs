using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_MS_Core.Entities;
using GK.Shared.Repository;
using GK.Shared.Repository;
using Restaurant_MS_Core.ViewModels;
using GK.Shared.Core;


namespace Pharmacy.UI.Reports
{
    public partial class StockDetailViewer : Form
    {
        static AppDbContext cxt;
        StockRepository repStocks;
        long StockId = 0;

        public StockDetailViewer()
        {
            InitializeComponent();
            InitializedObjects();
        }

        private void InitializedObjects()
        {
            cxt = new AppDbContext();
            repStocks = new StockRepository(cxt);
        }
        public StockDetailViewer(long StockId)
        {
            InitializeComponent();
            this.StockId = StockId;
            InitializedObjects();
        }

        private void StockDetailViewer_Load(object sender, EventArgs e)
        {
            try
            {
                StockVM StockObject;
                using (UnitOfWork u = new UnitOfWork())
                {
                    StockObject = u.StockRepository.GetStockDetailById(this.StockId);
                }

                //Stock objStock = repStocks.GetStockById_Edit(this.StockId);
                StockDetailRpt r = new StockDetailRpt();

                r.Database.Tables[0].SetDataSource(StockObject.StockItems);
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);

                // reportspecific info
                r.SetParameterValue("SupInvNo", StockObject.SupplierInvoiceNo);
                r.SetParameterValue("DocumentNo", (decimal)StockObject.DocumentNo);
                r.SetParameterValue("Date", StockObject.CreatedAt);
                r.SetParameterValue("SupplierInvoiceDate", StockObject.SupplierIvoiceDate);
                crystalReportViewer1.ReportSource = r;
                crystalReportViewer1.Show();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
    }
}