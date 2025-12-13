using Utilities;

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmShowSerialKey : Form
    {
        public frmShowSerialKey()
        {
            InitializeComponent();
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            txtSerialNo.Text = Security.GetSerailNo().ToString();
        }

        private void btnGenerateKey_Click(object sender, EventArgs e)
        {
            long Key = Security.GenerateKey(Convert.ToInt64(txtSerialNo.Text));
            txtKey.Text = Key.ToString();
        }

        private void bntCheck_Click(object sender, EventArgs e)
        {
            if (Security.CheckKey(Convert.ToInt64(txtKey.Text)))
            {
                MessageBox.Show("Activated");
            }
            else
            {
                MessageBox.Show("Incorrenct Key");
            }
        }

        private void frmShowSerialKey_Load(object sender, EventArgs e)
        {

        }
    }
}
