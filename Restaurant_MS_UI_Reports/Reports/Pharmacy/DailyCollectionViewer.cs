using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GK.Shared.Core;
using Restaurant_MS_Core.Entities;
using Restaurant_MS_Core.ViewModels;
using GK.Shared.Repository;
using GK.Shared.Repository;

using Restaurant_MS_UI.Menu.Main;

namespace Pharmacy.UI.Reports.Pharmacy
{
    public partial class DailyCollectionViewer : Form
    {
        UnitOfWork unitOfWork;

        public DailyCollectionViewer()
        {
            InitializeComponent();
        }

        private void DeadStockUI_Load(object sender, EventArgs e)
        {
            LoadCollection();
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnOk });
        }
        private void LoadCollection()
        {
            btnOk.PerformClick();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (dtpFrom.Value < dtpTo.Value)
            {
                MessageBox.Show("Start date can't be less than end date.", "Invalid Date Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            loadData();
        }

        private void loadData()
        {
            using (unitOfWork = new UnitOfWork())
            {
                lblTodayCollection.Text = unitOfWork.InvoiceRepository.GetTodayCollection(dtpFrom.Value, dtpTo.Value).ToString();
            }
        }
    }
}