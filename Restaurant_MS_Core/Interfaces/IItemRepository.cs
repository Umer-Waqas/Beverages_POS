using Restaurant_MS_Core.Entities;
using Restaurant_MS_Core.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_MS_Core.Interfaces
{
    public interface IItemRepository
    {
        IEnumerable<Item> GetItemsBySupplierID(long SupplierID);
        //List<ItemsVM> GetItemsWithStockData(int StartRecord, int NexRequired);
        //List<ItemsVM> GetItemsWithStockDataByStockDate(DateTime StockDate, int OffSet, int NextFetch);
        //List<ItemsVM> GetItemsWithStockDataByItemNameFilter(string FilterString, int OffSet, int NextFetch);
        //List<Item_SuppliersVM> GetSuppliersByItemIds(List<ItemsVM> itemsList);
    }
}