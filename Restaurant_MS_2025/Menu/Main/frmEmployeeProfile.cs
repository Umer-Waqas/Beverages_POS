

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmEmployeeProfile : Form
    {
        UnitOfWork unitOfWork;
        int PageNo = 1;
        int PageSize = SharedVariables.PageSize;
        private long EmployeeId = 0;
        public frmEmployeeProfile()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }
        public frmEmployeeProfile(long EmployeeId)
        {
            InitializeComponent();
            this.EmployeeId = EmployeeId;
            unitOfWork = new UnitOfWork();
        }
        private void frmPatientProfile_Load(object sender, EventArgs e)
        {
            //GetPatientData();
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnPrintInvoiceHistory, btnAddInvoice });
            SharedFunctions.SetSmallButtonsStyle(new[] { btnEditProfile });
            EmployeeInvoicesData();
            SharedFunctions.SetGridStyle(grdInvoices);
        }
        private void EmployeeInvoicesData()
        {
            EmployeeVM patient = unitOfWork.InvoiceRepository.GetEmployeeInvoices(this.EmployeeId, PageNo, PageSize);
            lblPatientName.Text = patient.Name;
            lblMrNo.Text = patient.MRNo.ToString();
            lblHwId.Text = "N/A";
            lblPhone.Text = patient.Phone;
            if (patient.ImagePath != null)
            {
                pbPatientImage.Image = Image.FromFile(patient.ImagePath);
            }

            int count = 0;
            foreach (InvoiceVM i in patient.Invoices)
            {
                string InvoiceItems = "";
                string InvoicePayments = "";
                string PaymentDates = "";
                foreach (InvoiceItemVM ii in i.InvoiceItems)
                {
                    InvoiceItems += ii.ItemName + ", ";
                }
                foreach (InvoicePaymentVM ip in i.InvoicePayments)
                {
                    InvoicePayments += ip.Payment + Environment.NewLine;
                    PaymentDates += ip.PaymentDate.ToShortDateString() + Environment.NewLine;
                }
                InvoiceItems = InvoiceItems.Trim().TrimEnd(',');
                grdInvoices.Rows.Add(++count, i.InvoiceId, i.InvoiceRefNo, InvoiceItems, i.SubTotal, InvoicePayments, i.Due > 0 ? i.Due : 0, i.Due < 0 ? -1 * i.Due : 0, PaymentDates, null, null);
            }
        }
        private void GetPatientData()
        {
            //get patient info
            Patient objPatient = unitOfWork.PatientRepository.GetPatientWithTags_ByPatientId(this.EmployeeId);
            lblPatientName.Text = objPatient.Name;
            lblHwId.Text = "N/A";
            lblPhone.Text = objPatient.Phone;
            if (objPatient.ImagePath != null)
            {
                pbPatientImage.Image = Image.FromFile(objPatient.ImagePath);
            }

            //get Last invoice details
            //InvoiceVM objInvoice = unitofWork.InvoiceRepository.GetPatient_LastInvoiceDetail(this.PatientId);
            //if (objInvoice != null)
            //{
            //    foreach (InvoiceItemVM i in objInvoice.InvoiceItems)
            //    {
            //        grdInvoiceItems.Rows.Add(i.ItemName, i.Amount);
            //    }
            //    lblTotalBilledAmount.Text = objInvoice.SubTotal.ToString();
            //    lblAmountPaid.Text = objInvoice.GrandTotal.ToString();
            //}            
            //get treatment plan
        }


        private void btnEditProfile_Click(object sender, EventArgs e)
        {
            if (this.EmployeeId > 0)
            {
                Form f = SharedFunctions.OpenForm(new frmAddEmployee(this.EmployeeId), this.MdiParent);
                f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
            }
            else
            {
                MessageBox.Show("No Employee Selected To Edit");
            }
        }

        private void btnAddInvoice_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmInvoice(this.EmployeeId), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void lblViewAll_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmPatientInvoices(this.EmployeeId), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ClearForm();
            unitOfWork = new UnitOfWork();
            grdInvoices.Rows.Clear();
            EmployeeInvoicesData();
        }

        private void ClearForm()
        {
            grdInvoiceItems.Rows.Clear();
        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }

        private void grdInvoices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                {
                    return;
                }
                long InvoiceId = Convert.ToInt64(grdInvoices.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
                if (e.ColumnIndex == grdInvoices.Columns["colEdit"].Index)
                {
                    frmInvoice i = new frmInvoice(true, false, Convert.ToInt64(InvoiceId));
                    i.Show();
                    return;
                }

                if (e.ColumnIndex == grdInvoices.Columns["colRefund"].Index)
                {
                    frmInvoice i = new frmInvoice(false, true, InvoiceId);
                    i.Show();
                    return;
                }
                if (e.ColumnIndex == grdInvoices.Columns["colDelete"].Index)
                {
                    DialogResult rs = MessageBox.Show("Are you Sure You Want To Delete Invoice", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    if (rs == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }
                    Invoice objInvoice = unitOfWork.InvoiceRepository.GetInvoiceById_Inc_Pmnts_cons(InvoiceId);
                    objInvoice.UpdatedAt = DateTime.Now;
                    objInvoice.IsActive = false;
                    objInvoice.IsUpdate = true;
                    //foreach (InvoiceItem i in objInvoice.InvoiceItems)
                    //{
                    //    i.IsActive = false;
                    //    i.IsUpdate = true;
                    //    unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                    //}
                    // commecnted while consume stock change
                    //foreach (StockConsumptionItem c in objInvoice.StockConsumption.StockConsumptionItems)
                    //{
                    //    c.IsUpdate = true;
                    //    c.IsActive = false;
                    //    c.UpdatedAt = DateTime.Now;
                    //    //unitOfWork.GetDbContext().Entry(c).State = System.Data.Entity.EntityState.Modified;
                    //}
                    foreach (InvoicePayment p in objInvoice.InvoicePayments)
                    {
                        p.IsUpdate = true;
                        p.IsActive = false;
                        p.UpdatedAt = DateTime.Now;
                        //unitOfWork.GetDbContext().Entry(p).State = System.Data.Entity.EntityState.Modified;
                    }
                    unitOfWork.Save();
                    MessageBox.Show("Patient Invoice Deleted Succesfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnRefresh.PerformClick();
                    return;
                }

                if (e.ColumnIndex == grdInvoices.Columns["colPrintInvoice"].Index)
                {
                    //Reports.Pharmacy.PatientInvoiceViewer v = new Reports.Pharmacy.PatientInvoiceViewer(InvoiceId);
                    //v.Show();
                    return;
                }
                if (e.ColumnIndex == grdInvoices.Columns["colPrintInvoiceRefund"].Index)
                {
                    //Reports.PatientInvoiceRefundViewer v = new Reports.PatientInvoiceRefundViewer(InvoiceId);
                    //v.Show();
                    return;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnPrintInvoiceHistory_Click(object sender, EventArgs e)
        {

        }
    }
}