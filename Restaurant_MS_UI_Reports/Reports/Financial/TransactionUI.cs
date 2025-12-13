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
using Pharmacy.UI.Reports.Pharmacy;
using System.Globalization;

namespace Pharmacy.UI.Reports.Financial
{
    public partial class TransactionUI : Form
    {
        private UnitOfWork unitOfWork;
        IPagedList<InvoiceTransactionsVM> Transactions;
        int PageNo = 1;
        bool isPageChanged = false;
        public TransactionUI()
        {
            InitializeComponent();
        }

        private void TransactionUI_Load(object sender, EventArgs e)
        {
            cmbPaymentMode.SelectedIndex = 0;
            SharedFunctions.SetGridStyle(grdData);
        }

        private void LoadTransactions()
        {
            try
            {
                long invNo = 0;
                int PaymentMode = cmbPaymentMode.SelectedIndex; // index is matching actual values in db, otherwise will need to update this row of code
                SummationsVM Sums = new SummationsVM();
                List<SummationsVM> GraphData = new List<SummationsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    if (txtInvoiceNoSearch.Text.Trim() != "")
                    {
                        invNo = Convert.ToInt64(txtInvoiceNoSearch.Text);
                        Sums = unitOfWork.InvoiceRepository.GetSums_Financial(invNo);
                        GraphData = unitOfWork.InvoiceRepository.GetTransactionsByInvNo_Financial_Graph(invNo, PaymentMode);
                        Transactions = unitOfWork.InvoiceRepository.GetTransactionsByInvNo_Financial(invNo, PaymentMode, PageNo, SharedVariables.PageSize);
                    }
                    else
                    {
                        Sums = unitOfWork.InvoiceRepository.GetSums_Financial(dtpFrom.Value, dtpTo.Value);
                        Transactions = unitOfWork.InvoiceRepository.GetTransactions_Financial(dtpFrom.Value, dtpTo.Value, PaymentMode, PageNo, SharedVariables.PageSize);
                        GraphData = unitOfWork.InvoiceRepository.GetTransactions_Financial_Graph(dtpFrom.Value, dtpTo.Value, PaymentMode);
                    }
                }
                if (Sums != null)
                {
                    lblTotalRevenue.Text = Sums.TotalRevenue == null ? "" : Math.Round((double)Sums.TotalRevenue, 2).ToString();
                    lblMostRevenueValue.Text = Sums.MaxMonthRevenue == null ? "" : Math.Round((double)Sums.MaxMonthRevenue, 2).ToString();
                    if (Sums.Month != null)
                    {
                        lblMostRevenueMonth.Text = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName((int)Sums.Month).ToString() + " " + Sums.Year.ToString();
                    }
                }
                else
                {
                    lblTotalRevenue.Text = "0";
                    lblMostRevenueValue.Text = "0";
                    lblMostRevenueMonth.Text = "";
                }

                grdData.Rows.Clear();
                string Description = "";
                string PaymentType = "";
                foreach (InvoiceTransactionsVM t in Transactions)
                {
                    Description = "";
                    if (t.IsProcedureInvoice)
                    {
                        foreach (InvoiceItemVM i in t.InvoiceItems)
                        {
                            Description += i.ProcedureName + ", ";
                        }
                        if (!string.IsNullOrEmpty(Description))
                        {
                            Description = Description.Trim().TrimEnd(',');
                        }
                    }
                    else
                    {
                        foreach (InvoiceItemVM i in t.InvoiceItems)
                        {
                            Description += i.ItemName + ", ";
                        }
                        if (!string.IsNullOrEmpty(Description))
                        {
                            Description = Description.Trim().TrimEnd(',');
                        }
                    }

                    foreach (InvoicePayment p in t.InvoicePayments)
                    {
                        switch (p.PaymentType)
                        {
                            case 1: PaymentType = "Cash"; break;
                            case 2: PaymentType = "Cheque"; break;
                            case 3: PaymentType = "Debit/Credit Card"; break;
                            case 4: PaymentType = "Online Payment"; break;
                            default: PaymentType = "Cash"; break;
                        }
                        grdData.Rows.Add(t.InvoiceId, t.InvoiceRefNo, t.Patient == null ? "" : t.Patient.Name, Description, Math.Round(t.GrandTotal, 2), p.Payment, Math.Round(t.TotalDiscount, 2), t.Due > 0 ? Math.Round(t.Due, 2) : 0, t.Due < 0 ? Math.Round(-1 * t.Due, 2) : 0, PaymentType, p.PaymentDate, t.IsProcedureInvoice);
                    }
                }
                btnFirstPage.Enabled = btnPrevious.Enabled = Transactions.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = Transactions.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, Transactions.PageCount);

