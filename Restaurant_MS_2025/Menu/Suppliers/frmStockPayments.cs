
namespace Restaurant_MS_UI.Menu.Suppliers
{
    public partial class frmStockPayments : Form
    {
        UnitOfWork unitOfWork;
        public DateTime? ActionTime = null; 
        long SupplierId = 0;
        public frmStockPayments()
        {
            InitializeComponent();
        }

        public frmStockPayments(long SupplierId)
        {
            InitializeComponent();
            this.SupplierId = SupplierId;
        }

        private void frmStockPayments_Load(object sender, EventArgs e)
        {
            SupplierStockVM suppStocks = new SupplierStockVM();
            using (unitOfWork = new UnitOfWork())
            {
                suppStocks = unitOfWork.SupplierRepository.GetSupplierStockAndPayments(this.SupplierId);
            }
            lblSupplierName.Text = suppStocks.Supplier.Name;
            double GrandTotal = 0;
            double TotalPaid = 0;
            double Due = 0;
            double varTotalPaid = 0;
            foreach (StockBillVM s in suppStocks.StockPayments)
            {
                varTotalPaid = (double)(s.TotalPaid == null ? 0 : s.TotalPaid);
                TotalPaid += varTotalPaid;
                GrandTotal += s.TotalAmount;
                if ((GrandTotal - TotalPaid) > 0)
                {
                    grdInvoices.Rows.Add(s.StockId, Math.Round(s.DocumentNo), s.SupplierInvoiceNo, s.SupplierInvoiceDate, s.TotalAmount, "Cash", (s.TotalAmount - varTotalPaid));
                }
            }

            Due = GrandTotal - TotalPaid;
            lblGrandTotal.Text = GrandTotal.ToString();
            lblPaidAmount.Text = TotalPaid.ToString();
            lblDue.Text = Due.ToString();
            SharedFunctions.SetGridStyle(grdInvoices);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.ActionTime = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
            try
            {
                List<Expense> PaymentsList = new List<Expense>();
                using (unitOfWork = new UnitOfWork())
                {
                    foreach (DataGridViewRow r in grdInvoices.Rows)
                    {
                        double pmt = Convert.ToDouble(r.Cells["colPaidAmount"].Value);
                        if (pmt <= 0) continue;
                        long StockID = Convert.ToInt64(r.Cells["colStockId"].Value);
                        long DocNum = Convert.ToInt64(r.Cells["colDocumentNo"].Value);
                        Expense exp = new Expense();
                        //InvoicePayment ip = new InvoicePayment();
                        int PaymentType = 1;
                        string paymentMode = r.Cells["colPaymentMode"].Value.ToString().ToLower();
                        switch (paymentMode)
                        {
                            case "cash":
                                PaymentType = 1;
                                break;
                            case "cheque":
                                PaymentType = 2;
                                break;
                            case "debit/credit card":
                                PaymentType = 3;
                                break;
                            case "online payment":
                                PaymentType = 4;
                                break;
                            default:
                                PaymentType = 1;
                                break;
                        }
                        exp.description = "Stock Payment(" + DocNum + ")";
                        exp.ExpenseCategoryId = 1;
                        exp.PaymentMode = PaymentType;
                        exp.Date = this.ActionTime.Value;
                        exp.CreatedAt = this.ActionTime.Value;
                        exp.UpdatedAt = this.ActionTime.Value;
                        exp.PracticeId = SharedVariables.PracticeId;
                        exp.AutoAdded = true;
                        exp.Amount = pmt;
                        exp.Stock = unitOfWork.StockRepository.GetById(StockID);
                        exp.Stock.TotalPaid += pmt;
                        if (exp.Stock.IsSynced)
                        {
                            exp.Stock.IsNew = false;
                            exp.Stock.IsUpdate = true;
                            exp.Stock.IsSynced = false;
                        }   

                        unitOfWork.StockRepository.Update(exp.Stock);
                        exp.Supplier = unitOfWork.SupplierRepository.GetById(SupplierId);
                        exp.IsActive = true;
                        exp.IsNew = true;
                        exp.UserId = SharedVariables.LoggedInUser.UserId;
                        exp.VoucherNo = unitOfWork.ExpenseRepository.GetVoucherNo();
                        unitOfWork.ExpenseRepository.Insert(exp);
                    }
                    unitOfWork.Save();
                    //unitOfWork.StockRepository.InsertStockPayments(PaymentsList);
                }
                MessageBox.Show("Payments Added Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grdInvoices_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if(e.ColumnIndex == grdInvoices.Columns["colPaidAmount"].Index)
            {
                double dueAmt = Convert.ToDouble(grdInvoices.Rows[e.RowIndex].Cells["colDueAmount"].Value);
                double PaidAmt = Convert.ToDouble(grdInvoices.Rows[e.RowIndex].Cells["colPaidAmount"].Value);
                if(PaidAmt > dueAmt)
                {
                    grdInvoices.Rows[e.RowIndex].Cells["colPaidAmount"].Value = dueAmt;
                    MessageBox.Show("Can't pay more than due amount.", "Extra Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
        }

        private void btnSavePrint_Click(object sender, EventArgs e)
        {
        }

        private void grdInvoices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}