using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Infrastructure
{
    public static class InfraUtilityFunctions
    {
        public static DateTime GetDateAccordingToDayCloseSetting()
        {
            DateTime returDate = DateTime.Now;
            if (SharedVariables.AdminPharmacySetting != null)
            {
                if (SharedVariables.AdminPharmacySetting.EnableDayClose)
                {
                    if (!SharedVariables.AdminPharmacySetting.UseDafaultStoreClosingSetting)
                        if (DateTime.Now.TimeOfDay >= new TimeSpan(00, 00, 00) && DateTime.Now.TimeOfDay <= SharedVariables.AdminPharmacySetting.DayCloseTime.Value.TimeOfDay && DateTime.Now.TimeOfDay < SharedVariables.AdminPharmacySetting.DayOpenTime.Value.TimeOfDay)
                        {
                            returDate = DateTime.Now.AddDays(-1);
                        }
                }
            }
            return returDate;
        }
    }
}
