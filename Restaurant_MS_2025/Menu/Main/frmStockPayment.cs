
namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmStockPayment : Form
    {
        UnitOfWork unitOfWork;
        private long StockId = 0;
        private decimal DocNo = 0;
        private long SupplierId = 0;
        private double TotalAmount = 0;
        public DateTime? ActionTime = null;

        public frmStockPayment()
        {
            InitializeComponent();
        }

        public frmStockPayment(long StockId)
        {
            InitializeComponent();
            this.StockId = StockId;
        }

        public frmStockPayment(long StockId, decimal documentNo, long supplierId)
        {
            InitializeComponent();
            this.StockId = StockId;
            this.DocNo = documentNo;
            this.SupplierId = supplierId;
        }

        private void frmStockPayments_Load(object sender, EventArgs e)
        {
            //  lblTotalAmount.Text = this.TotalAmount;
            SupplierStockVM suppStocks = new SupplierStockVM();
            if (SharedFunctions.CheckDayClosed())
            {
                this.BeginInvoke(new MethodInvoker(Close));
                return;
            }

            using (unitOfWork = new UnitOfWork())
            {
                var tpl = unitOfWork.StockRepository.GetStockPaymentTotals(this.StockId);
                lblTotalAmount.Text = tpl.Item1.ToString();
                lblPaid.Text = tpl.Item2.ToString();
                lblRemaining.Text = (tpl.Item1 - tpl.Item2).ToString();
            }


        }

        private void Save()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    double pmt = 0; double.TryParse(txtPayment.Text, out pmt);
                    Expense exp = new Expense();

                    int PaymentType = cmbPaymentMethod.SelectedIndex + 1;
                    exp.description = "Stock Payment(" + this.DocNo + ")";
                    exp.PaymentMode = PaymentType;
                    exp.Date = this.ActionTime.Value;
                    exp.CreatedAt = this.ActionTime.Value;
                    exp.UpdatedAt = this.ActionTime.Value;
                    exp.PracticeId = SharedVariables.PracticeId;
                    exp.UserId = SharedVariables.LoggedInUser.UserId;
                    exp.AutoAdded = false;
                    exp.Amount = pmt;
                    exp.Stock = unitOfWork.StockRepository.GetById(this.StockId);
                    exp.Stock.TotalPaid += pmt;
                    if (exp.Stock.IsSynced)
                    {
                        exp.Stock.IsNew = false;
                        exp.Stock.IsUpdate = true;
                        exp.Stock.IsSynced = false;
                    }

                    unitOfWork.StockRepository.Update(exp.Stock);
                    exp.SupplierId = this.SupplierId;
                    exp.IsActive = true;
                    exp.IsNew = true;
                    exp.VoucherNo = unitOfWork.ExpenseRepository.GetVoucherNo();
                    unitOfWork.ExpenseRepository.Insert(exp);
                    unitOfWork.Save();
                }
                MessageBox.Show("Payments Added Successfully. Press Ok to print", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.ActionTime = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
            Save();

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void btnSavePrint_Click(object sender, EventArgs e)
        {
            Save();
            //StockPaymentViewer viewer = new StockPaymentViewer(this.StockId);
            //viewer.Show();
        }

        private void grdInvoices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}