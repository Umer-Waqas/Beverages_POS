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
    public partial class frmAddEmployee : Form
    {
        UnitOfWork unitOfWork;
        private long EmployeeId = 0;

        public frmAddEmployee()
        {
            InitializeComponent();
        }

        public frmAddEmployee(long PatientId)
        {
            InitializeComponent();
            this.EmployeeId = PatientId;
        }
        private void btnShowSecContacts_Click(object sender, EventArgs e)
        {
            pnlSecondaryContacts.Visible = true;
        }

        private void btnHideSecContacts_Click(object sender, EventArgs e)
        {
            pnlSecondaryContacts.Visible = false;
        }




        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsValidInput())
                {
                    if (this.EmployeeId > 0)
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
                Employee obj = unitOfWork.EmployeeRepository.GetEmployeeById(this.EmployeeId);
                string LastImagePath = obj.ImagePath;

                fillObject(obj);
                if (obj.IsSynced)
                {
                    obj.IsNew = false;
                    obj.IsUpdate = true;
                    obj.IsSynced = false;
                }
                if (obj.ImagePath != null)
                {
                    obj.ImagePath = SharedFunctions.CopyFileToLocal(obj.ImagePath);
                }
                unitOfWork.EmployeeRepository.Update(obj);
                unitOfWork.Save();
            }
            MessageBox.Show("Employee Data Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }


        private void InsertPatient()
        {
            using (unitOfWork = new UnitOfWork())
            {
                Employee obj = new Employee();
                fillObject(obj);
                if (obj.ImagePath != null)
                {
                    obj.ImagePath = SharedFunctions.CopyFileToLocal(obj.ImagePath);
                }
                unitOfWork.EmployeeRepository.Insert(obj);
                unitOfWork.Save();
            }
            MessageBox.Show("Employee Data Saved Sucessfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
        private void fillObject(Employee obj)
        {

            obj.Name = txtName.Text.Trim();
            obj.Email = txtEmail.Text.Trim();
            obj.Phone = txtPhone.Text;
            obj.Phone2 = txtPhone2.Text;
            obj.Phone3 = txtPhone3.Text;
            obj.CountryCode1 = "+92";
            obj.CountryCode2 = "+92";
            obj.CountryCode3 = "+92";
            if (rbMale.Checked) { obj.Gender = "male"; }
            else if (rbFemale.Checked) { obj.Gender = "female"; }
            else { obj.Gender = "other"; }
            //else { objPatient.Gender = 0; }
            obj.DateOfBirth = dtpDOB.Value;
            obj.AgeYears = (int)numAgeYears.Value;
            obj.AgeMonths = (int)numAgeYears.Value;
            obj.AgeDays = (int)numAgeDays.Value;

            obj.Address = txtAddress.Text.Trim();
            obj.ReferredBy = "";
            obj.ImagePath = lblFilePath.Text.ToLower().Equals("no file choosen") ? null : lblFilePath.Text;
            SharedVariables.PatientStatus patientStatus;
            obj.Status = 0;
            obj.SMSPreferrence = 1;
            obj.IsActive = true;
            if (this.EmployeeId <= 0)
            {
                obj.IsNew = true;
                obj.CreatedAt = DateTime.Now;
                obj.IsActive = true;
            }
            obj.UpdatedAt = DateTime.Now;
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
                if (this.EmployeeId > 0)
                {
                    btnAddPatient.Text = "Update";
                    LoadPatientData();
                }
                else
                {
                    ShowNewMRNo();
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void LoadPatientData()
        {
            Patient p = new Patient();
            using (unitOfWork = new UnitOfWork())
            {
                p = unitOfWork.PatientRepository.GetPatientWithTags_ByPatientId(this.EmployeeId);
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
            lblFilePath.Text = p.ImagePath == null ? "No File Choosen" : p.ImagePath;
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
    }
}