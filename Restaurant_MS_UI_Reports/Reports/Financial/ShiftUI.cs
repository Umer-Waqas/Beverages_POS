using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_MS_Core.ViewModels;
using GK.Shared.Repository;
using Pharmacy.UI.Reports.Pharmacy;
using GK.Shared.Core;

namespace Pharmacy.UI.Reports.Financial
{
    public partial class ShiftUI : Form
    {
        UnitOfWork unitOfWork;
        public ShiftUI()
        {
            InitializeComponent();
        }

        private void ShiftUI_Load(object sender, EventArgs e)
        {
            cmbShift.SelectedIndexChanged -= cmbPaymentMode_SelectedIndexChanged;
            cmbShift.SelectedIndex = 0;
            LoadShiftData();
            cmbShift.SelectedIndexChanged += cmbPaymentMode_SelectedIndexChanged;
            SharedFunctions.SetGridStyle(grdData);
        }

        private List<ShiftCollectionVM> GetShiftsData()
        {
            int Shift = cmbShift.SelectedIndex;
            List<ShiftCollectionVM> CollectionList;
            using (unitOfWork = new UnitOfWork())
            {
                CollectionList = unitOfWork.InvoiceRepository.ShiftCollection(dtpFrom.Value, dtpTo.Value, Shift);
            }
            return CollectionList;
        }
        private void LoadShiftData()
        {
            List<ShiftCollectionVM> CollectionList = GetShiftsData();
            grdData.Rows.Clear();
            foreach (ShiftCollectionVM c in CollectionList)
            {
                grdData.Rows.Add(c.Shift, c.Collection);
            }

            chartShiftCollection.DataSource = CollectionList;
            chartShiftCollection.Series["ShiftCollection"].XValueMember = "Shift";
            chartShiftCollection.Series["ShiftCollection"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.String;
            chartShiftCollection.Series["ShiftCollection"].YValueMembers = "Collection";
            chartShiftCollection.Series["ShiftCollection"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
        }
        private void cmbPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadShiftData();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadShiftData();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            LoadShiftData();
        }


        private void btnPrint_Click(object sender, EventArgs e)
        {
            List<ShiftCollectionVM> CollectionList = GetShiftsData();
            dlgPrintOptions d = new dlgPrintOptions();
            d.ShowDialog();
            if (!d.IsCancelled)
            {
                if (d.IsPrint)
                {
                    ShiftViewer v = new ShiftViewer(d.Orientation, this.dtpFrom.Value, this.dtpTo.Value, CollectionList);
                    v.Show();
                }
                else if (d.IsSaveAsPdf)
                {
                    SavePdf(d.Orientation, CollectionList);
                }
            }
        }
        private void SavePdf(int Orientation, List<ShiftCollectionVM> CollectionList)
        {
            try
            {
                if (Orientation == 0)
                {
                    Reports.Financial.ShiftWiseCollection_Portrait r = new ShiftWiseCollection_Portrait();
                    r.SetDataSource(CollectionList);
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
                    ShiftWiseCollection_Landscape rpt = new ShiftWiseCollection_Landscape();
                    rpt.SetDataSource(CollectionList);
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

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                List<ShiftCollectionVM> CollectionList = GetShiftsData();
                Reports.Financial.ShiftWiseCollection_Landscape rpt = new ShiftWiseCollection_Landscape();
                rpt.SetDataSource(CollectionList);
                rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Financial Shift Collection Data Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }
    }
}
