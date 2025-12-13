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

namespace Restaurant_MS_UI.ShortCutDialogs
{
    public partial class frmStockConsumptionsShortCuts : Form
    {
        public frmStockConsumptionsShortCuts()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmStockConsumptionsShortCuts_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnClose, btnClose2 });
        }
    }
}
