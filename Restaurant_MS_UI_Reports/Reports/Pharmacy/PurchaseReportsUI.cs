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
    public partial class PurchaseReportsUI : Form
    {
        UnitOfWork unitOfWork;
        IPagedList<PurchaseReportVM> PurchaseReports;
        int PageNo = 1;
        public PurchaseReportsUI()
        {
            InitializeComponent();
        }

        private void PurchaseReportsUI_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                LoadPurchaseReports();
                LoadSuppliers();
                SharedFunctions.SetGridStyle(grdData);
            }
            catch(Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
        }

        private void LoadPurchaseReports()
        {
            try
            {
                PurchaseReportsMasterVM Master;
                using (unitOfWork = new UnitOfWork())
                {
                    long SupplierId = Convert.ToInt64(cmbSuppliers.SelectedValue);
                    if(SupplierId > 0)
                    {
                        Master = unitOfWork.StockRepository.GetAllPurchases(SupplierId ,PageNo, SharedVariables.PageSize);
                    }
                    else
                    {
                        Master = unitOfWork.StockRepository.GetAllPurchases(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                    }
                }
                this.PurchaseReports = Master.PurchaseReports;
                lblTotalPurchaseAmount.Text = Master.TotalPurchaseAmount.ToString();
                grdData.Rows.Clear();
                foreach (PurchaseReportVM p in Master.PurchaseReports)
                {
                    string PaymentDate = "";
                    if(p.LastPayment!=null)
                    {
                        PaymentDate = p.LastPayment.Date.ToString();
                    }
                    grdData.Rows.Add(p.StockId, p.DocumentNo, p.Supplier.Name, p.StockDate, p.SupplierInvoiceNo, p.SupplierInvoiceDate, Math.Round( p.Amount,2), Math.Round( p.Paid,2), Math.Round(p.Amount - p.Paid,2), PaymentDate);
                }
                btnFirstPage.Enabled = btnPrevious.Enabled = PurchaseReports.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = PurchaseReports.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, PurchaseReports.Count);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        private void LoadSuppliers()
        {
            try
            {
                cmbSuppliers.SelectedIndexChanged -= cmbSuppliers_SelectedIndexChanged;
                List<Supplier> Suppliers;
                using (unitOfWork = new UnitOfWork())
                {
                    Suppliers = unitOfWork.SupplierRepository.GetActiveSuppliers();
                }
                Supplier select = new Supplier { SupplierID = 0, Name = "Select Supplier" };
                Suppliers.Insert(0, select);
                cmbSuppliers.DataSource = Suppliers;
                cmbSuppliers.ValueMember = "SupplierId";
                cmbSuppliers.DisplayMember = "Name";
                cmbSuppliers.SelectedIndexChanged += cmbSuppliers_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            cmbSuppliers.SelectedIndex = 0;
            PageNo = 1;
            LoadPurchaseReports();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            cmbSuppliers.SelectedIndex = 0;
            PageNo = 1;
            LoadPurchaseReports();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadPurchaseReports();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = PurchaseReports.PageCount;
            LoadPurchaseReports();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadPurchaseReports();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadPurchaseReports();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //dlgPrintOptions d = new dlgPrintOptions();
            //d.ShowDialog();
            //if (!d.IsCancelled)
            //{
                //if (d.IsPrint)
                //{
                    PurchaseReportsViewer v = new PurchaseReportsViewer(0, this.dtpFrom.Value, this.dtpTo.Value);
                    v.Show();
                //}
                //else if (d.IsSaveAsPdf)
                //{
                //    SavePdf(d.Orientation);
                //}
            //}
        }

        private void SavePdf(int Orientation)
        {
            try
            {
                List<PurchaseReportVM> Result;
                using (unitOfWork = new UnitOfWork())
                {
                    if (cmbSuppliers.SelectedIndex > 0)
                    {
                        Result = unitOfWork.StockRepository.GetAllPurchases_Rpt(Convert.ToInt32(cmbSuppliers.SelectedValue));
                    }
                    else
                    {
                        Result = unitOfWork.StockRepository.GetAllPurchases_Rpt(this.dtpFrom.Value, this.dtpTo.Value);
                    }
                }
                if (Orientation == 0)
                {
                    Reports.Pharmacy.PurchaseReports_Portrait r = new PurchaseReports_Portrait();
                    r.SetDataSource(Result);
                    r.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        r.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Purchase Reports Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    PurchaseReports_Landscape rpt = new PurchaseReports_Landscape();
                    rpt.SetDataSource(Result);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Purchase Reports Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                List<PurchaseReportVM> Result;
                using (unitOfWork = new UnitOfWork())
                {
                    if (cmbSuppliers.SelectedIndex > 0)
                    {
                        Result = unitOfWork.StockRepository.GetAllPurchases_Rpt(Convert.ToInt32(cmbSuppliers.SelectedValue));
                    }
                    else
                    {
                        Result = unitOfWork.StockRepository.GetAllPurchases_Rpt(this.dtpFrom.Value, this.dtpTo.Value);
                    }
                }
                PurchaseReports_Landscape rpt = new PurchaseReports_Landscape();
                rpt.Database.Tables[0].SetDataSource(Result);
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
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
                    MessageBox.Show("Purchase Reports Exported Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            LoadPurchaseReports();
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > PurchaseReports.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadPurchaseReports();
                }
            }
        }
    }
}
