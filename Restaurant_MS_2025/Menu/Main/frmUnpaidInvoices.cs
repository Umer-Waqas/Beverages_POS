
namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmUnpaidInvoices : Form
    {
        enum FilterType { Default = 1, Date = 2, Patient = 3, User = 4, Invoice = 5 }
        FilterType filter = FilterType.Default;
        UnitOfWork unitOfWork;
        IPagedList<InvoiceTransactionsVM> Transactions;
        System.Timers.Timer PatientSearchTimer = new System.Timers.Timer();
        int PageNo = 1;
        bool isFilterapplied = false;
        int pageNoPatients = 1;
        bool IsNameSearching, IsPhoneSearching = false;
        long SelectedPatientId = 0;
        List<PatientSearchVM> PatientsGlobal = new List<PatientSearchVM>();
        DateTime ActionTime;

        public frmUnpaidInvoices()
        {
            InitializeComponent();
            PatientSearchTimer.Enabled = true;
            PatientSearchTimer.Interval = SharedVariables.AsyncDataLoadDelay;
            PatientSearchTimer.AutoReset = true;
            PatientSearchTimer.Elapsed += new System.Timers.ElapsedEventHandler(PatientSearchTimer_Elapsed);
            PatientSearchTimer.Stop();
        }

        private void frmInvoiceHistory_Load(object sender, EventArgs e)
        {
            try
            {
                this.ActionTime = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
                dtpTo.Value = dtpFrom.Value = this.ActionTime;

                SharedFunctions.SetGridStyle(grdInvoices);
                this.WindowState = FormWindowState.Maximized;
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddInvoice, btnRefreshForm, btnClose });
                LoadInvoices();
                LoadUsers();
                //LoadPatients();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
        }
        private void LoadUsers()
        {
            try
            {
                List<UserSelectVM> us = new List<UserSelectVM>();
                using (unitOfWork = new UnitOfWork())
                {
                    us = unitOfWork.UserRepository.GetAllActiveUsers();
                }
                UserSelectVM v = new UserSelectVM
                {
                    UserId = 0,
                    UserName = "Select All"
                };
                us.Insert(0, v);
                cmbUsers.SelectedIndexChanged -= cmbUsers_SelectedIndexChanged;
                cmbUsers.DataSource = us;
                cmbUsers.DisplayMember = "UserName";
                cmbUsers.ValueMember = "UserId";
                cmbUsers.SelectedIndexChanged += cmbUsers_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading users data. Please try again.", "User Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadInvoices()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    if (filter == FilterType.Date)
                    {
                        Transactions = unitOfWork.InvoiceRepository.GetTransactions(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                    }
                    //else if (filter == FilterType.Patient) // + date filter
                    //{
                    //    Transactions = unitOfWork.InvoiceRepository.GetTransactions(dtpFrom.Value, dtpTo.Value, this.SelectedPatientId, PageNo, SharedVariables.PageSize);// Convert.ToInt64(cmbPatient.SelectedValue));
                    //}
                    //else if (filter == FilterType.User) // + date filter
                    //{
                    //    Transactions = unitOfWork.InvoiceRepository.GetTransactions(dtpFrom.Value, dtpTo.Value, Convert.ToInt32(cmbUsers.SelectedValue), PageNo, SharedVariables.PageSize);
                    //}
                    //else if (filter == FilterType.Invoice) // date filter not required
                    //{
                    //    Transactions = unitOfWork.InvoiceRepository.GetTransactions(Convert.ToInt64(txtInvoiceNo.Text), PageNo, SharedVariables.PageSize);
                    //}
                    else // default
                    {
                        Transactions = unitOfWork.InvoiceRepository.GetTransactions(PageNo, SharedVariables.PageSize);
                    }
                }
                string PatientName = "";
                grdInvoices.Rows.Clear();
                foreach (InvoiceTransactionsVM i in Transactions.Items)
                {
                    PatientName = "";
                    if (i.Patient != null)
                    {
                        PatientName = i.Patient.Name;
                    }
                    grdInvoices.Rows.Add(i.InvoiceId, i.InvoiceRefNo, PatientName, (i.SaleQuantity - i.ReturnedQty), i.SubTotal, i.TotalDiscount, i.DiscountType == 1 ? "Value" : "%", i.GrandTotal, i.TotalPaid, i.Due > 0 ? i.Due : 0, i.Due < 0 ? -i.Due : 0, i.CreatedAt, i.IsProcedureInvoice, i.UserName);
                }
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, Transactions.PageCount);
                btnFirstPage.Enabled = btnPrevious.Enabled = Transactions.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = Transactions.HasNextPage;
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            ClearForm();
            LoadInvoices();
        }

        private void ClearForm()
        {
            this.ActionTime = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
            PageNo = 1;
            dtpFrom.Value = dtpTo.Value = this.ActionTime;
            txtInvoiceNo.Text = "";
            cmbPatient.SelectedIndex = -1;
            isFilterapplied = false;
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadInvoices();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = Transactions.PageCount;
            LoadInvoices();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadInvoices();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadInvoices();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            PageNo = 1;
            LoadInvoices();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            filter = FilterType.Date;
            PageNo = 1;
            LoadInvoices();
        }

        private void txtInvoiceNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtInvoiceNo.Text.Trim() != "")
                {
                    filter = FilterType.Invoice;
                }
                else
                {
                    filter = FilterType.Default;
                }
                cmbUsers.SelectedIndex = 0;
                txtSearchByName.Text = "";
                PageNo = 1;
                LoadInvoices();
            }
        }

        private void txtInvoiceNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void cmbPatient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt64(cmbPatient.SelectedValue) > 0)
            {
                txtInvoiceNo.Text = "";
                isFilterapplied = true;
            }
            PageNo = 1;
            LoadInvoices();
        }

        private void grdInvoices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            long InvoiceId = Convert.ToInt64(grdInvoices.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
            bool IsProcedureInvoice = Convert.ToBoolean(grdInvoices.Rows[e.RowIndex].Cells["colIsProcedureInvoice"].Value);
            if (e.ColumnIndex == grdInvoices.Columns["colEdit"].Index)
            {
                if (!SharedFunctions.IsActionallowed("Edit Invoice") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                {
                    MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                frmInvoice f = new frmInvoice(true, false, InvoiceId);
                f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
                f.Show();
                return;
            }

            if (e.ColumnIndex == grdInvoices.Columns["colRefund"].Index)
            {
                if (!SharedFunctions.IsActionallowed("Refund Payment") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                {
                    MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                frmInvoice f = new frmInvoice(false, true, InvoiceId);
                f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
                f.Show();
                return;

            }

            if (e.ColumnIndex == grdInvoices.Columns["colPrint"].Index)
            {
                //Reports.Pharmacy.PatientInvoiceViewer v = new Reports.Pharmacy.PatientInvoiceViewer(InvoiceId);
                //Reports.POSInvoices.POSInvoiceViewer v = new Reports.POSInvoices.POSInvoiceViewer(InvoiceId, true);
                ////v.Show();
                //v.Print();
                return;
            }

            if (e.ColumnIndex == grdInvoices.Columns["colPreview"].Index)
            {
                //Reports.Pharmacy.PatientInvoiceViewer v = new Reports.Pharmacy.PatientInvoiceViewer(InvoiceId);
                //Reports.POSInvoices.POSInvoiceViewer v = new Reports.POSInvoices.POSInvoiceViewer(InvoiceId, true);
                //v.Show();
                return;
            }
        }
        private void ChildForm_CLosed(object sender, FormClosedEventArgs e)
        {
            btnRefreshForm.PerformClick();
        }

        private void btnAddInvoice_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmInvoice(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_CLosed);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Search Patiend
        private void searchPatient()
        {
            string searchString = IsNameSearching ? txtSearchByName.Text.Trim().ToLower() : "";// txtSearchByPhone.Text.Trim().ToLower();
            if (searchString == "")
            {
                PatientSearchTimer.Stop();
                this.Invoke(new Action(() =>
                {
                    grdSuggestion.Visible = false;
                }));
                return;
            }

            IPagedList<PatientSearchVM> PatientsLocal = null;
            using (unitOfWork = new UnitOfWork())
            {
                PatientsLocal = unitOfWork.PatientRepository.SearchPatient(IsNameSearching, IsPhoneSearching, false, searchString, pageNoPatients);
                if (PatientsLocal.TotalCount <= 0) PatientSearchTimer.Stop();
                PatientsGlobal.AddRange(PatientsLocal.Items);
            }
            foreach (PatientSearchVM p in PatientsLocal.Items)
            {
                this.Invoke(new Action(() =>
                {
                    grdSuggestion.Rows.Add(p.PatientId, p.MrNo, p.PatientName, p.Phone);
                }));
            }
            if (PatientsGlobal != null && PatientsGlobal.Count > 0)
            {
                this.Invoke(new Action(() =>
                {
                    //grdSuggestion.Rows[0].Selected = false;
                    grdSuggestion.Visible = true;
                }));
            }
            else
            {
                this.Invoke(new Action(() =>
                {
                    grdSuggestion.Visible = false;
                }));
            }
        }
        private void LoadSearcherPatient()
        {
            PatientSearchTimer.Stop();
            if (grdSuggestion.SelectedRows.Count > 0)
            {
                //txtSearchByPhone.TextChanged -= txtSearchByPhone_TextChanged;
                txtSearchByName.TextChanged -= txtSearchByName_TextChanged;
                this.SelectedPatientId = Convert.ToInt64(grdSuggestion.SelectedRows[0].Cells["colPatientId"].Value);
                this.txtSearchByName.Text = grdSuggestion.SelectedRows[0].Cells["colGrdPatient"].Value.ToString();
                //this.txtSearchByPhone.Text = grdSuggestion.SelectedRows[0].Cells["colPhone"].Value.ToString();
                grdSuggestion.Visible = false;
                //txtSearchByPhone.TextChanged += txtSearchByPhone_TextChanged;
                txtSearchByName.TextChanged += txtSearchByName_TextChanged;
                if (this.SelectedPatientId > 0)
                {
                    txtInvoiceNo.Text = "";
                }
                PageNo = 1;
                filter = FilterType.Patient;
                LoadInvoices();
            }
        }
        private void PatientSearchTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            pageNoPatients += 1;
            string searchString = txtSearchByName.Text.ToLower();
            searchPatient();
        }
        private void txtSearchByName_Enter(object sender, EventArgs e)
        {
            grdSuggestion.Visible = false;
            grdSuggestion.BringToFront();
            grdSuggestion.Location = new Point(txtSearchByName.Location.X, txtSearchByName.Location.Y + txtSearchByName.Height + 4);
        }
        private void txtSearchByName_Leave(object sender, EventArgs e)
        {
            //grdSuggestion.Visible = false;
        }
        private void txtSearchByName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtSearchByName.Text.Trim().Equals(""))
                {
                    filter = FilterType.Default;
                    LoadInvoices();
                }
            }
            if (e.KeyCode == Keys.Down)
            {
                ShiftCtrlToSuggGrid();
            }
        }
        private void txtSearchByName_TextChanged(object sender, EventArgs e)
        {
            PatientSearchTimer.Stop();
            pageNoPatients = 1;
            PatientsGlobal = new List<PatientSearchVM>();
            grdSuggestion.Location = new Point(txtSearchByName.Location.X + 15, txtSearchByName.Location.Y + txtSearchByName.Height + 4);
            grdSuggestion.BringToFront();
            grdSuggestion.Rows.Clear();
            IsNameSearching = true;
            //IsPhoneSearching = false;
            searchPatient();
            PatientSearchTimer.Start();
        }
        private void grdSuggestion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            LoadSearcherPatient();
        }
        private void grdSuggestion_KeyPress(object sender, KeyPressEventArgs e)
        {
            // CHECKING FOR ALPHABET 
            TextBox TargetText = new TextBox();
            if (IsNameSearching) { TargetText = txtSearchByName; }
            //else if (IsPhoneSearching) { TargetText = txtSearchByPhone; }
            if ((e.KeyChar >= 65 && e.KeyChar <= 90)
                || (e.KeyChar >= 97 && e.KeyChar <= 122) || e.KeyChar >= 48 && e.KeyChar <= 57)
            {
                TargetText.Focus();
                TargetText.Text += (char)e.KeyChar;
                TargetText.SelectionStart = txtSearchByName.Text.Length;
            }

            if (e.KeyChar == (char)Keys.Back)
            {
                TargetText.Focus();
                TargetText.SelectionStart = txtSearchByName.Text.Length;
                //txtSearchByName.Text = txtSearchByName.Text.Substring(txtSearchByName.Text.Length, 1);
                //txtSearchByName.SelectionStart = 0;
                //txtSearchByName.SelectionLength = 1;
            }
        }
        private void grdSuggestion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadSearcherPatient();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                grdSuggestion.Visible = false;
            }
        }
        private void ShiftCtrlToSuggGrid()
        {
            if (grdSuggestion.Rows.Count > 0)
            {
                grdSuggestion.BringToFront();
                grdSuggestion.Focus();
                if (grdSuggestion.Rows.Count > 1)
                {
                    grdSuggestion.Rows[0].Selected = false;
                    grdSuggestion.Rows[1].Selected = true;
                }
                else
                {
                    grdSuggestion.Rows[0].Selected = true;
                }
            }
        }
        #endregion

        private void tbGotPageNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtGotoPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > Transactions.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadInvoices();
                }
            }
        }

        private void cmbUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtInvoiceNo.Text = "";
            txtSearchByName.Text = "";
            this.PageNo = 1;
            if (cmbUsers.SelectedIndex == 0)
            {
                filter = FilterType.Default;
            }
            else
            {
                filter = FilterType.User;
            }
            LoadInvoices();
        }
    }
}