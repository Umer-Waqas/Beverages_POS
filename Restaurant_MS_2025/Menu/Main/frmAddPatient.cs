using Restaurant_MS_Core.Entities;
using GK.Shared.Repository;
using GK.Shared.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GK.Shared.Core;


namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmAddPatient : Form
    {
        UnitOfWork unitOfWork;
        private long PatientId = 0;

        public frmAddPatient()
        {
            InitializeComponent();
        }

        public frmAddPatient(long PatientId)
        {
            InitializeComponent();
            this.PatientId = PatientId;
        }
        private void btnShowSecContacts_Click(object sender, EventArgs e)
        {
            pnlSecondaryContacts.Visible = true;
        }

        private void btnHideSecContacts_Click(object sender, EventArgs e)
        {
            pnlSecondaryContacts.Visible = false;
        }

        private void txtTags_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (isTagAdded())
                {
                    ToolTip tt = new ToolTip();
                    tt.Show("Tag Already Used", txtTags, 800);
                    return;
                }
                GenerateTag(txtTags.Text.Trim().ToUpper());
            }
        }

        private void GenerateTag(string TagText)
        {
            Label lblTag = new Label();
            lblTag.AutoSize = true;
            lblTag.BackColor = System.Drawing.SystemColors.Highlight;
            lblTag.ForeColor = System.Drawing.Color.White;
            lblTag.Location = new System.Drawing.Point(333, 354);
            var margin = lblTag.Margin;
            margin.Top = 5;
            lblTag.Margin = margin;
            lblTag.MinimumSize = new System.Drawing.Size(0, 18);
            lblTag.Name = "lblTag";
            lblTag.Size = new System.Drawing.Size(41, 18);
            lblTag.TabIndex = 20;
            lblTag.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblTag.Text = TagText;
            lblTag.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            lblTag.DoubleClick += new EventHandler(lblTag_DoubleClicked);
            pnlTags.Controls.Add(lblTag);
            txtTags.Text = "";
        }
        private bool isTagAdded()
        {
            foreach (Label l in pnlTags.Controls)
            {
                if (l.Text.ToLower() == txtTags.Text.Trim().ToLower())
                {
                    return true;
                }
            }
            return false;
        }
        private void lblTag_DoubleClicked(object sender, EventArgs e)
        {
            Label ClickedLbl = (Label)sender;
            pnlTags.Controls.Remove(ClickedLbl);
        }

        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    if (this.PatientId > 0)
                    {
                        UpdatePatient();
                        return;
                    }
                    InsertPatient();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void UpdatePatient()
        {
            using (unitOfWork = new UnitOfWork())
            {
                Patient objPatient = unitOfWork.PatientRepository.GetPatientWithTags_ByPatientId(this.PatientId);
                string LastImagePath = objPatient.ImagePath;
                List<Tag> ExistingTags = objPatient.Tags.ToList();
                foreach (Tag t in ExistingTags)
                {
                    objPatient.Tags.Remove(t);
                }
                fillObject(objPatient);
                if (objPatient.IsSynced)
                {
                    objPatient.IsNew = false;
                    objPatient.IsUpdate = true;
                    objPatient.IsSynced = false;
                }
                if (objPatient.ImagePath != null)
                {
                    objPatient.ImagePath = SharedFunctions.CopyFileToLocal(objPatient.ImagePath);
                }

                objPatient.Tags = new List<Tag>();
                Tag objTag = null;
                if (Convert.ToInt64(cmbTags.SelectedValue) > 0)
                {
                    objTag = unitOfWork.TagsRepository.GetById(cmbTags.SelectedValue);
                    objPatient.Tags.Add(objTag);
                }
                else
                {
                    foreach (Label l in pnlTags.Controls)
                    {
                        objTag = unitOfWork.TagsRepository.FindTagByName(l.Text);
                        if (objTag == null)
                        {
                            objTag = new Tag();
                            objTag.TagName = l.Text;
                        }
                        objPatient.Tags.Add(objTag);
                    }
                }
                unitOfWork.PatientRepository.Update(objPatient);
                unitOfWork.Save();
            }
            MessageBox.Show("Patient Data Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }


        private void InsertPatient()
        {
            using (unitOfWork = new UnitOfWork())
            {
                Patient objPatient = new Patient();
                fillObject(objPatient);
                if (objPatient.ImagePath != null)
                {
                    objPatient.ImagePath = SharedFunctions.CopyFileToLocal(objPatient.ImagePath);
                }
                objPatient.Tags = new List<Tag>();
                Tag objTag = null;
                if (Convert.ToInt64(cmbTags.SelectedValue) > 0)
                {
                    objTag = unitOfWork.TagsRepository.GetById(cmbTags.SelectedValue);
                    objPatient.Tags.Add(objTag);
                }
                else
                {
                    foreach (Label l in pnlTags.Controls)
                    {
                        objTag = unitOfWork.TagsRepository.FindTagByName(l.Text);
                        if (objTag == null)
                        {
                            objTag = new Tag();
                            objTag.TagName = l.Text;
                        }
                        objPatient.Tags.Add(objTag);
                    }
                }
                unitOfWork.PatientRepository.Insert(objPatient);
                unitOfWork.Save();
            }
            MessageBox.Show("Patient Data Saved Sucessfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private void fillObject(Patient objPatient)
        {

            objPatient.Name = txtName.Text.Trim();
            objPatient.Email = txtEmail.Text.Trim();
            objPatient.Phone = txtPhone.Text;
            objPatient.Phone2 = txtPhone2.Text;
            objPatient.Phone3 = txtPhone3.Text;
            objPatient.CountryCode1 = "+92";
            objPatient.CountryCode2 = "+92";
            objPatient.CountryCode3 = "+92";
            if (rbMale.Checked) { objPatient.Gender = "male"; }
            else if (rbFemale.Checked) { objPatient.Gender = "female"; }
            else { objPatient.Gender = "other"; }
            //else { objPatient.Gender = 0; }
            objPatient.DateOfBirth = dtpDOB.Value;
            objPatient.AgeYears = (int)numAgeYears.Value;
            objPatient.AgeMonths = (int)numAgeYears.Value;
            objPatient.AgeDays = (int)numAgeDays.Value;

            objPatient.Address = txtAddress.Text.Trim();
            objPatient.ReferredBy = txtRefferedBy.Text.Trim();
            objPatient.ImagePath = lblFilePath.Text.ToLower().Equals("no file choosen") ? null : lblFilePath.Text;
            SharedVariables.PatientStatus patientStatus;
            Enum.TryParse(cmbStatus.GetItemText(cmbStatus.SelectedItem), out patientStatus);
            objPatient.Status = (int)patientStatus;
            objPatient.SMSPreferrence = rbEnglish.Checked == true ? 1 : 2;

            objPatient.IsActive = true;
            if (this.PatientId <= 0)
            {
                objPatient.IsNew = true;
                objPatient.CreatedAt = DateTime.Now;
                objPatient.IsActive = true;
            }
            objPatient.UpdatedAt = DateTime.Now;
            objPatient.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
        }

        private bool IsValidInput()
        {
            bool ErrFound = false;
            if (string.IsNullOrEmpty(txtName.Text.Trim()))
            {
                ErrName.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrName.Visible = false;
            }
            if (!txtPhone.MaskCompleted)
            {
                ErrPhone.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrPhone.Visible = false;
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

        private void frmAddPatient_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetSmallButtonsStyle(new[] { btnShowSecContacts, btnChooseFile, btnTakePhoto });
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddPatient, btnClear });
                cmbBloodGroup.SelectedIndex = 0;
                if (this.PatientId > 0)
                {
                    btnAddPatient.Text = "Update";
                    LoadPatientData();
                }
                else
                {
                    ShowNewMRNo();
                }
                LoadTags();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void LoadPatientData()
        {
            pnlUpdateCntrols.Visible = true;
            Patient p = new Patient();
            using (unitOfWork = new UnitOfWork())
            {
                p = unitOfWork.PatientRepository.GetPatientWithTags_ByPatientId(this.PatientId);
            }
            
            txtName.Text = p.Name;
            txtEmail.Text = p.Email;
            txtPhone.Text = p.Phone;
            if (!string.IsNullOrEmpty(p.Phone2))
            {
                pnlSecondaryContacts.Visible = true;
                txtPhone2.Text = p.Phone2;
                txtPhone3.Text = p.Phone3;
            }
            if (p.Gender.Equals("Male"))
            {
                rbMale.Checked = true;
            }
            else if (p.Gender.Equals("Female"))
            {
                rbFemale.Checked = true;
            }
            dtpDOB.Value = (DateTime)p.DateOfBirth;
            numAgeYears.Value = (int)p.AgeYears;
            numAgeMonths.Value = (int)p.AgeMonths;
            numAgeDays.Value = (int)p.AgeDays;
            txtAddress.Text = p.Address;
            txtRefferedBy.Text = p.ReferredBy;
            foreach (Tag t in p.Tags)
            {
                GenerateTag(t.TagName);
            }
            lblFilePath.Text = p.ImagePath == null ? "No File Choosen" : p.ImagePath;
            cmbStatus.SelectedItem = ((SharedVariables.PatientStatus)p.Status).ToString();
            if (p.SMSPreferrence == 1)
            {
                rbEnglish.Checked = true;
            }
            else
            {
                rbUrdu.Checked = true;
            }
        }
        private void LoadTags()
        {
            List<Tag> tags = new List<Tag>();
            using (unitOfWork = new UnitOfWork())
            {
                tags = unitOfWork.TagsRepository.GetAll().ToList();
            }
            Tag DefaultTag = new Tag();
            DefaultTag.TagId = 0;
            DefaultTag.TagName = "Select Tag";
            tags.Insert(0, DefaultTag);
            cmbTags.DataSource = tags;
            cmbTags.ValueMember = "TagId";
            cmbTags.DisplayMember = "TagName";
        }
        private void ShowNewMRNo()
        {
            using (unitOfWork = new UnitOfWork())
            {
                txtMrNo.Text = unitOfWork.PatientRepository.GetNewMrNo().ToString();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ShowNewMRNo();
            btnAddPatient.Text = "Add";
        }

        private void btnChooseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Select Invoice Image";
            fd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;";
            fd.CheckFileExists = true;
            fd.CheckPathExists = true;
            fd.Multiselect = false;
            fd.RestoreDirectory = true;
            DialogResult rs = fd.ShowDialog();
            if (rs == DialogResult.OK)
            {
                string selectedFile = fd.FileName;
                lblFilePath.Text = selectedFile;
            }
        }

        private void cmbTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlTags.Controls.Clear();
        }
    }
}