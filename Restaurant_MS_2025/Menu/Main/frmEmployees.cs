

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmEmployees : Form
    {
        static AppDbContext cxt;
        private IPagedList<Employee> EmployeesList;
        EmployeeRepository repEmployees;
        TagsRepository repTags;
        int PageNo = 1;
        int PageSize = SharedVariables.PageSize;
        bool isByTag = false;
        bool isSearch = false;
        int SelectedTagId = 0;
        string SearchString = "";
        bool isAdvancedSearch = false;
        private void InitializedObjects()
        {
            cxt = new AppDbContext();
            repEmployees = new EmployeeRepository(cxt);
        }
        public frmEmployees()
        {
            InitializeComponent();
            InitializedObjects();
        }
        private void btnAddPatient_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmAddEmployee(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
        }

        private void frmPatients_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnRefresh, btnAddPatient });
                LoadForm();
                SharedFunctions.SetGridStyle(grdPatients);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading Employees data, please try again.", "Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadForm()
        {
            GetEmployeesAsync();
            cmbSearchBy.SelectedIndex = 0;
        }
        private void GetEmployeesAsync()
        {
            grdPatients.Rows.Clear();
            EmployeesList = repEmployees.GetEmployeesAsync(PageNo, PageSize);
            btnFirstPage.Enabled = btnPrevious.Enabled = EmployeesList.HasPreviousPage;
            btnLastPage.Enabled = btnNext.Enabled = EmployeesList.HasNextPage;
            SharedFunctions.ShowPageNo(lblPageNo, PageNo, EmployeesList.PageCount);
            foreach (Employee p in EmployeesList.Items)
            {
                grdPatients.Rows.Add(p.EmployeeId, p.Name, p.Gender, p.Phone);
            }
        }
        //private void GetPatientsAsync_ByTag(int TagId)
        //{
        //    grdPatients.Rows.Clear();
        //    PatientsList = repEmployees.GetPatientsAsync_ByTag(TagId, PageNo, PageSize);
        //    btnPrevious.Enabled = PatientsList.HasPreviousPage;
        //    btnNext.Enabled = PatientsList.HasNextPage;
        //    SharedFunctions.ShowPageNo(lblPageNo, PageNo, PatientsList.PageCount);
        //    foreach (Patient p in PatientsList)
        //    {
        //        grdPatients.Rows.Add(p.PatientId, p.Name, p.Gender, p.Phone, p.ReferredBy);
        //    }
        //}
        private void GetPatientsAsync_BySearch()
        {
            grdPatients.Rows.Clear();
            EmployeesList = repEmployees.GetEmployeesAsync_BySearch(SearchString, PageNo, PageSize);
            btnPrevious.Enabled = EmployeesList.HasPreviousPage;
            btnNext.Enabled = EmployeesList.HasNextPage;
            SharedFunctions.ShowPageNo(lblPageNo, PageNo, EmployeesList.PageCount);
            foreach (Employee p in EmployeesList.Items)
            {
                grdPatients.Rows.Add(p.EmployeeId, p.Name, p.Gender, p.Phone, p.ReferredBy);
            }
        }
        private void GetPatientsAsync_ByAdvancedSearch(string SearchString, string SearchBy)
        {
            grdPatients.Rows.Clear();
            EmployeesList = repEmployees.GetEmployeesAsync_ByAdvancedSearch(SearchString, SearchBy, PageNo, PageSize);
            btnPrevious.Enabled = EmployeesList.HasPreviousPage;
            btnNext.Enabled = EmployeesList.HasNextPage;
            SharedFunctions.ShowPageNo(lblPageNo, PageNo, EmployeesList.PageCount);
            foreach (Employee p in EmployeesList.Items)
            {
                grdPatients.Rows.Add(p.EmployeeId, p.Name, p.Gender, p.Phone, p.ReferredBy);
            }
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            pnlTags.Controls.Clear();
            PageNo = 1;
            isByTag = false;
            isSearch = false;
            SelectedTagId = 0;
            SearchString = "";
            lblAdvancedSearch.Text = "Advanced Search";
            isAdvancedSearch = false;
            cmbSearchBy.Visible = false;
            cmbSearchBy.SelectedIndex = 0;
            InitializedObjects();
            LoadForm();
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SearchString = txtSearch.Text.Trim();
                if (SearchString != "")
                {
                    if (isAdvancedSearch)
                    {
                        GetPatientsAsync_ByAdvancedSearch(SearchString, cmbSearchBy.GetItemText(cmbSearchBy.SelectedItem));
                        return;
                    }
                    isSearch = true;
                    GetPatientsAsync_BySearch();
                }
            }
        }

        private void lblAdvancedSearch_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!isAdvancedSearch)
            {
                lblAdvancedSearch.Text = "Geneal Search";
                isAdvancedSearch = true;
                cmbSearchBy.Visible = true;
            }
            else
            {
                lblAdvancedSearch.Text = "Advanced Search";
                isAdvancedSearch = false;
                cmbSearchBy.Visible = false;
            }
        }

        private void cmbSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void grdPatients_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    long EmployeeId = Convert.ToInt64(grdPatients.Rows[e.RowIndex].Cells["colEmployeeId"].Value);
                    if (e.ColumnIndex == grdPatients.Columns["colViewPatient"].Index)
                    {
                        Form f = SharedFunctions.OpenForm(new frmEmployeeProfile(EmployeeId), this.MdiParent);
                        f.FormClosed += new FormClosedEventHandler(ChildForm_Closed);
                    }
                    //else if (e.ColumnIndex == grdPatients.Columns["colAddInvoice"].Index)
                    //{
                    //    frmInvoice f = new frmInvoice(EmployeeId);
                    //    f.Show();
                    //}
                    else if (e.ColumnIndex == grdPatients.Columns["colDelete"].Index)
                    {
                        if (!SharedFunctions.IsActionallowed("Delete Employees") && !SharedVariables.LoggedInUser.UserRoles.Any(r => r.IsAdmin))
                        {
                            MessageBox.Show("You Do Not Have Permissions to Perform This Action", "Access Denied", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                        DialogResult rs = MessageBox.Show("Are you Sure, You Want To Delete This Patient", "Please Make Sure", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (rs == System.Windows.Forms.DialogResult.OK)
                        {
                            repEmployees.SetInActive(EmployeeId);
                            MessageBox.Show("Employee Deleted Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnRefresh.PerformClick();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void ChildForm_Closed(object sender, FormClosedEventArgs e)
        {
            btnRefresh.PerformClick();
        }
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = EmployeesList.TotalCount;
            GetEmployeesAsync();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo--;
            GetEmployeesAsync();
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            GetEmployeesAsync();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo++;
            GetEmployeesAsync();
        }

        private void txtGotoPage_KeyPress(object sender, KeyPressEventArgs e)
        {
            SharedFunctions.isValidIntegerKey(sender, e);
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > EmployeesList.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    GetEmployeesAsync();
                }
            }
        }
    }
}