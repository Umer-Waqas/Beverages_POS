

using GK.Shared.Repository;

namespace Restaurant_MS_Infrastructure.Repository
{
    public class StoreClosingRepository : Repository<StoreClosing>
    {
        AppDbContext cxt;
        DateTime ActionTime;
        public StoreClosingRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public StoreClosing GetLastStoreClosing()
        {
            return cxt.StoreClosings.OrderByDescending(c => c.StoreClosingId).FirstOrDefault();
        }

        public DateTime? LastClosingDate()
        {
            return cxt.StoreClosings.Where(sc => sc.IsActive).Select(sc => sc.ClosingDate).FirstOrDefault();
        }

        public bool IsStoreClosed()
        {
            if (!SharedVariables.AdminPharmacySetting.EnableDayClose)
            {
                return false;
            }

            try
            {
                var closing = cxt.StoreClosings.OrderByDescending(c => c.StoreClosingId).FirstOrDefault(c => c.IsActive);
                if (closing == null) return false;
                if (closing.ClosingDate.HasValue)
                {
                    SharedVariables.IsStoreClosed = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public StoreOpenRespVM OpenStore()
        {
            if (!SharedVariables.AdminPharmacySetting.EnableDayClose)
            {
                return new StoreOpenRespVM { OpenSuccess = true, IsPreviousDayClosed = true, IsOpenTimeReached = true };
            }
            UnitOfWork unitofWork = new UnitOfWork();
            StoreOpenRespVM resp = new StoreOpenRespVM();
            resp.OpenSuccess = false;
            resp.IsPreviousDayClosed = true;
            resp.IsOpenTimeReached = false;
            try
            {
                ActionTime = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
                StoreClosing obj = new StoreClosing();
                var lastStoreClosing = cxt.StoreClosings.Where(c => c.IsActive).OrderByDescending(c => c.StoreClosingId).FirstOrDefault();
                if (lastStoreClosing != null)
                {
                    if (!lastStoreClosing.ClosingDate.HasValue)
                    {
                        if (lastStoreClosing.OpenDate.Value.Date < ActionTime.Date)
                        {
                            resp.OpenSuccess = false;
                            resp.IsPreviousDayClosed = false;
                            return resp;
                        }
                        if (lastStoreClosing.OpenDate.Value.Date == ActionTime.Date)
                        {
                            resp.OpenSuccess = true;
                            resp.IsOpenTimeReached = true;
                            resp.IsPreviousDayClosed = true;
                            return resp;
                        }
                    }
                    else
                    {
                        resp.IsPreviousDayClosed = true;
                        if (lastStoreClosing.OpenDate.Value.Date == ActionTime.Date)
                        {
                            resp.OpenSuccess = false;
                            return resp;
                        }
                    }
                }

                if (!(DateTime.Now.TimeOfDay >= SharedVariables.AdminPharmacySetting.DayOpenTime.Value.TimeOfDay) || DateTime.Now.TimeOfDay > SharedVariables.AdminPharmacySetting.DayCloseTime.Value.TimeOfDay && DateTime.Now.TimeOfDay < SharedVariables.AdminPharmacySetting.DayOpenTime.Value.TimeOfDay)
                {
                    resp.OpenSuccess = false;
                    resp.IsOpenTimeReached = false;
                    return resp;
                }
                obj.OpeningCash = lastStoreClosing == null ? 0 : lastStoreClosing.ClosingCash;
                obj.TotalInflow = 0;
                obj.TotalOutFlow = 0;
                obj.SystemCash = 0;
                obj.OpeningBalance = lastStoreClosing == null ? (double)unitofWork.InvoiceRepository.GetRetailValueOfAvailableStock().RetailValueOfAvailableStock : lastStoreClosing.ClosingBalance;
                obj.ClosingBalance = 0;
                obj.PhysicalCash = 0;
                obj.CashDiff = 0;
                obj.OpenDate = ActionTime;
                obj.ClosingDate = null;
                obj.StoreClosedBy = null;
                obj.StoreOpenedBy = SharedVariables.LoggedInUser.UserId;
                obj.CreatedAt = ActionTime;
                obj.UpdatedAt = ActionTime;
                obj.CreatedBy = SharedVariables.LoggedInUser.UserId;
                obj.IsActive = true;
                obj.UserId = SharedVariables.LoggedInUser.UserId;

                // cash submission attrs
                obj.CashSubmittedToBank = 0;
                obj.CashSubmittedToHO = 0;
                obj.TotalCashSubmitted = 0;
                obj.ClosingCash = lastStoreClosing == null ? 0 : lastStoreClosing.OpeningCash;

                cxt.StoreClosings.Add(obj);
                cxt.SaveChanges();
                resp.OpenSuccess = true;
                resp.IsOpenTimeReached = true;
                resp.IsPreviousDayClosed = true;
            }
            catch (Exception ex)
            {
                resp.OpenSuccess = false;
                resp.IsPreviousDayClosed = true;
            }
            return resp;
        }


        public CashSummaryVM getLastCashSummary()
        {
            StoreClosing lastClosing = cxt.StoreClosings.Where(sc => sc.IsActive).OrderByDescending(sc => sc.StoreClosingId).FirstOrDefault();
            if (lastClosing != null)
            {
                if (lastClosing.ClosingDate == null)
                {
                    ActionTime = lastClosing.OpenDate.Value;
                }
                else
                {
                    ActionTime = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
                }
            }
            else
            {
                ActionTime = InfraUtilityFunctions.GetDateAccordingToDayCloseSetting();
            }
            var vm = new CashSummaryVM
            {
                IsFirstClosing = false,
                LastClosingDate = ActionTime,
                OpeningCash = (double?)(from x in cxt.StoreClosings where x.IsActive && (x.OpenDate.Value.Date) == (ActionTime.Date) orderby x.StoreClosingId descending select x.OpeningCash).FirstOrDefault() ?? 0,
                //SkimmedCash = (double?)(from sk in cxt.POSSkimmedCash where sk.IsActive && sk.CreatedAt > lastClosing.ClosingDate select sk.Cash).Sum() ?? 0,
                //SubmittedCash = (double?)(from pc in cxt.POSClosings where pc.IsActive && pc.CreatedAt > lastClosing.ClosingDate select pc.CashSubmitted).Sum() ?? 0,

                //ReceivedCash = (double?)(from rc in cxt.POSReceivedCash where rc.IsActive && rc.CreatedAt > lastClosing.ClosingDate select rc.Cash).Sum() ?? 0,
                //SupplierPayments = (double?)(from ex in cxt.Expenses where ex.IsActive && ex.PaymentMode == 1 && ex.CreatedAt > lastClosing.ClosingDate select ex.Amount).Sum() ?? 0,

                TotalInflow = cxt.InvoicePayments.Where(p => p.IsActive
                    && p.Payment > 0
                    && (p.PaymentDate.Date) == (ActionTime.Date))
                          .Sum(p => (double?)p.Payment) ?? 0,
                TotalOutFlow = cxt.Expenses.Where(e => e.IsActive
                && e.Amount > 0
                && (e.CreatedAt.Date) == (ActionTime.Date))
                      .Sum(e => (double?)e.Amount) ?? 0,
            };
            return vm;
        }
    }
}