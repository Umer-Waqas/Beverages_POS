
using Restaurant_MS_UI;

namespace Restaurant_MS_UI.App.MenuBar
{
    public partial class frmContactUs : Form
    {
        public frmContactUs()
        {
            InitializeComponent();
        }

        private void frmContactUs_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnOk });
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
