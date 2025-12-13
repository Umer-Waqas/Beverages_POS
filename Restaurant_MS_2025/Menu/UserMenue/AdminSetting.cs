

using Restaurant_MS_UI;

namespace Restaurant_MS_UI.App.MenuBar.UserMenue
{
    public partial class AdminSetting : Form
    {
        UnitOfWork unitOfWork;

        int InvoiceSettingId = 0;
        int ProcedureInvoiceSettingId = 0;
        long PractiseSettingId = 0;
        int PrintFormatSettingId = 0;
        int PharmacySettingId = 0;
        int ShiftMasterSettingId = 0;
        public AdminSetting()
        {
            InitializeComponent();
        }

        private void AdminSetting_Load(object sender, EventArgs e)
        {
            tcAdminSetting.TabPages.Remove(tpProcedureInvoiceSetting);// temporary removing procedure invoice as its not required.
            tcAdminSetting.TabPages.Remove(tpPrintSetting);// temporary removing procedure invoice as its not required.
            LoadAdminInvoiceSetting();
            LoadAdminProcedureInvoiceSetting();
            LoadAdminPractiseSetting();
            LoadAdminPrintFormatSetting();
            LoadAdminPharmacySetting();
            LoadAdminShiftsMasterSetting();
            LoadAdminShiftsSetting();
        }

