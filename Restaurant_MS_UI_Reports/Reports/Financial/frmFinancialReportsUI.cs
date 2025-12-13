
namespace Pharmacy.UI.Reports.Financial
{
    public partial class frmFinancialReportsUI : Form
    {
        public frmFinancialReportsUI()
        {
            InitializeComponent();
        }

        private void btnTransaction_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new TransactionUI(), this.MdiParent);
        }

        private void frmFinancialReportsUI_Load(object sender, EventArgs e)
        {

        }

        private void btnShift_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new ShiftUI(), this.MdiParent);
        }

        private void btnPaymentMode_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new PaymentModeUI(), this.MdiParent);
        }

        private void btnPendingPayments_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new PendingPayemntUI(), this.MdiParent);
        }

        private void btnRefundReport_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new RefundUI(), this.MdiParent);
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}