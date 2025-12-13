
using Restaurant_MS_Core.Entities;
using Restaurant_MS_Core.ViewModels;
using System.Data;
using System.Drawing;

namespace Restaurant_MS_Core
{
    public static class SharedVariables
    {
        //public static int Menu_MinWidth { get; set; }
        //public static int Menu_MaxWidth { get; set; }
        public static bool IsTesting = false;
        public static bool ShowActualExceptionMessages = true;
        public static string GeneralSuccMsg = "Record Saved Successfully";
        public static string GeneralInsertMsg = "Record Inserted Successfully";
        public static string GeneralUpdateMsg = "Record Updated Successfully";
        public static string GeneralErrMsg = "Faild to Perform Required Action, Please Try Again";


        // this variable it being used in dbconetxt and main form to indicate if app started.
        public static bool IsApplicationStarted = false;
        public static string SqlConnectionString { get; set; }

        public static User LoggedInUser;

        public static bool IsClientPC = true;
        //API variables
        public static int API_PerBatchCount = 5;
        //public static string Salt = "@#$sjaFDs?!djsRT";
        public static string API_serverPath = "";
        public static string API_Email = "";
        public static string API_Password = "";
        public static string API_Token = "";


        //public static int DeadStockDaysLimit = 30;
        public enum ConsumptionType { Services = 1, Sales = 2, Damages = 3, Returned = 4, Expired = 5, Exceed = 6 };
        public enum PatientStatus { Active = 1, Inactive = 2, Inactive_Deceased = 3 }
        public enum LoadStockAction { AllItemsBySupplier = 1, LowStockItemsBySupplier = 2, PrevSoldItemsBySupplier = 3, MissedSaleItemsBySupplier = 4, AllLowStockItems = 5, AllPreviouslySoldItems = 6, AllMissedSaleItems = 7 };

        public static int PageSize = 15; //13
        public static string PharmacyName { get; set; }
        public static string PharmacyAddress { get; set; }
        public static string PharmacyPhone { get; set; }

        public static Color DisabledColor { get { return Color.LightGray; } }
        public static Color EditingColor { get { return Color.Coral; } }
        public static AdminInvoiceSetting AdminInvoiceSetting { get; set; }
        public static AdminProcedureInvoiceSetting AdminProcedureInvoiceSetting { get; set; }
        public static AdminPractiseSetting AdminPractiseSetting { get; set; }
        public static AdminPrintFormatSetting AdminPrintFormatSetting { get; set; }
        public static AdminPharmacySetting AdminPharmacySetting { get; set; }
        public static AdminShiftMasterSetting AdminShiftMasterSetting { get; set; }
        public static List<AdminShiftSetting> AdminShiftSettings { get; set; }
        public enum PrintPageSize { A4 = 1, A8 = 2, A5 = 3 };
        public enum PageType { PlainPaper = 1, LetterHead = 2 };
        public enum PageOrientation { Portrait = 1, LandScape = 2 };
        public enum ExpiryPeriodUnit { Days = 0, Months = 2 };
        public enum ItemType { SaleItem = 1, DealItem = 2, BakedItem = 3, RawItem = 4 }
        //public static Color RowSelectionColor = Color.FromArgb(247, 247, 247);
        public static Color RowSelectionBackColor = Color.FromArgb(225, 248, 255);
        public static Color MenuSelectionBackColor = Color.FromArgb(42, 196, 244);
        public static Color MenuSelectionForeColor = Color.White;
        //public static List<Item> POSItems = null;
        public static int LowStockMailDaysInterval = 15; // days
        public static long LowStockMailRetryInterval = 1; // minute
        public static bool UseLocalDb = false;
        public static int PracticeId { get; set; }
        //public static int InitialItemsLoadSize = 50;
        public static int AsyncDataLoadDelay = 500; //milliseconds

        public static DataView BulkLoadedItemsDataView;
        public static List<BulkItemsVM> BulkLoadedItemsList;
        public static bool ReloadItemsRequired = false;

        public static int SideBarWidth { get; set; }
        public static int TopBarHeight { get; set; }
        public static int TopMenueBarHeight { get; set; }
        public static List<FlatDiscount> TodayDiscounts { get; set; }



        //variables being used to set and read from diference forms
        public static double TotalNotesAmount { get; set; }
        public static bool IsStoreClosed { get; set; }
        public static bool IsPreviousDayClosed { get; set; }
    }
}