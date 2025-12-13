

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class DlgAddStockFilter : Form
    {
        UnitOfWork unitOfWork;
        public DateTime? DateFrom;
        public DateTime? DateTo;
        List<StockAddFilterVM> LoadedItems = new List<StockAddFilterVM>();
        public List<StockAddFilterVM> SelectedItems = new List<StockAddFilterVM>();
        private readonly long supplierId;

        public DlgAddStockFilter()
        {
            InitializeComponent();
        }

        public DlgAddStockFilter(long SupplierId)
        {
            InitializeComponent();
            this.supplierId = SupplierId;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            grdItems.Rows.Clear();
            if (dtpFrom.Value > DateTime.Now
                || dtpTo.Value > DateTime.Now)
            {
                MessageBox.Show("Date seletion can't be greater than today.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dtpFrom.Value > dtpTo.Value)
            {
                MessageBox.Show("Date from filter can't be greater than Date to filter.", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!dtpFrom.Checked || !dtpTo.Checked)
            {
                MessageBox.Show("You must select both dates to apply this filter", "Invalid Date Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            using (unitOfWork = new UnitOfWork())
            {
                if (supplierId > 0)
                {
                    LoadedItems = unitOfWork.InvoiceRepository.LoadPreviouslySoldItems(dtpFrom.Value, dtpTo.Value);
                }
                else
                {
                    LoadedItems = unitOfWork.InvoiceRepository.LoadPreviouslySoldItems(dtpFrom.Value, dtpTo.Value);
                }
            }

            if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                foreach (var i in LoadedItems)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ConsumedStock - (i.ExpiredStock - i.ExpiredConsStock);
                    grdItems.Rows.Add(i.ItemId, i.ItemName, i.AvailableStock, i.Quantity, false);
                }
            }
            else
            {
                foreach (var i in LoadedItems)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ConsumedStock - (i.ExpiredStock - i.ExpiredConsStock) + i.HoldStock;
                    grdItems.Rows.Add(i.ItemId, i.ItemName, i.AvailableStock, i.Quantity, false);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            dtpFrom.Checked = false;
            dtpTo.Checked = false;
        }


        private void DlgAddStockFilter_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnApply, btnCancel, btnCancelAll, btnLoad });
            SharedFunctions.SetGridStyle(grdItems);
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            bool chkd = chkAll.Checked;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                r.Cells["colLoad"].Value = chkd;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            List<int> ids = new List<int>();
            //if (chkAll.Checked)
            //{
            //    SelectedItems = LoadedItems;
            //}
            //else
            //{
            int ItemId, Qty = 0;//, AvQty = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                bool chkd = Convert.ToBoolean(r.Cells["colLoad"].Value);
                if (chkd)
                {
                    ItemId = 0; Qty = 0;// AvQty = 0; itemName = "";
                    ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                    var item = LoadedItems.Where(i => i.ItemId == ItemId).FirstOrDefault();
                    if (item != null)
                    {
                        Qty = Convert.ToInt32(r.Cells["colQuantity"].Value);
                        item.Quantity = Qty;
                        SelectedItems.Add(item);
                    }
                }
            }
            //}
            this.Close();
        }
    }
}