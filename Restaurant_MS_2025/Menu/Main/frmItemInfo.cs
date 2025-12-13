

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmItemInfo : Form
    {
        enum FilterType { Default = 1, Batch = 2 };
        FilterType filter = FilterType.Default;
        UnitOfWork unitOfWork;
        int ItemId = 0;
        public frmItemInfo()
        {
            InitializeComponent();
        }

        public frmItemInfo(int ItemId)
        {
            InitializeComponent();
            this.ItemId = ItemId;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void LoadData()
        {
            try
            {
                grdConsumptions.Rows.Clear();
                grdStocks.Rows.Clear();
                grdAdjustments.Rows.Clear();
                LoadItemInfo();
                LoadItemStocks();
                LoadStockConsumptions();
                LoadStockAdjusmtents();

            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }


        private void frmItemInfo_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetGridStyle(grdAdjustments);
            SharedFunctions.SetControlForeColor(new[] { lblName, lblItemName, lblSu, lblStockingUnit, lblCategory, lblRL, lblReOrderLvl, lblCtg, lblbcd, lblBarcode });
            SharedFunctions.SetGridStyle(grdConsumptions);
            SharedFunctions.SetGridStyle(grdStocks);
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnGenBC, btnPrintBC, btnClose, btnAddStoc });
            LoadData();
            LoadItemBatches();
        }


        private void LoadItemBatches()
        {
            List<SelectListVM> batches = new List<SelectListVM>();
            using (unitOfWork = new UnitOfWork())
            {
                batches = unitOfWork.BatchRepository.GetItemBatches(this.ItemId);
            }
            batches.Insert(0, new SelectListVM { Value = 0, Text = "Select Batch" });

            cmbBatch.SelectedIndexChanged -= cmbBatch_SelectedIndexChanged;
            cmbBatch.DataSource = batches;
            cmbBatch.ValueMember = "Value";
            cmbBatch.DisplayMember = "Text";
            cmbBatch.SelectedIndexChanged += cmbBatch_SelectedIndexChanged;
        }

        private void LoadItemInfo()
        {
            Item i;
            using (unitOfWork = new UnitOfWork())
            {
                i = unitOfWork.ItemRspository.GetById(this.ItemId);
            }
            lblItemName.Text = i.ItemName;
            lblStockingUnit.Text = i.Unit.ToString();
            lblReOrderLvl.Text = i.ReOrderingLevel.ToString();
            //lblType.Text = i.IsNarcotic ? "Narcotic" : "Drug";
            lblBarcode.Text = i.Barcode;

            btnGenBC.Enabled = string.IsNullOrEmpty(i.Barcode);
            btnPrintBC.Enabled = !string.IsNullOrEmpty(i.Barcode);

        }

        private void LoadItemStocks()
        {
            List<BatchStockVM> BatchStocks;
            using (unitOfWork = new UnitOfWork())
            {
                BatchStocks = unitOfWork.ItemRspository.GetBatchStockByItemId(this.ItemId);
            }
            foreach (BatchStockVM i in BatchStocks)
            {
                i.AvailableStock = i.TotalStock - i.ExpiredStock - i.ConsumedStock + i.AdjustedStock;
                grdStocks.Rows.Add(i.BatchName, (i.TotalStock + i.AdjustedStock), i.AvailableStock, i.Expiry, i.CreatedAt);
            }
        }
        private void LoadStockConsumptions()
        {
            List<StockConsumptionItem> result;
            using (unitOfWork = new UnitOfWork())
            {
                if (filter == FilterType.Batch)
                {
                    result = unitOfWork.StockConsumptionRepository.GetConsStockByBatchId(Convert.ToInt32(cmbBatch.SelectedValue));
                }
                else // default
                {
                    result = unitOfWork.StockConsumptionRepository.GetConsumptionsByItemId(this.ItemId);
                }
            }
            foreach (StockConsumptionItem c in result)
            {
                grdConsumptions.Rows.Add(c.Batch.BatchName, c.Quantity, ((SharedVariables.ConsumptionType)c.ConsumptionType).ToString(), c.CreatedAt);

            }
        }
        private void LoadStockAdjusmtents()
        {
            List<AdjustmentItem> Items;
            using (unitOfWork = new UnitOfWork())
            {
                Items = unitOfWork.AdjustmentRepository.GetStockAdjustmentsByItemId(this.ItemId);
            }
            foreach (AdjustmentItem i in Items)
            {
                grdAdjustments.Rows.Add(i.Batch.BatchName, i.Quantity, i.Reason, i.CreatedAt);
            }
        }
        private void frmItemInfo_Shown(object sender, EventArgs e)
        {
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void frmItemInfo_Activated(object sender, EventArgs e)
        {
            btnRefresh.PerformClick();
        }

        private void btnGenBC_Click(object sender, EventArgs e)
        {
            try
            {
                //string barcode = SharedFunctions.GetTimestamp2(DateTime.Now);
                using (unitOfWork = new UnitOfWork())
                {
                    Item obj = unitOfWork.ItemRspository.GetById(this.ItemId);
                    obj.Barcode = this.ItemId.ToString("D5");
                    unitOfWork.ItemRspository.Update(obj);
                    unitOfWork.Save();
                    lblBarcode.Text = obj.Barcode;
                    btnGenBC.Enabled = false;
                    btnPrintBC.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while generating barcodes, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void btnPrintBC_Click(object sender, EventArgs e)
        {
            //Reports.frmItemBarcodeViewer v = new Reports.frmItemBarcodeViewer(this.ItemId);
            //v.Show();
        }

        private void cmbBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            filter = FilterType.Default;
            int batchId = Convert.ToInt32(cmbBatch.SelectedValue);
            if (batchId > 0)
            {
                filter = FilterType.Batch;
            }
            LoadStockConsumptions();
        }

        private void btnAddStoc_Click(object sender, EventArgs e)
        {
            btn f = new btn(this.ItemId);
            f.Show();
        }
    }
}