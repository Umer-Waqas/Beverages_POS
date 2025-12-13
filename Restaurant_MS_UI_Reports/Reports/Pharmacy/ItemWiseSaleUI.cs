using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GK.Shared.Core;
using Restaurant_MS_Core.Entities;
using Restaurant_MS_Core.ViewModels;
using GK.Shared.Repository;
using GK.Shared.Repository;

using Restaurant_MS_UI.Menu.Main;


namespace Pharmacy.UI.Reports.Pharmacy
{
    public partial class ItemWiseSaleUI : Form
    {
        UnitOfWork unitOfWork;
        IPagedList<ItemWiseSaleVM> ItemSales;
        int PageNo = 1;
        BackgroundWorker bgWItemsLoader = new BackgroundWorker();
        List<Item> Items = new List<Item>();
        public ItemWiseSaleUI()
        {
            InitializeComponent();
        }
        private void ItemWiseSaleUI_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                LoadItems();
                LoadItemsData();
                SharedFunctions.SetGridStyle(grdData);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
        }
        private void bgWItemsLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate()
            {
                cmbItems.Enabled = false;
                lblLoadingItems.Visible = true;
            });

            //if (SharedVariables.POSItems == null)
            //{
            using (unitOfWork = new UnitOfWork())
            {
                Items = unitOfWork.ItemRspository.GetActiveItems().ToList();
            }
            //}
        }
        private void bgWItemsLoader_RunWokrerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmbItems.SelectedIndexChanged -= cmbItems_SelectedIndexChanged;
            cmbItems.DataSource = Items;
            Item select = new Item { ItemId = 0, ItemName = "Select Item" };
            Items.Insert(0, select);
            cmbItems.ValueMember = "ItemId";
            cmbItems.DisplayMember = "ItemName";
            cmbItems.SelectedIndexChanged += cmbItems_SelectedIndexChanged;
            cmbItems.SelectedIndex = 0;
            cmbItems.Enabled = true;
            lblLoadingItems.Visible = false;
        }
        private void LoadItems()
        {

            this.bgWItemsLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWItemsLoader_DoWork);
            this.bgWItemsLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWItemsLoader_RunWokrerCompleted);
            this.bgWItemsLoader.RunWorkerAsync();
            //Thread worker = new Thread(() => { LoadItems_Async(); });
            //worker.Name = "ItemsLoader";
            //worker.IsBackground = true;
            //worker.Start(); 
        }
        private void LoadItemsData(bool isDefaultLoad = false)
        {
            try
            {
                grdData.Rows.Clear();
                ItemWiseSalesMasterVM Result;
                using (unitOfWork = new UnitOfWork())
                {
                    if (isDefaultLoad)
                    {
                        Result = unitOfWork.ItemRspository.GetItemWiseSales(null, null, null, 1, SharedVariables.PageSize);
                    }
                    else
                    {
                        Result = unitOfWork.ItemRspository.GetItemWiseSales(dtpFrom.Value, dtpTo.Value, Convert.ToInt64(cmbItems.SelectedValue), PageNo, SharedVariables.PageSize);
                    }
                }
                ItemSales = Result.ItemWiseSales;
           
                foreach (ItemWiseSaleVM i in ItemSales)
                {
                    grdData.Rows.Add(i.ItemName, i.AvailableStock, i.SaleCount);
                }
                lblTotalSales.Text = Result.GrandSalesTotalQuantity.ToString();
                chartItemSales.DataSource = ItemSales;
                chartItemSales.Series["SaleItems"].XValueMember = "ItemName";
                chartItemSales.Series["SaleItems"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
                chartItemSales.Series["SaleItems"].YValueMembers = "SaleCount";
                chartItemSales.Series["SaleItems"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, ItemSales.PageCount);
                btnFirstPage.Enabled = btnPrevious.Enabled = ItemSales.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = ItemSales.HasNextPage;
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadItemsData(false);
        }
        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadItemsData();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadItemsData();
        }
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = ItemSales.PageCount;
            LoadItemsData();
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadItemsData();
        }
        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadItemsData();
        }
        private void cmbItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadItemsData();
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            //dlgPrintOptions d = new dlgPrintOptions();
            //d.ShowDialog();
            //if (!d.IsCancelled)
            //{
            //    if (d.IsPrint)
            //    {
            //        if (cmbItems.SelectedIndex > 0)
            //        {
            var res = this.getReportData();
            ItemSalesViewer v = new ItemSalesViewer(0, res, this.dtpFrom.Value, this.dtpTo.Value);
            v.Show();
            //    }
            //    else
            //    {
            //        ItemSalesViewer v = new ItemSalesViewer(d.Orientation, this.dtpFrom.Value, this.dtpTo.Value);
            //        v.Show();
            //    }
            //}
            //else if (d.IsSaveAsPdf)
            //{
            //    SavePdf(d.Orientation);
            //}
            //}
        }
        private void SavePdf(int Orientation)
        {
            try
            {
                var res = this.getReportData();
                if (Orientation == 0)
                {
                    Reports.Pharmacy.ItemSalesRpt_Portrait r = new ItemSalesRpt_Portrait();
                    r.SetDataSource(res);
                    r.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    r.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    r.SetParameterValue("DateTo", this.dtpTo.Value);
                    r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        r.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("ItemWise Sales Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    ItemSalesRpt_Landscape rpt = new ItemSalesRpt_Landscape();
                    rpt.SetDataSource(res);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("ItemWise Sales Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        private List<ItemWiseSaleVM> getReportData()
        {
            ItemWiseSalesMasterVM res = new ItemWiseSalesMasterVM();
            using (unitOfWork = new UnitOfWork())
            {
                res = unitOfWork.ItemRspository.GetItemWiseSales(this.dtpFrom.Value, this.dtpTo.Value, Convert.ToInt32(cmbItems.SelectedValue), null, null);
            }
            return res.ItemWiseSales.ToList();
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (SharedVariables.AdminPractiseSetting == null)
                {
                    throw new Exception("Failed to load report headers, please try after login again");
                }

                var res = getReportData();
                ItemSalesRpt_Landscape rpt = new ItemSalesRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(res);
                if (!string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    rpt.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                rpt.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("ItemWise Sales Exported Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        private void txtGotoPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtGotoPage.Text.Equals(""))
                {
                    int _pageNo = int.Parse(txtGotoPage.Text);
                    if (_pageNo <= 0 || _pageNo > ItemSales.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadItems();
                }
            }
        }

        private void cmbItems_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}