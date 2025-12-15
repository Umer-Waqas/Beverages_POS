

using Pharmacy.Core.ViewModels;

namespace Restaurant_MS_UI.Menu.Suppliers
{
    public partial class frmSupplierPaymentHistory : Form
    {
        public long SupplierId { get; set; }
        public decimal? documentNo { get; set; }

        public frmSupplierPaymentHistory()
        {
            InitializeComponent();
        }

        public frmSupplierPaymentHistory(long supplierId, decimal? documentNumber = null)
        {
            InitializeComponent();
            this.SupplierId = supplierId;
            this.documentNo = documentNumber;
        }

        private void frmSupplierPaymentHistory_Load(object sender, EventArgs e)
        {
            this.dtpFromDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (this.documentNo != null)
            {
                loadPaymentHistory(null, null, this.documentNo);
                txtDocumentNo.Text = this.documentNo.ToString();
            }
            else
            {
                loadPaymentHistory(dtpFromDate.Value, dtpToDate.Value, null);
            }
        }

        private void loadPaymentHistory(DateTime? fromDate, DateTime? toDate, decimal? documentNo)
        {
            grdPayments.Rows.Clear();
            List<SupplierPaymentHistoryVM> data = new List<SupplierPaymentHistoryVM>();
            if (SupplierId > 0)
                using (UnitOfWork uw = new UnitOfWork())
                {
                    data = uw.ExpenseRepository.Query()
                        .Where(e => e.SupplierId == SupplierId)
                        .Where(e => (documentNo.HasValue && documentNo.Value > 0 && e.Stock.DocumentNo == documentNo) || (!documentNo.HasValue || documentNo.Value <= 0))
                        .Where(e => (fromDate.HasValue && toDate.HasValue && e.Date.Date >= fromDate.Value.Date && e.Date.Date <= toDate.Value.Date) || (!fromDate.HasValue || !toDate.HasValue))
                        .Select(e => new SupplierPaymentHistoryVM
                         {
                             ExpenseId = e.ExpenseId,
                             StockId = e.StockId.Value,
                             PaymentDate = e.Date,
                             DocumentNo = e.Stock.DocumentNo,
                             Description = e.description,
                             PaidAmount = e.Amount.Value,
                             PaymentMode = e.PaymentMode,
                             SupplierName = e.Supplier.Name
                         }).ToList();
                }
            string PaymentType = "";
            foreach (var d in data)
            {
                switch (d.PaymentMode)
                {
                    case 1:
                        PaymentType = "Cash";
                        break;
                    case 2:
                        PaymentType = "Cheque";
                        break;
                    case 3:
                        PaymentType = "Debit/Credit card";
                        break;
                    case 4:
                        PaymentType = "Online Payment";
                        break;
                    default:
                        PaymentType = "Cash";
                        break;
                }
                grdPayments.Rows.Add(d.ExpenseId, d.StockId, d.DocumentNo, d.PaymentDate, d.Description, PaymentType, d.PaidAmount);
            }
            this.lblSupplierName.Text = data.FirstOrDefault().SupplierName;
        }

        private void txtDocumentNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtDocumentNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDocumentNo.Text))
            {
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                loadPaymentHistory(null, null, decimal.Parse(txtDocumentNo.Text));
            }
        }

        private void grdPayments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            long expenseId = Convert.ToInt64(grdPayments.Rows[e.RowIndex].Cells["colExpenseId"].Value);
            if (e.ColumnIndex == grdPayments.Columns["colEdit"].Index)
            {
                frmEditSupplierPayment f = new frmEditSupplierPayment(expenseId);
                f.FormClosed += new FormClosedEventHandler(frmEditSupplierPayment_Closed);
                f.Show();
            }
            else if (e.ColumnIndex == grdPayments.Columns["colDelete"].Index)
            {
                try
                {
                    var res = MessageBox.Show("Please make sure you want to delete the payment?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == System.Windows.Forms.DialogResult.Yes)
                    {
                        using (UnitOfWork uw = new UnitOfWork())
                        {
                            var exps = uw.ExpenseRepository.Query().Where(ex => ex.ExpenseId == expenseId).FirstOrDefault();
                            exps.UpdatedAt = DateTime.Now;
                            exps.IsActive = false;
                            uw.ExpenseRepository.Update(exps);
                            uw.Save();
                        }
                        MessageBox.Show("Payment deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmEditSupplierPayment_Closed(object sender, FormClosedEventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtDocumentNo.Text))
            {
                this.loadPaymentHistory(dtpFromDate.Value, dtpToDate.Value, null);
            }
            else
            {
                this.loadPaymentHistory(null, null, decimal.Parse(this.txtDocumentNo.Text));
            }
        }
    }
}
