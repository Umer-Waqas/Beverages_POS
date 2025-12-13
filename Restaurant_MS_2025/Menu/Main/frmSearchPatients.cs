
namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmSearchPatients : Form
    {
        static AppDbContext cxt = new AppDbContext();
        PatientRepository repPatients = new PatientRepository(cxt);
        public long PatientId = 0;
        public string PatientName = "";
        public frmSearchPatients()
        {
            InitializeComponent();
        }
        private void frmSearchPatients_Load(object sender, EventArgs e)
        {
            LoadPatients();
            SharedFunctions.SetGridStyle(grdPatients);
        }
        private void LoadPatients()
        {
            List<Patient> patients = repPatients.GetAll().ToList();
            grdPatients.DataSource = patients;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            PatientId = Convert.ToInt64(grdPatients.SelectedRows[0].Cells["colPatientId"].Value);
            PatientName = grdPatients.SelectedRows[0].Cells["colPatientId"].Value.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
