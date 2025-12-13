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
using Restaurant_MS_Core.Entities;
using Restaurant_MS_Core.ViewModels;
using GK.Shared.Repository;
using GK.Shared.Repository;

using Restaurant_MS_UI.Menu.Main;

namespace Pharmacy.UI.Reports.Pharmacy
{
    public partial class ProfiStatementUI : Form
    {
        UnitOfWork unitOfWork;
        IPagedList<ProfitStatementVM> Discounts;
        int PageNo = 1;
        public ProfiStatementUI()
        {
            InitializeComponent();
        }

        private void ProfiStatementUI_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                LoadProfitStatements();
                SharedFunctions.SetGridStyle(grdData);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
        }

        private List<ProfitStatementVM> GetReportData()
        {
            List<ProfitStatementVM> PSt = new List<ProfitStatementVM>();
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    PSt = unitOfWork.InvoiceRepository.GetProfitStatements_Reports(dtpFrom.Value, dtpTo.Value);
                }
                foreach (ProfitStatementVM d in PSt)
                {
                    d.Profit = d.TotalRevenue - d.CostOfSales;
                    //d.Profit = d.TotalRevenue - d.CostOfSales - d.Discount;
                    d.Date = Convert.ToDateTime(d.Date).ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return PSt;
        }
        private void LoadProfitStatements()
        {
            try
            {
                ProfitStatementMasterVM vm = new ProfitStatementMasterVM();
                using (unitOfWork = new UnitOfWork())
                {
                    vm = unitOfWork.InvoiceRepository.GetProfitStatements(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                }
                this.Discounts = vm.ProfitStatementPaged;
                double profit = 0;
                grdData.Rows.Clear();
                foreach (ProfitStatementVM d in Discounts)
                {
                    //TotalRevenue += d.TotalRevenue;
                    //NetTotalSales += d.NetTotalSale;
                    //// GrandTotals += d.GrandTotals;
                    //TotalCost += d.CostOfSales;
                    //TotalDiscount += d.Discount;
                    ////profit = d.TotalRevenue-d.CostOfSales-d.Discount;
                    ////if (profit < 0) { profit = 0; }
                    profit = d.TotalRevenue - d.CostOfSales;
                    // (profit < 0) { profit = 0; }
                    grdData.Rows.Add(Convert.ToDateTime(d.Date).ToString("yyyy-MM-dd"), Math.Round(d.TotalRevenue, 2), Math.Round(d.CostOfSales, 2), Math.Round(d.Discount, 2), Math.Round(profit, 2));
                }
                lblTotalSales.Text = Math.Round(vm.TotalRevenue, 2).ToString();
                lblTotalCost.Text = Math.Round(vm.TotalCostOfSales, 2).ToString();
                lblTotalDiscount.Text = Math.Round(vm.TotalDiscount,2).ToString();
                //double TotalProfit = (TotalRevenue - TotalCost - TotalDiscount);
                double TotalProfit = (vm.TotalRevenue - vm.TotalCostOfSales);
                //if (TotalProfit < 0)
                //{
                //    TotalProfit = 0;
                //}
                //else
                //{
                lblTotalProfit.Text = Math.Round(TotalProfit, 2).ToString();
                //}

                btnFirstPage.Enabled = btnPrevious.Enabled = Discounts.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = Discounts.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, Discounts.PageCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadProfitStatements();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadProfitStatements();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadProfitStatements();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = Discounts.PageCount;
            LoadProfitStatements();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadProfitStatements();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadProfitStatements();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<ProfitStatementVM> PStmts = GetReportData();
            //dlgPrintOptions d = new dlgPrintOptions();
            //d.ShowDialog();
            //if (!d.IsCancelled)
            //{
            //    if (d.IsPrint)
            //    {
            ProfitStatementViewer v = new ProfitStatementViewer(0, this.dtpFrom.Value, this.dtpTo.Value, PStmts);
            v.Show();
            //    }
            //    else if (d.IsSaveAsPdf)
            //    {
            //        SavePdf(d.Orientation, PStmts);
            //    }
            //}
        }

        private void SavePdf(int Orientation, List<ProfitStatementVM> ProfitStatements)
        {
            try
            {
                if (Orientation == 0)
                {
                    Reports.Pharmacy.ProfitStatementRpt_Portrair r = new ProfitStatementRpt_Portrair();
                    r.SetDataSource(ProfitStatements);
                    r.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    r.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    r.SetParameterValue("DateTo", this.dtpTo.Value);
                    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        r.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Profit Statements Reports Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    ProfitStatementRpt_Landscape rpt = new ProfitStatementRpt_Landscape();
                    rpt.SetDataSource(ProfitStatements);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Discounts Reports Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (SharedVariables.AdminPractiseSetting == null)
                {
                    throw new Exception("Failed to load report headers, please try after login again");
                }
                List<ProfitStatementVM> Result;
                using (unitOfWork = new UnitOfWork())
                {
                    Result = GetReportData();
                }
                ProfitStatementRpt_Landscape rpt = new ProfitStatementRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(Result);
                if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Profit Statements Report Exported Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadProfitStatements();
        }

        private void chk100Per_CheckedChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadProfitStatements();
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > Discounts.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadProfitStatements();
                }
            }
        }
    }
}