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
using GK.Shared.Repository;
using GK.Shared.Core;
using Restaurant_MS_Core.Entities;
using Restaurant_MS_Core.ViewModels;

using System.IO;

namespace Pharmacy.UI.Reports.Pharmacy
{
    public partial class LowStockUI : Form
    {
        private UnitOfWork unitOfWork;
        IPagedList<LowStockVM> LowStocksList;
        int PageNo = 1;
        public LowStockUI()
        {
            InitializeComponent();
        }

        private void LowStockUI_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                LoadSuppliers();
                LoadLowStocks();
                SharedFunctions.SetGridStyle(grdData);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
        }

        private void LoadSuppliers()
        {
            cmbSuppliers.SelectedIndexChanged -= cmbSuppliers_SelectedIndexChanged;
            List<Supplier> Suppliers = new List<Supplier>();
            using (unitOfWork = new UnitOfWork())
            {
                Suppliers = unitOfWork.SupplierRepository.GetActiveSuppliers();
            }
            Supplier s = new Supplier
            {
                SupplierID = 0,
                Name = "Select Supplier"
            };
            Suppliers.Insert(0, s);
            cmbSuppliers.DataSource = Suppliers;
            cmbSuppliers.ValueMember = "SupplierID";
            cmbSuppliers.DisplayMember = "Name";
            cmbSuppliers.SelectedIndexChanged += cmbSuppliers_SelectedIndexChanged;
        }

        private void LoadLowStocks()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    long SId = Convert.ToInt64(cmbSuppliers.SelectedValue);
                    if (SId > 0)
                    {
                        LowStocksList = unitOfWork.ItemRspository.GetLowStockItems_BySupplier(SId, PageNo, SharedVariables.PageSize);
                    }
                    else
                    {
                        LowStocksList = unitOfWork.ItemRspository.GetLowStockItems(PageNo, SharedVariables.PageSize);
                    }
                }

                grdData.Rows.Clear();
                foreach (LowStockVM l in LowStocksList)
                {
                    l.ItemsLeft = l.TotalStock - l.ConsumedStock - l.ExpiredStock + l.AdjustedStock;
                    if (l.ItemsLeft <= l.ReOrderingLevel)
                    {
                        grdData.Rows.Add(l.ItemId, l.ItemName, l.ReOrderingLevel, l.ItemsLeft, l.LastStockAddedDate);
                    }
                }
                btnFirstPage.Enabled = btnPrevious.Enabled = LowStocksList.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = LowStocksList.HasPreviousPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, LowStocksList.PageCount);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadLowStocks();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = LowStocksList.PageCount;
            LoadLowStocks();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadLowStocks();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadLowStocks();
        }

        private void cmbSuppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadLowStocks();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<LowStockVM> LowStocksList = GetReportData();
            //dlgPrintOptions d = new dlgPrintOptions();
            //d.ShowDialog();
            //if (!d.IsCancelled)
            //{
            //    if (d.IsPrint)
            //{
            LowStocsViewer v = new LowStocsViewer(0, LowStocksList);
            v.Show();
            //}
            //else if (d.IsSaveAsPdf)
            //{
            //    SavePdf(d.Orientation);
            //}
            //}
        }

        private List<LowStockVM> GetReportData()
        {
            long SId = Convert.ToInt64(cmbSuppliers.SelectedValue);
            List<LowStockVM> Result = new List<LowStockVM>();
            using (unitOfWork = new UnitOfWork())
            {
                if (SId > 0)
                {
                    Result = unitOfWork.ItemRspository.GetLowStockItems_BySupplier_Rpt(SId);
                }
                else
                {
                    Result = unitOfWork.ItemRspository.GetLowStockItems_Rpt();
                }
            }
            foreach (LowStockVM r in Result)
            {
                r.ItemsLeft = r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock;
            }
            return Result;
        }

        private void GetReportsData()
        {
            long SId = Convert.ToInt64(cmbSuppliers.SelectedValue);
            if (SId > 0)
            {
                LowStocksList = unitOfWork.ItemRspository.GetLowStockItems_BySupplier(SId, PageNo, SharedVariables.PageSize);
            }
            else
            {
                LowStocksList = unitOfWork.ItemRspository.GetLowStockItems(PageNo, SharedVariables.PageSize);
            }
        }
        private void SavePdf(int Orientation)
        {
            try
            {
                List<LowStockVM> Result;
                using (unitOfWork = new UnitOfWork())
                {
                    if (cmbSuppliers.SelectedIndex > 0)
                    {
                        Result = unitOfWork.ItemRspository.GetLowStockItems_BySupplier_Rpt(Convert.ToInt32(cmbSuppliers.SelectedValue));
                    }
                    else
                    {
                        Result = unitOfWork.ItemRspository.GetLowStockItems_Rpt();
                    }
                }
                if (Orientation == 0)
                {
                    Reports.Pharmacy.LowStocksRpt_Portrait r = new LowStocksRpt_Portrait();
                    r.SetDataSource(Result);
                    r.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        r.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Transactions Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    LowStocksRpt_Landscape rpt = new LowStocksRpt_Landscape();
                    rpt.SetDataSource(Result);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Transactions Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                List<LowStockVM> Result = GetReportData();
                LowStocksRpt_Landscape rpt = new LowStocksRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(Result);
                if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("ItemWise Sales Exported Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        public string GetLowStockExcelSheet()
        {
            try
            {
                if (SharedVariables.AdminPractiseSetting == null)
                {
                    throw new Exception("Failed to load report headers, please try after login again");
                }
                List<LowStockVM> Result = GetReportData();
                LowStocksRpt_Landscape rpt = new LowStocksRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(Result);
                if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                try
                {
                    string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp");
                    System.IO.Directory.CreateDirectory(filePath);
                    filePath = Path.Combine(filePath, "LowStocksEmail" + SharedFunctions.GetTimestamp(DateTime.Now) + ".xls");
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, filePath);
                    return filePath;
                }
                catch(Exception ex)
                {
                    throw new Exception ("Unable to extract data to excel sheet");
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
                return null;
            }
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > LowStocksList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadLowStocks();
                }
            }
        }
    }
}
