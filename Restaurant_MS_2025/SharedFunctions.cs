

using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using Utilities;

namespace Restaurant_MS_UI
{
    public static class SharedFunctions
    {
        public static Form IsFormOpen(string Name)
        {
            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                if (frm.Name == Name)
                {
                    return frm;
                }
            }
            return null;
        }
        public static Form OpenForm(Form objForm, Form ParentForm)
        {
            //if (Dashboard != null)
            //{
            //    Panel d = (Panel)Dashboard;
            //    d.SendToBack();
            //}
            Form f = SharedFunctions.IsFormOpen(objForm.Name);
            if (f == null)
            {
                objForm.MdiParent = ParentForm;
                objForm.BringToFront();
                objForm.Show();
                objForm.WindowState = FormWindowState.Maximized;
                return objForm;
            }
            else
            {
                f.Activate();
                f.BringToFront();
                f.WindowState = FormWindowState.Maximized;
                return f;
            }
        }
        public static void isValidNumericKey(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }


            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        public static bool CheckDayClosed()
        {
            bool status = false;
            if (!SharedVariables.AdminPharmacySetting.EnableDayClose)
            {
                return false;
            }
            if (!SharedVariables.IsPreviousDayClosed)
            {
                MessageBox.Show("Previous day is not closed, you are not allowed to perform any action. please contact your system Administrator/Manager to close previous day before inserting any new transactions. Press Ok to close the application", "Previous Day not Closed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                status = true;
            }
            if (SharedVariables.IsStoreClosed)
            {
                MessageBox.Show("Store has been closed, you are not allowed to perform any action. please contact your system administrator. Press Ok to close the application", "Day Cloed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                status = true;
            }
            return status;
        }

        public static void isValidIntegerKey(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        public static void ShowErrorMessage(string Message, string ExceptionMessage, string MessageTitle)
        {
            if (SharedVariables.ShowActualExceptionMessages)
            {
                MessageBox.Show(ExceptionMessage, MessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(Message, MessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void ShowGeneralSuccessMessage()
        {
            MessageBox.Show("Opereation Performed Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowGeneralErrorMessage(Exception ex)
        {
            MessageBox.Show("An error occurred while performing required operation, please try again." + Environment.NewLine + ex != null ? ex.Message : "", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void ShowSuccessMessage(string Message, string MessageTitle)
        {
            MessageBox.Show(Message, MessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowWarningMessage(string Message, string MessageTitle)
        {
            MessageBox.Show(Message, MessageTitle, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        public static string GetTimestamp2(DateTime value)
        {
            return value.ToString("yyMMddHHmmss");
        }

        public static string CopyFileToLocal(string SourceFilePath)
        {
            string DestFileName = GetTimestamp(DateTime.Now) + Path.GetExtension(SourceFilePath);
            string destDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Images");
            string DestPath = Path.Combine(destDir, DestFileName);
            string sourchPath = SourceFilePath;

            if (File.Exists(sourchPath))
            {
                Directory.CreateDirectory(destDir);
                File.Copy(sourchPath, DestPath);
            }
            return DestPath;
        }

        public static void GenerateStockConsumptionTable(DataTable dtStockConsumption)
        {
            dtStockConsumption.Columns.Add("Quantity", typeof(int));
            dtStockConsumption.Columns.Add("ConsumptionType", typeof(int));
            dtStockConsumption.Columns.Add("Comment", typeof(string));
            dtStockConsumption.Columns.Add("Batch_BatchId", typeof(string));
            dtStockConsumption.Columns.Add("Item_ItemID", typeof(int));
            dtStockConsumption.Columns.Add("Invoice_InvoiceId", typeof(string));
            dtStockConsumption.Columns.Add("CreatedAt", typeof(DateTime));
            dtStockConsumption.Columns.Add("IsActive", typeof(bool));
            dtStockConsumption.Columns.Add("IsNew", typeof(bool));
        }

        public static void ShowPageNo(Control ShowingControl, int CurrentPage, int TotalPages)
        {
            if (ShowingControl is Label || ShowingControl is TextBox)
            {
                ShowingControl.Text = "(page)" + CurrentPage + " / " + TotalPages;
            }
            else
            {
                MessageBox.Show("You Can Speicify only Label OR TextBox For Showing Page no.", "Invalid Control", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void SetApplicationStartupValues()
        {
            SharedVariables.PharmacyName = "Desktop Pharmacy Practices";
            SharedVariables.PharmacyPhone = "0302xxxxxxx";
            SharedVariables.PharmacyAddress = "N/A";
        }

        public static List<RightsVM> CreateRights()
        {
            List<RightsVM> Rights = new List<RightsVM>();

            Rights.Add(new RightsVM { RightId = 1, RightDescription = "Create Users" });
            Rights.Add(new RightsVM { RightId = 2, RightDescription = "Edit Users" });
            Rights.Add(new RightsVM { RightId = 3, RightDescription = "Delete Users" });
            Rights.Add(new RightsVM { RightId = 4, RightDescription = "Edit Payment/Invoice Date" });
            Rights.Add(new RightsVM { RightId = 5, RightDescription = "View Financial Reports" });
            Rights.Add(new RightsVM { RightId = 6, RightDescription = "Delete Patients" });
            Rights.Add(new RightsVM { RightId = 7, RightDescription = "Edit Invoice" });
            Rights.Add(new RightsVM { RightId = 8, RightDescription = "Refund Payment" });
            Rights.Add(new RightsVM { RightId = 9, RightDescription = "Delete Invoice" });
            Rights.Add(new RightsVM { RightId = 10, RightDescription = "Add Stock" });
            Rights.Add(new RightsVM { RightId = 11, RightDescription = "Edit Stock" });
            Rights.Add(new RightsVM { RightId = 12, RightDescription = "View Pharmacy Report" });
            Rights.Add(new RightsVM { RightId = 13, RightDescription = "Edit Item" });
            Rights.Add(new RightsVM { RightId = 14, RightDescription = "Add Item" });
            Rights.Add(new RightsVM { RightId = 15, RightDescription = "Delete Item" });
            Rights.Add(new RightsVM { RightId = 16, RightDescription = "Edit Invoice Retail Price" });
            return Rights;
        }

        public static bool IsActionallowed(string Right)
        {
            foreach (UserRole r in SharedVariables.LoggedInUser.UserRoles)
            {
                if (r.Description == "super Admin")
                {
                    return true;
                }
            }

            long RightId = SharedVariables.LoggedInUser.Rights.Where(r => r.Description == Right).Select(r => r.RightId).FirstOrDefault();
            if (RightId > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        public static byte[] ImageToByteArray(Image x)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }

        public static DataTable GetImageTable(string ImagePath)
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add(new DataColumn("LogoImage", typeof(byte[])));
                Image img = Image.FromFile(ImagePath);
                byte[] ImageBytes = ImageToByteArray(img);
                DataRow dr = dt.NewRow();
                dr[0] = ImageBytes;
                dt.Rows.Add(dr);
                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("You pharmacy logo is not found, image will not show on invoices and prints." + Environment.NewLine + "You can fix this by adding you logo under admin setting", "Logo not found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return dt;
        }

        public static void SetLarggeButtonsStyle(Button[] Buttons)
        {
            foreach (Button b in Buttons)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 140, 90);
                b.BackColor = Color.FromArgb(0, 166, 90);
                b.ForeColor = Color.White;
                b.FlatAppearance.BorderSize = 1;
                //b.Height = 30; // setting this property disturbs controls location in all forms <(O_O)>
                b.Font = new Font("Microsoft Sans Serif", 9, FontStyle.Bold);
            }
        }


        public static void SetControlForeColor(Control[] Controls)
        {
            foreach (Control c in Controls)
            {
                c.ForeColor = Color.FromArgb(42, 196, 244); ;
            }
        }





        public static void SetSmallButtonsStyle(Button[] Buttons)
        {
            foreach (Button b in Buttons)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 140, 90);
                b.BackColor = Color.FromArgb(0, 166, 90);
                b.ForeColor = Color.White;
                b.FlatAppearance.BorderSize = 1;
                //b.Height = 23;
                b.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold);
            }
        }
        public static void SetGridStyle(DataGridView grid, bool IsInvoiceGrid = false)
        {
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point);
            if (!IsInvoiceGrid)
            {
                grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(246, 246, 246);
            }
            grid.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.FromArgb(225, 248, 255);
            grid.RowTemplate.DefaultCellStyle.SelectionForeColor = Color.Black;
            grid.RowHeadersVisible = false;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersHeight = 35;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.RowTemplate.Height = 32;
            grid.RowTemplate.DefaultCellStyle.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular);
            grid.BackgroundColor = Color.White;

        }


        //public static void UpdateCommonStyle(DataGridView Grid)
        //{
        //    Grid.DefaultCellStyle.BackColor = SharedVariables.RowSelectionColor;
        //}

        // code for sending emails through API
        //public static void Email(string filePath, List<string> Receivers)

        //method signature for sending email using Giga Keys Solutions email using credentials
        public static void Email(string sender, string senderPswd, List<string> Receivers, List<string> AttachedDocs)
        {
            try
            {
                // code for sending emails through API
                //Service service = new Service();
                //string jsonLogin = service.Login();
                //if (IsJson(jsonLogin))
                //{
                //    Login login = new Login();
                //    login = JsonConvert.DeserializeObject<Login>(jsonLogin);
                //    service.Token = login.auth_token;
                //    if (login.success)
                //    {
                //        string dataSentBYAPI = "";
                //        dataSentBYAPI = service.SendLowStocckEmail(Receivers,login.auth_token).ToString();
                //        //foreach (FinalReport e in UnSyncedReports)
                //        //{
                //        //    dataSentBYAPI = service.SyncFinalReport(e, AppData.Email, AppData.Token, login.practice_id).ToString();
                //        //    if (Shared.IsJson(dataSentBYAPI))
                //        //    {
                //        //        SyncResponse result = JsonConvert.DeserializeObject<SyncResponse>(dataSentBYAPI);
                //        //        if (result.status == "success" || result.status.Contains("present"))
                //        //        {
                //        //            using (cxt = new DAL.AnalyzerContext())
                //        //            {
                //        //                FinalReport rpt = cxt.Reports.Where(r => r.FinalReportId == e.FinalReportId).FirstOrDefault();
                //        //                rpt.IsSynced = true;
                //        //                cxt.SaveChanges();
                //        //            }
                //        //        }        
                //        //    }
                //        //}
                //    }

                //code for sending email using Giga Keys Solutions email using credentials
                //MailMessage message = new MailMessage();
                //SmtpClient smtp = new SmtpClient();
                //message.From = new MailAddress(sender);
                //foreach (string receiver in Receivers)
                //{
                //    message.To.Add(new MailAddress(receiver));
                //}
                //message.Subject = "Low Stocks";
                //message.IsBodyHtml = true; //to make message body as html  
                //if (AttachedDocs != null)
                //{
                //    foreach (string doc in AttachedDocs)
                //    {
                //        message.Attachments.Add(new Attachment(doc));
                //    }
                //}
                //message.Body = "Please find the attached doc for low stock";
                //smtp.Port = 587;
                //smtp.Host = "smtp.gmail.com"; //for gmail host  
                //smtp.EnableSsl = true;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new NetworkCredential(Sender, SenderPassword);
                //smtp.Send(message);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void EmailWithIp(string Subject, string MessageBody, string[] AttachedDocs, string Ip, string[] Receivers)
        {
            try
            {
                string to = "faani1786@gmail.com";
                string from = "umar.waqas.se@gmail.com";
                MailMessage message = new MailMessage(from, to);
                message.Subject = "Using the new SMTP client.";
                message.Body = @"Using this new feature, you can send an email message from an application very easily.";
                SmtpClient client = new SmtpClient(Ip);
                // Credentials are necessary if the server requires the client
                // to authenticate before it will send email on the client's behalf.
                client.UseDefaultCredentials = true;
                client.Send(message);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static bool IsInternetAvailable()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("http://clients3.google.com/generate_204"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        public static bool IsJson(string jsonData)
        {
            return jsonData.Trim().Substring(0, 1).IndexOfAny(new[] { '[', '{' }) == 0;
        }

        public static void SetComboDataSource(List<SelectListVM> DataSource, ComboBox ComboBoxControl, string DefaultText)
        {
            DataSource.Insert(0, new SelectListVM { Value = 0, Text = DefaultText });
            ComboBoxControl.DataSource = DataSource;
            ComboBoxControl.ValueMember = "Value";
            ComboBoxControl.DisplayMember = "Text";
        }

        public static string GetConnectionString()
        {
            //var conStr = ConfigurationManager.ConnectionStrings["AppDbContext"].ConnectionString;
            //return conStr;
            if (SharedVariables.UseLocalDb)
            {
                return ConfigurationManager.ConnectionStrings["PharmacyCon_Local"].ConnectionString;
            }
            var conStr = SqlConnectionStringBuilder();
            return conStr;
        }
     
        public static string SqlConnectionStringBuilder()
        {
           
            if (SharedVariables.UseLocalDb)
            {
                return ConfigurationManager.ConnectionStrings["PharmacyCon_Local"].ConnectionString;
            }
                        
            string EncryptedConStr = Environment.GetEnvironmentVariable("GK_RESTAURANT");
           
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder(EncryptedConStr);
            //string DataSource = Utilities.CryptopEngine.Decrypt(sqlBuilder.DataSource, Shared.Salt);
            string InitialCatalog = CryptopEngine.Decrypt(sqlBuilder.InitialCatalog, GlobalSharing.Salt);
            string UserId = CryptopEngine.Decrypt(sqlBuilder.UserID, GlobalSharing.Salt);
            string Password = CryptopEngine.Decrypt(sqlBuilder.Password, GlobalSharing.Salt);
            //string DataSource = "192.168.10.4,1433"; // use this connection string in a build where remote sql access is required.
            //string DataSource = Environment.MachineName.ToString() + @"\SQLEXPRESS";
            string DataSource = sqlBuilder.DataSource;
            sqlBuilder.InitialCatalog = InitialCatalog;
            sqlBuilder.UserID = UserId;
            sqlBuilder.Password = Password;
            sqlBuilder.DataSource = DataSource;
            sqlBuilder.IntegratedSecurity = false;
            sqlBuilder.PersistSecurityInfo = false;
            string Decrypted = sqlBuilder.ConnectionString.ToString();
            return sqlBuilder.ConnectionString;
        }


        //public static bool IsDayClosed()
        //{
        //    bool closed = false;
        //    if (SharedVariables.AdminPharmacySetting != null)
        //    {
        //        if (!SharedVariables.AdminPharmacySetting.EnforceDayClose)
        //        {
        //            if (DateTime.Now.TimeOfDay <= SharedVariables.AdminPharmacySetting.DayCloseTime.Value.TimeOfDay)
        //            {
        //                returDate = DateTime.Now.AddDays(-1);
        //            }
        //        }
        //    }
        //    return returDate;
        //}

        public static void CloseAllOpenedForms()
        {
            List<Form> openForms = new List<Form>();

            foreach (Form f in Application.OpenForms)
                openForms.Add(f);

            foreach (Form f in openForms)
            {
                if (f.Name != "frmMain")
                    f.Close();
            }
        }
    }
}