        private void LoadAdminInvoiceSetting()
        {
            try
            {
                if (SharedVariables.AdminInvoiceSetting != null)
                {
                    LoadInvoiceSetting(SharedVariables.AdminInvoiceSetting);
                }
                //if(SharedVariables.AdminPractiseSetting!=null)
                //{
                //    LoadPractiseSetting(SharedVariables.AdminPractiseSetting);
                //}
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Loading Invoice Settings, Please Try Again.", ex.Message, "Failed to Load Setting");
            }
        }
        private void LoadAdminProcedureInvoiceSetting()
        {
            try
            {
                if (SharedVariables.AdminProcedureInvoiceSetting != null)
                {
                    LoadProcedureInvoiceSetting(SharedVariables.AdminProcedureInvoiceSetting);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Loading Invoice Settings, Please Try Again.", ex.Message, "Failed to Load Setting");
            }
        }
        private void LoadAdminPractiseSetting()
        {
            try
            {
                if (SharedVariables.AdminPractiseSetting != null)
                {
                    LoadPractiseSetting(SharedVariables.AdminPractiseSetting);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Loading Invoice Settings, Please Try Again.", ex.Message, "Failed to Load Setting");
            }
        }
        private void LoadAdminPrintFormatSetting()
        {
            try
            {
                if (SharedVariables.AdminPrintFormatSetting != null)
                {
                    LoadPrintFormatSetting(SharedVariables.AdminPrintFormatSetting);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Loading Invoice Settings, Please Try Again.", ex.Message, "Failed to Load Setting");
            }
        }
        private void LoadAdminPharmacySetting()
        {
            try
            {
                if (SharedVariables.AdminPharmacySetting != null)
                {
                    LoadPharmacySetting(SharedVariables.AdminPharmacySetting);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Loading Invoice Settings, Please Try Again.", ex.Message, "Failed to Load Setting");
            }
        }

        private void LoadAdminShiftsMasterSetting()
        {
            try
            {
                if (SharedVariables.AdminShiftSettings != null)
                {
                    LoadShiftMasterSetting(SharedVariables.AdminShiftMasterSetting);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Loading Invoice Settings, Please Try Again.", ex.Message, "Failed to Load Setting");
            }
        }


        private void LoadAdminShiftsSetting()
        {
            try
            {
                if (SharedVariables.AdminShiftSettings != null)
                {
                    LoadShiftSetting(SharedVariables.AdminShiftSettings);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Occurred While Loading Invoice Settings, Please Try Again.", ex.Message, "Failed to Load Setting");
            }
        }

        private void LoadInvoiceSetting(AdminInvoiceSetting Setting)
        {
            // un register this event here, so at load time checkChanges should not be called, this is showing confirmation dialog which is not required at load time.
            chkOptionalBatchNo.CheckedChanged -= chkOptionalBatchNo_CheckedChanged;
            InvoiceSettingId = Setting.AdminInvoiceSettingId;
            if (Setting.PrintPageSize == (int)SharedVariables.PrintPageSize.A4)
            {
                rbA4.Checked = true;
            }
            else if (Setting.PrintPageSize == (int)SharedVariables.PrintPageSize.A8)
            {
                rbA8.Checked = true;
            }
            else if (Setting.PrintPageSize == (int)SharedVariables.PrintPageSize.A5)
            {
                rbA5.Checked = true;
            }
            numMarginTop.Value = Setting.MarginTop;
            numMarginLeft.Value = Setting.MarginLeft;
            numMarginRight.Value = Setting.MarginRight;
            numMarginBottom.Value = Setting.MarginBottom;
            if (Setting.FontSize == 15) { rbFs15.Checked = true; }
            else if (Setting.FontSize == 13) { rbFs13.Checked = true; }
            else if (Setting.FontSize == 11) { rbFs11.Checked = true; }
            else if (Setting.FontSize == 9) { rbFs9.Checked = true; }
            else if (Setting.FontSize == 7) { rbFs7.Checked = true; }
            if (Setting.PageType == (int)SharedVariables.PageType.PlainPaper) { rbPlainPaper.Checked = true; }
            else if (Setting.PageType == (int)SharedVariables.PageType.LetterHead) { rbLetterHead.Checked = true; }
            if (Setting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait) { rbPortrait.Checked = true; }
            else if (Setting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape) { rbLandscape.Checked = true; }

            if (Setting.ShowUserName) { chkShowUserName.Checked = true; }
            else { chkShowUserName.Checked = false; }

            if (Setting.ShowLogoWaterMark) { chkShowLogoWaterMark.Checked = true; }
            else { chkShowLogoWaterMark.Checked = false; }

            if (Setting.HideRateAndQuantityInPrintFormat) { chkHideRateAndQty.Checked = true; }
            else { chkHideRateAndQty.Checked = false; }

            if (Setting.ShowGrandtotalsInWords) { chkShowGTInWords.Checked = true; }
            else { chkShowGTInWords.Checked = false; }

            if (Setting.GrandTotalsOfInvoiceAsPaymentByDefault) { chkGrndTotalofInvAsPmt.Checked = true; }
            else { chkGrndTotalofInvAsPmt.Checked = false; }

            if (Setting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts) { chkHideDuesAndAdvance.Checked = true; }
            else { chkHideDuesAndAdvance.Checked = false; }

            chkShoPharmaName.Checked = Setting.PrintPractiseName;
            chkshowPharmaPhone.Checked = Setting.ShowPharmaPhone;
            chkShowPharmaAddress.Checked = Setting.ShowPharmaAddress;

            chkOptionalBatchNo.Checked = Setting.IsOptionalBatchNo;
            txtInvoiceNote.Text = Setting.InvoiceNote;
            chkAllowBelowCostSale.Checked = Setting.AllowBelowCostSale;
            chkOptionalBatchNo.CheckedChanged += chkOptionalBatchNo_CheckedChanged;
            chkAskLogin.Checked = Setting.IsAskLoginOnInvSave;
            txtPrinterName.Text = Setting.PrinterName;

            //patient details
            chkShowGend.Checked = Setting.ShowGender;
            chkShowPhone.Checked = Setting.ShowPhone;
            chkShowMR.Checked = Setting.ShowMR;
            chkShowEmail.Checked = Setting.ShowEmail;
            chkShowDOB.Checked = Setting.ShowDOB;
            chkShowAge.Checked = Setting.ShowAge;

            chkShowBonusQty.Checked = Setting.ShowBonusQty;
            chkShowSalesTax.Checked = Setting.ShowSalesTax;

            if (Setting.InvoiceLayout == 1) rb_Layout_1.Checked = true;
            else if (Setting.InvoiceLayout == 2) rb_Layout_2.Checked = true;
            else if (Setting.InvoiceLayout == 3) rb_Layout_3.Checked = true;
            else if (Setting.InvoiceLayout == 4) rb_Layout_4.Checked = true;
            else if (Setting.InvoiceLayout == 5) rb_Layout_5.Checked = true;
            else if (Setting.InvoiceLayout == 6) rb_Layout_6.Checked = true;
            else if (Setting.InvoiceLayout == 7) rb_Layout_7.Checked = true;
            if (Setting.A4_cols_format == 1) rb_col_format_1.Checked = true;
            else if (Setting.A4_cols_format == 2) rb_col_format_2.Checked = true;
        }
        private void LoadProcedureInvoiceSetting(AdminProcedureInvoiceSetting Setting)
        {
            ProcedureInvoiceSettingId = Setting.AdminProcedureInvoiceSettingId;
            if (Setting.PrintPageSize == (int)SharedVariables.PrintPageSize.A4)
            {
                rbA4_Proc.Checked = true;
            }
            else if (Setting.PrintPageSize == (int)SharedVariables.PrintPageSize.A8)
            {
                rbA8_Proc.Checked = true;
            }
            else if (Setting.PrintPageSize == (int)SharedVariables.PrintPageSize.A5)
            {
                rbA5_Proc.Checked = true;
            }
            numMarginTop_Proc.Value = Setting.MarginTop;
            numMarginLeft_Proc.Value = Setting.MarginLeft;
            numMarginRight_Proc.Value = Setting.MarginRight;
            numMarginBottom_Proc.Value = Setting.MarginBottom;
            if (Setting.FontSize == 15) { rbFs15_Proc.Checked = true; }
            else if (Setting.FontSize == 13) { rbFs13_Proc.Checked = true; }
            else if (Setting.FontSize == 11) { rbFs11_Proc.Checked = true; }
            else if (Setting.FontSize == 9) { rbFs9_Proc.Checked = true; }
            else if (Setting.FontSize == 7) { rbFs7_Proc.Checked = true; }
            if (Setting.PageType == (int)SharedVariables.PageType.PlainPaper) { rbPlainPaper_Proc.Checked = true; }
            else if (Setting.PageType == (int)SharedVariables.PageType.LetterHead) { rbLetterHead_Proc.Checked = true; }
            if (Setting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait) { rbPortrait_Proc.Checked = true; }
            else if (Setting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape) { rbLandscape_Proc.Checked = true; }

            if (Setting.ShowUserName) { chkShowUserName_Proc.Checked = true; }
            else { chkShowUserName_Proc.Checked = false; }

            if (Setting.ShowLogoWaterMark) { chkShowLogoWaterMark_Proc.Checked = true; }
            else { chkShowLogoWaterMark_Proc.Checked = false; }

            if (Setting.HideRateAndQuantityInPrintFormat) { chkHideRateAndQty_Proc.Checked = true; }
            else { chkHideRateAndQty_Proc.Checked = false; }

            if (Setting.ShowGrandtotalsInWords) { chkShowGTInWords_Proc.Checked = true; }
            else { chkShowGTInWords_Proc.Checked = false; }

            if (Setting.GrandTotalsOfInvoiceAsPaymentByDefault) { chkGrndTotalofInvAsPmt_Proc.Checked = true; }
            else { chkGrndTotalofInvAsPmt_Proc.Checked = false; }

            if (Setting.HidePaymentsAndDuesFromInvoiceAndPmntReceipts) { chkHideDuesAndAdvance_Proc.Checked = true; }
            else { chkHideDuesAndAdvance_Proc.Checked = false; }
            txtInvoiceNote_Proc.Text = Setting.InvoiceNote;
        }
        private void LoadPractiseSetting(AdminPractiseSetting setting)
        {
            this.PractiseSettingId = setting.AdminPractiseSettingId;
            txtPractiseName.Text = setting.Name;
            txtPractisePhone.Text = setting.Phone;
            txtPractiseAddress.Text = setting.Address;
            if (!string.IsNullOrEmpty(setting.LogoPath))
            {
                try
                {

                    Image img = Image.FromFile(setting.LogoPath);
                    pbLogo.Image = img;
                    lblImagePath.Text = setting.LogoPath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not find attached logo file, it has been moved or removed.", "Logo Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void LoadPrintFormatSetting(AdminPrintFormatSetting setting)
        {
            this.PrintFormatSettingId = setting.AdminPrintFormatSettingId;
            if (setting.HeaderFooterPref == 1)
            {
                rbHf1.Checked = true;
            }
            else if (setting.HeaderFooterPref == 2)
            {
                rbHf2.Checked = true;
            }
            else if (setting.HeaderFooterPref == 3)
            {
                rbHf3.Checked = true;
            }
            else if (setting.HeaderFooterPref == 4)
            {
                rbHf4.Checked = true;
            }
            else if (setting.HeaderFooterPref == 5)
            {
                rbHf5.Checked = true;
            }
            else if (setting.HeaderFooterPref == 6)
            {
                rbHf6.Checked = true;
            }

            // patient details selection
            if (setting.PatientDetailsPref == 1)
            {
                rbPd1.Checked = true;
            }
            else if (setting.PatientDetailsPref == 2)
            {
                rbPd2.Checked = true;
            }
            if (setting.PatientDetailsPref == 3)
            {
                rbPd3.Checked = true;
            }
            if (setting.PatientDetailsPref == 4)
            {
                rbPd4.Checked = true;
            }
            if (setting.PatientDetailsPref == 5)
            {
                rbPd5.Checked = true;
            }
            if (setting.PatientDetailsPref == 6)
            {
                rbPd6.Checked = true;
            }
        }
        private void LoadPharmacySetting(AdminPharmacySetting setting)
        {
            this.PharmacySettingId = setting.AdminPharmacySettingId;
            if (setting.IsItemConumptionFifo)
            {
                rbFifo.Checked = true;
            }
            chkAllowNegCons.Checked = setting.AllowNegCons;
            if (!setting.IsItemConumptionFifo)
            {
                rbLifo.Checked = true;
            }
            chkHoldStockOnInvHold.Checked = setting.IsHolsStockOnInvoiceHold;
            chkUseNewestPrice.Checked = setting.IsUseNewestStockPrice;
            chkItemDefUnitPOS.Checked = setting.IsItemDefUnitOnPOS;
            numExpPeriod.Value = setting.ExpiryPeriod;
            cmbExpPeriodUnit.SelectedIndex = setting.ExpiryPeriodUnit;
            chkShowRackNoInPOS.Checked = setting.ShowRackNoInPOS;

            chkEnableDayClose.Checked = setting.EnableDayClose;
            grpDayCloseSettings.Enabled = setting.EnableDayClose;
            chkUseDefaultSetting.Checked = setting.UseDafaultStoreClosingSetting;
            chkEnforceDayClose.Checked = setting.EnforceDayClose;
            dayOpenTime.Value = setting.DayOpenTime.Value;
            dayCloseTime.Value = setting.DayCloseTime.Value;

            chkAllowBatchOnAddStock.Checked = setting.AllowBatchEntryOnAddStock;
            chkShowBacthOnPOS.Checked = setting.ShowBatchNoOnPOS;
            chkEnableFBRIntegration.Checked = setting.EnableFBRIntegration;

            if (chkUseDefaultSetting.Checked)
            {
                dayOpenTime.Enabled = false;
                dayCloseTime.Enabled = false;
            }
        }

        private void LoadShiftMasterSetting(AdminShiftMasterSetting setting)
        {
            this.ShiftMasterSettingId = setting.AdminShiftMasterSettingId;
            chkEnableShifts.Checked = setting.ShiftsEnabled;
            chkEnforceLogout.Checked = setting.EnforceLogout;
        }

        private void LoadShiftSetting(List<AdminShiftSetting> setting)
        {
            foreach (var s in setting)
            {
                if (s.Code == 1)
                {
                    StTimeMorn.Value = s.StartTime;
                    EnTimeMorn.Value = s.EndTime;
                    chkMorn.Checked = s.IsActive;
                    continue;
                }

                if (s.Code == 2)
                {
                    StTimeAftNoon.Value = s.StartTime;
                    enTimeAftNoon.Value = s.EndTime;
                    chkAftNoon.Checked = s.IsActive;
                }

                if (s.Code == 3)
                {
                    StTimeEve.Value = s.StartTime;
                    enTimeEve.Value = s.EndTime;
                    chkEve.Checked = s.IsActive;
                }


                if (s.Code == 4)
                {
                    StTimeNt.Value = s.StartTime;
                    enTimeNt.Value = s.EndTime;
                    chkNt.Checked = s.IsActive;
                }
            }
        }
        private void AdjustControls(object sender)
        {
            RadioButton SelectedOption = (RadioButton)sender;
            if (SelectedOption == rbA4)
            {
                pnlPageSetting.Visible = true;
                //Point OrgLoc = new Point(8, 208);
                //pnlInvoiceSetting.Location = OrgLoc;
            }
            else if (SelectedOption == rbA8)
            {
                pnlPageSetting.Visible = false;
                int x = pnlPageSetting.Location.X;
                int y = pnlPageSetting.Location.Y;
                //pnlInvoiceSetting.Location = new Point(x, y);
            }
            else if (SelectedOption == rbA5)
            {
                pnlPageSetting.Visible = true;
                //Point OrgLoc = new Point(8, 208);
                //pnlInvoiceSetting.Location = OrgLoc;
            }
        }

        private void rbA8_CheckedChanged(object sender, EventArgs e)
        {
            if (rbA8.Checked)
            {
                AdjustControls(sender);
            }
        }

        private void rbA4_CheckedChanged(object sender, EventArgs e)
        {
            if (rbA4.Checked)
            {
                AdjustControls(sender);
            }
        }

        private void rbA5_CheckedChanged(object sender, EventArgs e)
        {
            if (rbA5.Checked)
            {
                AdjustControls(sender);
            }
        }

        private void SaveSetting()
        {
            try
            {
                AdminInvoiceSetting setting = new AdminInvoiceSetting();
                if (rbA4.Checked)
                {
                    setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A4;
                }
                else if (rbA8.Checked)
                {
                    setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A8;
                }
                else if (rbA5.Checked)
                {
                    setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A5;
                }
                setting.MarginTop = (int)numMarginTop.Value;
                setting.MarginLeft = (int)numMarginLeft.Value;
                setting.MarginRight = (int)numMarginRight.Value;
                setting.MarginBottom = (int)numMarginBottom.Value;
                if (rbFs15.Checked) { setting.FontSize = 15; }
                else if (rbFs13.Checked) { setting.FontSize = 13; }
                else if (rbFs11.Checked) { setting.FontSize = 11; }
                else if (rbFs9.Checked) { setting.FontSize = 9; }
                else if (rbFs7.Checked) { setting.FontSize = 7; }
                if (rbPlainPaper.Checked) { setting.PageType = (int)SharedVariables.PageType.PlainPaper; }
                else if (rbLetterHead.Checked) { setting.PageType = (int)SharedVariables.PageType.LetterHead; }
                if (rbPortrait.Checked) { setting.PageOrientation = (int)SharedVariables.PageOrientation.Portrait; }
                else if (rbLandscape.Checked) { setting.PageOrientation = (int)SharedVariables.PageOrientation.LandScape; }
                setting.ShowUserName = chkShowUserName.Checked;
                setting.ShowLogoWaterMark = chkShowLogoWaterMark.Checked;
                setting.HideRateAndQuantityInPrintFormat = chkHideRateAndQty.Checked;
                setting.ShowGrandtotalsInWords = chkShowGTInWords.Checked;
                setting.GrandTotalsOfInvoiceAsPaymentByDefault = chkGrndTotalofInvAsPmt.Checked;
                setting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts = chkHideDuesAndAdvance.Checked;
                setting.InvoiceNote = txtInvoiceNote.Text.Trim();

                setting.PrintPractiseName = chkShoPharmaName.Checked;
                setting.ShowPharmaPhone = chkshowPharmaPhone.Checked;
                setting.ShowPharmaAddress = chkShowPharmaAddress.Checked;

                setting.IsOptionalBatchNo = chkOptionalBatchNo.Checked;
                setting.AllowBelowCostSale = chkAllowBelowCostSale.Checked;
                setting.IsAskLoginOnInvSave = chkAskLogin.Checked;
                setting.PrinterName = txtPrinterName.Text;

                setting.ShowGender = chkShowGend.Checked;
                setting.ShowPhone = chkShowPhone.Checked;
                setting.ShowMR = chkShowMR.Checked;
                setting.ShowEmail = chkShowEmail.Checked;
                setting.ShowDOB = chkShowDOB.Checked;
                setting.ShowAge = chkShowAge.Checked;

                setting.ShowBonusQty = chkShowBonusQty.Checked;
                setting.ShowSalesTax = chkShowSalesTax.Checked;
                setting.InvoiceLayout = rb_Layout_1.Checked ? 1 : rb_Layout_2.Checked ? 2 : rb_Layout_3.Checked ? 3 : rb_Layout_4.Checked ? 4 : rb_Layout_5.Checked ? 5 : rb_Layout_6.Checked ? 6 : 7;
                setting.A4_cols_format = rb_col_format_1.Checked ? 1 : 2;

                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.AdminInvoiceSettingRepository.Insert(setting);
                    unitOfWork.Save();
                }
                SharedVariables.AdminInvoiceSetting = setting;
                //MessageBox.Show("Invoice Settings Saved Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Saving Invoice Setting.", ex.Message, "Failed");
            }
        }
        private void AddProcedureInvoiceSetting()
        {
            try
            {
                AdminProcedureInvoiceSetting setting = new AdminProcedureInvoiceSetting();
                if (rbA4_Proc.Checked)
                {
                    setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A4;
                }
                else if (rbA8_Proc.Checked)
                {
                    setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A8;
                }
                else if (rbA5_Proc.Checked)
                {
                    setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A5;
                }
                setting.MarginTop = (int)numMarginTop_Proc.Value;
                setting.MarginLeft = (int)numMarginLeft_Proc.Value;
                setting.MarginRight = (int)numMarginRight_Proc.Value;
                setting.MarginBottom = (int)numMarginBottom_Proc.Value;
                if (rbFs15_Proc.Checked) { setting.FontSize = 15; }
                else if (rbFs13_Proc.Checked) { setting.FontSize = 13; }
                else if (rbFs11_Proc.Checked) { setting.FontSize = 11; }
                else if (rbFs9_Proc.Checked) { setting.FontSize = 9; }
                else if (rbFs7_Proc.Checked) { setting.FontSize = 7; }
                if (rbPlainPaper_Proc.Checked) { setting.PageType = (int)SharedVariables.PageType.PlainPaper; }
                else if (rbLetterHead_Proc.Checked) { setting.PageType = (int)SharedVariables.PageType.LetterHead; }
                if (rbPortrait_Proc.Checked) { setting.PageOrientation = (int)SharedVariables.PageOrientation.Portrait; }
                else if (rbLandscape_Proc.Checked) { setting.PageOrientation = (int)SharedVariables.PageOrientation.LandScape; }
                setting.ShowUserName = chkShowUserName_Proc.Checked;
                setting.ShowLogoWaterMark = chkShowLogoWaterMark_Proc.Checked;
                setting.HideRateAndQuantityInPrintFormat = chkHideRateAndQty_Proc.Checked;
                setting.ShowGrandtotalsInWords = chkShowGTInWords_Proc.Checked;
                setting.GrandTotalsOfInvoiceAsPaymentByDefault = chkGrndTotalofInvAsPmt_Proc.Checked;
                setting.HidePaymentsAndDuesFromInvoiceAndPmntReceipts = chkHideDuesAndAdvance_Proc.Checked;
                setting.PrintPractiseName = chkPrintPractiseName_Proc.Checked;
                setting.InvoiceNote = txtInvoiceNote_Proc.Text.Trim();
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.AdminProcedureInvoiceSettingRepository.Insert(setting);
                    unitOfWork.Save();
                }
                SharedVariables.AdminProcedureInvoiceSetting = setting;
                //MessageBox.Show("Invoice Settings Saved Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Saving Procedure Invoice Setting.", ex.Message, "Failed");
            }
        }

        private void UpdateSetting()
        {
            try
            {
                AdminInvoiceSetting setting;
                using (unitOfWork = new UnitOfWork())
                {
                    setting = unitOfWork.AdminInvoiceSettingRepository.GetById(InvoiceSettingId);
                    if (rbA4.Checked)
                    {
                        setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A4;
                    }
                    else if (rbA8.Checked)
                    {
                        setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A8;
                    }
                    else if (rbA5.Checked)
                    {
                        setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A5;
                    }
                    setting.MarginTop = (int)numMarginTop.Value;
                    setting.MarginLeft = (int)numMarginLeft.Value;
                    setting.MarginRight = (int)numMarginRight.Value;
                    setting.MarginBottom = (int)numMarginBottom.Value;
                    if (rbFs15.Checked) { setting.FontSize = 15; }
                    else if (rbFs13.Checked) { setting.FontSize = 13; }
                    else if (rbFs11.Checked) { setting.FontSize = 11; }
                    else if (rbFs9.Checked) { setting.FontSize = 9; }
                    else if (rbFs7.Checked) { setting.FontSize = 7; }
                    if (rbPlainPaper.Checked) { setting.PageType = (int)SharedVariables.PageType.PlainPaper; }
                    else if (rbLetterHead.Checked) { setting.PageType = (int)SharedVariables.PageType.LetterHead; }
                    if (rbPortrait.Checked) { setting.PageOrientation = (int)SharedVariables.PageOrientation.Portrait; }
                    else if (rbLandscape.Checked) { setting.PageOrientation = (int)SharedVariables.PageOrientation.LandScape; }
                    setting.ShowUserName = chkShowUserName.Checked;
                    setting.ShowLogoWaterMark = chkShowLogoWaterMark.Checked;
                    setting.HideRateAndQuantityInPrintFormat = chkHideRateAndQty.Checked;
                    setting.ShowGrandtotalsInWords = chkShowGTInWords.Checked;
                    setting.GrandTotalsOfInvoiceAsPaymentByDefault = chkGrndTotalofInvAsPmt.Checked;
                    setting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts = chkHideDuesAndAdvance.Checked;

                    setting.PrintPractiseName = chkShoPharmaName.Checked;
                    setting.ShowPharmaPhone = chkshowPharmaPhone.Checked;
                    setting.ShowPharmaAddress = chkShowPharmaAddress.Checked;

                    setting.InvoiceNote = txtInvoiceNote.Text.Trim();
                    setting.IsOptionalBatchNo = chkOptionalBatchNo.Checked;
                    setting.AllowBelowCostSale = chkAllowBelowCostSale.Checked;
                    setting.IsAskLoginOnInvSave = chkAskLogin.Checked;
                    setting.PrinterName = txtPrinterName.Text;

                    setting.ShowGender = chkShowGend.Checked;
                    setting.ShowPhone = chkShowPhone.Checked;
                    setting.ShowMR = chkShowMR.Checked;
                    setting.ShowEmail = chkShowEmail.Checked;
                    setting.ShowDOB = chkShowDOB.Checked;
                    setting.ShowAge = chkShowAge.Checked;

                    setting.ShowBonusQty = chkShowBonusQty.Checked;
                    setting.ShowSalesTax = chkShowSalesTax.Checked;
                    setting.InvoiceLayout = rb_Layout_1.Checked ? 1 : rb_Layout_2.Checked ? 2 : rb_Layout_3.Checked ? 3 : rb_Layout_4.Checked ? 4 : rb_Layout_5.Checked ? 5 : rb_Layout_6.Checked ? 6 : 7;
                    setting.A4_cols_format = rb_col_format_1.Checked ? 1 : 2;
                    unitOfWork.AdminInvoiceSettingRepository.Update(setting);
                    unitOfWork.Save();
                    SharedVariables.AdminInvoiceSetting = setting; // update shared value
                    //MessageBox.Show("Invoice Settings Updated Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Saving Invoice Setting.", ex.Message, "Failed");
            }
        }
        private void UpdateProcedureInvoiceSetting()
        {
            try
            {
                AdminProcedureInvoiceSetting setting;
                using (unitOfWork = new UnitOfWork())
                {
                    setting = unitOfWork.AdminProcedureInvoiceSettingRepository.GetById(InvoiceSettingId);
                    if (rbA4_Proc.Checked)
                    {
                        setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A4;
                    }
                    else if (rbA8_Proc.Checked)
                    {
                        setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A8;
                    }
                    else if (rbA5_Proc.Checked)
                    {
                        setting.PrintPageSize = (int)SharedVariables.PrintPageSize.A5;
                    }
                    setting.MarginTop = (int)numMarginTop_Proc.Value;
                    setting.MarginLeft = (int)numMarginLeft_Proc.Value;
                    setting.MarginRight = (int)numMarginRight_Proc.Value;
                    setting.MarginBottom = (int)numMarginBottom_Proc.Value;
                    if (rbFs15_Proc.Checked) { setting.FontSize = 15; }
                    else if (rbFs13_Proc.Checked) { setting.FontSize = 13; }
                    else if (rbFs11_Proc.Checked) { setting.FontSize = 11; }
                    else if (rbFs9_Proc.Checked) { setting.FontSize = 9; }
                    else if (rbFs7_Proc.Checked) { setting.FontSize = 7; }
                    if (rbPlainPaper_Proc.Checked) { setting.PageType = (int)SharedVariables.PageType.PlainPaper; }
                    else if (rbLetterHead_Proc.Checked) { setting.PageType = (int)SharedVariables.PageType.LetterHead; }
                    if (rbPortrait_Proc.Checked) { setting.PageOrientation = (int)SharedVariables.PageOrientation.Portrait; }
                    else if (rbLandscape_Proc.Checked) { setting.PageOrientation = (int)SharedVariables.PageOrientation.LandScape; }
                    setting.ShowUserName = chkShowUserName_Proc.Checked;
                    setting.ShowLogoWaterMark = chkShowLogoWaterMark_Proc.Checked;
                    setting.HideRateAndQuantityInPrintFormat = chkHideRateAndQty_Proc.Checked;
                    setting.ShowGrandtotalsInWords = chkShowGTInWords_Proc.Checked;
                    setting.GrandTotalsOfInvoiceAsPaymentByDefault = chkGrndTotalofInvAsPmt_Proc.Checked;
                    setting.HidePaymentsAndDuesFromInvoiceAndPmntReceipts = chkHideDuesAndAdvance_Proc.Checked;
                    setting.PrintPractiseName = chkPrintPractiseName_Proc.Checked;
                    setting.InvoiceNote = txtInvoiceNote_Proc.Text.Trim();
                    unitOfWork.AdminProcedureInvoiceSettingRepository.Update(setting);
                    unitOfWork.Save();
                    SharedVariables.AdminProcedureInvoiceSetting = setting; // update shared value
                    //MessageBox.Show("Invoice Settings Updated Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Saving Procedure Invoice Setting.", ex.Message, "Failed");
            }
        }

        private void SaveInvoiceSetting()
        {
            if (InvoiceSettingId > 0)
            {
                UpdateSetting();
            }
            else
            {
                SaveSetting();
            }
        }
        private void SaveProcedureInvoiceSetting()
        {
            if (ProcedureInvoiceSettingId > 0)
            {
                UpdateProcedureInvoiceSetting();
            }
            else
            {
                AddProcedureInvoiceSetting();
            }
        }

        private void SavePractiseSetting()
        {
            if (PractiseSettingId > 0)
            {
                UpdatePractiseSetting();
            }
            else
            {
                InsertPractiseSetting();
            }
        }

        private void UpdatePractiseSetting()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    AdminPractiseSetting obj = unitOfWork.AdminPractiseSettingRepository.GetById(this.PractiseSettingId);
                    if (obj != null)
                    {
                        obj.Name = txtPractiseName.Text.Trim();
                        obj.Phone = txtPractisePhone.Text;
                        obj.Address = txtPractiseAddress.Text;
                        obj.LogoPath = lblImagePath.Text.Trim().ToLower() == "no file choosen" ? "" : lblImagePath.Text.Trim().ToLower();
                        obj.Backgroundpath = lblBackgroundPath.Text.Trim().ToLower() == "no file choosen" ? "" : lblBackgroundPath.Text.Trim().ToLower();
                        unitOfWork.AdminPractiseSettingRepository.Update(obj);
                        unitOfWork.Save();
                        SharedVariables.AdminPractiseSetting = obj;
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Updating Practise Setting.", ex.Message, "Error");
            }
        }

        private void InsertPractiseSetting()
        {
            try
            {
                AdminPractiseSetting obj = new AdminPractiseSetting();
                obj.Name = txtPractiseName.Text.Trim();
                obj.Phone = txtPractisePhone.Text.Trim();
                obj.Address = txtPractiseAddress.Text.Trim();

                obj.LogoPath = lblImagePath.Text.Trim().ToLower() == "no file choosen" ? "" : lblImagePath.Text.Trim().ToLower();
                obj.Backgroundpath = lblBackgroundPath.Text.Trim().ToLower() == "no file choosen" ? "" : lblBackgroundPath.Text.Trim().ToLower();
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.AdminPractiseSettingRepository.Insert(obj);
                    unitOfWork.Save();
                    SharedVariables.AdminPractiseSetting = obj;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Saving Practise Setting.", ex.Message, "Error");
            }
        }

        private void UpdatePrintformatSetting()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    AdminPrintFormatSetting obj = unitOfWork.AdminPrintFormatSettingRepository.GetById(this.PrintFormatSettingId);
                    if (obj != null)
                    {
                        if (rbHf1.Checked)
                        {
                            obj.HeaderFooterPref = 1;
                        }
                        else if (rbHf2.Checked)
                        {
                            obj.HeaderFooterPref = 2;
                        }
                        else if (rbHf3.Checked)
                        {
                            obj.HeaderFooterPref = 3;
                        }
                        else if (rbHf4.Checked)
                        {
                            obj.HeaderFooterPref = 4;
                        }
                        if (rbHf5.Checked)
                        {
                            obj.HeaderFooterPref = 5;
                        }
                        if (rbHf6.Checked)
                        {
                            obj.HeaderFooterPref = 6;
                        }
                        // patient details setting
                        if (rbPd1.Checked)
                        {
                            obj.PatientDetailsPref = 1;
                        }
                        else if (rbPd2.Checked)
                        {
                            obj.PatientDetailsPref = 2;
                        }
                        else if (rbPd3.Checked)
                        {
                            obj.PatientDetailsPref = 3;
                        }
                        else if (rbPd4.Checked)
                        {
                            obj.PatientDetailsPref = 4;
                        }
                        else if (rbPd5.Checked)
                        {
                            obj.PatientDetailsPref = 5;
                        }
                        else if (rbPd6.Checked)
                        {
                            obj.PatientDetailsPref = 6;
                        }
                        using (unitOfWork = new UnitOfWork())
                        {
                            unitOfWork.AdminPrintFormatSettingRepository.Update(obj);
                            unitOfWork.Save();
                            SharedVariables.AdminPrintFormatSetting = obj;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Updating Print Format Setting.", ex.Message, "Error");
            }
        }
        private void UpdatePharmacySetting()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    AdminPharmacySetting obj = unitOfWork.AdminPharmacySettingRepository.GetById(this.PharmacySettingId);
                    if (obj != null)
                    {
                        obj.IsItemConumptionFifo = rbFifo.Checked ? true : false;
                        obj.IsPharmacyTemps = chkPharmacyTemps.Checked;
                        obj.AllowNegCons = chkAllowNegCons.Checked;
                        obj.IsHolsStockOnInvoiceHold = chkHoldStockOnInvHold.Checked;
                        obj.IsUseNewestStockPrice = chkUseNewestPrice.Checked;
                        obj.IsItemDefUnitOnPOS = chkItemDefUnitPOS.Checked;
                        obj.ExpiryPeriod = (int)numExpPeriod.Value;
                        obj.ExpiryPeriodUnit = (int)((SharedVariables.ExpiryPeriodUnit)cmbExpPeriodUnit.SelectedIndex); // could have been assigned simply index from dropdown, but for sake of understading applied cast to relevant Enum type;
                        obj.ShowRackNoInPOS = chkShowRackNoInPOS.Checked;

                        // day open/close setting
                        obj.EnableDayClose = chkEnableDayClose.Checked;
                        obj.UseDafaultStoreClosingSetting = chkUseDefaultSetting.Checked;
                        obj.EnforceDayClose = chkEnforceDayClose.Checked;
                        obj.DayOpenTime = dayOpenTime.Value;
                        obj.DayCloseTime = dayCloseTime.Value;

                        obj.ShowBatchNoOnPOS = chkShowBacthOnPOS.Checked;
                        obj.AllowBatchEntryOnAddStock = chkAllowBatchOnAddStock.Checked;
                        obj.EnableFBRIntegration = chkEnableFBRIntegration.Checked;
                        using (unitOfWork = new UnitOfWork())
                        {
                            unitOfWork.AdminPharmacySettingRepository.Update(obj);
                            unitOfWork.Save();
                            SharedVariables.AdminPharmacySetting = obj;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Updating Pharmacy Setting.", ex.Message, "Error");
            }
        }
        private void InsertPharmacySetting()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    AdminPharmacySetting obj = new AdminPharmacySetting();

                    obj.IsItemConumptionFifo = rbFifo.Checked ? true : false;
                    obj.IsPharmacyTemps = chkPharmacyTemps.Checked;
                    obj.AllowNegCons = chkAllowNegCons.Checked;
                    obj.IsHolsStockOnInvoiceHold = chkHoldStockOnInvHold.Checked;
                    obj.IsUseNewestStockPrice = chkUseNewestPrice.Checked;
                    obj.IsItemDefUnitOnPOS = chkItemDefUnitPOS.Checked;
                    obj.ExpiryPeriod = (int)numExpPeriod.Value;
                    obj.ExpiryPeriodUnit = (int)((SharedVariables.ExpiryPeriodUnit)cmbExpPeriodUnit.SelectedIndex); // could have been assigned simply index from dropdown, but for sake of understading applied cast to relevant Enum type;
                    obj.ShowRackNoInPOS = chkShowRackNoInPOS.Checked;


                    // day open/close setting
                    obj.EnableDayClose = chkEnableDayClose.Checked;
                    obj.UseDafaultStoreClosingSetting = chkUseDefaultSetting.Checked;
                    obj.EnforceDayClose = chkEnforceDayClose.Checked;
                    obj.DayOpenTime = dayOpenTime.Value;
                    obj.DayCloseTime = dayCloseTime.Value;
                    obj.ShowBatchNoOnPOS = chkShowBacthOnPOS.Checked;
                    obj.AllowBatchEntryOnAddStock = chkAllowBatchOnAddStock.Checked;
                    obj.EnableFBRIntegration = chkEnableFBRIntegration.Checked;
                    using (unitOfWork = new UnitOfWork())
                    {
                        unitOfWork.AdminPharmacySettingRepository.Insert(obj);
                        unitOfWork.Save();
                        SharedVariables.AdminPharmacySetting = obj;
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Updating Pharmacy Setting.", ex.Message, "Error");
            }
        }
        private void InsertPrintFormatSetting()
        {
            try
            {
                AdminPrintFormatSetting obj = new AdminPrintFormatSetting();
                if (rbHf1.Checked)
                {
                    obj.HeaderFooterPref = 1;
                }
                else if (rbHf2.Checked)
                {
                    obj.HeaderFooterPref = 2;
                }
                else if (rbHf3.Checked)
                {
                    obj.HeaderFooterPref = 3;
                }
                else if (rbHf4.Checked)
                {
                    obj.HeaderFooterPref = 4;
                }
                if (rbHf5.Checked)
                {
                    obj.HeaderFooterPref = 5;
                }
                if (rbHf6.Checked)
                {
                    obj.HeaderFooterPref = 6;
                }
                // patient details setting
                if (rbPd1.Checked)
                {
                    obj.PatientDetailsPref = 1;
                }
                else if (rbPd2.Checked)
                {
                    obj.PatientDetailsPref = 2;
                }
                else if (rbPd3.Checked)
                {
                    obj.PatientDetailsPref = 3;
                }
                else if (rbPd4.Checked)
                {
                    obj.PatientDetailsPref = 4;
                }
                else if (rbPd5.Checked)
                {
                    obj.PatientDetailsPref = 5;
                }
                else if (rbPd6.Checked)
                {
                    obj.PatientDetailsPref = 6;
                }
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.AdminPrintFormatSettingRepository.Insert(obj);
                    unitOfWork.Save();
                    SharedVariables.AdminPrintFormatSetting = obj;
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Saving Print Format Setting.", ex.Message, "Error");
            }
        }

        private void InsertShiftMasterSetting()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    AdminShiftMasterSetting obj = new AdminShiftMasterSetting();

                    obj.ShiftsEnabled = chkEnableShifts.Checked;
                    obj.EnforceLogout = chkEnforceLogout.Checked;

                    using (unitOfWork = new UnitOfWork())
                    {
                        unitOfWork.AdminShiftMasterSettingRepository.Insert(obj);
                        unitOfWork.Save();
                        SharedVariables.AdminShiftMasterSetting = obj;
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Updating Pharmacy Setting.", ex.Message, "Error");
            }
        }
        private void UpdateShiftMasterSetting()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    AdminShiftMasterSetting obj = unitOfWork.AdminShiftMasterSettingRepository.GetById(this.ShiftMasterSettingId);
                    if (obj != null)
                    {
                        obj.ShiftsEnabled = chkEnableShifts.Checked;
                        obj.EnforceLogout = chkEnforceLogout.Checked;
                        using (unitOfWork = new UnitOfWork())
                        {
                            unitOfWork.AdminShiftMasterSettingRepository.Update(obj);
                            unitOfWork.Save();
                            SharedVariables.AdminShiftMasterSetting = obj;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Updating Shifts Setting.", ex.Message, "Error");
            }
        }


        private void UpdateShiftsSetting()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    List<AdminShiftSetting> obj = unitOfWork.AdminShiftsSettingRepository.GetAll().ToList();
                    if (obj != null)
                    {
                        foreach (var s in obj)
                        {
                            if (s.Code == 1)
                            {
                                s.StartTime = StTimeMorn.Value;
                                s.EndTime = EnTimeMorn.Value;
                                s.IsActive = chkMorn.Checked;
                            }

                            if (s.Code == 2)
                            {
                                s.StartTime = StTimeAftNoon.Value;
                                s.EndTime = enTimeAftNoon.Value;
                                s.IsActive = chkAftNoon.Checked;
                            }

                            if (s.Code == 3)
                            {
                                s.StartTime = StTimeEve.Value;
                                s.EndTime = enTimeEve.Value;
                                s.IsActive = chkEve.Checked;
                            }

                            if (s.Code == 4)
                            {
                                s.StartTime = StTimeNt.Value;
                                s.EndTime = enTimeNt.Value;
                                s.IsActive = chkNt.Checked;
                            }
                            unitOfWork.AdminShiftsSettingRepository.Update(s);
                        }

                        unitOfWork.Save();
                        SharedVariables.AdminShiftSettings = unitOfWork.AdminShiftsSettingRepository.GetAll().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An Error Ocurred While Updating Shifts Setting.", ex.Message, "Error");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                SavePractiseSetting();
                SaveInvoiceSetting();
                SaveProcedureInvoiceSetting();
                SavePrintFormatSetting();
                SavePharmacySetting();
                SaveShiftMasterSetting();
                SaveShiftsSetting();
                MessageBox.Show("Practise setting updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An error occurred while updating practise setting.", ex.Message, "Error");
            }
        }

        private void SavePrintFormatSetting()
        {
            if (this.PrintFormatSettingId > 0)
            {
                UpdatePrintformatSetting();
            }
            else
            {
                InsertPrintFormatSetting();
            }
        }
        private void SavePharmacySetting()
        {
            if (this.PharmacySettingId > 0)
            {
                UpdatePharmacySetting();
            }
            else
            {
                InsertPharmacySetting();
            }
        }

        private void SaveShiftMasterSetting()
        {
            if (this.ShiftMasterSettingId > 0)
            {
                UpdateShiftMasterSetting();
            }
            else
            {
                InsertShiftMasterSetting();
            }
        }

        private void SaveShiftsSetting()
        {
            UpdateShiftsSetting();
        }

        private void lblRemoveFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lblImagePath.Text = "No File Choosen";
        }

        private void btnAttachImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Select Invoice Image";
            fd.Filter = "Image Files|*.jpg;*.jpeg;";
            fd.CheckFileExists = true;
            fd.CheckPathExists = true;
            fd.Multiselect = false;
            fd.RestoreDirectory = true;
            DialogResult rs = fd.ShowDialog();
            if (rs == DialogResult.OK)
            {
                string selectedFile = fd.FileName;
                pbLogo.Image = Image.FromFile(selectedFile);
                string DestPath = SharedFunctions.CopyFileToLocal(selectedFile);
                lblImagePath.Text = DestPath;
            }
        }

        private void rbHf_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                foreach (Control other in pnlHeadersFooters.Controls)
                {
                    if (other is Panel)
                    {
                        foreach (Control ctr in other.Controls)
                        {
                            if (ctr.GetType() == typeof(RadioButton))
                            {
                                if (ctr == rb)
                                {
                                    continue;
                                }
                                ((RadioButton)ctr).Checked = false;
                            }
                        }
                    }
                }
            }
        }

        private void rbPd_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                foreach (Control other in pnlPatientDetails.Controls)
                {
                    if (other is Panel)
                    {
                        foreach (Control ctr in other.Controls)
                        {
                            if (ctr.GetType() == typeof(RadioButton))
                            {
                                if (ctr == rb)
                                {
                                    continue;
                                }
                                ((RadioButton)ctr).Checked = false;
                            }
                        }
                    }
                }
            }
        }

