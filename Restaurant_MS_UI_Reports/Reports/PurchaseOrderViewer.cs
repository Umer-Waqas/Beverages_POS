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
using GK.Shared.Repository;
using Restaurant_MS_Core.ViewModels;

namespace Pharmacy.UI.Reports
{
    public partial class PurchaseOrderViewer : Form
    {
        private UnitOfWork unitOfWork;
        long PurchaseOrderId = 0;
        
        public PurchaseOrderViewer()
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
        }
        public PurchaseOrderViewer(long PurchaseOrderId)
        {
            InitializeComponent();
            unitOfWork = new UnitOfWork();
            this.PurchaseOrderId = PurchaseOrderId;
        }
        private void PurchaseOrderViewer_Load(object sender, EventArgs e)
        {
            try
            {
                PurchaseOrderVM p = unitOfWork.PurchaseOrderRepository.GetPurchaseOrderById_print(this.PurchaseOrderId);
                List<PurchaseOrderItemVM> FinalList = new List<PurchaseOrderItemVM>();
                
                PurchaseOrderRpt r = new PurchaseOrderRpt();
                r.Database.Tables[0].SetDataSource(p.PurchaseOrderItems);
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    r.Database.Tables[1].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone); 
                r.SetParameterValue("PurchaseOrderNo", p.PurchaseOrderNo.ToString());
                r.SetParameterValue("Date", p.CreatedAt);
                r.SetParameterValue("Supplier", p.Supplier == null? "" : p.Supplier.Name);
                crystalReportViewer1.ReportSource = r;
                crystalReportViewer1.Show();
            }
            catch(Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
    }
}