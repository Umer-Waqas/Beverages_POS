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
    public partial class StockHOReturnViewer : Form
    {
        long AdjustmentID = 0;
        public StockHOReturnViewer()
        {
            InitializeComponent();
        }

        public StockHOReturnViewer(long AdjustmentId)
        {
            InitializeComponent();
            this.AdjustmentID = AdjustmentId;
        }

        private void AdjustmentsViewer_Load(object sender, EventArgs e)
        {
            AdjustmentsVM ad = null;
            using (UnitOfWork uow = new UnitOfWork())
            {
                ad = uow.AdjustmentRepository.GetAdjustments_Rpt(this.AdjustmentID);
            }
            rptStockHOReturn r = new rptStockHOReturn();
            r.Database.Tables[0].SetDataSource(ad.AdjustmentItems);
            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
            {
                r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
            }
            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
            r.SetParameterValue("AdjustmentNo", ad.AdjustmentId);
            crystalReportViewer1.ReportSource = r;
            crystalReportViewer1.Show();
        }
    }
}
