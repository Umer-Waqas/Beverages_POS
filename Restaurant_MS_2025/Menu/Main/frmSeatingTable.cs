

namespace Restaurant_MS_UI.Menu.Main
{
    public partial class frmSeatingTable : Form
    {
        private int TableId = 0;
        public frmSeatingTable()
        {
            InitializeComponent();
        }
        public frmSeatingTable(int TableId)
        {
            InitializeComponent();
            this.TableId = TableId;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (IsValidInput())
            {
                if (this.TableId > 0)
                {
                    update();
                }
                else
                {
                    insert();
                }
            }
            loadTablesData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private void fillObj(ref SeatingTable st, bool isActive = true)
        //{
        //    st.TableName = txtTableName.Text.Trim();
        //    st.TableCode = txtTableCode.Text.Trim();
        //    st.IsActive = isActive;
        //    if (this.TableId > 0)
        //    {
        //        st.IsNew = false;
        //        st.IsUpdate = true;
        //    }
        //    else
        //    {
        //        st.CreatedAt = DateTime.Now;
        //        st.IsNew = true;
        //        st.IsUpdate = false;
        //    }
        //    st.UpdatedAt = DateTime.Now;
        //    st.IsSynced = false;
        //}

        private void insert()
        {
            try
            {
                //SeatingTable obj = new SeatingTable();
                //fillObj(ref obj);
                //using (UnitOfWork uw = new UnitOfWork())
                //{
                //    uw.SeatingTableRspository.Insert(obj);
                //    uw.Save();
                //}
                //SharedFunctions.ShowGeneralSuccessMessage();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowGeneralErrorMessage(ex);
            }
        }

        private void update()
        {
            try
            {
                using (UnitOfWork uw = new UnitOfWork())
                {
                  //  var obj = uw.SeatingTableRspository.GetById(this.TableId);
                  //  fillObj(ref obj);
                    //uw.SeatingTableRspository.Update(obj);
                    //uw.Save();
                }
                SharedFunctions.ShowGeneralSuccessMessage();
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowGeneralErrorMessage(ex);
            }
        }

        private void grdTables_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            var row = grdTables.Rows[e.RowIndex];
            this.TableId = Convert.ToInt32(row.Cells["colId"].Value);
            if (e.ColumnIndex == grdTables.Columns["colBtnEdit"].Index)
            {
                try
                {
                    txtTableName.Text = row.Cells["colTableName"].Value.ToString();
                    txtTableCode.Text = row.Cells["colTableCode"].Value.ToString();
                }
                catch (Exception ex)
                {
                    this.TableId = 0;
                    SharedFunctions.ShowGeneralErrorMessage(ex);
                }
            }
            else if (e.ColumnIndex == grdTables.Columns["colBtnDelete"].Index)
            {
                try
                {
                    using (UnitOfWork uw = new UnitOfWork())
                    {
                       // var obj = uw.SeatingTableRspository.GetById(this.TableId);
                       // this.fillObj(ref obj, isActive: false);
                       // uw.SeatingTableRspository.Update(obj);
                        //uw.Save();
                    }
                }
                catch (Exception ex)
                {
                    SharedFunctions.ShowGeneralErrorMessage(ex);
                }
            }
        }
        private bool IsValidInput()
        {
            bool ErrFound = false;
            if (string.IsNullOrEmpty(txtTableName.Text.Trim()))
            {
                ErrTableName.Visible = true;
                ErrFound = true;
            }
            else
            {
                ErrTableName.Visible = false;
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

        private void frmSeatingTable_Load(object sender, EventArgs e)
        {
            loadTablesData();
        }
        private void loadTablesData()
        {
            grdTables.Rows.Clear();
            using (UnitOfWork uw = new UnitOfWork())
            {
                //var tables = uw.SeatingTableRspository.Query().Where(t => t.IsActive).ToList();
                //foreach (var t in tables)
                //{
                //    grdTables.Rows.Add(t.SeatingTableId, t.TableName, t.TableCode);
                //}
            }
        }
    }
}