                chart1.DataSource = GraphData;
                chart1.Series["Retail Value"].XValueMember = "Date";
                chart1.Series["Retail Value"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                chart1.Series["Retail Value"].YValueMembers = "MaxMonthRevenue";
                chart1.Series["Retail Value"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;

                //chartRetailValue.DataSource = GraphData;
                //chartRetailValue.Series["Retail Value"].XValueMember = "Date";
                //chartRetailValue.Series["Retail Value"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
                //chartRetailValue.Series["Retail Value"].YValueMembers = "MaxMonthRevenue";
                //chartRetailValue.Series["Retail Value"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadTransactions();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
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

                //Reports.Pharmacy.PatientInvoiceViewer v = new PatientInvoiceViewer(InvoiceId);
                //v.Show();
                //return;
            }

            //if (e.ColumnIndex == grdData.Columns["colEdit"].Index)
            //{
            //    frmInvoice f = new frmInvoice(true, false, InvoiceId);
            //    f.Show();
            //    return;
            //}

            //if (e.ColumnIndex == grdData.Columns["colReturn"].Index)
            //{
            //    frmInvoice f = new frmInvoice(false, true, InvoiceId);
            //    f.Show();
            //    return;
            //}
        }

        private List<InvoiceTransactionsVM> GetReportData()
        {
            try
            {
                List<InvoiceTransactionsVM> Result;
                List<InvoiceTransactionsVM> FinalResult = new List<InvoiceTransactionsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    string Description = "";
                    string PaymentType = "";
                    Result = unitOfWork.InvoiceRepository.GetTransactions_Financial_Reports(dtpFrom.Value, dtpTo.Value, cmbPaymentMode.SelectedIndex);
                    foreach (InvoiceTransactionsVM t in Result)
                    {
                        Description = "";
                        if (t.IsProcedureInvoice)
                        {
                            foreach (InvoiceItemVM i in t.InvoiceItems)
                            {
                                Description += i.ProcedureName + ", ";
                            }
                            if (!string.IsNullOrEmpty(Description))
                            {
                                Description = Description.Trim().TrimEnd(',');
                            }
                        }
                        else
                        {
                            foreach (InvoiceItemVM i in t.InvoiceItems)
                            {
                                Description += i.ItemName + ", ";
                            }
                            if (!string.IsNullOrEmpty(Description))
                            {
                                Description = Description.Trim().TrimEnd(',');
                            }
                        }

                        foreach (InvoicePayment p in t.InvoicePayments)
                        {
                            switch (p.PaymentType)
                            {
                                case 1: PaymentType = "Cash"; break;
                                case 2: PaymentType = "Cheque"; break;
                                case 3: PaymentType = "Debit/Credit Card"; break;
                                case 4: PaymentType = "Online Payment"; break;
                                default: PaymentType = "Cash"; break;
                            }
                            InvoiceTransactionsVM obj = new InvoiceTransactionsVM();
                            obj.InvoiceId = t.InvoiceId;
                            if (t.Patient != null)
                            {
                                obj.PatientName = t.Patient.Name;
                            }
                            else
                            {
                                obj.PatientName = "";
                            }
                            obj.Description = Description;
                            obj.SubTotal = t.SubTotal;
                            obj.GrandTotal = t.GrandTotal;
                            obj.TotalPaid = p.Payment;
                            obj.TotalDiscount = t.TotalDiscount;
                            obj.Due = t.Due;
                            obj.Advance = t.Advance;
                            obj.PaymentType = PaymentType;
                            obj.PaymentDate = p.PaymentDate;
                            FinalResult.Add(obj);
                        }
                    }
                }
                return FinalResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<InvoiceTransactionsVM> Transactions = GetReportData();
            int PaymentMode = cmbPaymentMode.SelectedIndex;
            dlgPrintOptions d = new dlgPrintOptions();
            d.ShowDialog();
            if (!d.IsCancelled)
            {
                if (d.IsPrint)
                {
                    Reports.Financial.TrancationsViewer v = new TrancationsViewer(d.Orientation, this.dtpFrom.Value, this.dtpTo.Value, Transactions);
                    v.Show();
                }
                else if (d.IsSaveAsPdf)
                {
                    SavePdf(d.Orientation, Transactions);
                }
            }
        }

        private void SavePdf(int Orientation, List<InvoiceTransactionsVM> Transactions)
        {
            try
            {
                if (Orientation == 0)
                {
                    Reports.Financial.TransactionsRpt_Portrait rpt = new Reports.Financial.TransactionsRpt_Portrait();
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
                        MessageBox.Show("Financial Transactions Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    Reports.Financial.TransactionsRpt_Landscape rpt = new Reports.Financial.TransactionsRpt_Landscape();
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
                        MessageBox.Show("Financial Transactions Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                List<InvoiceTransactionsVM> Transactions = GetReportData();
                Reports.Financial.TransactionsRpt_Landscape rpt = new TransactionsRpt_Landscape();
                rpt.SetDataSource(Transactions);
                rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Financial Transactions Data Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void cmbPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadTransactions();
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
    }
}