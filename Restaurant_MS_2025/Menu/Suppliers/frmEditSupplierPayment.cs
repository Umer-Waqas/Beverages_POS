
using Microsoft.EntityFrameworkCore;

namespace Restaurant_MS_UI.Menu.Suppliers
{
    public partial class frmEditSupplierPayment : Form
    {
        public long ExpenseId { get; set; }
        public frmEditSupplierPayment()
        {
            InitializeComponent();
        }

        public frmEditSupplierPayment(long ExpenseId)
        {
            InitializeComponent();
            this.ExpenseId = ExpenseId;
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                using (UnitOfWork uw = new UnitOfWork())
                {
                    var exp = uw.ExpenseRepository.Query().Where(ex => ex.ExpenseId == this.ExpenseId).FirstOrDefault();
                    exp.Date = dtpExpenseDate.Value;
                    exp.Amount = double.Parse(txtAmount.Text);
                    exp.description = txtNotes.Text;
                    exp.PaymentMode = cmbPaymentMode.SelectedIndex + 1;
                    uw.ExpenseRepository.Update(exp);
                    uw.Save();
                    this.Close();
                    MessageBox.Show("Payment updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEditSupplierPayment_Load(object sender, EventArgs e)
        {
            try
            {
                Expense exp = new Expense();
                using (UnitOfWork uw = new UnitOfWork())
                {
                    exp = uw.ExpenseRepository.Query().Where(ex => ex.ExpenseId == this.ExpenseId).Include(ex=>ex.Stock).Include(ex=>ex.Supplier).FirstOrDefault();
                }
                dtpExpenseDate.Value = exp.Date;
                txtAmount.Text = exp.Amount.ToString();
                cmbPaymentMode.SelectedIndex = exp.PaymentMode - 1;
                txtNotes.Text = exp.description;
                lblDocNo.Text = exp.Stock.DocumentNo.ToString();
                lblSupplierName.Text = exp.Supplier.Name;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading expense data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}