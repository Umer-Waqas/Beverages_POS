
using Utilities;

namespace Restaurant_MS_UI.App
{
    public partial class SplashForm : Form
    {
        System.Windows.Forms.Timer t;
        public SplashForm()
        {
            InitializeComponent();
        }

        private void SplashForm_Load(object sender, EventArgs e)
        {
            t = new System.Windows.Forms.Timer();
            t.Interval = 1000;
            t.Tick += Timer_Tick;
            t.Start();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                bool crashSystem = false;
                t.Stop();
                SharedVariables.SqlConnectionString = SharedFunctions.GetConnectionString();
                SharedVariables.IsApplicationStarted = true;
                // load all configurations and any other application wise data here
                using (UnitOfWork uw = new UnitOfWork())
                {
                    LoadConfigurations(uw);
                    HwData objHw = uw.HwDataRepository.GetAll().OrderByDescending(r => r.HwDataId).FirstOrDefault();
                    if (string.IsNullOrEmpty(objHw.SystemExpiry))
                    {
                        crashSystem = true;
                    }
                    else
                    {
                        DateTime dtExp;
                        bool parsed = DateTime.TryParse(CryptopEngine.Decrypt(objHw.SystemExpiry.ToString(), GlobalSharing.Salt), out dtExp);
                        if (!parsed)
                        {
                            crashSystem = true;
                        }
                        else
                        {
                            if (DateTime.Now.Date < dtExp)
                            {
                                crashSystem = false;
                            }
                            else
                            {
                                crashSystem = true;
                            }
                        }
                    }
                }
                if (crashSystem)
                {
                    // call api here to check if aplication has been activated(practiseId)
                    MessageBox.Show("System Crashed and stopped working, please contact Giga Keys Solutions for further processing.", "System Crashed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    Application.Exit();
                }
                this.Invoke(new Action(() =>
                {
                    this.Hide();
                    if (IsApplicationActivated())
                    {
                        new frmMain().Show();
                    }
                    else
                    {
                        new ActivationForm().Show();
                    }

                }));

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message);
                    if (ex.InnerException.InnerException != null)
                    {
                        MessageBox.Show(ex.InnerException.InnerException.Message);
                        if (ex.InnerException.InnerException.InnerException != null)
                        {
                            MessageBox.Show(ex.InnerException.Message);
                        }
                    }
                }
                MessageBox.Show("System Crashed and stopped working, please contact Giga Keys Solutions for further processing. Exception :  " + ex.Message, "System Crashed", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                Application.Exit();
            }
        }

        private bool IsApplicationActivated()
        {
            try
            {
                string Status = "";// CryptopEngine.Decrypt(Properties.Settings.Default.Activated.ToString(), GlobalSharing.Salt);
                if (!string.IsNullOrEmpty(Status) && Convert.ToBoolean(Status))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void OpenMainFrom(object obj)
        {
            frmMain f = new frmMain();
            f.WindowState = FormWindowState.Maximized;
            Application.Run(f);
        }

        private void OpenActivationFrom(object obj)
        {
            ActivationForm f = new ActivationForm();
            f.BringToFront();
            //f.WindowState = FormWindowState.Maximized;
            Application.Run(f);
        }

        private void LoadConfigurations(UnitOfWork unitOfWork)
        {
            try
            {
                #region Admin Invoice
                List<FlatDiscount> TodayDiscounts = new List<FlatDiscount>();
                DataTable dt = new DataTable();
                dt.Columns.Add("ItemId", typeof(int));
                dt.Columns.Add("Item Name", typeof(string));
                //dt.Columns.Add("Barcode", typeof(string));
                //dt.Columns.Add("Generic Name", typeof(string));
                //dt.Columns.Add("Unit", typeof(string));
                //dt.Columns.Add("Manufacturer", typeof(string));
                //dt.Columns.Add("Cost Price", typeof(double));
                dt.Columns.Add("Retail Price", typeof(double));
                dt.Columns.Add("Quantity", typeof(int));

                List<BulkItemsVM> bulkList = new List<BulkItemsVM>();

                SharedVariables.AdminInvoiceSetting = unitOfWork.AdminInvoiceSettingRepository.GetAll().LastOrDefault();
                SharedVariables.AdminProcedureInvoiceSetting = unitOfWork.AdminProcedureInvoiceSettingRepository.GetAll().LastOrDefault();
                SharedVariables.AdminPractiseSetting = unitOfWork.AdminPractiseSettingRepository.GetAll().LastOrDefault();
                SharedVariables.AdminPrintFormatSetting = unitOfWork.AdminPrintFormatSettingRepository.GetAll().LastOrDefault();


                if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.Enable_FBR))
                {
                    SharedVariables.AdminPractiseSetting.Enable_FBR = CryptopEngine.Decrypt(SharedVariables.AdminPractiseSetting.Enable_FBR, GlobalSharing.Salt);
                }
                else
                {
                    SharedVariables.AdminPractiseSetting.Enable_FBR = "0";
                }
                if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.FBR_POSID))
                {
                    SharedVariables.AdminPractiseSetting.FBR_POSID = CryptopEngine.Decrypt(SharedVariables.AdminPractiseSetting.FBR_POSID, GlobalSharing.Salt);
                }
                //if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.FBR_AccessCode))
                //{
                //    SharedVariables.AdminPractiseSetting.FBR_AccessCode = CryptopEngine.Decrypt(SharedVariables.AdminPractiseSetting.FBR_AccessCode, GlobalSharing.Salt);
                //}

                SharedVariables.AdminPharmacySetting = unitOfWork.AdminPharmacySettingRepository.GetAll().LastOrDefault();

                if (!SharedVariables.IsTesting) // this will avoid extra load time when there is no need to load items data for testing
                {
                    SharedVariables.BulkLoadedItemsList = unitOfWork.ItemRspository.GetBulkAllActiveItems();
                }
                SharedVariables.TodayDiscounts = unitOfWork.FlatDiscountRepository.getTodayDiscounts();
                SharedVariables.AdminShiftSettings = unitOfWork.AdminShiftsSettingRepository.GetActiveShifts();
                SharedVariables.AdminShiftMasterSetting = unitOfWork.AdminShiftMasterSettingRepository.GetAll().FirstOrDefault();

                foreach (var i in SharedVariables.BulkLoadedItemsList)
                {
                    //dt.Rows.Add(i.ItemId, i.ItemName, i.Barcode, i.GenericName, i.Unit, i.Manufacturer, i.CostPrice, i.RetailPrice, 1);
                    dt.Rows.Add(i.ItemId, i.ItemName, i.RetailPrice, 1);
                }
                SharedVariables.BulkLoadedItemsDataView = new DataView(dt);

                #endregion Admin Invoice Settings
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message);
                    if (ex.InnerException.InnerException != null)
                    {
                        MessageBox.Show(ex.InnerException.InnerException.Message);
                        if (ex.InnerException.InnerException.InnerException != null)
                        {
                            MessageBox.Show(ex.InnerException.Message);
                        }
                    }
                }
                SharedFunctions.ShowErrorMessage("Failed to load Some Settings, you can Continue With Your Work.", ex.Message, "Failed to Load Some Settings.");
            }
        }

        private void LoadDefaultItems()
        {
            frmWaiting f = new frmWaiting();
            f.StartPosition = FormStartPosition.CenterParent;
            f.ShowDialog();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }
    }
}