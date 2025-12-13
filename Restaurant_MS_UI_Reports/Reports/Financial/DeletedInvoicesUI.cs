

using Pharmacy.UI.Reports.Pharmacy;

namespace Pharmacy.UI.Reports.Financial
{
    public partial class DeletedInvoicesUI : Form
    {
        private UnitOfWork unitOfWork;
        IPagedList<InvoiceTransactionsVM> Transactions;
        int PageNo = 1;
        bool isPageChanged = false;
        public DeletedInvoicesUI()
        {
            InitializeComponent();
        }

        private void TransactionUI_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetGridStyle(grdData);
        }

        private void LoadTransactions()
        {
            try
            {
                //SummationsVM Sums = new SummationsVM();
                List<SummationsVM> GraphData = new List<SummationsVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    //Sums = unitOfWork.InvoiceRepository.GetSums_Financial(dtpFrom.Value, dtpTo.Value);
                    Transactions = unitOfWork.InvoiceRepository.GetDeletedInvoices_Financial(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                }

                grdData.Rows.Clear();
                string Description = "";
                foreach (InvoiceTransactionsVM t in Transactions.Items)
                {
                    foreach (InvoiceItemVM i in t.InvoiceItems)
                    {
                        Description += i.ItemName + ", ";
                    }
                    if (!string.IsNullOrEmpty(Description))
                    {
                        Description = Description.Trim().TrimEnd(',');
                    }
                    grdData.Rows.Add(t.Patient == null ? "" : t.Patient.Name,  t.InvoiceId, Description, Math.Round(t.GrandTotal,2), Math.Round( t.TotalPaid,2), t.CreatedAt, t.User == null ? "" : t.User.UserName, t.DeletedAt);
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

        private List<InvoiceTransactionsVM> LoadTransactions_Report()
        {
            try
            {                
                List<InvoiceTransactionsVM> Transactions;         
                using (unitOfWork = new UnitOfWork())
                {
                    Transactions = unitOfWork.InvoiceRepository.GetDeletedInvoices_Financial_Report(dtpFrom.Value, dtpTo.Value);
                }
                string Description = "";
                foreach (InvoiceTransactionsVM t in Transactions)
                {
                    foreach (InvoiceItemVM i in t.InvoiceItems)
                    {
                        Description += i.ItemName + ", ";
                    }
                    if (!string.IsNullOrEmpty(Description))
                    {
                        t.Description = Description.Trim().TrimEnd(',');
                    }
                                        
                    if (t.Patient != null)
                    {  
                        t.PatientName = t.Patient.Name;
                    }
                    else
                    {
                        t.PatientName = "";
                    }                    
                }
                return Transactions;
            }
            catch (Exception ex)
            {
                throw ex;
                //SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
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
            if (e.ColumnIndex == grdData.Columns["colPrint"].Index)
            {
                //Reports.Pharmacy.PatientInvoiceViewer v = new PatientInvoiceViewer(InvoiceId);
                //v.Show();
                //return;
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
                    Result = unitOfWork.InvoiceRepository.GetTransactions_Financial_Reports(dtpFrom.Value, dtpTo.Value, 1);
                    foreach (InvoiceTransactionsVM t in Result)
                    {
                        foreach (InvoiceItemVM i in t.InvoiceItems)
                        {
                            Description += i.ItemName + ", ";
                        }
                        if (!string.IsNullOrEmpty(Description))
                        {
                            Description = Description.Trim().TrimEnd(',');
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
            List<InvoiceTransactionsVM> Transactions = LoadTransactions_Report();            
            dlgPrintOptions d = new dlgPrintOptions();
            d.ShowDialog();
            if (!d.IsCancelled)
            {
                if (d.IsPrint)
                {
                    Reports.Financial.RefundViewer v = new RefundViewer(d.Orientation, this.dtpFrom.Value, this.dtpTo.Value, Transactions);
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
                //if (Orientation == 0)
                //{
                //    Reports.Financial.TransactionsRpt_Portrait rpt = new Reports.Financial.TransactionsRpt_Portrait();
                //    rpt.SetDataSource(Transactions);
                //    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                //    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                //    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                //    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                //    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                //    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                //    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //    {
                //        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                //        MessageBox.Show("Financial Transactions Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }
                //}
                //else
                //{
                //    Reports.Financial.TransactionsRpt_Landscape rpt = new Reports.Financial.TransactionsRpt_Landscape();
                //    rpt.SetDataSource(Transactions);
                //    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                //    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                //    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                //    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                //    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                //    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                //    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //    {
                //        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                //        MessageBox.Show("Financial Transactions Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    }
                //}

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
                //rpt.SetDataSource(Transactions);
                //rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                //rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                //rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                //rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                //rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                //rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                //if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                //    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                //    MessageBox.Show("Financial Transactions Data Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
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