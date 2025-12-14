

using Utilities;

namespace Restaurant_MS_UI.App
{
    public partial class frmLogin : Form
    {
        UnitOfWork unitOfWork;
        public frmLogin()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }
        private Tuple<bool, string, string> IsPasswordRemembered()
        {
            using UnitOfWork unitOfWork = new UnitOfWork();
            var RememberUserCredentialsSetting = unitOfWork.AppSettingsRepository.GetAppSetting(AppSettingKeys.RememberUserCredentials);
            var RememberUserNameSetting = unitOfWork.AppSettingsRepository.GetAppSetting(AppSettingKeys.RememberUserName);
            var RememberUserPasswordSetting = unitOfWork.AppSettingsRepository.GetAppSetting(AppSettingKeys.RememberUserPassword);


            if (RememberUserCredentialsSetting == "true" && !string.IsNullOrEmpty(RememberUserNameSetting) && !string.IsNullOrEmpty(RememberUserPasswordSetting))
            {
                string UserName = "";
                string Password = "";
                try
                {
                    UserName = RememberUserNameSetting;
                    Password = RememberUserPasswordSetting;
                    return new Tuple<bool, string, string>(true, UserName, Password);
                }
                catch (Exception ex)
                {
                    return new Tuple<bool, string, string>(false, "", "");
                }
            }
            else
            {
                return new Tuple<bool, string, string>(false, "", "");
            }
        }
        /// <summary>
        /// }
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsValidInput()) return;
                string UserName = txtUserName.Text.Trim();
                string Password = txtPassword.Text;


                SharedVariables.LoggedInUser = unitOfWork.UserRepository.IsLoggedIn(UserName, Password);
                if (chkRememberMe.Checked && SharedVariables.LoggedInUser != null)
                {
                    using UnitOfWork unitOfWork = new UnitOfWork();
                    var rememberSetting = new Appsettings()
                    {
                        Key = AppSettingKeys.RememberUserCredentials,
                        Value = "true"
                    };
                    unitOfWork.AppSettingsRepository.SaveAppSetting(rememberSetting);


                    var rememberSettingUserName = new Appsettings()
                    {
                        Key = AppSettingKeys.RememberUserName,
                        Value = SharedVariables.LoggedInUser.Email,
                    };
                    unitOfWork.AppSettingsRepository.SaveAppSetting(rememberSettingUserName);


                    var rememberSettingPassword = new Appsettings()
                    {
                        Key = AppSettingKeys.RememberUserPassword,
                        Value = SharedVariables.LoggedInUser.Password
                    };
                    unitOfWork.AppSettingsRepository.SaveAppSetting(rememberSettingPassword);
                }
                else
                {
                    using UnitOfWork unitOfWork = new UnitOfWork();
                    var rememberSetting = new Appsettings()
                    {
                        Key = AppSettingKeys.RememberUserCredentials,
                        Value = "false"
                    };
                    unitOfWork.AppSettingsRepository.SaveAppSetting(rememberSetting);


                    var rememberSettingUserName = new Appsettings()
                    {
                        Key = AppSettingKeys.RememberUserName,
                        Value = "",
                    };
                    unitOfWork.AppSettingsRepository.SaveAppSetting(rememberSettingUserName);


                    var rememberSettingPassword = new Appsettings()
                    {
                        Key = AppSettingKeys.RememberUserPassword,
                        Value = ""
                    };
                    unitOfWork.AppSettingsRepository.SaveAppSetting(rememberSettingPassword);
                }
                //Properties.Settings.Default.Save();
                if (SharedVariables.LoggedInUser == null)
                {
                    ErrMessage.Show();
                    return;
                }
                insertLoginHistory();
                this.Close();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Couldn't login you, please try run the application again", ex.Message, "Login Failed");
            }
        }
        private void insertLoginHistory()
        {
            try
            {
                LoginHistory h = new LoginHistory();
                h.UserId = SharedVariables.LoggedInUser.UserId;
                h.CreatedAt = DateTime.Now;
                h.UpdatedAt = DateTime.Now;
                unitOfWork.LoginHistoryRepository.Insert(h);
                unitOfWork.Save();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private bool IsValidInput()
        {
            bool ErrFound = false;
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                ErrUserName.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrUserName.Visible = false;
            }

            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                errPswd.Visible = true;
                ErrFound = true;
            }
            else
            {
                errPswd.Visible = false;
            }
            return !ErrFound;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SharedVariables.LoggedInUser = null;
            this.Close();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            var Result = IsPasswordRemembered();
            if (Result.Item1)
            {
                txtUserName.Text = Result.Item2;
                txtPassword.Text = Result.Item3;
                chkRememberMe.Checked = true;
            }
        }
    }
}