        private void chkOptionalBatchNo_CheckedChanged(object sender, EventArgs e)
        {
            if (chkOptionalBatchNo.Checked)
            {
                DialogResult res = MessageBox.Show("If any stock item has multiple batches, then on POS screen available quantity will be picked depending on FIFO/LIFO consumption setting. All other batches and there quantity will be hidden. you can revert this setting any time to show all batches and their stock on POS screen.", "Critical Alert", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (res != System.Windows.Forms.DialogResult.Yes)
                {
                    chkOptionalBatchNo.Checked = false;
                }
            }
        }

        private void chkEnableShifts_CheckedChanged(object sender, EventArgs e)
        {
            pnlShifts.Visible = chkEnableShifts.Checked;
        }

        private void chkEnforceLogout_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkEnableShifts.Checked)
            {
                chkEnforceLogout.Checked = false;
            }
        }

        private void btnAttachBackground_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Select Background Image";
            fd.Filter = "Image Files|*.jpg;*.jpeg;";
            fd.CheckFileExists = true;
            fd.CheckPathExists = true;
            fd.Multiselect = false;
            fd.RestoreDirectory = true;
            DialogResult rs = fd.ShowDialog();
            if (rs == DialogResult.OK)
            {
                string selectedFile = fd.FileName;
                pbBackground.Image = Image.FromFile(selectedFile);
                string DestPath = SharedFunctions.CopyFileToLocal(selectedFile);
                lblBackgroundPath.Text = DestPath;
            }
        }

        private void btnUpdatePractise_Click(object sender, EventArgs e)
        {
            try
            {
                SavePractiseSetting();
                SaveInvoiceSetting();
                SaveProcedureInvoiceSetting();
                SavePrintFormatSetting();
                SavePharmacySetting();
                SaveShiftMasterSetting();
                SaveShiftsSetting();
                MessageBox.Show("Practise setting updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("An error occurred while updating practise setting.", ex.Message, "Error");
            }
        }

        private void chkUseDefaultSetting_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseDefaultSetting.Checked)
            {
                dayOpenTime.Enabled = false;
                dayCloseTime.Enabled = false;
                chkEnforceDayClose.Checked = false;
                dayOpenTime.Value = new DateTime(2022, 01, 01, 00, 00, 00);
                dayCloseTime.Value = new DateTime(2022, 01, 01, 23, 59, 00);
            }
            else
            {
                chkEnforceDayClose.Checked = false;
                dayOpenTime.Enabled = true;
                dayCloseTime.Enabled = true;
            }
        }

        private void dayOpenTime_ValueChanged(object sender, EventArgs e)
        {

        }

        private void chkEnableDayClose_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableDayClose.Checked)
            {
                grpDayCloseSettings.Enabled = true;
            }
            else
            {
                grpDayCloseSettings.Enabled = false;
            }
        }

        private void Toggle_Layout_Events(bool register)
        {
            if (register)
            {
                rb_Layout_1.CheckedChanged += rb_Layout_1_CheckedChanged;
                rb_Layout_2.CheckedChanged += rb_Layout_2_CheckedChanged;
                rb_Layout_3.CheckedChanged += rb_Layout_3_CheckedChanged;
                rb_Layout_4.CheckedChanged += rb_Layout_4_CheckedChanged;
                rb_Layout_5.CheckedChanged += rb_Layout_5_CheckedChanged;
                rb_Layout_6.CheckedChanged += rb_Layout_6_CheckedChanged;
                rb_Layout_7.CheckedChanged += rb_Layout_7_CheckedChanged;
            }
            else
            {
                rb_Layout_1.CheckedChanged -= rb_Layout_1_CheckedChanged;
                rb_Layout_2.CheckedChanged -= rb_Layout_2_CheckedChanged;
                rb_Layout_3.CheckedChanged -= rb_Layout_3_CheckedChanged;
                rb_Layout_4.CheckedChanged -= rb_Layout_4_CheckedChanged;
                rb_Layout_5.CheckedChanged -= rb_Layout_5_CheckedChanged;
                rb_Layout_6.CheckedChanged -= rb_Layout_6_CheckedChanged;
                rb_Layout_7.CheckedChanged -= rb_Layout_7_CheckedChanged;
            }
        }

        private void Toggle_A4_Col_Format_Events(bool register)
        {
            if (register)
            {
                rb_col_format_1.CheckedChanged += rb_col_format_1_CheckedChanged;
                rb_col_format_2.CheckedChanged += rb_col_format_2_CheckedChanged;
            }
            else
            {
                rb_col_format_1.CheckedChanged -= rb_col_format_1_CheckedChanged;
                rb_col_format_2.CheckedChanged -= rb_col_format_2_CheckedChanged;
            }
        }

        private void rb_Layout_1_CheckedChanged(object sender, EventArgs e)
        {
            Toggle_Layout_Events(false);
            rb_Layout_2.Checked = rb_Layout_3.Checked = rb_Layout_4.Checked = rb_Layout_5.Checked = rb_Layout_6.Checked = rb_Layout_7.Checked = false;
            rb_Layout_1.Checked = true;
            Toggle_Layout_Events(true);
        }

        private void rb_Layout_2_CheckedChanged(object sender, EventArgs e)
        {
            Toggle_Layout_Events(false);
            rb_Layout_1.Checked = rb_Layout_3.Checked = rb_Layout_4.Checked = rb_Layout_5.Checked = rb_Layout_6.Checked = rb_Layout_7.Checked = false;
            rb_Layout_2.Checked = true;
            Toggle_Layout_Events(true);
        }

        private void rb_Layout_3_CheckedChanged(object sender, EventArgs e)
        {
            Toggle_Layout_Events(false);
            rb_Layout_2.Checked = rb_Layout_1.Checked = rb_Layout_4.Checked = rb_Layout_5.Checked = rb_Layout_6.Checked = rb_Layout_7.Checked = false;
            rb_Layout_3.Checked = true;
            Toggle_Layout_Events(true);
        }

        private void rb_Layout_4_CheckedChanged(object sender, EventArgs e)
        {
            Toggle_Layout_Events(false);
            rb_Layout_2.Checked = rb_Layout_3.Checked = rb_Layout_1.Checked = rb_Layout_5.Checked = rb_Layout_6.Checked = rb_Layout_7.Checked = false;
            rb_Layout_4.Checked = true;
            Toggle_Layout_Events(true);
        }

        private void rb_Layout_5_CheckedChanged(object sender, EventArgs e)
        {
            Toggle_Layout_Events(false);
            rb_Layout_2.Checked = rb_Layout_3.Checked = rb_Layout_4.Checked = rb_Layout_1.Checked = rb_Layout_6.Checked = rb_Layout_7.Checked = false;
            rb_Layout_5.Checked = true;
            Toggle_Layout_Events(true);
        }

        private void rb_Layout_6_CheckedChanged(object sender, EventArgs e)
        {
            Toggle_Layout_Events(false);
            rb_Layout_2.Checked = rb_Layout_3.Checked = rb_Layout_4.Checked = rb_Layout_5.Checked = rb_Layout_1.Checked = rb_Layout_7.Checked = false;
            rb_Layout_6.Checked = true;
            Toggle_Layout_Events(true);
        }

        private void rb_Layout_7_CheckedChanged(object sender, EventArgs e)
        {
            Toggle_Layout_Events(false);
            rb_Layout_2.Checked = rb_Layout_3.Checked = rb_Layout_4.Checked = rb_Layout_5.Checked = rb_Layout_6.Checked = rb_Layout_1.Checked = false;
            rb_Layout_7.Checked = true;
            Toggle_Layout_Events(true);
        }

        private void rb_col_format_1_CheckedChanged(object sender, EventArgs e)
        {
            Toggle_A4_Col_Format_Events(false);
            rb_col_format_2.Checked = false;
            rb_col_format_1.Checked = true;
            Toggle_A4_Col_Format_Events(true);
        }

        private void rb_col_format_2_CheckedChanged(object sender, EventArgs e)
        {
            Toggle_A4_Col_Format_Events(false);
            rb_col_format_1.Checked = false;
            rb_col_format_2.Checked = true;
            Toggle_A4_Col_Format_Events(true);
        }
    }
}