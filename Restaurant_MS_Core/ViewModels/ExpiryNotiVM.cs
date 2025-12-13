using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.ViewModels
{
    public class ExpiryNotiVM
    {
        public long ItemId { get; set; }
        public string ItemName { get; set; }
        public string BatchNo { get; set; }
        public string LastTrDate { get; set; }
        public DateTime BatchExp { get; set; }
        public int ExpiredStock { get; set; }
        public int ExpiredConsumedStock { get; set; }
        public int ActualExpired { get; set; }
    }
}