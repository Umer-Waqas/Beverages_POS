using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using GK.Shared.Core;
using Restaurant_MS_Core.ViewModels;
using GK.Shared.Repository;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.Reports.POSInvoices
{
    public partial class POSInvoiceViewer : Form
    {
        string printerName = SharedVariables.AdminInvoiceSetting.PrinterName;//System.Configuration.ConfigurationManager.ConnectionStrings["PrinterName"].ToString();
        private long InvoiceId;
        private bool IsDetailedInvoice;
        UnitOfWork unitOfWork;
        public POSInvoiceViewer()
        {
            InitializeComponent();
        }

        public void Print()
        {
            try
            {
                if (IsDetailedInvoice)
                {
                    InvoiceVM i = new InvoiceVM();
                    using (unitOfWork = new UnitOfWork())
                    {
                        i = unitOfWork.InvoiceRepository.GetPOSInvoiceDetailById(this.InvoiceId);
                    }

                    //i.InvoicePayments.ToList().RemoveAt(0);
                    //if (SharedVariables.AdminInvoiceSetting == null)
                    //{
                    //    DefaultDetailedPrint(i);
                    //    return;
                    //}
                    if (i.DiscountType == 2)
                    {
                        i.TotalDiscount = (i.SubTotal / 100) * i.TotalDiscount;
                    }
                    foreach (InvoicePaymentVM v in i.InvoicePayments)
                    {
                        if (v.PaymentType == 1) { v.PaymentTypeString = "Cash"; }
                        else if (v.PaymentType == 2) { v.PaymentTypeString = "Cheque"; }
                        else if (v.PaymentType == 3) { v.PaymentTypeString = "Debit/Credit Card"; }
                        else if (v.PaymentType == 4) { v.PaymentTypeString = "Online Payment"; }
                    }
                    //                    Dine In
                    //Take Away
                    //Delivery
                    //Guest
                    //Self
                    //Staff
                    if (i.OrderType == 1) { i.OrderTypeString = "Dine In"; }
                    else if (i.OrderType == 2) { i.OrderTypeString = "Take Away"; }
                    else if (i.OrderType == 3) { i.OrderTypeString = "Delivery"; }
                    else if (i.OrderType == 4) { i.OrderTypeString = "Guest"; }
                    else if (i.OrderType == 5) { i.OrderTypeString = "Self"; }
                    else if (i.OrderType == 6) { i.OrderTypeString = "Staff"; }

                    if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A4)
                    {
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                        {
                            POSInvoiceA4_Detailed_Portrait r = new POSInvoiceA4_Detailed_Portrait();
                            //PageMargins p = new PageMargins();
                            //p.leftMargin = 2000;
                            //p.rightMargin= 2000;                            
                            //p.topMargin= 5000;
                            //r.PrintOptions.ApplyPageMargins(p);
                            //PageMargins p = r.PrintOptions.PageMargins.topMargin;
                            //PageMargins p = r.PrintOptions.PageMargins.rightMargin;
                            //PageMargins p = r.PrintOptions.PageMargins.leftMargin;
                            //r.Section1.SectionFormat.BackgroundColor = Color.Green;//.ReportObjects["PharmacyName"].Left = 20;
                            //int l = r.ReportDefinition.ReportObjects["PharmacyName1"].Left = 5000;

                            //int w = r.ReportDefinition.ReportObjects["PharmacyName1"].Width;
                            //Console.WriteLine("w=> " + w.ToString());
                            //r.ReportDefinition.ReportObjects["LogoImage1"].Left = 5000;
                            //foreach (ReportObject reportObject in r.ReportDefinition.ReportObjects)
                            //{
                            //    Console.WriteLine("=> " + reportObject.Name + Environment.NewLine);
                            //}
                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }

                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreatedAt);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }


                            r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            r.SetParameterValue("TotalDiscount", "Rs. " + Math.Round(i.TotalDiscount).ToString());
                            r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.SubTotal - i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.SubTotal - i.TotalDiscount;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            double RefundAmount = i.TotalRefund;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            r.SetParameterValue("RefundAmountNumVal", RefundAmount);
                            r.SetParameterValue("RefundAmount", "Rs. " + Math.Round(RefundAmount).ToString());

                            r.SetParameterValue("HeaderFooterPref", SharedVariables.AdminPrintFormatSetting.HeaderFooterPref);
                            r.SetParameterValue("PatientDetailPref", SharedVariables.AdminPrintFormatSetting.PatientDetailsPref);
                            r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                            r.SetParameterValue("Setting_ShowUserName", SharedVariables.AdminInvoiceSetting.ShowUserName);
                            r.SetParameterValue("Setting_HideRateAndQuantity", SharedVariables.AdminInvoiceSetting.HideRateAndQuantityInPrintFormat);
                            r.SetParameterValue("Setting_HideDuesAndAdvanceFromInvoiceAndPayamnetReceipts", SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts);
                            r.SetParameterValue("Setting_PrintPractiseName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
                            r.SetParameterValue("A4_Col_Headers_Format", SharedVariables.AdminInvoiceSetting.A4_cols_format);

                            //crystalReportViewer1.ReportSource = r;
                            //crystalReportViewer1.Show();
                            try
                            {
                                r.PrintOptions.PrinterName = this.printerName;
                                r.PrintToPrinter(1, true, 0, 0);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Could not find printer, please specify printer name under => USers/AdminSetting/POS Print Setting", "Invalid Printer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                        {
                            POSInvoiceA4_Detailed_Landscape r = new POSInvoiceA4_Detailed_Landscape();
                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }

                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreatedAt);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            //crystalReportViewer1.ReportSource = r;
                            //crystalReportViewer1.Show();
                            try
                            {
                                r.PrintOptions.PrinterName = this.printerName;
                                r.PrintToPrinter(1, true, 0, 0);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Could not find printer, please specify printer name under => USers/AdminSetting/POS Print Setting", "Invalid Printer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A5)
                    {
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                        {
                            POSInvoiceA5_Detailed_Portrait r = new POSInvoiceA5_Detailed_Portrait();
                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }

                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);

                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreatedAt);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            //crystalReportViewer1.ReportSource = r;
                            //crystalReportViewer1.Show();
                            try
                            {
                                r.PrintOptions.PrinterName = this.printerName;
                                r.PrintToPrinter(1, true, 0, 0);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Could not find printer, please specify printer name under => USers/AdminSetting/POS Print Setting", "Invalid Printer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                        {
                        }
                    }
                    else
                    {
                        POSInvoiceA8_Detailed r = new POSInvoiceA8_Detailed();
                        //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                        r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                        r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                        if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                        {
                            r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                        }
                        if (SharedVariables.AdminPractiseSetting.Enable_FBR == "1" && !string.IsNullOrEmpty(i.FBR_InvoiceNo))
                        {
                            r.SetParameterValue("fbr_invoice_no", i.FBR_InvoiceNo);
                            QRCodeGenerator qr = new QRCodeGenerator();
                            QRCodeData data = qr.CreateQrCode(i.FBR_InvoiceNo, QRCodeGenerator.ECCLevel.Q);
                            QRCode code = new QRCode(data);
                            var image = code.GetGraphic(3);
                            //qrCodeContainer.Image = image;


                            DataTable dtFBR_Data = new DataTable();

                            dtFBR_Data.Columns.Add(new DataColumn("QRCode", typeof(byte[])));
                            dtFBR_Data.Columns.Add(new DataColumn("FbrLogo", typeof(byte[])));

                            // qr_code
                            byte[] ImageBytes_qrcode = SharedFunctions.ImageToByteArray(image);
                            DataRow dr = dtFBR_Data.NewRow();
                            dr[0] = ImageBytes_qrcode;

                            // fbr_logo
                            Image img = Properties.Resources.fbr_logo2;
                            //string fbrlogoPath = @"d:\hyperion bytes\giga keys solutions\gk_restaurant_ms\gk_restaurant\pharmacy.ui\bin\debug\images\fbr_logo2.jpg";
                            byte[] ImageBytes_fbrLogo = SharedFunctions.ImageToByteArray(img);
                            dr[1] = ImageBytes_fbrLogo;
                            dtFBR_Data.Rows.Add(dr);
                            r.Database.Tables["dtFbrData"].SetDataSource(dtFBR_Data);
                        }
                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                        r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                        r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                        r.SetParameterValue("CreateDate", i.CreatedAt);
                        r.SetParameterValue("InvoiceNote", i.Note);
                        r.SetParameterValue("fbr_enabled", false); // SharedVariables.AdminPractiseSetting.Enable_FBR);
                        r.SetParameterValue("fbr_invoice_no", string.IsNullOrEmpty(i.FBR_InvoiceNo) ? "" : i.FBR_InvoiceNo);

                        if (i.ObjPatient == null)
                        {
                            r.SetParameterValue("PatientName", "");
                            r.SetParameterValue("MrNo", "");
                            r.SetParameterValue("PatientPhone", "");
                            r.SetParameterValue("Gender", "");
                            r.SetParameterValue("DateOfBirth", "");
                            r.SetParameterValue("Age", "");
                            r.SetParameterValue("PatientAddress", "");
                        }
                        else
                        {
                            r.SetParameterValue("PatientName", i.ObjPatient.Name);
                            r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                            r.SetParameterValue("PatientAddress", i.ObjPatient.Address);

                            r.SetParameterValue("Gender", i.ObjPatient.Gender);
                            r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.ObjPatient.DateOfBirth.ToString());
                            r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                        }


                        r.SetParameterValue("ShowPhone", SharedVariables.AdminInvoiceSetting.ShowPhone);
                        r.SetParameterValue("ShowMr", SharedVariables.AdminInvoiceSetting.ShowMR);
                        r.SetParameterValue("ShowGender", SharedVariables.AdminInvoiceSetting.ShowGender);
                        r.SetParameterValue("ShowDOB", SharedVariables.AdminInvoiceSetting.ShowDOB);
                        r.SetParameterValue("ShowAge", SharedVariables.AdminInvoiceSetting.ShowAge);

                        r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                        r.SetParameterValue("TotalDiscount", "Rs. " + Math.Round(i.TotalDiscount).ToString());
                        r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.SubTotal - i.TotalDiscount).ToString() + "/-");
                        //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                        r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                        double AdvanceAmount = i.TotalPaid - i.SubTotal - i.TotalDiscount;
                        double DueAmount = i.GrandTotal - i.TotalPaid;
                        double RefundAmount = i.TotalRefund;
                        r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                        r.SetParameterValue("DueAmountNumVal", DueAmount);
                        r.SetParameterValue("RefundAmountNumVal", RefundAmount);
                        r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                        r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                        r.SetParameterValue("RefundAmount", "Rs. " + Math.Round(RefundAmount).ToString());
                        r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                        r.SetParameterValue("Setting_ShowUserName", SharedVariables.AdminInvoiceSetting.ShowUserName);
                        r.SetParameterValue("Setting_HideRateAndQuantity", SharedVariables.AdminInvoiceSetting.HideRateAndQuantityInPrintFormat);
                        r.SetParameterValue("Setting_HideDuesAndAdvanceFromInvoiceAndPayamnetReceipts", SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts);

                        r.SetParameterValue("ShowPharmaName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
                        r.SetParameterValue("ShowPharmaPhone", SharedVariables.AdminInvoiceSetting.ShowPharmaPhone);
                        r.SetParameterValue("ShowPharmaAddress", SharedVariables.AdminInvoiceSetting.ShowPharmaAddress);
                        r.SetParameterValue("InvoiceLAyout", SharedVariables.AdminInvoiceSetting.InvoiceLayout);
                        r.SetParameterValue("OrderType", i.OrderType);
                        r.SetParameterValue("OrderTypeString", i.OrderTypeString);

                        //crystalReportViewer1.Show();
                        //crystalReportViewer1.PrintMode = CrystalDecisions.Windows.Forms.PrintMode.PrintToPrinter;
                        try
                        {
                            if (
                                SharedVariables.AdminPractiseSetting.PracticeId == 9
                                || SharedVariables.AdminPractiseSetting.PracticeId == 4
                                || SharedVariables.AdminPractiseSetting.PracticeId == 11
                                )// needed to add this, direct print was not working for this.
                            {
                                crystalReportViewer1.ReportSource = r;
                                crystalReportViewer1.ShowPrintButton = false;
                                crystalReportViewer1.PrintReport();
                            }
                            else
                            {
                                r.PrintOptions.PrinterName = this.printerName;
                                r.PrintToPrinter(1, true, 0, 0);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Verify your printer name. " + this.printerName + ", please specify printer name under => USers/AdminSetting/POS Print Setting" + Environment.NewLine + "Exception message :: " + ex.Message, "Invalid Printer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }

                }
                else
                {
                    InvoiceVM i = new InvoiceVM();
                    using (unitOfWork = new UnitOfWork())
                    {
                        i = unitOfWork.InvoiceRepository.GetPOSInvoiceById(this.InvoiceId);
                    }
                    string Description = "";
                    foreach (InvoiceItemVM item in i.InvoiceItems)
                    {
                        Description += item.ItemName + ", ";
                    }
                    Description = Description.Trim().TrimEnd(',');
                    string PaymentMode = getPaymentModeString(i.PaymentType);
                    //if(SharedVariables.AdminInvoiceSetting == null)
                    //{
                    //    DefaultGeneralPrint(i, Description);
                    //    return;
                    //}
                    if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A4)
                    {
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                        {
                            POSInvoiceA4_Portrait r = new POSInvoiceA4_Portrait();
                            //string path = @"D:\DesktopApplications\Pharmacy\Pharmacy - InvoiceLinwItemRemove\Pharmacy.UI\Theme\HWlogoWithoutText.jpg";
                            //DataTable dt = SharedFunctions.GetImageTable(path);                            
                            //r.Database.Tables[3].SetDataSource(dt);
                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }
                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreatedAt);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("Description", Description);
                            r.SetParameterValue("PaymentMode", PaymentMode);

                            //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            //crystalReportViewer1.ReportSource = r;
                            //crystalReportViewer1.Show();
                            try
                            {
                                r.PrintOptions.PrinterName = this.printerName;
                                r.PrintToPrinter(1, true, 0, 0);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Could not find printer, please specify printer name under => USers/AdminSetting/POS Print Setting", "Invalid Printer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                        {
                            POSInvoiceA4_Landscape r = new POSInvoiceA4_Landscape();

                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }
                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreatedAt);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("Description", Description);
                            r.SetParameterValue("PaymentMode", PaymentMode);

                            //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            //crystalReportViewer1.ReportSource = r;
                            //crystalReportViewer1.Show();
                            try
                            {
                                r.PrintOptions.PrinterName = this.printerName;
                                r.PrintToPrinter(1, true, 0, 0);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Could not find printer, please specify printer name under => USers/AdminSetting/POS Print Setting", "Invalid Printer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A5)
                    {
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                        {
                            POSInvoiceA5_Portrait r = new POSInvoiceA5_Portrait();

                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }

                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreatedAt);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("Description", Description);
                            r.SetParameterValue("PaymentMode", PaymentMode);

                            //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            //crystalReportViewer1.ReportSource = r;
                            //crystalReportViewer1.Show();
                            try
                            {
                                r.PrintOptions.PrinterName = this.printerName;
                                r.PrintToPrinter(1, true, 1, 1);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Could not find printer, please specify printer name under => USers/AdminSetting/POS Print Setting", "Invalid Printer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        else if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                        {
                            POSInvoiceA5_Landscape r = new POSInvoiceA5_Landscape();

                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }

                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreatedAt);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("Description", Description);
                            r.SetParameterValue("PaymentMode", PaymentMode);

                            //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            //crystalReportViewer1.ReportSource = r;
                            //crystalReportViewer1.Show();
                            try
                            {
                                r.PrintOptions.PrinterName = this.printerName;
                                r.PrintToPrinter(1, true, 0, 0);
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Could not find printer, please specify printer name under => USers/AdminSetting/POS Print Setting", "Invalid Printer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    else //A8
                    {
                        POSInvoiceA8 r = new POSInvoiceA8();
                        //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                        //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                        //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                        if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                        {
                            r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                        }
                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                        r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                        r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                        r.SetParameterValue("CreateDate", i.CreatedAt);
                        r.SetParameterValue("InvoiceNote", i.Note);
                        if (i.ObjPatient == null)
                        {
                            r.SetParameterValue("PatientName", "");
                            r.SetParameterValue("MrNo", "");
                            r.SetParameterValue("PatientPhone", "");
                            r.SetParameterValue("Gender", "");
                            r.SetParameterValue("DateOfBirth", "");
                            r.SetParameterValue("Age", "");
                        }
                        else
                        {
                            r.SetParameterValue("PatientName", i.ObjPatient.Name);
                            r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                            r.SetParameterValue("Gender", i.ObjPatient.Gender);
                            r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                            r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                        }


                        //r.SetParameterValue("ShowPhone", SharedVariables.AdminInvoiceSetting.ShowPhone);
                        //r.SetParameterValue("ShowMr", SharedVariables.AdminInvoiceSetting.ShowMR);
                        //r.SetParameterValue("ShowGender", SharedVariables.AdminInvoiceSetting.ShowGender);
                        //r.SetParameterValue("ShowDOB", SharedVariables.AdminInvoiceSetting.ShowDOB);
                        //r.SetParameterValue("ShowAge", SharedVariables.AdminInvoiceSetting.ShowAge);

                        r.SetParameterValue("Description", Description);
                        r.SetParameterValue("PaymentMode", PaymentMode);

                        //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                        //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                        //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                        r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                        double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                        double DueAmount = i.GrandTotal - i.TotalPaid;
                        r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                        r.SetParameterValue("DueAmountNumVal", DueAmount);
                        r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                        r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());

                        //r.SetParameterValue("ShowPharmaName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
                        //r.SetParameterValue("ShowPharmaPhone", SharedVariables.AdminInvoiceSetting.ShowPharmaPhone);
                        //r.SetParameterValue("ShowPharmaAddress", SharedVariables.AdminInvoiceSetting.ShowPharmaAddress);
                        //crystalReportViewer1.ReportSource = r;
                        //crystalReportViewer1.Show();
                        try
                        {
                            if (SharedVariables.AdminPractiseSetting.PracticeId == 9
                                || SharedVariables.AdminPractiseSetting.PracticeId == 4
                                || SharedVariables.AdminPractiseSetting.PracticeId == 11
                                )// needed to add this, direct print was not working for this.
                            {
                                crystalReportViewer1.ReportSource = r;
                                crystalReportViewer1.ShowPrintButton = false;
                                crystalReportViewer1.PrintReport();
                            }
                            else
                            {
                                r.PrintOptions.PrinterName = this.printerName;
                                r.PrintToPrinter(1, true, 0, 0);
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Could not find printer, please specify printer name under => USers/AdminSetting/POS Print Setting", "Invalid Printer Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.InnerException.Message, "Error");
                }
                else
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                }
            }
        }

        public POSInvoiceViewer(long InvoiceId, bool IsDetailedInvoice)
        {
            InitializeComponent();
            this.InvoiceId = InvoiceId;
            this.IsDetailedInvoice = IsDetailedInvoice;
        }

        //private void DefaultDetailedPrint(InvoiceVM i)
        //{
        //    POSInvoiceA4_Detailed_Portrait r = new POSInvoiceA4_Detailed_Portrait();
        //    //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
        //    foreach (InvoicePaymentVM v in i.InvoicePayments)
        //    {
        //        if (v.PaymentType == 1) { v.PaymentTypeString = "Cash"; }
        //        else if (v.PaymentType == 2) { v.PaymentTypeString = "Cheque"; }
        //        else if (v.PaymentType == 3) { v.PaymentTypeString = "Debit/Credit Card"; }
        //        else if (v.PaymentType == 4) { v.PaymentTypeString = "Online Payment"; }
        //    }
        //    r.Database.Tables[1].SetDataSource(i.InvoiceItems);
        //    r.Database.Tables[2].SetDataSource(i.InvoicePayments);
        //    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
        //    {
        //        r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
        //    }           
        //    r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
        //    r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
        //    r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);          
        //    r.SetParameterValue("InvoiceId", i.InvoiceId.ToString("D6"));
        //    r.SetParameterValue("CreatedAt", i.CreatedAt);
        //    if (i.ObjPatient == null)
        //    {
        //        r.SetParameterValue("PatientName", "");
        //        r.SetParameterValue("MrNo", "");
        //        r.SetParameterValue("PatientPhone", "");
        //        r.SetParameterValue("Gender", "");
        //        r.SetParameterValue("DateOfBirth", "");
        //        r.SetParameterValue("Age", "");
        //    }
        //    else
        //    {
        //        r.SetParameterValue("PatientName", i.ObjPatient.Name);
        //        r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
        //        r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
        //        r.SetParameterValue("Gender", i.ObjPatient.Gender == 1 ? "Male" : "Female");
        //        r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
        //        r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
        //    }

        //    r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
        //    r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
        //    //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
        //    r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
        //    double AdvanceAmount = i.TotalPaid - i.GrandTotal;
        //    double DueAmount = i.GrandTotal - i.TotalPaid;
        //    r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
        //    r.SetParameterValue("DueAmountNumVal", DueAmount);
        //    r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
        //    r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
        //    crystalReportViewer1.ReportSource = r;
        //    crystalReportViewer1.Show();
        //}

        //private void DefaultGeneralPrint(InvoiceVM i, string Description)
        //{
        //    POSInvoiceA4_Portrait r = new POSInvoiceA4_Portrait();

        //    //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
        //    //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
        //    //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
        //    if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
        //    {
        //        r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
        //    }

        //    //r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
        //    //r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
        //    //r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone); 

        //    r.SetParameterValue("InvoiceId", i.InvoiceId.ToString("D6"));
        //    r.SetParameterValue("CreatedAt", i.CreatedAt);
        //    if (i.ObjPatient == null)
        //    {
        //        r.SetParameterValue("PatientName", "");
        //        r.SetParameterValue("MrNo", "");
        //        r.SetParameterValue("PatientPhone", "");
        //        r.SetParameterValue("Gender", "");
        //        r.SetParameterValue("DateOfBirth", "");
        //        r.SetParameterValue("Age", "");
        //    }
        //    else
        //    {
        //        r.SetParameterValue("PatientName", i.ObjPatient.Name);
        //        r.SetParameterValue("MrNo", i.ObjPatient.MRNo);
        //        r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
        //        r.SetParameterValue("Gender", i.ObjPatient.Gender == 1 ? "Male" : "Female");
        //        r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
        //        r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
        //    }

        //    r.SetParameterValue("Description", Description);
        //    string PaymentMode = "";
        //    if (i.PaymentType == 1) { PaymentMode = "Cash"; }
        //    else if (i.PaymentType == 2) { PaymentMode = "Cheque"; }
        //    else if (i.PaymentType == 3) { PaymentMode = "Debit/Credit Card"; }
        //    else if (i.PaymentType == 4) { PaymentMode = "Online Payment"; }

        //    r.SetParameterValue("PaymentMode", PaymentMode);

        //    //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
        //    //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
        //    //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
        //    r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(i.TotalPaid));
        //    double AdvanceAmount = i.TotalPaid - i.GrandTotal;
        //    double DueAmount = i.GrandTotal - i.TotalPaid;
        //    r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
        //    r.SetParameterValue("DueAmountNumVal", DueAmount);
        //    r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
        //    r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
        //    crystalReportViewer1.ReportSource = r;
        //    crystalReportViewer1.Show();
        //}
        private void POSInvoiceViewer_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsDetailedInvoice)
                {
                    InvoiceVM i = new InvoiceVM();
                    using (unitOfWork = new UnitOfWork())
                    {
                        i = unitOfWork.InvoiceRepository.GetPOSInvoiceDetailById(this.InvoiceId);
                    }
                    //i.InvoicePayments.ToList().RemoveAt(0);
                    //if (SharedVariables.AdminInvoiceSetting == null)
                    //{
                    //    DefaultDetailedPrint(i);
                    //    return;
                    //}
                    if (i.DiscountType == 2)
                    {
                        i.TotalDiscount = (i.SubTotal / 100) * i.TotalDiscount;
                    }
                    foreach (InvoicePaymentVM v in i.InvoicePayments)
                    {
                        if (v.PaymentType == 1) { v.PaymentTypeString = "Cash"; }
                        else if (v.PaymentType == 2) { v.PaymentTypeString = "Cheque"; }
                        else if (v.PaymentType == 3) { v.PaymentTypeString = "Debit/Credit Card"; }
                        else if (v.PaymentType == 4) { v.PaymentTypeString = "Online Payment"; }
                    }

                    if (i.OrderType == 1) { i.OrderTypeString = "Dine In"; }
                    else if (i.OrderType == 2) { i.OrderTypeString = "Take Away"; }
                    else if (i.OrderType == 3) { i.OrderTypeString = "Delivery"; }
                    else if (i.OrderType == 4) { i.OrderTypeString = "Guest"; }
                    else if (i.OrderType == 5) { i.OrderTypeString = "Self"; }
                    else if (i.OrderType == 6) { i.OrderTypeString = "Staff"; }

                    if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A4)
                    {
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                        {
                            POSInvoiceA4_Detailed_Portrait r = new POSInvoiceA4_Detailed_Portrait();
                            //PageMargins p = new PageMargins();
                            //p.leftMargin = 2000;
                            //p.rightMargin= 2000;                            
                            //p.topMargin= 5000;
                            //r.PrintOptions.ApplyPageMargins(p);
                            //PageMargins p = r.PrintOptions.PageMargins.topMargin;
                            //PageMargins p = r.PrintOptions.PageMargins.rightMargin;
                            //PageMargins p = r.PrintOptions.PageMargins.leftMargin;
                            //r.Section1.SectionFormat.BackgroundColor = Color.Green;//.ReportObjects["PharmacyName"].Left = 20;
                            //int l = r.ReportDefinition.ReportObjects["PharmacyName1"].Left = 5000;

                            //int w = r.ReportDefinition.ReportObjects["PharmacyName1"].Width;
                            //Console.WriteLine("w=> " + w.ToString());
                            //r.ReportDefinition.ReportObjects["LogoImage1"].Left = 5000;
                            //foreach (ReportObject reportObject in r.ReportDefinition.ReportObjects)
                            //{
                            //    Console.WriteLine("=> " + reportObject.Name + Environment.NewLine);
                            //}
                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }

                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreatedAt);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                                r.SetParameterValue("PatientAddress", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.ObjPatient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                                r.SetParameterValue("PatientAddress", i.ObjPatient.Address);
                            }


                            r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            r.SetParameterValue("TotalDiscount", "Rs. " + Math.Round(i.TotalDiscount).ToString());
                            r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.SubTotal - i.TotalDiscount;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            double RefundAmount = i.TotalRefund;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            r.SetParameterValue("RefundAmountNumVal", RefundAmount);
                            r.SetParameterValue("RefundAmount", "Rs. " + Math.Round(RefundAmount).ToString());
                            r.SetParameterValue("OrderTypeString", i.OrderTypeString);

                            r.SetParameterValue("HeaderFooterPref", SharedVariables.AdminPrintFormatSetting.HeaderFooterPref);
                            r.SetParameterValue("PatientDetailPref", SharedVariables.AdminPrintFormatSetting.PatientDetailsPref);
                            r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                            r.SetParameterValue("Setting_ShowUserName", SharedVariables.AdminInvoiceSetting.ShowUserName);
                            r.SetParameterValue("Setting_HideRateAndQuantity", SharedVariables.AdminInvoiceSetting.HideRateAndQuantityInPrintFormat);
                            r.SetParameterValue("Setting_HideDuesAndAdvanceFromInvoiceAndPayamnetReceipts", SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts);
                            r.SetParameterValue("Setting_PrintPractiseName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
                            r.SetParameterValue("A4_Col_Headers_Format", SharedVariables.AdminInvoiceSetting.A4_cols_format);
                            crystalReportViewer1.ReportSource = r;
                            crystalReportViewer1.Show();
                        }
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                        {
                            POSInvoiceA4_Detailed_Landscape r = new POSInvoiceA4_Detailed_Landscape();
                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }

                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreatedAt", i.CreatedAt);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            crystalReportViewer1.ReportSource = r;
                            crystalReportViewer1.Show();
                        }
                    }
                    else if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A5)
                    {
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                        {
                            POSInvoiceA5_Detailed_Portrait r = new POSInvoiceA5_Detailed_Portrait();
                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }

                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);

                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreatedAt", i.CreatedAt);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            crystalReportViewer1.ReportSource = r;
                            crystalReportViewer1.Show();
                        }
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                        {
                        }
                    }
                    else
                    {
                        POSInvoiceA8_Detailed r = new POSInvoiceA8_Detailed();
                        //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                        r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                        r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                        if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                        {
                            r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                        }
                        if (SharedVariables.AdminPharmacySetting.EnableFBRIntegration && !string.IsNullOrEmpty(i.FBR_InvoiceNo))
                        {
                            QRCodeGenerator qr = new QRCodeGenerator();
                            QRCodeData data = qr.CreateQrCode(i.FBR_InvoiceNo, QRCodeGenerator.ECCLevel.Q);
                            QRCode code = new QRCode(data);
                            var image = code.GetGraphic(3);
                            //qrCodeContainer.Image = image;


                            DataTable dtFBR_Data = new DataTable();

                            dtFBR_Data.Columns.Add(new DataColumn("QRCode", typeof(byte[])));
                            dtFBR_Data.Columns.Add(new DataColumn("FbrLogo", typeof(byte[])));

                            // qr_code
                            byte[] ImageBytes_qrcode = SharedFunctions.ImageToByteArray(image);
                            DataRow dr = dtFBR_Data.NewRow();
                            dr[0] = ImageBytes_qrcode;

                            // fbr_logo
                            Image img = Properties.Resources.fbr_logo;
                            byte[] ImageBytes_fbrLogo = SharedFunctions.ImageToByteArray(img);
                            dr[0] = ImageBytes_fbrLogo;
                            dtFBR_Data.Rows.Add(dr);
                            r.Database.Tables["dtFbrData"].SetDataSource(dtFBR_Data);
                        }
                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                        r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                        r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                        r.SetParameterValue("CreateDate", i.CreatedAt);
                        r.SetParameterValue("InvoiceNote", i.Note);

                        r.SetParameterValue("fbr_invoice_no", string.IsNullOrEmpty(i.FBR_InvoiceNo) ? "" : i.FBR_InvoiceNo);
                        r.SetParameterValue("fbr_enabled", false); // SharedVariables.AdminPharmacySetting.EnableFBRIntegration);


                        // section to load qrcode and fbr logo on invoice


                        if (i.ObjPatient == null)
                        {
                            r.SetParameterValue("PatientName", "");
                            r.SetParameterValue("MrNo", "");
                            r.SetParameterValue("PatientPhone", "");
                            r.SetParameterValue("Gender", "");
                            r.SetParameterValue("DateOfBirth", "");
                            r.SetParameterValue("Age", "");
                            r.SetParameterValue("PatientAddress", "");
                        }
                        else
                        {
                            r.SetParameterValue("PatientName", i.ObjPatient.Name);
                            r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                            r.SetParameterValue("Gender", i.ObjPatient.Gender);
                            r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.ObjPatient.DateOfBirth.ToString());
                            r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            r.SetParameterValue("PatientAddress", i.ObjPatient.Address);
                        }

                        r.SetParameterValue("ShowPhone", SharedVariables.AdminInvoiceSetting.ShowPhone);
                        r.SetParameterValue("ShowMr", SharedVariables.AdminInvoiceSetting.ShowMR);
                        r.SetParameterValue("ShowGender", SharedVariables.AdminInvoiceSetting.ShowGender);
                        r.SetParameterValue("ShowDOB", SharedVariables.AdminInvoiceSetting.ShowDOB);
                        r.SetParameterValue("ShowAge", SharedVariables.AdminInvoiceSetting.ShowAge);


                        r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                        r.SetParameterValue("TotalDiscount", "Rs. " + Math.Round(i.TotalDiscount).ToString());
                        r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.SubTotal - i.TotalDiscount).ToString() + "/-");
                        r.SetParameterValue("OrderTypeString", i.OrderTypeString);
                        //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                        r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                        double AdvanceAmount = i.TotalPaid - i.SubTotal - i.TotalDiscount;
                        double DueAmount = i.GrandTotal - i.TotalPaid;
                        double RefundAmount = i.TotalRefund;
                        r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                        r.SetParameterValue("DueAmountNumVal", DueAmount);
                        r.SetParameterValue("RefundAmountNumVal", RefundAmount);
                        r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                        r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                        r.SetParameterValue("RefundAmount", "Rs. " + Math.Round(RefundAmount).ToString());
                        r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
                        r.SetParameterValue("Setting_ShowUserName", SharedVariables.AdminInvoiceSetting.ShowUserName);
                        r.SetParameterValue("Setting_HideRateAndQuantity", SharedVariables.AdminInvoiceSetting.HideRateAndQuantityInPrintFormat);
                        r.SetParameterValue("Setting_HideDuesAndAdvanceFromInvoiceAndPayamnetReceipts", SharedVariables.AdminInvoiceSetting.HideDuesAndAdvanceFromInvoiceAndPmntReceipts);

                        r.SetParameterValue("ShowPharmaName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
                        r.SetParameterValue("ShowPharmaPhone", SharedVariables.AdminInvoiceSetting.ShowPharmaPhone);
                        r.SetParameterValue("ShowPharmaAddress", SharedVariables.AdminInvoiceSetting.ShowPharmaAddress);
                        r.SetParameterValue("InvoiceLAyout", SharedVariables.AdminInvoiceSetting.InvoiceLayout);
                        r.SetParameterValue("OrderType", i.OrderType);
                        r.SetParameterValue("OrderTypeString", i.OrderTypeString);

                        crystalReportViewer1.ReportSource = r;
                        crystalReportViewer1.Show();
                    }

                }
                else
                {
                    InvoiceVM i = new InvoiceVM();
                    using (unitOfWork = new UnitOfWork())
                    {
                        i = unitOfWork.InvoiceRepository.GetPOSInvoiceById(this.InvoiceId);
                    }
                    string Description = "";
                    foreach (InvoiceItemVM item in i.InvoiceItems)
                    {
                        Description += item.ItemName + ", ";
                    }
                    Description = Description.Trim().TrimEnd(',');
                    string PaymentMode = getPaymentModeString(i.PaymentType);
                    //if(SharedVariables.AdminInvoiceSetting == null)
                    //{
                    //    DefaultGeneralPrint(i, Description);
                    //    return;
                    //}
                    if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A4)
                    {
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                        {
                            POSInvoiceA4_Portrait r = new POSInvoiceA4_Portrait();
                            //string path = @"D:\DesktopApplications\Pharmacy\Pharmacy - InvoiceLinwItemRemove\Pharmacy.UI\Theme\HWlogoWithoutText.jpg";
                            //DataTable dt = SharedFunctions.GetImageTable(path);                            
                            //r.Database.Tables[3].SetDataSource(dt);
                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }
                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreatedAt);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("Description", Description);
                            r.SetParameterValue("PaymentMode", PaymentMode);

                            //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            crystalReportViewer1.ReportSource = r;
                            crystalReportViewer1.Show();
                        }
                        else if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                        {
                            POSInvoiceA4_Landscape r = new POSInvoiceA4_Landscape();

                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }
                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreatedAt);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("Description", Description);
                            r.SetParameterValue("PaymentMode", PaymentMode);

                            //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            crystalReportViewer1.ReportSource = r;
                            crystalReportViewer1.Show();
                        }
                    }
                    else if (SharedVariables.AdminInvoiceSetting.PrintPageSize == (int)SharedVariables.PrintPageSize.A5)
                    {
                        if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.Portrait)
                        {
                            POSInvoiceA5_Portrait r = new POSInvoiceA5_Portrait();

                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }

                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreatedDate", i.CreatedAt);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("Description", Description);
                            r.SetParameterValue("PaymentMode", PaymentMode);

                            //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            crystalReportViewer1.ReportSource = r;
                            crystalReportViewer1.Show();
                        }
                        else if (SharedVariables.AdminInvoiceSetting.PageOrientation == (int)SharedVariables.PageOrientation.LandScape)
                        {
                            POSInvoiceA5_Landscape r = new POSInvoiceA5_Landscape();

                            //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                            //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                            //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                            if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                            {
                                r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                            }

                            r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                            r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                            r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                            r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                            r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                            r.SetParameterValue("CreateDate", i.CreatedAt);
                            r.SetParameterValue("InvoiceNote", i.Note);
                            if (i.ObjPatient == null)
                            {
                                r.SetParameterValue("PatientName", "");
                                r.SetParameterValue("MrNo", "");
                                r.SetParameterValue("PatientPhone", "");
                                r.SetParameterValue("Gender", "");
                                r.SetParameterValue("DateOfBirth", "");
                                r.SetParameterValue("Age", "");
                            }
                            else
                            {
                                r.SetParameterValue("PatientName", i.ObjPatient.Name);
                                r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                                r.SetParameterValue("Gender", i.ObjPatient.Gender);
                                r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                                r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                            }

                            r.SetParameterValue("Description", Description);
                            r.SetParameterValue("PaymentMode", PaymentMode);

                            //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                            //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                            //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                            r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                            double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                            double DueAmount = i.GrandTotal - i.TotalPaid;
                            r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                            r.SetParameterValue("DueAmountNumVal", DueAmount);
                            r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                            r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());
                            crystalReportViewer1.ReportSource = r;
                            crystalReportViewer1.Show();
                        }
                    }
                    else //A8
                    {
                        POSInvoiceA8 r = new POSInvoiceA8();
                        //PatientInvoice2Rpt r = new PatientInvoice2Rpt();
                        //r.Database.Tables[1].SetDataSource(i.InvoiceItems);
                        //r.Database.Tables[2].SetDataSource(i.InvoicePayments);
                        if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                        {
                            r.Database.Tables[3].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                        }

                        r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                        r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                        r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                        r.SetParameterValue("PractiseInvoiceNote", SharedVariables.AdminInvoiceSetting.InvoiceNote);

                        r.SetParameterValue("InvoiceId", i.InvoiceRefNo.ToString());
                        r.SetParameterValue("CreateDate", i.CreatedAt);
                        r.SetParameterValue("InvoiceNote", i.Note);
                        if (i.ObjPatient == null)
                        {
                            r.SetParameterValue("PatientName", "");
                            r.SetParameterValue("MrNo", "");
                            r.SetParameterValue("PatientPhone", "");
                            r.SetParameterValue("Gender", "");
                            r.SetParameterValue("DateOfBirth", "");
                            r.SetParameterValue("Age", "");
                        }
                        else
                        {
                            r.SetParameterValue("PatientName", i.ObjPatient.Name);
                            r.SetParameterValue("PatientPhone", i.ObjPatient.Phone);
                            r.SetParameterValue("Gender", i.ObjPatient.Gender);
                            r.SetParameterValue("DateOfBirth", i.ObjPatient.DateOfBirth == null ? "" : i.Patient.DateOfBirth.ToString());
                            r.SetParameterValue("Age", i.ObjPatient.AgeYears.ToString() + " Years");
                        }

                        //r.SetParameterValue("ShowPhone", SharedVariables.AdminInvoiceSetting.ShowPhone);
                        //r.SetParameterValue("ShowMr", SharedVariables.AdminInvoiceSetting.ShowMR);
                        //r.SetParameterValue("ShowGender", SharedVariables.AdminInvoiceSetting.ShowGender);
                        //r.SetParameterValue("ShowDOB", SharedVariables.AdminInvoiceSetting.ShowDOB);
                        //r.SetParameterValue("ShowAge", SharedVariables.AdminInvoiceSetting.ShowAge);

                        r.SetParameterValue("Description", Description);
                        r.SetParameterValue("PaymentMode", PaymentMode);

                        //r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                        //r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                        //r.SetParameterValue("FirstPayment", "Rs. " + Math.Round(i.FirstPayment).ToString() + "/-");
                        r.SetParameterValue("TotalPaid", "Rs. " + Math.Round(i.TotalPaid));
                        double AdvanceAmount = i.TotalPaid - i.GrandTotal;
                        double DueAmount = i.GrandTotal - i.TotalPaid;
                        r.SetParameterValue("AdvanceAmountNumVal", AdvanceAmount);
                        r.SetParameterValue("DueAmountNumVal", DueAmount);
                        r.SetParameterValue("AdvanceAmount", "Rs. " + Math.Round(AdvanceAmount).ToString());
                        r.SetParameterValue("DueAmount", "Rs. " + Math.Round(DueAmount).ToString());

                        //r.SetParameterValue("ShowPharmaName", SharedVariables.AdminInvoiceSetting.PrintPractiseName);
                        //r.SetParameterValue("ShowPharmaPhone", SharedVariables.AdminInvoiceSetting.ShowPharmaPhone);
                        //r.SetParameterValue("ShowPharmaAddress", SharedVariables.AdminInvoiceSetting.ShowPharmaAddress);

                        crystalReportViewer1.ReportSource = r;
                        crystalReportViewer1.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.InnerException.Message, "Error");
                }
                else
                {
                    SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
                }
            }
        }

        private string getPaymentModeString(int PaymentType)
        {
            if (PaymentType == 1) { return "Cash"; }
            else if (PaymentType == 2) { return "Cheque"; }
            else if (PaymentType == 3) { return "Debit/Credit Card"; }
            else if (PaymentType == 4) { return "Online Payment"; }
            else { return "Incorrect Payment Mode"; }
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}