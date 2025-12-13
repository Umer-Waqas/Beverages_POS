

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmChequeInfo : Form
    {
        static AppDbContext cxt = new AppDbContext();
        ChequeInfoRepository repChequeInfo = new ChequeInfoRepository(cxt);
        public ChequeInfo ChequeInfo = new ChequeInfo();
        public frmChequeInfo()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtXhequeNo.Text = "";
            txtBankName.Text = "";
            txtChequeStatus.Text = "";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {               
                this.ChequeInfo.ChequeNo = txtXhequeNo.Text.Trim();
                this.ChequeInfo.BankName = txtBankName.Text.Trim();
                this.ChequeInfo.Status = txtChequeStatus.Text.Trim();                
                //SharedFunctions.ShowSuccessMessage(SharedVariables.GeneralSuccMsg, "Success");
                this.Close();
            }
            catch(Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void frmChequeInfo_Load(object sender, EventArgs e)
        {

        }

        private void bntCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}