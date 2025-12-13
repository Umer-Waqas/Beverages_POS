using GK.Shared.Core;
using Restaurant_MS_Core.ViewModels;
using GK.Shared.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pharmacy.UI.Reports.Pharmacy
{
    public partial class ItemsListViewer : Form
    {
        int Orientation = 0;
        DateTime DateFrom;
        DateTime DateTo;
        UnitOfWork unitOfWork;
        public ItemsListViewer()
        {
            InitializeComponent();
        }

        public ItemsListViewer(int Orientation, DateTime DateFrom, DateTime DateTo)
        {
            InitializeComponent();
            this.Orientation = Orientation;
            this.DateFrom = DateFrom;
            this.DateTo = DateTo;
        }
        private void DeadStocksViewer_Load(object sender, EventArgs e)
        {
            try
            {
                List<ItemNamesVM> Result;
                using (unitOfWork = new UnitOfWork())
                {
                    Result = unitOfWork.ItemRspository.GetActiveItemsNames();
                }
                //if (Orientation == 0)
                //{
                Reports.Pharmacy.ItemsListRpt r = new ItemsListRpt();
                if (SharedVariables.AdminPractiseSetting != null && !string.IsNullOrEmpty(SharedVariables.AdminPractiseSetting.LogoPath))
                {
                    r.Database.Tables[0].SetDataSource(SharedFunctions.GetImageTable(SharedVariables.AdminPractiseSetting.LogoPath));
                }
                else
                {
                    r.Database.Tables[0].SetDataSource(new DataTable());
                }
                r.Database.Tables[1].SetDataSource(Result);
                r.SetParameterValue("PharmacyName", SharedVariables.AdminPractiseSetting.Name);
                r.SetParameterValue("PharmacyAddress", SharedVariables.AdminPractiseSetting.Address);
                r.SetParameterValue("PharmacyPhone", SharedVariables.AdminPractiseSetting.Phone);
                r.SetParameterValue("DateFrom", this.DateFrom);
                r.SetParameterValue("DateTo", this.DateTo);
                r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);

                crystalReportViewer1.ReportSource = r;
                crystalReportViewer1.Show();
                //}
                //else
                //{
                //Reports.Pharmacy.ItemsListRpt r = new ItemsListRpt();
                //r.SetDataSource(Result);
                //r.SetParameterValue("PharmacyName", SharedVariables.PharmacyName);
                //r.SetParameterValue("PharmacyAddress", SharedVariables.PharmacyAddress);
                //r.SetParameterValue("PharmacyPhone", SharedVariables.PharmacyPhone);
                //r.SetParameterValue("DateFrom", this.DateFrom);
                //r.SetParameterValue("DateTo", this.DateTo);
                //r.SetParameterValue("UserName", SharedVariables.LoggedInUser.UserName);
            }
            catch (Exception ex)
            {
                SharedFunctions.ShowErrorMessage(SharedVariables.GeneralErrMsg, ex.Message, "Error");
            }
        }
    }
}
