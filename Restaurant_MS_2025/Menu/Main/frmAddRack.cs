

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmAddRack : Form
    {
        UnitOfWork unitOfWork;
        private int RackId;
        public frmAddRack()
        {
            InitializeComponent();
        }
        public frmAddRack(int RackId)
        {
            InitializeComponent();
            this.RackId = RackId;
        }
        private void frmAddRack_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] {btnSave, btnClose });
            if (this.RackId > 0)
            {
                loadRack();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loadRack()
        {
            Rack r = new Rack();
            using(unitOfWork = new UnitOfWork ())
            {
                r =unitOfWork.RackRepository.GetById(this.RackId);
            }
            this.txtRackName.Text = r.Name;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtRackName.Text.Trim().Equals(""))
            {
                ErrRack.Visible = true;
                return;
            }
            else
            {
                if(this.RackId > 0)
                {
                    if (!RackExists_Update(this.txtRackName.Text.Trim().ToLower(), this.RackId))
                    {
                        UpdateData();
                    }
                    else
                    {
                        MessageBox.Show("Rack with Given Name Already Exists, Please Specify Another Name", "Manufacturer Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                else
                {
                    if (!RackExists_new(txtRackName.Text.Trim()))
                    {
                        Save();
                    }
                    else
                    {
                        MessageBox.Show("Rack with Given Name Already Exists, Please Specify Another Name", "Manufacturer Already Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void Save()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    Rack r = new Rack();
                    r.Name = txtRackName.Text.Trim();
                    r.IsNew = true;
                    r.IsActive = true;
                    r.CreatedAt = DateTime.Now;
                    r.UpdatedAt = DateTime.Now;
                    r.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    unitOfWork.RackRepository.Insert(r);
                    unitOfWork.Save();
                    this.Close();
                    MessageBox.Show("Rack data inserted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while inserting rack data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateData()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    Rack r = unitOfWork.RackRepository.GetById(this.RackId);
                    r.Name = txtRackName.Text.Trim();
                    if (r.IsSynced)
                    {
                        r.IsNew = false;
                        r.IsUpdate = true;
                        r.IsSynced = false;
                    }
                    r.UpdatedAt = DateTime.Now;
                    r.User = unitOfWork.UserRepository.GetById(SharedVariables.LoggedInUser.UserId);
                    unitOfWork.RackRepository.Update(r);
                    unitOfWork.Save();
                    this.Close();
                    MessageBox.Show("Rack data Updated successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating rack data, please try again.", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool RackExists_new(string name)
        {
            using (unitOfWork = new UnitOfWork())
            {
                return unitOfWork.RackRepository.RackExists_New(name);
            }
        }
        private bool RackExists_Update(string name, int TemplateId)
        {
            using (unitOfWork = new UnitOfWork())
            {
                return unitOfWork.RackRepository.RackExists_Update(name, this.RackId);
            }
        }

    }
}
