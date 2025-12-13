using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_MS_Core.ViewModels;
using GK.Shared.Repository;
using GK.Shared.Core;

namespace Pharmacy.UI.Reports.Pharmacy
{
    public partial class IncomeStatementUI : Form
    {
        UnitOfWork unitOfWork;
        IncomeStatementVM IncomeStatement;
        public IncomeStatementUI()
        {
            InitializeComponent();
        }

        private void IncomeStatementUI_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint });                
                LoadIncomeStatement();
                SharedFunctions.SetGridStyle(grdData);
            }
            catch(Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
        }

        private void LoadIncomeStatement()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    IncomeStatement = unitOfWork.GeneralRepository.GetIncomeStatement(dtpFrom.Value, dtpTo.Value);
                }
                grdData.Rows.Clear();
                grdData.Rows.Add("Total Stock Payments", Math.Round(IncomeStatement.TotalStockPayments,2));
                grdData.Rows.Add("Total Revenue", Math.Round(IncomeStatement.TotalRevenue,2));
                grdData.Rows.Add("NET INCOME", Math.Round(IncomeStatement.TotalRevenue - IncomeStatement.TotalStockPayments,2));
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            LoadIncomeStatement();
        }
        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadIncomeStatement();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //dlgPrintOptions d = new dlgPrintOptions();
            //d.ShowDialog();
            //if (!d.IsCancelled)
            //{
            //    if (d.IsPrint)
            //    {
                    IncomeStatementViewer v = new IncomeStatementViewer(0, this.dtpFrom.Value, this.dtpTo.Value, IncomeStatement);
                    v.Show();
            //    }
            //    else if (d.IsSaveAsPdf)
            //    {
            //        SavePdf(d.Orientation);
            //    }
            //}
        }
        private void SavePdf(int Orientation)
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("StatementType", typeof(string));
                dt.Columns.Add("Value", typeof(double));
                dt.Rows.Add("Total Stock Payments", this.IncomeStatement.TotalStockPayments);
                dt.Rows.Add("Total Revenue", this.IncomeStatement.TotalRevenue);
                dt.Rows.Add("NET INCOME", this.IncomeStatement.TotalRevenue - this.IncomeStatement.TotalStockPayments);
                if (Orientation == 0)
                {
                    Reports.Pharmacy.IncomeStatementRpt_Portrait r = new IncomeStatementRpt_Portrait();
                    r.SetDataSource(dt);
                    r.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    r.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    r.SetParameterValue("DateTo", this.dtpTo.Value);
                    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        r.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Incomestatement Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    IncomeStatementRpt_Landscape rpt = new IncomeStatementRpt_Landscape();
                    rpt.SetDataSource(dt);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("IncomeStatement Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                DataTable dt = new DataTable();
                dt.Columns.Add("StatementType", typeof(string));
                dt.Columns.Add("Value", typeof(double));
                dt.Rows.Add("Total Stock Payments", this.IncomeStatement.TotalStockPayments);
                dt.Rows.Add("Total Revenue", this.IncomeStatement.TotalRevenue);
                dt.Rows.Add("NET INCOME", this.IncomeStatement.TotalRevenue - this.IncomeStatement.TotalStockPayments);

                IncomeStatementRpt_Landscape rpt = new IncomeStatementRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(dt);
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
                    MessageBox.Show("IncomeStatement Exported Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }
    }
}