
using Newtonsoft.Json;

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmInvoice : Form
    {
        bool isNewItemAdded = false;
        bool isInvoiceUpdated = false;
        IPagedList<InvoiceVM> holdingInvoices;
        public int HoldingInvoiceId = 0;
        private int InvHoldPageNo = 0;
        private UnitOfWork unitOfWork;
        List<BatchStockVM> BatchStockList;
        List<InvoicePayment> DeletedPayments = new List<InvoicePayment>();
        List<InvoiceItemVM> DeletedInvoiceItems;
        List<Item> POSItems = new List<Item>();
        List<ItemDetailVM> AddedItemsData = new List<ItemDetailVM>();
        List<PatientSearchVM> PatientsGlobal = new List<PatientSearchVM>();
        List<Item> ItemsGlobal = new List<Item>();
        DataTable dtStockConsumption = new DataTable();
        long PatientId = 0;
        long InvoiceEditId = 0;
        bool IsInvoiceEdit = false;
        bool IsInvoiceRefund = false;
        bool IsDiscountByItem = false;
        int EditingRowIndex = -1;
        bool IsNameSearching, IsPhoneSearching = false;
        bool IsReturnedQuantityFound = false;
        private double InitialTotalRefundValue = 0;
        private double LoadedInvoice_TotalPaid = 0;
        private double TotalPaid = 0;
        BackgroundWorker bgWItemsLoader = new BackgroundWorker();
        System.Timers.Timer PatientSearchTimer = new System.Timers.Timer();
        int pageNoPatients = 1;
        private int SelectedItemId = 0;
        public string SelectedItemName = "";
        bool isBalanceActive = true;
        bool isLoadingForm = false; // this will be used in different places to negate an operation while invoice data/Form data is being loaded.
        double scannedItemQuantity = 0;
        FBR_Response fbrResponse = new FBR_Response();
        DateTime ActionTime;

        public frmInvoice()
        {
            InitializeComponent();
            DeletedInvoiceItems = new List<InvoiceItemVM>();
            PatientSearchTimer.Enabled = true;
            PatientSearchTimer.Interval = SharedVariables.AsyncDataLoadDelay;
            PatientSearchTimer.AutoReset = true;
            PatientSearchTimer.Elapsed += new System.Timers.ElapsedEventHandler(PatientSearchTimer_Elapsed);
            PatientSearchTimer.Stop();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            //this.uC_SearchItems1.OnEnterKeyDown += uC_SearchItems1_OnEnterKeyDown;
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }

        public frmInvoice(long PatientId)
        {
            InitializeComponent();
            this.PatientId = PatientId;
            DeletedInvoiceItems = new List<InvoiceItemVM>();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }

        public frmInvoice(bool IsInvoiceEdit, bool IsInvoiceRefund, long InvoiceEditId)
        {
            InitializeComponent();
            this.InvoiceEditId = InvoiceEditId;
            this.IsInvoiceEdit = IsInvoiceEdit;
            this.IsInvoiceRefund = IsInvoiceRefund;
            DeletedInvoiceItems = new List<InvoiceItemVM>();
            this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }

        private void uC_SearchItems1_OnItemSelectionChanged()
        {
            this.SelectedItemId = this.uC_SearchItems1.SelectedItemId;
            this.SelectedItemName = this.uC_SearchItems1.SelectedItemName;
            if (this.SelectedItemId <= 0)
            {
                btnClear.PerformClick();
                return;
            }
            btnAddItem.PerformClick();
            //LoadUnitsData();
            //GetItemBatches(this.SelectedItemId);
            //if (cmbSelectBatch.Items.Count > 1)
            //{
            //    cmbSelectBatch.Focus();
            //}
            //else
            //{
            //    btnAddItem.PerformClick();
            //}
            //MessageBox.Show(this.SelectedItemId + " | " + this.SelectedItemName);
        }

        private void LoadUnitsData()
        {
            using (unitOfWork = new UnitOfWork())
            {
                //numUnitCost.Value = (decimal)unitOfWork.ItemRspository.GetUnitCostByItemId(ItemId);
                Item objItem = unitOfWork.ItemRspository.GetById(this.SelectedItemId); ;
                string unit = objItem.Unit;
                cmbUnit.Items.Add(unit);
                cmbUnit.Items.Add("Units");
                cmbUnit.SelectedIndex = 0;
                numConvUnit.Value = objItem.ConversionUnit;
            }
        }
        private void LoadItemData()
        {
            using (unitOfWork = new UnitOfWork())
            {
                //numUnitCost.Value = (decimal)unitOfWork.ItemRspository.GetUnitCostByItemId(ItemId);
                Item objItem = unitOfWork.ItemRspository.GetById(this.SelectedItemId);
                string unit = objItem.Unit;
                cmbUnit.Items.Add(unit);
                cmbUnit.Items.Add("Units");
                cmbUnit.SelectedIndex = 0;
                numConvUnit.Value = objItem.ConversionUnit;
            }
        }
        private void bgWItemsLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            //this.Invoke((MethodInvoker)delegate()
            //{
            //});

            //if (SharedVariables.POSItems == null)
            //{
            //using (unitOfWork = new UnitOfWork())
            //{
            //POSItems = unitOfWork.ItemRspository.GetActiveItems().ToList();
            //POSItems = unitOfWork.ItemRspository.GetActiveItems(20).ToList();
            //foreach(Item i in POSItems)
            //{
            //    grdSItems.Rows.Add(i.ItemId, i.item)
            //}
            //}
            //}
        }
        private void bgWItemsLoader_RunWokrerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //grdSItems.DataSource = POSItems;
            //cmbSelectItems.SelectedIndexChanged -= cmbSelectItems_SelectedIndexChanged;
            //cmbSelectItems.DataSource = POSItems;
            //cmbSelectItems.ValueMember = "ItemId";
            //cmbSelectItems.DisplayMember = "ItemName";
            //cmbSelectItems.SelectedIndexChanged += cmbSelectItems_SelectedIndexChanged;
            //cmbSelectItems.SelectedIndex = -1;
            //cmbSelectItems.Enabled = true;
            //lblLoadingItems.Visible = false;
        }
        private void setNumericUD_toEmpty()
        {
            numRate.ResetText();
            numDiscount.ResetText();
        }
        private void ShowHoldingInvoices(int p_pageNo)
        {
            try
            {
                int totalHold = 0;
                using (unitOfWork = new UnitOfWork())
                {
                    if (InvHoldPageNo == 0)
                    {
                        totalHold = unitOfWork.InvoiceRepository.GetHoldingInvoicesCount();
                        grpInvoiceHolding.Text = "Holding Invoices[" + InvHoldPageNo + " / " + totalHold + "]";
                    }
                    else
                    {
                        holdingInvoices = unitOfWork.InvoiceRepository.GetHoldingInvoices(p_pageNo, 1);
                        //this.HoldingInvoiceId = (int)holdingInvoices.FirstOrDefault().InvoiceId;
                        grpInvoiceHolding.Text = "Holding Invoices[" + InvHoldPageNo + " / " + holdingInvoices.PageCount + "]";
                        //LoadHoldingInvoice(holdingInvoices.FirstOrDefault());
                    }
                }

                if (InvHoldPageNo == 0)
                {
                    if (totalHold > 0)
                    {
                        btnNext.Enabled = btnLastPage.Enabled = true;
                        btnPrevious.Enabled = btnFirstPage.Enabled = false;
                    }
                    else
                    {
                        btnNext.Enabled = btnLastPage.Enabled = false;
                        btnPrevious.Enabled = btnFirstPage.Enabled = false;
                    }
                }
                else
                {
                    btnNext.Enabled = btnLastPage.Enabled = holdingInvoices.HasNextPage;
                    if (InvHoldPageNo >= 1 || holdingInvoices.HasPreviousPage)
                    {
                        btnFirstPage.Enabled = btnPrevious.Enabled = true;
                    }
                    else
                    {
                        btnFirstPage.Enabled = btnPrevious.Enabled = false;
                    }
                }
                CalculateTotals();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading next invoice, please try after reloading invoice screen.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void frmConsumeStock_Load(object sender, EventArgs e)
        {
            cmbPaymentStatus.SelectedIndex = 0;
            //chkBarcodePref.Checked = Properties.Settings.Default.UseBarcodeSearch;
            //chkSearchItemPref.Checked = !Properties.Settings.Default.UseBarcodeSearch;
            LoadForm(false);
            if (!SharedVariables.LoggedInUser.UserRoles.Any(r => r.UserRoleId == 1))
            {
                grdItems.Columns["colDiscount"].Visible = false;
                grdItems.Columns["colDiscountType"].Visible = false;
                //numTotalDiscount.Enabled = false;
                txtDiscount.Enabled = false;
                cmbTotalDiscountType.Enabled = false;
            }
            if (SharedVariables.LoggedInUser.UserRoles.Any(r => r.UserRoleId == 2) && SharedVariables.LoggedInUser.CanGiveDiscount)
            {
                grdItems.Columns["colDiscount"].Visible = true;
                grdItems.Columns["colDiscountType"].Visible = true;
                //numTotalDiscount.Enabled = true;
                txtDiscount.Enabled = true;
                cmbTotalDiscountType.Enabled = true;
            }
            if (!SharedVariables.AdminPharmacySetting.ShowRackNoInPOS)
            {
                grdItems.Columns["colRackNo"].Visible = false;
            }
            if (SharedVariables.AdminPharmacySetting.IsPharmacyTemps)
            {
                cmbTemplates.Visible = true;
                lblTemplate.Visible = true;
            }
            if (SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo)
            {
                grdItems.Columns["colBatch"].Visible = false;
            }
            resetSearchFocus();
        }
        private void LoadForm(bool isLocallyEditing)
        {
            isLoadingForm = true; // purpose of unregister/register events and isLoading form is same. but we can't unregister grdItems_RowsAdded event, so it will be used there for same purpose
            loadTables();
            LoadEmployees();
            UnRegisterAllEvents();
            if (SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo)
            {
                grdItems.Columns["colBatch"].Visible = false;
            }

            grdItems.Columns["colBonusQuantity"].Visible = SharedVariables.AdminInvoiceSetting.ShowBonusQty;
            grdItems.Columns["colSalesTax"].Visible = SharedVariables.AdminInvoiceSetting.ShowSalesTax;
            grdItems.Columns["colSalesTaxType"].Visible = SharedVariables.AdminInvoiceSetting.ShowSalesTax;
            grdItems.Columns["colCostPrice"].Visible = SharedVariables.AdminInvoiceSetting.ShowSalesTax;

            cmbAssignedTo.SelectedIndex = 0;
            cmbOrderType.SelectedIndex = 1;
            cmbPaymentStatus.SelectedIndex = 0;
            cmbOrderStatus.SelectedIndex = 0;

            if (!isLocallyEditing)
            {
                ShowHoldingInvoices(1);
                SharedFunctions.SetGridStyle(grdItems, true);
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnSave, btnSaveAndPrint, btnClearAll, btnCancelHold, btnHold, btn1stInvoice, btnNextInvoice, btnPreviousInvoice, btnLastInvoice });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnAddItem, btnAddtoAdv, btnAddToBal, btnClear, btnAddItem, btnSearchItems });
                this.WindowState = FormWindowState.Maximized;
            }

            if (!SharedFunctions.IsActionallowed("Edit Invoice Retail Price") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                //numRate.Enabled = false;
                //MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //return;
            }
            cmbPmntType1.SelectedIndex = 0;
            cmbPmntType2.SelectedIndex = 0;
            cmbPmntType3.SelectedIndex = 0;
            cmbPmntType4.SelectedIndex = 0;
            cmbPmntType5.SelectedIndex = 0;
            cmbTotalDiscountType.SelectedIndex = 0;
            try
            {
                Tuple<double, double, double> invoiceTotals = null;
                if ((IsInvoiceEdit || IsInvoiceRefund) && this.InvoiceEditId > 0)
                {
                    grpInvoiceHolding.Enabled = false;
                    isBalanceActive = false;
                    if (IsInvoiceRefund)
                    {
                        pnlDues.Visible = false;
                        grdItems.Columns["colEdit"].Visible = false;
                        grdItems.Columns["colRemove"].Visible = false;
                        grdItems.Columns["colReturnToggle"].Visible = true;
                        grdItems.Columns["colAffectStock"].Visible = true;

                        this.Text = "Invoice Refund";
                        hideItemsDetails();
                        pnlContainer.Location = new Point(pnlContainer.Location.X, pnlContainer.Location.Y - 55);
                        grdItems.Columns["colReturnQuantity"].Visible = true;
                        pnlRefund.Visible = true;
                        //numTotalDiscount.Enabled = false;
                        txtDiscount.Enabled = false;
                        cmbTotalDiscountType.Enabled = false;
                    }
                    invoiceTotals = LoadInvoice(this.InvoiceEditId);
                    if (IsReturnedQuantityFound)
                    {
                        grdItems.Columns["colEdit"].Visible = false;
                        grdItems.Columns["colRemove"].Visible = false;
                        DisableItemsDetails();
                        pnlDue.Visible = false;
                        pnlDues.Visible = false;
                    }
                    if (IsInvoiceEdit)
                    {
                        pnlInvoiceEditControls.Enabled = true;
                        this.Text = "Invoice Edit";
                    }
                    CalculateTotals(); // in case of edit/return grdItems_CellValuChanges function is not called,
                }
                if (invoiceTotals != null)// && !IsDiscountByItem)
                {
                    //numTotalDiscount.Value = (decimal)Math.Round(invoiceTotals.Item1, 2, MidpointRounding.AwayFromZero);
                    txtDiscount.Text = Math.Round(invoiceTotals.Item1, 2, MidpointRounding.AwayFromZero).ToString();
                    txtDiscount_TextChanged(null, null);
                    //numTotalDiscount_ValueChanged(null, null);
                }
                // don't move below two lines up, otherwise auto-calculation on invoice-edit/refund will be disturbed.
                cmbDiscountType.SelectedIndex = 1; // for add item panel
                // don't below line anywhere else, this will updted numModifieddiscount values after automatic calculatoin
                if (invoiceTotals != null)
                {
                    numModifiedDiscount.Value = (decimal)invoiceTotals.Item3;
                }

                setNumericUD_toEmpty();
                txtAmount.Text = "";
                txtNetAmount.Text = "";
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
            //txtSearchByPhone.TextChanged += txtSearchByPhone_TextChanged;
            //txtSearchByName.TextChanged += txtSearchByName_TextChanged;
            //cmbSelectItems.Focus();
            if (!isLocallyEditing)
            {
                SharedFunctions.SetGridStyle(grdItems, true);
                loadTemplates();
            }
            RegisterAllEvents();
            isLoadingForm = false;
        }
        private void loadTemplates()
        {
            try
            {
                cmbTemplates.SelectedIndexChanged -= cmbTemplates_SelectedIndexChanged;
                List<Template> templates = new List<Template>();
                using (unitOfWork = new UnitOfWork())
                {
                    templates = unitOfWork.TemplateRepository.GetAllActiveTemplates();
                }
                Template temp = new Template();
                temp.TemplateId = 0;
                temp.Name = "Select Template";
                templates.Insert(0, temp);
                cmbTemplates.DataSource = templates;
                cmbTemplates.ValueMember = "TemplateId";
                cmbTemplates.DisplayMember = "Name";
                cmbTemplates.SelectedIndexChanged -= cmbTemplates_SelectedIndexChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading templates, please try again.", "Templates missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void loadTables()
        {
            using (UnitOfWork uw = new UnitOfWork())
            {
                /// var tables = uw.SeatingTableRspository.Query().Where(t => t.IsActive).Select(t => new SelectListVM { Value = t.SeatingTableId, Text = t.TableName.ToString() }).ToList();
                //  SharedFunctions.SetComboDataSource(tables, cmbTableNo, "Select Seating Table");
            }
        }

        private void LoadEmployees()
        {
            using (UnitOfWork uw = new UnitOfWork())
            {
                var employees = uw.EmployeeRepository.GetSelectList();
                SharedFunctions.SetComboDataSource(employees, cmbAssignedTo, "Select Assignee");
            }
        }

        private void DisableItemsDetails()
        {
            lblSearchItem.Enabled = false;
            uC_SearchItems1.Enabled = false;
            //txtBarcode.Enabled = false;
            lblTemplate.Enabled = false;
            cmbTemplates.Enabled = false;
            btnSearchItems.Enabled = false;
        }

        private void hideItemsDetails()
        {
            lblSearchItem.Visible = false;
            uC_SearchItems1.Visible = false;
            lblTemplate.Visible = false;
            cmbTemplates.Visible = false;
            btnSearchItems.Visible = false;
            chkRepeatedScans.Visible = false;
            //txtBarcode.Visible = false;
            chkBarcodePref.Visible = false;
            chkSearchItemPref.Visible = false;

        }
        private void LoadPateint()
        {
            Patient objPatient = new Patient();
            using (unitOfWork = new UnitOfWork())
            {
                objPatient = unitOfWork.PatientRepository.GetById(this.PatientId);
            }
            if (objPatient != null)
            {
                //pnlPatientDetails.Enabled = false;
            }
            txtSearchByName.Text = objPatient.Name;
            txtSearchByPhone.Text = objPatient.Phone;
        }
        //below method "LoadHoldingInvoice" is duplicate of this "LoadInvoice" method. for any changes in any method make consideratino on both.
        private void UnRegisterAllEvents()
        {
            txtSearchByName.TextChanged -= txtSearchByName_TextChanged;
            txtSearchByPhone.TextChanged -= txtSearchByPhone_TextChanged;
            txtAddress.TextChanged -= txtAddress_TextChanged;
            //numTotalDiscount.ValueChanged -= numTotalDiscount_ValueChanged;
            txtDiscount.TextChanged -= txtDiscount_TextChanged;
            numModifiedDiscount.ValueChanged -= numTotalDiscount_ValueChanged;
            cmbTotalDiscountType.SelectedIndexChanged -= cmbTotalDiscountType_SelectedIndexChanged;
            dtpPmntType1.ValueChanged -= dtpPmntType1_ValueChanged;
            dtpPmntType2.ValueChanged -= dtpPmntType2_ValueChanged;
            dtpPmntType3.ValueChanged -= dtpPmntType3_ValueChanged;
            dtpPmntType4.ValueChanged -= dtpPmntType4_ValueChanged;
            dtpPmntType5.ValueChanged -= dtpPmntType5_ValueChanged;
            cmbPmntType1.SelectedIndexChanged -= cmbPmntType1_SelectedIndexChanged;
            cmbPmntType2.SelectedIndexChanged -= cmbPmntType2_SelectedIndexChanged;
            cmbPmntType3.SelectedIndexChanged -= cmbPmntType3_SelectedIndexChanged;
            cmbPmntType4.SelectedIndexChanged -= cmbPmntType4_SelectedIndexChanged;
            cmbPmntType5.SelectedIndexChanged -= cmbPmntType5_SelectedIndexChanged;
            //txtPayment.TextChanged -= txtPayment_TextChanged;
            //numPmnt1.ValueChanged -= Payments_ValueChanged;
            //numPmnt2.ValueChanged -= Payments_ValueChanged;
            //numPmnt3.ValueChanged -= Payments_ValueChanged;
            //numPmnt4.ValueChanged -= Payments_ValueChanged;
            //numPmnt5.ValueChanged -= Payments_ValueChanged;
            txtInvoiceNote.TextChanged -= txtInvoiceNote_TextChanged;
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
        }
        private void RegisterAllEvents()
        {
            txtSearchByName.TextChanged += txtSearchByName_TextChanged;
            txtSearchByPhone.TextChanged += txtSearchByPhone_TextChanged;
            txtAddress.TextChanged += txtAddress_TextChanged;
            //numTotalDiscount.ValueChanged += numTotalDiscount_ValueChanged;
            txtDiscount.TextChanged += txtDiscount_TextChanged;
            cmbTotalDiscountType.SelectedIndexChanged += cmbTotalDiscountType_SelectedIndexChanged;
            dtpPmntType1.ValueChanged += dtpPmntType1_ValueChanged;
            dtpPmntType2.ValueChanged += dtpPmntType2_ValueChanged;
            dtpPmntType3.ValueChanged += dtpPmntType3_ValueChanged;
            dtpPmntType4.ValueChanged += dtpPmntType4_ValueChanged;
            dtpPmntType5.ValueChanged += dtpPmntType5_ValueChanged;
            cmbPmntType1.SelectedIndexChanged += cmbPmntType1_SelectedIndexChanged;
            cmbPmntType2.SelectedIndexChanged += cmbPmntType2_SelectedIndexChanged;
            cmbPmntType3.SelectedIndexChanged += cmbPmntType3_SelectedIndexChanged;
            cmbPmntType4.SelectedIndexChanged += cmbPmntType4_SelectedIndexChanged;
            cmbPmntType5.SelectedIndexChanged += cmbPmntType5_SelectedIndexChanged;
            //txtPayment.TextChanged += txtPayment_TextChanged;
            //numPmnt1.ValueChanged += Payments_ValueChanged;
            //numPmnt2.ValueChanged += Payments_ValueChanged;
            //numPmnt3.ValueChanged += Payments_ValueChanged;
            //numPmnt4.ValueChanged += Payments_ValueChanged;
            //numPmnt5.ValueChanged += Payments_ValueChanged;
            txtInvoiceNote.TextChanged += txtInvoiceNote_TextChanged;
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }
        private Tuple<double, double, double> LoadInvoice(long InvoiceId)
        {
            InvoiceVM objInvoice = new InvoiceVM();
            using (unitOfWork = new UnitOfWork())
            {
                objInvoice = unitOfWork.InvoiceRepository.GetInvoiceByInvoiceId(InvoiceId);
            }


            txtSearchByName.Text = objInvoice.Patient.Name;
            txtSearchByPhone.Text = objInvoice.Patient.Phone;
            txtAddress.Text = objInvoice.Patient.Address;
            this.PatientId = objInvoice.Patient.PatientId;
            lblInvoiceNo.Text = objInvoice.InvoiceRefNo.ToString();
            pnlLoadedInvoiceData.Visible = true;
            this.LoadedInvoice_TotalPaid = objInvoice.TotalPaid;

            cmbOrderType.SelectedIndex = objInvoice.OrderType - 1;
            cmbAssignedTo.SelectedValue = objInvoice.EmployeeId == null ? 0 : (long)objInvoice.EmployeeId;
            cmbTableNo.SelectedValue = objInvoice.SeatingTableId == null ? 0 : (long)objInvoice.SeatingTableId;
            //cmbOrderStatus.SelectedIndex = objInvoice.OrderStatus - 1;
            cmbPaymentStatus.SelectedIndex = objInvoice.PaymentStatus - 1;
            if (objInvoice.PaymentStatus != 2)
            {
                pnlPayment.BackColor = Color.Red;
            }
            txtInvoiceNote.Text = objInvoice.Note;
            foreach (InvoiceItemVM i in objInvoice.InvoiceItems)
            {
                if (!i.IsActive) { continue; }
                if (i.ReturnedQuantity > 0) { IsReturnedQuantityFound = true; }
                AddItemtoGrid(i);
            }
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                foreach (DataGridViewCell c in r.Cells)
                {
                    if (IsInvoiceRefund && c.ColumnIndex == grdItems.Columns["colAffectStock"].Index)
                    {
                        continue;
                    }
                    c.ReadOnly = true;
                }
                //r.DefaultCellStyle.BackColor = Color.LightGray;
                r.Cells["colReturnQuantity"].ReadOnly = true;
            }
            numDiscount_ValueChanged(null, null);
            cmbTotalDiscountType.SelectedIndex = objInvoice.DiscountType - 1;
            //numTotalDiscount.ValueChanged += numTotalDiscount_ValueChanged;
            //numTotalDiscount.Value = (decimal)Math.Round(objInvoice.TotalDiscount, 2, MidpointRounding.AwayFromZero);
            numModifiedDiscount.Value = (decimal)objInvoice.ModifiedDiscount;
            lblAdvance.Text = objInvoice.Advance.ToString();
            int count = 1;

            //load patient details if available
            if (objInvoice.Patient.PatientId > 0)
            {
                //pnlPatientDetails.Enabled = false;
                txtSearchByName.Text = objInvoice.Patient.Name;
                txtSearchByPhone.Text = objInvoice.Patient.Phone;
            }

            foreach (InvoicePaymentVM p in objInvoice.InvoicePayments)
            {
                if (p.Payment < 0)
                {
                    InitialTotalRefundValue += Math.Abs(p.Payment);
                    continue;
                }
                if (count == 1)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        pnlPmnt1.Enabled = false;
                        chkPmnt1.Enabled = false;
                        chkPmnt1.Checked = true;

                        chkPmnt2.Visible = false;
                        chkPmnt2.Checked = false;
                        chkPmnt2.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt1.Enabled = true;
                        chkPmnt1.Enabled = true;
                        chkPmnt1.Checked = true;

                        chkPmnt2.Checked = false;
                        chkPmnt2.Enabled = true;
                        pnlPmnt2.Enabled = true;
                        pnlPmnt2.Visible = false;
                    }
                    txtInvPmntId1.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType1.Value = p.PaymentDate;
                    cmbPmntType1.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    //numPmnt1.Value = (decimal)p.Payment;
                    txtPayment.Text = p.Payment.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId1.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId1.Tag = null;
                    }
                }
                if (count == 2)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        pnlPmnt2.Enabled = false;
                        chkPmnt2.Checked = true;
                        chkPmnt2.Enabled = false;

                        chkPmnt3.Visible = false;
                        chkPmnt3.Checked = false;
                        chkPmnt3.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt1.Enabled = false;
                        chkPmnt1.Enabled = false;
                        chkPmnt1.Checked = true;

                        pnlPmnt2.Enabled = true;
                        chkPmnt2.Enabled = true;
                        chkPmnt2.Checked = true;

                        chkPmnt3.Checked = false;
                        chkPmnt3.Enabled = true;
                        pnlPmnt3.Enabled = true;
                        pnlPmnt3.Visible = false;
                    }

                    txtInvPmntId2.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType2.Value = p.PaymentDate;
                    cmbPmntType2.SelectedIndex = p.PaymentType - 1; // because index is zero based.
                    numPmnt2.Value = (decimal)p.Payment;
                    //txtChequeInfoId2.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId2.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId2.Tag = null;
                    }
                }
                if (count == 3)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        pnlPmnt3.Enabled = false;
                        chkPmnt3.Checked = true;
                        chkPmnt3.Enabled = false;

                        chkPmnt4.Visible = false;
                        chkPmnt4.Checked = false;
                        chkPmnt4.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt2.Enabled = false;
                        chkPmnt2.Enabled = false;
                        chkPmnt2.Checked = true;

                        pnlPmnt3.Enabled = true;
                        chkPmnt3.Enabled = true;
                        chkPmnt3.Checked = true;

                        chkPmnt4.Checked = false;
                        chkPmnt4.Enabled = true;
                        pnlPmnt4.Enabled = true;
                        pnlPmnt4.Visible = false;
                    }
                    txtInvPmntId3.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType3.Value = p.PaymentDate;
                    cmbPmntType3.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt3.Value = (decimal)p.Payment;
                    //txtChequeInfoId3.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId3.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId3.Tag = null;
                    }
                }
                if (count == 4)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        pnlPmnt4.Enabled = false;
                        chkPmnt4.Checked = true;
                        chkPmnt4.Enabled = false;

                        chkPmnt5.Visible = false;
                        chkPmnt5.Checked = false;
                        chkPmnt5.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt3.Enabled = false;
                        chkPmnt3.Enabled = false;
                        chkPmnt3.Checked = true;

                        pnlPmnt4.Enabled = true;
                        chkPmnt4.Enabled = true;
                        chkPmnt4.Checked = true;

                        chkPmnt5.Checked = false;
                        chkPmnt5.Enabled = true;
                        pnlPmnt5.Enabled = true;
                        pnlPmnt5.Visible = false;
                    }
                    txtInvPmntId4.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType4.Value = p.PaymentDate;
                    cmbPmntType4.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt4.Value = (decimal)p.Payment;
                    //txtChequeInfoId4.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId4.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId4.Tag = null;
                    }
                }
                if (count == 5)
                {
                    if (IsInvoiceRefund || IsReturnedQuantityFound)// no payment allowed in these both casses
                    {
                        chkPmnt5.Checked = true;
                        pnlPmnt5.Enabled = false;
                        chkPmnt5.Enabled = false;
                    }
                    else
                    {
                        pnlPmnt4.Enabled = false;
                        chkPmnt4.Enabled = false;
                        chkPmnt4.Checked = true;

                        pnlPmnt5.Enabled = true;
                        chkPmnt5.Enabled = true;
                        chkPmnt5.Checked = true; ;
                    }
                    txtInvPmntId5.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType5.Value = p.PaymentDate;
                    cmbPmntType5.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt5.Value = (decimal)p.Payment;
                    //txtChequeInfoId5.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId5.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId5.Tag = null;
                    }
                }
                count++;
            }
            //ShowLatestRefund();
            txtRefundAmount.Text = Math.Round(double.Parse(InitialTotalRefundValue.ToString()), 2, MidpointRounding.AwayFromZero).ToString();
            return new Tuple<double, double, double>(objInvoice.TotalDiscount, objInvoice.GrandTotal, objInvoice.ModifiedDiscount);
        }
        private void LoadHoldingInvoice(InvoiceVM objInvoice)
        {
            txtSearchByName.Text = objInvoice.Patient.Name;
            txtSearchByPhone.Text = objInvoice.Patient.Phone;
            this.PatientId = objInvoice.Patient.PatientId;
            lblInvoiceNo.Text = objInvoice.InvoiceRefNo.ToString();
            pnlLoadedInvoiceData.Visible = true;
            this.LoadedInvoice_TotalPaid = objInvoice.TotalPaid;
            txtInvoiceNote.Text = objInvoice.Note;
            foreach (InvoiceItemVM i in objInvoice.InvoiceItems)
            {
                if (!i.IsActive) { continue; }
                if (i.ReturnedQuantity > 0) { IsReturnedQuantityFound = true; }
                AddItemtoGrid(i);
            }
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                r.DefaultCellStyle.BackColor = Color.LightGray;
                r.Cells["colReturnQuantity"].ReadOnly = true;
            }
            numDiscount_ValueChanged(null, null);
            cmbTotalDiscountType.SelectedIndex = objInvoice.DiscountType - 1;
            //numTotalDiscount.Value = (decimal)Math.Round(objInvoice.TotalDiscount, 2, MidpointRounding.AwayFromZero);
            txtDiscount.Text = Math.Round(objInvoice.TotalDiscount, 2, MidpointRounding.AwayFromZero).ToString();
            numModifiedDiscount.Value = (decimal)objInvoice.ModifiedDiscount;
            lblAdvance.Text = objInvoice.Advance.ToString();
            int count = 1;
            cmbPmntType1.SelectedIndexChanged -= cmbPmntType1_SelectedIndexChanged;
            cmbPmntType2.SelectedIndexChanged -= cmbPmntType2_SelectedIndexChanged;
            cmbPmntType3.SelectedIndexChanged -= cmbPmntType3_SelectedIndexChanged;
            cmbPmntType4.SelectedIndexChanged -= cmbPmntType4_SelectedIndexChanged;
            cmbPmntType5.SelectedIndexChanged -= cmbPmntType5_SelectedIndexChanged;

            foreach (InvoicePaymentVM p in objInvoice.InvoicePayments)
            {

                if (count == 1)
                {

                    pnlPmnt1.Enabled = true;
                    chkPmnt1.Enabled = true;
                    chkPmnt1.Checked = true;

                    chkPmnt2.Checked = false;
                    chkPmnt2.Enabled = true;
                    pnlPmnt2.Enabled = true;
                    pnlPmnt2.Visible = false;

                    txtInvPmntId1.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType1.Value = p.PaymentDate;
                    cmbPmntType1.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    //numPmnt1.Value = (decimal)p.Payment;
                    txtPayment.Text = p.Payment.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId1.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId1.Tag = null;
                    }
                }
                if (count == 2)
                {

                    //pnlPmnt1.Enabled = false;
                    //chkPmnt1.Enabled = false;
                    chkPmnt1.Checked = true;

                    pnlPmnt2.Enabled = true;
                    chkPmnt2.Enabled = true;
                    chkPmnt2.Checked = true;

                    chkPmnt3.Checked = false;
                    chkPmnt3.Enabled = true;
                    pnlPmnt3.Enabled = true;
                    pnlPmnt3.Visible = false;

                    txtInvPmntId2.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType2.Value = p.PaymentDate;
                    cmbPmntType2.SelectedIndex = p.PaymentType - 1; // because index is zero based.
                    numPmnt2.Value = (decimal)p.Payment;
                    //txtChequeInfoId2.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId2.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId2.Tag = null;
                    }
                }
                if (count == 3)
                {

                    //pnlPmnt2.Enabled = false;
                    //chkPmnt2.Enabled = false;
                    chkPmnt2.Checked = true;

                    pnlPmnt3.Enabled = true;
                    chkPmnt3.Enabled = true;
                    chkPmnt3.Checked = true;

                    chkPmnt4.Checked = false;
                    chkPmnt4.Enabled = true;
                    pnlPmnt4.Enabled = true;
                    pnlPmnt4.Visible = false;

                    txtInvPmntId3.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType3.Value = p.PaymentDate;
                    cmbPmntType3.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt3.Value = (decimal)p.Payment;
                    //txtChequeInfoId3.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId3.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId3.Tag = null;
                    }
                }
                if (count == 4)
                {

                    //pnlPmnt3.Enabled = false;
                    //chkPmnt3.Enabled = false;
                    chkPmnt3.Checked = true;

                    pnlPmnt4.Enabled = true;
                    chkPmnt4.Enabled = true;
                    chkPmnt4.Checked = true;

                    chkPmnt5.Checked = false;
                    chkPmnt5.Enabled = true;
                    pnlPmnt5.Enabled = true;
                    pnlPmnt5.Visible = false;

                    txtInvPmntId4.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType4.Value = p.PaymentDate;
                    cmbPmntType4.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt4.Value = (decimal)p.Payment;
                    //txtChequeInfoId4.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId4.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId4.Tag = null;
                    }
                }
                if (count == 5)
                {


                    //pnlPmnt4.Enabled = false;
                    //chkPmnt4.Enabled = false;
                    chkPmnt4.Checked = true;

                    pnlPmnt5.Enabled = true;
                    chkPmnt5.Enabled = true;
                    chkPmnt5.Checked = true; ;

                    txtInvPmntId5.Text = p.InvoicePaymentId.ToString();
                    dtpPmntType5.Value = p.PaymentDate;
                    cmbPmntType5.SelectedIndex = p.PaymentType - 1; // because index is zero based. 
                    numPmnt5.Value = (decimal)p.Payment;
                    //txtChequeInfoId5.Text = p.ChequeInfoId.ToString();
                    if (p.ChequeNumber != "")
                    {
                        ChequeInfo ci = new ChequeInfo();
                        ci.ChequeNo = p.ChequeNumber;
                        ci.BankName = p.BankName;
                        ci.Status = p.ChequeStatus;
                        txtChequeInfoId5.Tag = ci;
                    }
                    else
                    {
                        txtChequeInfoId5.Tag = null;
                    }
                }
                count++;
            }
            cmbPmntType1.SelectedIndexChanged += cmbPmntType1_SelectedIndexChanged;
            cmbPmntType2.SelectedIndexChanged += cmbPmntType2_SelectedIndexChanged;
            cmbPmntType3.SelectedIndexChanged += cmbPmntType3_SelectedIndexChanged;
            cmbPmntType4.SelectedIndexChanged += cmbPmntType4_SelectedIndexChanged;
            cmbPmntType5.SelectedIndexChanged += cmbPmntType5_SelectedIndexChanged;
        }
        private void ShowLatestRefund(InvoicePaymentVM p)
        {
            dtpRedundDate.Value = p.PaymentDate;
            txtRefundReason.Text = p.RefundReason;
            txtRefundAmount.Text = p.Payment.ToString();
        }
        private void SetDisabledColor()
        {
        }

        private void LoadItems()
        {
            this.bgWItemsLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWItemsLoader_DoWork);
            this.bgWItemsLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWItemsLoader_RunWokrerCompleted);
            this.bgWItemsLoader.RunWorkerAsync();
            //Thread worker = new Thread(() => { LoadItems_Async(); });
            //worker.Name = "ItemsLoader";
            //worker.IsBackground = true;
            //worker.Start(); 
        }
        //private void LoadItems_Async()
        //{
        //    this.BeginInvoke(new Action(() =>
        //    {
        //        cmbSelectItems.Enabled = false;
        //        lblLoadingItems.Visible = true;
        //    }));
        //    List<Item> Items = new List<Item>();
        //    using (unitOfWork = new UnitOfWork())
        //    {
        //        Items = unitOfWork.ItemRspository.GetActiveItems().ToList();
        //    }

        //    this.BeginInvoke(new Action(() =>
        //    {
        //        cmbSelectItems.SelectedIndexChanged -= cmbSelectItems_SelectedIndexChanged;
        //        cmbSelectItems.DataSource = Items;
        //        cmbSelectItems.ValueMember = "ItemId";
        //        cmbSelectItems.DisplayMember = "ItemName";
        //        cmbSelectItems.SelectedIndexChanged += cmbSelectItems_SelectedIndexChanged;
        //        cmbSelectItems.SelectedIndex = -1;
        //    }));

        //    this.BeginInvoke(new Action(() =>
        //    {
        //        cmbSelectItems.Enabled = true;
        //        lblLoadingItems.Visible = false;
        //    }));
        //}
        private Tuple<double, double, double, double> GetGrandTotals()
        {
            double ReturnedQty = 0;
            double subTotal = 0;
            double GrandTotal = 0;
            double ReturnTotal = 0;
            double Discount = 0;
            double SalesTax = 0;
            double NetProfit = 0;
            double totalCost = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                //if (r.Index != EditingRowIndex)
                //{
                double colSaleQty = 0; double.TryParse(r.Cells["colQuantity"].Value.ToString(), out colSaleQty);
                double colRate = Convert.ToDouble(r.Cells["colRate"].Value);
                double colCost = Convert.ToDouble(r.Cells["colCostPrice"].Value);
                double colAmount = Convert.ToDouble(r.Cells["colAmount"].Value);
                double colDiscount = Convert.ToDouble(r.Cells["colDiscount"].Value);
                string DiscountType = r.Cells["colDiscountType"].Value.ToString();
                double colSalesTax = Convert.ToDouble(r.Cells["colSalesTax"].Value);
                string SalesTaxType = r.Cells["colSalesTaxType"].Value.ToString();
                ReturnedQty = Convert.ToDouble(r.Cells["colReturnQtyInitial"].Value);
                subTotal += colAmount;
                if (DiscountType == "%")
                {
                    colDiscount = (colDiscount / 100) * colAmount;
                }
                if (SalesTaxType == "%")
                {
                    colSalesTax = (colSalesTax / 100) * colAmount;
                }
                ReturnTotal += (colRate * ReturnedQty) - ((colDiscount / colSaleQty) * ReturnedQty);
                Discount += colDiscount;
                SalesTax += colSalesTax;
                if (colDiscount > 0)
                {
                    IsDiscountByItem = true;
                }
                totalCost += colCost * colSaleQty;
                //}
            }
            if (!IsDiscountByItem)
            {
                double disc = 0; double.TryParse(txtDiscount.Text, out disc);
                //Discount = Convert.ToDouble(numTotalDiscount.Value);
                Discount = disc;
            }
            subTotal = Math.Round(subTotal, 2, MidpointRounding.AwayFromZero);
            Discount = Math.Round(Discount, 2, MidpointRounding.AwayFromZero);
            SalesTax = Math.Round(SalesTax, 2, MidpointRounding.AwayFromZero);
            ReturnTotal = Math.Round(ReturnTotal, 2, MidpointRounding.AwayFromZero);
            GrandTotal = subTotal - Discount + SalesTax - ReturnTotal;
            return new Tuple<double, double, double, double>(subTotal, GrandTotal, Discount, totalCost);
        }

        private double GetTotalCost()
        {
            double TotalCost = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                double colSaleQty = 0; double.TryParse(r.Cells["colQuantity"].Value.ToString(), out colSaleQty);
                double colCost = Convert.ToDouble(r.Cells["colCostPrice"].Value);
                TotalCost += colCost * colSaleQty;
            }
            return TotalCost;
        }
        // no reffernce ti this
        //private void CalculateTotals()
        //{
        //    Tuple<double, double, double> GrandTotals = GetGrandTotals();
        //    double SubTotal = GrandTotals.Item1;
        //    double GrandTotal = GrandTotals.Item2;
        //    double TotalDiscount = GrandTotals.Item3;

        //    double rate = (double)numRate.Value;
        //    int qty = (int)numQuantity.Value;
        //    double amount = rate * qty;
        //    double discount = (double)numDiscount.Value;
        //    string discountType = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem);
        //    double Refund = 0;
        //    Refund = txtCurrentRefund.Text == "" ? 0 : double.Parse(txtCurrentRefund.Text);
        //    if (discountType.ToLower() == "%")
        //    {
        //        discount = (discount / 100) * amount;
        //    }
        //    SubTotal += amount;
        //    GrandTotal += amount - discount + Refund;
        //    //if (IsDiscountChanged)
        //    // {
        //    TotalDiscount += discount;
        //    //}
        //    txtAmount.Text = amount.ToString();
        //    txtNetAmount.Text = (amount - discount).ToString();
        //    lblSubTotal.Text = Math.Round(SubTotal, 2, MidpointRounding.AwayFromZero).ToString();
        //    lblGrandTotal.Text = Math.Round(GrandTotal, 2, MidpointRounding.AwayFromZero).ToString();
        //    numTotalDiscount.Value = (decimal)Math.Round(TotalDiscount, 2, MidpointRounding.AwayFromZero);
        //    numModifiedDiscount.Value = (decimal)TotalDiscount;
        //    if (!(this.IsInvoiceRefund))
        //    {
        //        if (numPmnt1.Enabled && this.LoadedInvoice_TotalPaid == 0)
        //        {
        //            if (SharedVariables.AdminInvoiceSetting.GrandTotalsOfInvoiceAsPaymentByDefault)
        //            {
        //                numPmnt1.Value = (decimal)GrandTotal;
        //            }
        //        }
        //        //var NewPayable = GetEditablePaymentBox();
        //        //if (NewPayable != null)
        //        //{
        //        //    NumericUpDown PmntBox = (NumericUpDown)NewPayable.Item1;
        //        //    string n = PmntBox.Name;
        //        //    double newPayment = GrandTotal - LoadedInvoice_TotalPaid;
        //        //    PmntBox.Value = (decimal)newPayment;
        //        //}
        //    }
        //    GetCustomerTotals();
        //}
        private void CalculateTotals()
        {
            Tuple<double, double, double, double> GrandTotals = GetGrandTotals();
            double SubTotal = GrandTotals.Item1;
            double GrandTotal = GrandTotals.Item2;
            double TotalDiscount = GrandTotals.Item3;
            double netProfit = GrandTotals.Item4;

            // commented 2020-12-29
            //double rate = Convert.ToDouble(r.Cells["colRate"].Value);
            //int qty = Convert.ToInt32(r.Cells["colQuantity"].Value);
            //double amount = rate * qty;
            //double discount = Convert.ToDouble(r.Cells["colDiscount"].Value);
            //int discountType = Convert.ToInt32(r.Cells["colDiscountTypeId"].Value);
            //double Refund = 0;
            //Refund = txtCurrentRefund.Text == "" ? 0 : double.Parse(txtCurrentRefund.Text);
            //if (discountType == 2)
            //{
            //    discount = (discount / 100) * amount;
            //}
            // commented 2020-12-29

            //SubTotal += amount;
            //GrandTotal += amount - discount + Refund;
            //TotalDiscount += discount;            
            lblSubTotal.Text = Math.Round(SubTotal, 2, MidpointRounding.AwayFromZero).ToString();
            lblGrandTotal.Text = Math.Round(GrandTotal, 2, MidpointRounding.AwayFromZero).ToString();
            double totalDisc = Math.Round(TotalDiscount, 2, MidpointRounding.AwayFromZero); // this is special case.
            //numTotalDiscount.Value = (decimal)totalDisc;
            txtDiscount.Text = totalDisc.ToString();
            numModifiedDiscount.Value = (decimal)TotalDiscount;
            lblNetProfit.Text = Math.Round((GrandTotal - netProfit)).ToString();
            decimal gt = 0;
            if (!(this.IsInvoiceRefund))
            {
                //if (numPmnt1.Enabled && this.LoadedInvoice_TotalPaid == 0)
                if (txtPayment.Enabled && this.LoadedInvoice_TotalPaid == 0)
                {
                    if (SharedVariables.AdminInvoiceSetting.GrandTotalsOfInvoiceAsPaymentByDefault)
                    {
                        decimal.TryParse(GrandTotal.ToString(), out gt);
                        //numPmnt1.Value = gt;
                        txtPayment.Text = gt.ToString();
                    }
                }
                //var NewPayable = GetEditablePaymentBox();
                //if (NewPayable != null)
                //{
                //    NumericUpDown PmntBox = (NumericUpDown)NewPayable.Item1;
                //    string n = PmntBox.Name;
                //    double newPayment = GrandTotal - LoadedInvoice_TotalPaid;
                //    PmntBox.Value = (decimal)newPayment;
                //}
            }
            GetCustomerTotals();
        }
        private Tuple<Control, double> GetEditablePaymentBox()
        {
            //double TotalPaid = (double)(numPmnt1.Value + numPmnt2.Value + numPmnt3.Value + numPmnt4.Value + numPmnt5.Value);
            //if (numPmnt1.Enabled && numPmnt1.Visible) return new Tuple<Control, double>(numPmnt1, TotalPaid);
            //if (numPmnt2.Enabled && numPmnt2.Visible) return new Tuple<Control, double>(numPmnt2, TotalPaid);
            //if (numPmnt3.Enabled && numPmnt3.Visible) return new Tuple<Control, double>(numPmnt3, TotalPaid);
            //if (numPmnt4.Enabled && numPmnt4.Visible) return new Tuple<Control, double>(numPmnt4, TotalPaid);
            //if (numPmnt5.Enabled && numPmnt5.Visible) return new Tuple<Control, double>(numPmnt5, TotalPaid);
            //else
            return null;
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            this.isNewItemAdded = true;
            if (IsValidInput())
            {
                //InvoiceItem objInvoiceItem = new InvoiceItem();
                ////FillObject(objInvoiceItem);
                //if (EditingRowIndex >= 0)
                //{
                //    if (!isValidQuantityEntered())
                //    {
                //        MessageBox.Show("Sold Quantity Can't be Greater Than Available Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //        numQuantity.Focus();
                //        numQuantity.Select(0, numQuantity.Value.ToString().Length);
                //        return;
                //    }
                //    UpdateRow(objInvoiceItem);
                //}
                //else if (ItemAlreadyAdded(Convert.ToInt32(this.SelectedItemId), Convert.ToInt64(cmbSelectBatch.SelectedValue)))
                //{
                //    MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}
                //else
                //{
                //    if (!isValidQuantityEntered())
                //    {
                //        MessageBox.Show("Sold Quantity Can't be Greater Than Available Quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //        numQuantity.Focus();
                //        numQuantity.Select(0, numQuantity.Value.ToString().Length);
                //        return;
                //    }
                //    AddItemtoGrid(objInvoiceItem);
                //}
                //Clear();
                //cmbSelectItems.Focus();

                // adding row with default values to avoid any validatio errors
                //grdItems.CurrentCellDirtyStateChanged -= grdItems_CurrentCellDirtyStateChanged;

                //    
                if (this.cmbTemplates.SelectedIndex > 0)
                {
                    TemplateVM res = GetTemplateItems(Convert.ToInt32(this.cmbTemplates.SelectedValue));
                    foreach (TemplateItemVm i in res.TemplateItems)
                    {
                        if (ItemAlreadyAdded(i.Item.ItemId))
                        {
                            MessageBox.Show("Selected Item (" + i.Item.ItemName + ") Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            continue;
                        }
                        grdItems.Rows.Add(
                            0, //invoiceitemid
                            i.Item.ItemId, //ItemId
                            "", //itemname
                            "", // batch
                            0, //batchid
                            i.Item.Rack.Name,
                            "", //unit
                            1, //conversionunit
                            0, //unitcost
                            0, //rate
                            0, //available quantity
                            i.Quantity, //quantity
                            0, // bonus quantity
                            0, //amount
                            0, //net amount
                            0, //discount
                            "%", //disocunttype
                            0, // sales Tax
                            "%", //sales tax type
                            true, //isnewrow
                            0, //return
                            0, //initial return
                            false, //affect stock
                            false // isOptionalBatchNo
                            );
                    }
                }
                else
                {
                    //if (ItemAlreadyAdded(Convert.ToInt32(this.SelectedItemId), Convert.ToInt64(cmbSelectBatch.SelectedValue)))
                    if (ItemAlreadyAdded(this.SelectedItemId))
                    {
                        MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //uC_SearchItems1.SetText = "";                       
                        uC_SearchItems1.SetFocus();
                        return;
                    }
                    grdItems.Rows.Add(
                        0,//invoiceitemid
                        this.SelectedItemId,//ItemId
                        "",//itemname
                        "",//batch
                        0,//btachid
                        "",// rackno
                        "",//unit
                        1,//conv unit
                        0,//unitcot
                        0,//retail price
                        0,//available qty
                        this.scannedItemQuantity > 0 ? this.scannedItemQuantity : 1,//qty
                        0, // bonus quantity
                        0,//amount
                        0,//net amount
                        0,//discount
                        "%", //discount type
                        0, // sales tax
                        "%", // sales tax type
                        true,//is new row
                        0,//return
                        0,//initial return
                        false,// affect stock
                        false // isoptionalbatchno.
                        );
                }
                btnClear.PerformClick();
                //uC_SearchItems1.SetFocus();
                if (!chkRepeatedScans.Checked)
                {
                    grdItems.CurrentCell = grdItems.Rows[grdItems.Rows.Count - 1].Cells["colQuantity"];
                    grdItems.BeginEdit(true);
                }
                else
                {
                    resetSearchFocus();
                }
            }
        }

        private void resetSearchFocus()
        {
            if (chkSearchItemPref.Checked)
            {
                this.uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
                this.uC_SearchItems1.SetText = "";
                this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
                this.uC_SearchItems1.SetFocus();
            }
            else
            {
                //txtBarcode.Text = "";
                //txtBarcode.Focus();
            }
        }


        private TemplateVM GetTemplateItems(int templateId)
        {
            using (unitOfWork = new UnitOfWork())
            {
                return unitOfWork.TemplateRepository.getTemplateById(templateId);
            }
        }
        private void UpdateRow(InvoiceItem objInvoiceItem)
        {
            DataGridViewRow r = grdItems.Rows[EditingRowIndex];
            r.Cells["colBatchId"].Value = objInvoiceItem.BatchId;
            r.Cells["colBatch"].Value = objInvoiceItem.Batch.BatchName;
            r.Cells["colUnit"].Value = objInvoiceItem.Unit.ToString();
            //r.Cells["colConvUnit"].Value = objInvoiceItem.ConvUnit;
            r.Cells["colRate"].Value = objInvoiceItem.Rate;
            r.Cells["colQuantity"].Value = objInvoiceItem.Quantity;
            r.Cells["colAmount"].Value = objInvoiceItem.Amount;
            r.Cells["colNetAmount"].Value = objInvoiceItem.NetAmount;
            r.Cells["colDiscount"].Value = objInvoiceItem.Discount;
            //r.Cells["colDiscountType"].Value = objInvoiceItem.DiscountTypeString;
            r.Cells["colDiscountTypeId"].Value = objInvoiceItem.DiscountType;
            DataGridViewRowsAddedEventArgs e = new DataGridViewRowsAddedEventArgs(EditingRowIndex, 1);
            grdItems_RowsAdded(null, e);
            //r.Cells["colIsNewRow"].Value = true;
        }
        private void AddItemtoGrid(InvoiceItemVM objInvoiceItem)
        {
            objInvoiceItem.UnitString = objInvoiceItem.Unit == 0 ? objInvoiceItem.Item.Unit : "Units";
            objInvoiceItem.Rate = objInvoiceItem.Unit == 0 ? objInvoiceItem.Rate * objInvoiceItem.Item.ConversionUnit : objInvoiceItem.Rate;
            grdItems.Rows.Add(objInvoiceItem.InvoiceItemId, objInvoiceItem.ItemId, objInvoiceItem.ItemName, objInvoiceItem.BatchName, objInvoiceItem.BatchId, objInvoiceItem.Rack, objInvoiceItem.UnitString, objInvoiceItem.Item.ConversionUnit, objInvoiceItem.Unit == 0 ? objInvoiceItem.UnitCost * objInvoiceItem.Item.ConversionUnit : objInvoiceItem.UnitCost, objInvoiceItem.Rate, 0, objInvoiceItem.Unit == 0 ? objInvoiceItem.Quantity / objInvoiceItem.Item.ConversionUnit : objInvoiceItem.Quantity, objInvoiceItem.Unit == 0 ? objInvoiceItem.BonusQuantity / objInvoiceItem.Item.ConversionUnit : objInvoiceItem.BonusQuantity, objInvoiceItem.Amount, objInvoiceItem.NetAmount, objInvoiceItem.Discount, objInvoiceItem.DiscountTypeString, objInvoiceItem.SalesTax, objInvoiceItem.SalesTaxTypeString, false, objInvoiceItem.ReturnedQuantity, objInvoiceItem.ReturnedQuantity, objInvoiceItem.AffectStock, objInvoiceItem.IsOptionalBatchNo);
        }
        private void FillObject(InvoiceItem objInvoiceItem)
        {
            objInvoiceItem.ItemId = this.SelectedItemId;//Convert.ToInt32(cmbSelectItems.SelectedValue);
            //objInvoiceItem.ItemName = this.SelectedItemName; //cmbSelectItems.GetItemText(cmbSelectItems.SelectedItem);
            //objInvoiceItem.UnitString = cmbUnit.GetItemText(cmbUnit.SelectedItem);
            //objInvoiceItem.ConvUnit = (int)numConvUnit.Value;
            objInvoiceItem.BatchId = Convert.ToInt32(cmbSelectBatch.SelectedValue);
            //objInvoiceItem.BatchName = cmbSelectBatch.GetItemText(cmbSelectBatch.SelectedItem);
            objInvoiceItem.Rate = (double)numRate.Value;
            objInvoiceItem.Quantity = (int)numQuantity.Value;
            objInvoiceItem.Amount = double.Parse(txtAmount.Text);
            objInvoiceItem.NetAmount = Math.Round(double.Parse(txtNetAmount.Text), 2, MidpointRounding.AwayFromZero);
            objInvoiceItem.Discount = (double)numDiscount.Value;
            objInvoiceItem.DiscountType = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem).ToLower() == "value" ? 1 : 2;
            //objInvoiceItem.DiscountTypeString = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem);
        }
        private void FillObject(InvoiceItem objInvoiceItem, DataGridViewRow r)
        {
            objInvoiceItem.ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            //objInvoiceItem.ItemName = r.Cells["colItemName"].Value.ToString();
            //int batch = Convert.ToInt32(grdItems.Rows[r.Index].Cells["colBatch"].Value);
            objInvoiceItem.BatchId = Convert.ToInt32(r.Cells["colBatchId"].Value) > 0 ? Convert.ToInt32(r.Cells["colBatchId"].Value) : 1;  // 1 is default batch id
            //objInvoiceItem.BatchId = batch;
            //objInvoiceItem.BatchName = cmbSelectBatch.GetItemText(cmbSelectBatch.SelectedItem);


            objInvoiceItem.Unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
            //objInvoiceItem.ConvUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
            objInvoiceItem.Rate = Convert.ToDouble(r.Cells["colRate"].Value);
            //objInvoiceItem.Rate = objInvoiceItem.Unit == 0 ? objInvoiceItem.Rate / objInvoiceItem.ConvUnit : objInvoiceItem.Rate; // to save quantity always in broken form.                        
            objInvoiceItem.PerUnitCostPrice = Convert.ToDouble(r.Cells["colCostPrice"].Value);
            //objInvoiceItem.PerUnitCostPrice = objInvoiceItem.Unit == 0 ? objInvoiceItem.PerUnitCostPrice / objInvoiceItem.ConvUnit : objInvoiceItem.PerUnitCostPrice; // to save quantity always in broken form.                        
            double qt = 0; double.TryParse(r.Cells["colQuantity"].Value.ToString(), out qt);
            objInvoiceItem.Quantity = qt;
            //objInvoiceItem.Quantity = objInvoiceItem.Unit == 0 ? objInvoiceItem.Quantity * objInvoiceItem.ConvUnit : objInvoiceItem.Quantity; // to save quantity always in broken form.                        
            double bQt = 0; double.TryParse(r.Cells["colBonusQuantity"].Value.ToString(), out bQt);
            objInvoiceItem.BonusQuantity = bQt;
            //objInvoiceItem.BonusQuantity = objInvoiceItem.Unit == 0 ? objInvoiceItem.BonusQuantity * objInvoiceItem.ConvUnit : objInvoiceItem.BonusQuantity; // to save quantity always in broken form


            objInvoiceItem.Amount = Convert.ToDouble(r.Cells["colAmount"].Value);
            objInvoiceItem.NetAmount = Convert.ToDouble(r.Cells["colNetAmount"].Value);
            objInvoiceItem.CalculatedNetAmount = objInvoiceItem.NetAmount;
            objInvoiceItem.Discount = Convert.ToDouble(r.Cells["colDiscount"].Value);
            //objInvoiceItem.Discount = objInvoiceItem.Unit == 0 ? objInvoiceItem.Discount / objInvoiceItem.ConvUnit : objInvoiceItem.Discount; // to save quantity always in broken form.
            objInvoiceItem.DiscountType = r.Cells["colDiscountType"].Value.ToString() == "%" ? 1 : 0;//   Convert.ToInt32(r.Cells["colDiscountTypeId"].Value);

            objInvoiceItem.SalesTax = Convert.ToDouble(r.Cells["colSalesTax"].Value);
            //objInvoiceItem.Discount = objInvoiceItem.Unit == 0 ? objInvoiceItem.Discount / objInvoiceItem.ConvUnit : objInvoiceItem.Discount; // to save quantity always in broken form.
            objInvoiceItem.SalesTaxType = r.Cells["colSalesTaxType"].Value.ToString() == "%" ? 1 : 0;//   Convert.ToInt32(r.Cells["colDiscountTypeId"].Value);
        }
        private bool IsValidInput()
        {
            ErrMessage.Text = "Alert : Please Fill All The Mandatory Fields.";
            bool ErrFound = false;
            if (this.SelectedItemId <= 0 && cmbTemplates.SelectedIndex <= 0)// (cmbSelectItems.SelectedValue == null)
            {
                ErrSelectItem.Visible = true;
                ErrFound = true;
                //cmbSelectItems.Focus();
                this.uC_SearchItems1.Focus();
                ErrMessage.Visible = true;
                return false;
            }
            else
            {
                ErrMessage.Visible = false;
                ErrSelectItem.Visible = false;
            }

            //if (cmbSelectBatch.SelectedValue == null)
            //{
            //    ErrSelectBatch.Visible = true;
            //    ErrFound = true;
            //    cmbSelectBatch.Focus();
            //    ErrMessage.Visible = true;
            //    return false;
            //}
            //else
            //{
            //    ErrMessage.Visible = false;
            //    ErrSelectBatch.Visible = false;
            //}
            //if (Convert.ToDouble(numRate.Value) <= 0)
            //{
            //    errRate.Visible = true;
            //    ErrFound = true;
            //    numRate.Focus();
            //    ErrMessage.Visible = true;
            //    return false;
            //}
            //else
            //{
            //    ErrMessage.Visible = false;
            //    errRate.Visible = false;
            //}
            //if (Convert.ToInt32(numQuantity.Value) <= 0)
            //{
            //    ErrQuantity.Visible = true;
            //    ErrFound = true;
            //    numQuantity.Focus();
            //    ErrMessage.Visible = true;
            //    return false;
            //}
            //else
            //{
            //    ErrMessage.Visible = false;
            //    ErrQuantity.Visible = false;
            //}

            //if (Convert.ToInt32(numQuantity.Value) > int.Parse(txtAvlQty.Text))
            //{
            //    ErrQuantity.Visible = true;
            //    ErrFound = true;
            //    numQuantity.Focus();
            //    MessageBox.Show("Please enter valid quantity", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    ErrMessage.Visible = true;
            //    return false;
            //}
            //else
            //{
            //    ErrMessage.Visible = false;
            //    ErrQuantity.Visible = false;
            //}

            //if (!IsCorrectItemDiscount())
            //{
            //    Errdisc.Visible = true;
            //    ErrMessage.Text = "Incorrect Discount Values.";
            //    ErrFound = true;
            //}
            //else
            //{
            //    ErrMessage.Visible = false;
            //    Errdisc.Visible = false;
            //}

            if (!ErrFound)
            {
                //ErrMessage.Visible = false;
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
            if (e.ColumnIndex == grdItems.Columns["colEdit"].Index
                //|| e.ColumnIndex == grdItems.Columns["colRemove"].Index
                )
            {
                if ((IsInvoiceEdit || IsInvoiceRefund) && !Convert.ToBoolean(grdItems.Rows[e.RowIndex].Cells["colIsNewRow"].Value))
                {
                    MessageBox.Show("Can't Edit This Row", "Invalid Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            //if (e.ColumnIndex == grdItems.Columns["colEdit"].Index) // this column later removed and edit functionality has been shifted to datagridview
            //{
            //    Clear();
            //    RegisterEvents(false);
            //    //cmbSelectItems.Enabled = false;
            //    this.uC_SearchItems1.Enabled = false;
            //    FillForm(grdItems.Rows[e.RowIndex]);
            //    grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = SharedVariables.EditingColor;
            //    EditingRowIndex = e.RowIndex;
            //    this.btnAddItem.Text = "Update Item";
            //    RegisterEvents(true);
            //    //grdItems.Rows.RemoveAt(e.RowIndex);
            //}
            if (e.ColumnIndex == grdItems.Columns["colRemove"].Index)
            {
                long InvoiceItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colInvoiceItemId"].Value);
                if (InvoiceItemId > 0)
                {
                    DialogResult rs = MessageBox.Show("Are You Sure You Want To Delete Selected Item From Invoice?", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (rs == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }
                    AddToDeletedItems(grdItems.Rows[e.RowIndex]);
                }
                //if (e.RowIndex == EditingRowIndex) { Clear(); EditingRowIndex = -1; }
                //else if (e.RowIndex < EditingRowIndex) { EditingRowIndex--; }
                int ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                ItemDetailVM foundItem = this.AddedItemsData.Where(i => i.Item.ItemId == ItemId).FirstOrDefault();
                if (foundItem != null)
                {
                    this.AddedItemsData.Remove(foundItem);
                    grdItems.Rows.RemoveAt(e.RowIndex);
                }
                else
                {
                    MessageBox.Show("Some error occurred while removing selected item, please try after reloading current invoice.", "Unexpected Issue Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void AddToDeletedItems(DataGridViewRow r)
        {
            InvoiceItemVM DeletedITem = new InvoiceItemVM();
            DeletedITem.InvoiceItemId = Convert.ToInt32(r.Cells["colInvoiceItemId"].Value);
            DeletedITem.ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
            DeletedITem.BatchId = Convert.ToInt32(r.Cells["colBatchId"].Value);
            DeletedInvoiceItems.Add(DeletedITem);
        }
        private void RegisterEvents(bool Register)
        {
            if (Register)
            {
                numRate.ValueChanged += numRate_ValueChanged;
                numQuantity.ValueChanged += numQuantity_ValueChanged;
                numDiscount.ValueChanged += numDiscount_ValueChanged;
                cmbDiscountType.SelectedIndexChanged += cmbDiscountType_SelectedIndexChanged;
            }
            else
            {
                numRate.ValueChanged -= numRate_ValueChanged;
                numQuantity.ValueChanged -= numQuantity_ValueChanged;
                numDiscount.ValueChanged -= numDiscount_ValueChanged;
                cmbDiscountType.SelectedIndexChanged -= cmbDiscountType_SelectedIndexChanged;
            }
        }
        //private void FillForm(DataGridViewRow r)
        //{

        //    this.SelectedItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
        //    using (unitOfWork = new UnitOfWork())
        //    {
        //        string unit = unitOfWork.ItemRspository.GetUnitByItemId(this.SelectedItemId);
        //        cmbUnit.Items.Add(unit);
        //        cmbUnit.Items.Add("Units");
        //    }
        //    cmbUnit.SelectedIndex = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
        //    numConvUnit.Value = Convert.ToInt32(r.Cells["colConvUnit"].Value);
        //    this.uC_SearchItems1.OnItemSelectionChanged -= uC_SearchItems1_OnItemSelectionChanged;
        //    this.uC_SearchItems1.SetText = r.Cells["colItemName"].Value.ToString();
        //    this.uC_SearchItems1.OnItemSelectionChanged += uC_SearchItems1_OnItemSelectionChanged;
        //    GetItemBatches(this.SelectedItemId);
        //    //.SelectedValue = Convert.ToInt32(r.Cells["colItemId"].Value);

        //    cmbSelectBatch.SelectedValue = Convert.ToInt32(r.Cells["colBatchId"].Value);
        //    numRate.Value = Convert.ToDecimal(r.Cells["colRate"].Value);
        //    numQuantity.Value = Convert.ToInt32(r.Cells["colQuantity"].Value);
        //    numDiscount.Value = Convert.ToDecimal(r.Cells["colDiscount"].Value);
        //    cmbDiscountType.SelectedItem = r.Cells["colDiscounttype"].Value.ToString();
        //    txtAmount.Text = r.Cells["colAmount"].Value.ToString();
        //    txtNetAmount.Text = r.Cells["colNetAmount"].Value.ToString();
        //}
        private bool ItemAlreadyAdded(long ItemId)
        {
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                if (Convert.ToInt32(r.Cells["colItemId"].Value) == ItemId
                    //&& Convert.ToInt32(r.Cells["colBatchId"].Value) == BatchId)
                    )
                {
                    return true;
                }
            }
            return false;
            //return false;
        }
        //private void LoadItemBatches(int ItemId)
        //{
        //    cmbSelectBatch.DataSource = null;
        //    cmbSelectBatch.SelectedIndex = -1;
        //    if (ItemId >= 0)
        //    {
        //        cmbSelectBatch.SelectedIndexChanged -= cmbSelectBatch_SelectedIndexChanged;
        //        GetItemBatches(ItemId);
        //        cmbSelectBatch.SelectedIndexChanged += cmbSelectBatch_SelectedIndexChanged;
        //    }
        //    if (cmbSelectBatch.Items.Count > 0)
        //    {
        //        cmbSelectBatch.SelectedIndex = 0;
        //        cmbSelectBatch_SelectedIndexChanged(null, null);
        //    }
        //}
        //private void cmbSelectItems_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    cmbSelectBatch.DataSource = null;
        //    cmbSelectBatch.SelectedIndex = -1;
        //    if (cmbSelectItems.SelectedIndex >= 0)
        //    {
        //        cmbSelectBatch.SelectedIndexChanged -= cmbSelectBatch_SelectedIndexChanged;
        //        GetItemBatches(Convert.ToInt32(cmbSelectItems.SelectedValue));
        //        cmbSelectBatch.SelectedIndexChanged += cmbSelectBatch_SelectedIndexChanged;
        //    }
        //    if (cmbSelectBatch.Items.Count > 0)
        //    {
        //        cmbSelectBatch.SelectedIndex = 0;
        //        cmbSelectBatch_SelectedIndexChanged(null, null);
        //    }
        //}
        //private void GetItemBatches(int ItemId)
        //{
        //    using (unitOfWork = new UnitOfWork())
        //    {
        //        BatchStockList = unitOfWork.ItemRspository.GetBatchStockByItemId(ItemId);
        //    }
        //    List<BatchStockVM> FinalResult = new List<BatchStockVM>();
        //    foreach (BatchStockVM i in BatchStockList)
        //    {
        //        i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock);
        //        i.BatchName = i.BatchName;// +" | Available Stock : " + i.AvailableStock;
        //        if (i.AvailableStock > 0)
        //        {
        //            FinalResult.Add(i);
        //        }
        //    }
        //    cmbSelectBatch.SelectedIndexChanged -= cmbSelectBatch_SelectedIndexChanged;
        //    cmbSelectBatch.DataSource = FinalResult;
        //    cmbSelectBatch.ValueMember = "BatchId";
        //    cmbSelectBatch.DisplayMember = "BatchName";
        //    cmbSelectBatch.SelectedIndexChanged += cmbSelectBatch_SelectedIndexChanged;
        //    if (this.cmbSelectBatch.Items.Count > 0)
        //    {
        //        cmbSelectBatch.SelectedIndex = -1; // to trigger in case there is single item in combobox
        //        cmbSelectBatch.SelectedIndex = 0;
        //    }
        //}
        private FlatDiscount loadFlatDiscount(int ItemId, DataGridViewRow r)
        {
            int dayOfWeek = (int)DateTime.Now.DayOfWeek;
            return SharedVariables.TodayDiscounts
                .Where(
                            d =>
                            (
                                d.IsAllItems
                            //|| d.DiscountItems.Any(i => i.Item.ItemId == ItemId)
                            )
                            &&
                            (
                                d.IsAllTimes
                                ||
                                (
                                    DateTime.Now.TimeOfDay >= d.StartTime.TimeOfDay
                                    && DateTime.Now.TimeOfDay <= d.EndTime.TimeOfDay
                                )
                            )
                            &&
                            (
                                d.IsAllDays
                                ||
                                d.SelectedDays.Contains(dayOfWeek.ToString())
                            )
                        )
                        .LastOrDefault();
        }
        private void GetSelectedItemData(int ItemId, DataGridViewRow r)
        {
            double qty = 0; double.TryParse(r.Cells["colQuantity"].Value.ToString(), out qty);
            double avQty = 0;
            double Rate = 0;
            double unitCost = 0;
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            Item objItem = new Item();
            ItemDetailVM itemDetail = new ItemDetailVM();
            FlatDiscount objDiscount = new FlatDiscount();
            using (unitOfWork = new UnitOfWork())
            {
                itemDetail = unitOfWork.ItemRspository.GetItemDetail(ItemId, SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);
                AddedItemsData.Add(itemDetail); // this list will be used  at time of saving invoice to reduce db cost.
                //BatchStockList = itemDetail.BatchStockList.ToList();
            }
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            r.Cells["colItemName"].Value = itemDetail.Item.ItemName;
            r.Cells["colConvUnit"].Value = itemDetail.Item.ConversionUnit;
            r.Cells["colRackNo"].Value = itemDetail.Item.Rack == null ? "" : itemDetail.Item.Rack.Name;
            //if (!isLoadingform)
            //{
            //    r.Cells["colRate"].Value = itemDetail.RetailPrice * itemDetail.Item.ConversionUnit;
            //}
            List<BatchStockVM> FinalResult = new List<BatchStockVM>();


            //foreach (BatchStockVM i in BatchStockList)
            //{
            //    i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock);
            //    if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold) // if not hold then add holded stock in total available stock
            //    {
            //        i.AvailableStock += i.HoldStock;
            //    }
            //    i.BatchName = i.BatchName + " [ " + i.AvailableStock + " ]";
            //    if (i.AvailableStock > 0)
            //    {
            //        FinalResult.Add(i);
            //    }
            //}

            //if (!SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo)
            //{
            //    DataGridViewComboBoxCell dgCmbBatch = (DataGridViewComboBoxCell)r.Cells["colBatch"];
            //    dgCmbBatch.DataSource = FinalResult;
            //    dgCmbBatch.ValueMember = "BatchId";
            //    dgCmbBatch.DisplayMember = "BatchName";

            //    if (FinalResult.Count > 0)
            //    {
            //        r.Cells["colBatchId"].Value = FinalResult[0].BatchId;
            //        r.Cells["colBatch"].Value = FinalResult[0].BatchName;
            //        if (SharedVariables.AdminPharmacySetting.IsItemDefUnitOnPOS)
            //        {
            //            avQty = FinalResult[0].AvailableStock / itemDetail.Item.ConversionUnit;
            //            Rate = FinalResult[0].RetailPrice * itemDetail.Item.ConversionUnit;
            //            unitCost = FinalResult[0].CostPrice * itemDetail.Item.ConversionUnit;
            //        }
            //        else
            //        {
            //            Rate = FinalResult[0].RetailPrice;
            //            avQty = FinalResult[0].AvailableStock;
            //            unitCost = FinalResult[0].CostPrice;
            //        }
            //    }
            //}
            //else
            //{
            //    double aq = FinalResult.Sum(f => f.AvailableStock);
            //    if (SharedVariables.AdminPharmacySetting.IsItemDefUnitOnPOS)
            //    {
            //        avQty = (aq / itemDetail.Item.ConversionUnit);
            //    }
            //    else
            //    {
            //        avQty = aq;
            //    }
            //}

            //r.Cells["colAvailableQty"].Value = avQty;
            //if (avQty < qty)
            //{
            //    if (!SharedVariables.AdminPharmacySetting.AllowNegCons)
            //    {
            //        MessageBox.Show("Availabe quantuity is less than enterd quantity. quantity will be set to available quantity automatically", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        r.Cells["colQuantity"].Value = avQty;
            //    }
            //    else
            //    {
            //        r.Cells["colQuantity"].Value = qty;
            //        //r.DefaultCellStyle.SelectionBackColor = Color.Red;
            //        //r.DefaultCellStyle.BackColor = Color.Red;
            //    }
            //}
            //else
            //{
            //    r.Cells["colQuantity"].Value = qty;
            //}
            r.Cells["colRate"].Value = itemDetail.Item.RetailPrice;
            r.Cells["colCostPrice"].Value = itemDetail.Item.UnitCostPrice;

            FlatDiscount fd = loadFlatDiscount(ItemId, r);
            if (fd != null)
            {
                r.Cells["colDiscount"].Value = fd.Discount;
                r.Cells["colDiscountType"].Value = fd.DiscountType == 0 ? "%" : "Value";
            }

            DataGridViewComboBoxCell dgCmbUnit = (DataGridViewComboBoxCell)r.Cells["colUnit"];
            string unit = itemDetail.Item.Unit;

            dgCmbUnit.Items.Add(unit);
            dgCmbUnit.Items.Add("Units");

            if (SharedVariables.AdminPharmacySetting.IsItemDefUnitOnPOS)
            {
                r.Cells["colUnit"].Value = dgCmbUnit.Items[0];
            }
            else
            {
                r.Cells["colUnit"].Value = dgCmbUnit.Items[1];
            }

            r.Cells["colConvUnit"].Value = itemDetail.Item.ConversionUnit;
            CalculateItemTotals(r);
            CalculateTotals();
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }

        private void GetSelectedItemData_1(int ItemId, DataGridViewRow r)
        {
            double avQty = 0, convUnit = 1;
            double rate = 0, cost = 0;
            convUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
            string sellUnit = r.Cells["colUnit"].Value.ToString();
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            Item objItem = new Item();
            ItemDetailVM itemDetail = new ItemDetailVM();
            FlatDiscount objDiscount = new FlatDiscount();
            using (unitOfWork = new UnitOfWork())
            {
                itemDetail = unitOfWork.ItemRspository.GetItemDetail(ItemId, SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);
                AddedItemsData.Add(itemDetail); // this list will be used  at time of saving invoice to reduce db cost.
                //BatchStockList = itemDetail.BatchStockList.ToList();
            }

            r.Cells["colItemName"].Value = itemDetail.Item.ItemName;
            r.Cells["colConvUnit"].Value = itemDetail.Item.ConversionUnit;
            r.Cells["colRackNo"].Value = itemDetail.Item.Rack == null ? "" : itemDetail.Item.Rack.Name;
            List<BatchStockVM> FinalResult = new List<BatchStockVM>();

            //if (this.HoldingInvoiceId > 0)
            //{
            //    foreach (BatchStockVM i in BatchStockList)
            //    {
            //        i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock);
            //        if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold) // if not hold then add holded stock in total available stock
            //        {
            //            i.AvailableStock += i.HoldStock;
            //        }
            //        i.BatchName = i.BatchName + " [ " + i.AvailableStock + " ]";
            //        if (i.AvailableStock > 0) // for holding invoices its mandatory to show only those batches which have some stock;
            //        {
            //            FinalResult.Add(i);
            //        }
            //    }
            //}
            //else // this will be called to populate in case user opens an old invoice for view and update, so here we will load all batches even if there is not stock in them, because its for diaplay purpose and no more consumptions will be done in edit made for old items.
            //{
            //    foreach (BatchStockVM i in BatchStockList)
            //    {
            //        // no need to check available stock in case of form loading, as it may have become zero now.
            //        i.AvailableStock = i.AvailableStock = (i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredStock);
            //        if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold) // if not hold then add holded stock in total available stock
            //        {
            //            i.AvailableStock += i.HoldStock;
            //        }
            //        i.BatchName = i.BatchName + " [ " + i.AvailableStock + " ]";
            //        FinalResult.Add(i);
            //    }
            //}


            //if (!SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo)
            //{
            //    DataGridViewComboBoxCell dgCmbBatch = (DataGridViewComboBoxCell)r.Cells["colBatch"];
            //    dgCmbBatch.DataSource = FinalResult;
            //    dgCmbBatch.ValueMember = "BatchId";
            //    dgCmbBatch.DisplayMember = "BatchName";

            //    if (FinalResult.Count > 0)
            //    {
            //        int selectedBatchId = Convert.ToInt32(r.Cells["colBatchId"].Value);
            //        if (this.HoldingInvoiceId > 0)
            //        {
            //            var batch = FinalResult.Where(b => b.BatchId == selectedBatchId).FirstOrDefault();
            //            if (batch != null)
            //            {
            //                r.Cells["colBatch"].Value = batch.BatchName;
            //                r.Cells["colBatchId"].Value = batch.BatchId;
            //                if (sellUnit != "Units")
            //                {
            //                    avQty = batch.AvailableStock / convUnit;
            //                    rate = batch.RetailPrice * convUnit;
            //                    cost = batch.CostPrice * convUnit;
            //                }
            //                else
            //                {
            //                    avQty = batch.AvailableStock;
            //                    rate = batch.RetailPrice;
            //                    cost = batch.CostPrice;
            //                }
            //            }
            //            else
            //            {
            //                r.Cells["colBatch"].Value = FinalResult[0].BatchName;
            //                r.Cells["colBatchId"].Value = FinalResult[0].BatchId;
            //                if (sellUnit != "Units")
            //                {
            //                    avQty = FinalResult[0].AvailableStock / convUnit;
            //                    rate = FinalResult[0].RetailPrice * convUnit;
            //                    cost = FinalResult[0].CostPrice * convUnit;
            //                }
            //                else
            //                {
            //                    avQty = FinalResult[0].AvailableStock;
            //                    rate = FinalResult[0].RetailPrice;
            //                    cost = FinalResult[0].CostPrice;
            //                }
            //            }
            //        }
            //        else
            //        {
            //            r.Cells["colBatch"].Value = r.Cells["colBatch"].Value;
            //            r.Cells["colBatchId"].Value = r.Cells["colBatchId"].Value;
            //            if (sellUnit != "Units")
            //            {
            //                avQty = FinalResult[0].AvailableStock / convUnit;
            //                rate = FinalResult[0].RetailPrice * convUnit;
            //                cost = FinalResult[0].CostPrice * convUnit;
            //            }
            //            else
            //            {
            //                avQty = FinalResult[0].AvailableStock;
            //                rate = FinalResult[0].RetailPrice;
            //                cost = FinalResult[0].CostPrice;
            //            }
            //        }
            //        //{
            //        //    r.Cells["colBatchId"].Value = FinalResult[0].BatchId;
            //        //    r.Cells["colBatch"].Value = FinalResult[0].BatchId;
            //        //    avQty = (int)Math.Floor((double)(FinalResult[0].AvailableStock / itemDetail.Item.ConversionUnit));
            //        //    Rate = FinalResult[0].RetailPrice * itemDetail.Item.ConversionUnit;
            //        //}
            //    }
            //}
            //else
            //{
            //    double aq = FinalResult.Sum(f => f.AvailableStock);
            //    avQty = (int)Math.Floor((double)(aq / itemDetail.Item.ConversionUnit));
            //    cost = FinalResult[0].CostPrice;
            //    rate = FinalResult[0].RetailPrice;
            //}
            rate = itemDetail.Item.RetailPrice;
            r.Cells["colRate"].Value = rate;
            r.Cells["colCostPrice"].Value = cost;
            //r.Cells["colAvailableQty"].Value = avQty;

            DataGridViewComboBoxCell dgCmbUnit = (DataGridViewComboBoxCell)r.Cells["colUnit"];
            string unit = itemDetail.Item.Unit;
            dgCmbUnit.Items.Add(unit);
            dgCmbUnit.Items.Add("Units");
            r.Cells["colUnit"].Value = r.Cells["colUnit"].Value;
            CalculateItemTotals(r);
            CalculateTotals();
            grdItems.CellValueChanged += grdItems_CellValueChanged;
        }
        private bool isValidQuantityEntered()
        {
            double AvailableStock = BatchStockList.Where(x => x.BatchId == Convert.ToInt32(cmbSelectBatch.SelectedValue)).FirstOrDefault().AvailableStock;
            if (numQuantity.Value > (decimal)AvailableStock)
            {
                //ToolTip tt = new ToolTip();
                //numQuantity.Value = 1;
                //ErrMessage.Text = "Alert : Sold Quantity Can't be Greater Than Available Quantity.";
                //ErrMessage.Visible = true;
                //tt.Show("Please Enter Valid Quantity", numQuantity, 800);
                //numQuantity.Select(0, 1);
                return false;
            }
            else
            {
                ErrMessage.Visible = false;
            }
            return true;
        }
        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {
            CalculateItemTotals();
            ////IsDiscountChanged = false;
            //if (cmbSelectBatch.SelectedIndex >= 0)
            //{
            //    //isValidQuantityEntered();
            //    CalculateTotals();
            //}
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private void Clear()
        {
            try
            {
                this.isNewItemAdded = false;
                //RegisterEvents(false);
                //cmbSelectItems.SelectedIndex = -1;
                //cmbSelectBatch.SelectedIndex = -1;
                //cmbDiscountType.SelectedIndex = 1;
                //cmbSelectItems.Enabled = true;
                //uC_SearchItems1.Enabled = true;
                //numQuantity.Value = 1;
                //numRate.Value = 0;
                //numDiscount.Value = 0;
                //this.btnAddItem.Text = "Add Item";
                //if (EditingRowIndex > -1)
                //{
                //    grdItems.Rows[EditingRowIndex].DefaultCellStyle.BackColor = Color.White;
                //}
                //EditingRowIndex = -1;
                //setNumericUD_toEmpty();
                //txtAmount.Text = "";
                //txtNetAmount.Text = "";
                //txtAvlQty.Text = "";
                //if (grdItems.Rows.Count == 0)
                //{
                //    numTotalDiscount.Enabled = true;
                //    numModifiedDiscount.Value = 0;
                //    numTotalDiscount.Value = 0;
                //}


                //cmbSelectBatch.DataSource = null;
                //cmbUnit.SelectedIndex = -1;
                //cmbUnit.Items.Clear();
                //numConvUnit.Value = 0;

                // this line must be called at end of clear method.
                this.SelectedItemId = 0;
                this.scannedItemQuantity = 0;
                cmbTemplates.SelectedIndex = 0;
                resetSearchFocus();
                //RegisterEvents(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            try
            {
                ClearAll(false);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Clearing Screen, Please Try Again OR ReOpen This Screen.", ex.Message, "Error");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            fbrResponse = new FBR_Response();
            this.ActionTime = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
            SaveInvoice(true, false, this.HoldingInvoiceId);
        }

        private User loginUser()
        {
            frmInvoiceLogin login = new frmInvoiceLogin();
            login.BringToFront();
            login.StartPosition = FormStartPosition.CenterParent;
            login.ShowDialog();
            return login.VerfiedUser;
        }
        private long SaveInvoice(bool ShowSuccessDialog, bool isHoldInvoice, int HoldingInvoiceId)
        {
            int userId = 0;
            User workingUser;
            if (SharedVariables.AdminInvoiceSetting.IsAskLoginOnInvSave && !isHoldInvoice)
            {
                workingUser = loginUser();
                if (workingUser == null)
                {
                    MessageBox.Show("Invoice save cancelled, You are not allowed to save invoice", "Un Authorized", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return 0;
                }
                userId = workingUser.UserId;
            }
            userId = SharedVariables.LoggedInUser.UserId;
            long InvoiceId = 0;
            if (grdItems.Rows.Count <= 0)
            {
                MessageBox.Show("Can't Save Empty Invoice, ", "Empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return 0;
            }
            //if (EditingRowIndex > -1) // this commented, this functionality has been removed from invoice screen, but others
            //{
            //    MessageBox.Show("Can't Save Invoice While Editing an Item, ", "Editing Invoice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return 0;
            //}

            // payment validation
            if (!IsPaymentDone())
            {
                return 0;
            }
            if (isHoldInvoice) // this will be only true when "Hold" button is pressed.
            {
                if (this.HoldingInvoiceId > 0)
                {
                    InvoiceId = EditInvoice(this.HoldingInvoiceId, ShowSuccessDialog, false, userId);
                }
                else
                {
                    InvoiceId = InsertInvoice(ShowSuccessDialog, isHoldInvoice, userId);
                }
                if (InvoiceId > 0)
                {
                    InvHoldPageNo = 0;
                }
            }
            else
            {
                if (HoldingInvoiceId > 0) // this will be true if user is editing a holding invoice
                {
                    InvoiceId = EditInvoice(HoldingInvoiceId, ShowSuccessDialog, true, userId);
                    if (InvoiceId > 0)
                    {
                        InvHoldPageNo = 0;
                        ShowHoldingInvoices(InvHoldPageNo);
                    }
                }
                else
                {
                    if (this.InvoiceEditId > 0)
                    {
                        if (IsInvoiceEdit)
                        {
                            InvoiceId = EditInvoice((int)this.InvoiceEditId, ShowSuccessDialog, false, userId);
                        }
                        else if (IsInvoiceRefund)
                        {
                            InvoiceId = RefundInvoice(ShowSuccessDialog, userId);
                        }
                    }
                    else
                    {
                        InvoiceId = InsertInvoice(ShowSuccessDialog, isHoldInvoice, userId);
                    }
                }
            }
            if (InvoiceId > 0)
            {
                ClearAll(true);
            }
            return InvoiceId;
        }
        private long InsertInvoice(bool ShowSuccessMsg, bool isHoldInvoice, int UserId)
        {
            TransactionScope scope;
            double due = 0;
            double balance = 0;
            double advance = 0;
            using (scope = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Invoice objInvoice = new Invoice();
                        objInvoice.SubTotal = double.Parse(lblSubTotal.Text);
                        objInvoice.GrandTotal = double.Parse(lblGrandTotal.Text);
                        double disc = 0; double.TryParse(txtDiscount.Text, out disc);
                        //objInvoice.TotalDiscount = Convert.ToDouble(numTotalDiscount.Value);
                        objInvoice.TotalDiscount = disc;
                        objInvoice.DiscountType = cmbTotalDiscountType.GetItemText(cmbTotalDiscountType.SelectedItem).ToLower() == "value" ? 1 : 2;
                        objInvoice.ModifiedDiscount = objInvoice.DiscountType == 1 ? objInvoice.TotalDiscount : ((objInvoice.TotalDiscount / 100) * objInvoice.SubTotal);
                        objInvoice.InvoiceRefNo = unitOfWork.InvoiceRepository.GetInvoiceRefNo();
                        objInvoice.OrderType = cmbOrderType.SelectedIndex + 1;
                        objInvoice.EmployeeId = Convert.ToInt32(cmbAssignedTo.SelectedValue) == 0 ? null : (int?)(Convert.ToInt32(cmbAssignedTo.SelectedValue));
                        if (cmbOrderType.SelectedIndex == 0)
                        {
                            // objInvoice.SeatingTableId = Convert.ToInt32(cmbTableNo.SelectedValue) == 0 ? null : (int?)(Convert.ToInt32(cmbTableNo.SelectedValue));
                        }
                        else
                        {
                            // objInvoice.SeatingTableId = null;
                        }

                        objInvoice.PaymentStatus = cmbPaymentStatus.SelectedIndex + 1;
                        objInvoice.OrderStatus = cmbOrderStatus.SelectedIndex + 1;
                        due = double.Parse(lblDue.Text);
                        balance = double.Parse(lblBalance.Text);
                        advance = double.Parse(lblAdvance.Text);

                        objInvoice.CreatedAt = this.ActionTime;
                        objInvoice.UpdatedAt = this.ActionTime;
                        objInvoice.IsNew = true;
                        objInvoice.IsActive = true;
                        //objInvoice.InvoiceItems = new List<InvoiceItem>();
                        objInvoice.UserId = SharedVariables.LoggedInUser.UserId;
                        objInvoice.IsProcedureInvoice = false;
                        objInvoice.IsHoldInvoice = isHoldInvoice;
                        objInvoice.Note = txtInvoiceNote.Text.Trim();
                        StockConsumption ObjCons = new StockConsumption();
                        ObjCons.CreatedAt = this.ActionTime;
                        ObjCons.UpdatedAt = this.ActionTime;
                        ObjCons.IsNew = true;
                        ObjCons.IsActive = true;
                        ObjCons.UserId = SharedVariables.LoggedInUser.UserId;
                        ObjCons.StockConsumptionItems = new List<StockConsumptionItem>();
                        int SrNo = 0;
                        double TCost = 0;
                        bool isLineItemDiscount = false;
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            //int val = Convert.ToInt32(r.Cells["colBatch"].Value);
                            InvoiceItem objInvoiceItem = new InvoiceItem();
                            FillObject(objInvoiceItem, r);
                            if (objInvoiceItem.Discount > 0)
                            {
                                isLineItemDiscount = true;
                            }
                            objInvoiceItem.SerialNo = SrNo += 1;
                            //objInvoiceItem.Item = unitOfWork.ItemRspository.GetById(objInvoiceItem.ItemId);
                            if (objInvoiceItem.Quantity <= 0)
                            {
                                MessageBox.Show("Some items have 0 quantity, please enter quantity value for all items.", "Batch missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return 0;
                            }
                            //if (SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo)
                            //{
                            //    objInvoiceItem.Batch = null;
                            //    objInvoiceItem.IsOptionalBatch = true;
                            //    double avQty = Convert.ToDouble(r.Cells["colAvailableQty"].Value);
                            //    if (avQty == 0)
                            //    {
                            //        MessageBox.Show("Some items have 0 stock, please remove those items from cart.", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //        return 0;
                            //    }
                            //    else if (objInvoiceItem.Quantity > avQty)
                            //    {
                            //        MessageBox.Show("Please enter valid sale quantity for all items.", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //        return 0;
                            //    }
                            //}
                            //else
                            //{
                            //    objInvoiceItem.IsOptionalBatch = false;
                            //    if (objInvoiceItem.BatchId <= 0)
                            //    {
                            //        MessageBox.Show("Please enter batch information for all items.", "Batch missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //        return 0;
                            //    }
                            //    else
                            //    {
                            //        //objInvoiceItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                            //    }

                            //    if (!IsStockAvailable(r, objInvoiceItem.BatchId, objInvoiceItem.ItemId) && !SharedVariables.AdminPharmacySetting.AllowNegCons)
                            //    {
                            //        MessageBox.Show("Stock not available for item <" + objInvoiceItem.ItemName + ">, please check sale quantity", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //        return 0;
                            //    }
                            //}
                            objInvoiceItem.Category = unitOfWork.CategoryRepository.GetCategoryByItemId(objInvoiceItem.ItemId);
                            // botton lines code has been shifted in "InsertStockConsumption()" method
                            //objInvoiceItem.TotalCostPrice = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                            //objInvoice.TotalUnitCost += objInvoiceItem.TotalCostPrice;

                            //following 2 lines of code were added due to default items issue handeling.
                            //objInvoiceItem.Item.IsSyncable = true;
                            //unitOfWork.GetDbContext().Entry(objInvoiceItem.Item).State = System.Data.Entity.EntityState.Modified;
                            objInvoiceItem.IsNew = true;
                            objInvoiceItem.IsActive = true;
                            objInvoiceItem.CreatedAt = this.ActionTime;
                            objInvoiceItem.UpdatedAt = this.ActionTime;
                            objInvoiceItem.UserId = SharedVariables.LoggedInUser.UserId;
                            TCost += InsertStockConsumption(ObjCons, ref objInvoiceItem);
                            objInvoice.TotalUnitCost = TCost;
                            //objInvoice.InvoiceItems.Add(objInvoiceItem);
                        }

                        /// get value for calculatedNetAmount value by
                        if (!isLineItemDiscount)
                        {
                            if (objInvoice.TotalDiscount > 0)
                            {
                                double perc = 0;
                                if (objInvoice.DiscountType == 2) // %
                                {
                                    perc = objInvoice.TotalDiscount;
                                }
                                else
                                {
                                    perc = (objInvoice.TotalDiscount / objInvoice.SubTotal) * 100;
                                }

                                //foreach (var i in objInvoice.InvoiceItems)
                                //{
                                //    i.CalculatedNetAmount = i.NetAmount - (i.NetAmount * perc / 100);
                                //}
                            }
                        }

                        if (!AddInvoicePayments(objInvoice))
                        {
                            return 0;
                        }
                        objInvoice.TotalPaid = objInvoice.InvoicePayments.FirstOrDefault().Payment;
                        if (this.PatientId <= 0 && (txtSearchByName.Text.Trim() != "" || txtSearchByPhone.Text.Trim() != ""))
                        {
                            Patient newP = new Patient();
                            newP.User = unitOfWork.UserRepository.GetById(UserId);
                            newP.Gender = "";
                            newP.Name = string.IsNullOrEmpty(txtSearchByName.Text.Trim()) ? "walk in" : txtSearchByName.Text.Trim();
                            newP.Phone = txtSearchByPhone.Text.Trim();
                            newP.Address = txtAddress.Text.Trim();
                            newP.CreatedAt = this.ActionTime;
                            newP.UpdatedAt = this.ActionTime;
                            newP.IsActive = true;
                            newP.IsNew = true;
                            newP.Gender = "other";
                            objInvoice.Patient = newP;
                        }
                        else
                        {
                            objInvoice.Patient = unitOfWork.PatientRepository.GetById(this.PatientId);
                            objInvoice.Patient.Name = txtSearchByName.Text;
                            objInvoice.Patient.Phone = txtSearchByPhone.Text;
                            objInvoice.Patient.Address = txtAddress.Text;
                        }

                        if (due > 0)
                        {
                            objInvoice.Due = due;
                        }
                        else if (advance > 0)
                        {
                            objInvoice.Due = -advance;
                        }
                        objInvoice.StockConsumption = ObjCons;
                        if (SharedVariables.AdminPractiseSetting.Enable_FBR == "1")
                        {
                            if (SharedVariables.AdminPractiseSetting.FBR_POSID == null)
                            {
                                MessageBox.Show("FBR POS ID not found.", "FBR POS NOT FOUND", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                fbrResponse = Generate_FBR_InvoiceNumber(objInvoice);
                                objInvoice.FBR_InvoiceNo = fbrResponse.InvoiceNumber;
                            }
                        }

                        unitOfWork.InvoiceRepository.Insert(objInvoice);
                        unitOfWork.Save();

                        // for sake of saving invoiceid to stockConsumption :  its not relation but required for syncing purpose with web
                        StockConsumption con = unitOfWork.StockConsumptionRepository.GetStockConumptionById_(objInvoice.StockConsumption.StockConsumptionId);
                        con.InvoiceId = objInvoice.InvoiceId;
                        foreach (StockConsumptionItem sci in con.StockConsumptionItems)
                        {
                            sci.InvoiceId = objInvoice.InvoiceId;
                            unitOfWork.GetDbContext().Entry<StockConsumptionItem>(sci).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        }
                        unitOfWork.StockConsumptionRepository.Update(con);
                        unitOfWork.Save();
                        scope.Complete();
                        if (ShowSuccessMsg)
                        {
                            MessageBox.Show("Invoice Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        return objInvoice.InvoiceId;
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.InnerException.Message, "Error");
                    }
                    else
                    {
                        SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                    }
                    scope.Dispose();
                    return 0;
                }
            }
        }

        private bool IsStockAvailable(DataGridViewRow r, long batchId, long ItemId)
        {
            BatchStockVM vm = new BatchStockVM();
            double saleQty = 0; double.TryParse(r.Cells["colQuantity"].Value.ToString(), out saleQty);

            vm = unitOfWork.ItemRspository.GetBatchStock(batchId, ItemId, SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);

            if (vm != null)
            {
                int convUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
                string unit = r.Cells["colUnit"].Value.ToString();
                double availableQty = vm.TotalStock + vm.AdjustedStock - vm.ConsumedStock - vm.ExpiredStock;
                if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold) // if not hold then add holded stock in total available stock
                {
                    availableQty += vm.HoldStock;
                }
                double rate = vm.RetailPrice;
                if (unit != "Units")
                {
                    rate = vm.RetailPrice * convUnit;
                    availableQty = (int)Math.Floor((double)(availableQty / convUnit));
                }
                if (saleQty <= availableQty)
                {
                    return true;
                }
            }
            return false;
        }
        private double InsertStockConsumption(StockConsumption objConsumption, ref InvoiceItem objInvoiceItem)
        {
            double itemTotalCost = 0;
            double remainingQty = objInvoiceItem.Quantity;
            if (SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo)
            {
                foreach (ItemDetailVM i in this.AddedItemsData)
                {
                    if (i.Item.ItemId == objInvoiceItem.ItemId)
                    {
                        foreach (BatchStockVM b in i.BatchStockList)
                        {
                            if (remainingQty <= b.AvailableStock)
                            {
                                objInvoiceItem.PerUnitCostPrice = unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.ItemId, b.BatchId);
                                itemTotalCost += objInvoiceItem.PerUnitCostPrice * remainingQty;
                                this.ConsumeStock(ref objConsumption, objInvoiceItem.ItemId, (int)b.BatchId, remainingQty);
                                break;
                            }
                            else
                            {
                                double consumeQty = b.AvailableStock;
                                remainingQty -= consumeQty;
                                objInvoiceItem.PerUnitCostPrice = unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.ItemId, b.BatchId);
                                itemTotalCost += objInvoiceItem.PerUnitCostPrice * consumeQty;
                                this.ConsumeStock(ref objConsumption, objInvoiceItem.ItemId, (int)b.BatchId, consumeQty);
                            }
                        }
                        break;
                    }
                }
                objInvoiceItem.TotalCostPrice = itemTotalCost;
            }
            else
            {
                objInvoiceItem.PerUnitCostPrice = unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.ItemId, objInvoiceItem.BatchId);
                objInvoiceItem.TotalCostPrice = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                itemTotalCost += objInvoiceItem.TotalCostPrice;
                this.ConsumeStock(ref objConsumption, objInvoiceItem.ItemId, (int)objInvoiceItem.BatchId, objInvoiceItem.Quantity);
            }
            return itemTotalCost;
        }

        private void ConsumeStock(ref StockConsumption objConsumption, long itemId, int batchId, double quantity)
        {
            StockConsumptionItem sc = new StockConsumptionItem();
            sc.UserId = SharedVariables.LoggedInUser.UserId;
            sc.Quantity = quantity;
            sc.ConsumptionType = (int)SharedVariables.ConsumptionType.Sales;
            sc.Comment = "Sale Transaction";
            sc.ItemId = itemId;
            sc.BatchId = batchId;
            sc.CreatedAt = this.ActionTime;
            sc.UpdatedAt = this.ActionTime;
            sc.IsNew = true;
            sc.StockConsumptionId = objConsumption.StockConsumptionId;
            sc.IsActive = true;
            objConsumption.StockConsumptionItems.Add(sc);
        }
        private long EditInvoice(int invoiceId, bool ShowSuccessMsg, bool isSavingHoldInvoice, int userId) // param "isSavingHoldInvoice" : will true only when user is going to perform "Save" action on an invoice that was on hold
        {
            TransactionScope scope;
            using (scope = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Invoice objInvoice = new Invoice();
                        objInvoice = unitOfWork.InvoiceRepository.GetInvoicceByIdWithDetails(invoiceId);
                        objInvoice.SubTotal = double.Parse(lblSubTotal.Text);
                        objInvoice.GrandTotal = double.Parse(lblGrandTotal.Text);
                        double disc = 0; double.TryParse(txtDiscount.Text, out disc);
                        objInvoice.OrderType = cmbOrderType.SelectedIndex + 1;
                        objInvoice.EmployeeId = Convert.ToInt32(cmbAssignedTo.SelectedValue) == 0 ? null : (int?)(Convert.ToInt32(cmbAssignedTo.SelectedValue));
                        if (cmbOrderType.SelectedIndex == 0)
                        {
                            //objInvoice.SeatingTableId = Convert.ToInt32(cmbTableNo.SelectedValue) == 0 ? null : (int?)(Convert.ToInt32(cmbTableNo.SelectedValue));
                        }
                        else
                        {
                            //  objInvoice.SeatingTableId = null;
                        }
                        objInvoice.PaymentStatus = cmbPaymentStatus.SelectedIndex + 1;
                        ///objInvoice.TotalDiscount = Convert.ToDouble(numTotalDiscount.Value);
                        objInvoice.TotalDiscount = disc;
                        objInvoice.DiscountType = cmbTotalDiscountType.GetItemText(cmbTotalDiscountType.SelectedItem).ToLower() == "value" ? 1 : 2;
                        if (!IsReturnedQuantityFound)
                        {
                            objInvoice.ModifiedDiscount = objInvoice.DiscountType == 1 ? objInvoice.TotalDiscount : ((objInvoice.TotalDiscount / 100) * objInvoice.SubTotal);
                        }
                        objInvoice.InvoiceNote = txtInvoiceNote.Text.Trim();
                        objInvoice.UpdatedAt = this.ActionTime;
                        objInvoice.User = unitOfWork.UserRepository.GetById(userId);
                        objInvoice.Note = txtInvoiceNote.Text.Trim();
                        objInvoice.TotalUnitCost = 0;
                        if (objInvoice.Patient != null)
                        {
                            objInvoice.Patient.Name = txtSearchByName.Text.Trim();
                            objInvoice.Patient.Phone = txtSearchByPhone.Text.Trim();
                            objInvoice.Patient.Address = txtAddress.Text.Trim();
                        }
                        if (isSavingHoldInvoice)
                        {
                            objInvoice.IsHoldInvoice = false;
                        }

                        if (objInvoice.IsSynced)
                        {
                            objInvoice.IsNew = false;
                            objInvoice.IsUpdate = true;
                            objInvoice.IsSynced = false;
                        }

                        long InvoiceItemId = 0;
                        foreach (InvoiceItemVM DeletedItem in DeletedInvoiceItems)
                        {
                            InvoiceItem _DeletedItem = new InvoiceItem();// objInvoice.InvoiceItems.Where(i => i.Item.ItemId == DeletedItem.ItemId && i.Batch.BatchId == DeletedItem.BatchId && i.IsActive).FirstOrDefault();
                            _DeletedItem.IsUpdate = true;
                            _DeletedItem.IsActive = false;
                            _DeletedItem.IsSynced = false;
                            _DeletedItem.UpdatedAt = this.ActionTime;
                            unitOfWork.GetDbContext().Entry(_DeletedItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            // commected for consume stock change
                            List<StockConsumptionItem> DeletedCons = objInvoice.StockConsumption.StockConsumptionItems.Where(c => c.Item.ItemId == DeletedItem.ItemId && c.Batch.BatchId == DeletedItem.BatchId && c.IsActive).ToList();

                            //StockConsumptionItem DeletedCon = new StockConsumptionItem();//objInvoice.StockConsumptions.Where(c => c.Item.ItemId == DeletedItem.ItemId && c.Batch.BatchId == DeletedItem.BatchId && c.IsActive).FirstOrDefault();DeletedCon.IsSynced = false;
                            foreach (StockConsumptionItem dltd in DeletedCons)
                            {
                                dltd.IsActive = false;
                                dltd.IsUpdate = true;
                                dltd.UpdatedAt = this.ActionTime;
                            }
                            //unitOfWork.GetDbContext().Entry(DeletedCon).State = System.Data.Entity.EntityState.Modified;
                        }
                        int srNo = 0;
                        double qt = 0;
                        double itemDisc = 0;
                        bool isLineItemDiscount = false;
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            qt = 0; double.TryParse(r.Cells["colQuantity"].Value.ToString(), out qt);
                            itemDisc = 0; double.TryParse(r.Cells["colDiscount"].Value.ToString(), out itemDisc);
                            InvoiceItemId = Convert.ToInt32(r.Cells["colInvoiceItemId"].Value);
                            if (qt <= 0)
                            {
                                MessageBox.Show("Some items have 0 quantity, please enter quantity value for all items.", "Batch missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return 0;
                            }
                            if (itemDisc > 0)
                            {
                                isLineItemDiscount = true;
                            }
                            if (InvoiceItemId == 0) // new items
                            {
                                InvoiceItem objInvoiceItem = new InvoiceItem();
                                FillObject(objInvoiceItem, r);

                                if (SharedVariables.AdminInvoiceSetting.IsOptionalBatchNo)
                                {
                                    objInvoiceItem.Batch = null;
                                    objInvoiceItem.IsOptionalBatch = true;
                                    int avQty = Convert.ToInt32(r.Cells["colAvailableQty"].Value);
                                    if (avQty == 0)
                                    {
                                        MessageBox.Show("Some items have 0 stock, please remove those items from cart.", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return 0;
                                    }
                                    else if (objInvoiceItem.Quantity > avQty)
                                    {
                                        MessageBox.Show("Please enter valid sale quantity for all items.", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return 0;
                                    }
                                }
                                else
                                {
                                    objInvoiceItem.IsOptionalBatch = false;
                                    if (objInvoiceItem.BatchId <= 0)
                                    {
                                        MessageBox.Show("Please enter batch information for all items.", "Batch missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return 0;
                                    }
                                    else
                                    {
                                        objInvoiceItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                    }
                                    //if (!IsStockAvailable(r, objInvoiceItem.Batch.BatchId, objInvoiceItem.ItemId))
                                    //{
                                    //    MessageBox.Show("Stock not available for item <" + objInvoiceItem.ItemName + ">, please check sale quantity", "Stock Not Available", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    //    return 0;
                                    //}
                                }
                                objInvoiceItem.SerialNo = srNo += 1;
                                //objInvoiceItem.Item = unitOfWork.ItemRspository.GetById(objInvoiceItem.ItemId);
                                //objInvoiceItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                objInvoiceItem.Category = unitOfWork.CategoryRepository.GetCategoryByItemId(objInvoiceItem.ItemId);
                                //commented due change in loginc of calculcating PerUnitCost and total unit cost for invoice and invoice items.
                                //objInvoiceItem.PerUnitCostPrice = unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.Item.ItemId, objInvoiceItem.Batch.BatchId);
                                //objInvoiceItem.TotalCostPrice = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                                //objInvoice.TotalUnitCost += objInvoiceItem.TotalCostPrice;

                                //following 2 lines of code were added due to default items issue handeling.
                                //objInvoiceItem.Item.IsSyncable = true;
                                //unitOfWork.GetDbContext().Entry(objInvoiceItem.Item).State = System.Data.Entity.EntityState.Added;

                                objInvoiceItem.IsNew = true;
                                objInvoiceItem.IsActive = true;
                                objInvoiceItem.CreatedAt = this.ActionTime;
                                objInvoiceItem.UpdatedAt = this.ActionTime;
                                objInvoiceItem.UserId = SharedVariables.LoggedInUser.UserId;
                                this.InsertStockConsumption(objInvoice.StockConsumption, ref objInvoiceItem);
                                objInvoiceItem.PerUnitCostPrice = unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.ItemId, objInvoiceItem.Batch.BatchId);
                                objInvoiceItem.TotalCostPrice = objInvoiceItem.PerUnitCostPrice * objInvoiceItem.Quantity;
                                objInvoiceItem.InvoiceId = objInvoice.InvoiceId;
                                //objInvoice.InvoiceItems.Add(objInvoiceItem);
                                //StockConsumptionItem scItem = new StockConsumptionItem();
                                //scItem.User = unitOfWork.UserRepository.GetById(userId);
                                //scItem.Quantity = objInvoiceItem.Quantity;
                                //scItem.ConsumptionType = (int)SharedVariables.ConsumptionType.Sales;
                                //scItem.Comment = "Sale Transaction";
                                //scItem.Item = objInvoiceItem.Item;
                                //scItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                                //scItem.CreatedAt = DateTime.Now;
                                //scItem.UpdatedAt = DateTime.Now;
                                //scItem.IsNew = true;
                                //scItem.IsActive = true;
                                //objInvoice.StockConsumption.StockConsumptions.Add(scItem);                                
                            }
                            // editing of an item is not allowed currently. simply uncomment below else part to enable editing of an item.
                            // also if you gonna enable editing of an item then manage stock consumption for optional batch case as in case of new invoice items insertion.
                            //else
                            //{
                            //    InvoiceItem objInvoiceItem = objInvoice.InvoiceItems.Where(item => item.InvoiceItemId == InvoiceItemId).FirstOrDefault();
                            //    FillObject(objInvoiceItem, r);
                            //    //UpdateRow(objInvoiceItem);
                            //    // item is not being updated in this case
                            //    //objInvoiceItem.Item = unitOfWork.ItemRspository.GetById(objInvoiceItem.ItemId);
                            //    objInvoiceItem.SerialNo = srNo += 1;
                            //    objInvoiceItem.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                            //    objInvoiceItem.Category = unitOfWork.CategoryRepository.GetCategoryByItemId(objInvoiceItem.ItemId);
                            //    objInvoiceItem.TotalCostPrice = (unitOfWork.ItemRspository.GetUnitCost(objInvoiceItem.Item.ItemId, objInvoiceItem.Batch.BatchId) * objInvoiceItem.Quantity);
                            //    objInvoice.TotalUnitCost += objInvoiceItem.TotalCostPrice;
                            //    objInvoiceItem.IsUpdate = true;
                            //    objInvoiceItem.IsSynced = false;
                            //    objInvoiceItem.UpdatedAt = DateTime.Now;
                            //    unitOfWork.GetDbContext().Entry(objInvoiceItem).State = System.Data.Entity.EntityState.Modified;
                            //    // commecnted for consume stock change..
                            //    StockConsumptionItem ModifiedCon = objInvoice.StockConsumption.StockConsumptions.Where(c => c.Item == objInvoiceItem.Item && c.Batch == objInvoiceItem.Batch).FirstOrDefault();
                            //    //StockConsumptionItem ModifiedCon = new StockConsumptionItem();//objInvoice.StockConsumptions.Where(c => c.Item == objInvoiceItem.Item && c.Batch == objInvoiceItem.Batch).FirstOrDefault();
                            //    ModifiedCon.User = unitOfWork.UserRepository.GetById(userId);
                            //    ModifiedCon.Quantity = objInvoiceItem.Quantity;
                            //    ModifiedCon.ConsumptionType = (int)SharedVariables.ConsumptionType.Sales;
                            //    ModifiedCon.Comment = "Sale Transaction";
                            //    //sc.Item = objInvoiceItem.Item;
                            //    ModifiedCon.Batch = unitOfWork.BatchRepository.GetById(objInvoiceItem.BatchId);
                            //    ModifiedCon.IsUpdate = true;
                            //    ModifiedCon.UpdatedAt = DateTime.Now;
                            //    ModifiedCon.IsSynced = false;
                            //    unitOfWork.GetDbContext().Entry(ModifiedCon).State = System.Data.Entity.EntityState.Modified;
                            //}
                        }
                        //objInvoice.TotalUnitCost = objInvoice.InvoiceItems.Where(ii => ii.IsActive).Sum(r => r.TotalCostPrice);
                        /// get value for calculatedNetAmount value by
                        if (!isLineItemDiscount)
                        {
                            if (objInvoice.TotalDiscount > 0)
                            {
                                double perc = 0;
                                if (objInvoice.DiscountType == 2) // %
                                {
                                    perc = objInvoice.TotalDiscount;
                                }
                                else
                                {
                                    perc = (objInvoice.TotalDiscount / objInvoice.SubTotal) * 100;
                                }

                                //foreach (var i in objInvoice.InvoiceItems)
                                //{
                                //    i.CalculatedNetAmount = i.NetAmount - (i.NetAmount * perc / 100);
                                //}
                            }
                        }
                        if (!IsReturnedQuantityFound)
                        {
                            List<InvoicePayment> NewPaymentsMade = new List<InvoicePayment>();
                            if (!AddInvoicePayments(NewPaymentsMade))
                            {
                                return 0;
                            }

                            objInvoice.TotalPaid = this.TotalPaid;
                            bool found = false;

                            foreach (InvoicePayment dp in DeletedPayments)
                            {
                                InvoicePayment DeletedPayment = objInvoice.InvoicePayments.Where(p => p.InvoicePaymentId == dp.InvoicePaymentId).FirstOrDefault();
                                DeletedPayment.IsUpdate = true;
                                DeletedPayment.IsSynced = false;
                                DeletedPayment.UpdatedAt = this.ActionTime;
                                DeletedPayment.IsActive = false;
                                unitOfWork.GetDbContext().Entry(DeletedPayment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            }
                            foreach (InvoicePayment p in NewPaymentsMade)
                            {
                                found = false;
                                foreach (InvoicePayment p2 in objInvoice.InvoicePayments)
                                {
                                    if (p.InvoicePaymentId == p2.InvoicePaymentId && p.InvoicePaymentId != 0)
                                    {
                                        found = true;
                                        p2.Payment = p.Payment;
                                        p2.ChequeNumber = p.ChequeNumber;
                                        p2.BankName = p.BankName;
                                        p2.ChequeStatus = p.ChequeStatus;
                                        p2.IsUpdate = true;
                                        p2.IsSynced = false;
                                        p2.UpdatedAt = this.ActionTime;
                                        p2.Dues = objInvoice.Due;
                                        unitOfWork.GetDbContext().Entry(p2).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                    }
                                }
                                if (!found)
                                {
                                    objInvoice.InvoicePayments.Add(p);
                                }
                            }
                        }
                        if (PatientId > 0)
                        {
                            objInvoice.Patient = unitOfWork.PatientRepository.GetById(PatientId);
                            objInvoice.Patient.Name = txtSearchByName.Text.Trim();
                            objInvoice.Patient.Phone = txtSearchByPhone.Text.Trim();
                            objInvoice.Patient.Address = txtAddress.Text.Trim();
                            objInvoice.Patient.UpdatedAt = this.ActionTime;
                            objInvoice.Patient.IsActive = true;
                            objInvoice.Patient.IsNew = false;
                            objInvoice.Patient.IsUpdate = true;
                        }
                        else
                        {
                            if (txtSearchByName.Text.Trim() != "" || txtSearchByPhone.Text.Trim() != "")
                            {
                                Patient newP = new Patient();
                                newP.Name = txtSearchByName.Text.Trim();
                                newP.Gender = "";
                                newP.Phone = txtSearchByPhone.Text.Trim();
                                newP.Address = txtAddress.Text.Trim();
                                newP.CreatedAt = this.ActionTime;
                                newP.UpdatedAt = this.ActionTime;
                                newP.IsActive = true;
                                newP.UserId = SharedVariables.LoggedInUser.UserId;
                                newP.IsNew = true;
                                objInvoice.Patient = newP;
                            }
                        }
                        unitOfWork.InvoiceRepository.Update(objInvoice);
                        unitOfWork.Save();
                        scope.Complete();
                        if (ShowSuccessMsg)
                        {
                            MessageBox.Show("Invoice Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        return invoiceId;
                    }
                }
                catch (Exception ex)
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                    scope.Dispose();
                    return 0;
                }
            }
        }
        private long RefundInvoice(bool ShowSuccessMsg, int userId)
        {
            TransactionScope scope;
            using (scope = new TransactionScope())
            {
                try
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Invoice objInvoice = new Invoice();
                        objInvoice = unitOfWork.InvoiceRepository.GetInvoiceById_Inc_Pmnts_cons(this.InvoiceEditId);
                        objInvoice.SubTotal = double.Parse(lblSubTotal.Text);
                        objInvoice.GrandTotal = double.Parse(lblGrandTotal.Text);
                        double disc = 0; double.TryParse(txtDiscount.Text, out disc);
                        //objInvoice.TotalDiscount = Convert.ToDouble(numTotalDiscount.Value);
                        objInvoice.TotalDiscount = disc;
                        objInvoice.ModifiedDiscount = Convert.ToDouble(numModifiedDiscount.Value);
                        objInvoice.DiscountType = cmbTotalDiscountType.GetItemText(cmbTotalDiscountType.SelectedItem).ToLower() == "value" ? 1 : 2;
                        objInvoice.UpdatedAt = this.ActionTime;
                        objInvoice.User = unitOfWork.UserRepository.GetById(userId);//  SharedVariables.LoggedInUser;
                        objInvoice.Note = txtInvoiceNote.Text.Trim();

                        if (objInvoice.IsSynced)
                        {
                            objInvoice.IsNew = false;
                            objInvoice.IsUpdate = true;
                            objInvoice.IsSynced = false;
                        }
                        int ItemId = 0;
                        double RtrndQty = 0;
                        bool AffectStock = true;
                        double intialReturn = 0;
                        double quantity = 0;
                        double returnCost = 0;
                        string itemName = "";
                        bool affectStock = false;
                        bool isLineItemDisc = false;
                        double lineItemDisc = 0;
                        foreach (DataGridViewRow r in grdItems.Rows)
                        {
                            lineItemDisc = 0; double.TryParse(r.Cells["colDiscount"].Value.ToString(), out lineItemDisc);
                            if (lineItemDisc > 0)
                            {
                                isLineItemDisc = true;
                            }
                            returnCost = 0;
                            affectStock = false;
                            intialReturn = Convert.ToDouble(r.Cells["colReturnQtyInitial"].Value);
                            if (intialReturn > 0)
                            {
                                continue;
                            }
                            itemName = r.Cells["colItemName"].Value.ToString();
                            AffectStock = Convert.ToBoolean(r.Cells["colAffectStock"].Value);
                            ItemId = Convert.ToInt32(r.Cells["colItemId"].Value);
                            double.TryParse(r.Cells["colReturnQuantity"].Value.ToString(), out RtrndQty);
                            double.TryParse(r.Cells["colQuantity"].Value.ToString(), out quantity);
                            if (RtrndQty > quantity)
                            {
                                MessageBox.Show("Returned quantity can't be greater than sold quantity. please check for item " + itemName, "Invalid Return", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return 0;
                            }
                            int unit = r.Cells["colUnit"].Value.ToString() == "Units" ? 1 : 0;
                            if (RtrndQty > 0)
                            {
                                //foreach (InvoiceItem i in objInvoice.InvoiceItems)
                                //{
                                //    if (i.Item.ItemId == ItemId && i.IsActive)
                                //    {
                                //        i.IsUpdate = true;
                                //        i.UpdatedAt = this.ActionTime;
                                //        i.IsSynced = false;
                                //        i.ReturnedQuantity = i.Unit == 0 ? RtrndQty * i.Item.ConversionUnit : RtrndQty;
                                //        i.PerUnitCostPrice = i.TotalCostPrice / i.Quantity; //unitOfWork.ItemRspository.GetUnitCost(i.Item.ItemId, i.Batch.BatchId);
                                //        i.CalculatedNetAmount = i.NetAmount - (i.NetAmount / i.Quantity) * i.ReturnedQuantity;
                                //        returnCost = i.PerUnitCostPrice * i.ReturnedQuantity;
                                //        i.TotalCostPrice = (i.PerUnitCostPrice * i.Quantity) - returnCost;
                                //        objInvoice.TotalUnitCost -= returnCost;
                                //        i.AffectStock = affectStock;
                                //        unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                                //    }
                                //}
                                //commented for consume stock change
                                if (AffectStock) // only revert/update stock consumption if affectStock checkbox is checked..
                                {
                                    foreach (StockConsumptionItem s in objInvoice.StockConsumption.StockConsumptionItems)
                                    {
                                        if (s.Item.ItemId == ItemId && s.IsActive)
                                        {
                                            RtrndQty = unit == 0 ? RtrndQty * s.Item.ConversionUnit : RtrndQty;
                                            double NewQty = s.Quantity - RtrndQty;
                                            s.Quantity = NewQty;
                                            s.UpdatedAt = this.ActionTime;
                                            if (s.IsSynced)
                                            {
                                                s.IsNew = false;
                                                s.IsUpdate = true;
                                                s.IsSynced = false;
                                            }
                                        }
                                        unitOfWork.GetDbContext().Entry(s).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                                    }
                                }
                            }
                        }
                        InvoicePayment ExistingRefund = objInvoice.InvoicePayments.Where(p => p.Payment < 0).Select(p => p).FirstOrDefault();
                        double refund = 0;
                        double.TryParse(txtRefundAmount.Text, out refund);
                        if (ExistingRefund == null)
                        {
                            if (refund > 0)
                            {
                                InvoicePayment p = new InvoicePayment();
                                p.PaymentType = 1;
                                p.PaymentDate = dtpRedundDate.Value;
                                p.CreatedAt = this.ActionTime;
                                p.UpdatedAt = dtpRedundDate.Value;
                                p.IsActive = true;
                                p.IsNew = true;
                                p.Payment = -1 * refund;
                                p.MethodType = 1; // in case of return method type : 1 else : 0 =>  required for syncing purpose. not used locally
                                p.User = unitOfWork.UserRepository.GetById(userId);
                                objInvoice.InvoicePayments.Add(p);
                            }
                        }
                        else
                        {
                            if (refund > 0)
                            {
                                ExistingRefund.MethodType = 1;
                                ExistingRefund.Dues = objInvoice.Due;
                                ExistingRefund.IsUpdate = true;
                                ExistingRefund.IsSynced = false;
                                ExistingRefund.UpdatedAt = this.ActionTime;
                                ExistingRefund.User = unitOfWork.UserRepository.GetById(userId);
                                ExistingRefund.Payment = -1 * refund;
                            }
                        }

                        double totalRefundAmount = 0;
                        double.TryParse(txtRefundAmount.Text, out totalRefundAmount);
                        objInvoice.TotalPaid = objInvoice.InvoicePayments.Where(p => p.IsActive).Sum(p => p.Payment);
                        //if (objInvoice.InvoicePayments.Where(i => i.Payment > 0).Sum(i => i.Payment) == totalRefundAmount)
                        //if (objInvoice.InvoiceItems.Sum(i => i.Quantity) == objInvoice.InvoiceItems.Sum(i => i.ReturnedQuantity))
                        //{
                        //    if (objInvoice.Patient != null)
                        //    {
                        //        objInvoice.Patient.Dues += objInvoice.Due;
                        //        if (objInvoice.Patient.IsSynced)
                        //        {
                        //            objInvoice.Patient.IsNew = false;
                        //            objInvoice.Patient.IsUpdate = true;
                        //            objInvoice.Patient.IsSynced = false;
                        //        }
                        //    }
                        //    objInvoice.Due = 0;
                        //}
                        /// get value for calculatedNetAmount value by
                        if (!isLineItemDisc)
                        {
                            if (objInvoice.TotalDiscount > 0)
                            {
                                double perc = 0;
                                if (objInvoice.DiscountType == 2) // %
                                {
                                    perc = objInvoice.TotalDiscount;
                                }
                                else
                                {
                                    perc = (objInvoice.TotalDiscount / objInvoice.SubTotal) * 100;
                                }

                                //foreach (var i in objInvoice.InvoiceItems)
                                //{
                                //    i.CalculatedNetAmount = i.NetAmount - (i.NetAmount * perc / 100);
                                //    i.CalculatedNetAmount -= (i.CalculatedNetAmount / i.Quantity) * i.ReturnedQuantity;
                                //}
                            }
                        }

                        //List<InvoicePayment> NewPaymentsMade = new List<InvoicePayment>();
                        //if (!AddInvoicePayments(NewPaymentsMade))
                        //{
                        //    return 0;
                        //}
                        //bool found = false;
                        //foreach (InvoicePayment p in NewPaymentsMade)
                        //{
                        //    //found = false;
                        //    //foreach (InvoicePayment p2 in objInvoice.InvoicePayments)
                        //    //{
                        //    //    if (p.InvoicePaymentId == p2.InvoicePaymentId && p.InvoicePaymentId > 0)
                        //    //    {
                        //    //        found = true;
                        //    //        p2.Payment = p.Payment;
                        //    //        p.IsUpdate = true;
                        //    //        p.UpdatedAt = DateTime.Now;
                        //    //        unitOfWork.GetDbContext().Entry(p2).State = System.Data.Entity.EntityState.Modified;
                        //    //    }
                        //    //}
                        //    //if (!found)
                        //    //{
                        //    //    objInvoice.InvoicePayments.Add(p);
                        //    //}
                        //    if (p.Payment < 0)
                        //    {
                        //        objInvoice.InvoicePayments.Add(p);
                        //    }
                        //}
                        //if (double.Parse(lblDue.Text) > 0)
                        //{
                        //    MessageBox.Show("Due Amount is (" + lblDue.Text.ToString() + " ). You Can't Insert Invoice With Due Payments.", "Due Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    return;
                        //}

                        if (PatientId > 0)
                        {
                            objInvoice.Patient = unitOfWork.PatientRepository.GetById(PatientId);
                        }
                        //foreach (DataRow r in dtStockConsumption.Rows)
                        //{
                        //    r["Invoice_InvoiceId"] = this.InvoiceEditId;
                        //}
                        //unitOfWork.StockConsumptionRespository.BulkInsert(dtStockConsumption);
                        unitOfWork.InvoiceRepository.Update(objInvoice);
                        unitOfWork.Save();
                        scope.Complete();
                        if (ShowSuccessMsg)
                        {
                            MessageBox.Show("Invoice Refund Saved Successfully", "Refund Successs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        return this.InvoiceEditId;
                    }
                }
                catch (Exception ex)
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                    scope.Dispose();
                    return 0;
                }
            }
        }
        private bool AddInvoicePayments(Invoice objInvoice)
        {
            objInvoice.InvoicePayments = new List<InvoicePayment>();
            InvoicePayment objIp = new InvoicePayment();
            objIp.InvoicePaymentId = int.Parse(txtInvPmntId1.Text);
            objIp.PaymentType = cmbPmntType1.SelectedIndex + 1;
            double pmt = 0; double.TryParse(txtPayment.Text, out pmt);
            //objIp.Payment = (double)numPmnt1.Value;
            objIp.Payment = pmt;
            objIp.PaymentDate = objInvoice.CreatedAt;
            objIp.IsNew = true;
            objIp.IsActive = true;
            objIp.CreatedAt = objInvoice.CreatedAt;
            objIp.UpdatedAt = objInvoice.CreatedAt;
            objIp.UserId = SharedVariables.LoggedInUser.UserId;
            if (objIp.PaymentType == 2)//cheque selected
            {
                if (txtChequeInfoId1.Tag != null)
                {
                    ChequeInfo ci = (ChequeInfo)txtChequeInfoId1.Tag;
                    objIp.ChequeNumber = ci.ChequeNo;
                    objIp.BankName = ci.BankName;
                    objIp.ChequeStatus = ci.Status;
                }
                //if (txtChequeInfoId1.Text != "")
                //{
                //    long chequeId = Convert.ToInt64(txtChequeInfoId1.Text);
                //    if (chequeId > 0)
                //    {                        
                //        objIp.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                //    }
                //}
                //else
                //{
                //    DialogResult rs = MessageBox.Show("Cheque Details Not Found For First Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                //    if (rs != DialogResult.OK)
                //    {
                //        return false;
                //    }
                //}
            }
            objInvoice.InvoicePayments.Add(objIp);
            return true;
        }
        private bool AddInvoicePayments(List<InvoicePayment> PaymentsList)
        {
            User u = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
            //if (numPmnt1.Value <= 0)
            //{
            //    MessageBox.Show("Please Enter Some Payment OR Uncheck Zero Payment Boxes", "Zero Payment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return false;
            //}

            if (IsInvoiceRefund)// this is set true if invoice is loaded from return button
            {
                InvoicePayment objIp6 = new InvoicePayment();
                objIp6.PaymentType = 1;
                objIp6.RefundReason = txtRefundReason.Text.Trim();
                objIp6.Payment = txtCurrentRefund.Text == "" ? 0 : -double.Parse(txtCurrentRefund.Text);
                objIp6.PaymentDate = dtpRedundDate.Value;
                objIp6.IsNew = true;
                objIp6.IsActive = true;
                objIp6.CreatedAt = this.ActionTime;
                objIp6.UpdatedAt = this.ActionTime;
                objIp6.User = u;
                PaymentsList.Add(objIp6);
                return true;
            }
            InvoicePayment objIp = new InvoicePayment();
            objIp.InvoicePaymentId = int.Parse(txtInvPmntId1.Text);
            objIp.PaymentType = cmbPmntType1.SelectedIndex + 1;
            double pmt = 0; double.TryParse(txtPayment.Text, out pmt);
            //objIp.Payment = (double)numPmnt1.Value;
            objIp.Payment = pmt;
            objIp.PaymentDate = this.ActionTime;//dtpPmntType1.Value;
            objIp.IsNew = true;
            objIp.IsActive = true;
            objIp.CreatedAt = this.ActionTime;
            objIp.UpdatedAt = this.ActionTime;
            objIp.User = u;
            this.TotalPaid += objIp.Payment;
            if (objIp.PaymentType == 2)//cheque selected
            {
                if (txtChequeInfoId1.Tag != null)
                {
                    ChequeInfo ci = (ChequeInfo)txtChequeInfoId1.Tag;
                    objIp.ChequeNumber = ci.ChequeNo;
                    objIp.BankName = ci.BankName;
                    objIp.ChequeStatus = ci.Status;
                }
                //if (txtChequeInfoId1.Text != "")
                //{
                //    long chequeId = Convert.ToInt64(txtChequeInfoId1.Text);
                //    if (chequeId > 0)
                //    {
                //        objIp.ChequeInfo = unitOfWork.ChequeInfoRepository.GetById(chequeId);
                //    }
                //}
                //else
                //{
                //    DialogResult rs = MessageBox.Show("Cheque Details Not Found For First Payment, Are You Sure You Want To Save Without Cheque Details.", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                //    if (rs != DialogResult.OK)
                //    {
                //        return false;
                //    }
                //}
            }
            PaymentsList.Add(objIp);
            return true;
        }
        private void btnAddPayment_Click(object sender, EventArgs e)
        {
            pnlPmnt2.Visible = true;
        }
        private void chkPmnt2_CheckedChanged(object sender, EventArgs e)
        {
            ////pnlPmnt2.Enabled = chkPmnt2.Checked;
            //if (chkPmnt3.Checked)
            //{
            //    chkPmnt2.Checked = true;
            //    return;
            //}
            //if (chkPmnt2.Checked)
            //{
            //    if (numPmnt1.Value <= 0)
            //    {
            //        if (!pnlPmnt1.Enabled)
            //        {

            //        }
            //        else
            //        {
            //            chkPmnt2.Checked = false;
            //            ToolTip tt = new ToolTip();
            //            tt.Show("Enter Payment Here.", numPmnt1, 1000);
            //            return;
            //        }
            //    }
            //    chkPmnt3.Visible = true;
            //    pnlPmnt2.Visible = true;
            //}
            //else
            //{
            //    pnlPmnt2.Visible = false;

            //    chkPmnt3.Checked = false;
            //    chkPmnt3.Visible = false;
            //    pnlPmnt3.Visible = false;

            //    chkPmnt4.Checked = false;
            //    chkPmnt4.Visible = false;
            //    pnlPmnt4.Visible = false;

            //    long pmntId = 0;
            //    long.TryParse(txtInvPmntId2.Text, out pmntId);
            //    if (pmntId > 0)
            //    {
            //        DeletedPayments.Add(new InvoicePayment { InvoicePaymentId = pmntId });
            //    }

            //    numPmnt2.Value = 0;
            //    dtpPmntType2.Value = DateTime.Now;
            //    txtChequeInfoId2.Text = "";
            //    txtInvPmntId2.Text = "0";

            //}
        }
        private void chkPmnt3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPmnt4.Checked)
            {
                chkPmnt3.Checked = true;
                return;
            }
            if (numPmnt2.Value <= 0)
            {
                if (!pnlPmnt2.Enabled) { }
                else
                {
                    chkPmnt3.Checked = false;
                    ToolTip tt = new ToolTip();
                    tt.Show("Enter Payment Here.", numPmnt2, 1000);
                    return;
                }
            }
            //pnlPmnt3.Enabled = chkPmnt3.Checked;
            if (chkPmnt3.Checked)
            {
                chkPmnt4.Visible = true;
                pnlPmnt3.Visible = true;
            }
            else
            {
                pnlPmnt3.Visible = false;
                chkPmnt4.Checked = false;
                chkPmnt4.Visible = false;
                pnlPmnt4.Visible = false;


                long pmntId = 0;
                long.TryParse(txtInvPmntId3.Text, out pmntId);
                if (pmntId > 0)
                {
                    DeletedPayments.Add(new InvoicePayment { InvoicePaymentId = pmntId });
                }

                numPmnt3.Value = 0;
                dtpPmntType3.Value = this.ActionTime;
                txtChequeInfoId3.Text = "";
                txtInvPmntId3.Text = "0";
            }
        }
        private void chkPmnt4_CheckedChanged(object sender, EventArgs e)
        {

            if (chkPmnt5.Checked)
            {
                chkPmnt4.Checked = true;
                return;
            }
            if (numPmnt3.Value <= 0)
            {
                if (!pnlPmnt3.Enabled) { }
                else
                {
                    chkPmnt4.Checked = false;
                    ToolTip tt = new ToolTip();
                    tt.Show("Enter Payment Here.", numPmnt2, 1000);
                    return;
                }
            }
            //pnlPmnt3.Enabled = chkPmnt3.Checked;
            if (chkPmnt4.Checked)
            {
                chkPmnt5.Visible = true;
                pnlPmnt4.Visible = true;
            }
            else
            {
                pnlPmnt4.Visible = false;
                chkPmnt5.Checked = false;
                chkPmnt5.Visible = false;
                pnlPmnt5.Visible = false;

                long pmntId = 0;
                long.TryParse(txtInvPmntId4.Text, out pmntId);
                if (pmntId > 0)
                {
                    DeletedPayments.Add(new InvoicePayment { InvoicePaymentId = pmntId });
                }

                numPmnt4.Value = 0;
                dtpPmntType4.Value = this.ActionTime;
                txtChequeInfoId4.Text = "";
                txtInvPmntId4.Text = "0";
            }
            ////////////////////////////////

        }
        private void chkPmnt5_CheckedChanged(object sender, EventArgs e)
        {
            if (numPmnt4.Value <= 0)
            {
                if (!pnlPmnt4.Enabled) { }
                else
                {
                    chkPmnt5.Checked = false;
                    ToolTip tt = new ToolTip();
                    tt.Show("Enter Payment Here.", numPmnt3, 1000);
                    return;
                }
            }
            pnlPmnt5.Visible = chkPmnt5.Checked;
            if (!chkPmnt5.Checked)
            {
                long pmntId = 0;
                long.TryParse(txtInvPmntId5.Text, out pmntId);
                if (pmntId > 0)
                {
                    DeletedPayments.Add(new InvoicePayment { InvoicePaymentId = pmntId });
                }

                numPmnt5.Value = 0;
                dtpPmntType5.Value = this.ActionTime;
                txtChequeInfoId5.Text = "";
                txtInvPmntId5.Text = "0";
            }
        }
        private void numDiscount_ValueChanged(object sender, EventArgs e)
        {
            //numTotalDiscount.Enabled = false;
            numDiscount.Value = Math.Round(numDiscount.Value, 2, MidpointRounding.AwayFromZero);
            if (IsCorrectItemDiscount())
            {
                CalculateItemTotals();
                //numTotalDiscount.Value = 0;
                //numModifiedDiscount.Value = 0;
                //CalculateTotals();
            }
            else
            {
                MessageBox.Show("You are giving more thatn 100 % discount, Please tet it to tess than er equal 100 %", "Incorrect Discount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // numDiscount.Value = 100;
                //cmbDiscountType.SelectedIndex = 1;
            }
        }
        private bool IsCorrectItemDiscount()
        {
            int discountType = cmbDiscountType.SelectedIndex + 1;
            double Disc = (double)numDiscount.Value;
            double Amount = 0;
            double.TryParse(txtAmount.Text.Trim(), out Amount);
            if (discountType == 2)
            {
                Disc = (Disc / 100) * Amount;
            }

            if (Disc > Amount)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void CalculateItemTotals()
        {
            double rate = (double)numRate.Value;
            int qty = (int)numQuantity.Value;
            double amount = rate * qty;
            double discount = (double)numDiscount.Value;
            string discountType = cmbDiscountType.GetItemText(cmbDiscountType.SelectedItem);
            double Refund = 0;
            Refund = txtCurrentRefund.Text == "" ? 0 : double.Parse(txtCurrentRefund.Text);
            if (discountType.ToLower() == "%")
            {
                discount = (discount / 100) * amount;
            }
            txtAmount.Text = amount.ToString();
            txtNetAmount.Text = (amount - discount).ToString();
            //if (IsDiscountChanged)
            // {            
            //}            
            //numTotalDiscount.Value = 0;
            //numModifiedDiscount.Value = 0;
            //CalculateTotals();
        }
        private void CalculateItemTotals(DataGridViewRow r)
        {
            double rate = Convert.ToDouble(r.Cells["colRate"].Value);
            double qty = 0; double.TryParse(r.Cells["colQuantity"].Value.ToString(), out qty);
            double amount = rate * qty;

            double discount = Convert.ToDouble(r.Cells["colDiscount"].Value);
            string discountType = r.Cells["colDiscountType"].Value.ToString();

            double salesTax = Convert.ToDouble(r.Cells["colSalesTax"].Value);
            string salesTaxType = r.Cells["colSalesTaxType"].Value.ToString();


            if (discountType.ToLower() == "%")
            {
                discount = (discount / 100) * amount;
            }

            if (salesTaxType.ToLower() == "%")
            {
                salesTax = (salesTax / 100) * amount;
            }

            ////////////////////////// validate user allowed discount here /////////////////////////
            double AllowedDiscount = this.getUserAllowedDiscount(amount);
            if (discount > AllowedDiscount)
            {
                if (discountType.ToLower() == "%")
                {
                    MessageBox.Show("Invalid Discount!" + Environment.NewLine + "You can give max " + SharedVariables.LoggedInUser.DiscLimit + " % discount.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Invalid Discount!" + Environment.NewLine + "You can give max " + AllowedDiscount + " rupees discount.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                r.Cells["colDiscount"].Value = 0;
                return;
            }
            ////////////////////////////

            //double NetAmount = Math.Round((amount - discount), 2, MidpointRounding.AwayFromZero);
            double NetAmount = Math.Round((amount - discount + salesTax), 2, MidpointRounding.AwayFromZero);
            r.Cells["colAmount"].Value = amount;
            r.Cells["colNetAmount"].Value = NetAmount.ToString();
        }

        private double getUserAllowedDiscount(double amount)
        {
            double allowedDisc = 0;
            if (SharedVariables.LoggedInUser.UserRoles.Any(rl => rl.UserRoleId == 1)) // restriction for admin
            {
                allowedDisc = 100;
            }
            else if (SharedVariables.LoggedInUser.CanGiveDiscount) //only pharmacists can give discount currently
            {
                allowedDisc = SharedVariables.LoggedInUser.DiscLimit;
            }
            allowedDisc = (allowedDisc / 100) * amount;
            return allowedDisc;
        }


        private void numRate_ValueChanged(object sender, EventArgs e)
        {
            CalculateItemTotals();
        }
        private void cmbDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //numTotalDiscount.Enabled = false;
            if (IsCorrectItemDiscount())
            {
                CalculateItemTotals();
                //numTotalDiscount.Value = 0;
                //numModifiedDiscount.Value = 0;
                //CalculateTotals();
            }
            else
            {
                MessageBox.Show("You are giving more thatn 100 % discount, Please tet it to tess than er equal 100 %", "Incorrect Discount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //numDiscount.Value = 100;
                //cmbDiscountType.SelectedIndex = 1;
            }
        }
        private ChequeInfo showChequeInfoDialog()
        {
            frmChequeInfo f = new frmChequeInfo();
            f.ShowDialog();
            ChequeInfo ChequeInfo = f.ChequeInfo;
            return ChequeInfo;
        }
        private void cmbPmntType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            if (cmbPmntType1.SelectedIndex == 1)
            {
                ChequeInfo ChequeInfo = showChequeInfoDialog();
                txtChequeInfoId1.Tag = ChequeInfo;
            }
        }
        private void cmbPmntType2_SelectedIndexChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            if (cmbPmntType2.SelectedIndex == 1)
            {
                ChequeInfo ChequeInfo = showChequeInfoDialog();
                txtChequeInfoId2.Tag = ChequeInfo;
            }
        }
        private void cmbPmntType3_SelectedIndexChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            if (cmbPmntType3.SelectedIndex == 1)
            {
                ChequeInfo ChequeInfo = showChequeInfoDialog();
                txtChequeInfoId3.Tag = ChequeInfo;
            }
        }
        private void cmbPmntType4_SelectedIndexChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            if (cmbPmntType4.SelectedIndex == 1)
            {
                ChequeInfo ChequeInfo = showChequeInfoDialog();
                txtChequeInfoId4.Tag = ChequeInfo;
            }
        }
        private void cmbPmntType5_SelectedIndexChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            if (cmbPmntType5.SelectedIndex == 1)
            {
                ChequeInfo ChequeInfo = showChequeInfoDialog();
                txtChequeInfoId5.Tag = ChequeInfo;
            }
        }
        private void grdItems_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                int ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                if (!isLoadingForm)
                {
                    isInvoiceUpdated = true;
                }
                //if (Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colDiscount"].Value) > 0)
                //{
                //    IsDiscountByItem = true;
                //    numTotalDiscount.Enabled = false;
                //    cmbTotalDiscountType.Enabled = false;
                //}      
                if (isLoadingForm || this.HoldingInvoiceId > 0 && !isNewItemAdded)
                {
                    this.GetSelectedItemData_1(ItemId, grdItems.Rows[e.RowIndex]);
                }
                else
                {
                    this.GetSelectedItemData(ItemId, grdItems.Rows[e.RowIndex]);
                }

                //CalculateTotals(grdItems.Rows[e.RowIndex]);
                //DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)grdItems.Rows[e.RowIndex].Cells["colBatch"];
                //if (cell.Items.Count > 0)
                //{
                //    grdItems.Rows[e.RowIndex].Cells["colBatch"].Value = ((BatchStockVM)cell.Items[0]).BatchName;
                //}               
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
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
                //numTotalDiscount.Enabled = true;
                txtDiscount.Enabled = true;
                cmbTotalDiscountType.Enabled = true;
            }
            Tuple<double, double, double, double> GrandTotals = GetGrandTotals();
            lblSubTotal.Text = Math.Round(GrandTotals.Item1, 2, MidpointRounding.AwayFromZero).ToString();
            lblGrandTotal.Text = Math.Round(GrandTotals.Item2, 2, MidpointRounding.AwayFromZero).ToString();
            //if (!this.IsInvoiceRefund && numPmnt1.Enabled)
            if (!this.IsInvoiceRefund && txtPayment.Enabled)
            {
                if (SharedVariables.AdminInvoiceSetting.GrandTotalsOfInvoiceAsPaymentByDefault)
                {
                    //numPmnt1.Value = (decimal)Math.Round(GrandTotals.Item2, 2, MidpointRounding.AwayFromZero);
                    txtPayment.Text = Math.Round(GrandTotals.Item2, 2, MidpointRounding.AwayFromZero).ToString();
                }
            }
            //numTotalDiscount.Value = (decimal)Math.Round(GrandTotals.Item3, 2, MidpointRounding.AwayFromZero);
            txtDiscount.Text = Math.Round(GrandTotals.Item3, 2, MidpointRounding.AwayFromZero).ToString();
            numModifiedDiscount.Value = (decimal)GrandTotals.Item3;
            GetCustomerTotals();
        }

        private void RemoveSelectedRow()
        {
            DataGridViewRow r = grdItems.SelectedRows[0];
            int rIndex = r.Index;
            long InvId = Convert.ToInt64(r.Cells["colInvoiceItemID"].Value);
            if (InvId > 0)
            {
                MessageBox.Show("This item can't be deleted.", "Action Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            grdItems.Rows.RemoveAt(rIndex);
        }

        private Control LastActiveControl = null;
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            if (keyData == (Keys.Alt | Keys.I) || keyData == (Keys.Shift | Keys.OemQuestion))
            {
                this.uC_SearchItems1.Focus();
                //cmbSelectItems.Focus();
                return true;
            }

            // remove selected row from datagridview
            if (keyData == (Keys.Alt | Keys.R))
            {
                if (this.ActiveControl.Name == grdItems.Name)
                {
                    RemoveSelectedRow();
                }
                else
                {
                    this.LastActiveControl = this.ActiveControl;
                    var res = MessageBox.Show("Are you sure you want remove selected row from grid.", "Please Make Sure", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (res == System.Windows.Forms.DialogResult.Yes)
                    {
                        this.ActiveControl = grdItems;
                        RemoveSelectedRow();
                        this.ActiveControl = this.LastActiveControl;
                    }
                }
                return true;
            }

            // remove selected row from datagridview
            if (keyData == (Keys.Alt | Keys.Q))
            {
                if (this.ActiveControl.Name == grdItems.Name)
                {
                    DataGridViewRow r = grdItems.SelectedRows[0];
                    grdItems.CurrentCell = grdItems.SelectedRows[0].Cells["colQuantity"];
                }
                else
                {
                    if (grdItems.Rows.Count > 0)
                    {
                        this.ActiveControl = grdItems;
                        grdItems.Rows[0].Selected = true;
                        grdItems.CurrentCell = grdItems.SelectedRows[0].Cells["colQuantity"];
                        grdItems.CurrentCell.Selected = true;
                    }
                }
                return true;
            }

            if (keyData == (Keys.F1))
            {
                btnSearchItems.PerformClick();
                return true;
            }

            if (keyData == (Keys.Alt | Keys.A))
            {
                btnAddItem.PerformClick();
                return true;
            }
            if (keyData == (Keys.Alt | Keys.G))
            {
                grdItems.Focus();
                if (grdItems.Rows.Count >= 1)
                {
                    grdItems.Rows[0].Selected = true;
                }
                return true;
            }
            if (keyData == (Keys.Alt | Keys.D))
            {
                btnClear.PerformClick();
                return true;
            }

            if (keyData == (Keys.Alt | Keys.X))
            {
                //numTotalDiscount.Focus();
                txtDiscount.Focus();
                return true;
            }

            if (keyData == (Keys.Alt | Keys.C))
            {
                // numPmnt1.Focus();
                txtPayment.Focus();
                return true;
            }

            if (keyData == (Keys.Alt | Keys.P))
            {
                txtSearchByName.Focus();
                return true;
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.A))
            {
                SharedFunctions.OpenForm(new frmItems(), this.MdiParent);
                return true;
            }

            if (keyData == (Keys.Control | Keys.Shift | Keys.T))
            {
                SharedFunctions.OpenForm(new frmManageStocks(), this.MdiParent);
                return true;
            }


            if (keyData == (Keys.Control | Keys.S))
            {
                btnSave.PerformClick();
                return true;
            }

            if (keyData == (Keys.Control | Keys.P))
            {
                btnSaveAndPrint.PerformClick();
                return true;
            }
            if (keyData == (Keys.Control | Keys.D))
            {
                btnClearAll.PerformClick();
                return true;
            }

            if (keyData == (Keys.Control | Keys.N))
            {
                txtSearchByName.Focus();
                return true;
            }

            if (keyData == (Keys.F10))
            {
                resetSearchFocus();
                return true;
            }
            #region grid short cuts
            if (keyData == (Keys.Alt | Keys.E))
            {
                if (grdItems.SelectedRows.Count > 0)
                {
                    int colIndex = grdItems.Columns["colEdit"].Index;
                    int rowIndex = grdItems.SelectedRows[0].Index;
                    DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
                    grdItems_CellContentClick(grdItems, e);
                    cmbSelectBatch.Focus();
                }
                return true;
            }

            //if (keyData == (Keys.Alt | Keys.R))
            //{
            //    {
            //        int colIndex = grdItems.Columns["colRemove"].Index;
            //        int rowIndex = grdItems.SelectedRows[0].Index;
            //        DataGridViewCellEventArgs e = new DataGridViewCellEventArgs(colIndex, rowIndex);
            //        grdItems_CellContentClick(grdItems, e);
            //        cmbSelectBatch.Focus();
            //    }
            //    return true;
            //}
            #endregion grid short cuts
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void Payments_ValueChanged(object sender, EventArgs e)
        {
            //commented while changin num field to text
            //isInvoiceUpdated = true;
            //GetCustomerTotals();

        }
        private void GetCustomerTotals()
        {
            double Receiveable = double.Parse(lblGrandTotal.Text);
            double pmt = 0; double.TryParse(txtPayment.Text, out pmt);
            double payments = pmt;// (double)(numPmnt1.Value);// + numPmnt2.Value + numPmnt3.Value + numPmnt4.Value + numPmnt5.Value);
            double due = 0;

            //if (Receiveable - payments >= 0)
            //{
            //    due = Receiveable - payments;
            //    Balance = 0;
            //}
            //else if (payments - Receiveable >= 0)
            //{
            //    Balance = payments - Receiveable;
            //    due = 0;
            //}
            due = Receiveable - payments;
            if (due > 0)
            {
                lblDue.Text = Math.Round(due, 2).ToString();// due.ToString();
                lblBalance.Text = "0";
                lblAdvance.Text = "0";
            }
            else// due less than zero becomes advance
            {
                lblDue.Text = "0";
                lblBalance.Text = Math.Round(-1 * due, 2, MidpointRounding.AwayFromZero).ToString();//Balance.ToString();
                //if (isBalanceActive)
                //{
                //    lblBalance.Text = Math.Round(-1 * due, 2, MidpointRounding.AwayFromZero).ToString();//Balance.ToString();
                //}
                //else
                //{
                //    lblAdvance.Text = Math.Round(-1 * due, 2, MidpointRounding.AwayFromZero).ToString();//Balance.ToString();
                //}
            }

            if (double.Parse(lblBalance.Text) > 0)
            {
                btnAddtoAdv.Enabled = true;
            }
            else
            {
                btnAddtoAdv.Enabled = false;
            }

            if (double.Parse(lblAdvance.Text) > 0)
            {
                btnAddToBal.Enabled = true;
            }
            else
            {
                btnAddToBal.Enabled = false;
            }
            if (this.IsInvoiceRefund)
            {
                btnAddToBal.Enabled = false;
                btnAddtoAdv.Enabled = false;
            }
        }
        private void numTotalDiscount_ValueChanged(object sender, EventArgs e)
        {
            //isInvoiceUpdated = true;
            ////numTotalDiscount.Value = Math.Round(numTotalDiscount.Value, 2, MidpointRounding.AwayFromZero);
            //double disc = 0; double.TryParse(txtDiscount.Text, out disc);
            //txtDiscount.Text = Math.Round(disc, 2, MidpointRounding.AwayFromZero).ToString();
            //if (grdItems.Rows.Count <= 0)
            //{ return; }

            //double SubTotal = 0;
            //double.TryParse(lblSubTotal.Text.Trim(), out SubTotal);

            //double AllowedDiscount = this.getUserAllowedDiscount(SubTotal);

            //if (disc > AllowedDiscount)
            //{
            //    if (cmbTotalDiscountType.SelectedIndex == 1)
            //    {
            //        MessageBox.Show("Invalid Discount!" + Environment.NewLine + "You can give max " + SharedVariables.LoggedInUser.DiscLimit + " % discount.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //    else
            //    {
            //        MessageBox.Show("Invalid Discount!" + Environment.NewLine + "You can give max " + AllowedDiscount + " rupees discount.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }
            //    //numTotalDiscount.Value = 0;
            //    txtDiscount.Text = "";
            //    return;
            //}
            //else
            //{
            //    GetTotalsByTotalDiscountChanges();
            //}

            ////if (isCorrectTotalDiscount())
            ////{
            ////    //if (numTotalDiscount.Enabled)
            ////    //{
            ////    //}
            ////    GetTotalsByTotalDiscountChanges();
            ////}
        }

        private bool isCorrectTotalDiscount()
        {
            double SubTotal = double.Parse(lblSubTotal.Text);
            int discType = cmbTotalDiscountType.SelectedIndex + 1;
            double disc = 0; double.TryParse(txtDiscount.Text, out disc);
            //double TotalDisc = (double)numTotalDiscount.Value;
            double TotalDisc = disc;
            if (discType == 2) // percent
            {
                double val = (TotalDisc / 100);
                TotalDisc = val * SubTotal;
            }
            if (TotalDisc > SubTotal)
            {
                ToolTip tt = new ToolTip();
                MessageBox.Show("Incorrect total discount value", "Incorrect discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //numTotalDiscount.Focus();
                txtDiscount.Focus();
                return false;
            }
            else
            {
                return true;
            }
        }
        private void cmbTotalDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            if (grdItems.Rows.Count <= 0)
            { return; }
            if (isCorrectTotalDiscount())
            {
                if (txtDiscount.Enabled)
                {
                    GetTotalsByTotalDiscountChanges();
                }
            }
        }
        private void GetTotalsByTotalDiscountChanges()
        {
            Tuple<double, double, double, double> GrandTotals = GetGrandTotals();
            double SubTotal = GrandTotals.Item1;
            double GrandTotal = GrandTotals.Item2;
            double disc = 0; double.TryParse(txtDiscount.Text, out disc);
            //double discount = (double)numTotalDiscount.Value;
            double discount = disc;
            if (cmbTotalDiscountType.SelectedIndex == 1)
            {
                discount = (discount / 100) * SubTotal;
            }
            lblSubTotal.Text = Math.Round(SubTotal, 2, MidpointRounding.AwayFromZero).ToString();
            if (SubTotal == 0)
            {
                lblGrandTotal.Text = (0).ToString();
                if (!this.IsInvoiceRefund && chkPmnt1.Enabled)
                {
                    //numPmnt1.Value = 0;
                    txtPayment.Text = "";
                }
            }
            else
            {
                lblGrandTotal.Text = Math.Round((SubTotal - discount), 2, MidpointRounding.AwayFromZero).ToString();
                //if (numPmnt1.Enabled && this.LoadedInvoice_TotalPaid == 0)
                if (txtPayment.Enabled && this.LoadedInvoice_TotalPaid == 0)
                {
                    if (SharedVariables.AdminInvoiceSetting.GrandTotalsOfInvoiceAsPaymentByDefault)
                    {
                        //numPmnt1.Value = (decimal)Math.Round((SubTotal - discount), 2, MidpointRounding.AwayFromZero);
                        txtPayment.Text = Math.Round((SubTotal - discount), 2, MidpointRounding.AwayFromZero).ToString();
                    }
                }
            }
            lblNetProfit.Text = Math.Round((SubTotal - discount) - this.GetTotalCost(), 2).ToString();
            GetCustomerTotals();
        }
        private void cmbSelectBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            getItemQuantityByBatches();
        }

        private void getItemQuantityByBatches()
        {
            if (cmbSelectBatch.SelectedIndex >= 0)
            {
                BatchStockVM r = BatchStockList.Where(b => b.BatchId == Convert.ToInt64(cmbSelectBatch.SelectedValue)).FirstOrDefault();
                if (r != null)
                {
                    double rate = 0;
                    int ItemId = this.SelectedItemId; // Convert.ToInt32(cmbSelectItems.SelectedValue);
                    int? batchId = Convert.ToInt32(cmbSelectBatch.SelectedValue);
                    using (unitOfWork = new UnitOfWork())
                    {
                        rate = unitOfWork.ItemRspository.GetItemRate(ItemId, batchId > 0 ? batchId : null);
                    }

                    double totalQty = r.TotalStock + r.AdjustedStock - r.ConsumedStock - r.ExpiredStock;
                    int conUnit = (int)numConvUnit.Value;
                    if (cmbUnit.SelectedIndex >= 0)
                    {
                        if (cmbUnit.SelectedIndex == 0)
                        {
                            totalQty = (int)Math.Floor((double)totalQty / (double)conUnit);
                            rate = rate * conUnit;
                        }
                    }
                    txtAvlQty.Text = totalQty.ToString();
                    numRate.Value = (decimal)rate;
                }
            }
        }

        private void getAvailableStock(DataGridViewRow r, int batchId, int ItemId)
        {
            BatchStockVM vm = new BatchStockVM();
            using (unitOfWork = new UnitOfWork())
            {
                vm = unitOfWork.ItemRspository.GetBatchStock(batchId, ItemId, SharedVariables.AdminPharmacySetting.IsUseNewestStockPrice);
            }
            if (vm != null)
            {
                int convUnit = Convert.ToInt32(r.Cells["colConvUnit"].Value);
                string unit = r.Cells["colUnit"].Value.ToString();
                double availableQty = vm.TotalStock + vm.AdjustedStock - vm.ConsumedStock - vm.ExpiredStock;
                if (!SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    availableQty += vm.HoldStock;
                }
                double rate = vm.RetailPrice;
                double unitCost = vm.CostPrice;
                if (unit != "Units")
                {
                    rate = vm.RetailPrice * convUnit;
                    availableQty = (int)Math.Floor((double)(availableQty / convUnit));
                }
                r.Cells["colRate"].Value = rate;
                r.Cells["colCostPrice"].Value = unitCost;
                r.Cells["colAvailableQty"].Value = availableQty;
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
                //txtSearchPatient.Text = f.PatientName;
            }
        }
        private void ResetPayments()
        {
            pnlPmnt1.Enabled = true;
            pnlPmnt2.Enabled = true;
            pnlPmnt3.Enabled = true;
            pnlPmnt4.Enabled = true;
            pnlPmnt5.Enabled = true;

            chkPmnt2.Enabled = true;
            chkPmnt3.Enabled = true;
            chkPmnt4.Enabled = true;
            chkPmnt5.Enabled = true;

            pnlPmnt2.Visible = false;
            pnlPmnt3.Visible = false;
            pnlPmnt4.Visible = false;
            pnlPmnt5.Visible = false;

            chkPmnt3.Visible = false;
            chkPmnt4.Visible = false;
            chkPmnt5.Visible = false;

            dtpPmntType1.Value = this.ActionTime;
            dtpPmntType2.Value = this.ActionTime;
            dtpPmntType3.Value = this.ActionTime;
            dtpPmntType4.Value = this.ActionTime;
            dtpPmntType5.Value = this.ActionTime;

            cmbPmntType1.SelectedIndex = 0;
            cmbPmntType2.SelectedIndex = 0;
            cmbPmntType3.SelectedIndex = 0;
            cmbPmntType4.SelectedIndex = 0;
            cmbPmntType5.SelectedIndex = 0;

            //numPmnt1.Value = 0;
            //numPmnt2.Value = 0;
            //numPmnt3.Value = 0;
            //numPmnt4.Value = 0;
            //numPmnt5.Value = 0;
            txtPayment.Text = "";

            txtChequeInfoId1.Text = "";
            txtChequeInfoId2.Text = "";
            txtChequeInfoId3.Text = "";
            txtChequeInfoId4.Text = "";
            txtChequeInfoId5.Text = "";

            txtInvPmntId1.Text = "0";
            txtInvPmntId2.Text = "0";
            txtInvPmntId3.Text = "0";
            txtInvPmntId4.Text = "0";
            txtInvPmntId5.Text = "0";

            chkPmnt5.Checked = false;
            chkPmnt4.Checked = false;
            chkPmnt3.Checked = false;
            chkPmnt2.Checked = false;
        }
        private void ClearAll(bool isAfterSave)
        {
            UnRegisterAllEvents();
            if (grdItems.Rows.Count > 0)
            {
                DialogResult rs = new DialogResult();
                if (isAfterSave)
                {
                    rs = DialogResult.OK;
                }
                else
                {
                    rs = MessageBox.Show("There are Some Unsaved Items in Grid, Are You Sure You Want To Clear", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                }
                if (rs == DialogResult.OK)
                {
                    Clear();
                    this.AddedItemsData = new List<ItemDetailVM>();
                    isInvoiceUpdated = false;
                    isLoadingForm = false;
                    this.lblInvoiceNo.Text = "";
                    // Start:   variables relevant to invoice Hold feature
                    this.HoldingInvoiceId = 0;
                    //InvHoldPageNo = 0;
                    //ShowHoldingInvoices(this.InvHoldPageNo);
                    // End:     variables relevant to invoice Hold feature
                    grdItems.Rows.Clear();
                    PatientId = 0;
                    lblGrandTotal.Text = "0";
                    lblBalance.Text = "0";
                    lblAdvance.Text = "0";
                    lblDue.Text = "0";
                    lblSubTotal.Text = "0";
                    dtStockConsumption.Rows.Clear();
                    dtStockConsumption.AcceptChanges();
                    //txtSearchPatient.Text = "";
                    InvoiceEditId = 0;
                    IsInvoiceEdit = false;
                    IsInvoiceRefund = false;
                    IsDiscountByItem = false;
                    EditingRowIndex = -1;
                    IsNameSearching = false;
                    IsPhoneSearching = false;
                    numDiscount.Value = 0;
                    //numTotalDiscount.Value = 0;
                    txtDiscount.Text = "";
                    cmbTotalDiscountType.SelectedIndex = 0;
                    //numModifiedDiscount.Value = 0;
                    //patient details controls clear
                    txtSearchByName.Text = "";
                    txtSearchByPhone.Text = "";
                    txtAddress.Text = "";
                    lblNetProfit.Text = "";
                    ResetPayments();

                    pnlPatientDetails.Enabled = true;

                    grdItems.Columns["colEdit"].Visible = true;
                    grdItems.Columns["colRemove"].Visible = true;
                    grdItems.Columns["colReturnToggle"].Visible = false;

                    IsReturnedQuantityFound = false;
                    DeletedPayments = new List<InvoicePayment>();
                    DeletedInvoiceItems = new List<InvoiceItemVM>();
                    txtInvoiceNote.Text = "";
                    LoadedInvoice_TotalPaid = 0;
                    this.TotalPaid = 0;
                    cmbOrderType.SelectedIndex = 0;
                    cmbPaymentStatus.SelectedIndex = 0;
                    cmbOrderStatus.SelectedIndex = 0;
                }
            }
            RegisterAllEvents();
        }
        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAddPatient(), this.MdiParent);
        }
        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            try
            {
                LoadItems();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Refershing Items, Please Try Reopen Sale Form.", ex.Message, "Error");
            }
        }
        private void grdItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            grdItems.CellValueChanged -= grdItems_CellValueChanged;
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == grdItems.Columns["colReturnQuantity"].Index)
            {
                double Qty = 0; double.TryParse(grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value.ToString(), out Qty);
                double RQty = 0;
                double.TryParse(grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value.ToString(), out RQty);
                grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = RQty; // this to avoid funrther checks on calculateFormuale
                if (RQty > Qty)
                {
                    MessageBox.Show("Return Quantity Can't be Greater Than Sale Quantity", "Invalid Return", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    grdItems.Rows[e.RowIndex].Cells["colReturnQuantity"].Value = Qty;
                    grdItems.CellValueChanged += grdItems_CellValueChanged;
                    return;
                }
                CalculateRefundAmount2();
            }
            double avQty = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value);
            double sQty = 0; double.TryParse(grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value.ToString(), out sQty);
            if (e.ColumnIndex == grdItems.Columns["colQuantity"].Index
                || e.ColumnIndex == grdItems.Columns["colBonusQuantity"].Index
                || e.ColumnIndex == grdItems.Columns["colDiscount"].Index
                || e.ColumnIndex == grdItems.Columns["colRate"].Index
                || e.ColumnIndex == grdItems.Columns["colDiscountType"].Index
                || e.ColumnIndex == grdItems.Columns["colSalesTax"].Index
                || e.ColumnIndex == grdItems.Columns["colSalesTaxType"].Index
                )
            {
                if (e.ColumnIndex == grdItems.Columns["colRate"].Index)
                {
                    double cst = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colCostPrice"].Value);
                    double ret = Convert.ToDouble(grdItems.Rows[e.RowIndex].Cells["colRate"].Value);
                    if (!SharedVariables.AdminInvoiceSetting.AllowBelowCostSale)
                    {
                        if (ret < cst)
                        {
                            MessageBox.Show("Retail can't be less than cost price.", "Ivalid Retail price", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            grdItems.Rows[e.RowIndex].Cells["colRate"].Value = grdItems.Rows[e.RowIndex].Cells["colCostPrice"].Value;
                        }
                    }
                }
                CalculateItemTotals(grdItems.Rows[e.RowIndex]);
                CalculateTotals();
            }
            else if (e.ColumnIndex == grdItems.Columns["colBatch"].Index || e.ColumnIndex == grdItems.Columns["colBatchId"].Index)
            {
                int ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                int batchId = 0;
                batchId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value);
                if (e.ColumnIndex == grdItems.Columns["colBatchId"].Index) // in case of edit
                {
                    batchId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value);
                }
                else
                {
                    batchId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colBatch"].Value);
                    grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value = batchId;
                }
                getAvailableStock(grdItems.Rows[e.RowIndex], batchId, ItemId);
                CalculateItemTotals(grdItems.Rows[e.RowIndex]);
                CalculateTotals();
            }
            else if (e.ColumnIndex == grdItems.Columns["colUnit"].Index)
            {
                int ItemId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colItemId"].Value);
                int batchId = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colBatchId"].Value);
                int cu = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["ColConvUnit"].Value);
                //int qt = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value);
                BatchStockVM vm = this.AddedItemsData.Where(i => i.Item.ItemId == ItemId).FirstOrDefault().BatchStockList.Where(b => b.BatchId == batchId).FirstOrDefault();
                if (grdItems.Rows[e.RowIndex].Cells["colUnit"].Value.ToString().Equals("Units"))
                {
                    grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value = vm.AvailableStock;
                    grdItems.Rows[e.RowIndex].Cells["colRate"].Value = vm.RetailPrice;
                    grdItems.Rows[e.RowIndex].Cells["colCostPrice"].Value = vm.CostPrice;
                }
                else
                {
                    grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value = Math.Floor((double)(vm.AvailableStock / cu));
                    grdItems.Rows[e.RowIndex].Cells["colRate"].Value = vm.RetailPrice * cu;
                    grdItems.Rows[e.RowIndex].Cells["colCostPrice"].Value = vm.CostPrice * cu;
                }
                CalculateItemTotals(grdItems.Rows[e.RowIndex]);
                CalculateTotals();

            }
            grdItems.CellValueChanged += grdItems_CellValueChanged;

            if (e.ColumnIndex != grdItems.Columns["colReturnQuantity"].Index && e.ColumnIndex != grdItems.Columns["colReturnToggle"].Index && e.ColumnIndex != grdItems.Columns["colAffectStock"].Index)
            {
                avQty = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value);
                sQty = 0; double.TryParse(grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value.ToString(), out sQty);
                if (sQty > avQty)
                {
                    if (!SharedVariables.AdminPharmacySetting.AllowNegCons)
                    {
                        MessageBox.Show("Please enter valid quantity from available quantity.", "Invalid Sale Quantity", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value = avQty;
                    }
                    else
                    {
                        grdItems.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Red;
                        grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
                else
                {
                    grdItems.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = SharedVariables.RowSelectionBackColor;
                    grdItems.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.White;
                }
            }
            //avQty = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colAvailableQty"].Value);
            //sQty = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value);
            //if (sQty > avQty)
            //{
            //    MessageBox.Show("Please enter valid quantity from available quantity.", "Invalid Sale Quantity", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    grdItems.Rows[e.RowIndex].Cells["colQuantity"].Value = avQty;
            //}
        }
        private void CalculateRefundAmount()
        {
            double totalRefundAmount = 0;
            double GrandTotal = double.Parse(lblGrandTotal.Text);
            double SubTotal = double.Parse(lblSubTotal.Text);

            double Rate = 0;
            double ItemTotal = 0;
            double ItemReturnTotal = 0;
            double ItemReturnGrandTotal = 0;
            double DiscAmt = 0;
            foreach (DataGridViewRow r in grdItems.Rows)
            {
                double rtQty = 0; double.TryParse(r.Cells["colReturnQuantity"].Value.ToString(), out rtQty);
                double SaleQty = 0; double.TryParse(r.Cells["colQuantity"].Value.ToString(), out SaleQty);
                if (rtQty <= 0)
                {
                    continue;
                }
                Rate = Convert.ToInt32(r.Cells["colRate"].Value);
                ItemTotal = Convert.ToDouble(r.Cells["colAmount"].Value);
                ItemReturnTotal = rtQty * Rate;
                ItemReturnGrandTotal += ItemReturnTotal;
                DiscAmt = 0;
                if (IsDiscountByItem)
                {
                    int DiscountType = Convert.ToInt32(r.Cells["colDiscountTypeId"].Value);
                    DiscAmt = Convert.ToDouble(r.Cells["colDiscount"].Value);
                    if (DiscountType == 2)
                    {
                        DiscAmt = (DiscAmt / 100) * ItemTotal;
                    }
                    DiscAmt = (DiscAmt / SaleQty) * rtQty;
                }
                else
                {
                    DiscAmt = GetGrandDiscount();
                    double Ratio = ItemTotal / SubTotal;
                    DiscAmt = ((DiscAmt * Ratio) / SaleQty) * rtQty;
                }
                double RefundAmount = ItemReturnTotal - DiscAmt;
                totalRefundAmount += RefundAmount;
            }
            double GDisc = 0;
            GDisc = GetGrandDiscount();
            GrandTotal = SubTotal - GDisc - totalRefundAmount;
            lblGrandTotal.Text = Math.Round(GrandTotal, 2).ToString();
            double CurrentRefund = totalRefundAmount - InitialTotalRefundValue;
            txtCurrentRefund.Text = Math.Round(CurrentRefund, 2, MidpointRounding.AwayFromZero).ToString();
            txtRefundAmount.Text = Math.Round(totalRefundAmount, 2, MidpointRounding.AwayFromZero).ToString();
            double due = GrandTotal - GetPaymentsSum();

            if (due < 0)
            {
                lblBalance.Text = Math.Round((-1 * due), 2, MidpointRounding.AwayFromZero).ToString();
                lblDue.Text = "0";
            }
            else
            {
                lblBalance.Text = "0";
                lblAdvance.Text = "0";
                lblDue.Text = Math.Round(due, 2, MidpointRounding.AwayFromZero).ToString();
            }
            if (double.Parse(lblBalance.Text) > 0)
            {
                btnAddtoAdv.Enabled = true;
            }
            else
            {
                btnAddtoAdv.Enabled = false;
            }
        }
        private void CalculateRefundAmount2()
        {
            try
            {
                //_WOD : Without Deductions
                double GrandTotal_WOD = double.Parse(lblSubTotal.Text.ToString()) - GetGrandDiscount();

                double TotalPaid = GetPaymentsSum();
                double totalRefundAmount = 0;
                double totalRefundAmountWP = 0;
                double GrandTotal = double.Parse(lblGrandTotal.Text);
                double SubTotal = double.Parse(lblSubTotal.Text);

                double totalCost = 0;
                double RetQtydiscount = 0;

                double Rate = 0;
                double Cost = 0;
                double ItemTotal = 0;
                double ItemReturnTotal = 0;
                double ItemReturnTotalWP = 0; // WP : for when customer has paid some amount prior to returning
                double ItemReturnGrandTotalWP = 0; // WP : for when customer has paid some amount prior to returning
                double ItemReturnGrandTotal = 0;
                double DiscAmt = 0;
                foreach (DataGridViewRow r in grdItems.Rows)
                {
                    double rtQty = 0; double.TryParse(r.Cells["colReturnQuantity"].Value.ToString(), out rtQty);
                    double SaleQty = 0; double.TryParse(r.Cells["colQuantity"].Value.ToString(), out SaleQty);
                    if (rtQty <= 0)
                    {
                        continue;
                    }

                    Rate = Convert.ToDouble(r.Cells["colRate"].Value);
                    Cost = Convert.ToDouble(r.Cells["colCostPrice"].Value);
                    ItemTotal = Convert.ToDouble(r.Cells["colAmount"].Value);

                    ItemReturnTotal = rtQty * Rate;
                    ItemReturnGrandTotal += ItemReturnTotal;
                    DiscAmt = 0;
                    if (IsDiscountByItem)
                    {

                        string DiscountType = r.Cells["colDiscountType"].Value.ToString();
                        DiscAmt = Convert.ToDouble(r.Cells["colDiscount"].Value);
                        double NetAmount = Convert.ToDouble(r.Cells["colNetAmount"].Value);
                        if (DiscountType == "%")
                        {
                            DiscAmt = (DiscAmt / 100) * ItemTotal;
                        }
                        if (TotalPaid > 0) // only show refund if customer have paid some amount
                        {
                            // 4 rows commecnted at 2020-07-17 1626
                            //double Ratio = ((ItemTotal - DiscAmt) / GrandTotal_WOD);
                            //double Share = (Ratio * TotalPaid) / SaleQty;
                            //ItemReturnTotalWP = Share * rtQty;
                            //ItemReturnGrandTotalWP += ItemReturnTotalWP;
                            // 2 rows added at 2020-07-17 1626          
                            double Ratio = ((NetAmount) / GrandTotal_WOD);
                            double Share = (Ratio * TotalPaid) / SaleQty;
                            ItemReturnTotalWP = Share * rtQty;
                            ItemReturnGrandTotalWP += ItemReturnTotalWP;
                        }
                        DiscAmt = (DiscAmt / SaleQty) * rtQty;
                    }
                    else
                    {
                        DiscAmt = GetGrandDiscount();
                        double Ratio = 0;
                        if (SubTotal != 0)
                        {
                            Ratio = ItemTotal / SubTotal;
                        }
                        double DiscByLineItem = (DiscAmt * Ratio);
                        DiscAmt = ((DiscAmt * Ratio) / SaleQty) * rtQty;
                        if (TotalPaid > 0) // only show refund if customer have paid some amount
                        {
                            double Ratio2 = ((ItemTotal - DiscByLineItem) / GrandTotal_WOD);
                            double Share = (Ratio2 * TotalPaid) / SaleQty;
                            ItemReturnTotalWP = Share * rtQty;
                            ItemReturnGrandTotalWP += ItemReturnTotalWP;
                        }
                    }
                    //double RefundAmountWP = ItemReturnTotalWP + DiscAmt;
                    double RefundAmountWP = ItemReturnTotalWP;
                    double RefundAmount = ItemReturnTotal - DiscAmt;
                    totalRefundAmount += RefundAmount;
                    totalRefundAmountWP += RefundAmountWP;
                    RetQtydiscount += DiscAmt; // this value contains value for discount share for returned item quantity.
                    totalCost += ((SaleQty * Cost) - (rtQty * Cost));
                }
                double GDisc = 0;
                GDisc = GetGrandDiscount();
                GrandTotal = SubTotal - GDisc - totalRefundAmount;
                lblGrandTotal.Text = Math.Round(GrandTotal, 2, MidpointRounding.AwayFromZero).ToString();
                double CurrentRefund = totalRefundAmountWP - InitialTotalRefundValue;
                txtCurrentRefund.Text = Math.Round(CurrentRefund, 2, MidpointRounding.AwayFromZero).ToString();
                txtRefundAmount.Text = Math.Round(totalRefundAmountWP, 2, MidpointRounding.AwayFromZero).ToString();
                lblNetProfit.Text = Math.Round((GrandTotal - totalCost), 2).ToString();
                //dont calculate due in case of refund
                //double due = GrandTotal - GetPaymentsSum();

                numModifiedDiscount.Value = (decimal)(GDisc - RetQtydiscount);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while performing refund calculations, please try again.", "Calculation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private double GetPaymentsSum()
        {
            double TotalPaid = 0;
            double pmt = 0; double.TryParse(txtPayment.Text, out pmt);
            TotalPaid = pmt;//+ numPmnt2.Value + numPmnt3.Value + numPmnt4.Value + numPmnt5.Value);
            return TotalPaid;
        }
        private double GetGrandDiscount()
        {
            double DiscAmt = 0;
            double SubTotal = double.Parse(lblSubTotal.Text);
            int DiscountType = cmbTotalDiscountType.SelectedIndex + 1;
            //DiscAmt = (double)numTotalDiscount.Value;
            double.TryParse(txtDiscount.Text, out DiscAmt);
            if (DiscountType == 2)
            {
                DiscAmt = (DiscAmt / 100) * SubTotal;
            }
            return DiscAmt;
        }
        private void grdItems_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colReturnQuantity"].Index)
            {
                e.Control.KeyPress += new KeyPressEventHandler(SharedFunctions.isValidNumericKey);
            }
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index)
            {
                e.Control.KeyPress += new KeyPressEventHandler(SharedFunctions.isValidNumericKey);
            }
            //ComboBox cmb = e.Control as ComboBox;
        }
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
        }

        private void numPmnt5_ValueChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            GetCustomerTotals();
        }
        private void btnSaveAndPrint_Click(object sender, EventArgs e)
        {
            //fbrResponse = new FBR_Response();
            //this.ActionTime = SharedFunctions.GetDateAccordingToDayCloseSetting();
            //long invoiceid = SaveInvoice(false, false, 0);
            //if (invoiceid > 0)
            //{
            //    //Reports.Pharmacy.PatientInvoiceViewer v = new Reports.Pharmacy.PatientInvoiceViewer(invoiceid);
            //    Reports.POSInvoices.POSInvoiceViewer v = new Reports.POSInvoices.POSInvoiceViewer(invoiceid, true);
            //    v.Print();
            //    //v.Show();
            //}
        }
        private void cmbSelectItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }
        private void lblDue_TextChanged(object sender, EventArgs e)
        {
            //if(double.Parse(lblDue.Text) == 0)
            //{
            //    pnlDue.Visible = false;
            //}
            //else
            //{
            //    pnlDue.Visible = true;
            //}
        }

        private void btnAddNewItem_Click(object sender, EventArgs e)
        {
            if (!SharedFunctions.IsActionallowed("Add Item") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Form f = SharedFunctions.OpenForm(new frmItems(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void btnAddStock_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new btn(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void numQuantity_Enter(object sender, EventArgs e)
        {
            numQuantity.Select(0, numQuantity.Value.ToString().Length);
        }

        private void numRate_Enter(object sender, EventArgs e)
        {
            numRate.Select(0, numRate.Value.ToString().Length);
        }

        private void numDiscount_Enter(object sender, EventArgs e)
        {
            numDiscount.Select(0, numDiscount.Value.ToString().Length);
        }

        private void pbInfo_Click(object sender, EventArgs e)
        {
            ShortCutDialogs.frmInvoiceShortCusts f = new ShortCutDialogs.frmInvoiceShortCusts();
            f.ShowDialog();
        }

        private void numTotalDiscount_Leave(object sender, EventArgs e)
        {
            // stop user from leaving if total discount amount goes greater than sub-total.
            //if (!isCorrectTotalDiscount())
            //{
            //    numTotalDiscount.Focus();
            //}
        }

        private void pnlLoadedInvoiceData_Paint(object sender, PaintEventArgs e)
        {
        }

        private void numRate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void numQuantity_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void numDiscount_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void cmbDiscountType_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void cmbSelectItems_TextUpdate(object sender, EventArgs e)
        {
            //PageNoProds = 1;
            //prodsTimer.Start();
            //ItemsGlobal = new List<Item>();
            //SearchItems();
        }

        #region search patients
        #region search patient phone
        private void txtSearchByPhone_TextChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            PatientSearchTimer.Stop();
            pageNoPatients = 1;
            PatientsGlobal = new List<PatientSearchVM>();
            grdSuggestion.Location = new Point(txtSearchByPhone.Location.X + 15, txtSearchByPhone.Location.Y + txtSearchByPhone.Height + 4);
            grdSuggestion.BringToFront();
            grdSuggestion.Rows.Clear();
            //if (txtSearchByPhone.Text != "") { txtSearchByPhone.TextChanged -= txtSearchByPhone_TextChanged; txtSearchByPhone.Text = ""; txtSearchByPhone.TextChanged += txtSearchByPhone_TextChanged; }
            IsNameSearching = false;
            IsPhoneSearching = true;
            string searchString = txtSearchByPhone.Text.ToLower();
            PatientSearchTimer.Start();
            searchPatient();
            ///////////////////

            //grdSuggestion.Location = new Point(txtSearchByPhone.Location.X + 15, txtSearchByPhone.Location.Y + txtSearchByPhone.Height + 4);
            //grdSuggestion.BringToFront();
            //IsPhoneSearching = true;
            //// if (txtSearchByName.Text != "") { txtSearchByName.TextChanged -= txtSearchByName_TextChanged; txtSearchByName.Text = ""; txtSearchByName.TextChanged += txtSearchByName_TextChanged; }
            //grdSuggestion.Rows.Clear();
            //string searchString = txtSearchByPhone.Text.ToLower();
            //if (searchString != "")
            //{
            //    IPagedList<PatientSearchVM> Patients;// new List<PatientSearchVM>();
            //    using (unitOfWork = new UnitOfWork())
            //    {
            //        Patients = unitOfWork.PatientRepository.SearchPatient(false, true, false, searchString, pageNoPatients);
            //    }
            //    foreach (PatientSearchVM p in Patients)
            //    {
            //        grdSuggestion.Rows.Add(p.PatientId, p.MrNo, p.PatientName, p.Phone);
            //    }
            //    if (Patients.Count > 0)
            //    {
            //        grdSuggestion.Rows[0].Selected = false;
            //        grdSuggestion.Visible = true;
            //    }
            //    else
            //    {
            //        grdSuggestion.Visible = false;
            //    }
            //}
            //else
            //{
            //    grdSuggestion.Visible = false;
            //}
        }
        private void txtSearchByPhone_Enter(object sender, EventArgs e)
        {
            grdSuggestion.Visible = false;
            grdSuggestion.BringToFront();
            grdSuggestion.Location = new Point(txtSearchByPhone.Location.X, txtSearchByPhone.Location.Y + txtSearchByPhone.Height);
        }
        private void txtSearchByPhone_Leave(object sender, EventArgs e)
        {
            //grdSuggestion.Visible = false;
        }
        private void txtSearchByPhone_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                ShiftCtrlToSuggGrid();
            }
            //if (e.KeyCode == Keys.Enter)
            //{
            //    grdSuggestion.Rows.Clear();
            //    string searchString = txtSearchByName.Text.ToLower();
            //    if (searchString != "")
            //    {
            //        List<PatientSuggVM> Patients = unitOfWork.PatientRepository.SearchPatient(false, true, false, searchString);
            //        foreach (PatientSuggVM p in Patients)
            //        {
            //            grdSuggestion.Rows.Add(p.PatientId, p.Result);
            //        }
            //        if (Patients.Count > 0)
            //        {
            //            grdSuggestion.Location = new Point(txtSearchByPhone.Location.X, grdSuggestion.Location.Y);
            //            grdSuggestion.Visible = true;
            //            grdSuggestion.Focus();
            //            grdSuggestion.Rows[0].Selected = true;
            //        }
            //    }
            //}
        }
        #endregion
        private void searchPatient()
        {
            string searchString = IsNameSearching ? txtSearchByName.Text.Trim().ToLower() : txtSearchByPhone.Text.Trim().ToLower();
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
                string address = "";
                txtSearchByPhone.TextChanged -= txtSearchByPhone_TextChanged;
                txtSearchByName.TextChanged -= txtSearchByName_TextChanged;
                this.PatientId = Convert.ToInt64(grdSuggestion.SelectedRows[0].Cells["colPatientId"].Value);
                this.txtSearchByName.Text = grdSuggestion.SelectedRows[0].Cells["colPatient"].Value.ToString();
                this.txtSearchByPhone.Text = grdSuggestion.SelectedRows[0].Cells["colPhone"].Value.ToString();
                using (unitOfWork = new UnitOfWork())
                {
                    address = unitOfWork.PatientRepository.GetById(this.PatientId).Address;
                }
                this.txtAddress.Text = address;
                grdSuggestion.Visible = false;
                txtSearchByPhone.TextChanged += txtSearchByPhone_TextChanged;
                txtSearchByName.TextChanged += txtSearchByName_TextChanged;
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
            grdSuggestion.Location = new Point(txtSearchByName.Location.X, txtSearchByPhone.Location.Y + txtSearchByName.Height);
        }
        private void txtSearchByName_Leave(object sender, EventArgs e)
        {
            //grdSuggestion.Visible = false;
        }
        private void txtSearchByName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                ShiftCtrlToSuggGrid();
            }
        }
        private void txtSearchByName_TextChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            if (txtSearchByName.Text.Trim() == "")
            {
                this.PatientId = 0;
                txtSearchByPhone.Text = "";
                txtAddress.Text = "";
                return;
            }
            PatientSearchTimer.Stop();
            pageNoPatients = 1;
            PatientsGlobal = new List<PatientSearchVM>();
            grdSuggestion.Location = new Point(txtSearchByName.Location.X + 15, txtSearchByName.Location.Y + txtSearchByName.Height + 4);
            grdSuggestion.BringToFront();
            grdSuggestion.Rows.Clear();
            IsNameSearching = true;
            IsPhoneSearching = false;
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
            else if (IsPhoneSearching) { TargetText = txtSearchByPhone; }
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
        #endregion search patients
        private void frmInvoice_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.PatientSearchTimer.Stop();
        }

        private void uC_SearchItems1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                cmbSelectBatch.Focus();
            }
            if (e.KeyCode == Keys.Enter)
            {
                btnAddItem.PerformClick();
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            getItemQuantityByBatches();
        }

        private void btnAddtoAdv_Click(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            lblAdvance.Text = lblBalance.Text;
            lblBalance.Text = "0";
            btnAddtoAdv.Enabled = false;
            btnAddToBal.Enabled = true;
            isBalanceActive = false;
        }

        private void btnAddToBal_Click(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            lblBalance.Text = lblAdvance.Text;
            lblAdvance.Text = "0";
            btnAddToBal.Enabled = false;
            btnAddtoAdv.Enabled = true;
            isBalanceActive = true;
        }

        private void cmbTemplates_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbTemplates.SelectedValue) > 0)
            {

            }
        }

        private void btnHold_Click(object sender, EventArgs e)
        {
            long invId = 0;
            if (this.HoldingInvoiceId > 0)
            {
                invId = SaveInvoice(false, true, this.HoldingInvoiceId);
            }
            else
            {
                invId = SaveInvoice(false, true, 0);
            }
            if (invId > 0)
            {
                this.InvHoldPageNo = 0;
                ShowHoldingInvoices(this.InvHoldPageNo);
            }
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            ClearAll(true);
            this.InvHoldPageNo = 1;
            ShowHoldingInvoices(this.InvHoldPageNo);
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            ClearAll(true);
            this.InvHoldPageNo -= 1;
            ShowHoldingInvoices(this.InvHoldPageNo);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            ClearAll(true);
            this.InvHoldPageNo += 1;
            ShowHoldingInvoices(this.InvHoldPageNo);
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            ClearAll(true);
            if (this.holdingInvoices == null)
            {
                int totalHold = 0;
                using (unitOfWork = new UnitOfWork())
                {
                    totalHold = unitOfWork.InvoiceRepository.GetHoldingInvoicesCount();
                }
                this.InvHoldPageNo = totalHold;
            }
            else
            {
                this.InvHoldPageNo = holdingInvoices.PageCount;
            }
            ShowHoldingInvoices(this.InvHoldPageNo);
        }

        private void btnCancelHold_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.HoldingInvoiceId > 0)
                {
                    using (unitOfWork = new UnitOfWork())
                    {
                        Invoice obj = unitOfWork.InvoiceRepository.DeleteHoldingInvoice(this.HoldingInvoiceId);
                        obj.IsActive = false;
                        //foreach (InvoiceItem i in obj.InvoiceItems)
                        //{
                        //    i.IsActive = false;
                        //    unitOfWork.GetDbContext().Entry(i).State = System.Data.Entity.EntityState.Modified;
                        //}
                        obj.StockConsumption.IsActive = false;
                        //{
                        //    si.IsActive = false;
                        //    unitOfWork.GetDbContext().Entry(si).State = System.Data.Entity.EntityState.Modified;
                        //}
                        foreach (InvoicePayment ip in obj.InvoicePayments)
                        {
                            ip.IsActive = false;
                            //unitOfWork.GetDbContext().Entry(ip).State = System.Data.Entity.EntityState.Modified;
                        }
                        unitOfWork.InvoiceRepository.Update(obj);
                        unitOfWork.Save();
                    }
                    MessageBox.Show("Invoice has been cancelled.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearAll(true);
                    this.InvHoldPageNo = 0;
                    ShowHoldingInvoices(this.InvHoldPageNo);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while cancelling holding invoice.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtAvlQty_TextChanged(object sender, EventArgs e)
        {

        }

        private void grdItems_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //if(e.Exception.Message == "DataGridViewComboBoxCell value is not valid")
            //{
            //    object value = grdItems.Rows[e.RowIndex].Cells["colCmbBatch"].Value;

            //}
        }

        private void grdItems_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colDiscountType"].Index
                || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colSalesTaxType"].Index
             || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colBatch"].Index
             || grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colUnit"].Index)
            {
                grdItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void btnNextInvoice_Click(object sender, EventArgs e)
        {
            pnlInvoiceEditControls.Enabled = false;
            int id = 0;
            int invoiceNo = 0;
            int.TryParse(lblInvoiceNo.Text, out invoiceNo);
            if (invoiceNo >= 0)
            {
                using (unitOfWork = new UnitOfWork())
                {
                    if (invoiceNo == 0)
                    {
                        id = unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(100001); // sttart from first invoice number.. 100001 is programmatically generated first invoice by default when first invoice is created..
                    }
                    else
                    {
                        id = unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(invoiceNo + 1);
                    }
                }
            }
            if (this.InvoiceEditId > 0)
            {
                if (isInvoiceUpdated)
                {
                    IsInvoiceEdit = true;
                    SaveInvoice(true, false, 0);
                }
            }
            ClearAll(true);
            if (id > 0)
            {
                this.InvoiceEditId = id;
                this.IsInvoiceEdit = true;
                LoadForm(true);
            }
            else
            {
                MessageBox.Show("No more invoices found..", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            pnlInvoiceEditControls.Enabled = true;
            btnPreviousInvoice.Enabled = true;
        }

        private void btnLastInvoice_Click(object sender, EventArgs e)
        {
            pnlInvoiceEditControls.Enabled = false;
            int id = 0;
            using (unitOfWork = new UnitOfWork())
            {
                id = unitOfWork.InvoiceRepository.getMaxInvoiceId();
            }
            if (this.InvoiceEditId > 0)
            {
                if (isInvoiceUpdated)
                {
                    IsInvoiceEdit = true;
                    SaveInvoice(true, false, 0);
                }
            }
            ClearAll(true);

            if (id > 0)
            {
                this.InvoiceEditId = id;
                this.IsInvoiceEdit = true;
                LoadForm(true);
            }
            else
            {
                MessageBox.Show("No more invoices found..", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            pnlInvoiceEditControls.Enabled = true;
            btnNextInvoice.Enabled = false;
            btnPreviousInvoice.Enabled = true;
        }

        private void brnPreviousInvoice_Click(object sender, EventArgs e)
        {
            pnlInvoiceEditControls.Enabled = false;
            int id = 0;
            int invoiceNo = 0;
            int.TryParse(lblInvoiceNo.Text, out invoiceNo);
            if (invoiceNo > 0)
            {
                using (unitOfWork = new UnitOfWork())
                {
                    id = unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(invoiceNo - 1);
                }
            }
            if (this.InvoiceEditId > 0)
            {
                if (this.isInvoiceUpdated)
                {
                    IsInvoiceEdit = true;
                    SaveInvoice(true, false, 0);
                }
            }
            ClearAll(true);
            if (id > 0)
            {
                this.InvoiceEditId = id;
                this.IsInvoiceEdit = true;
                LoadForm(true);
            }
            else
            {
                MessageBox.Show("No more invoices found..", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            pnlInvoiceEditControls.Enabled = true;
            btnNextInvoice.Enabled = true;
        }

        private void btn1stInvoice_Click(object sender, EventArgs e)
        {
            pnlInvoiceEditControls.Enabled = false;

            int id = 0;
            using (unitOfWork = new UnitOfWork())
            {
                id = unitOfWork.InvoiceRepository.getInvoiceIdByRefNo(100001);
            }
            if (this.InvoiceEditId > 0)
            {
                if (isInvoiceUpdated)
                {
                    IsInvoiceEdit = true;
                    SaveInvoice(true, false, 0);
                }
            }
            ClearAll(true);

            if (id > 0)
            {
                this.InvoiceEditId = id;
                this.IsInvoiceEdit = true;
                LoadForm(true);
            }
            else
            {
                MessageBox.Show("No more invoices found..", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            pnlInvoiceEditControls.Enabled = true;
            btnPreviousInvoice.Enabled = false;
            btnNext.Enabled = true;
        }

        private void btnSearchItems_Click(object sender, EventArgs e)
        {
            frmSearchItem f = new frmSearchItem();
            f.OnItemSelected += frmSearchItem_OnItemSelected;
            f.ShowDialog();
        }
        private void frmSearchItem_OnItemSelected(int ItemId, int Quantity)
        {
            if (ItemAlreadyAdded(ItemId))
            {
                MessageBox.Show("Selected Item Has Already been Added", "Already Added", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            this.isNewItemAdded = true;
            grdItems.Rows.Add(
                0,//invoiceitemid
                ItemId, //ItemId
                "", //itemname
                "", //batch
                0, //batchid
                "", //rackno
                "", //unit
                1, //conversion unit
                0, //unitcost
                0, //rate
                0, //available quantity
                Quantity,  //quantity
                0, //Bonus Quantity
                0, //amount
                0, //net amount
                0, //discount
                "%",  // discount type
                0, // sales tax,
                "%", // sales tax type
                true,  //isnew row
                0, //return
                0,//initial return
                false, // affect stock
                false // isoptionalbatchno
                );
            btnClear.PerformClick();
            //MessageBox.Show("item loaded : " + ItemId);
        }

        private void dtpPmntType1_ValueChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true; // will be used only case of when using NEXT and PREVIOUS buttons while moving between invoices.
        }

        private void dtpPmntType2_ValueChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true; // will be used only case of when using NEXT and PREVIOUS buttons while moving between invoices.
        }

        private void dtpPmntType3_ValueChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true; // will be used only case of when using NEXT and PREVIOUS buttons while moving between invoices.
        }

        private void dtpPmntType4_ValueChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true; // will be used only case of when using NEXT and PREVIOUS buttons while moving between invoices.
        }

        private void dtpPmntType5_ValueChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true; // will be used only case of when using NEXT and PREVIOUS buttons while moving between invoices.
        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
        }

        private void txtInvoiceNote_TextChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
        }

        private void grdItems_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index)
                {
                    uC_SearchItems1.SetFocus();
                }
            }
        }

        private void grdItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (grdItems.CurrentCell.ColumnIndex == grdItems.Columns["colQuantity"].Index)
                {
                    uC_SearchItems1.SetFocus();
                }
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                using (unitOfWork = new UnitOfWork())
                {
                    //this.SelectedItemId = (int)unitOfWork.ItemRspository.getItemIdByBarcode(txtBarcode.Text.Trim().ToLower());
                    //if (this.SelectedItemId <= 0)
                    //{
                    //    if (txtBarcode.Text.Trim().Length >= 12)
                    //    {
                    //        var weight = txtBarcode.Text.Trim().Substring(7, 5);
                    //        double wt = 0; double.TryParse(weight, out wt);
                    //        wt = wt / 1000;
                    //        var barcode = txtBarcode.Text.Trim().Substring(2, 5);
                    //        this.SelectedItemId = (int)unitOfWork.ItemRspository.getItemIdByBarcode(barcode.ToLower());
                    //        if (this.SelectedItemId > 0)
                    //        {
                    //            this.scannedItemQuantity = wt;
                    //            btnAddItem.PerformClick();
                    //            this.SelectedItemId = 0;
                    //            this.isNewItemAdded = false;
                    //        }
                    //        else
                    //        {
                    //            MessageBox.Show("No item found with given barcode.", "Item Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    btnAddItem.PerformClick();
                    //    this.SelectedItemId = 0;
                    //    this.isNewItemAdded = false;
                    //}
                }
            }
        }

        private void grdItems_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //resetSearchFocus();
        }

        private void chkBarcodePref_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBarcodePref.Checked)
            {
                chkSearchItemPref.Checked = false;
            }
            //Properties.Settings.Default.UseBarcodeSearch = chkBarcodePref.Checked;
            //Properties.Settings.Default.Save();
            resetSearchFocus();
        }

        private void chkSearchItemPref_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSearchItemPref.Checked)
            {
                chkBarcodePref.Checked = false;
            }
            //Properties.Settings.Default.UseBarcodeSearch = chkBarcodePref.Checked;
            //Properties.Settings.Default.Save();
            resetSearchFocus();
        }

        private void txtPayment_TextChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            GetCustomerTotals();
            double pmt = 0; double.TryParse(txtPayment.Text, out pmt);
            if (pmt <= 0)
            {
                txtPayment.TextChanged -= txtPayment_TextChanged;
                txtPayment.Text = "";
                txtPayment.TextChanged += txtPayment_TextChanged;
            }
        }

        private void txtPayment_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            isInvoiceUpdated = true;
            //numTotalDiscount.Value = Math.Round(numTotalDiscount.Value, 2, MidpointRounding.AwayFromZero);
            double disc = 0; double.TryParse(txtDiscount.Text, out disc);
            txtDiscount.TextChanged -= txtDiscount_TextChanged;
            txtDiscount.Text = Math.Round(disc, 2, MidpointRounding.AwayFromZero).ToString();
            if (disc <= 0)
            {
                txtDiscount.TextChanged -= txtDiscount_TextChanged;
                txtDiscount.Text = "";
            }
            txtDiscount.TextChanged += txtDiscount_TextChanged;
            if (grdItems.Rows.Count <= 0)
            { return; }

            double SubTotal = 0;
            double.TryParse(lblSubTotal.Text.Trim(), out SubTotal);

            double AllowedDiscount = this.getUserAllowedDiscount(SubTotal);

            if (disc > AllowedDiscount)
            {
                if (cmbTotalDiscountType.SelectedIndex == 1)
                {
                    MessageBox.Show("Invalid Discount!" + Environment.NewLine + "You can give max " + SharedVariables.LoggedInUser.DiscLimit + " % discount.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Invalid Discount!" + Environment.NewLine + "You can give max " + AllowedDiscount + " rupees discount.", "Invalid Discount", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                //numTotalDiscount.Value = 0;
                txtDiscount.Text = "";
                return;
            }
            else
            {
                GetTotalsByTotalDiscountChanges();
            }

            //if (isCorrectTotalDiscount())
            //{
            //    //if (numTotalDiscount.Enabled)
            //    //{
            //    //}
            //    GetTotalsByTotalDiscountChanges();
            //}

        }

        private void txtDiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidNumericKey(sender, e);
        }

        private void frmInvoice_Activated(object sender, EventArgs e)
        {
        }

        private void btnUnPaidInvoices_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmUnpaidInvoices(), this.MdiParent);
        }

        private FBR_Response Generate_FBR_InvoiceNumber(Invoice objInvoice)
        {
            FBR_Invoice objInv = new FBR_Invoice();

            objInv.InvoiceNumber = objInvoice.InvoiceRefNo.ToString();
            objInv.POSID = SharedVariables.AdminPractiseSetting.FBR_POSID;
            objInv.USIN = "111111";

            objInv.DateTime = objInvoice.CreatedAt;

            objInv.BuyerNTN = null;

            objInv.BuyerCNIC = null;

            objInv.BuyerName = null;

            objInv.BuyerPhoneNumber = null;

            objInv.PaymentMode = 1;

            objInv.TotalSaleValue = objInvoice.SubTotal;

            //objInv.TotalQuantity = objInvoice.InvoiceItems.Sum(i => i.Quantity);

            objInv.TotalBillAmount = 0;

            objInv.TotalTaxCharged = objInvoice.TotalSalesTax;

            objInv.Discount = objInvoice.TotalDiscount;

            objInv.FurtherTax = 0;

            objInv.InvoiceType = 1;

            objInv.Items = new List<FBR_Invoice_Items>();
            //foreach (var i in objInvoice.InvoiceItems)
            //{


            //    FBR_Invoice_Items objItem = new FBR_Invoice_Items();
            //    objItem.ItemCode = i.ItemId.ToString();

            //    objItem.ItemName = i.ItemName;

            //    objItem.Quantity = i.Quantity;

            //    objItem.TotalAmount = i.CalculatedNetAmount;

            //    objItem.SaleValue = i.Rate;

            //    objItem.TaxCharged = i.SalesTax == null ? 0 : i.SalesTax.Value;

            //    objItem.TaxRate = 6;

            //    objItem.PCTCode = getRandom_8_digit_number().ToString();

            //    objItem.FurtherTax = 0;

            //    objItem.InvoiceType = 1;

            //    objItem.Discount = i.Discount;
            //    objInv.Items.Add(objItem);
            //}
            HttpClient Client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(objInv), Encoding.UTF8, "application/json");
            HttpResponseMessage response = Client.PostAsync("http://localhost:8524/api/IMSFiscal/GetInvoiceNumberByModel", content).Result;

            FBR_Response resp = new FBR_Response();
            if (response.IsSuccessStatusCode)
            {
                var json = response.Content.ReadAsStringAsync();
                resp = JsonConvert.DeserializeObject<FBR_Response>(json.Result);
            }
            return resp;
        }

        private int getRandom_8_digit_number()
        {
            Random rnd = new Random();
            int myRandomNo = rnd.Next(10000000, 99999999);
            return myRandomNo;
        }

        private bool IsPaymentDone()
        {
            if (cmbPaymentStatus.SelectedIndex == 0)
            {
                double paid = 0;
                double.TryParse(txtPayment.Text, out paid);
                if (paid <= 0)
                {
                    MessageBox.Show("Please add some payment before changing status to paid", "Payment Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            return true;
        }

        private void btnLoadPendingOrders_Click(object sender, EventArgs e)
        {
            frmPendingOrders f = new frmPendingOrders();
            f.MdiParent = this.MdiParent;
            f.Show();
        }

        private void cmbOrderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblTableNo.Visible = cmbOrderType.SelectedIndex == 0;
            cmbTableNo.Visible = cmbOrderType.SelectedIndex == 0;
        }

        private void cmbActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbActions.SelectedIndex == 0)
            {
                SharedFunctions.OpenForm(new frmPendingOrders(), this.MdiParent);
            }
            else if (cmbActions.SelectedIndex == 1)
            {
                SharedFunctions.OpenForm(new frmInvoiceHistory(), this.MdiParent);
            }
            else if (cmbActions.SelectedIndex == 2)
            {
                SharedFunctions.OpenForm(new frmItems(), this.MdiParent);
            }
        }

        private void btnViewPendingOrders_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmPendingOrders(), this.MdiParent);
        }

        private void btnViewInvoiceHistory_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmInvoiceHistory(), this.MdiParent);
        }
    }
}