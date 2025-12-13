
namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmConsumeStockUpdate : Form
    {
        UnitOfWork unitOfWork;
        private long StockConumptionId = 0;
        public frmConsumeStockUpdate()
        {
            InitializeComponent();
        }

        public frmConsumeStockUpdate(long StockConsumptionId)
        {
            InitializeComponent();
            this.StockConumptionId = StockConsumptionId;
            unitOfWork = new UnitOfWork();
        }
        private void frmConsumeStockUpdate_Load(object sender, EventArgs e)
        {
            try
            {
                LoadStockConsumption();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void LoadStockConsumption()
        {
            StockConsumptionItem s = unitOfWork.StockConsumptionRepository.GetConsumptionById(this.StockConumptionId);
            List<BatchStockVM> BatchStockList = unitOfWork.ItemRspository.GetBatchStockByItemId((int)s.Item.ItemId);            
            foreach (BatchStockVM i in BatchStockList)
            {
                i.BatchName = i.BatchName + " | Available Stock : " + i.AvailableStock;
            }
            cmbBatchNo.DataSource = BatchStockList;
            cmbBatchNo.ValueMember = "BatchId";
            cmbBatchNo.DisplayMember = "BatchName";

            txtItemName.Text= s.Item.ItemName;
            txtQty.Text = s.Quantity.ToString();
            cmbConsumptionType.SelectedIndex = s.ConsumptionType - 1;
            txtComment.Text = s.Comment;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                StockConsumptionItem s = unitOfWork.StockConsumptionRepository.GetConsumptionById(this.StockConumptionId);
                s.Comment = txtComment.Text;
                s.ConsumptionType = cmbConsumptionType.SelectedIndex + 1;
                //.StockConsumptionRepository.Update(s);
                unitOfWork.Save();
                MessageBox.Show("Stock Consumption Data Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch(Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
