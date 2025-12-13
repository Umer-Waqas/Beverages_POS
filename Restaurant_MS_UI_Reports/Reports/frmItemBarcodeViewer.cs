using GK.Shared.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.Reports
{
    public partial class frmItemBarcodeViewer : Form
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        private int ItemId = 0;
        public frmItemBarcodeViewer()
        {
            InitializeComponent();
        }

        public frmItemBarcodeViewer(int ItemId)
        {
            InitializeComponent();
            this.ItemId = ItemId;
        }

        private void frmItemBarcodeViewer_Load(object sender, EventArgs e)
        {
            Tuple<string, string> barcodeData;
            using (unitOfWork = new UnitOfWork())
            {
                barcodeData = unitOfWork.ItemRspository.GetItemBarcode(this.ItemId);
            }
            //rptItemBarcode r = new rptItemBarcode();
            rptItemBarcode_single r = new rptItemBarcode_single();
            r.SetParameterValue("barcode", "*" + barcodeData.Item1 + "*");
            r.SetParameterValue("itemName", barcodeData.Item2);
            crystalReportViewer1.ReportSource = r;
            crystalReportViewer1.Show();
        }
    }
}