using GK.Shared.Core;
using Restaurant_MS_Core.Entities;
using GK.Shared.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmAddCategory : Form
    {
        UnitOfWork unitOfWork;
        public frmAddCategory()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!txtCategory.Text.Trim().Equals(""))
                {
                    Category c = new Category();
                    c.Name = txtCategory.Text.Trim();
                    c.IsSystemCategory = false;

                    c.CreatedAt = DateTime.Now;
                    c.UpdatedAt = DateTime.Now;
                    c.IsActive = true;
                    c.IsNew = true;
                    c.IsActive = true;
                    using (unitOfWork = new UnitOfWork())
                    {
                        c.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                        unitOfWork.CategoryRepository.Insert(c);
                        unitOfWork.Save();
                    }
                    this.Close();
                    MessageBox.Show("Category saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException.Message.Contains("IX_Name"))
                    {
                        MessageBox.Show("Category with Given Name Already Exists, Please Specify Another Name", "Supplier Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                MessageBox.Show("Error occurred while saving category, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddCategory_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnCancel, btnSave });
        }
    }
}
