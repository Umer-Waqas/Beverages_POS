using GK.Shared.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.Reports.Accounts
{
    public partial class frmAccountsReportsUI : Form
    {
        public frmAccountsReportsUI()
        {
            InitializeComponent();
        }
        private void frmFinancialReportsUI_Load(object sender, EventArgs e)
        {
        }
        private void btnTransaction_Click(object sender, EventArgs e)
        {
            //SharedFunctions.OpenForm(new TransactionUI(), this.MdiParent);
        }
        private void btnShift_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new BillPaymentsUI(), this.MdiParent);
        }
    }
}