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
    public partial class NarcoticDrugsUI : Form
    {
        int PageNo = 1;
        private UnitOfWork unitOfWork;
        private IPagedList<NarcoticsDrugInvoiceVM> Invoices;
        public NarcoticDrugsUI()
        {
            InitializeComponent();
        }

        private void NarcoticUI_Load(object sender, EventArgs e)
        {
            try
            {
                SharedFunctions.SetLarggeButtonsStyle(new[] { btnExcel, btnPrint });
                SharedFunctions.SetSmallButtonsStyle(new[] { btnFirstPage, btnPrevious, btnNext, btnLastPage });
                LoadDrugs();
                SharedFunctions.SetGridStyle(grdData);
            }
            catch(Exception ex)
            {
                SharedFunctions.ShowErrorMessage("Error occurred while loading form data, please try again.", ex.Message, "Failed");
            }
        }

        private void LoadDrugs()
        {
            try
            {
                using (unitOfWork = new UnitOfWork())
                {
                    if(!this.txtSearchInv.Text.Trim().Equals(""))
                    {
                        //Invoices = unitOfWork.InvoiceRepository.GetInvoicesWithNarcoticDrugs(Int64.Parse(txtSearchInv.Text.Trim()), PageNo, SharedVariables.PageSize);
                    }
                    else if (!this.txtSearchPatient.Text.Trim().Equals(""))
                    {
                        //Invoices = unitOfWork.InvoiceRepository.GetInvoicesWithNarcoticDrugs(dtpFrom.Value, dtpTo.Value, txtSearchPatient.Text.Trim().ToLower(), PageNo, SharedVariables.PageSize);
                    }
                    else if (!this.txtSearchDrug.Text.Trim().Equals(""))
                    {
                        //Invoices = unitOfWork.InvoiceRepository.GetInvoicesWithNarcoticDrugs(dtpFrom.Value, dtpTo.Value, txtSearchDrug.Text.Trim().ToLower(), PageNo, SharedVariables.PageSize, true);
                    }
                    else
                    {
                        //Invoices = unitOfWork.InvoiceRepository.GetInvoicesWithNarcoticDrugs(dtpFrom.Value, dtpTo.Value, PageNo, SharedVariables.PageSize);
                    }
                }
                grdData.Rows.Clear();

                double Quantity = 0;
                double ReturnedQuantity = 0;
                double TotalRefund = 0;
                double Value = 0;
                foreach (NarcoticsDrugInvoiceVM i in Invoices)
                {
                    TotalRefund = 0;
                    ReturnedQuantity = 0;
                    Quantity = 0;
                    Value = 0;
                    if(i.DiscountType == 2)
                    {
                        i.TotalDiscount = (i.TotalDiscount/ 100) * i.SubTotal;
                    }                   
                    foreach (InvoiceItemVM ii in i.InvoiceItems)
                    {
                        i.NarcoticDrugs += ii.ItemName + ", ";
                        Quantity += ii.Quantity;
                        ReturnedQuantity += ii.ReturnedQuantity;
                        if (i.Payment < 0)
                        {
                            TotalRefund += (((ii.NetAmount / (i.SubTotal - i.TotalDiscount)) * i.TotalPaid) / Quantity) * ReturnedQuantity;
                        }
                        Value += ((ii.NetAmount/ (i.SubTotal - i.TotalDiscount)) * i.Payment);
                    }
                    if (!string.IsNullOrEmpty(i.NarcoticDrugs))
                    {
                        i.NarcoticDrugs = i.NarcoticDrugs.Trim().TrimEnd(',');
                    }
                    if (i.Payment < 0)
                    {
                        grdData.Rows.Add(i.InvoiceId.ToString("D6"), i.PatientName, i.NarcoticDrugs, ReturnedQuantity, Math.Round(-1 * TotalRefund, 2), i.PaymentDate == DateTime.MinValue ? "" : i.PaymentDate.ToString(), i.CreatedAt);
                    }
                    else
                    {
                        grdData.Rows.Add(i.InvoiceId.ToString("D6"), i.PatientName, i.NarcoticDrugs, Quantity, Math.Round(Value, 2), i.PaymentDate == DateTime.MinValue ? "" : i.PaymentDate.ToString(), i.CreatedAt);
                    }                   
                }
                btnFirstPage.Enabled = btnPrevious.Enabled = Invoices.HasPreviousPage;
                btnLastPage.Enabled = btnNext.Enabled = Invoices.HasNextPage;
                SharedFunctions.ShowPageNo(lblPageNo, PageNo, Invoices.PageCount);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
        //    dlgPrintOptions d = new dlgPrintOptions();
        //    d.ShowDialog();
        //    if (!d.IsCancelled)
        //    {
                List<NarcoticsDrugInvoiceVM> Invoices =GetReportData();
        //        if (d.IsPrint)
        //        {
                    NarcoticDrugsViewer v = new NarcoticDrugsViewer(0, this.dtpFrom.Value, this.dtpTo.Value, Invoices);
                    v.Show();
            //    }
            //    else if (d.IsSaveAsPdf)
            //    {
            //        SavePdf(d.Orientation, Invoices);
            //    }
            //}
        }

        private List<NarcoticsDrugInvoiceVM> GetReportData()
        {
            List<NarcoticsDrugInvoiceVM> Invs;
            List<NarcoticsDrugInvoiceVM> FinalTransactions = new List<NarcoticsDrugInvoiceVM>();
            using (unitOfWork = new UnitOfWork())
            {
                if (!this.txtSearchInv.Text.Trim().Equals(""))
                {
                    //Invs = unitOfWork.InvoiceRepository.GetInvoicesWithNarcoticDrugs(long.Parse(txtSearchInv.Text.Trim()));
                }
                else if (!this.txtSearchPatient.Text.Trim().Equals(""))
                {
                    //Invs = unitOfWork.InvoiceRepository.GetInvoicesWithNarcoticDrugs(dtpFrom.Value, dtpTo.Value, txtSearchPatient.Text.Trim().ToLower());
                }
                else if (!this.txtSearchDrug.Text.Trim().Equals(""))
                {
                    //Invs = unitOfWork.InvoiceRepository.GetInvoicesWithNarcoticDrugs(dtpFrom.Value, dtpTo.Value, txtSearchDrug.Text.Trim().ToLower(), true);
                }
                else
                {
                    //Invs = unitOfWork.InvoiceRepository.GetInvoicesWithNarcoticDrugs(dtpFrom.Value, dtpTo.Value);
                }
            }
            double Quantity = 0;
            double ReturnedQuantity = 0;
            double TotalRefund = 0;
            double Value = 0;

            //foreach (NarcoticsDrugInvoiceVM i in Invs)
            //{
            //    i.InvoiceNoString = i.InvoiceId.ToString("D6");
            //    TotalRefund = 0;
            //    ReturnedQuantity = 0;
            //    Quantity = 0;
            //    Value = 0;
            //    if (i.DiscountType == 2)
            //    {
            //        i.TotalDiscount = (i.TotalDiscount / 100) * i.SubTotal;
            //    }

            //    foreach (InvoiceItemVM ii in i.InvoiceItems)
            //    {
            //        i.NarcoticDrugs += ii.ItemName + ", ";
            //        Quantity += ii.Quantity;
            //        ReturnedQuantity += ii.ReturnedQuantity;
            //        if (i.Payment < 0)
            //        {
            //            TotalRefund += (((ii.NetAmount / (i.SubTotal - i.TotalDiscount)) * i.TotalPaid) / Quantity) * ReturnedQuantity;
            //        }
            //        Value += ((ii.NetAmount / (i.SubTotal - i.TotalDiscount)) * i.Payment);
            //    }
            //    if (!string.IsNullOrEmpty(i.NarcoticDrugs))
            //    {
            //        i.NarcoticDrugs = i.NarcoticDrugs.Trim().TrimEnd(',');
            //    }
            //    if (i.PaymentDate == DateTime.MinValue)
            //    {
            //        i.PaymentDateString = "";
            //    }
            //    else
            //    {
            //        i.PaymentDateString = i.PaymentDate.ToString();
            //    }
            //    //if (i.TotalRefund < 0)
            //    //{
            //    //    NarcoticsDrugInvoiceVM newT = new NarcoticsDrugInvoiceVM();
            //    //    newT.InvoiceId = i.InvoiceId;
            //    //    newT.InvoiceNoString = i.InvoiceId.ToString("D6");
            //    //    newT.CreatedAt = i.CreatedAt;
            //    //    newT.GrandTotal = i.GrandTotal;
            //    //    newT.SubTotal = i.SubTotal;
            //    //    newT.TotalDiscount  = i.TotalDiscount;
            //    //    newT.DiscountType  = i.DiscountType;
            //    //    newT.PaymentDate = i.PaymentDate;
            //    //    newT.Quantity = ReturnedQuantity;
            //    //    newT.Value = -1 * TotalRefund;
            //    //    newT.PaymentDateString = i.PaymentDateString;
            //    //    newT.PatientName = i.PatientName;
            //    //    newT.TotalPaid = i.TotalRefund;
            //    //    newT.TotalRefund = i.TotalRefund;
            //    //    newT.InvoiceItems = i.InvoiceItems;
            //    //    newT.NarcoticDrugs = i.NarcoticDrugs;
            //    //    FinalTransactions.Add(newT);                    
            //    //}
            //    i.Quantity = Quantity;
            //    if(i.Payment < 0)
            //    {
            //        i.Value = -1 * TotalRefund;
            //    }
            //    else
            //    {
            //        i.Value = Value;
            //    }
            //    //FinalTransactions.Add(i);                
            //}
            //return FinalTransactions;
            //return Invs;
            return null;
        }
        private void SavePdf(int Orientation, List<NarcoticsDrugInvoiceVM> Invoices)
        {
            try
            {             
                if (Orientation == 0)
                {
                    NarcoticDrugsRpt_Portrait rpt = new NarcoticDrugsRpt_Portrait();
                    rpt.SetDataSource(Invoices);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Narcotic Drugs Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    NarcoticDrugsRpt_Landscape rpt = new NarcoticDrugsRpt_Landscape();
                    rpt.SetDataSource(Invoices);
                    rpt.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    rpt.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    rpt.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    rpt.SetParameterValue("DateFrom", this.dtpFrom.Value);
                    rpt.SetParameterValue("DateTo", this.dtpTo.Value);
                    rpt.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                    if (dlgSavePdf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, dlgSavePdf.FileName);
                        MessageBox.Show("Narcotic Drugs Saved Successfully." + Environment.NewLine + "Destination:" + dlgSavePdf.FileName, "Save Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            PageNo += 1;
            LoadDrugs();
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            PageNo = Invoices.PageCount;
            LoadDrugs();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            PageNo -= 1;
            LoadDrugs();
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            PageNo = 1;
            LoadDrugs();
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                if (SharedVariables.AdminPractiseSetting == null)
                {
                    throw new Exception("Failed to load report headers, please try after login again");
                }
                List<NarcoticsDrugInvoiceVM> Invoices = GetReportData();
                NarcoticDrugsRpt_Landscape rpt = new NarcoticDrugsRpt_Landscape();
                rpt.Database.Tables[0].SetDataSource(Invoices);
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
                rpt.SetParameterValue("TotalPaid", Invoices.Sum(rs=>rs.TotalPaid));
                if (dlgSaveExcel.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    rpt.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.Excel, dlgSaveExcel.FileName);
                    MessageBox.Show("Narcotic Drugs Data Exported Successfully." + Environment.NewLine + "Destination:" + dlgSaveExcel.FileName, "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Export Failed");
            }
        }

        private void grdData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) { return; }
            if(e.ColumnIndex == grdData.Columns["colPrint"].Index)
            {
                long InvoiceId = Convert.ToInt64(grdData.Rows[e.RowIndex].Cells["colInvoiceId"].Value);
                Reports.POSInvoices.POSInvoiceViewer v = new POSInvoices.POSInvoiceViewer(InvoiceId, true);
                v.Show();
            }
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            this.LoadDrugs();
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            PageNo = 1;
            this.LoadDrugs();
        }

        private void txtSearchInv_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if (!txtSearchInv.Text.Trim().Equals(""))
                {                    
                    txtSearchDrug.Text = "";
                    txtSearchPatient.Text = "";
                    PageNo = 1;
                    this.LoadDrugs();
                }
            }
        }

        private void txtSearchPatient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtSearchPatient.Text.Trim().Equals(""))
                {
                    txtSearchInv.Text = "";
                    txtSearchDrug.Text = "";
                    PageNo = 1;
                    this.LoadDrugs();
                }
            }
        }

        private void txtSearchDrug_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!txtSearchDrug.Text.Trim().Equals(""))
                {
                    txtSearchInv.Text = "";
                    txtSearchPatient.Text = "";
                    PageNo = 1;
                    this.LoadDrugs();
                }
            }
        }

        private void txtSearchInv_KeyPress(object sender, KeyPressEventArgs e)
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
                    if (_pageNo <= 0 || _pageNo > Invoices.PageCount)
                    {
                        MessageBox.Show("Please enter a valid page no.", "Invalid Page No", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    this.PageNo = _pageNo;
                    LoadDrugs();
                }
            }
        }    
    }
}