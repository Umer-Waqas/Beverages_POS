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
    public partial class StockStatisticsUI : Form
    {
        UnitOfWork unitOfWork;
        public StockStatisticsUI()
        {
            InitializeComponent();
        }

        private void StockStatisticsUI_Load(object sender, EventArgs e)
        {
            LoadStockStatistics();
            //this.RetailValueChart.Series[0].Points.AddXY("m1", 100);
            //this.RetailValueChart.Series[0].Points.AddXY("m2", 200);
            //this.RetailValueChart.Series[0].Points.AddXY("m3", 300);
            //this.RetailValueChart.Series[0].Points.AddXY("m4", -100);
            //this.RetailValueChart.Series[0].Points.AddXY("m5", -200);
            //this.RetailValueChart.Series[0].Points.AddXY("m5", -300);
        }

        private void LoadStockStatistics()
        {
            using (unitOfWork = new UnitOfWork())
            {
                //var Result = 

                StockStatisticsParentVM Result = unitOfWork.StockRepository.GetStockStatistics(dtpFrom.Value, dtpTo.Value);
                //unitOfWork.StockRepository.TestMethod(dtpFrom.Value, dtpTo.Value);

                //"Retail Value"
                //"Retail Value"
                //"Retail Value"

                this.RetailValueChart.DataSource = Result.StockStatisticsList;
                this.RetailValueChart.Series["Retail Value"].XValueMember = "MonthDay";
                this.RetailValueChart.Series["Retail Value"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
                this.RetailValueChart.Series["Retail Value"].YValueMembers = "TotalRetailValue";
                this.CostValueChart.Series[0].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;


                //this.RetailValueChart.Series[0].Points.AddXY("m1", 100);
                //this.RetailValueChart.Series[0].Points.AddXY("m2", 200);
                //this.RetailValueChart.Series[0].Points.AddXY("m3", 300);
                //this.RetailValueChart.Series[0].Points.AddXY("m4", -100);
                //this.RetailValueChart.Series[0].Points.AddXY("m5", -200);
                //this.RetailValueChart.Series[0].Points.AddXY("m5", -300);
                //this.RetailValueChart.Update();


                this.CostValueChart.DataSource = Result.StockStatisticsList;
                this.CostValueChart.Series["Cost Value"].XValueMember = "MonthDay";
                this.CostValueChart.Series["Cost Value"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
                this.CostValueChart.Series["Cost Value"].YValueMembers = "TotalCostValue";
                this.CostValueChart.Series["Cost Value"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;


                /////////////////////
                //retailChart2.DataSource = Result.StockStatisticsList;
                //retailChart2.Series["Retail Value"].XValueMember = "MonthDay";
                //retailChart2.Series["Retail Value"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
                //retailChart2.Series["Retail Value"].YValueMembers = "TotalRetailValue";

                //costChart2.DataSource = Result.StockStatisticsList;
                //costChart2.Series["Cost Value"].XValueMember = "MonthDay";
                //costChart2.Series["Cost Value"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
                //costChart2.Series["Cost Value"].YValueMembers = "TotalCostValue";

                lblRetailValue.Text = Math.Round(Result.TotalRetValOfAvailableStock, 2).ToString();
                lblCostValue.Text = Math.Round(Result.TotalCostValOfAvailableStock, 2).ToString();

            }
        }

        private void dtpFrom_ValueChanged(object sender, EventArgs e)
        {
            LoadStockStatistics();
        }

        private void dtpTo_ValueChanged(object sender, EventArgs e)
        {
            LoadStockStatistics();
        }

    }
}