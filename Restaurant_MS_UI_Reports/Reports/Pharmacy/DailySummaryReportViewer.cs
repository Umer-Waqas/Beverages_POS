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
    public partial class DailySummaryReportViewer : Form
    {
        UnitOfWork unitOfWork;
        int Orientation = 0;
        DateTime DateFrom;
        DateTime DateTo;
        List<ProfitStatementVM> ProfitStatements;


        public DailySummaryReportViewer()
        {
            InitializeComponent();
        }

        public DailySummaryReportViewer(int Orientation, DateTime DateFrom, DateTime DateTo, List<ProfitStatementVM> ProfitStatements)
        {
            InitializeComponent();
            this.Orientation = Orientation;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
            this.ProfitStatements = ProfitStatements;
        }

        private void ProfitStatementViewer_Load(object sender, EventArgs e)
        {
            dtpSummaryDate.ValueChanged -= dtpSummaryDate_ValueChanged;
            var date = dtpSummaryDate.Value = SharedFunctions.GetDateAccordingToDayCloseSetting();
            dtpSummaryDate.ValueChanged += dtpSummaryDate_ValueChanged;
            loadSummaryReport(date);
        }

        private void loadSummaryReport(DateTime summaryDate)
        {
            try
            {
                DailySummaryReportWrapprtVM res = new DailySummaryReportWrapprtVM();
                using (unitOfWork = new UnitOfWork())
                {
                    res = unitOfWork.GeneralRepository.GetDailySummaryReport(summaryDate);
                }
                Reports.Pharmacy.DailySummaryRpt r = new DailySummaryRpt();
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    r.Database.Tables["dtLogo"].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                else
                {
                    r.Database.Tables["dtLogo"].SetDataSource(new DataTable());
                }
                r.Database.Tables["dtDailySummaryReport"].SetDataSource(res.CetagoryWiseSummary);
                r.Database.Tables["dtExpenseSummary"].SetDataSource(res.Expenses);
                r.Database.Tables["dtAdjustmentSummary"].SetDataSource(res.Adjustments);
                r.Database.Tables["dtStoreClosing"].SetDataSource(res.StoreClosingSummary);

                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                r.SetParameterValue("PharmacyPhone", " (" + SharedVariables.AdminPractiseSetting.Phone + " )");
                r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                r.SetParameterValue("LocalPurchase", res.LocalPurchase);
                r.SetParameterValue("HOPurchase", res.HOPurchase);
                r.SetParameterValue("TotalDiscount", res.TotalDiscount);
                r.SetParameterValue("OpeningBalance", res.OpeningBalance);
                r.SetParameterValue("SummaryDate", dtpSummaryDate.Value.Date);
                var totalPurchase = res.CetagoryWiseSummary.Sum(s => (double?)s.TotalPurchase) ?? 0;
                var totalSale = res.CetagoryWiseSummary.Sum(s => (double?)s.TotalSale) ?? 0;
                var totalHOReturn = res.CetagoryWiseSummary.Sum(s => (double?)s.TotalHoReturn) ?? 0;
                var totalAdjustment = res.CetagoryWiseSummary.Sum(a => (double?)a.TotalAdjustment) ?? 0;
                res.ClosingBalance = totalPurchase - totalSale - totalHOReturn + totalAdjustment + res.OpeningBalance;
                r.SetParameterValue("ClosingBalance", res.ClosingBalance);


                crystalReportViewer1.ReportSource = r;
                crystalReportViewer1.Show();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void dtpSummaryDate_ValueChanged(object sender, EventArgs e)
        {
            loadSummaryReport(dtpSummaryDate.Value);
        }
    }
}
