

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmInvoiceEdit : Form
    {
        static AppDbContext cxt = new AppDbContext();
        ItemsRepository repItems = new ItemsRepository(cxt);
        InvoiceRepository repInvoice = new InvoiceRepository(cxt);
        UnitOfWork unitOfWork = new UnitOfWork();
        PatientRepository repPatients = new PatientRepository(cxt);
        List<BatchStockVM> BatchStockList = new List<BatchStockVM>();
        Invoice objInvoice = new Invoice();
        DataTable dtStockConsumption = new DataTable();
        long PatientId = 0;
        long InvoiceId = 0;

        bool IsInvoiceEdit = false;
        long InvoiceEditId = 0;
        public frmInvoiceEdit()
        {
            InitializeComponent();
            objInvoice.InvoiceItems = new List<InvoiceItem>();
        }
        public frmInvoiceEdit(long InvoiceId)
        {
            InitializeComponent();
            objInvoice.InvoiceItems = new List<InvoiceItem>();
            this.InvoiceId = InvoiceId;
        }

        private void frmConsumeStock_Load(object sender, EventArgs e)
        {
            if (IsInvoiceEdit && InvoiceEditId > 0)
            {
                LoadInvoice(this.InvoiceEditId);
            }
            if (this.PatientId > 0)// patient invoice
            {
                LoadPateint();
            }
            cmbSelectItems.SelectedIndexChanged -= cmbSelectItems_SelectedIndexChanged;
            LoadItems();
            cmbSelectItems.SelectedIndexChanged += cmbSelectItems_SelectedIndexChanged;
            cmbDiscountType.SelectedIndex = 0;
            cmbPmntType1.SelectedIndex = 0;
            cmbPmntType2.SelectedIndex = 0;
            cmbPmntType3.SelectedIndex = 0;
            cmbPmntType4.SelectedIndex = 0;
            cmbTotalDiscountType.SelectedIndex = 0;
            cmbSelectItems.SelectedIndex = -1;
            SharedFunctions.GenerateStockConsumptionTable(dtStockConsumption);
        }
        private void LoadPateint()
        {
            Patient objPatient = repPatients.GetById(this.PatientId);
            txtSearchPatient.Text = objPatient.Name;
        }
        private void LoadInvoice(long InvoiceId)
        {
            InvoiceVM objInvoice = repInvoice.GetInvoiceByInvoiceId(InvoiceId);
            txtSearchPatient.Text = objInvoice.Patient.Name;
            this.PatientId = objInvoice.Patient.PatientId;
            lblInvoiceNo.Text = objInvoice.InvoiceId.ToString();
            pnlLoadedInvoiceData.Visible = true;
            foreach (InvoiceItemVM i in objInvoice.InvoiceItems)
            {

            }
        }
        private void LoadItems()
        {
            List<Item> items = repItems.GetAll().ToList();
            cmbSelectItems.DataSource = items;
            cmbSelectItems.ValueMember = "ItemId";
            cmbSelectItems.DisplayMember = "ItemName";
        }

        private Tuple<double, double, double> GetGrandTotals()
        {
            double subTotal = 0;
            double GrandTotal = 0;
            double Discount = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                double colAmount = Convert.ToDouble(r.Cells["colAmount"].Value);
                double colDiscount = Convert.ToDouble(r.Cells["colDiscount"].Value);
                int colDiscountType = Convert.ToInt32(r.Cells["colDiscountTypeid"].Value);
                subTotal += colAmount;
                if (colDiscountType == 2)
                {
                    colDiscount = (colDiscount / 100) * colAmount;
                }
                Discount += colDiscount;
            }
            GrandTotal = subTotal - Discount;
            return new Tuple<double, double, double>(subTotal, GrandTotal, Discount);
        }
        private void CalculateTotals()
        {

            Tuple<double, double, double> GrandTotals = GetGrandTotals();
            double SubTotal = GrandTotals.Item1;
            double GrandTotal = GrandTotals.Item2;
            double TotalDiscount = GrandTotals.Item3;

            double rate = (double)numRate.Value;
            int qty = (int)numQuantity.Value;
            double amount = rate * qty;
            double discount = (double)numDiscount.Value;
            string discountType = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem);

            if (discountType.ToLower() == "%")
            {
                discount = (discount / 100) * amount;
            }
            SubTotal += amount;
            GrandTotal += amount - discount;
            TotalDiscount += discount;
            numTotalDiscount.Value = (decimal)TotalDiscount;
            txtAmount.Text = amount.ToString();
            lblSubTotal.Text = SubTotal.ToString();
            lblGrandTotal.Text = GrandTotal.ToString();
            GetCustomerTotals();
        }
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (IsValidInput())
            {
                if (ItemAlreadyAdded(Convert.ToInt32(cmbSelectItems.SelectedValue)))
                {
                    MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                AddItemtoGridAndList();
                Clear();
            }
        }

        private void AddItemtoGridAndList()
        {
            InvoiceItem objInvoiceItem = new InvoiceItem();
            FillObject(objInvoiceItem);
            objInvoice.InvoiceItems.Add(objInvoiceItem);
            grdItems.Rows.Add(objInvoiceItem.ItemId, objInvoiceItem.Item.ItemName, objInvoiceItem.BatchId, objInvoiceItem.Batch.BatchName, objInvoiceItem.Rate, objInvoiceItem.Quantity, objInvoiceItem.Amount, objInvoiceItem.Discount, objInvoiceItem.DiscountType.ToString(), objInvoiceItem.DiscountType, true);
        }
        /// <summary>
        /// This Method is Called When Invoice Is Loaded For Editing Purpose
        /// </summary>
        /// <param name="objInvoiceItem"></param>
        private void AddItemtoGridAndList(InvoiceItemVM objInvoiceItem)
        {
            objInvoiceItem.DiscountTypeString = objInvoiceItem.DiscountType == 1 ? "Value" : "%";
            grdItems.Rows.Add(objInvoiceItem.ItemId, objInvoiceItem.ItemName, objInvoiceItem.BatchId, objInvoiceItem.BatchName, objInvoiceItem.Rate, objInvoiceItem.Quantity, objInvoiceItem.Amount, objInvoiceItem.Discount, objInvoiceItem.DiscountTypeString, objInvoiceItem.DiscountType, false);
        }

        private void FillObject(InvoiceItem objInvoiceItem)
        {
            objInvoiceItem.ItemId = Convert.ToInt32(cmbSelectItems.SelectedValue);
            objInvoiceItem.Item.ItemName = cmbSelectItems.GetItemText(cmbSelectItems.SelectedItem);
            objInvoiceItem.BatchId = Convert.ToInt32(cmbSelectBatch.SelectedValue);
            objInvoiceItem.Batch.BatchName = cmbSelectBatch.GetItemText(cmbSelectBatch.SelectedItem);
            objInvoiceItem.Rate = (double)numRate.Value;
            objInvoiceItem.Quantity = (int)numQuantity.Value;
            objInvoiceItem.Amount = double.Parse(txtAmount.Text);
            objInvoiceItem.Discount = (double)numDiscount.Value;
            objInvoiceItem.DiscountType = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem).ToLower() == "value" ? 1 : 2;
            //objInvoiceItem.DiscountType = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem);
        }

        private bool IsValidInput()
        {
            bool ErrFound = false;
            if (Convert.ToInt32(cmbSelectItems.SelectedValue) < 0)
            {
                ErrSelectItem.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrSelectItem.Visible = false;
            }
            if (cmbSelectBatch.SelectedValue == null)
            {
                ErrSelectBatch.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrSelectBatch.Visible = false;
            }
            if (Convert.ToDouble(numRate.Value) <= 0)
            {
                errRate.Visible = true;
                ErrFound = true;
            }
            else
            {
                errRate.Visible = false;
            }
            if (Convert.ToInt32(numQuantity.Value) <= 0)
            {
                ErrQuantity.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrQuantity.Visible = false;
            }
            if (Convert.ToUInt32(numQuantity.Value) <= 0)
            {
                ErrQuantity.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrQuantity.Visible = false;
            }
            if (!ErrFound)
            {
                ErrMessage.Visible = false;
                return true;
            }
            else
            {
                ErrMessage.Visible = true;
                return false;
            }
        }

        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            InvoiceItem objRemove = objInvoice.InvoiceItems.Where(i => i.ItemId == Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value)).FirstOrDefault();
            if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
            {
                FillForm(grdItems.Rows[e.RowIndex]);
                dtStockConsumption.Rows.RemoveAt(e.RowIndex);
                objInvoice.InvoiceItems.Remove(objRemove);
                grdItems.Rows.RemoveAt(e.RowIndex);
            }
            if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
            {
                objInvoice.InvoiceItems.Remove(objRemove);
                dtStockConsumption.Rows.RemoveAt(e.RowIndex);
                grdItems.Rows.RemoveAt(e.RowIndex);
            }
        }

        private void FillForm(DataGridViewRow r)
        {
            cmbSelectItems.SelectedValue = Convert.ToInt32(r.Cells["colItemId"].Value);
            cmbSelectBatch.SelectedValue = Convert.ToInt32(r.Cells["colBatchId"].Value);
            numRate.Value = Convert.ToDecimal(r.Cells["colRate"].Value);
            numQuantity.Value = Convert.ToInt32(r.Cells["colQuantity"].Value);
            txtAmount.Text = r.Cells["colAmount"].Value.ToString();
            numDiscount.Value = Convert.ToDecimal(r.Cells["colDiscount"].Value);
            cmbDiscountType.SelectedItem = r.Cells["colDiscounttype"].ToString();
        }

        private bool ItemAlreadyAdded(int ItemId)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == ItemId)
                {
                    return true;
                }
            }
            return false;
        }

        private void cmbSelectItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbSelectBatch.DataSource = null;
            cmbSelectBatch.SelectedIndex = -1;
            if (cmbSelectItems.SelectedIndex >= 0)
            {
                cmbSelectBatch.SelectedIndexChanged -= cmbSelectBatch_SelectedIndexChanged;
                GetItemBatches(Convert.ToInt32(cmbSelectItems.SelectedValue));
                cmbSelectBatch.SelectedIndexChanged += cmbSelectBatch_SelectedIndexChanged;
            }
            if (cmbSelectBatch.Items.Count > 0)
            {
                cmbSelectBatch.SelectedIndex = 0;
                cmbSelectBatch_SelectedIndexChanged(null, null);
            }
        }

        private void GetItemBatches(int ItemId)
        {
            BatchStockList = repItems.GetBatchStockByItemId(ItemId);
            List<BatchStockVM> FinalResult = new List<BatchStockVM>();
            foreach (BatchStockVM i in BatchStockList)
            {
                //i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock);
                i.BatchName = i.BatchName + " | Available Stock : " + i.AvailableStock;
                if (i.AvailableStock > 0)
                {
                    FinalResult.Add(i);
                }
            }
            cmbSelectBatch.DataSource = FinalResult;
            cmbSelectBatch.ValueMember = "BatchId";
            cmbSelectBatch.DisplayMember = "BatchName";
        }

        private bool isValidQuantityEntered()
        {
            double AvailableStock = BatchStockList.Where(x => x.BatchId == Convert.ToInt32(cmbSelectBatch.SelectedValue)).FirstOrDefault().AvailableStock;
            if (numQuantity.Value > (decimal)AvailableStock)
            {
                ToolTip tt = new ToolTip();
                numQuantity.Value = 1;
                tt.Show("Please Enter Valid Quantity", numQuantity, 800);
                numQuantity.Select(0, 1);
                return false;
            }
            return true;
        }
        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {
            if (cmbSelectBatch.SelectedIndex >= 0)
            {
                isValidQuantityEntered();
                CalculateTotals();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            cmbSelectItems.SelectedIndex = -1;
            cmbSelectBatch.SelectedIndex = -1;
            numQuantity.Value = 1;
        }

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            ClearAll(false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            TransactionScope scope;
            using (scope = new TransactionScope())
            {
                try
                {
                    //Invoice objInvoice = new Invoice();
                    objInvoice.CreatedAt = DateTime.Now;
                    objInvoice.SubTotal = double.Parse(lblSubTotal.Text);
                    objInvoice.GrandTotal = double.Parse(lblGrandTotal.Text);
                    objInvoice.TotalDiscount = Convert.ToDouble(numTotalDiscount.Value);
                    objInvoice.DiscountType = cmbTotalDiscountType.GetItemText(cmbTotalDiscountType.SelectedItem).ToLower() == "value" ? 1 : 2;
                    objInvoice.Due = double.Parse(lblDue.Text);
                    foreach (InvoiceItem i in objInvoice.InvoiceItems)
                    {
                        i.Item = unitOfWork.ItemRspository.GetById(i.ItemId);
                        i.Batch = unitOfWork.BatchRepository.GetById(i.BatchId);
                        dtStockConsumption.Rows.Add(i.Quantity, (int)SharedVariables.ConsumptionType.Sales, "Sale Transaction", i.BatchId, i.ItemId, null, DateTime.Now);
                    }
                    if (!AddInvoicePayments(objInvoice))
                    {
                        return;
                    }
                    if (double.Parse(lblDue.Text) > 0)
                    {
                        MessageBox.Show("Due Amount is (" + lblDue.Text.ToString() + " ). You Can't Insert Invoice With Due Payments.", "Due Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (PatientId > 0)
                    {
                        objInvoice.Patient = unitOfWork.PatientRepository.GetById(PatientId);
                    }
                    long invoiceId = unitOfWork.InvoiceRepository.InsertInvoice(objInvoice);
                    foreach (DataRow r in dtStockConsumption.Rows)
                    {
                        r["Invoice_InvoiceId"] = invoiceId;
                    }
                    //unitOfWork.StockConsumptionRepository.BulkInsert(dtStockConsumption);
                    scope.Complete();
                    SharedFunctions.ShowSuccessMessage(SharedVariables.GeneralSuccMsg, "Success");
                    ClearAll(true);
                }
                catch (Exception ex)
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                    scope.Dispose();
                }
            }
        }

        private bool AddInvoicePayments(Invoice objInvoice)
        {
            objInvoice.InvoicePayments = new List<InvoicePayment>();
            if (numPmnt1.Value <= 0)
            {
                MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            InvoicePayment objIp = new InvoicePayment();
            objIp.PaymentType = cmbPmntType1.SelectedIndex + 1;
            objIp.Payment = (double)numPmnt1.Value;
            objIp.PaymentDate = dtpPmntType1.Value;
            if (objIp.PaymentType == 2)//cheque selected
            {
                if (txtChequeInfoId1.Text != "")
                {
                    long chequeId = Convert.ToInt64(txtChequeInfoId1.Text);
                    //objIp.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                }
                else
                {
                    DialogResult rs = MessageBox.Show("Cheque Details Not Found For First Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs != DialogResult.OK)
                    {
                        return false;
                    }
                }
            }
            objInvoice.InvoicePayments.Add(objIp);

            if (chkPmnt2.Checked)
            {
                if (numPmnt2.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp2 = new InvoicePayment();
                objIp2.PaymentType = cmbPmntType2.SelectedIndex + 1;
                objIp2.Payment = (double)numPmnt2.Value;
                objIp2.PaymentDate = dtpPmntType2.Value;
                if (objIp2.PaymentType == 2)//cheque selected
                {
                    if (txtChequeInfoId2.Text != "")
                    {
                        long chequeId = Convert.ToInt64(txtChequeInfoId2.Text);
                        //objIp2.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                    }
                    else
                    {
                        DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                }
                objInvoice.InvoicePayments.Add(objIp2);
            }

            if (chkPmnt3.Checked)
            {
                if (numPmnt3.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp3 = new InvoicePayment();
                objIp3.PaymentType = cmbPmntType3.SelectedIndex + 1;
                objIp3.Payment = (double)numPmnt3.Value;
                objIp3.PaymentDate = dtpPmntType3.Value;
                if (objIp3.PaymentType == 2)//cheque selected
                {
                    if (txtChequeInfoId3.Text != "")
                    {
                        long chequeId = Convert.ToInt64(txtChequeInfoId3.Text);
                        // objIp3.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                    }
                    else
                    {
                        DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                }
                objInvoice.InvoicePayments.Add(objIp3);
            }

            if (chkPmnt4.Checked)
            {
                if (numPmnt4.Value <= 0)
                {
                    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                InvoicePayment objIp4 = new InvoicePayment();
                objIp4.PaymentType = cmbPmntType4.SelectedIndex + 1;
                objIp4.Payment = (double)numPmnt4.Value;
                objIp4.PaymentDate = dtpPmntType4.Value;
                if (objIp4.PaymentType == 2)//cheque selected
                {
                    if (txtChequeInfoId4.Text != "")
                    {
                        long chequeId = Convert.ToInt64(txtChequeInfoId4.Text);
                        //objIp4.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                    }
                    else
                    {
                        DialogResult rs = MessageBox.Show("Cheque Details Not Found For Second Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (rs != DialogResult.OK)
                        {
                            return false;
                        }
                    }
                }
                objInvoice.InvoicePayments.Add(objIp4);
            }
            return true;
        }
        private void btnAddPayment_Click(object sender, EventArgs e)
        {
            pnlPmnt2.Visible = true;
        }

        private void chkPmnt2_CheckedChanged(object sender, EventArgs e)
        {
            pnlPmnt2.Enabled = chkPmnt2.Checked;
        }

        private void chkPmnt3_CheckedChanged(object sender, EventArgs e)
        {
            pnlPmnt3.Enabled = chkPmnt3.Checked;
        }

        private void chkPmnt4_CheckedChanged(object sender, EventArgs e)
        {
            pnlPmnt4.Enabled = chkPmnt4.Checked;
        }

        private void numDiscount_ValueChanged(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        private void numRate_ValueChanged(object sender, EventArgs e)
        {
            CalculateTotals();
        }

        private void cmbDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalculateTotals();
        }
        private long showChequeInfoDialog()
        {
            //frmChequeInfo f = new frmChequeInfo();
            //f.ShowDialog();
            //long ChequeInfoId = f.ChequeInfoId;
            //return ChequeInfoId;
            return 0;
        }
        private void cmbPmntType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPmntType1.SelectedIndex == 1)
            {
                long id = showChequeInfoDialog();
                txtChequeInfoId1.Text = id == 0 ? "" : id.ToString();
            }
        }

        private void cmbPmntType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPmntType2.SelectedIndex == 1)
            {
                long id = showChequeInfoDialog();
                txtChequeInfoId2.Text = id == 0 ? "" : id.ToString();
            }
        }
        private void cmbPmntType3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPmntType3.SelectedIndex == 1)
            {
                long id = showChequeInfoDialog();
                txtChequeInfoId3.Text = id == 0 ? "" : id.ToString();
            }
        }
        private void cmbPmntType4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPmntType4.SelectedIndex == 1)
            {
                long id = showChequeInfoDialog();
                txtChequeInfoId4.Text = id == 0 ? "" : id.ToString();
            }
        }

        private void grdItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colDiscount"].Value) > 0)
            {
                numTotalDiscount.Enabled = false;
                cmbTotalDiscountType.Enabled = false;
            }
        }

        private void grdItems_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            bool discountFound = false;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToDouble(r.Cells["colDiscount"].Value) > 0)
                {
                    discountFound = true;
                }
            }
            if (!discountFound)
            {
                numTotalDiscount.Enabled = true;
                cmbTotalDiscountType.Enabled = true;
            }
            Tuple<double, double, double> GrandTotals = GetGrandTotals();
            lblSubTotal.Text = GrandTotals.Item1.ToString();
            lblGrandTotal.Text = GrandTotals.Item2.ToString();
            numTotalDiscount.Value = (decimal)GrandTotals.Item3;
            GetCustomerTotals();
        }

        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Shift | Keys.I))
            {
                cmbSelectItems.Focus();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Payments_ValueChanged(object sender, EventArgs e)
        {
            GetCustomerTotals();
        }

        private void GetCustomerTotals()
        {
            double Receiveable = double.Parse(lblGrandTotal.Text);
            double payments = (double)(numPmnt1.Value + numPmnt2.Value + numPmnt3.Value + numPmnt4.Value);
            double advance = 0;
            double due = 0;
            if (Receiveable - payments >= 0)
            {
                due = Receiveable - payments;
                advance = 0;
            }
            else if (payments - Receiveable >= 0)
            {
                advance = payments - Receiveable;
                due = 0;
            }
            lblDue.Text = due.ToString();
            lblAdvance.Text = advance.ToString();
        }

        private void numTotalDiscount_ValueChanged(object sender, EventArgs e)
        {
            if (numTotalDiscount.Enabled)
            {
                GetTotalsByTotalDiscountChanges();
            }
        }

        private void cmbTotalDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (numTotalDiscount.Enabled)
            {
                GetTotalsByTotalDiscountChanges();
            }
        }

        private void GetTotalsByTotalDiscountChanges()
        {
            Tuple<double, double, double> GrandTotals = GetGrandTotals();
            double SubTotal = GrandTotals.Item1;
            double GrandTotal = GrandTotals.Item2;
            double discount = (double)numTotalDiscount.Value;
            if (cmbTotalDiscountType.SelectedIndex == 1)
            {
                discount = (discount / 100) * SubTotal;
            }
            lblSubTotal.Text = SubTotal.ToString();
            lblGrandTotal.Text = (SubTotal - discount).ToString();
            GetCustomerTotals();
        }

        private void cmbSelectBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSelectBatch.SelectedIndex >= 0)
            {
                int ItemId = Convert.ToInt32(cmbSelectItems.SelectedValue);
                int? batchId = Convert.ToInt32(cmbSelectBatch.SelectedValue);
                numRate.Value = (decimal)repItems.GetItemRate(ItemId, batchId > 0 ? batchId : null);
            }
        }

        private void txtSearchPatient_KeyDown(object sender, KeyEventArgs e)
        {
            ShowPatientData();
        }

        private void txtSearchPatient_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowPatientData();
        }

        private void ShowPatientData()
        {
            frmSearchPatient f = new frmSearchPatient();
            f.ShowDialog();
            if (f.PatientId > 0)
            {
                PatientId = f.PatientId;
                txtSearchPatient.Text = f.PatientName;
            }
        }

        private void ClearAll(bool isAfterSave)
        {
            if (grdItems.Rows.Count > 0)
            {
                DialogResult rs = new DialogResult();
                if (isAfterSave)
                {
                    rs = DialogResult.Yes;
                }
                else
                {
                    rs = MessageBox.Show("There are Some Unsaved Items in Grid, Are You Sure You Want To Clear", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                }
                if (rs == DialogResult.OK)
                {
                    grdItems.Rows.Clear();
                    Clear();
                    PatientId = 0;
                    objInvoice = new Invoice();
                    objInvoice.InvoiceItems = new List<InvoiceItem>();
                    dtStockConsumption.Rows.Clear();
                    dtStockConsumption.AcceptChanges();
                    txtSearchPatient.Text = "";
                    unitOfWork = new UnitOfWork();
                }
            }
        }

        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAddPatient(), this.MdiParent);
        }
    }
}