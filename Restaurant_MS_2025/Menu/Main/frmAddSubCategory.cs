

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmAddSubCategory : Form
    {
        UnitOfWork unitOfWork;
        public frmAddSubCategory()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsValidInput()) { return; }
                Category c = new Category();
                c.Name = txtCategory.Text.Trim();
                c.CreatedAt = DateTime.Now;
                c.UpdatedAt = DateTime.Now;
                c.ParentId = Convert.ToInt64(cmbCategory.SelectedValue);
                c.IsActive = true;
                c.IsNew = true;
                c.IsActive = true;
                c.UserId = SharedVariables.LoggedInUser.UserId;
                using (unitOfWork = new UnitOfWork())
                {
                    unitOfWork.CategoryRepository.Insert(c);
                    unitOfWork.Save();
                }
                this.Close();
                MessageBox.Show("Sub category saved successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            LoadCategories();
        }

        private void LoadCategories()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    List<Category> cts = new List<Category>();
                    cts = unitOfWork.CategoryRepository.GetAllActiveCategories();
                    Category c = new Category();
                    c.CategoryId = 0;
                    c.Name = "Select Category";
                    cts.Insert(0, c);
                    cmbCategory.DataSource = cts;
                    cmbCategory.ValueMember = "CategoryId";
                    cmbCategory.DisplayMember = "Name";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading categories, please try again.", "Categories not laoded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private bool IsValidInput()
        {
            bool ErrFound = false;
            if (cmbCategory.SelectedIndex <= 0)
            {
                errCategory.Visible = true;
                ErrFound = true;
            }
            else
            {
                errCategory.Visible = false;
            }

            if (txtCategory.Text.Trim().Equals(""))
            {
                errCategoryName.Visible = true;
                ErrFound = true;
            }
            else
            {
                errCategoryName.Visible = false;
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
    }
}
