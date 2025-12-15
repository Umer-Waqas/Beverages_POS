using GK.Shared.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_MS_UI.Menu.Suppliers
{
    public class UISharedFunctions
    {
        public static bool CheckDayClosed(Form RefernceForm)
        {
            bool status = false;
            if (SharedVariables.AdminPharmacySetting.EnableDayClose)
            {
                return false;
            }
            if (!SharedVariables.IsPreviousDayClosed)
            {
                MessageBox.Show("Previous day is not closed, you are not allowed to perform any action. please contact your system Administrator/Manager to close previous day before inserting any new transactions. Press Ok to close the application", "Previous Day not Closed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                status = true;
            }
            if (SharedVariables.IsStoreClosed)
            {
                MessageBox.Show("Store has been closed, you are not allowed to perform any action. please contact your system administrator. Press Ok to close the application", "Day Cloed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                status = true;
            }
            if (status)
            {
                //try
                //{
                //    RefernceForm.Close();
                //}
                //catch (Exception ex)
                //{
                //}
            }
            return status;
        }

        public static bool CheckDayClosed()
        {
            bool status = false;
            if (!SharedVariables.AdminPharmacySetting.EnableDayClose)
            {
                return false;
            }
            if (!SharedVariables.IsPreviousDayClosed)
            {
                MessageBox.Show("Previous day is not closed, you are not allowed to perform any action. please contact your system Administrator/Manager to close previous day before inserting any new transactions. Press Ok to close the application", "Previous Day not Closed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                status = true;
            }
            if (SharedVariables.IsStoreClosed)
            {
                MessageBox.Show("Store has been closed, you are not allowed to perform any action. please contact your system administrator. Press Ok to close the application", "Day Cloed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                status = true;
            }
            return status;
        }
    }
}
