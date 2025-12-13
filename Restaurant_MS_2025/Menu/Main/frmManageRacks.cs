

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmManageRacks : Form
    {
        UnitOfWork unitOfWork;
        int pageNo = 1;
        IPagedList<Rack> racks;
        public frmManageRacks()
        {
            InitializeComponent();
        }

        private void frmRacks_Load(object sender, EventArgs e)
        {
            SharedFunctions.SetLarggeButtonsStyle(new[] { btnAddRack, btnRefreshForm });
            SharedFunctions.SetSmallButtonsStyle(new[] { btnPrint, btnExcel });
            SharedFunctions.SetGridStyle(grdItems);
            Loadracks();
        }

        private void Loadracks()
        {
            using(unitOfWork = new UnitOfWork ())
            {
                 racks = unitOfWork.RackRepository.GetActiveRacks(pageNo, SharedVariables.PageSize);
            }
            grdItems.Rows.Clear();
            if (racks!=null && racks.TotalCount > 0)
            {
                foreach (var a in racks.Items)
                {
                    grdItems.Rows.Add(a.RackId, a.Name);
                }

                btnPrevious.Enabled = btnFirstPage.Enabled = racks.HasPreviousPage;
                btnNext.Enabled = btnLastPage.Enabled = racks.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, pageNo, racks.PageCount);
            }
        }

        private void grdItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if(e.ColumnIndex == grdItems.Columns["colBtnEdit"].Index)
            {
                int id = Convert.ToInt32(grdItems.Rows[e.RowIndex].Cells["colRackId"].Value);
                SharedFunctions.OpenForm(new frmAddRack(id), this.MdiParent);
                return;
            }
        }

        private void btnRefreshForm_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            Loadracks();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            pageNo += 1;
            Loadracks();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            pageNo = racks.PageCount;
            Loadracks();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            pageNo -= 1;
            Loadracks();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            pageNo = 1;
            Loadracks();
        }

        private void btnAddRack_Click(object sender, EventArgs e)
        {
            Form f = SharedFunctions.OpenForm(new frmAddRack(), this.MdiParent);
            f.FormClosed += new FormClosedEventHandler(this.childForm_Closed);
        }

        private void childForm_Closed(object sender, FormClosedEventArgs e)
        {
            this.btnRefreshForm.PerformClick();
        }
    }
}