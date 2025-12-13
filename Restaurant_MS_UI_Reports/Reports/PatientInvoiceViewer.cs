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
using GK.Shared.Repository;
using Restaurant_MS_Core.ViewModels;
using GK.Shared.Repository;

namespace Pharmacy.UI.Reports
{
    public partial class PatientInvoiceViewer : Form
    {
        private long InvoiceId = 0;
        UnitOfWork unitOfWork;
        public PatientInvoiceViewer()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }

        public PatientInvoiceViewer(long InvoiceId)
        {
            InitializeComponent();
            this.InvoiceId = InvoiceId;
            unitOfWork = new UnitOfWork();
        }

        private void PatientInvoice_Load(object sender, EventArgs e)
        {
            //InvoiceVM objInvoice = unitOfWork.InvoiceRepository.GetInvoiceByIdDetails_IncPatient(this.InvoiceId);
            try
            {
                InvoiceVM i = unitOfWork.InvoiceRepository.GetInvoiceByIdDetails_IncPatient(this.InvoiceId);
                if (string.IsNullOrEmpty(i.Patient.Name))
                {
                    PatientInvoiceRpt r = new PatientInvoiceRpt();
                    r.SetDataSource(i.InvoiceItems);
                    r.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    r.SetParameterValue("InvoiceId", i.InvoiceId);
                    r.SetParameterValue("CreatedAt", i.CreatedAt);
                    r.SetParameterValue("PatientName", string.IsNullOrEmpty(i.Patient.Name) ? "" : i.Patient.Name);
                    r.SetParameterValue("MrNo", i.Patient.MRNo == 0 ? "" : i.Patient.MRNo.ToString());
                    r.SetParameterValue("PatientPhone", string.IsNullOrEmpty(i.Patient.Phone) ? "" : i.Patient.Phone);

                    r.SetParameterValue("Gender", i.Patient.Gender);
                    if (i.Patient.DateOfBirth == null)
                    {
                        r.SetParameterValue("DateOfBirth", "");
                    }
                    else
                    {
                        r.SetParameterValue("DateOfBirth", i.Patient.DateOfBirth);
                    }
                    if (i.Patient.AgeYears == 0)
                    {
                        r.SetParameterValue("Age", "");
                    }
                    else
                    {
                        r.SetParameterValue("Age", i.Patient.AgeYears.ToString() + " Years");
                    }


                    r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                    r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                    r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(i.TotalPaid).ToString() + "/-");
                    r.SetParameterValue("RefundDate", i.RefundDate);
                    r.SetParameterValue("TotalRefund", "Rs. " + Math.Round(i.TotalRefund));
                    r.SetParameterValue("RemainingPaid", "Rs. " + Math.Round(i.TotalPaid - i.TotalRefund) + "/-");
                    crystalReportViewer1.ReportSource = r;
                    crystalReportViewer1.Show();
                }
                else
                {
                    PatientInvoiceRpt r = new PatientInvoiceRpt();
                    r.SetDataSource(i.InvoiceItems);
                    r.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                    r.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                    r.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                    r.SetParameterValue("InvoiceId", i.InvoiceId);
                    r.SetParameterValue("CreatedAt", i.CreatedAt);
                    r.SetParameterValue("PatientName", string.IsNullOrEmpty(i.Patient.Name) ? "" : i.Patient.Name);
                    r.SetParameterValue("MrNo", i.Patient.MRNo == 0 ? "" : i.Patient.MRNo.ToString());
                    r.SetParameterValue("PatientPhone", string.IsNullOrEmpty(i.Patient.Phone) ? "" : i.Patient.Phone);

                    r.SetParameterValue("Gender", i.Patient.Gender);
                    if (i.Patient.DateOfBirth == null)
                    {
                        r.SetParameterValue("DateOfBirth", "");
                    }
                    else
                    {
                        r.SetParameterValue("DateOfBirth", i.Patient.DateOfBirth);
                    }
                    if (i.Patient.AgeYears == 0)
                    {
                        r.SetParameterValue("Age", "");
                    }
                    else
                    {
                        r.SetParameterValue("Age", i.Patient.AgeYears.ToString() + " Years");
                    }


                    r.SetParameterValue("SubTotal", "Rs. " + Math.Round(i.SubTotal).ToString());
                    r.SetParameterValue("GrandTotal", "Rs. " + Math.Round(i.GrandTotal).ToString() + "/-");
                    r.SetParameterValue("PaidAmount", "Rs. " + Math.Round(i.TotalPaid).ToString() + "/-");
                    r.SetParameterValue("RefundDate", i.RefundDate);
                    r.SetParameterValue("TotalRefund", "Rs. " + Math.Round(i.TotalRefund));
                    r.SetParameterValue("RemainingPaid", "Rs. " + Math.Round(i.TotalPaid - i.TotalRefund) + "/-");
                    crystalReportViewer1.ReportSource = r;
                    crystalReportViewer1.Show();
                }                              
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
    }
}