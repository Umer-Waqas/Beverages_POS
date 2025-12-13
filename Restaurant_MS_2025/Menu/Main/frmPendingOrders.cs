
namespace Restaurant_MS_UI.Menu.Main
{

    public partial class frmPendingOrders : Form
    {
        private long SelectedInvoiceId = 0;
        System.Windows.Forms.Timer orderTimer = new System.Windows.Forms.Timer();
        public frmPendingOrders()
        {
            InitializeComponent();
            orderTimer.Interval = 1000 * 5;
            orderTimer.Enabled = true;
            orderTimer.Tick += new EventHandler(OrderTimer_Ticked);
        }

        private void frmPendingOrders_Load(object sender, EventArgs e)
        {
            rbPending.Checked = true;
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnClose, btnInvoice });
            if (SharedVariables.AdminPharmacySetting.OrderWaningAlertTime != null)
            {
                txtWarningAlert.Text = SharedVariables.AdminPharmacySetting.OrderWaningAlertTime.ToString();
            }
            else
            {
                txtWarningAlert.Text = "0";
            }

            orderTimer.Start();
        }

        public void OrderTimer_Ticked(object ojb, EventArgs e)
        {
            LoadPendingOrders(Enums.PendingOrdersViewType.Pending);
        }

        private void LoadPendingOrders(Enums.PendingOrdersViewType OrderViewType)
        {
            int warningTime = int.Parse(txtWarningAlert.Text);
            grdItems.Rows.Clear();
            List<PendingOrdersVM> pendingOrders = new List<PendingOrdersVM>();
            using (UnitOfWork uw = new UnitOfWork())
            {
                pendingOrders = uw.InvoiceRepository.GetPendingOrders(OrderViewType);
            }
            string paymentStatus = "";
            string orderType = "";
            foreach (var o in pendingOrders)
            {
                if (o.OrderType == 1)
                {
                    orderType = "Dine In";
                }
                else if (o.OrderType == 2)
                {
                    orderType = "Take Away";
                }
                else if (o.OrderType == 3)
                {
                    orderType = "Home Delivery";
                }


                if (o.PaymentStatus == 1)
                {
                    paymentStatus = "Paid";
                }
                else if (o.PaymentStatus == 2)
                {
                    paymentStatus = "Un-Paid";
                }
                else if (o.PaymentStatus == 3)
                {
                    paymentStatus = "Payment On Delivery";
                }

                var timeSpent = DateTime.Now.Subtract(o.OrderStartTime);
                var rowIndex = grdItems.Rows.Add(o.InvoiceId, o.CustomerName, o.CustomerPhone, o.CustomerAddress, o.TotalAmount, o.DiscountAmount, o.GrandTotal, paymentStatus, orderType, o.OrderStatusString, o.OrderStartTime.ToString("HH:mm:ss"), Math.Round(timeSpent.TotalMinutes, 2));
                var r = grdItems.Rows[rowIndex];
                if (warningTime > 0)
                {
                    if (timeSpent.TotalMinutes >= warningTime + 5)
                    {
                        r.DefaultCellStyle.BackColor = Color.Red;
                        r.DefaultCellStyle.ForeColor = Color.White;
                        r.DefaultCellStyle.SelectionBackColor = Color.Red;
                        r.DefaultCellStyle.SelectionForeColor = Color.White;
                    }
                    else if (timeSpent.TotalMinutes >= warningTime)
                    {
                        r.DefaultCellStyle.BackColor = Color.Yellow;
                        r.DefaultCellStyle.ForeColor = Color.Red;
                        r.DefaultCellStyle.SelectionBackColor = Color.Yellow;
                        r.DefaultCellStyle.SelectionForeColor = Color.Red;
                    }
                    else
                    {
                        r.DefaultCellStyle.BackColor = Color.White;
                        r.DefaultCellStyle.ForeColor = Color.Black;
                        r.DefaultCellStyle.SelectionBackColor = Color.LightBlue;
                        r.DefaultCellStyle.SelectionForeColor = Color.White;
                    }
                }
            }
        }

        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex < 0)
            {
                return;
            }
            if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
            {
                //if (!SharedFunctions.IsActionallowed("Edit Invoice") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                //{
                //    MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    return;
                //}

                long invoiceId = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
                frmInvoice f = new frmInvoice(true, false, invoiceId);
                f.Show();
                return;
            }
            if (e.ColumnIndex == grdItems.Columns["colEdit"].Index)
            {
                //if (!SharedFunctions.IsActionallowed("Edit Invoice") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                //{
                //    MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //    return;
                //}

                long invoiceId = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
                frmInvoice f = new frmInvoice(true, false, invoiceId);
                f.Show();
                return;
            }

            if (e.ColumnIndex == grdItems.Columns["colPaid"].Index)
            {
                long invoiceId = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
                this.SelectedInvoiceId = invoiceId;
                PaymentDone(this.SelectedInvoiceId);
            }

            if (e.ColumnIndex == grdItems.Columns["colCompleted"].Index)
            {
                long invoiceId = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
                this.SelectedInvoiceId = invoiceId;
                OrderCompleted(this.SelectedInvoiceId);
            }
        }

        private void PaymentDone(long SelectedInvoiceId)
        {
            //if (!SharedFunctions.IsActionallowed("Edit Invoice") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            //{
            //    MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}


            if (SelectedInvoiceId <= 0)
            {
                MessageBox.Show("Ivalid invoice number, please try after refresing this page.", "Invalid Invoice Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool paidSuccess = false;
            using (UnitOfWork uw = new UnitOfWork())
            {
                paidSuccess = uw.InvoiceRepository.SetInvoiceStatusToPaid(SelectedInvoiceId);
            }
            if (paidSuccess)
            {
                MessageBox.Show("Payment done successfully,", "Payment Failed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //long _invoiceId = 0;
                //foreach (DataGridViewRow r in grdItems.Rows)
                //{
                //    Int64.TryParse(r.Cells["colInvoiceId"].Value.ToString(), out _invoiceId);
                //    if (SelectedInvoiceId == _invoiceId)
                //    {
                //        grdItems.Rows.RemoveAt(r.Index);
                //        break;
                //    }
                //}
            }
            else
            {
                MessageBox.Show("An error occurred while adding invoice payment, please try with edit invoice.", "Payment Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void OrderCompleted(long SelectedInvoiceId)
        {
            //if (!SharedFunctions.IsActionallowed("Edit Invoice") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            //{
            //    MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}


            if (SelectedInvoiceId <= 0)
            {
                MessageBox.Show("Ivalid invoice number, please try after refresing this page.", "Invalid Invoice Number", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool completedSuccess = false;
            using (UnitOfWork uw = new UnitOfWork())
            {
                completedSuccess = uw.InvoiceRepository.SetInvoiceORderStatusToCompleted(SelectedInvoiceId);
            }
            if (completedSuccess)
            {
                MessageBox.Show("Order Completed Successfully,", "Order Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                long _invoiceId = 0;
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    Int64.TryParse(r.Cells["colInvoiceId"].Value.ToString(), out _invoiceId);
                    if (SelectedInvoiceId == _invoiceId)
                    {
                        grdItems.Rows.RemoveAt(r.Index);
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("An error occurred while completing the order, please try with edit invoice.", "Order Completion Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadPendingOrders(Enums.PendingOrdersViewType.Pending);

        }

        private void frmPendingOrders_Enter(object sender, EventArgs e)
        {
            LoadPendingOrders(Enums.PendingOrdersViewType.Pending);
        }

        private void textBoxX2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnCloseCustomerDetails_Click(object sender, EventArgs e)
        {
            HideDetailsPanel();
        }

        private void HideDetailsPanel()
        {
            this.SelectedInvoiceId = 0;
            pnlCustomerDetails.SendToBack();
            pnlCustomerDetails.Visible = false;
        }
        private void grdItems_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.SelectedInvoiceId = Convert.ToInt64(grdItems.Rows[e.RowIndex].Cells["colInvoiceId"].Value);

            txtCustomerName.Text = "";
            txtCustomerPhone.Text = "";
            txtCustomerAddress.Text = "";
            lblGrandTotal.Text = ".....";
            lblPaymentStatus.Text = ".....";
            lblOrderType.Text = "......";
            lblStartTime.Text = ".....";
            lblTimeSpent.Text = ".....";

            pnlCustomerDetails.BringToFront();
            pnlCustomerDetails.Visible = true;

            var row = grdItems.Rows[e.RowIndex];
            var name = row.Cells["colCustomerName"].Value.ToString();
            var phone = row.Cells["colCustomerPhone"].Value.ToString();
            var address = row.Cells["colCustomerAddress"].Value.ToString();
            var grandTotal = row.Cells["colGrandTotal"].Value.ToString();
            var paymentStatus = row.Cells["colPaymentStatus"].Value.ToString();
            var orderType = row.Cells["colOrderType"].Value.ToString();
            var orderStartTime = row.Cells["colOrderStartTime"].Value.ToString();
            var timeSpent = row.Cells["colTimeSpent"].Value.ToString();
            txtCustomerName.Text = name;
            txtCustomerPhone.Text = phone;
            txtCustomerAddress.Text = address;
            lblGrandTotal.Text = grandTotal;
            lblPaymentStatus.Text = paymentStatus;
            lblOrderType.Text = orderType;
            lblStartTime.Text = orderStartTime;
            lblTimeSpent.Text = timeSpent;
        }

        private void pnlCustomerDetails_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pnlCustomerDetails_DoubleClick(object sender, EventArgs e)
        {
            HideDetailsPanel();
        }

        private void btnPaymentDone_Click(object sender, EventArgs e)
        {
            PaymentDone(this.SelectedInvoiceId);
            HideDetailsPanel();
        }


        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtWarningAlert_Leave(object sender, EventArgs e)
        {
            try
            {
                using (UnitOfWork uw = new UnitOfWork())
                {
                    var setting = uw.AdminPharmacySettingRepository.Query().FirstOrDefault();
                    int warningTime = 0;
                    int.TryParse(txtWarningAlert.Text, out warningTime);

                    setting.OrderWaningAlertTime = warningTime;
                    uw.AdminPharmacySettingRepository.Update(setting);
                    uw.Save();
                    SharedVariables.AdminPharmacySetting.OrderWaningAlertTime = warningTime;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updaintg warning alert time in database, please try again.", "Failed to Save", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void frmPendingOrders_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.orderTimer.Stop();
        }

        private void frmPendingOrders_Activated(object sender, EventArgs e)
        {
            this.rbPending.Checked = true;

        }
        private void rbAll_CheckedChanged(object sender, EventArgs e)
        {
            orderTimer.Stop();
            LoadPendingOrders(Enums.PendingOrdersViewType.All);
        }

        private void rbPending_CheckedChanged(object sender, EventArgs e)
        {
            orderTimer.Start();
            LoadPendingOrders(Enums.PendingOrdersViewType.Pending);
        }

        private void rbCompleted_CheckedChanged(object sender, EventArgs e)
        {
            orderTimer.Stop();
            LoadPendingOrders(Enums.PendingOrdersViewType.Completed);
        }

        private void btnInvoice_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmInvoice(), this.MdiParent);
        }
    }
}