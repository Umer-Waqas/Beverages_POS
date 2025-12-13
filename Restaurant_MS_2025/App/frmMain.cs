
using Restaurant_MS_UI.App.MenuBar.Preferences;
using Restaurant_MS_UI.App.MenuBar.ShopActivities;
using Restaurant_MS_UI.App.MenuBar.UserMenue;
using Restaurant_MS_UI.Menu.Help;
using System.Timers;
using Utilities;

namespace Restaurant_MS_UI.App
{
    public partial class frmMain : Form
    {
        BackgroundWorker bgW_LowStockItems = new BackgroundWorker();

        private bool isMenuCpsd = false;
        System.Timers.Timer PnlAnimTimer = new System.Timers.Timer();
        System.Timers.Timer CheckLogout = new System.Timers.Timer();
        ToolStripMenuItem clickedItem;
        UnitOfWork unitOfWork;
        System.Timers.Timer EmailRetryTimer = new System.Timers.Timer();

        enum CurrentNotitype { LowStock = 1, Expiry = 2 };
        CurrentNotitype currNotiType = CurrentNotitype.LowStock;
        //System.Timers.Timer tCheckFormsCollection = new System.Timers.Timer();
        int PnlMenuWidth = 0;
        private bool isMenuPanelVisible = true;
        public frmMain()
        {
            InitializeComponent();
            this.PnlMenuWidth = pnlMenu.Width;
        }
        private void SetactiveItem(object sender)
        {
            clickedItem = (ToolStripMenuItem)sender;
            SetToolStripItems(menuStrip1.Items);
        }
        private void SetToolStripItems(ToolStripItemCollection dropDownItems)
        {
            try
            {
                foreach (object obj in dropDownItems)
                //for each object.
                {
                    ToolStripMenuItem subMenu = obj as ToolStripMenuItem;
                    //Try cast to ToolStripMenuItem as it could be toolstrip separator as well.

                    if (subMenu != null)
                    //if we get the desired object type.
                    {
                        if (subMenu.HasDropDownItems) // if subMenu has children
                        {
                            SetToolStripItems(subMenu.DropDownItems); // Call recursive Method.
                        }
                        else // Do the desired operations here.
                        {

                            //subMenu.Visible = UserRights.Where(x =>
                            //x.FormID == Convert.ToInt32(subMenu.Tag)).First().CanAccess;
                            if (subMenu.Text == clickedItem.Text)
                            {
                                subMenu.BackColor = SharedVariables.MenuSelectionBackColor;
                                subMenu.ForeColor = SharedVariables.MenuSelectionForeColor;
                            }
                            else
                            {
                                subMenu.BackColor = SystemColors.Control;
                                subMenu.ForeColor = Color.Black;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SetToolStripItems",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void iTemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmAllItems(), this);
        }
        private void addStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new btn(), this);
        }
        private void addSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SharedFunctions.OpenForm(new frmAddSupplier(), this);
        }
        private void addMultipleItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SharedFunctions.OpenForm(new frmMultipleItems(), this);
        }
        private void addPurchaseOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmPurchaseOrder(), this);
        }
        private void purchaseOrdersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            if (SharedFunctions.CheckDayClosed()) { return; }
            SharedFunctions.OpenForm(new frmPurchaseOrders(), this);
        }
        private void allItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAllItems(), this);
        }
        private void addPatientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAddPatient(), this);
        }
        private void consumeStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SetactiveItem(sender);
            if (SharedFunctions.CheckDayClosed()) { return; }
            SharedFunctions.OpenForm(new frmConsumeStocks(), this);
        }
        private void pateintsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmCustomers(), this);
        }
        private void invoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmInvoice(), this);
        }
        private void shortCutKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmShortCutKeys(), this);
        }
        private void addPurchaseOrderStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAddStock2(), this);
        }
        private void manageStocksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            if (SharedFunctions.CheckDayClosed()) { return; }
            SharedFunctions.OpenForm(new frmManageStocks(), this);
        }
        private void stockAdjustmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmStockAdjustments(), this);
        }
        private void stockAdjustmentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SetactiveItem(sender);
            if (SharedFunctions.CheckDayClosed()) { return; }
            SharedFunctions.OpenForm(new frmStockAdjustments(), this);
        }
        private void tCheckFormsCollection_Elapsed(object sender, ElapsedEventArgs e)
        {
            //FormCollection openFroms = Application.OpenForms;
            //int activeFormsCount = 0;
            //foreach (Form form in openFroms)
            //{
            //    if (Form.ActiveForm.Name == form.Name)
            //    {
            //        activeFormsCount += 1;
            //    }               
            //}

            //int count = Application.OpenForms.Count;
            //if (count == 2)
            //{
            //    this.Invoke(new Action(() =>
            //    {
            //        pnlDashBoard.BringToFront();
            //    }));
            //}
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

            loadMainForm();
        }


        private void loadMainForm()
        {
            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.Backgroundpath))
            {
                this.BackgroundImageLayout = ImageLayout.Center;
                try
                {
                    this.BackgroundImage = new Bitmap(SharedVariables.AdminPractiseSetting.Backgroundpath);
                }
                catch (Exception)
                {
                }
            }

            this.Text += SharedVariables.AdminPractiseSetting.Name;
            this.Text += !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.Phone) ? "(" + SharedVariables.AdminPractiseSetting.Phone + ")" : "";
            pnlNotifications.MouseWheel += new MouseEventHandler(pnlNotifications_MouseWheel);
            //key.SetValue("d1", DateTime.Now);

            // timer to auto show dashboard
            //System.Timers.Timer tCheckFormsCollection = new System.Timers.Timer();
            //tCheckFormsCollection.Interval = 2000;
            //tCheckFormsCollection.Enabled = true;
            //tCheckFormsCollection.Elapsed += new ElapsedEventHandler(tCheckFormsCollection_Elapsed);
            //tCheckFormsCollection.Start();
            //SharedVariables.SideBarWidth = panel1.Width;
            //SharedVariables.TopBarHeight = panel11.Height;
            //SharedVariables.TopMenueBarHeight = menuStrip1.Height;


            //pharmacyToolStripMenuItem.Visible = false;
            //patientsToolStripMenuItem.Visible = false;
            //pereferencesToolStripMenuItem.Visible = false;
            //reportsToolStripMenuItem.Visible = false;
            //userToolStripMenuItem.Visible = false;
            //securityToolStripMenuItem.Visible = false;
            ////windowToolStripMenuItem.Visible = false;
            //indoorManagementsToolStripMenuItem.Visible = false;
            //indoorPatientsToolStripMenuItem.Visible = false;

            this.WindowState = FormWindowState.Minimized;
            this.WindowState = FormWindowState.Maximized;
            this.BringToFront();
            lblAppStatus.Text = "Please Login to Continue...";
            //Controls.OfType<MdiClient>().FirstOrDefault().BackColor = Color.FromArgb(42, 196, 244);
            Controls.OfType<MdiClient>().FirstOrDefault().BackColor = Color.White;
            //SharedFunctions.SetApplicationStartupValues();
            DisableNotRequiredModules();
            LoginUser();
            if (SharedVariables.LoggedInUser == null)
            {
                menuStrip1.Enabled = false;
                return;
            }
            else
            {
                menuStrip1.Enabled = true;
            }
            // call this setting after "SharedVariables.AdminPharmacySetting". below line is dependent on its setting value
            using (unitOfWork = new UnitOfWork())
            {
                var res = unitOfWork.StoreClosingRepository.OpenStore();
                SharedVariables.IsPreviousDayClosed = (bool)res.IsPreviousDayClosed;
                SharedVariables.IsStoreClosed = (bool)!res.OpenSuccess;
                if (!res.IsPreviousDayClosed.Value)
                {
                    MessageBox.Show("Previous day is not closed.Please contact Administrator/Manager to close previous day.", "Day Close Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    SharedVariables.IsPreviousDayClosed = false;
                    goto end;
                }
                if (!res.IsOpenTimeReached.Value)
                {
                    MessageBox.Show("Store has been closed and Store open time is not yet reached. please wait till " + SharedVariables.AdminPharmacySetting.DayOpenTime.Value.ToString("hh:mm:ss tt"), "Day Close Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    goto end;
                }

                SharedVariables.IsStoreClosed = unitOfWork.StoreClosingRepository.IsStoreClosed();
            }
            if (!SharedVariables.IsStoreClosed)
            {
                SharedFunctions.OpenForm(new frmInvoice(), this);
            }
        //if (SharedVariables.LoggedInUser != null) // go further if user is logged in successfully
        //{
        //    CheckForLowStockMail();
        //}




            // register and run background workers
        //this.bgW_LowStockItems.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgW_LowStockItems_DoWork);
        //this.bgW_LowStockItems.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgW_LowStockItems_RWComp);
        //this.bgW_LowStockItems.RunWorkerAsync();
        end:
            int a = 0;
        }

        private void LogoutUser_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (SharedVariables.LoggedInUser.AdminShiftSettings != null && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin) && SharedVariables.LoggedInUser.AdminShiftSetting.EndTime.TimeOfDay <= DateTime.Now.TimeOfDay)
                {
                    MessageBox.Show("Your shift timing is over, system is going to logout.", "Shit Eneded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while checking user shift timings.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void LoadDefaultItems()
        {
            try
            {

                //bool proceedToLoadData = false;
                //using (unitOfWork = new UnitOfWork())
                //{
                //    if (unitOfWork.SupplierRepository.SuppliersExist() && unitOfWork.ItemRspository.ItemsCount() > 1)
                //    {
                //        MessageBox.Show("Data already loaded", "Data exists", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        return;
                //    }
                //    else if (!unitOfWork.SupplierRepository.SuppliersExist() || unitOfWork.ItemRspository.ItemsCount() == 1)
                //    {
                //        proceedToLoadData = true;
                //    }
                //}
                //if (proceedToLoadData)
                //{
                //    string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Inventory-list.xls");
                //    DataTable dt = ReadExcel(path, ".xls");
                //    loadDefaultItemsData_FromExcelSheet(dt);
                //    //loadInventoryData_FromExcelSheet(dt);
                //}
                using (unitOfWork = new UnitOfWork())
                {
                    bool exists = unitOfWork.ItemRspository.ExistsAny();
                    bool removed = unitOfWork.AdminPractiseSettingRepository.IsUserDeletedDefaultData();
                    if (exists || removed)
                    {
                        return;
                    }
                }
                frmWaiting f = new frmWaiting();
                f.StartPosition = FormStartPosition.CenterParent;
                f.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DisableNotRequiredModules()
        {
            Disable_IndoorModule();
            Disable_ProcedureInvoice();
            DisableNotRquiredItems_Pharmacy();
        }
        private void DisableNotRquiredItems_Pharmacy()
        {
            financialReportsToolStripMenuItem.Visible = false;
        }
        private void Disable_ProcedureInvoice()
        {
            //proceduresListToolStripMenuItem.Visible = false;
            //procedureInvoiceToolStripMenuItem.Visible = false;
        }
        private void Disable_IndoorModule()
        {
            //indoorManagementsToolStripMenuItem.Visible = false;
            //indoorPatientsToolStripMenuItem.Visible = false;
            //pereferencesToolStripMenuItem.Visible = false;
        }
        private void CheckForLowStockMail()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    //DateTime LastSent = DateTime.Now.AddDays(-16);
                    DateTime LastSent = unitOfWork.LowStockMailRepository.GetLastMailSentDate();
                    int daysDiff = (DateTime.Now.Date - LastSent.Date).Days;
                    if (daysDiff > 15)
                    {
                        //Thread worker = new Thread(() => { sendAsyncronousEmails(); });
                        //worker.Name = "LowStockMailSender";
                        //worker.IsBackground = true;
                        //worker.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("System failed to send low stock mail to practise." + Environment.NewLine + "Please check your internet connection." + Environment.NewLine + " Sytem will try after " + SharedVariables.LowStockMailRetryInterval + " minutes.", "Failed to send low stock mail.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //EmailRetryTimer.Interval = SharedVariables.LowStockMailRetryInterval * 60 * 1000;
                //EmailRetryTimer.Enabled = true;
                //EmailRetryTimer.Elapsed += new System.Timers.ElapsedEventHandler(EmailRetryTimer_Elapsed);
                //EmailRetryTimer.AutoReset = true;
                //EmailRetryTimer.Start();
            }
        }
        //private void sendAsyncronousEmails()
        //{
        //    try
        //    {
        //        if (!SharedFunctions.IsInternetAvailable())
        //        {
        //            throw new Exception("Intenet un-available.");
        //        }
        //        Reports.Pharmacy.LowStockUI f = new Reports.Pharmacy.LowStockUI();
        //        string filePath = f.GetLowStockExcelSheet();
        //        List<string> AdminUsers = new List<string>();
        //        using (unitOfWork = new UnitOfWork())
        //        {
        //            // AdminUsers = unitOfWork.UserRepository.GetAdminUsersEmails();
        //        }
        //        //SharedFunctions.Email(filePath , new List<string> { "faani1786@gmail.com", "umar.waqas.se@gmail.com" });
        //        //SharedFunctions.Email("Low Stock", "email from umar", new[] { filePath }, "umar.getReports@gmail.com", "Alikes1425",AdminUsers);
        //        using (unitOfWork = new UnitOfWork())
        //        {
        //            unitOfWork.LowStockMailRepository.AddLastMailSentDate(new LowStockMail { LastSent = DateTime.Now, CreatedAt = DateTime.Now });
        //            unitOfWork.Save();
        //        }
        //        if (EmailRetryTimer.Enabled)
        //        {
        //            EmailRetryTimer.Enabled = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.BeginInvoke(new Action(() => { MessageBox.Show("An error occurred while generating low stock mail to admins, please check your internet connection.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Information); }));
        //    }
        //}
        //private void EmailRetryTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    CheckForLowStockMail();
        //}
        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData)
        {
            //#region short cuts for main screens
            //if (keyData == (Keys.Control | Keys.A))
            //{
            //    SharedFunctions.OpenForm(new frmAllItems(), this);
            //}
            //if (keyData == (Keys.Control | Keys.Alt | Keys.A))
            //{
            //    SharedFunctions.OpenForm(new frmMultipleItems(), this);
            //}
            //if (keyData == (Keys.Control | Keys.Shift | Keys.A))
            //{
            //    SharedFunctions.OpenForm(new frmItems(), this);
            //}

            //if (keyData == (Keys.Control | Keys.B))
            //{
            //    SharedFunctions.OpenForm(new frmManageStocks(), this);
            //}
            //if (keyData == (Keys.Control | Keys.Shift | Keys.B))
            //{
            //    SharedFunctions.OpenForm(new frmAddStock(), this);
            //}

            //if (keyData == (Keys.Control | Keys.C))
            //{
            //    SharedFunctions.OpenForm(new frmPurchaseOrders(), this);
            //}
            //if (keyData == (Keys.Control | Keys.Shift | Keys.C))
            //{
            //    SharedFunctions.OpenForm(new frmPurchaseOrder(), this);
            //}

            //if (keyData == (Keys.Control | Keys.D))
            //{
            //    SharedFunctions.OpenForm(new frmStockConsumptions(), this);
            //}
            //if (keyData == (Keys.Control | Keys.Shift | Keys.D))
            //{
            //    SharedFunctions.OpenForm(new frmStockConsumption(), this);
            //}

            //if (keyData == (Keys.Control | Keys.E))
            //{
            //    SharedFunctions.OpenForm(new frmStockAdjustments(), this);
            //}
            //if (keyData == (Keys.Control | Keys.Shift | Keys.E))
            //{
            //    SharedFunctions.OpenForm(new frmStockAdjustment(), this);
            //}

            //if (keyData == (Keys.Control | Keys.F))
            //{
            //    SharedFunctions.OpenForm(new frmSuppliers(), this);
            //}
            //if (keyData == (Keys.Control | Keys.Shift | Keys.F))
            //{
            //    SharedFunctions.OpenForm(new frmAddSupplier(), this);
            //}

            //if (keyData == (Keys.Control | Keys.O))
            //{
            //    frmInvoice f = new frmInvoice();
            //    f.MdiParent = this;
            //    f.Show();
            //}

            if (keyData == (Keys.Control | Keys.Shift | Keys.M))
            {
                MainMenue_Click(lblPharmacy, null);
            }

            //if (keyData == (Keys.Control | Keys.H))
            //{
            //    SharedFunctions.OpenForm(new frmDrugReturn(), this);
            //}
            //#endregion short cuts for main screens
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void ApplyUserRights()
        {
            //pharmacyToolStripMenuItem.Enabled = true;
            //patientsToolStripMenuItem.Enabled = true;
            //pereferencesToolStripMenuItem.Enabled = true;
            //reportsToolStripMenuItem.Enabled = true;
            //securityToolStripMenuItem.Enabled = true;
            lblPharmacy.Enabled = true;
            lblCustomers.Enabled = true;
            lblReports.Enabled = true;
            lblUsers.Enabled = true;
            lblPreferences.Enabled = true;
            btnDashboard.Enabled = true;
            lblshopactivities.Enabled = true;
            lblContactUs.Enabled = true;
            if (SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                usersToolStripMenuItem.Available = true;
                usersToolStripMenuItem.Visible = true;
            }
        }
        private void patientsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmCustomers(), this);
        }
        private void pOSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            if (SharedFunctions.CheckDayClosed()) { return; }
            frmInvoice f = new frmInvoice();
            f.MdiParent = this;
            f.Show();
        }
        private void drugReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmDrugReturn(), this);
        }
        private void stockSuppliersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SetactiveItem(sender);
            //SharedFunctions.OpenForm(new frmSuppliers(), this);
        }
        private void createUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAddUser(), this);
        }
        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmUsers(), this);
        }
        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            LoginUser();
        }
        private bool LoginUser()
        {
            frmLogin login = new frmLogin();
            login.BringToFront();
            login.StartPosition = FormStartPosition.CenterParent;
            login.ShowDialog();

            if (SharedVariables.LoggedInUser == null)
            {
                btnToggleLogin.Text = "Login";
                return false;
            }

            // start it once user is logged in.
            //CheckLogout.Interval = 1000 * 5; //seconds
            //CheckLogout.Enabled = true;
            //CheckLogout.AutoReset = true;
            //CheckLogout.Elapsed += new ElapsedEventHandler(LogoutUser_Elapsed);
            //CheckLogout.Start();

            LoadDefaultItems();
            lblAppStatus.Text = "Welcome :: " + SharedVariables.LoggedInUser.UserName + "@" + " [Session Started at " + DateTime.Now + "]";
            // lblUserName.Text = SharedVariables.LoggedInUser.UserName;
            //this.statusStrip.
            //loginToolStripMenuItem.Visible = false;
            //logoutToolStripMenuItem.Visible = true;
            //usersToolStripMenuItem.Visible = true;
            btnCreateInvoice.Enabled = true;
            btnInvoiceHistory.Enabled = true;
            btnPurchaseOrders.Enabled = true;
            btnItems.Enabled = true;
            if (SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                adminSettingToolStripMenuItem.Visible = true;
            }

            ApplyUserRights();
            //SharedFunctions.OpenForm(new frmInvoice(), this);
            btnToggleLogin.Text = "Logout";
            btnToggleLogin.Tag = "1";
            return true;
        }
        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            LogoutUser();
        }
        private void LogoutUser()
        {
            try
            {
                //Properties.Settings.Default.IsSavePassword = false;
                //Properties.Settings.Default.UserName = "";
                //Properties.Settings.Default.Password = "";
                Application.Restart();
            }
            catch (Exception ex)
            {

            }
        }
        private void serialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmShowSerialKey(), this);
        }
        private void pharmacyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!SharedFunctions.IsActionallowed("View Pharmacy Report") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.Description.Equals("Admin")))
            {
                MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            SetactiveItem(sender);
            //SharedFunctions.OpenForm(new Reports.Pharmacy.frmPharmacyReports(), this);
        }
        private void invoiceHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmInvoiceHistory(), this);
        }
        private void financialReportsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            if (!SharedFunctions.IsActionallowed("View Financial Reports") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
            {
                MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            //SharedFunctions.OpenForm(new Reports.Financial.frmFinancialReportsUI(), this);
        }

        private void accountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            //SharedFunctions.OpenForm(new Restaurant_MS_UI.App.Reports.Accounts.frmAccountsReportsUI(), this);
        }
        private void procedureInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void adminSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new AdminSetting(), this);
        }
        private void consumeStock2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            if (SharedFunctions.CheckDayClosed()) { return; }
            SharedFunctions.OpenForm(new frmStockConsumptions(), this);
        }
        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            LayoutMdi(MdiLayout.Cascade);
        }
        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            LayoutMdi(MdiLayout.TileVertical);
        }
        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
        private void ToggleStatusBarMenuStripItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            if (statusStrip.Visible == true)
            {
                statusStrip.Visible = false;
                ToggleStatusBarMenuStripItem.Text = "Show Status Bar";
            }
            else
            {
                statusStrip.Visible = true;
                ToggleStatusBarMenuStripItem.Text = "Hide Status Bar";
            }
        }
        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void loadDefaultDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void amountConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAmountConverter(), this);
        }
        private void admittedPAtientsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void bedAllocationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private int lastclicledIndex = -1;
        private void MainMenue_Click(object sender, EventArgs e)
        {
            if (this.isMenuCpsd)
            {
                this.toggleSideMennu(true);
                this.isMenuCpsd = false;
            }

            //if (sender is Label)
            //{
            //    string senderName = ((Label)sender).Name;
            //    if (senderName == lblPharmacy.Name)
            //    {
            //        sender = MPharmacy;

            //    }
            //    else if (senderName == lblPatients.Name)
            //    {
            //        sender = MPatients;
            //    }
            //    else if (senderName == lblStocks.Name)
            //    {
            //        sender = MStocks;
            //    }
            //    else if (senderName == lblStockAdjustments.Name)
            //    {
            //        sender = MStockAdjustments;
            //    }
            //    else if (senderName == lblPurchases.Name)
            //    {
            //        sender = MPurchases;
            //    }
            //    else if (senderName == lblUsers.Name)
            //    {
            //        sender = MUsers;
            //    }
            //    else if (senderName == lblReports.Name)
            //    {
            //        sender = MReports;
            //    }
            //}
            ShowSubCategories(sender);
        }
        private void ShowSubCategories(object sender)
        {
            //Panel p = (Panel)sender;
            Label p = (Label)sender;
            int Menuindex = pnlMenu.Controls.GetChildIndex(p);
            int subMenuIndex = pnlMenu.Controls.GetChildIndex(pnlSubMenu);
            //PopulateSubMenu(p.Name);
            if (Menuindex == lastclicledIndex)
            {
                pnlSubMenu.Visible = !pnlSubMenu.Visible;
            }
            else
            {
                int localIndex = Menuindex;
                if (subMenuIndex > Menuindex)
                {
                    localIndex += 1;
                }
                pnlSubMenu.Visible = true;
                pnlMenu.Controls.SetChildIndex(pnlSubMenu, localIndex);
            }
            if (Menuindex > 0)
            {
                if (subMenuIndex < Menuindex)
                {
                    lastclicledIndex = Menuindex - 1;
                }
                else
                {
                    lastclicledIndex = Menuindex;
                }
            }
            else
            {
                lastclicledIndex = Menuindex;
            }
            Label l = new Label();
            l.Name = "lblActiveSymb";
            l.Width = 6;
            l.Height = 35;
            l.BackColor = Color.FromArgb(0, 141, 76);
            foreach (Control mItem in pnlMenu.Controls)
            {
                if (mItem is FlowLayoutPanel)
                {
                    continue;
                }
                foreach (Control c in mItem.Controls)
                {
                    if (c is Label)
                    {
                        if (c.Name == l.Name)
                        {
                            mItem.Controls.Remove(c);
                        }
                    }
                }
                mItem.BackColor = Color.FromArgb(34, 45, 50);
            }
            p.BackColor = Color.FromArgb(30, 40, 44);
            p.Controls.Add(l);
            l.BringToFront();
        }
        private void setSubmenuStyle(Label lbl, string lblText)
        {
            lbl.Text = lblText;
            lbl.Tag = "level2";
            lbl.Width = 200;
            lbl.Height = 32;
            lbl.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            lbl.BackColor = Color.FromArgb(44, 59, 65);
            lbl.ForeColor = Color.White;
            lbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14, GraphicsUnit.Pixel);
            lbl.TextAlign = ContentAlignment.MiddleLeft;

            //lbl.Click += new EventHandler(subMenu_Clcik);
            lbl.MouseEnter += new EventHandler(subMenu_MouseEnter);
            lbl.MouseLeave += new EventHandler(subMenu_MouseLeave);
        }

        private void subMenu_MouseEnter(object sender, EventArgs e)
        {
            ToggleSubMenuHoverStyle(sender, true);
        }
        private void subMenu_MouseLeave(object sender, EventArgs e)
        {
            ToggleSubMenuHoverStyle(sender, false);
        }
        private void ToggleMainMenuHoverStyle(object sender, bool isMouseEnter)
        {
            Label l = (Label)sender;
            if (isMouseEnter)
            {
                l.Font = new Font("Microsoft Sans Serif", 18, GraphicsUnit.Pixel);
            }
            else
            {
                l.Font = new Font("Microsoft Sans Serif", 16, GraphicsUnit.Pixel);
            }
        }
        private void ToggleSubMenuHoverStyle(object sender, bool isMouseEnter)
        {
            Label l = (Label)sender;
            if (isMouseEnter)
            {
                l.Font = new Font("Microsoft Sans Serif", 14, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Pixel);
            }
            else
            {
                l.Font = new Font("Microsoft Sans Serif", 14, GraphicsUnit.Pixel);
            }
        }
        //private void subMenu_Clcik(object sender, EventArgs e)
        //{
        //    Label clickedOption = (Label)sender;
        //    switch (clickedOption.Name.ToLower())
        //    {
        //        //pharmacy sub menu
        //        case "lblitems": SharedFunctions.OpenForm(new frmAllItems(), this); break;
        //        case "lblmanagestock": SharedFunctions.OpenForm(new frmManageStocks(), this); break;
        //        case "lblpurchaseorders": SharedFunctions.OpenForm(new frmPurchaseOrders(), this); break;
        //        case "lblconsumestock": SharedFunctions.OpenForm(new frmStockConsumptions(), this); break;
        //        case "lblstockadjustment": SharedFunctions.OpenForm(new frmStockAdjustments(), this); break;
        //        case "lblstocksupplier": SharedFunctions.OpenForm(new frmSuppliers(), this); break;
        //        case "lblpos": SharedFunctions.OpenForm(new frmInvoice(), this); break;
        //        case "lblinvoicehistory": SharedFunctions.OpenForm(new frmInvoiceHistory(), this); break;
        //        case "lbldrugreturn": SharedFunctions.OpenForm(new frmDrugReturn(), this); break;
        //        case "lblmanufacturers": SharedFunctions.OpenForm(new frmManufacturers(), this); break;
        //        case "lbltemplates": SharedFunctions.OpenForm(new frmTemplates(), this); break;
        //        case "lblstockaudit": SharedFunctions.OpenForm(new frmStockAudits(), this); break;
        //        case "lblrackview": SharedFunctions.OpenForm(new frmRackView(), this); break;
        //        case "lblmissedsales": SharedFunctions.OpenForm(new frmMissedSales(), this); break;


        //        case "lblfinancial":
        //            if (!SharedFunctions.IsActionallowed("View Financial Reports") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
        //            {
        //                MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //                return;
        //            }
        //            //SharedFunctions.OpenForm(new Reports.Financial.frmFinancialReportsUI(), this);
        //            break;
        //        //case "lblaccounts": SharedFunctions.OpenForm(new Reports.Accounts.frmAccountsReportsUI(), this); break;
        //        case "lblpharmacy":
        //            if (!SharedFunctions.IsActionallowed("View Pharmacy Report") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
        //            {
        //                MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        //                return;
        //            }
        //            //SharedFunctions.OpenForm(new Reports.Pharmacy.frmPharmacyReports(), this);
        //            break;
        //        case "lblproductreports": SharedFunctions.OpenForm(new Reports.Product.frmProductReports(), this); break;


        //        //users sub menu
        //        case "lblusers": SharedFunctions.OpenForm(new frmUsers(), this); break;
        //        case "lbllogin": SharedFunctions.OpenForm(new frmLogin(), this); break;
        //        case "lbllogout": LogoutUser(); break;
        //        case "lbladminsetting": SharedFunctions.OpenForm(new AdminSetting(), this); break;
        //        //case "lblloaddefaultitems": LoadDefaultItems(); break;
        //        case "lblremovedefaultitems": removeDefaultItems(); break;
        //        // main menu
        //        case "lblpatients": SharedFunctions.OpenForm(new frmCustomers(), this); break;
        //        case "lbldiscounts": SharedFunctions.OpenForm(new Restaurant_MS_UI.App.MenuBar.Preferences.frmDiscounts(), this); break;

        //        // shop activities
        //        case "lblposclosing": SharedFunctions.OpenForm(new Restaurant_MS_UI.App.MenuBar.ShopActivities.frmPOSCosing(), this); break;
        //        case "lblstoreclosing": SharedFunctions.OpenForm(new Restaurant_MS_UI.App.MenuBar.ShopActivities.frmStoreClosing(), this); break;

        //    }
        //}

        //private void PopulateSubMenu(string MainMenu)
        //{
        //    pnlSubMenu.Controls.Clear();
        //    if (MainMenu.ToLower().Equals("lblpharmacy"))
        //    {
        //        Label l1 = new Label();
        //        l1.Name = "lblitems";
        //        this.setSubmenuStyle(l1, "Items");
        //        Label l2 = new Label();
        //        l2.Name = "lblmanagestock";
        //        this.setSubmenuStyle(l2, "Manage Stock");
        //        Label l3 = new Label();
        //        l3.Name = "lblpurchaseorders";
        //        this.setSubmenuStyle(l3, "Purchase Orders");
        //        Label l4 = new Label();
        //        l4.Name = "lblconsumestock";
        //        this.setSubmenuStyle(l4, "Consume Stock");
        //        Label l5 = new Label();
        //        l5.Name = "lblstockadjustment";
        //        this.setSubmenuStyle(l5, "Stock Adjustment");
        //        Label l6 = new Label();
        //        l6.Name = "lblstocksupplier";
        //        this.setSubmenuStyle(l6, "Stock Supplier");
        //        Label l7 = new Label();
        //        l7.Name = "lblpos";
        //        this.setSubmenuStyle(l7, "POS");
        //        Label l8 = new Label();
        //        l8.Name = "lblinvoicehistory";
        //        this.setSubmenuStyle(l8, "Invoice History");
        //        Label l9 = new Label();
        //        l9.Name = "lbldrugreturn";
        //        this.setSubmenuStyle(l9, "Drug Return");
        //        Label l10 = new Label();
        //        l10.Name = "lblmanufacturers";
        //        this.setSubmenuStyle(l10, "Manufacturers");
        //        Label l11 = new Label();
        //        l11.Name = "lbltemplates";
        //        this.setSubmenuStyle(l11, "Templates");
        //        Label l12 = new Label();
        //        l12.Name = "lblstockaudit";
        //        this.setSubmenuStyle(l12, "Stock Audit");
        //        Label l13 = new Label();
        //        l13.Name = "lblrackview";
        //        this.setSubmenuStyle(l13, "Rack View");
        //        Label l14 = new Label();
        //        l14.Name = "lblmissedsales";
        //        this.setSubmenuStyle(l14, "Missed Sales");
        //        pnlSubMenu.Controls.AddRange(new[] { l1, l2, l3, l4, l5, l6, l7, l8, l9, l10, l11, l12, l13, l14 });
        //    }
        //    else if (MainMenu.ToLower().Equals("lblcustomers"))
        //    {
        //        SharedFunctions.OpenForm(new frmCustomers(), this);
        //    }
        //    else if (MainMenu.ToLower().Equals("lblreports"))
        //    {
        //        Label l1 = new Label();
        //        l1.Name = "lblfinancial";
        //        this.setSubmenuStyle(l1, "Financial");
        //        Label l2 = new Label();
        //        l2.Name = "lblaccounts";
        //        this.setSubmenuStyle(l2, "Accounts");
        //        Label l3 = new Label();
        //        l3.Name = "lblpharmacy";
        //        this.setSubmenuStyle(l3, "POS");
        //        Label l4 = new Label();
        //        l4.Name = "lblproductreports";
        //        this.setSubmenuStyle(l4, "Product Reports");
        //        pnlSubMenu.Controls.AddRange(new[] { l1, l2, l3, l4 });
        //    }
        //    else if (MainMenu.ToLower().Equals("lblusers"))
        //    {
        //        Label l1 = new Label();
        //        l1.Name = "lblusers";
        //        this.setSubmenuStyle(l1, "Users");
        //        //Label l2 = new Label();
        //        //l2.Name = "lbllogin";
        //        //this.setSubmenuStyle(l2, "Login");
        //        //Label l3 = new Label();
        //        //l3.Name = "lbllogout";
        //        //this.setSubmenuStyle(l3, "Logout");
        //        Label l4 = new Label();
        //        l4.Name = "lbladminsetting";
        //        this.setSubmenuStyle(l4, "Admin Setting");
        //        //Label l5 = new Label();
        //        //l5.Name = "lblloaddefaultitems";
        //        //this.setSubmenuStyle(l5, "Load Default Items");
        //        if (!SharedVariables.AdminPractiseSetting.UserDeletedDefaultData)
        //        {
        //            Label l6 = new Label();
        //            l6.Name = "lblremovedefaultitems";
        //            this.setSubmenuStyle(l6, "Remove Default Items");
        //            pnlSubMenu.Controls.Add(l6);
        //        }
        //        pnlSubMenu.Controls.AddRange(new[] { l1, /*l2, l3,*/ l4 });
        //    }
        //    else if (MainMenu.ToLower().Equals("lblpreferences"))
        //    {
        //        Label l1 = new Label();
        //        l1.Name = "lbldiscounts";
        //        this.setSubmenuStyle(l1, "Discounts");
        //        pnlSubMenu.Controls.AddRange(new[] { l1 });
        //    }
        //    else if (MainMenu.ToLower().Equals("lblshopactivities"))
        //    {
        //        Label l1 = new Label();
        //        l1.Name = "lblposclosing";
        //        this.setSubmenuStyle(l1, "POS Closing");

        //        Label l2 = new Label();
        //        l2.Name = "lblstoreclosing";
        //        this.setSubmenuStyle(l2, "Store Closing");

        //        pnlSubMenu.Controls.AddRange(new[] { l1, l2 });
        //    }
        //    else if (MainMenu.ToLower().Equals("lblcontactus"))
        //    {
        //        frmContactUs f = new frmContactUs();
        //        f.StartPosition = FormStartPosition.Manual;
        //        f.StartPosition = FormStartPosition.CenterScreen;
        //        f.ShowDialog();
        //    }
        //}
        private void removeDefaultItems()
        {
            // remove default items if none of the followings is done
            // stock added
            // consumptions done OR invoice added(both basically add consumption)
            // adjustment added
            bool canDeleteDefaults = true;
            using (unitOfWork = new UnitOfWork())
            {
                if (unitOfWork.StockRepository.anyActiveStockExists())
                {
                    canDeleteDefaults = false;
                }
                if (unitOfWork.StockConsumptionRepository.AnyActiveConsExists())
                {
                    canDeleteDefaults = false;
                }
                if (unitOfWork.AdjustmentRepository.AnyActiveAdjExists())
                {
                    canDeleteDefaults = false;
                }
                if (canDeleteDefaults)
                {
                    lblAppStatus.BackColor = Color.Yellow;
                    lblAppStatus.Text = "Please hold on, we clearing default data...";
                    lblAppStatus.BackColor = System.Drawing.SystemColors.Control;
                    if (unitOfWork.GeneralRepository.ClearDefaultData())
                    {
                        MessageBox.Show("Default data cleared successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        try
                        {
                            AdminPractiseSetting setting = unitOfWork.AdminPractiseSettingRepository.getAdminPracStting();
                            setting.UserDeletedDefaultData = true;
                            unitOfWork.AdminPractiseSettingRepository.Update(setting);
                            unitOfWork.Save();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to update default items deletion status, item will get loaded again, please contact giga keys solutions if this issue persists.", "Status Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        Application.Restart();
                    }
                    else
                    {
                        MessageBox.Show("An error occurred while default data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Default data can't be cleared, You have performed some transactions.", "Operation Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }


        //private void pnlAnimTimer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    this.BeginInvoke(new Action(() =>
        //        {
        //            if (this.isMenuPanelVisible)
        //            {
        //                while (pnlMenu.Width > 0)
        //                {
        //                    pnlMenu.Width -= 20;
        //                    if (pnlMenu.Width <= 0)
        //                    {
        //                        PnlAnimTimer.Stop();
        //                    }
        //                }
        //            }
        //            else
        //            {

        //                while (pnlMenu.Width < PnlMenuWidth)
        //                {
        //                    pnlMenu.Width += 20;
        //                    if (pnlMenu.Width >= PnlMenuWidth)
        //                    {
        //                        PnlAnimTimer.Stop();
        //                    }
        //                }
        //            }
        //        }));
        //    this.Invalidate();
        //}
        private void pnlMenu_Paint(object sender, PaintEventArgs e)
        {
            if (pnlMenu.VerticalScroll.Visible)
            {
                Console.WriteLine("scrool bar visible.");
            }
        }
        private void MPharmacy_MouseEnter(object sender, EventArgs e)
        {
            ToggleMainMenuHoverStyle(sender, true);
        }
        private void MPharmacy_MouseLeave(object sender, EventArgs e)
        {
            ToggleMainMenuHoverStyle(sender, false);
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void loadDefaultItemsData_FromExcelSheet(DataTable dtSource)
        {
            string suppName = "";
            using (unitOfWork = new UnitOfWork())
            {

                if (unitOfWork.SupplierRepository.SuppliersExist())
                {
                    goto LoadItems;
                }
                List<Supplier> sl = new List<Supplier>();
                foreach (DataRow r in dtSource.Rows)
                {
                    suppName = "";
                    suppName = r["Supplier"].ToString().Trim();
                    if (suppName == "")
                    {
                        continue;
                    }
                    Supplier objSupplier = sl.Where(sp => sp.Name.ToLower().Equals(suppName.ToLower())).FirstOrDefault();
                    if (objSupplier == null)
                    {
                        objSupplier =
                        new Supplier
                        {
                            Name = suppName,
                            Phone = "",
                            Address = "",
                            PrimaryContactPersonName = "",
                            PrimaryContactPersonPhone = "",
                            OpeningBalance = 0,
                            CreatedAt = DateTime.Now,
                            UpdatedAt = DateTime.Now,
                            IsActive = true,
                            IsNew = true,
                            IsUpdate = false,
                            IsSynced = false
                        };
                        sl.Add(objSupplier);
                    }
                    else
                    {

                    }
                }
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.SupplierRepository.InsertRange(sl);
                    unitOfWork.Save();
                }
            }

        LoadItems:
            List<Item> il = new List<Item>();
            using (UnitOfWork unitOfWork = new UnitOfWork())
            {
                foreach (DataRow r in dtSource.Rows)
                {
                    Item objItem = new Item();
                    objItem.ItemName = r["Product Name"].ToString().Trim();
                    if (objItem.ItemName == "")
                    {
                        continue;
                    }
                    objItem.Barcode = r["Product Code"].ToString();
                    objItem.RetailPrice = Convert.ToDouble(r["Retail Price"]);
                    suppName = "";
                    suppName = r["Supplier"].ToString().Trim();
                    Supplier objSupplier = unitOfWork.SupplierRepository.GetSupplierByName(suppName.ToLower());
                    objItem.Rack = null;
                    objItem.Unit = "";
                    objItem.ReOrderingLevel = 0;
                    objItem.OpeningStock = 0;
                    objItem.UnitCostPrice = 0;
                    objItem.IsActive = true;
                    objItem.IsNew = true;
                    objItem.IsUpdate = false;
                    objItem.IsSynced = false;
                    objItem.CreatedAt = DateTime.Now;
                    objItem.UpdatedAt = DateTime.Now;
                    objItem.IsDefault = false;
                    objItem.Suppliers = new List<Supplier>();
                    objItem.Suppliers.Add(objSupplier);
                    il.Add(objItem);
                }
                unitOfWork.ItemRspository.InsertRange(il);
                unitOfWork.Save();
            }
            MessageBox.Show("Data loaded successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //tCheckFormsCollection.Stop();
            EmailRetryTimer.Stop();
        }

        //bool pnlShown = false;
        private void brnDashboard_Click(object sender, EventArgs e)
        {
            //if (pnlShown)
            //{
            Form f = SharedFunctions.OpenForm(new Dashboard(), this);
            f.WindowState = FormWindowState.Maximized;
            //pnlShown = false;
            //}
            //else
            //{
            //    this.pnlDashBoard.BringToFront();
            //    pnlShown = true;
            //}
            //foreach(frosm)
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
        }

        private void btnToggleLogin_Click(object sender, EventArgs e)
        {
            loadMainForm();
            //if (btnToggleLogin.Tag.ToString().Equals("0"))
            //{
            //    if (LoginUser())
            //    {
            //        btnToggleLogin.Text = "Logout";
            //        btnToggleLogin.Tag = "1";
            //        menuStrip1.Enabled = true;
            //    }
            //}
            //else
            //{
            //    LogoutUser();
            //}
        }

        private void btnCreateInvoice_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmInvoice(), this.MdiParent);
        }

        private void btnInvoiceHistory_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmInvoiceHistory(), this);
        }

        private void btnPurchaseOrders_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmPurchaseOrders(), this);
        }

        private void btnDashboard_Enter(object sender, EventArgs e)
        {
            //btnDashboard.Image = 
        }

        private void btnDashboard_Leave(object sender, EventArgs e)
        {

        }

        private void btnSyncData_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (!SharedFunctions.IsInternetAvailable())
            //    {
            //        MessageBox.Show("Please check your internet connection and try again.", "No Internet", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        return;
            //    }
            //    ServiceController sc = new ServiceController("PharmacySyncingService");
            //    if (sc.Status == ServiceControllerStatus.Running)
            //    {
            //        sc.Stop();
            //        sc.Start();
            //        MessageBox.Show("Syncing process started, it will be auotmatically be completed in backend." + Environment.NewLine + " you can continue with your work.", "Try Again", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //    else if (sc.Status == ServiceControllerStatus.StopPending)
            //    {
            //        MessageBox.Show("Please wait for service to stop, then try again.", "Try Again", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //    else
            //    {
            //        sc.Start();
            //        MessageBox.Show("Syncing process started, it will be auotmatically be completed in backend." + Environment.NewLine + " you can continue with your work.", "Try Again", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("An error occurred while starting syncing process, please try again later.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void btnItems_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmAllItems(), this);
        }


        private void toggleSideMennu(bool expand)
        {
            if (expand)
            {
                //btnTogMenu.Location = new Point(btnTogMenu.Location.X - 50, btnTogMenu.Location.Y);
                panel1.Width = SharedVariables.SideBarWidth;
                pnlMenu.Width = 230;
                pnlSideTopHeader.Width = 231;
                // line1.Width = 230;

                pictureBox1.Location = new Point(pictureBox1.Location.X + 89, pictureBox1.Location.Y);
                lblTogBtn.Location = new Point(lblTogBtn.Location.X + 178, lblTogBtn.Location.Y);
                //lblTogBtn.Image = Properties.Resources.MenuCollapse;
                pictureBox1.Width = 45;
                pictureBox1.Height = 65;
                //lblUserName.Visible = true;

                pnlMenu.AutoScroll = true;
                pnlMenu.HorizontalScroll.Enabled = true;
                pnlMenu.HorizontalScroll.Visible = true;
            }
            else
            {
                panel1.Width = 50;
                pnlSideTopHeader.Width = 50;
                //  line1.Width = 50;

                pictureBox1.Location = new Point(pictureBox1.Location.X - 89, pictureBox1.Location.Y);
                lblTogBtn.Location = new Point(lblTogBtn.Location.X - 178, lblTogBtn.Location.Y);
                //lblTogBtn.Image = Properties.Resources.MenuExpand;
                pictureBox1.Width = 45;
                pictureBox1.Height = 40;
                // lblUserName.Visible = false;
                //pnlMenu.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                pnlMenu.Width = 44;
                pnlMenu.AutoScroll = false;
                pnlMenu.HorizontalScroll.Visible = false;
                //pnlMenu.HorizontalScroll.Maximum = 0;
                //pnlMenu.AutoScroll = false;
                //pnlMenu.VerticalScroll.Visible = false;
                //pnlMenu.AutoScroll = true;
            }
        }

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams handleParam = new CreateParams();
        //        handleParam.ExStyle |= 0x02000000;
        //        return handleParam;
        //    }
        //}

        private void btnNotification_Click(object sender, EventArgs e)
        {
            this.LS_PageNo = 1;

            if (pnlNotisContainer.Tag.ToString() == "0")
            {
                //if (!bgW_LowStockItems.IsBusy)
                //{
                //    bgW_LowStockItems.RunWorkerAsync();
                //}
                this.index = 0;
                pnlNotifications.Controls.Clear();
                LoadLStockItems(LS_PageNo);
                pnlNotisContainer.Visible = true;
                pnlNotisContainer.Tag = "1";
                pnlNotisContainer.Location = new Point(btnNotification.Location.X - 110, btnNotification.Location.Y + 60);
            }
            else
            {
                pnlNotisContainer.Tag = "0";
                pnlNotisContainer.Visible = false;
            }
        }

        private void pnlNotifications_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblTogBtn_Click(object sender, EventArgs e)
        {
            if (this.isMenuCpsd)
            {
                toggleSideMennu(expand: true);
            }
            else
            {
                toggleSideMennu(expand: false);
                pnlSubMenu.Visible = false;
            }
            this.isMenuCpsd = !this.isMenuCpsd;
        }




        IPagedList<LowStockNotiVM> LowStockNotis;
        IPagedList<ExpiryNotiVM> ExpiryNotis;
        /// <summary>
        /// page no for low stock items dynamic loading.
        /// </summary>
        int LS_PageNo = 1;
        /// <summary>
        /// page no for expiry items dynamic loading.
        /// </summary>
        int Exp_PageNo = 1;
        int pageSize = 20;
        // background workers
        int index = 0;
        private void bgW_LowStockItems_DoWork(object sender, DoWorkEventArgs e)
        {

        }
        private void bgW_LowStockItems_RWComp(object sender, RunWorkerCompletedEventArgs e)
        {

            //this.bgW_LowStockItems.RunWorkerAsync();
        }

        private void pnlNotifications_Scroll(object sender, ScrollEventArgs e)
        {


            if (e.NewValue > e.OldValue)
            {
                NextItems();
            }
        }

        private void NextItems()
        {
            if (currNotiType == CurrentNotitype.LowStock)
            {
                LS_PageNo += 1;
                LoadLStockItems(LS_PageNo);
            }
            else
            {
                Exp_PageNo += 1;
                LoadExpItems(Exp_PageNo);
            }
        }

        private void pnlNotifications_MouseWheel(object sender, MouseEventArgs e)
        {
            NextItems();
        }

        private void LoadLStockItems(int PageNo)
        {
            using (unitOfWork = new UnitOfWork())
            {
                LowStockNotis = unitOfWork.ItemRspository.GetLowStockNotis(PageNo, pageSize);
            }

            foreach (var i in LowStockNotis.Items)
            {
                if (i.AvailableStock >= i.ReorderingLevel) { continue; }
                index++;
                Panel pnlNoti = new Panel();
                pnlNoti.SuspendLayout();
                pnlNoti.BorderStyle = BorderStyle.FixedSingle;
                pnlNoti.Location = new System.Drawing.Point(3, 3);
                pnlNoti.Name = "pnlNoti";
                pnlNoti.Size = new System.Drawing.Size(378, 52);
                pnlNoti.TabIndex = 0;
                pnlNoti.BackColor = Color.FromArgb(255, 157, 150);
                pnlNoti.Tag = i.ItemId;
                pnlNoti.Click += (sender, EventArgs) => { pnlNoti_Clicked(sender, EventArgs, i.ItemId); };// new EventHandler(pnlNoti_Clicked);
                pnlNoti.Cursor = Cursors.Hand;

                Label lblCount = new Label();
                lblCount.AutoSize = true;
                lblCount.Location = new System.Drawing.Point(2, 10);
                lblCount.Name = "lblCount";
                lblCount.Size = new System.Drawing.Size(38, 14);
                lblCount.TabIndex = 0;
                lblCount.Text = index.ToString();

                Label lblDescription = new Label();
                lblDescription.AutoSize = true;
                lblDescription.Location = new System.Drawing.Point(41, 10);
                lblDescription.Name = "lblDescription";
                lblDescription.Size = new System.Drawing.Size(38, 14);
                lblDescription.TabIndex = 0;
                lblDescription.Click += (sender, EventArgs) => { pnlNoti_Clicked(sender, EventArgs, i.ItemId); };
                lblDescription.Text = i.ItemName;

                Label lblNotiType = new Label();
                lblNotiType.AutoSize = true;
                lblNotiType.Location = new System.Drawing.Point(pnlNoti.Width - 60, 2);
                lblNotiType.Name = "lblType";
                lblNotiType.Size = new System.Drawing.Size(38, 14);
                lblNotiType.TabIndex = 0;
                lblNotiType.Click += (sender, EventArgs) => { pnlNoti_Clicked(sender, EventArgs, i.ItemId); };
                lblNotiType.Text = "Low Stock";

                // 
                // lblTime
                // 
                Label lblTime = new Label();
                lblTime.AutoSize = true;
                lblTime.Location = new System.Drawing.Point(41, 26);
                lblTime.Name = "lblTime";
                lblTime.Size = new System.Drawing.Size(38, 14);
                lblTime.TabIndex = 0;
                lblTime.Text = DateTime.Now.ToString();
                lblTime.Click += (sender, EventArgs) => { pnlNoti_Clicked(sender, EventArgs, i.ItemId); };
                pnlNoti.Controls.Add(lblTime);

                pnlNoti.Controls.Add(lblCount);
                pnlNoti.Controls.Add(lblDescription);
                //pnlNoti.ResumeLayout(false);
                pnlNoti.Controls.Add(lblNotiType);
                this.pnlNotifications.Controls.Add(pnlNoti);
            }
            //this.pnlNotifications.ResumeLayout(true);
        }
        private void LoadExpItems(int PageNo)
        {
            int ExpiryPeriodCount = SharedVariables.AdminPharmacySetting.ExpiryPeriod;

            if (ExpiryPeriodCount <= 0)
            {
                return;
            }

            DateTime ExpNotiPeriod = DateTime.Now;
            if (SharedVariables.AdminPharmacySetting.ExpiryPeriodUnit == (int)SharedVariables.ExpiryPeriodUnit.Days)
            {
                ExpNotiPeriod = DateTime.Now.AddDays(ExpiryPeriodCount);
            }
            else
            {
                ExpNotiPeriod = DateTime.Now.AddMonths(ExpiryPeriodCount);
            }


            using (unitOfWork = new UnitOfWork())
            {
                ExpiryNotis = unitOfWork.ItemRspository.GetExpiryNotis(ExpNotiPeriod, Exp_PageNo, pageSize);
            }

            int daysExpReaching = 0;


            //this.pnlNotifications.SuspendLayout();
            foreach (var i in ExpiryNotis.Items)
            {
                index++;
                Panel pnlNoti = new Panel();
                pnlNoti.SuspendLayout();
                pnlNoti.BorderStyle = BorderStyle.FixedSingle;
                pnlNoti.Location = new System.Drawing.Point(3, 3);
                pnlNoti.Name = "pnlNoti";
                pnlNoti.Size = new System.Drawing.Size(378, 52);
                pnlNoti.TabIndex = 0;
                pnlNoti.BackColor = Color.FromArgb(254, 233, 128);
                pnlNoti.Tag = i.ItemId;
                pnlNoti.Cursor = Cursors.Hand;

                Label lblCount = new Label();
                lblCount.AutoSize = true;
                lblCount.Location = new System.Drawing.Point(2, 10);
                lblCount.Name = "lblCount";
                lblCount.Size = new System.Drawing.Size(38, 14);
                lblCount.TabIndex = 0;
                lblCount.Text = index.ToString();

                Label lblDescription = new Label();
                lblDescription.AutoSize = true;
                lblDescription.Location = new System.Drawing.Point(41, 10);
                lblDescription.Name = "lblDescription";
                lblDescription.Size = new System.Drawing.Size(38, 14);
                lblDescription.TabIndex = 0;
                lblDescription.Text = i.ItemName;

                Label lblNotiType = new Label();
                lblNotiType.AutoSize = true;
                lblNotiType.Location = new System.Drawing.Point(pnlNoti.Width - 150, 2);
                lblNotiType.Name = "lblType";
                lblNotiType.Size = new System.Drawing.Size(38, 14);
                lblNotiType.TabIndex = 0;

                daysExpReaching = (i.BatchExp - DateTime.Now).Days;
                lblNotiType.Text = "Expiry Reaching in " + daysExpReaching + (daysExpReaching == 1 ? " Day" : " Days");

                // 
                // lblTime
                // 
                Label lblTime = new Label();
                lblTime.AutoSize = true;
                lblTime.Location = new System.Drawing.Point(41, 26);
                lblTime.Name = "lblTime";
                lblTime.Size = new System.Drawing.Size(38, 14);
                lblTime.TabIndex = 0;
                string expDate = i.BatchExp.ToString("yyyy-MMM-dd");
                lblTime.Text = "Batch: " + i.BatchNo + " => Expiry : " + expDate;
                pnlNoti.Controls.Add(lblTime);

                pnlNoti.Controls.Add(lblCount);
                pnlNoti.Controls.Add(lblDescription);
                //pnlNoti.ResumeLayout(false);
                pnlNoti.Controls.Add(lblNotiType);
                this.pnlNotifications.Controls.Add(pnlNoti);
            }
            //this.pnlNotifications.ResumeLayout(true);
        }
        public void pnlNoti_Clicked(object sender, EventArgs e, long ItemId)
        {
            btn f = new btn(ItemId);
            f.Show();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblLowStock_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            currNotiType = CurrentNotitype.LowStock;
            index = 0;
            LS_PageNo = 1;
            pnlNotifications.Controls.Clear();
            LoadLStockItems(this.LS_PageNo);
        }

        private void lblExpiry_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            currNotiType = CurrentNotitype.Expiry;
            index = 0;
            Exp_PageNo = 1;
            pnlNotifications.Controls.Clear();
            LoadExpItems(this.Exp_PageNo);

        }

        private void lblSubsNoti_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pnlNotifications.Controls.Clear();
            try
            {
                int days = 0;
                DateTime dtExp = new DateTime();
                using (unitOfWork = new UnitOfWork())
                {
                    dtExp = DateTime.Parse(CryptopEngine.Decrypt(unitOfWork.HwDataRepository.GetSystemExpiryDate(), GlobalSharing.Salt));
                    days = (dtExp - DateTime.Now).Days;
                }
                Panel pnlNoti = new Panel();
                pnlNoti.BorderStyle = BorderStyle.FixedSingle;
                pnlNoti.Location = new System.Drawing.Point(12, 3);
                pnlNoti.Name = "pnlNoti";
                pnlNoti.Size = new System.Drawing.Size(395, 200);
                pnlNoti.TabIndex = 0;
                pnlNoti.BackColor = Color.FromArgb(108, 212, 252);
                pnlNoti.Tag = 0;
                pnlNoti.Cursor = Cursors.Hand;

                Label lblDescription = new Label();
                lblDescription.Font = new Font("Arial", 11);
                lblDescription.Location = new System.Drawing.Point(5, 90);
                lblDescription.Name = "lblDescription";
                //lblDescription.Size = new System.Drawing.Size(38, 14);
                lblDescription.MaximumSize = new Size(pnlNoti.Width - 10, 0);
                lblDescription.AutoSize = true;
                lblDescription.TabIndex = 0;
                if (days > 60)
                {
                    lblDescription.ForeColor = Color.Green;
                }
                else if (days > 30 && days < 60)
                {
                    lblDescription.ForeColor = Color.Yellow;
                }
                else
                {
                    lblDescription.ForeColor = Color.Red;
                }
                lblDescription.Text = "Your subscription will expire in " + days + "Day(s) on " + dtExp.ToString("yyyy-MMM-dd");


                //// 
                //// lblTime
                //// 
                //Label lblTime = new Label();
                //lblTime.AutoSize = true;
                //lblTime.Location = new System.Drawing.Point(5, 34);
                //lblTime.Name = "lblTime";
                //lblTime.Size = new System.Drawing.Size(38, 14);
                //lblTime.TabIndex = 0;
                //lblTime.Text = "Your Subscription started on " + dtExp.ToString("yyyy-MMM-dd");
                //pnlNoti.Controls.Add(lblTime);

                pnlNoti.Controls.Add(lblDescription);
                //pnlNoti.Controls.Add(lblTime);
                this.pnlNotifications.Controls.Add(pnlNoti);
            }
            catch (Exception ex)
            {

            }
        }

        private void manufacturersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmManufacturers(), this);
        }

        private void templatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmTemplates(), this);
        }

        private void stockAuditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            if (SharedFunctions.CheckDayClosed()) { return; }
            SharedFunctions.OpenForm(new frmStockAudits(), this);
        }

        private void rackViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmStockAudits(), this);
        }

        private void missedSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetactiveItem(sender);
            SharedFunctions.OpenForm(new frmMissedSales(), this);
        }

        private void customersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmCustomers(), this);
        }

        private void discountsManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDiscounts f = new frmDiscounts();
            f.Show();
        }

        private void storeClosingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!SharedVariables.AdminPharmacySetting.EnableDayClose)
            {
                MessageBox.Show("You can't use store closing, because not enabled in your system. goto User/Admin Setting page and then Enable Day Close under Store tab.", "Store Closing Not Enabled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SharedFunctions.OpenForm(new frmStoreClosing(), this);
            //if (!SharedVariables.IsStoreClosed)
            //{
            //    SharedFunctions.OpenForm(new frmStoreClosing(), this);
            //}
            //else
            //{
            //    MessageBox.Show("Can't perform this action when store is closed.", "Not Allowed After Store Closing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }


        private void saleCounterClosingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmPOSCosing(), this);
        }

        private void expenseManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SharedFunctions.CheckDayClosed()) { return; }
            SharedFunctions.OpenForm(new frmExpense(), this);
        }

        private void headOfficeReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SharedFunctions.CheckDayClosed()) { return; }
            SharedFunctions.OpenForm(new frmStockHOReturns(), this);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmUnpaidInvoices(), this);
        }

        private void manageExpensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void seatingTablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmSeatingTable(), this);
        }

        private void employeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmEmployees(), this);
        }

        private void recipiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SharedFunctions.OpenForm(new frmRecipe(), this);
        }
    }
}