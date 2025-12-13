

using Restaurant_MS_Infrastructure.Database;
using Restaurant_MS_Infrastructure.Repository;

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmAddTag : Form
    {
        static AppDbContext cxt = new AppDbContext();
        TagsRepository repTags = new TagsRepository(cxt);
        PatientRepository repPatients = new PatientRepository(cxt);
        UnitOfWork unitOfWork = new UnitOfWork();
        long PatientId = 0;
        public long TagID = 0;
        public string TagName = "";
        public frmAddTag()
        {
            InitializeComponent();
        }
        public frmAddTag(long PatientId)
        {
            InitializeComponent();
            this.PatientId = PatientId;
        }

        private void frmAddTag_Load(object sender, EventArgs e)
        {
            LoadAllTags();
        }
        private void LoadAllTags()
        {
            List<Tag> tags = repTags.GetAll().ToList();
            cmbTags.DataSource = tags;
            cmbTags.ValueMember = "TagId";
            cmbTags.DisplayMember = "TagName";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                long TagID = Convert.ToInt64(cmbTags.SelectedValue);
                bool Exists = unitOfWork.PatientRepository.PatientAlreadyHasTag(TagID, this.PatientId);
                if(Exists)
                {
                    throw new Exception("Selected Tag is Already Attached With This Patient");
                }
                unitOfWork.TagsRepository.AddTag(TagID, this.PatientId);
                this.TagID = TagID;
                this.TagName = cmbTags.GetItemText(cmbTags.SelectedItem);
                this.Close();
            }
            catch(Exception ex)
            {
                if(SharedVariables.ShowActualExceptionMessages)
                {
                    ErrMessage.Text = ex.Message;
                }
                else
                {
                    ErrMessage.Text = SharedVariables.GeneralErrMsg;
                }
                pnlError.Visible = true;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDismiss_Click(object sender, EventArgs e)
        {
            pnlError.Visible = false;
        }
    }
}