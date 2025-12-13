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
    public partial class TransactionUI : Form
    {

        private UnitOfWork unitOfWork;
        IPagedList<InvoiceTransactionsVM> Transactions;
        enum FilterType { Default = 1, InvoiceNo = 2, Date = 3, Patient = 4 };
        FilterType FilterApplied;
        int PageNo = 1;
        public TransactionUI()
        {
            InitializeComponent();
            this.FilterApplied = FilterType.Default;
        }

        private void TransactionUI_Load(object sender, EventArgs e)
        {
            dtpFrom.ValueChanged -= dtpFrom_ValueChanged;
            try
            {
                SharedFunctions.SetGridStyle(grdData);
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint, btnRefresh});
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                LoadTransactions();
                loadPatients();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
            dtpFrom.ValueChanged += dtpFrom_ValueChanged;
        }

        private void loadPatients()
        {
            try
            {
                List<SelectListVM> lst = new List<SelectListVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    lst = unitOfWork.PatientRepository.GetSelectList();
                }
                lst.Insert(0, new SelectListVM { Value = 0, Text = "Select Customer" });
                cmbPatient.DataSource = lst;
                cmbPatient.ValueMember = "Text";
                cmbPatient.ValueMember = "Value";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading patients data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void LoadTransactions()
        {
            if (txtInvoiceNoSearch.Text.Trim() != "")
            {
                this.FilterApplied = FilterType.InvoiceNo;
            }
            try
            {
                SummationsVM Sums = new SummationsVM();
                using (unitOfWork = new UnitOfWork())
                {
                    if (this.FilterApplied == FilterType.Patient)
                    {
                        Sums = unitOfWork.InvoiceRepository.GetSums();
                        Transactions = unitOfWork.InvoiceRepository.GetTransactions_Rpt(Convert.ToInt64(cmbPatient.SelectedValue), PageNo, SharedVariables.PageSize);
                    }
                    else if (this.FilterApplied == FilterType.InvoiceNo)
                    {
                        long invNo = Convert.ToInt64(txtInvoiceNoSearch.Text);
                        Sums = unitOfWork.InvoiceRepository.GetSums(invNo);
                        Transactions = unitOfWork.InvoiceRepository.GetTransactions_rpt(invNo, PageNo, SharedVariables.PageSize);
                    }
                    else if (this.FilterApplied == FilterType.Date)
                    {
                        Sums = unitOfWork.InvoiceRepository.GetSums(dtpFrom.Value, dtpTo.Value);
                        Transactions = unitOfWork.InvoiceRepository.GetTransactions_Rpt(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                    }
                    else //default
                    {
                        Sums = unitOfWork.InvoiceRepository.GetSums();
                        Transactions = unitOfWork.InvoiceRepository.GetTransactions_Rpt(PageNo, SharedVariables.PageSize);
                    }
                }
                lblTotalRevenue.Text = Math.Round((double)Sums.TotalRevenue, 2).ToString();
                if (FilterApplied == FilterType.Default) // no query is being performed in other casses for performance reasons
                {
                    lblRetailValue.Text = Math.Round((double)Sums.RetailValueOfAvailableStock, 2).ToString();
                    lblCostValue.Text = Math.Round((double)Sums.CostOfAvailableStock, 2).ToString();
                }
                grdData.Rows.Clear();
                foreach (InvoiceTransactionsVM t in Transactions)
                {
                    //LastTransaction = index == 0 ? t : Transactions[index - 1];

                    //if(t.DiscountType == 2)
                    //{
                    //    t.TotalDiscount = t.TotalDiscount / 100 * t.SubTotal;
                    //}
                    //if (LastTransaction.TotalRefund < 0 && LastTransaction.InvoiceId!=t.InvoiceId)
                    //{
                    //    grdData.Rows.Add(LastTransaction.InvoiceId.ToString("D6"), LastTransaction.Patient == null ? "" : LastTransaction.Patient.Name, LastTransaction.SubTotal, LastTransaction.TotalRefund, LastTransaction.PaymentDate, LastTransaction.CreatedAt, LastTransaction.IsProcedureInvoice);
                    //}
                    //if(t.TotalPaid > 0)
                    //{                       
                    //   grdData.Rows.Add(t.InvoiceId.ToString("D6"), t.Patient == null ? "" : t.Patient.Name, t.SubTotal, t.TotalPaid, t.PaymentDate, t.CreatedAt, t.IsProcedureInvoice);
                    //}
                    //index++;
                    grdData.Rows.Add(t.InvoiceId, t.InvoiceRefNo, t.Patient == null ? "" : t.Patient.Name, Math.Round(t.SubTotal, 2), Math.Round(t.TotalPaid, 2), t.PaymentDate, t.PaymentCreatedAt, t.IsProcedureInvoice);
                }
                btnFirstPage.Enabled = btnPrevious.Enabled = Transactions.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = Transactions.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, Transactions.PageCount);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            if (dtpTo.Value < dtpFrom.Value)
            {
                dtpTo.Value = dtpFrom.Value;
                MessageBox.Show("Invalid date filter, Date from must be less that Date to.", "Invalid Filter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.FilterApplied = FilterType.Date;
            PageNo = 1;
            LoadTransactions();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {

            if (dtpTo.Value < dtpFrom.Value)
            {
                dtpTo.Value = dtpFrom.Value;
                MessageBox.Show("Invalid date filter, Date from must be less that Date to.", "Invalid Filter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.FilterApplied = FilterType.Date;
            PageNo = 1;
            LoadTransactions();
        }

        private void grdData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            long InvoiceId = Convert.ToInt64(grdData.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
            bool IsProcedureInvoice = Convert.ToBoolean(grdData.Rows[e.RowIndex].Cells["colIsProcedureInvoice"].Value);
            if (e.ColumnIndex == grdData.Columns["colPrint"].Index)
            {
                Reports.POSInvoices.POSInvoiceViewer v = new POSInvoices.POSInvoiceViewer(InvoiceId, true);
                v.Show();
                return;
            }

            if (e.ColumnIndex == grdData.Columns["colEdit"].Index)
            {
                frmInvoice f = new frmInvoice(true, false, InvoiceId);
                f.Show();
                return;
            }

            if (e.ColumnIndex == grdData.Columns["colReturn"].Index)
            {
                frmInvoice f = new frmInvoice(false, true, InvoiceId);
                f.Show();
                return;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                //dlgPrintOptions d = new dlgPrintOptions();
                //d.ShowDialog();
                //if (!d.IsCancelled)
                //{
                List<InvoiceTransactionsVM> Transactions = GetDataForReport();
                //    if (d.IsPrint)
                //    {
                InvoiceTransactionsViewer v = new InvoiceTransactionsViewer(0, this.dtpFrom.Value, this.dtpTo.Value, Transactions);
                v.Show();
                //    }
                //    else if (d.IsSaveAsPdf)
                //    {
                //        SavePdf(d.Orientation, Transactions);
                //    }
                //}
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Failed to open report, please try again.", ex.Message, "Print Failed");
            }

        }

        private List<InvoiceTransactionsVM> GetDataForReport()
        {
            List<InvoiceTransactionsVM> Finaltransactions = new List<InvoiceTransactionsVM>();
            List<InvoiceTransactionsVM> Transactions = new List<InvoiceTransactionsVM>();
            using (unitOfWork = new UnitOfWork())
            {
                if (txtInvoiceNoSearch.Text.Trim() != "")
                {
                    Transactions = unitOfWork.InvoiceRepository.GetTransactions(long.Parse(txtInvoiceNoSearch.Text));
                }
                else
                {
                    Transactions = unitOfWork.InvoiceRepository.GetTransactions(this.dtpFrom.Value, this.dtpTo.Value);
                }
            }
            return Transactions;
            //foreach (InvoiceTransactionsVM t in Transactions)
            //{
            //    if (t.TotalRefund < 0)
            //    {
            //        InvoiceTransactionsVM newT = new InvoiceTransactionsVM();
            //        newT.InvoiceId = t.InvoiceId;
            //        newT.CreatedAt = t.CreatedAt;
            //        newT.TotalAmount = t.TotalAmount;
            //        newT.TotalDiscount = t.TotalDiscount;
            //        newT.DiscountType = t.DiscountType;
            //        newT.GrandTotal = t.GrandTotal;
            //        newT.SubTotal = t.SubTotal;
            //        newT.Due = t.Due;
            //        newT.Advance = t.Advance;
            //        newT.SaleQuantity = t.SaleQuantity;
            //        newT.ReturnedQty = t.ReturnedQty;
            //        newT.PaymentDate = t.PaymentDate;
            //        newT.TotalPaid = t.TotalRefund;
            //        newT.TotalRefund = t.TotalRefund;
            //        newT.Patient = t.Patient;
            //        newT.PatientName = t.PatientName;
            //        newT.IsProcedureInvoice = t.IsProcedureInvoice;
            //        Finaltransactions.Add(newT);
            //    }
            //    Finaltransactions.Add(t);
            //}
            //return Finaltransactions;
        }

        private void SavePdf(int Orientation, List<InvoiceTransactionsVM> Transactions)
        {
            try
            {
                if (Orientation == 0)
                {
                    InvoiceTransactionsRpt_Portrait rpt = new InvoiceTransactionsRpt_Portrait();
                    rpt.SetDataSource(Transactions);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Transactions Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    InvoiceTransactionsRpt_Landscape rpt = new InvoiceTransactionsRpt_Landscape();
                    rpt.SetDataSource(Transactions);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
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
                List<InvoiceTransactionsVM> Transactions = this.GetDataForReport();
                InvoiceTransactionsRpt_Landscape rpt = new InvoiceTransactionsRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(Transactions);
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
                    MessageBox.Show("Transactions Data Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadTransactions();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadTransactions();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadTransactions();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = Transactions.PageCount;
            LoadTransactions();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                PageNo = 1;
                LoadTransactions();
            }
        }

        private void txtInvoiceNoSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > Transactions.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadTransactions();
                }
            }
        }

        private void cmbPatient_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PageNo = 1;
            if (cmbPatient.SelectedIndex > 0)
            {
                FilterApplied = FilterType.Patient;
            }
            else
            {
                FilterApplied = FilterType.Default;
            }
            LoadTransactions();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadTransactions();
        }
    }
}