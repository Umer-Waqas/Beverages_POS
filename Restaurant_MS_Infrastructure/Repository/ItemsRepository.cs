

using Restaurant_MS_Core.Entities;

namespace Restaurant_MS_Infrastructure.Repository
{
    public class ItemsRepository : Repository<Item>, IItemRepository
    {
        AppDbContext cxt = null;
        public ItemsRepository(AppDbContext cxt)
            : base(cxt)
        {
            this.cxt = cxt;
        }

        public double GetUnitCostByItemId(int ItemId)
        {
            return cxt.Items.Where(i => i.ItemId == ItemId).Select(i => i.UnitCostPrice).FirstOrDefault();
        }
        public string GetUnitByItemId(long ItemId)
        {
            return cxt.Items.Where(i => i.ItemId == ItemId).Select(i => i.Unit).FirstOrDefault();
        }
        public Item GetItemByMappedProductId(long ItemId)
        {
            return cxt.Items.FirstOrDefault(i => i.MappedProductId == ItemId);
        }
        public Item MyProperty { get; set; }
        public List<Item> GetActiveItems(bool loadRawItems = false)
        {
            return cxt.Items.Where(i => i.IsActive && !i.IsDefault && i.IsRawItem == loadRawItems).ToList();
        }
        public List<ItemNamesVM> GetActiveItemsNames()
        {
            return cxt.Items.Where(i => i.IsActive && !i.IsDefault)
                .Select(i => new ItemNamesVM
                {
                    ItemName = i.ItemName,
                    ManufacturerId = i.ManufacturerId.HasValue ? i.ManufacturerId.Value : 0,
                    ManufacturerName = i.Manufacturer != null ? i.Manufacturer.Name : ""
                }).ToList();
        }
        public List<long> GetItemsHavingNoBarcodes()
        {
            return cxt.Items.Where(i => i.IsActive && !i.IsDefault && (i.Barcode == null || i.Barcode == "")).Select(i => i.ItemId).ToList();
        }

        public List<BulkItemsVM> GetBulkAllActiveItems()
        {
            //if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
            //{
            return (from i in cxt.Items
                    where i.IsActive && !i.IsDefault
                    select new BulkItemsVM
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        Barcode = i.Barcode,
                        Unit = i.Unit,
                        Manufacturer = i.Manufacturer.Name,
                        //RetailPrice = (double?)(from si in cxt.StockItems where si.Item.ItemId == i.ItemId && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault()
                        //            == 0 ? i.RetailPrice
                        //            : (double?)(from si in cxt.StockItems where si.Item.ItemId == i.ItemId && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() ?? 0,
                        RetailPrice = i.RetailPrice,
                        CostPrice = i.UnitCostPrice,
                        //CostPrice = (double?)(from si in cxt.StockItems where si.Item.ItemId == i.ItemId && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault()
                        //            == 0 ? i.RetailPrice
                        //            : (double?)(from si in cxt.StockItems where si.Item.ItemId == i.ItemId && si.IsActive orderby si.StockItemId descending select si.UnitCost).FirstOrDefault() ?? 0
                    })
                    .OrderByDescending(r => r.ItemId)
                    .ToList();
            //}
            //else
            //{
            //    return (from i in cxt.Items
            //            where i.IsActive && !i.IsDefault
            //            select new BulkItemsVM
            //            {
            //                ItemId = i.ItemId,
            //                ItemName = i.ItemName,
            //                GenericName = i.ChecmicalName,
            //                Manufacturer = i.Manufacturer.Name,
            //                RetailPrice = (double?)(from si in cxt.StockItems where si.Item.ItemId == i.ItemId && si.IsActive orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault() ?? 0
            //            })
            //    .ToList();
            //}
        }
        public List<Item> GetActiveItems(int GetCount)
        {
            return cxt.Items.Where(i => i.IsActive && !i.IsDefault).Take(GetCount).ToList();
        }

        public IPagedList<Item> GetMatchingItems(string SearchString, int PageNo, int PageSize)
        {
            return cxt.Items
                .Where(i => i.IsActive && !i.IsDefault && (i.ItemName.ToLower().Contains(SearchString) || i.Barcode.ToLower().Equals(SearchString)))
                .OrderBy(i => i.ItemId)
                .ToPagedList(PageNo, PageSize);
        }

        public IEnumerable<Item> GetItemsBySupplierID(long SupplierID)
        {
            return (from i in cxt.Items
                    where i.Suppliers.Any(s => s.SupplierID == SupplierID) && i.IsActive
                    select i).ToList();
        }

        public Item GetDefaultItem()
        {
            return cxt.Items
                .Where(i => i.IsDefault).FirstOrDefault();
        }

        public int GetStockItemsCount()
        {
            var count = (from i in cxt.Items
                         join si in cxt.StockItems on i.ItemId equals si.Item.ItemId into stockItems
                         from si in stockItems.DefaultIfEmpty()
                         join s in cxt.Stocks on si.Stock.StockId equals s.StockId into stocks
                         from s in stocks.DefaultIfEmpty()
                         where i.IsActive
                         group i by new { i.ItemId, i.ItemName } into g
                         select g.Key).Count();

            return count;
        }

        public int GetStockItemsCount(string filterString)
        {
            if (string.IsNullOrWhiteSpace(filterString))
            {
                return cxt.Items
                    .Where(i => i.IsActive)
                    .GroupBy(i => i.ItemId)
                    .Count();
            }

            return cxt.Items
                .Where(i => i.IsActive && i.ItemName.Contains(filterString))
                .GroupBy(i => i.ItemId)
                .Count();
        }

        public int GetStockItemsCount(DateTime stockDate)
        {
            var dateOnly = stockDate.Date; // Use Date property to get date part only

            var count = (from i in cxt.Items
                         join si in cxt.StockItems on i.ItemId equals si.Item.ItemId
                         join s in cxt.Stocks on si.Stock.StockId equals s.StockId
                         where i.IsActive &&
                               s.CreatedAt.Date == dateOnly // Compare dates only (ignoring time)
                         group i by new { i.ItemId, i.ItemName } into g
                         select g.Key).Count();

            return count;
        }

        public List<BatchStockVM> GetItemsWithStockDataByItemId(int ItemId)
        {
            //            string qry = @"select x.batchId, x.BatchName, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, b.Expiry, b.CreatedAt  from(
            //                            select b.BatchId, b.BatchName, 
            //                            (select isnull(sum(si.Quantity), 0) from stockitems si where si.Item_ItemID = @ItemId and si.Batch_BatchId = b.BatchId and si.IsActive = 1) as TotalStock,
            //                            (
            //                            (select ISNULL(sum(Quantity),0) from stockitems si where si.Item_ItemID = @ItemId and si.Batch_BatchId = b.BatchId and si.IsActive = 1)
            //                            -
            //                            (select ISNULL(sum(Quantity),0) from StockConsumptions sc where sc.Item_ItemID = @ItemId and sc.Batch_BatchId = b.BatchId and sc.IsActive = 1)
            //                            -
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = @ItemId and si.IsActive = 1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            )as availableStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.AdjustmentId where ai.Item_ItemID = @ItemId and ai.IsActive =1) as AdjustedStock
            //                            from StockItems si
            //                            inner join Batches b on si.Batch_BatchId = b.BatchId
            //                            where si.Item_ItemID = @ItemId AND si.IsActive = 1
            //                            group by b.BatchId, b.BatchName
            //                            )x
            //                            inner join Batches b on x.BatchId = b.BatchId Order by b.BatchId Desc";
            //List<BatchStockVM> Result = cxt.Database.SqlQuery<BatchStockVM>(qry, new SqlParameter("@ItemId", ItemId)).ToList();

            List<BatchStockVM> Result = (from si in cxt.StockItems
                                         join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                                         where si.Item.ItemId == ItemId
                                         group si by si.Batch.BatchId into BatchGroup
                                         join b_ in cxt.Batches on BatchGroup.Key equals b_.BatchId
                                         select new BatchStockVM()
                                         {
                                             BatchId = b_.BatchId,
                                             BatchName = b_.BatchName,
                                             TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == ItemId && ts.Batch.BatchId == b_.BatchId select ts).Sum(ts => (double?)ts.Quantity) ?? 0,
                                             ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                                             ExpiredStock = (from es in cxt.StockItems join b_in in cxt.Batches on es.Batch.BatchId equals b_in.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == b_.BatchId && (b_in.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0,
                                             AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_.BatchId select adj_item).Sum(a => (double?)a.Quantity) ?? 0
                                         }).ToList();

            return Result;
        }
        //here
        public IPagedList<ItemsVM> GetItemsWithStockData(int PageNo, int PageSize)
        {
            //            string qry = @"select x.ItemId, x.ItemName, i.IsNarcotic,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemId, i.ItemName,
            //                            (select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.ItemId and StockItems.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.ItemId and StockItems.IsActive = 1)
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from StockConsumptions si where si.Item_ItemID = i.ItemId and si.IsActive = 1 )
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive =1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) < CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.AdjustmentId where ai.Item_ItemID = i.ItemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.ItemId = si.Item_ItemID
            //                            WHERE i.IsActive = 1
            //							group by ItemId, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemId = i.ItemId 
            //                            WHERE i.IsActive = 1
            //                            order by x.ItemId Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", OffSet), new SqlParameter("@NextFetch", NextFetch)).ToList();
            IPagedList<ItemsVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Select(vm => new ItemsVM
               {
                   ItemId = vm.ItemId,
                   ItemName = vm.ItemName,
                   RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                   Manufacturer = vm.Manufacturer,
                   Barcode = vm.Barcode,
                   Unit = vm.Unit,
                   ConversionUnit = vm.ConversionUnit,
                   ReorderingLevel = vm.ReOrderingLevel,
                   SuppliersList = vm.Suppliers.ToList(),
                   TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == vm.ItemId).Sum(si => (double?)si.Quantity + si.BonusQuantity) ?? 0,
                   ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == vm.ItemId select s).Sum(s => (double?)s.Quantity) ?? 0,
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == vm.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0,
                   AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == vm.ItemId select ai).Sum(ai => (double?)ai.Quantity) ?? 0,
                   ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == vm.ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                   HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == vm.ItemId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
               }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);
            return r;
        }


        public ItemsVM getStockByItemId(int ItemId)
        {
            ItemsVM r = cxt.Items
             .Where(i => i.ItemId == ItemId)
             .Where(i => i.IsActive)
             .Where(i => !i.IsDefault)
             .Select(vm => new ItemsVM
             {
                 ItemId = vm.ItemId,
                 ItemName = vm.ItemName,
                 //Manufacturer = vm.Manufacturer,
                 //Barcode = vm.Barcode,
                 Unit = vm.Unit,
                 ConversionUnit = vm.ConversionUnit,
                 //ReorderingLevel = vm.ReOrderingLevel,
                 //IsNarcotic = vm.IsNarcotic,
                 //SuppliersList = vm.Suppliers.ToList(),
                 RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == vm.ItemId orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault(),
                 TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == vm.ItemId).Sum(si => (double?)si.Quantity + si.BonusQuantity) ?? 0,
                 ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == vm.ItemId select s).Sum(s => (double?)s.Quantity) ?? 0,
                 ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == vm.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0
                                -
                                (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == vm.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0,
                 AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == vm.ItemId select ai).Sum(ai => (double?)ai.Quantity) ?? 0,
                 HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == vm.ItemId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
             }).FirstOrDefault();
            return r;
        }
        public List<ItemsVM> GetItemsWithStockData()
        {
            //            string qry = @"select x.ItemId, x.ItemName, i.IsNarcotic,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemId, i.ItemName,
            //                            (select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.ItemId and StockItems.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.ItemId and StockItems.IsActive = 1)
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from StockConsumptions si where si.Item_ItemID = i.ItemId and si.IsActive = 1 )
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive =1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) < CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.AdjustmentId where ai.Item_ItemID = i.ItemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.ItemId = si.Item_ItemID
            //                            WHERE i.IsActive = 1
            //							group by ItemId, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemId = i.ItemId 
            //                            WHERE i.IsActive = 1
            //                            order by x.ItemId Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", OffSet), new SqlParameter("@NextFetch", NextFetch)).ToList();
            List<ItemsVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Select(vm => new ItemsVM
               {
                   ItemId = vm.ItemId,
                   ItemName = vm.ItemName,
                   Barcode = vm.Barcode,
                   Unit = vm.Unit,
                   ReorderingLevel = vm.ReOrderingLevel,
                   SuppliersList = vm.Suppliers.ToList(),
                   TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == vm.ItemId).Sum(si => (double?)si.Quantity) ?? 0,
                   ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == vm.ItemId select s).Sum(s => (double?)s.Quantity) ?? 0,
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == vm.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0,
                   AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == vm.ItemId select ai).Sum(ai => (double?)ai.Quantity) ?? 0,
               }).OrderByDescending(i => i.ItemId).ToList();
            return r;
        }
        //report data
        public List<ItemsVM> GetItemsWithStockData_Print()
        {
            //            string qry = @"select x.ItemId, x.ItemName, i.IsNarcotic,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemId, i.ItemName,
            //                            (select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.ItemId and StockItems.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.ItemId and StockItems.IsActive = 1)
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from StockConsumptions si where si.Item_ItemID = i.ItemId and si.IsActive = 1 )
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive =1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) < CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.AdjustmentId where ai.Item_ItemID = i.ItemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.ItemId = si.Item_ItemID
            //                            WHERE i.IsActive = 1
            //							group by ItemId, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemId = i.ItemId 
            //                            WHERE i.IsActive = 1
            //                            order by x.ItemId Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", OffSet), new SqlParameter("@NextFetch", NextFetch)).ToList();
            //return cxt.Items
            //   .Where(i => i.IsActive)
            //   .Select(vm => new ItemsVM
            //   {
            //       ItemId = vm.ItemId,
            //       ItemName = vm.ItemName,
            //       Barcode = vm.Barcode,
            //       Unit = vm.Unit,
            //       ReorderingLevel = vm.ReOrderingLevel,
            //       IsNarcotic = vm.IsNarcotic,
            //       SuppliersList = vm.Suppliers.ToList(),
            //       TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == vm.ItemId).Sum(si => (int?)si.Quantity) ?? 0),
            //       ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == vm.ItemId select s).Sum(s => (int?)s.Quantity) ?? 0,
            //       ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == vm.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
            //       AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == vm.ItemId select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
            //   }).OrderByDescending(i => i.ItemId).ToList();

            List<ItemsVM> r = cxt.Items
            .Where(i => i.IsActive)
            .Select(vm => new ItemsVM
            {
                ItemId = vm.ItemId,
                ItemName = vm.ItemName,
                Barcode = vm.Barcode,
                Unit = vm.Unit,
                ReorderingLevel = vm.ReOrderingLevel,
                SuppliersList = vm.Suppliers.ToList(),
                TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == vm.ItemId).Sum(si => (double?)si.Quantity) ?? 0,
                ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == vm.ItemId select s).Sum(s => (double?)s.Quantity) ?? 0,
                ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == vm.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0,
                AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == vm.ItemId select ai).Sum(ai => (double?)ai.Quantity) ?? 0,
            }).OrderByDescending(i => i.ItemId).ToList();
            return r;
        }
        public IPagedList<ItemsVM> GetItemsWithStockDataByStockDate(DateTime ItemCreatedAt, int PageNo, int PageSize)
        {
            //            string qry = @"select x.ItemId, x.ItemName,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemId, i.ItemName,
            //                            (select isnull(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.StockId where si.Item_ItemID = i.ItemId and si.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.StockId where si.Item_ItemID = i.ItemId and si.IsActive = 1)
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from StockConsumptions s where s.Item_ItemID = i.ItemId and s.IsActive = 1)
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive = 1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) < CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.AdjustmentId where ai.Item_ItemID = i.ItemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.ItemId = si.Item_ItemID
            //                            where i.IsActive = 1 and CONVERT(varchar, i.CreatedAt,23) = @ItemCreatedAt
            //							group by ItemId, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemId = i.ItemId 
            //                            where i.IsActive = 1
            //                            order by x.ItemId Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", 1), new SqlParameter("@NextFetch", 100), new SqlParameter("@ItemCreatedAt", ItemCreatedAt.ToString("yyyy-MM-dd "))).ToList();


            IPagedList<ItemsVM> r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => (i.CreatedAt) == (ItemCreatedAt))
                .Where(i => !i.IsDefault)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemId,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == vm.ItemId).Sum(si => (double?)si.Quantity + si.BonusQuantity) ?? 0,
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == vm.ItemId select s).Sum(s => (double?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == vm.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == vm.ItemId select ai).Sum(ai => (double?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == vm.ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == vm.ItemId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);

            return r;
        }
        public IPagedList<ItemsVM> GetItemsWithStockDataByManuf(int ManunfId, int PageNo, int PageSize)
        {
            //            string qry = @"select x.ItemId, x.ItemName,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemId, i.ItemName,
            //                            (select isnull(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.StockId where si.Item_ItemID = i.ItemId and si.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.StockId where si.Item_ItemID = i.ItemId and si.IsActive = 1)
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from StockConsumptions s where s.Item_ItemID = i.ItemId and s.IsActive = 1)
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive = 1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) < CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.AdjustmentId where ai.Item_ItemID = i.ItemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.ItemId = si.Item_ItemID
            //                            where i.IsActive = 1 and CONVERT(varchar, i.CreatedAt,23) = @ItemCreatedAt
            //							group by ItemId, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemId = i.ItemId 
            //                            where i.IsActive = 1
            //                            order by x.ItemId Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", 1), new SqlParameter("@NextFetch", 100), new SqlParameter("@ItemCreatedAt", ItemCreatedAt.ToString("yyyy-MM-dd "))).ToList();


            IPagedList<ItemsVM> r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => i.Manufacturer.ManufacturerId == ManunfId)
                .Where(i => !i.IsDefault)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemId,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == vm.ItemId).Sum(si => (double?)si.Quantity + si.BonusQuantity) ?? 0,
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == vm.ItemId select s).Sum(s => (double?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == vm.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == vm.ItemId select ai).Sum(ai => (double?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == vm.ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == vm.ItemId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);

            return r;
        }
        public IPagedList<ItemsVM> GetItemsWithStockDataByCat(int CatId, int PageNo, int PageSize)
        {
            //            string qry = @"select x.ItemId, x.ItemName,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemId, i.ItemName,
            //                            (select isnull(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.StockId where si.Item_ItemID = i.ItemId and si.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.StockId where si.Item_ItemID = i.ItemId and si.IsActive = 1)
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from StockConsumptions s where s.Item_ItemID = i.ItemId and s.IsActive = 1)
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive = 1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) < CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.AdjustmentId where ai.Item_ItemID = i.ItemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.ItemId = si.Item_ItemID
            //                            where i.IsActive = 1 and CONVERT(varchar, i.CreatedAt,23) = @ItemCreatedAt
            //							group by ItemId, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemId = i.ItemId 
            //                            where i.IsActive = 1
            //                            order by x.ItemId Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", 1), new SqlParameter("@NextFetch", 100), new SqlParameter("@ItemCreatedAt", ItemCreatedAt.ToString("yyyy-MM-dd "))).ToList();


            IPagedList<ItemsVM> r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => i.Category.CategoryId == CatId)
                .Where(i => !i.IsDefault)
                //.Select(vm => new ItemsVM { }).ToPagedList(PageNo, PageSize);
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemId,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == vm.ItemId).Sum(si => (double?)si.Quantity + si.BonusQuantity) ?? 0,
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == vm.ItemId select s).Sum(s => (double?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == vm.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == vm.ItemId select ai).Sum(ai => (double?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == vm.ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == vm.ItemId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);

            return r;
        }
        public IPagedList<ItemsVM> GetItemsWithStockDataBySupp(int SuppId, int PageNo, int PageSize)
        {
            //            string qry = @"select x.ItemId, x.ItemName,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemId, i.ItemName,
            //                            (select isnull(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.StockId where si.Item_ItemID = i.ItemId and si.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.StockId where si.Item_ItemID = i.ItemId and si.IsActive = 1)
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from StockConsumptions s where s.Item_ItemID = i.ItemId and s.IsActive = 1)
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive = 1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) < CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.AdjustmentId where ai.Item_ItemID = i.ItemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.ItemId = si.Item_ItemID
            //                            where i.IsActive = 1 and CONVERT(varchar, i.CreatedAt,23) = @ItemCreatedAt
            //							group by ItemId, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemId = i.ItemId 
            //                            where i.IsActive = 1
            //                            order by x.ItemId Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", 1), new SqlParameter("@NextFetch", 100), new SqlParameter("@ItemCreatedAt", ItemCreatedAt.ToString("yyyy-MM-dd "))).ToList();

            //IPagedList<ItemsVM>
            var r = cxt.Suppliers
            .Where(s => s.SupplierID == SuppId && s.IsActive).FirstOrDefault();

            var r2 = cxt.Entry(r).Collection(s => s.Items)
                .Query()
                .Where(i => i.IsActive)
                .OrderByDescending(i => i.ItemId)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemId,
                    ItemName = vm.ItemName,
                    RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                    Manufacturer = vm.Manufacturer,
                    Unit = vm.Unit,
                    ConversionUnit = vm.ConversionUnit,
                    SuppliersList = vm.Suppliers.ToList(),
                    TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == vm.ItemId).Sum(si => (double?)si.Quantity + si.BonusQuantity) ?? 0,
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == vm.ItemId select s).Sum(s => (double?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == vm.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == vm.ItemId select ai).Sum(ai => (double?)ai.Quantity) ?? 0,
                    ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == vm.ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == vm.ItemId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                })
                .OrderByDescending(i => i.ItemId)
                .ToPagedList(PageNo, PageSize);

            return r2;
        }
        public List<ItemsVM> GetItemsWithStockDataByStockDate(DateTime ItemCreatedAt)
        {
            //            string qry = @"select x.ItemId, x.ItemName,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemId, i.ItemName,
            //                            (select isnull(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.StockId where si.Item_ItemID = i.ItemId and si.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems si inner join stocks s on s.stockId = si.StockId where si.Item_ItemID = i.ItemId and si.IsActive = 1)
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from StockConsumptions s where s.Item_ItemID = i.ItemId and s.IsActive = 1)
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive = 1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) < CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.AdjustmentId where ai.Item_ItemID = i.ItemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.ItemId = si.Item_ItemID
            //                            where i.IsActive = 1 and CONVERT(varchar, i.CreatedAt,23) = @ItemCreatedAt
            //							group by ItemId, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemId = i.ItemId 
            //                            where i.IsActive = 1
            //                            order by x.ItemId Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", 1), new SqlParameter("@NextFetch", 100), new SqlParameter("@ItemCreatedAt", ItemCreatedAt.ToString("yyyy-MM-dd "))).ToList();


            List<ItemsVM> r = cxt.Items
                .Where(i => i.IsActive)
                .Where(i => (i.CreatedAt) == (ItemCreatedAt))
                .Where(i => !i.IsDefault)
                .Select(vm => new ItemsVM
                {
                    ItemId = vm.ItemId,
                    ItemName = vm.ItemName,
                    TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == vm.ItemId).Sum(si => (double?)si.Quantity) ?? 0,
                    SuppliersList = vm.Suppliers.ToList(),
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == vm.ItemId select s).Sum(s => (double?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == vm.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == vm.ItemId select ai).Sum(ai => (double?)ai.Quantity) ?? 0,
                }).OrderByDescending(i => i.ItemId).ToList();
            return r;
        }
        public IPagedList<ItemsVM> GetItemsWithStockDataByItemNameFilter(string FilterString, int PageNo, int PageSize)
        {
            //            string qry = @"select x.ItemId, x.ItemName,
            //                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
            //                            select i.ItemId, i.ItemName,
            //                            (select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.ItemId and StockItems.IsActive = 1) as TotalStock,
            //                            (
            //                            	(select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.ItemId and StockItems.IsActive =1 )
            //                            	-
            //                            	(select ISNULL(sum(Quantity),0) from stockConsumptions s where s.Item_ItemID = i.ItemId and s.IsActive = 1)
            //                                -
            //                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive = 1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
            //                            ) as AvailableStock,
            //                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) <CONVERT(varchar,getdate(),23)) as ExpiredStock,
            //                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.AdjustmentId where ai.Item_ItemID = i.ItemId and ai.IsActive =1) as AdjustedStock
            //                            from items i
            //                            left join stockitems si on i.ItemId = si.Item_ItemID
            //                            where i.ItemName like '%" + FilterString + "%' AND i.IsActive = 1" +
            //                            @" group by ItemId, ItemName
            //                            )x                            
            //                            inner join Items i on x.ItemId = i.ItemId 
            //                            WHERE i.IsActive = 1
            //                            order by x.ItemId Desc
            //                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", OffSet), new SqlParameter("@NextFetch", NextFetch)).ToList();

            IPagedList<ItemsVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => i.ItemName.ToLower().Contains(FilterString))
               .Where(i => !i.IsDefault)
               .Select(vm => new ItemsVM
               {
                   ItemId = vm.ItemId,
                   ItemName = vm.ItemName,
                   RackNo = vm.Rack == null ? "" : vm.Rack.Name,
                   Manufacturer = vm.Manufacturer,
                   SuppliersList = vm.Suppliers.ToList(),
                   Unit = vm.Unit,
                   ConversionUnit = vm.ConversionUnit,
                   TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == vm.ItemId).Sum(si => (double?)si.Quantity + si.BonusQuantity) ?? 0,
                   ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == vm.ItemId select s).Sum(s => (double?)s.Quantity) ?? 0,
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == vm.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0,
                   AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == vm.ItemId select ai).Sum(ai => (double?)ai.Quantity) ?? 0,
                   ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == vm.ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                   HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == vm.ItemId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
               }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);
            return r;
        }
        public List<ItemsVM> GetItemsWithStockDataByItemNameFilter(string FilterString)
        {
            string qry = @"select x.ItemId, x.ItemName,
                            i.Barcode, '' as suppliers, i.ReOrderingLevel, i.Unit, (x.TotalStock + x.AdjustedStock) as TotalStock , (x.AvailableStock + x.AdjustedStock) as AvailableStock, x.ExpiredStock from (							
                            select i.ItemId, i.ItemName,
                            (select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.ItemId and StockItems.IsActive = 1) as TotalStock,
                            (
                            	(select ISNULL(sum(Quantity),0) from StockItems where StockItems.Item_ItemID = i.ItemId and StockItems.IsActive =1 )
                            	-
                            	(select ISNULL(sum(Quantity),0) from stockConsumptions s where s.Item_ItemID = i.ItemId and s.IsActive = 1)
                                -
                                (select ISNULL(sum(Quantity),0) from StockItems si inner join  Batches b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive = 1 and CONVERT(varchar, b.Expiry,23) < CONVERT(varchar, GETDATE(), 23))
                            ) as AvailableStock,
                            (select ISNULL(sum(Quantity),0) from StockItems si inner join [Batches] b on si.Batch_BatchId = b.BatchId where si.Item_ItemID = i.ItemId and si.IsActive = 1 and CONVERT(varchar,b.Expiry,23) <CONVERT(varchar,getdate(),23)) as ExpiredStock,
                            (select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.AdjustmentId where ai.Item_ItemID = i.ItemId and ai.IsActive =1) as AdjustedStock
                            from items i
                            left join stockitems si on i.ItemId = si.Item_ItemID
                            where i.ItemName like '%" + FilterString + "%' AND i.IsActive = 1" +
                            @" group by ItemId, ItemName
                            )x                            
                            inner join Items i on x.ItemId = i.ItemId 
                            WHERE i.IsActive = 1
                            order by x.ItemId Desc
                            OFFSET @OffSet ROWS FETCH NEXT @NextFetch ROWS ONLY";
            //List<ItemsVM> itemsList = cxt.Database.SqlQuery<ItemsVM>(qry, new SqlParameter("@OffSet", OffSet), new SqlParameter("@NextFetch", NextFetch)).ToList();

            List<ItemsVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => i.ItemName.ToLower().Contains(FilterString))
               .Where(i => !i.IsDefault)
               .Select(vm => new ItemsVM
               {
                   ItemId = vm.ItemId,
                   ItemName = vm.ItemName,
                   TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == vm.ItemId).Sum(si => (double?)si.Quantity) ?? 0,
                   SuppliersList = vm.Suppliers.ToList(),
                   ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == vm.ItemId select s).Sum(s => (double?)s.Quantity) ?? 0,
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == vm.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0,
                   AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == vm.ItemId select ai).Sum(ai => (double?)ai.Quantity) ?? 0,
               }).OrderByDescending(i => i.ItemId).ToList();
            return r;
        }
        public List<Item_SuppliersVM> GetSuppliersByItemIds(List<ItemsVM> itemsList)
        {
            List<long> ids = new List<long>();
            foreach (ItemsVM i in itemsList)
            {
                ids.Add(i.ItemId);
            }

            List<Item_SuppliersVM> vm = (from i in cxt.Items
                                         from s in i.Suppliers
                                         where ids.Contains(i.ItemId)
                                         select new Item_SuppliersVM()
                                         {
                                             ItemId = i.ItemId,
                                             SupplierId = s.SupplierID,
                                             SupplierName = s.Name
                                         }).ToList();
            return vm;
        }
        public List<BatchStockVM> GetBatchStockByItemId(int ItemId)
        {
            //            string qry = @"select x.Item_ItemID, x.batchId, x.BatchName, (x.TotalStock-x.ExpiredStock-x.ConsumedStock + x.AdjustedStock) as AvailableStock 
            //                        from(
            //                		select b.BatchId, b.BatchName, si.Item_ItemID, ISNULL(sum(Quantity),0) as TotalStock,
            //                		(select isnull( sum(Quantity), 0) from StockItems si_ inner join Batches b_ on si_.Batch_BatchId = b_.BatchId where si_.Item_ItemID = si.Item_ItemID and si_.Batch_BatchId = b.BatchId and si_.IsActive = 1 and CONVERT(varchar, b_.Expiry, 23) < CONVERT(varchar, GETDATE(), 23)) as ExpiredStock
            //                		,(select isnull( sum(Quantity), 0) from StockConsumptions sc_ inner join Batches b_ on sc_.Batch_BatchId = b_.BatchId where sc_.Item_ItemID = si.Item_ItemID and sc_.Batch_BatchId = b.BatchId and sc_.IsActive = 1) as ConsumedStock
            //                        ,(select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.AdjustmentId where ai.Item_ItemID = si.Item_itemId and ai.IsActive =1) as AdjustedStock
            //                		from stockitems si
            //                		inner join Batches b on si.Batch_BatchId = b.BatchId
            //                		where si.Item_ItemID = @ItemId and si.IsActive = 1
            //                		group by b.BatchId, b.BatchName, si.Item_ItemID
            //                	)x
            //                left join Batches b on b.BatchId = x.BatchId
            //                where (x.TotalStock-x.ExpiredStock-x.ConsumedStock) > 0";
            //            List<BatchStockVM> Result = cxt.Database.SqlQuery<BatchStockVM>(qry, new SqlParameter("@ItemId", ItemId)).ToList();

            if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
            {
                List<BatchStockVM> Result = (from si in cxt.StockItems
                                             join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                                             where si.Item.ItemId == ItemId
                                             group si by si.Batch.BatchId into BatchGroup
                                             join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
                                             select new BatchStockVM()
                                             {
                                                 BatchId = b_g.BatchId,
                                                 BatchName = b_g.BatchName,
                                                 TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (double?)ts.Quantity + ts.BonusQuantity) ?? 0,
                                                 ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                                                 ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == b_g.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity + es.BonusQuantity) ?? 0,
                                                 AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (double?)a.Quantity) ?? 0
                                             })
                                             .OrderBy(r => r.BatchId)
                                             .ToList();
                return Result;
            }
            else
            {
                List<BatchStockVM> Result = (from si in cxt.StockItems
                                             join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                                             where si.Item.ItemId == ItemId
                                             group si by si.Batch.BatchId into BatchGroup
                                             join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
                                             select new BatchStockVM()
                                             {
                                                 BatchId = b_g.BatchId,
                                                 BatchName = b_g.BatchName,
                                                 TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (double?)ts.Quantity + ts.BonusQuantity) ?? 0,
                                                 ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                                                 ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == b_g.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0,
                                                 AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (double?)a.Quantity) ?? 0
                                             })
                                             .OrderByDescending(r => r.BatchId)
                                             .ToList();
                return Result;
            }
        }
        public ItemDetailVM GetItemDetailWithBatchStock(long ItemId, bool useNewestStockPrice)
        {
            ItemDetailVM Result = new ItemDetailVM();
            Result.Item = cxt.Items.Where(i => i.ItemId == ItemId).Include(i => i.Rack).FirstOrDefault();
            if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
            {
                List<BatchStockVM> bStock = (from si in cxt.StockItems
                                             join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                                             where si.Item.ItemId == ItemId
                                             group si by si.Batch.BatchId into BatchGroup
                                             join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
                                             select new BatchStockVM
                                             {
                                                 BatchId = b_g.BatchId,
                                                 BatchName = b_g.BatchName,
                                                 TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (double?)ts.Quantity + ts.BonusQuantity) ?? 0,
                                                 ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                                                 HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                                                 //SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold ?
                                                 //(from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0
                                                 //:
                                                 //(from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0
                                                 //+
                                                 //(from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                                 ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == b_g.BatchId && (b_.Expiry) < (DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (double?)rs.Quantity) ?? 0
                                                                -
                                                                (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == b_g.BatchId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                                                 AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
                                                 RetailPrice =
                                                 useNewestStockPrice ?
                                                 (double?)(from es in cxt.StockItems where es.Item.ItemId == ItemId orderby es.StockItemId descending select es.RetailPrice).FirstOrDefault() ?? 0
                                                 :
                                                 (double?)(from es in cxt.StockItems where es.Item.ItemId == ItemId && es.Batch.BatchId == b_g.BatchId orderby es.StockItemId descending select es.RetailPrice).FirstOrDefault() ?? 0,
                                                 CostPrice =
                                                 useNewestStockPrice ?
                                                 (double?)(from es in cxt.StockItems where es.Item.ItemId == ItemId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0
                                                 :
                                                 (double?)(from es in cxt.StockItems where es.Item.ItemId == ItemId && es.Batch.BatchId == b_g.BatchId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0,
                                             })
                                           .OrderBy(r => r.BatchId)
                                           .ToList();
                Result.BatchStockList = bStock;
            }
            else
            {
                List<BatchStockVM> bStock = (from si in cxt.StockItems
                                             join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                                             where si.Item.ItemId == ItemId
                                             group si by si.Batch.BatchId into BatchGroup
                                             join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
                                             select new BatchStockVM
                                             {
                                                 BatchId = b_g.BatchId,
                                                 BatchName = b_g.BatchName,
                                                 TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (double?)ts.Quantity + ts.BonusQuantity) ?? 0,
                                                 ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                                                 HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                                                 //SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold ?
                                                 //    (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0
                                                 //    :
                                                 //    (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0
                                                 //    +
                                                 //    (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
                                                 ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == b_g.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity + es.BonusQuantity) ?? 0,
                                                 AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
                                                 RetailPrice = (double?)(from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == b_g.BatchId && (b_.Expiry == null || (b_.Expiry) > (DateTime.Now)) select es).OrderByDescending(r => r.StockItemId).FirstOrDefault().RetailPrice ?? 0,
                                             })
                                           .OrderByDescending(r => r.BatchId)
                                           .ToList();
                Result.BatchStockList = bStock;
            }
            return Result;
        }
        public ItemDetailVM GetItemDetail(long ItemId, bool useNewestStockPrice)
        {
            ItemDetailVM Result = new ItemDetailVM();
            Result.Item = cxt.Items.Where(i => i.ItemId == ItemId).Include(i => i.Rack).FirstOrDefault();
            //if (SharedVariables.AdminPharmacySetting.IsItemConumptionFifo)
            //{
            //    List<BatchStockVM> bStock = (from si in cxt.StockItems
            //                                 join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
            //                                 where si.Item.ItemId == ItemId
            //                                 group si by si.Batch.BatchId into BatchGroup
            //                                 join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
            //                                 select new BatchStockVM
            //                                 {
            //                                     BatchId = b_g.BatchId,
            //                                     BatchName = b_g.BatchName,
            //                                     TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (double?)ts.Quantity + ts.BonusQuantity) ?? 0,
            //                                     ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
            //                                     HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
            //                                     //SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold ?
            //                                     //(from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0
            //                                     //:
            //                                     //(from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0
            //                                     //+
            //                                     //(from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
            //                                     ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == b_g.BatchId && (b_.Expiry) < (DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (double?)rs.Quantity) ?? 0
            //                                                    -
            //                                                    (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == b_g.BatchId && (b_.Expiry) < (DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
            //                                     AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
            //                                     RetailPrice =
            //                                     useNewestStockPrice ?
            //                                     (double?)(from es in cxt.StockItems where es.Item.ItemId == ItemId orderby es.StockItemId descending select es.RetailPrice).FirstOrDefault() ?? 0
            //                                     :
            //                                     (double?)(from es in cxt.StockItems where es.Item.ItemId == ItemId && es.Batch.BatchId == b_g.BatchId orderby es.StockItemId descending select es.RetailPrice).FirstOrDefault() ?? 0,
            //                                     CostPrice =
            //                                     useNewestStockPrice ?
            //                                     (double?)(from es in cxt.StockItems where es.Item.ItemId == ItemId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0
            //                                     :
            //                                     (double?)(from es in cxt.StockItems where es.Item.ItemId == ItemId && es.Batch.BatchId == b_g.BatchId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0,
            //                                 })
            //                               .OrderBy(r => r.BatchId)
            //                               .ToList();
            //    Result.BatchStockList = bStock;
            //}
            //else
            //{
            //    List<BatchStockVM> bStock = (from si in cxt.StockItems
            //                                 join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
            //                                 where si.Item.ItemId == ItemId
            //                                 group si by si.Batch.BatchId into BatchGroup
            //                                 join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
            //                                 select new BatchStockVM
            //                                 {
            //                                     BatchId = b_g.BatchId,
            //                                     BatchName = b_g.BatchName,
            //                                     TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (double?)ts.Quantity + ts.BonusQuantity) ?? 0,
            //                                     ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
            //                                     HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
            //                                     //SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold ?
            //                                     //    (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0
            //                                     //    :
            //                                     //    (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0
            //                                     //    +
            //                                     //    (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
            //                                     ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == b_g.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity + es.BonusQuantity) ?? 0,
            //                                     AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
            //                                     RetailPrice = (double?)(from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == b_g.BatchId && (b_.Expiry == null || (b_.Expiry) > (DateTime.Now)) select es).OrderByDescending(r => r.StockItemId).FirstOrDefault().RetailPrice ?? 0,
            //                                 })
            //                               .OrderByDescending(r => r.BatchId)
            //                               .ToList();
            //    Result.BatchStockList = bStock;
            //}
            return Result;
        }
        public double GetItemAvailableQty(int ItemId)
        {
            var AvStock = (from si in cxt.StockItems
                           where si.Item.ItemId == ItemId
                           select new
                           {
                               TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == ItemId select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (double?)r.Qty + r.bQty) ?? 0,
                               ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                               HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,

                               ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && (b_.Expiry) < (DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (double?)rs.Quantity) ?? 0
                                              -
                                              (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                               AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == ItemId && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (double?)a.Qty) ?? 0
                           }).FirstOrDefault();
            // if deduct hol quantity
            if (AvStock != null)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    return AvStock.TotalStock + AvStock.AdjustedStock - AvStock.ExpiredStock - AvStock.ConsumedStock - AvStock.HoldStock;
                }
                else
                {
                    return AvStock.TotalStock + AvStock.AdjustedStock - AvStock.ExpiredStock - AvStock.ConsumedStock;
                }
            }
            else
            {
                return 0;
            }
        }

        public ItemsVM GetItemWithAvQty(long ItemId)
        {
            ItemsVM res = cxt.Items.Where(i => i.ItemId == ItemId)
                .Select(i => new ItemsVM
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    ConversionUnit = i.ConversionUnit,
                    Unit = i.Unit,
                    UnitCost = i.UnitCostPrice,
                    RetailPrice = i.RetailPrice,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (double?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && (b_.Expiry) < (DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (double?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == ItemId && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (double?)a.Qty) ?? 0
                }).FirstOrDefault();

            if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                res.AvailableStock = res.TotalStock + res.AdjustedStock - res.ConsumedStock - res.ExpiredStock;
            }
            else
            {
                res.AvailableStock = res.TotalStock + res.AdjustedStock - res.ConsumedStock - res.ExpiredStock + res.HoldStock;
            }
            return res;
        }

        public ItemDetailVM GetItemDetailWithStock(int ItemId)
        {
            //            string qry = @"select x.Item_ItemID, x.batchId, x.BatchName, (x.TotalStock-x.ExpiredStock-x.ConsumedStock + x.AdjustedStock) as AvailableStock 
            //                        from(
            //                		select b.BatchId, b.BatchName, si.Item_ItemID, ISNULL(sum(Quantity),0) as TotalStock,
            //                		(select isnull( sum(Quantity), 0) from StockItems si_ inner join Batches b_ on si_.Batch_BatchId = b_.BatchId where si_.Item_ItemID = si.Item_ItemID and si_.Batch_BatchId = b.BatchId and si_.IsActive = 1 and CONVERT(varchar, b_.Expiry, 23) < CONVERT(varchar, GETDATE(), 23)) as ExpiredStock
            //                		,(select isnull( sum(Quantity), 0) from StockConsumptions sc_ inner join Batches b_ on sc_.Batch_BatchId = b_.BatchId where sc_.Item_ItemID = si.Item_ItemID and sc_.Batch_BatchId = b.BatchId and sc_.IsActive = 1) as ConsumedStock
            //                        ,(select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.AdjustmentId where ai.Item_ItemID = si.Item_itemId and ai.IsActive =1) as AdjustedStock
            //                		from stockitems si
            //                		inner join Batches b on si.Batch_BatchId = b.BatchId
            //                		where si.Item_ItemID = @ItemId and si.IsActive = 1
            //                		group by b.BatchId, b.BatchName, si.Item_ItemID
            //                	)x
            //                left join Batches b on b.BatchId = x.BatchId
            //                where (x.TotalStock-x.ExpiredStock-x.ConsumedStock) > 0";
            //            List<BatchStockVM> Result = cxt.Database.SqlQuery<BatchStockVM>(qry, new SqlParameter("@ItemId", ItemId)).ToList();

            ItemDetailVM Result = (from si in cxt.StockItems
                                   where si.Item.ItemId == ItemId && si.IsActive
                                   select new ItemDetailVM
                                   {
                                       Item = cxt.Items.Where(i => i.ItemId == ItemId).FirstOrDefault(),
                                       AvailableStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == ItemId select ts).Sum(ts => (double?)ts.Quantity + ts.BonusQuantity) ?? 0
                                        - (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId select cs).Sum(cs => (double?)cs.Quantity) ?? 0
                                        - (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.IsActive && es.Item.ItemId == ItemId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity + es.BonusQuantity) ?? 0
                                        + (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == ItemId && adj_item.IsActive select adj_item).Sum(a => (double?)a.Quantity) ?? 0
                                   }).FirstOrDefault();
            return Result;
        }
        public BatchStockVM GetBatchStock(long batchId, long ItemId, bool UseNewestStockPrice)
        {
            //            string qry = @"select x.Item_ItemID, x.batchId, x.BatchName, (x.TotalStock-x.ExpiredStock-x.ConsumedStock + x.AdjustedStock) as AvailableStock 
            //                        from(
            //                		select b.BatchId, b.BatchName, si.Item_ItemID, ISNULL(sum(Quantity),0) as TotalStock,
            //                		(select isnull( sum(Quantity), 0) from StockItems si_ inner join Batches b_ on si_.Batch_BatchId = b_.BatchId where si_.Item_ItemID = si.Item_ItemID and si_.Batch_BatchId = b.BatchId and si_.IsActive = 1 and CONVERT(varchar, b_.Expiry, 23) < CONVERT(varchar, GETDATE(), 23)) as ExpiredStock
            //                		,(select isnull( sum(Quantity), 0) from StockConsumptions sc_ inner join Batches b_ on sc_.Batch_BatchId = b_.BatchId where sc_.Item_ItemID = si.Item_ItemID and sc_.Batch_BatchId = b.BatchId and sc_.IsActive = 1) as ConsumedStock
            //                        ,(select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.AdjustmentId where ai.Item_ItemID = si.Item_itemId and ai.IsActive =1) as AdjustedStock
            //                		from stockitems si
            //                		inner join Batches b on si.Batch_BatchId = b.BatchId
            //                		where si.Item_ItemID = @ItemId and si.IsActive = 1
            //                		group by b.BatchId, b.BatchName, si.Item_ItemID
            //                	)x
            //                left join Batches b on b.BatchId = x.BatchId
            //                where (x.TotalStock-x.ExpiredStock-x.ConsumedStock) > 0";
            //List<BatchStockVM> Result = cxt.Database.SqlQuery<BatchStockVM>(qry, new SqlParameter("@ItemId", ItemId)).ToList();

            //BatchStockVM Result = (from si in cxt.StockItems
            //                       join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
            //                       where si.Item.ItemId == ItemId && si.Batch.BatchId == batchId
            //                       group si by si.Batch.BatchId into BatchGroup
            //                       join b_g in cxt.Batches on BatchGroup.Key equals b_g.BatchId
            //                       select new BatchStockVM()
            //                       {
            //                           BatchId = b_g.BatchId,
            //                           BatchName = b_g.BatchName,
            //                           TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == ItemId && ts.Batch.BatchId == b_g.BatchId select ts).Sum(ts => (int?)(ts.Quantity + ts.BonusDiscount)) ?? 0,
            //                           ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == b_g.BatchId select cs).Sum(cs => (int?)cs.Quantity) ?? 0,
            //                           ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == b_g.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (int?)es.Quantity) ?? 0,
            //                           AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == b_g.BatchId select adj_item).Sum(a => (int?)a.Quantity) ?? 0,
            //                           RetailPrice = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == b_g.BatchId && (b_.Expiry == null || (b_.Expiry) > (DateTime.Now)) select es).OrderByDescending(r => r.StockItemId).FirstOrDefault().RetailPrice,
            //                       }).FirstOrDefault();
            //return Result;
            BatchStockVM Result =
                                  new BatchStockVM()
                                  {
                                      TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == ItemId && ts.Batch.BatchId == batchId select ts).Sum(ts => (double?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                                      ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                                      HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                                      ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == batchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0
                                                      -
                                                     (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == batchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0,

                                      AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == ItemId && adj_item.IsActive && adj_item.Batch.BatchId == batchId select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
                                      //RetailPrice = (double?) (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == batchId && (b_.Expiry == null || (b_.Expiry) > (DateTime.Now)) select es).OrderByDescending(r => r.StockItemId).FirstOrDefault().RetailPrice ?? 0,
                                      RetailPrice =
                                       UseNewestStockPrice ?
                                                        (double?)(from es in cxt.StockItems where es.Item.ItemId == ItemId orderby es.StockItemId descending select es.RetailPrice).FirstOrDefault() ?? 0
                                                        :
                                                        (double?)(from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == batchId orderby es.StockItemId descending select es.RetailPrice).FirstOrDefault() ?? 0,
                                      CostPrice =
                                       UseNewestStockPrice ?
                                                       (double?)(from es in cxt.StockItems where es.Item.ItemId == ItemId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0
                                                       :
                                                       (double?)(from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemId && es.Batch.BatchId == batchId orderby es.StockItemId descending select es.UnitCost).FirstOrDefault() ?? 0
                                  };
            return Result;
        }

        //public List<BatchStockVM> GetBatchStockByItemId(long ItemId, long BatchId)
        //{
        //    string qry = @"select x.Item_ItemID, x.batchId, x.BatchName, (x.TotalStock-x.ExpiredStock-x.ConsumedStock + x.AdjustedStock) as AvailableStock 
        //                from(
        //        		select b.BatchId, b.BatchName, si.Item_ItemID, ISNULL(sum(Quantity),0) as TotalStock,
        //        		(select isnull( sum(Quantity), 0) from StockItems si_ inner join Batches b_ on si_.Batch_BatchId = b_.BatchId where si_.Item_ItemID = si.Item_ItemID and si_.Batch_BatchId = b.BatchId and si_.IsActive = 1 and CONVERT(varchar, b_.Expiry, 23) < CONVERT(varchar, GETDATE(), 23)) as ExpiredStock
        //        		,(select isnull( sum(Quantity), 0) from StockConsumptions sc_ inner join Batches b_ on sc_.Batch_BatchId = b_.BatchId where sc_.Item_ItemID = si.Item_ItemID and sc_.Batch_BatchId = b.BatchId and sc_.IsActive = 1) as ConsumedStock
        //                ,(select isnull(sum(Quantity),0) from AdjustmentItems ai inner join Adjustments a on a.AdjustmentId = ai.AdjustmentId where ai.Item_ItemID = si.Item_ItemID and ai.IsActive =1) as AdjustedStock
        //        		from stockitems si
        //        		left join Batches b on si.Batch_BatchId = b.BatchId
        //        		where si.Item_ItemID = @ItemId and si.IsActive = 1
        //                AND si.Batch_BatchId = @BatchID
        //        		group by b.BatchId, b.BatchName, si.Item_ItemID
        //        	)x
        //        left join Batches b on b.BatchId = x.BatchId
        //        where (x.TotalStock-x.ExpiredStock-x.ConsumedStock) > 0";

        //    List<BatchStockVM> result = cxt.Set<BatchStockVM>()
        //.FromSqlRaw(sql, itemId, batchId)
        //.ToList();

        //    return result;

        //    //List<BatchStockVM> result = cxt.Database.SqlQuery<BatchStockVM>(qry, new SqlParameter("@ItemId", ItemId), new SqlParameter("@BatchID", BatchId)).ToList();
        //    //return result;
        //}

        public double GetItemRate(int iItemId, int? BatchId)
        {
            return cxt.StockItems.Where(i => i.Item.ItemId == iItemId && i.Batch.BatchId == BatchId).OrderByDescending(i => i.StockItemId).Select(si => si.RetailPrice).FirstOrDefault();
            //return obj.RetailPrice;
        }

        public double GetUnitCost(long iItemId, long? BatchId)
        {
            return cxt.StockItems.Where(i => i.Item.ItemId == iItemId && i.Batch.BatchId == BatchId).OrderByDescending(i => i.StockItemId).Select(si => si.UnitCost).FirstOrDefault();
        }

        public Item GetItemData_ByITemId(int ItemId)
        {
            return cxt.Items
                .Where(i => i.ItemId == ItemId)
                .Include(i => i.Suppliers)
                .Include(i => i.Manufacturer)
                .Include(i => i.Rack)
                .Include(i => i.Category)
                //.Include(i => i.Category1)
                .FirstOrDefault();
        }


        public int ItemAlreadyExists(string itemName, string barcode)
        {
            itemName = itemName?.ToLower() ?? string.Empty;
            barcode = barcode?.ToLower() ?? string.Empty;

            // Check if item exists with same name
            bool nameExists = cxt.Items
                .Any(i => i.IsActive && i.ItemName.ToLower() == itemName);

            if (nameExists)
                return 1;

            // Check if barcode exists (only if barcode is not empty)
            bool barcodeExists = !string.IsNullOrWhiteSpace(barcode)
                && cxt.Items.Any(i => i.IsActive
                                      && i.Barcode != null
                                      && i.Barcode.ToLower() == barcode);

            return barcodeExists ? 2 : 0;
        }

        public int ItemAlreadyExists(long ItemId, string ItemName, string Barcode)
        {
            ItemName = ItemName?.ToLower() ?? string.Empty;
            Barcode = Barcode?.ToLower() ?? string.Empty;

            // Check if another item exists with same name (excluding current item)
            bool nameExists = cxt.Items
                .Any(i => i.IsActive
                        && i.ItemId != ItemId
                        && i.ItemName.ToLower() == ItemName);

            if (nameExists)
                return 1;

            // Check if barcode exists on another item (only if barcode is not empty)
            bool barcodeExists = !string.IsNullOrWhiteSpace(ItemName)
                && cxt.Items.Any(i => i.IsActive
                                    && i.ItemId != ItemId
                                    && i.Barcode != null
                                    && i.Barcode.ToLower() == Barcode);

            return barcodeExists ? 2 : 0;

        }


        public ItemWiseSalesMasterVM GetItemWiseSales(DateTime? DateFrom, DateTime? DateTo, long? ItemId, int? PageNo, int? PageSize)
        {
            ItemWiseSalesMasterVM v = new ItemWiseSalesMasterVM();
            var query = (from i in cxt.Invoices
                         join ii in cxt.InvoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                         where i.IsActive && ii.IsActive
                         && ii.ReturnedQuantity != ii.Quantity
                         && (ItemId.HasValue && ItemId.Value > 0 && ii.Item.ItemId == ItemId || !ItemId.HasValue || ItemId.Value <= 0)
                         && (DateFrom.HasValue && (i.CreatedAt) >= (DateFrom) && (i.CreatedAt) <= (DateTo) || !DateFrom.HasValue)
                         group ii by ii.Item.ItemId into ItemsG
                         select new ItemWiseSaleVM
                         {
                             ItemId = ItemsG.Key,
                             ItemName = (from item in cxt.Items where item.ItemId == ItemsG.Key select item.ItemName).FirstOrDefault(),
                             SaleCount = ItemsG.Sum(item => item.Quantity) - ItemsG.Sum(item => item.ReturnedQuantity),
                             TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == ItemsG.Key select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (double?)r.Qty + r.bQty) ?? 0,
                             ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                             HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                             ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemsG.Key && (b_.Expiry) < (DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (double?)rs.Quantity) ?? 0
                                            -
                                            (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemsG.Key && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                             AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == ItemsG.Key && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (double?)a.Qty) ?? 0,
                         }).OrderBy(i => i.ItemName);

            v.GrandSalesTotalQuantity = query.Sum(r => (double?)r.SaleCount) ?? 0;
            IPagedList<ItemWiseSaleVM> SalesList = query.ToPagedList(PageNo.HasValue ? PageNo.Value : 1, PageSize.HasValue ? PageSize.Value : int.MaxValue);


            // just filling available stock variable by looping above retrived results
            foreach (ItemWiseSaleVM i in SalesList.Items)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
                else
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
            }

            v.ItemWiseSales = SalesList;
            return v;
        }

        public ItemWisePurchaseMasterVM GetItemWisePurchases(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            IPagedList<ItemWisePurchaseVM> PurchasesList = (from i in cxt.Stocks
                                                            join ii in cxt.StockItems on i equals ii.Stock
                                                            where i.IsActive && ii.IsActive && ii.Quantity > 0
                                                            && (i.CreatedAt) >= (DateFrom)
                                                            && (i.CreatedAt) <= (DateTo)
                                                            //orderby i.InvoiceId descending
                                                            group ii by ii.Item.ItemId into ItemsG
                                                            select new ItemWisePurchaseVM
                                                            {
                                                                ItemId = ItemsG.Key,
                                                                ItemName = (from item in cxt.Items where item.ItemId == ItemsG.Key select item.ItemName).FirstOrDefault(),
                                                                PurchaseCount = ItemsG.Sum(i => i.Quantity),
                                                                TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == ItemsG.Key select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (double?)r.Qty + r.bQty) ?? 0,
                                                                ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                                                                HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                                                                ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemsG.Key && (b_.Expiry) < (DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (double?)rs.Quantity) ?? 0
                                                                               -
                                                                               (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemsG.Key && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                                                                AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == ItemsG.Key && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (double?)a.Qty) ?? 0,
                                                            }).OrderBy(i => i.ItemName).ToPagedList(PageNo, PageSize);



            // just filling available stock variable by looping above retrived results
            foreach (ItemWisePurchaseVM i in PurchasesList.Items)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
                else
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
            }


            ItemWisePurchaseMasterVM v = new ItemWisePurchaseMasterVM();
            v.GrandPurchaseTotalQuantity = cxt.Stocks
                                               .Where(i => i.IsActive
                                                   && (i.CreatedAt) >= (DateFrom) &&
                                                   (i.CreatedAt) <= (DateTo))
                                                    //.Sum(inv => (long?)inv.StockItems.Where(item => item.IsActive)
                                                        .Sum(item => 0);
            v.ItemWisePurchases = PurchasesList;
            return v;
        }

        public ItemWisePurchaseMasterVM GetItemWisePurchases(DateTime DateFrom, DateTime DateTo, int ItemId)
        {
            IPagedList<ItemWisePurchaseVM> PurchasesList = (from i in cxt.Stocks
                                                            join ii in cxt.StockItems on i equals ii.Stock
                                                            where i.IsActive && ii.IsActive && ii.Quantity > 0 && ii.Item.ItemId == ItemId
                                                            && (i.CreatedAt) >= (DateFrom)
                                                            && (i.CreatedAt) <= (DateTo)
                                                            //orderby i.InvoiceId descending
                                                            group ii by ii.Item.ItemId into ItemsG
                                                            select new ItemWisePurchaseVM
                                                            {
                                                                ItemId = ItemsG.Key,
                                                                ItemName = (from item in cxt.Items where item.ItemId == ItemsG.Key select item.ItemName).FirstOrDefault(),
                                                                PurchaseCount = ItemsG.Sum(i => i.Quantity),
                                                                TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == ItemsG.Key select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (double?)r.Qty + r.bQty) ?? 0,
                                                                ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                                                                HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemsG.Key select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                                                                ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemsG.Key && (b_.Expiry) < (DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (double?)rs.Quantity) ?? 0
                                                                               -
                                                                               (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == ItemsG.Key && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                                                                AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == ItemsG.Key && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (double?)a.Qty) ?? 0,
                                                            }).OrderBy(i => i.ItemName).ToPagedList(1, 1);


            // just filling available stock variable by looping above retrived results
            foreach (ItemWisePurchaseVM i in PurchasesList.Items)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
                else
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
            }


            ItemWisePurchaseMasterVM v = new ItemWisePurchaseMasterVM();
            v.GrandPurchaseTotalQuantity = cxt.Stocks
                                               .Where(i => i.IsActive
                                                   && (i.CreatedAt) >= (DateFrom) &&
                                                   (i.CreatedAt) <= (DateTo))
                                                    .Sum(inv => (long?)inv.StockItems.Where(item => item.IsActive)
                                                        .Sum(item => item.Quantity));
            v.ItemWisePurchases = PurchasesList;
            return v;
        }

        public ItemWiseRevenueMasterVM GetItemWiseRevenue_Item(DateTime? startDate, DateTime? endDate, long? CategoryId, long? ItemId, int? pageNo, int? pageSize)
        {
            //var x = (from i in cxt.Invoices
            //         join ii in cxt.invoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
            //         //join ip in cxt.invoicePayments on i.InvoiceId equals ip.Invoice.InvoiceId
            //         where (i.IsActive && ii.IsActive && ii.Item.ItemId == ItemId)
            //         group i by ii.Item
            //     ).OrderByDescending(r => r.Key.ItemId).ToPagedList(pageNo, pageSize);
            //var valueOfX = x;
            //List<long> ids = new List<long>();
            //List<ItemWiseRevenueVM> lst = new List<ItemWiseRevenueVM>();
            //double revenue = 0;
            //double perItemPaymentShare = 0;
            //double ActualSaleQty = 0;
            //foreach (IGrouping<Item, Invoice> i in x)
            //{
            //    foreach (Invoice t in i)
            //    {
            //        var r = cxt.invoiceItems.Where(inv => inv.Invoice.InvoiceId == t.InvoiceId && inv.Item.ItemId == i.Key.ItemId).FirstOrDefault();
            //        var invoice = cxt.Invoices.Where(inv => inv.InvoiceId == t.InvoiceId).FirstOrDefault();
            //        double actualPaid = (double?)cxt.Entry(invoice).Collection(inv => inv.InvoicePayments).Query().Where(p => p.IsActive && p.Payment > 0).Sum(p => (double?)p.Payment) ?? 0;
            //        ActualSaleQty = r.Quantity - r.ReturnedQuantity;
            //        //perItemPaymentShare = (((r.Amount / r.Quantity) * ActualSaleQty) / t.SubTotal) * actualPaid;
            //        perItemPaymentShare = (((r.Amount / t.SubTotal) * actualPaid) / r.Quantity);
            //        if (double.IsNaN(perItemPaymentShare)) { perItemPaymentShare = 0; }
            //        revenue += ActualSaleQty * perItemPaymentShare;
            //    }
            //    ItemWiseRevenueVM vm = new ItemWiseRevenueVM
            //    {
            //        ItemName = i.Key.ItemName,
            //        Revenue = revenue
            //    };
            //    lst.Add(vm);
            //    ids.Add(i.Key.ItemId);
            //}

            //var res = cxt.Items.Where(i => ids.Contains(i.ItemId))
            //    .Select(i => new AvailableQtyVM
            //    {
            //        ItemId = i.ItemId,
            //        TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (double?)r.Qty + r.bQty) ?? 0,
            //        ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
            //        HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == i.ItemId select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
            //        ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && (b_.Expiry) < (DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (double?)rs.Quantity) ?? 0
            //                       -
            //                       (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && (b_.Expiry) < (DateTime.Now) select new { Quantity = es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
            //        AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (double?)a.Qty) ?? 0,
            //    }).ToList();

            //foreach (var i in res)
            //{
            //    ItemWiseRevenueVM obj = lst.Where(vm => vm.ItemId == vm.ItemId).FirstOrDefault();
            //    if (obj != null)
            //    {
            //        if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            //        {
            //            obj.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
            //        }
            //        else
            //        {
            //            obj.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
            //        }
            //    }
            //}

            //ItemWiseRevenueMasterVM v = new ItemWiseRevenueMasterVM();
            //v.HasNextPage = x.HasNextPage;
            //v.HasPreviousPage = x.HasPreviousPage;
            //v.IsFirstPage = x.IsFirstPage;
            //v.IsLastPage = x.IsLastPage;
            //v.PageCount = x.PageCount;
            //v.ItemWiseRevenue = lst;
            //return v;
            var itemWiseRevenue = (from i in cxt.Invoices
                                   join ii in cxt.InvoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                                   where i.IsActive && ii.IsActive
                                   && (startDate.HasValue && (ii.CreatedAt) >= (startDate) && (ii.CreatedAt) <= (endDate) || !startDate.HasValue)
                                   && (CategoryId.HasValue && CategoryId.Value > 0 && ii.Item.CategoryId == CategoryId || !CategoryId.HasValue || CategoryId.Value <= 0)
                                   && (ItemId.HasValue && ItemId.Value > 0 && ii.ItemId == ItemId || !ItemId.HasValue || ItemId.Value <= 0)
                                   group ii by ii.Item into grpResult
                                   select new ItemWiseRevenueVM
                                   {
                                       ItemId = grpResult.Key.ItemId,
                                       ItemName = grpResult.Key.ItemName,
                                       Revenue = grpResult.Sum(r => r.NetAmount)
                                   }
                 ).OrderByDescending(r => r.ItemId)
                 .ToPagedList(pageNo.HasValue ? pageNo.Value : 1, pageSize.HasValue ? pageSize.Value : int.MaxValue);

            ItemWiseRevenueMasterVM v = new ItemWiseRevenueMasterVM();
            v.HasNextPage = itemWiseRevenue.HasNextPage;
            v.HasPreviousPage = itemWiseRevenue.HasPreviousPage;
            v.IsFirstPage = itemWiseRevenue.IsFirstPage;
            v.IsLastPage = itemWiseRevenue.IsLastPage;
            v.PageCount = itemWiseRevenue.PageCount;
            v.ItemWiseRevenue = itemWiseRevenue.ToList();
            return v;
        }

        public List<ItemWiseSaleVM> GetItemWiseSales_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            List<ItemWiseSaleVM> SalesList = (from i in cxt.Invoices
                                              join ii in cxt.InvoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                                              where i.IsActive && ii.IsActive
                                              && (i.CreatedAt) >= (DateFrom)
                                              && (i.CreatedAt) <= (DateTo)
                                              //orderby i.InvoiceId descending
                                              group ii by ii.Item.ItemName into ItemsG
                                              select new ItemWiseSaleVM
                                              {
                                                  ItemName = ItemsG.Key,
                                                  SaleCount = ItemsG.Sum(i => i.Quantity) - ItemsG.Sum(i => i.ReturnedQuantity),
                                              }).OrderBy(i => i.ItemName).ToList();
            return SalesList;
        }
        public List<ItemWiseSaleVM> GetItemWiseSales_Rpt(DateTime DateFrom, DateTime DateTo, int ItemId)
        {
            List<ItemWiseSaleVM> SalesList = (from i in cxt.Invoices
                                              join ii in cxt.InvoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                                              where i.IsActive && ii.IsActive && ii.Item.ItemId == ItemId
                                              && (i.CreatedAt) >= (DateFrom)
                                              && (i.CreatedAt) <= (DateTo)
                                              group ii by ii.Item.ItemName into ItemsG
                                              select new ItemWiseSaleVM
                                              {
                                                  ItemName = ItemsG.Key,
                                                  SaleCount = ItemsG.Sum(i => i.Quantity) - ItemsG.Sum(i => i.ReturnedQuantity),
                                              }).OrderBy(i => i.ItemName).ToList();
            return SalesList;
        }

        public List<ItemWisePurchaseVM> GetItemWisePurchase_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            List<ItemWisePurchaseVM> PurchaseList = (from i in cxt.Stocks
                                                     join ii in cxt.StockItems on i equals ii.Stock
                                                     where i.IsActive && ii.IsActive
                                                     && (i.CreatedAt) >= (DateFrom)
                                                     && (i.CreatedAt) <= (DateTo)
                                                     //orderby i.InvoiceId descending
                                                     group ii by ii.Item.ItemName into ItemsG
                                                     select new ItemWisePurchaseVM
                                                     {
                                                         ItemName = ItemsG.Key,
                                                         PurchaseCount = ItemsG.Sum(i => i.Quantity),
                                                     }).OrderBy(i => i.ItemName).ToList();
            return PurchaseList;
        }

        public List<ItemWisePurchaseVM> GetItemWisePurchase_Rpt(DateTime DateFrom, DateTime DateTo, int ItemId)
        {
            List<ItemWisePurchaseVM> PurchaseList = (from i in cxt.Stocks
                                                     join ii in cxt.StockItems on i equals ii.Stock
                                                     where i.IsActive && ii.IsActive && ii.Item.ItemId == ItemId
                                                     && (i.CreatedAt) >= (DateFrom)
                                                     && (i.CreatedAt) <= (DateTo)
                                                     //orderby i.InvoiceId descending
                                                     group ii by ii.Item.ItemName into ItemsG
                                                     select new ItemWisePurchaseVM
                                                     {
                                                         ItemName = ItemsG.Key,
                                                         PurchaseCount = ItemsG.Sum(i => i.Quantity),
                                                     }).OrderBy(i => i.ItemName).ToList();
            return PurchaseList;
        }

        public List<ItemWiseSaleVM> GetItemWiseRevenue_Rpt(int ItemId)
        {
            List<ItemWiseSaleVM> SalesList = (from i in cxt.Invoices
                                              join ii in cxt.InvoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                                              where i.IsActive && ii.IsActive && ii.Item.ItemId == ItemId
                                              group ii by ii.Item.ItemName into ItemsG
                                              select new ItemWiseSaleVM
                                              {
                                                  ItemName = ItemsG.Key,
                                                  SaleRevenue = ItemsG.Sum(i => (double?)i.Amount) ?? 0,
                                              }).OrderBy(i => i.ItemName).ToList();
            return SalesList;
        }
        public List<ItemWiseSaleVM> GetItemWiseRevenue_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            List<ItemWiseSaleVM> SalesList = (from i in cxt.Invoices
                                              join ii in cxt.InvoiceItems on i.InvoiceId equals ii.Invoice.InvoiceId
                                              where i.IsActive && ii.IsActive
                                              && (i.CreatedAt) >= (DateFrom)
                                              && (i.CreatedAt) <= (DateTo)
                                              //orderby i.InvoiceId descending
                                              group ii by ii.Item.ItemName into ItemsG
                                              select new ItemWiseSaleVM
                                              {
                                                  ItemName = ItemsG.Key,
                                                  SaleRevenue = ItemsG.Sum(i => (double?)i.Amount) ?? 0,
                                              }).OrderBy(i => i.ItemName).ToList();
            return SalesList;
        }

        public IPagedList<LowStockVM> GetLowStockItems(int PageNo, int PageSize)
        {
            return (from i in cxt.Items
                    where i.IsActive && !i.IsDefault
                    select new LowStockVM()
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        ReOrderingLevel = i.ReOrderingLevel,
                        TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId select ts).Sum(ts => (double?)ts.Quantity) ?? 0,
                        ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                        ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0,
                        AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
                        LastStockAddedDate = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId orderby ts.StockItemId descending select ts.CreatedAt).FirstOrDefault()
                    })
              .Where(r => r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock <= r.ReOrderingLevel)
              .OrderByDescending(r => r.ItemId).ToPagedList(PageNo, PageSize);
        }
        public IPagedList<LowStockVM> GetLowStockItems_BySupplier(long SupplierId, int PageNo, int PageSize)
        {
            return (from i in cxt.Items
                    where i.IsActive && i.Suppliers.Any(s => s.SupplierID == SupplierId)
                    select new LowStockVM()
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        ReOrderingLevel = i.ReOrderingLevel,
                        TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId select ts).Sum(ts => (double?)ts.Quantity) ?? 0,
                        ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                        ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0,
                        AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
                        LastStockAddedDate = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId orderby ts.StockItemId descending select ts.CreatedAt).FirstOrDefault()
                    })
                    .Where(r => r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock <= r.ReOrderingLevel)
                    .OrderByDescending(r => r.ItemId).ToPagedList(PageNo, PageSize);
        }
        public List<LowStockVM> GetLowStockItems_Rpt()
        {
            return (from i in cxt.Items
                    where i.IsActive && !i.IsDefault
                    select new LowStockVM()
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        ReOrderingLevel = i.ReOrderingLevel,
                        TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId select ts).Sum(ts => (double?)ts.Quantity) ?? 0,
                        ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                        ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0,
                        AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
                        LastStockAddedDate = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId orderby ts.StockItemId descending select ts.CreatedAt).FirstOrDefault()
                    })
                    .Where(r => r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock <= r.ReOrderingLevel)
                    .OrderByDescending(r => r.ItemId).ToList();
        }
        public List<LowStockVM> GetLowStockItems_BySupplier_Rpt(long SupplierId)
        {
            return (from i in cxt.Items
                    where i.IsActive && !i.IsDefault && i.Suppliers.Any(s => s.SupplierID == SupplierId)
                    select new LowStockVM()
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        ReOrderingLevel = i.ReOrderingLevel,
                        TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId select ts).Sum(ts => (double?)ts.Quantity) ?? 0,
                        ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                        ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0,
                        AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
                        LastStockAddedDate = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId orderby ts.StockItemId descending select ts.CreatedAt).FirstOrDefault()
                    })
                    .Where(r => r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock <= r.ReOrderingLevel)
                    .OrderByDescending(r => r.ItemId).ToList();
        }
        public void SetItemDataInactive(int ItemId)
        {
            //            string qry = @"Update Items set IsActive = 0, IsUpdate = 1, IsSynced = 0, UpdatedAt = @UpdatedAt where ItemId = @ItemId 
            //                            Update StockItems Set IsActive = 0, IsUpdate = 1, IsSynced = 0, UpdatedAt = @UpdatedAt where ItemId = @ItemId 
            //                            Update AdjustmentItems Set IsActive = 0, IsUpdate = 1, IsSynced = 0, UpdatedAt = @UpdatedAt where ItemId = @ItemId";

            string qry = @"Update Items set IsActive = 0, IsSynced = 0  where ItemId = @ItemId 
                             Update StockItems Set IsActive = 0, IsUpdate = 1, IsSynced = 0 where ItemId = @ItemId 
                             Update AdjustmentItems Set IsActive = 0, IsUpdate = 1, IsSynced = 0 where ItemId = @ItemId";

            cxt.Database.ExecuteSqlRaw(qry, new SqlParameter("@ItemId", ItemId));
        }

        public IPagedList<DeadStockVM> GetDeadStock(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            return cxt.Items
                .Where(i => i.IsActive && !i.IsDefault)
                .Select(i => new DeadStockVM
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    AvailableQuantity = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == i.ItemId).Sum(si => (double?)si.Quantity + si.BonusQuantity) ?? 0)
                                        - (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == i.ItemId select s).Sum(s => (double?)s.Quantity) ?? 0

                                        //total expired stock expired stock - expired stock consumption
                                        - (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == i.ItemId && b.Expiry != null && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0
                                        - (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == i.ItemId && b.Expiry != null && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0

                                        + (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == i.ItemId select ai).Sum(ai => (double?)ai.Quantity) ?? 0,
                    LastSalesAt = (from c in cxt.StockConsumptionItems where c.IsActive && c.Item.ItemId == i.ItemId && c.Quantity > 0 && (c.CreatedAt) >= (DateFrom) && (c.CreatedAt) <= (DateTo) orderby c.StockConsumptionItemId descending select c.CreatedAt == null ? "" : c.CreatedAt.ToString()).FirstOrDefault()
                }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);

            // commented now
            //int[] LastMonthSaleItemIds = cxt.invoiceItems
            //                             .Where(i => i.IsActive
            //                                 && (i.CreatedAt) >= (DateFrom)
            //                                 && (i.CreatedAt) <= (DateTo)
            //                                 ).Select(i => i.Item.ItemId).ToArray();

            // get * from items id not in sold items

            // commented now
            //List<Item> DeadStockItems = cxt.Items.Where(r => !r.IsDefault && !LastMonthSaleItemIds.Contains(r.ItemId)).Select(x => x).ToList();

            //LastMonthSaleItemIds.Contains(cxt.Items.Select(i=>i.ItemId))
            //return DeadStockItems
            //    .Where(i => i.IsActive)
            //    .Select(i => new DeadStockVM
            //    {
            //        ItemId = i.ItemId,
            //        ItemName = i.ItemName,
            //        AvailableQuantity = ((cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == i.ItemId).Sum(si => (int?)si.Quantity) ?? 0)
            //                            - (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == i.ItemId select s).Sum(s => (int?)s.Quantity) ?? 0
            //                            - (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == i.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0
            //                            - (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == i.ItemId select ai).Sum(ai => (int?)ai.Quantity) ?? 0),
            //        LastSalesAt = (from inv in cxt.invoiceItems where inv.IsActive && inv.Item.ItemId == i.ItemId && inv.Quantity > 0 && (inv.CreatedAt) < (DateFrom) orderby inv.CreatedAt descending select inv.CreatedAt == null ?  "" : inv.CreatedAt.ToString()).FirstOrDefault()
            //    }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);

            //IPagedList<DeadStockVM> Result = DeadStockItems.Skip((PageNo - 1) * PageSize).Take(PageNo * PageSize)
            //    .Where(i => i.IsActive)
            //    .Select(i => new DeadStockVM
            //    {
            //        ItemId = i.ItemId,
            //        ItemName = i.ItemName,
            //        TotalStock = (cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == i.ItemId).Sum(si => (int?)si.Quantity) ?? 0),
            //        ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == i.ItemId select s).Sum(s => (int?)s.Quantity) ?? 0,
            //        ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == i.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0,
            //        AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == i.ItemId select ai).Sum(ai => (int?)ai.Quantity) ?? 0,
            //        LastSalesAt = (from inv in cxt.invoiceItems where inv.IsActive && inv.Item.ItemId == i.ItemId && inv.Quantity > 0 && (inv.CreatedAt) < (DateFrom) orderby inv.CreatedAt descending select inv.CreatedAt == null ? "" : inv.CreatedAt.ToString()).FirstOrDefault()
            //    }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);
            //int PageCount = Convert.ToInt32(Math.Ceiling((double)(DeadStockItems.Count / PageSize)));
            //foreach (DeadStockVM v in Result)
            //{
            //    v.AvailableQuantity = (int)v.TotalStock - (int)v.ExpiredStock + (int)v.AdjustedStock + (int)v.ConsumedStock;
            //}
            //return new Tuple<IPagedList<DeadStockVM>, int>(Result, PageCount);
        }
        public List<DeadStockVM> GetDeadStock_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            //return cxt.Items
            //    .Where(i => i.IsActive)
            //    .Select(i => new DeadStockVM
            //    {
            //        ItemId = i.ItemId,
            //        ItemName = i.ItemName,
            //        AvailableQuantity = ((cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == i.ItemId).Sum(si => (int?)si.Quantity) ?? 0)
            //                            - (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == i.ItemId select s).Sum(s => (int?)s.Quantity) ?? 0
            //                            - (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == i.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0
            //                            + (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == i.ItemId select ai).Sum(ai => (int?)ai.Quantity) ?? 0),
            //        LastSalesAt = (from inv in cxt.invoiceItems where inv.IsActive && inv.Item.ItemId == i.ItemId && inv.Quantity > 0 && (inv.CreatedAt) < (DateFrom) orderby inv.CreatedAt descending select inv.CreatedAt == null ? "" : inv.CreatedAt.ToString()).FirstOrDefault()
            //    }).OrderByDescending(i => i.ItemId).ToList();
            long[] LastMonthSaleItemIds = cxt.InvoiceItems
                                         .Where(i => i.IsActive
                                             && (i.CreatedAt) >= (DateFrom)
                                             && (i.CreatedAt) <= (DateTo)
                                             ).Select(i => i.Item.ItemId).ToArray();

            // get * from items id not in sold items
            List<Item> DeadStockItems = cxt.Items.Where(r => !r.IsDefault && !LastMonthSaleItemIds.Contains(r.ItemId)).Select(x => x).ToList();
            //LastMonthSaleItemIds.Contains(cxt.Items.Select(i=>i.ItemId))
            //return DeadStockItems
            //    .Where(i => i.IsActive)
            //    .Select(i => new DeadStockVM
            //    {
            //        ItemId = i.ItemId,
            //        ItemName = i.ItemName,
            //        AvailableQuantity = ((cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == i.ItemId).Sum(si => (int?)si.Quantity) ?? 0)
            //                            - (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == i.ItemId select s).Sum(s => (int?)s.Quantity) ?? 0
            //                            - (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == i.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (int?)s.Quantity) ?? 0
            //                            - (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == i.ItemId select ai).Sum(ai => (int?)ai.Quantity) ?? 0),
            //        LastSalesAt = (from inv in cxt.invoiceItems where inv.IsActive && inv.Item.ItemId == i.ItemId && inv.Quantity > 0 && (inv.CreatedAt) < (DateFrom) orderby inv.CreatedAt descending select inv.CreatedAt == null ?  "" : inv.CreatedAt.ToString()).FirstOrDefault()
            //    }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);

            List<DeadStockVM> Result = DeadStockItems
                .Where(i => i.IsActive)
                .Select(i => new DeadStockVM
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == i.ItemId).Sum(si => (double?)si.Quantity) ?? 0,
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == i.ItemId select s).Sum(s => (double?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == i.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == i.ItemId select ai).Sum(ai => (double?)ai.Quantity) ?? 0,
                    LastSalesAt = (from inv in cxt.InvoiceItems where inv.IsActive && inv.Item.ItemId == i.ItemId && inv.Quantity > 0 && (inv.CreatedAt) < (DateFrom) orderby inv.CreatedAt descending select inv.CreatedAt == null ? "" : inv.CreatedAt.ToString()).FirstOrDefault()
                }).OrderByDescending(i => i.ItemId).ToList();

            foreach (DeadStockVM v in Result)
            {
                v.AvailableQuantity = v.TotalStock - v.ExpiredStock + v.AdjustedStock + v.ConsumedStock;
            }
            return Result;
        }

        public IPagedList<ExpiryVM> GetItemsExpiry(DateTime DateFrom, DateTime DateTo, int PageNo, int PageSize)
        {
            IPagedList<ExpiryVM> ExpiredItems = (
             from s in cxt.StockItems
             join b in cxt.Batches on s.Batch equals b
             where s.IsActive
             && (b.Expiry) >= (DateFrom)
             && (b.Expiry) <= (DateTo)
             group s by
             new
             {
                 s.Item,
                 s.Batch
             } into g
             select new ExpiryVM
             {
                 ItemId = g.Key.Item.ItemId,
                 ItemName = g.Key.Item.ItemName,
                 BatchName = g.Key.Batch.BatchName,
                 Quantity = g.Sum(r => r.Quantity),
                 ExpiryDate = (DateTime)g.Key.Batch.Expiry,
                 StockAdditionDate = g.Where(r => r.Item == g.Key.Item && r.Batch == g.Key.Batch).Select(r => r.CreatedAt).FirstOrDefault(),
                 TotalCost = g.Sum(r => r.TotalCost),
                 TotalRetailValue = g.Sum(r => r.Quantity * r.RetailPrice)
             }).OrderByDescending(r => r.ItemId).ToPagedList(PageNo, PageSize);
            return ExpiredItems;
        }
        public List<ExpiryVM> GetItemsExpiry_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            List<ExpiryVM> ExpiredItems = (
             from s in cxt.StockItems
             join b in cxt.Batches on s.Batch equals b
             where s.IsActive
             && (b.Expiry) >= (DateFrom)
             && (b.Expiry) <= (DateTo)
             group s by
             new
             {
                 s.Item,
                 s.Batch
             } into g
             select new ExpiryVM
             {
                 ItemId = g.Key.Item.ItemId,
                 ItemName = g.Key.Item.ItemName,
                 BatchName = g.Key.Batch.BatchName,
                 Quantity = g.Sum(r => r.Quantity),
                 ExpiryDate = (DateTime)g.Key.Batch.Expiry,
                 StockAdditionDate = g.Where(r => r.Item == g.Key.Item && r.Batch == g.Key.Batch).Select(r => r.CreatedAt).FirstOrDefault(),
                 TotalCost = g.Sum(r => r.TotalCost),
                 TotalRetailValue = g.Sum(r => r.Quantity * r.RetailPrice)
             }).OrderByDescending(r => r.ItemId).ToList();
            return ExpiredItems;
        }

        public long ItemsCount()
        {
            return cxt.Items.Count();
        }

        //public void updatePrices(string qry)
        //{
        //    cxt.Database.CommandTimeout = 2000;
        //    cxt.Database.ExecuteSqlCommand(qry);
        //}

        public int GetLowStockItemsCount()
        {
            return (from i in cxt.Items
                    where i.IsActive
                    select new LowStockVM()
                    {
                        ItemId = i.ItemId,
                        //ItemName = i.ItemName,
                        ReOrderingLevel = i.ReOrderingLevel,
                        TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId select ts).Sum(ts => (double?)ts.Quantity) ?? 0,
                        ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                        ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && b_.Expiry != null && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0,
                        AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
                        //LastStockAddedDate = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId orderby ts.StockItemId descending select ts.CreatedAt).FirstOrDefault()
                    })
              .Where(r => r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock < r.ReOrderingLevel)
              .Count();
        }

        public int GetOutOfStockItemsCount()
        {
            //List<LowStockVM> v = 
            return (from i in cxt.Items
                    where i.IsActive && !i.IsDefault
                    select new LowStockVM()
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        ReOrderingLevel = i.ReOrderingLevel,
                        TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId select ts).Sum(ts => (double?)ts.Quantity) ?? 0,
                        ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                        ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && b_.Expiry != null && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0,
                        AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
                        //LastStockAddedDate = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId orderby ts.StockItemId descending select ts.CreatedAt).FirstOrDefault()
                    })
                            .Where(r => r.TotalStock - r.ConsumedStock - r.ExpiredStock + r.AdjustedStock <= 0)
                            .Count();
        }

        public int GetExpiredItemsCount(DateTime? date)
        {
            double? ExpiredStock = 0;
            if (date != null)
            {
                ExpiredStock = (from es in cxt.StockItems
                                join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                join i in cxt.Items on es.Item.ItemId equals i.ItemId
                                where i.IsActive && es.IsActive && b_.Expiry != null && (b_.Expiry) < (DateTime.Now)
                                select es).Sum(es => es.Quantity);
            }
            else
            {
                ExpiredStock = (from es in cxt.StockItems
                                join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId
                                join i in cxt.Items on es.Item.ItemId equals i.ItemId
                                where i.IsActive && es.IsActive && b_.Expiry != null && (b_.Expiry) < (DateTime.Now)
                                select es).Sum(es => (double?)es.Quantity) ?? 0;
            }
            return (int)ExpiredStock;
        }

        public IPagedList<ItemsEditVM> GetItemsForBulkEdit(int PageNo, int PageSize)
        {
            IPagedList<ItemsEditVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Select(vm => new ItemsEditVM
               {
                   ItemId = vm.ItemId,
                   ItemName = vm.ItemName,
                   Rack = vm.Rack,
                   UnitCost = vm.UnitCostPrice,
                   RetailPrice = vm.RetailPrice,
                   ReorderingLevel = vm.ReOrderingLevel,
                   Barcode = vm.Barcode,
                   CategoryId = (int?)vm.Category.CategoryId ?? 0,
                   CategoryName = vm.Category.Name,
                   Suppliers = vm.Suppliers.Where(s => s.IsActive).Select(sup => new SupplierInfoVM { Name = sup.Name }).ToList()
               }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);
            return r;
        }

        public IPagedList<ItemsEditVM> GetItemsForBulkEdit(string filter, int PageNo, int PageSize)
        {
            IPagedList<ItemsEditVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Where(i => i.ItemName.ToLower().StartsWith(filter))
               .Select(vm => new ItemsEditVM
               {
                   ItemId = vm.ItemId,
                   ItemName = vm.ItemName,
                   Rack = vm.Rack,
                   UnitCost = vm.UnitCostPrice,
                   RetailPrice = vm.RetailPrice,
                   ReorderingLevel = vm.ReOrderingLevel,
                   Barcode = vm.Barcode,
                   CategoryId = vm.Category != null ? (int)vm.Category.CategoryId : 0,
                   CategoryName = vm.Category != null ? vm.Category.Name : "",
                   Suppliers = vm.Suppliers.Where(s => s.IsActive).Select(sup => new SupplierInfoVM { Name = sup.Name }).ToList()
               }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);
            return r;
        }
        public IPagedList<ItemsEditVM> GetItemsForBulkEdit_Sup(int SupplierId, int PageNo, int PageSize)
        {

            try
            {
                var res = cxt.Suppliers
                       .Where(s => s.SupplierID == SupplierId).FirstOrDefault();

                IPagedList<ItemsEditVM> r = cxt.Entry(res)
                    .Collection(rs => rs.Items)
                    .Query()
                    .Where(i => i.IsActive)
                    .Select(vm => new ItemsEditVM
                    {
                        ItemId = vm.ItemId,
                        ItemName = vm.ItemName,
                        Rack = vm.Rack,
                        UnitCost = vm.UnitCostPrice,
                        RetailPrice = vm.RetailPrice,
                        ReorderingLevel = vm.ReOrderingLevel,
                        Barcode = vm.Barcode,
                        CategoryId = vm.Category != null ? (int)vm.Category.CategoryId : 0,
                        CategoryName = vm.Category != null ? vm.Category.Name : "",
                        Suppliers = vm.Suppliers.Where(s => s.IsActive).Select(sup => new SupplierInfoVM { Name = sup.Name }).ToList()
                    })
                    .OrderByDescending(i => i.ItemId)
                    .ToPagedList(PageNo, PageSize);
                return r;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public IPagedList<ItemsEditVM> GetItemsForBulkEdit_Cat(int CategoryId, int PageNo, int PageSize)
        {
            IPagedList<ItemsEditVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Where(i => i.Category.CategoryId == CategoryId)
               .Select(vm => new ItemsEditVM
               {
                   ItemId = vm.ItemId,
                   ItemName = vm.ItemName,
                   Rack = vm.Rack,
                   UnitCost = vm.UnitCostPrice,
                   RetailPrice = vm.RetailPrice,
                   ReorderingLevel = vm.ReOrderingLevel,
                   Barcode = vm.Barcode,
                   CategoryId = (int)vm.Category.CategoryId,
                   CategoryName = vm.Category.Name,
                   Suppliers = vm.Suppliers.Where(s => s.IsActive).Select(sup => new SupplierInfoVM { Name = sup.Name }).ToList()
               }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);
            return r;
        }
        public IPagedList<ItemsEditVM> GetItemsForBulkEdit_Manuf(int ManufId, int PageNo, int PageSize)
        {
            IPagedList<ItemsEditVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Where(i => i.Manufacturer.ManufacturerId == ManufId)
               .Select(vm => new ItemsEditVM
               {
                   ItemId = vm.ItemId,
                   ItemName = vm.ItemName,
                   Rack = vm.Rack,
                   UnitCost = vm.UnitCostPrice,
                   RetailPrice = vm.RetailPrice,
                   ReorderingLevel = vm.ReOrderingLevel,
                   Barcode = vm.Barcode,
                   CategoryId = vm.Category != null ? (int)vm.Category.CategoryId : 0,
                   CategoryName = vm.Category != null ? vm.Category.Name : "",
                   Suppliers = vm.Suppliers.Where(s => s.IsActive).Select(sup => new SupplierInfoVM { Name = sup.Name }).ToList()
               }).OrderByDescending(i => i.ItemId).ToPagedList(PageNo, PageSize);
            return r;
        }
        public List<Item> getItemsByIds(List<long> ids)
        {
            return cxt.Items.Where(i => i.IsActive && ids.Contains(i.ItemId)).ToList();
        }

        public ItemsDataVM getItemData(int ItemId)
        {
            return cxt.Items.Where(i => i.ItemId == ItemId)
                .Select(i => new ItemsDataVM()
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    RetailPrice = i.RetailPrice
                }).FirstOrDefault();
        }
        public bool ExistsAny()
        {
            return cxt.Items.Any(i => !i.IsDefault);
        }

        // profit margin UI data
        public IPagedList<ProfitMarginVM> GetProfitMargin(int pageNo = 1, int pageSize = 1)
        {
            return cxt.Items
                .Where(i => i.IsActive && !i.IsDefault)
                .Select(m => new ProfitMarginVM
                {
                    ItemId = m.ItemId,
                    ItemName = m.ItemName,
                    CategoryName = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<ProfitMarginVM> GetProfitMargin_Item(int ItemId, int pageNo = 1, int pageSize = 1)
        {
            return cxt.Items
                .Where(i => i.IsActive)
                .Where(i => i.ItemId == ItemId)
                .Select(m => new ProfitMarginVM
                {
                    ItemId = m.ItemId,
                    ItemName = m.ItemName,
                    CategoryName = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<ProfitMarginVM> GetProfitMargin_cat(int categoryId, int pageNo = 1, int pageSize = 1)
        {
            return cxt.Items
                .Where(i => i.IsActive)
                .Where(i => i.Category.CategoryId == categoryId)
                .Select(m => new ProfitMarginVM
                {
                    ItemId = m.ItemId,
                    ItemName = m.ItemName,
                    CategoryName = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<ProfitMarginVM> GetProfitMargin(DateTime DateFrom, DateTime DateTo, int pageNo = 1, int pageSize = 1)
        {
            return cxt.Items
                .Where(i => i.IsActive && !i.IsDefault)
                .Where(i => (i.CreatedAt) >= (DateFrom) && (i.CreatedAt) <= (DateTo))
                .Select(m => new ProfitMarginVM
                {
                    ItemId = m.ItemId,
                    ItemName = m.ItemName,
                    CategoryName = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }

        // profit margin Report data
        public List<ProfitMarginVM> GetProfitMargin_Rpt()
        {
            return cxt.Items
                .Where(i => i.IsActive && !i.IsDefault)
                .Select(m => new ProfitMarginVM
                {
                    ItemId = m.ItemId,
                    ItemName = m.ItemName,
                    CategoryName = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<ProfitMarginVM> GetProfitMargin_Item_Rpt(int ItemId)
        {
            return cxt.Items
                .Where(i => i.IsActive)
                .Where(i => i.ItemId == ItemId)
                .Select(m => new ProfitMarginVM
                {
                    ItemId = m.ItemId,
                    ItemName = m.ItemName,
                    CategoryName = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<ProfitMarginVM> GetProfitMargin_cat_Rpt(int categoryId)
        {
            return cxt.Items
                .Where(i => i.IsActive)
                .Where(i => i.Category.CategoryId == categoryId)
                .Select(m => new ProfitMarginVM
                {
                    ItemId = m.ItemId,
                    ItemName = m.ItemName,
                    CategoryName = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<ProfitMarginVM> GetProfitMargin_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            return cxt.Items
                .Where(i => i.IsActive && !i.IsDefault)
                .Where(i => (i.CreatedAt) >= (DateFrom) && (i.CreatedAt) <= (DateTo))
                .Select(m => new ProfitMarginVM
                {
                    ItemId = m.ItemId,
                    ItemName = m.ItemName,
                    CategoryName = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).ToList();
        }


        // Discounted Items (selected items)
        public IPagedList<DiscountedItemVM> GetDiscountedItems(int DiscountId, int pageNo = 1, int pageSize = 1)
        {
            return (from d in cxt.FlatDiscounts
                    join di in cxt.DiscountItems on d.FlatDiscountId equals di.FlatDiscountId
                    join i in cxt.Items on di.Item.ItemId equals i.ItemId
                    where i.IsActive && di.IsActive && d.Discount > 0 && d.FlatDiscountId == DiscountId
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        CategoryName = i.Category.Name,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountName = d.Name,
                        DiscountType = d.DiscountType,
                        Discount = d.Discount
                    }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<DiscountedItemVM> GetDiscountedItems_Item(int DiscountId, int ItemId, int pageNo = 1, int pageSize = 1)
        {
            return (from d in cxt.FlatDiscounts
                    join di in cxt.DiscountItems on d.FlatDiscountId equals di.FlatDiscountId
                    join i in cxt.Items on di.Item.ItemId equals i.ItemId
                    where i.IsActive && di.IsActive && d.Discount > 0 && i.ItemId == ItemId && d.FlatDiscountId == DiscountId
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        CategoryName = i.Category.Name,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountName = d.Name,
                        DiscountType = d.DiscountType,
                        Discount = d.Discount
                    }
                 ).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<DiscountedItemVM> GetDiscountedItems_cat(int DiscountId, int categoryId, int pageNo = 1, int pageSize = 1)
        {
            return (from d in cxt.FlatDiscounts
                    join di in cxt.DiscountItems on d.FlatDiscountId equals di.FlatDiscountId
                    join i in cxt.Items on di.Item.ItemId equals i.ItemId
                    where i.IsActive && di.IsActive && d.Discount > 0 && i.Category.CategoryId == categoryId && d.FlatDiscountId == DiscountId
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        CategoryName = i.Category.Name,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountName = d.Name,
                        DiscountType = d.DiscountType,
                        Discount = d.Discount
                    }
                 ).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<DiscountedItemVM> GetDiscountedItems(DateTime DateFrom, DateTime DateTo, int pageNo = 1, int pageSize = 1)
        {
            return
                (from d in cxt.FlatDiscounts
                 join di in cxt.DiscountItems on d.FlatDiscountId equals di.FlatDiscountId
                 join i in cxt.Items on di.Item.ItemId equals i.ItemId
                 where i.IsActive && di.IsActive && d.Discount > 0 && !d.IsAllItems
                 && (d.StartDate) >= (DateFrom)
                 && (d.EndDate) <= (DateTo)

                 select new DiscountedItemVM
                 {
                     ItemId = i.ItemId,
                     ItemName = i.ItemName,
                     CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                     RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                     DiscountName = d.Name,
                     DiscountType = d.DiscountType,
                     Discount = d.Discount
                 }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }


        // Discounted Items report data (selected Items)
        public List<DiscountedItemVM> GetDiscountedItems_Rpt(int DiscountId)
        {
            return (from d in cxt.FlatDiscounts
                    join di in cxt.DiscountItems on d.FlatDiscountId equals di.FlatDiscountId
                    join i in cxt.Items on di.Item.ItemId equals i.ItemId
                    where i.IsActive && di.IsActive && d.Discount > 0 && d.FlatDiscountId == DiscountId
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountName = d.Name,
                        DiscountType = d.DiscountType,
                        Discount = d.Discount
                    }
                 ).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<DiscountedItemVM> GetDiscountedItems_Item_Rpt(int DiscountId, int ItemId)
        {
            return (from d in cxt.FlatDiscounts
                    join di in cxt.DiscountItems on d.FlatDiscountId equals di.FlatDiscountId
                    join i in cxt.Items on di.Item.ItemId equals i.ItemId
                    where i.IsActive && di.IsActive && d.Discount > 0 && i.ItemId == ItemId && d.FlatDiscountId == DiscountId
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountName = d.Name,
                        DiscountType = d.DiscountType,
                        Discount = d.Discount
                    }
                 ).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<DiscountedItemVM> GetDiscountedItems_cat_Rpt(int DiscountId, int categoryId)
        {
            return (from d in cxt.FlatDiscounts
                    join di in cxt.DiscountItems on d.FlatDiscountId equals di.FlatDiscountId
                    join i in cxt.Items on di.Item.ItemId equals i.ItemId
                    where i.IsActive && di.IsActive && d.Discount > 0 && i.Category.CategoryId == categoryId && d.FlatDiscountId == DiscountId
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountName = d.Name,
                        DiscountType = d.DiscountType,
                        Discount = d.Discount
                    }
                 ).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<DiscountedItemVM> GetDiscountedItems_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            return (from d in cxt.FlatDiscounts
                    join di in cxt.DiscountItems on d.FlatDiscountId equals di.FlatDiscountId
                    join i in cxt.Items on di.Item.ItemId equals i.ItemId
                    where i.IsActive && di.IsActive && d.Discount > 0
                    && (i.CreatedAt) >= (DateFrom)
                    && (i.CreatedAt) <= (DateTo)

                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountName = d.Name,
                        DiscountType = d.DiscountType,
                        Discount = d.Discount
                    }
                 ).OrderByDescending(r => r.ItemId).ToList();
        }


        // Discounted Items (All items)
        public IPagedList<DiscountedItemVM> GetDiscountedItems_all(int FaltDiscId, int pageNo = 1, int pageSize = 1)
        {
            return (from i in cxt.Items
                    where i.IsActive
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        CategoryName = i.Category.Name,
                        //CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        //RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountData = (from d in cxt.FlatDiscounts
                                        where d.FlatDiscountId == FaltDiscId
                                        select new FlatDiscountVM
                                        {
                                            FlatDiscountId = d.FlatDiscountId,
                                            Name = d.Name,
                                            Code = d.Code,
                                            Discount = d.Discount,
                                            DiscountType = d.DiscountType
                                        }).FirstOrDefault()
                    }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<DiscountedItemVM> GetDiscountedItems_Item_all(int FaltDiscId, int ItemId, int pageNo = 1, int pageSize = 1)
        {
            return (from i in cxt.Items
                    where i.IsActive && i.ItemId == ItemId
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        CategoryName = i.Category.Name,
                        //CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        //RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountData = (from d in cxt.FlatDiscounts
                                        where d.FlatDiscountId == FaltDiscId
                                        select new FlatDiscountVM
                                        {
                                            FlatDiscountId = d.FlatDiscountId,
                                            Name = d.Name,
                                            Code = d.Code,
                                            Discount = d.Discount,
                                            DiscountType = d.DiscountType
                                        }).FirstOrDefault()
                    }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<DiscountedItemVM> GetDiscountedItems_cat_all(int FaltDiscId, int categoryId, int pageNo = 1, int pageSize = 1)
        {
            return (from i in cxt.Items
                    where i.IsActive && i.Category.CategoryId == categoryId
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        CategoryName = i.Category.Name,
                        //CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        //RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountData = (from d in cxt.FlatDiscounts
                                        where d.FlatDiscountId == FaltDiscId
                                        select new FlatDiscountVM
                                        {
                                            FlatDiscountId = d.FlatDiscountId,
                                            Name = d.Name,
                                            Code = d.Code,
                                            Discount = d.Discount,
                                            DiscountType = d.DiscountType
                                        }).FirstOrDefault()
                    }).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }

        // Discounted Items report data (All Items)
        public List<DiscountedItemVM> GetDiscountedItems_Rpt_all(int FaltDiscId)
        {
            return (from i in cxt.Items
                    where i.IsActive
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountData = (from d in cxt.FlatDiscounts
                                        where d.FlatDiscountId == FaltDiscId
                                        select new FlatDiscountVM
                                        {
                                            FlatDiscountId = d.FlatDiscountId,
                                            Name = d.Name,
                                            Code = d.Code,
                                            Discount = d.Discount,
                                            DiscountType = d.DiscountType
                                        }).FirstOrDefault()
                    }).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<DiscountedItemVM> GetDiscountedItems_Item_Rpt_all(int FaltDiscId, int ItemId)
        {

            return (from i in cxt.Items
                    where i.IsActive && i.ItemId == ItemId
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountData = (from d in cxt.FlatDiscounts
                                        where d.FlatDiscountId == FaltDiscId
                                        select new FlatDiscountVM
                                        {
                                            FlatDiscountId = d.FlatDiscountId,
                                            Name = d.Name,
                                            Code = d.Code,
                                            Discount = d.Discount,
                                            DiscountType = d.DiscountType
                                        }).FirstOrDefault(),

                    }).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<DiscountedItemVM> GetDiscountedItems_cat_Rpt_all(int FaltDiscId, int categoryId)
        {
            return (from i in cxt.Items
                    where i.IsActive && i.Category.CategoryId == categoryId
                    select new DiscountedItemVM
                    {
                        ItemId = i.ItemId,
                        ItemName = i.ItemName,
                        CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                        RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == i.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                        DiscountData = (from d in cxt.FlatDiscounts
                                        where d.FlatDiscountId == FaltDiscId
                                        select new FlatDiscountVM
                                        {
                                            FlatDiscountId = d.FlatDiscountId,
                                            Name = d.Name,
                                            Code = d.Code,
                                            Discount = d.Discount,
                                            DiscountType = d.DiscountType
                                        }).FirstOrDefault()
                    }).OrderByDescending(r => r.ItemId).ToList();
        }


        // Product Prices
        public IPagedList<ProductPriceVM> GetProductPrices(int pageNo = 1, int pageSize = 1)
        {
            return cxt.Items
                .Where(i => i.IsActive && !i.IsDefault)
                .Select(m => new ProductPriceVM
                {
                    ItemId = m.ItemId,
                    ItemName = m.ItemName,
                    Category = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<ProductPriceVM> GetProductPrices_Item(int ItemId, int pageNo = 1, int pageSize = 1)
        {
            return cxt.Items
                .Where(i => i.IsActive && i.ItemId == ItemId)
                .Select(m => new ProductPriceVM
                {
                    ItemId = m.ItemId,
                    ItemName = m.ItemName,
                    Category = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<ProductPriceVM> GetProductPrices_cat(int categoryId, int pageNo = 1, int pageSize = 1)
        {
            return cxt.Items
                .Where(i => i.IsActive && i.Category.CategoryId == categoryId)
                .Select(m => new ProductPriceVM
                {
                    ItemId = m.ItemId,
                    ItemName = m.ItemName,
                    Category = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }
        public IPagedList<ProductPriceVM> GetProductPrices(DateTime DateFrom, DateTime DateTo, int pageNo = 1, int pageSize = 1)
        {
            return cxt.Items
                .Where(i => i.IsActive && !i.IsDefault)
                .Where(i => (i.CreatedAt) >= (DateFrom)
                    && (i.CreatedAt) >= (DateFrom)
                )
                .Select(m => new ProductPriceVM
                {
                    ItemId = m.ItemId,
                    ItemName = m.ItemName,
                    Category = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
        }


        // Product prices report data
        public List<ProductPriceVM> GetProductPrices_Rpt()
        {
            return cxt.Items
                .Where(i => i.IsActive && !i.IsDefault)
                .Select(m => new ProductPriceVM
                {
                    ItemId = m.ItemId,
                    ItemName = m.ItemName,
                    Category = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<ProductPriceVM> GetProductPrices_Item_Rpt(int ItemId)
        {
            return cxt.Items
                .Where(i => i.IsActive && i.ItemId == ItemId)
                .Select(m => new ProductPriceVM
                {
                    ItemId = m.ItemId,
                    ItemName = m.ItemName,
                    Category = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<ProductPriceVM> GetProductPrices_cat_Rpt(int categoryId)
        {
            return cxt.Items
                .Where(i => i.IsActive && i.Category.CategoryId == categoryId)
                .Select(m => new ProductPriceVM
                {
                    ItemId = m.ItemId,
                    ItemName = m.ItemName,
                    Category = m.Category == null ? "" : m.Category.Name,
                    CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                    RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
                }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();
        }
        public List<ProductPriceVM> GetProductPrices_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            return cxt.Items
               .Where(i => i.IsActive && !i.IsDefault)
               .Where(i => (i.CreatedAt) >= (DateFrom)
                   && (i.CreatedAt) >= (DateFrom)
               )
               .Select(m => new ProductPriceVM
               {
                   ItemId = m.ItemId,
                   ItemName = m.ItemName,
                   Category = m.Category == null ? "" : m.Category.Name,
                   CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
                   RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
               }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();
        }



        // Product Expiry
        public IPagedList<ProductExpiryVM> GetProductExpiry(int pageNo = 1, int pageSize = 1)
        {
            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemId equals si.Item.ItemId
                       where i.IsActive && si.IsActive
                       select new ProductExpiryVM
                       {
                           ItemId = i.ItemId,
                           ItemName = i.ItemName,
                           BatchNo = si.Batch.BatchName,
                           BatchExp = si.Batch.Expiry == null ? "" : si.Batch.Expiry.Value.ToString(),
                           LastTrDate = (from ii in cxt.InvoiceItems where ii.Item.ItemId == i.ItemId && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreatedAt).FirstOrDefault()
                            == default(DateTime) ? "" :
                            (from ii in cxt.InvoiceItems where ii.Item.ItemId == i.ItemId && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreatedAt).FirstOrDefault().ToString(),
                           TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId && ts.Batch.BatchId == si.Batch.BatchId select ts).Sum(ts => (double?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                           ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId && cs.Batch.BatchId == si.Batch.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                           //HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                           ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && es.Batch.BatchId == si.Batch.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0
                                          -
                                         (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && es.Batch.BatchId == si.Batch.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0,

                           AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive && adj_item.Batch.BatchId == si.Batch.BatchId select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);

            foreach (var r in res.Items)
            {
                r.Quantity = r.TotalStock + r.AdjustedStock - r.ExpiredStock - r.ConsumedStock;
            }
            return res;
        }
        public IPagedList<ProductExpiryVM> GetProductExpiry_Item(int ItemId, int pageNo = 1, int pageSize = 1)
        {
            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemId equals si.Item.ItemId
                       where i.IsActive && si.IsActive && i.ItemId == ItemId
                       select new ProductExpiryVM
                       {
                           ItemId = i.ItemId,
                           ItemName = i.ItemName,
                           BatchNo = si.Batch.BatchName,
                           BatchExp = si.Batch.Expiry == null ? "" : si.Batch.Expiry.Value.ToString(),
                           LastTrDate = (from ii in cxt.InvoiceItems where ii.Item.ItemId == i.ItemId && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreatedAt).FirstOrDefault()
                            == default(DateTime) ? "" :
                            (from ii in cxt.InvoiceItems where ii.Item.ItemId == i.ItemId && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreatedAt).FirstOrDefault().ToString(),
                           TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId && ts.Batch.BatchId == si.Batch.BatchId select ts).Sum(ts => (double?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                           ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId && cs.Batch.BatchId == si.Batch.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                           //HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                           ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && es.Batch.BatchId == si.Batch.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0
                                          -
                                         (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && es.Batch.BatchId == si.Batch.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0,

                           AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive && adj_item.Batch.BatchId == si.Batch.BatchId select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);

            foreach (var r in res.Items)
            {
                r.Quantity = r.TotalStock + r.AdjustedStock - r.ExpiredStock - r.ConsumedStock;
            }
            return res;
        }
        public IPagedList<ProductExpiryVM> GetProductExpiry_cat(int categoryId, int pageNo = 1, int pageSize = 1)
        {
            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemId equals si.Item.ItemId
                       where i.IsActive && si.IsActive && i.Category.CategoryId == categoryId
                       select new ProductExpiryVM
                       {
                           ItemId = i.ItemId,
                           ItemName = i.ItemName,
                           BatchNo = si.Batch.BatchName,
                           BatchExp = si.Batch.Expiry == null ? "" : si.Batch.Expiry.Value.ToString(),
                           LastTrDate = (from ii in cxt.InvoiceItems where ii.Item.ItemId == i.ItemId && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreatedAt).FirstOrDefault()
                            == default(DateTime) ? "" :
                            (from ii in cxt.InvoiceItems where ii.Item.ItemId == i.ItemId && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreatedAt).FirstOrDefault().ToString(),
                           TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId && ts.Batch.BatchId == si.Batch.BatchId select ts).Sum(ts => (double?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                           ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId && cs.Batch.BatchId == si.Batch.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                           //HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                           ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && es.Batch.BatchId == si.Batch.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0
                                          -
                                         (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && es.Batch.BatchId == si.Batch.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0,

                           AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive && adj_item.Batch.BatchId == si.Batch.BatchId select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);

            foreach (var r in res.Items)
            {
                r.Quantity = r.TotalStock + r.AdjustedStock - r.ExpiredStock - r.ConsumedStock;
            }
            return res;
        }
        public IPagedList<ProductExpiryVM> GetProductExpiry(DateTime DateFrom, DateTime DateTo, int pageNo = 1, int pageSize = 1)
        {

            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemId equals si.Item.ItemId
                       where i.IsActive && si.IsActive &&
                        (i.CreatedAt) >= (DateFrom)
                        && (i.CreatedAt) >= (DateFrom)
                       select new ProductExpiryVM
                       {
                           ItemId = i.ItemId,
                           ItemName = i.ItemName,
                           BatchNo = si.Batch.BatchName,
                           BatchExp = si.Batch.Expiry == null ? "" : si.Batch.Expiry.Value.ToString(),
                           LastTrDate = (from ii in cxt.InvoiceItems where ii.Item.ItemId == i.ItemId && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreatedAt).FirstOrDefault()
                            == default(DateTime) ? "" :
                            (from ii in cxt.InvoiceItems where ii.Item.ItemId == i.ItemId && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreatedAt).FirstOrDefault().ToString(),
                           TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId && ts.Batch.BatchId == si.Batch.BatchId select ts).Sum(ts => (double?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                           ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId && cs.Batch.BatchId == si.Batch.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                           //HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                           ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && es.Batch.BatchId == si.Batch.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0
                                          -
                                         (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && es.Batch.BatchId == si.Batch.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0,

                           AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive && adj_item.Batch.BatchId == si.Batch.BatchId select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);

            foreach (var r in res.Items)
            {
                r.Quantity = r.TotalStock + r.AdjustedStock - r.ExpiredStock - r.ConsumedStock;
            }
            return res;
        }


        //// Product Expiry report data
        //public List<ProductExpiryVM> GetProductExpiry_Rpt()
        //{
        //    return cxt.Items
        //        .Where(i => i.IsActive && !i.IsDefault)
        //        .Select(m => new ProductPriceVM
        //        {
        //            ItemId = m.ItemId,
        //            ItemName = m.ItemName,
        //            Category = m.Category == null ? "" : m.Category.Name,
        //            CostPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.UnitCost).FirstOrDefault() ?? 0,
        //            RetailPrice = (double?)(from s in cxt.StockItems where s.IsActive && s.Item.ItemId == m.ItemId orderby s.StockItemId descending select s.RetailPrice).FirstOrDefault() ?? 0,
        //        }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();
        //}
        public List<ProductExpiryVM> GetProductExpiry_Item_Rpt(int ItemId)
        {
            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemId equals si.Item.ItemId
                       where i.IsActive && si.IsActive && i.ItemId == ItemId
                       select new ProductExpiryVM
                       {
                           ItemId = i.ItemId,
                           ItemName = i.ItemName,
                           BatchNo = si.Batch.BatchName,
                           BatchExp = si.Batch.Expiry == null ? "" : si.Batch.Expiry.Value.ToString(),
                           LastTrDate = (from ii in cxt.InvoiceItems where ii.Item.ItemId == i.ItemId && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreatedAt).FirstOrDefault()
                            == default(DateTime) ? "" :
                            (from ii in cxt.InvoiceItems where ii.Item.ItemId == i.ItemId && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreatedAt).FirstOrDefault().ToString(),
                           TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId && ts.Batch.BatchId == si.Batch.BatchId select ts).Sum(ts => (double?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                           ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId && cs.Batch.BatchId == si.Batch.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                           //HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                           ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && es.Batch.BatchId == si.Batch.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0
                                          -
                                         (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && es.Batch.BatchId == si.Batch.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0,

                           AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive && adj_item.Batch.BatchId == si.Batch.BatchId select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();

            foreach (var r in res)
            {
                r.Quantity = r.TotalStock + r.AdjustedStock - r.ExpiredStock - r.ConsumedStock;
            }
            return res;
        }
        public List<ProductExpiryVM> GetProductExpiry_cat_Rpt(int categoryId)
        {
            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemId equals si.Item.ItemId
                       where i.IsActive && si.IsActive && i.Category.CategoryId == categoryId
                       select new ProductExpiryVM
                       {
                           ItemId = i.ItemId,
                           ItemName = i.ItemName,
                           BatchNo = si.Batch.BatchName,
                           BatchExp = si.Batch.Expiry == null ? "" : si.Batch.Expiry.Value.ToString(),
                           LastTrDate = (from ii in cxt.InvoiceItems where ii.Item.ItemId == i.ItemId && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreatedAt).FirstOrDefault()
                            == default(DateTime) ? "" :
                            (from ii in cxt.InvoiceItems where ii.Item.ItemId == i.ItemId && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreatedAt).FirstOrDefault().ToString(),
                           TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId && ts.Batch.BatchId == si.Batch.BatchId select ts).Sum(ts => (double?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                           ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId && cs.Batch.BatchId == si.Batch.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                           //HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                           ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && es.Batch.BatchId == si.Batch.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0
                                          -
                                         (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && es.Batch.BatchId == si.Batch.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0,

                           AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive && adj_item.Batch.BatchId == si.Batch.BatchId select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();

            foreach (var r in res)
            {
                r.Quantity = r.TotalStock + r.AdjustedStock - r.ExpiredStock - r.ConsumedStock;
            }
            return res;
        }
        public List<ProductExpiryVM> GetProductExpiry_Rpt(DateTime DateFrom, DateTime DateTo)
        {
            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemId equals si.Item.ItemId
                       where i.IsActive && si.IsActive &&
                        (i.CreatedAt) >= (DateFrom)
                        && (i.CreatedAt) >= (DateFrom)
                       select new ProductExpiryVM
                       {
                           ItemId = i.ItemId,
                           ItemName = i.ItemName,
                           BatchNo = si.Batch.BatchName,
                           BatchExp = si.Batch.Expiry == null ? "" : si.Batch.Expiry.Value.ToString(),
                           LastTrDate = (from ii in cxt.InvoiceItems where ii.Item.ItemId == i.ItemId && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreatedAt).FirstOrDefault()
                            == default(DateTime) ? "" :
                            (from ii in cxt.InvoiceItems where ii.Item.ItemId == i.ItemId && ii.Batch.BatchId == si.Batch.BatchId orderby ii.InvoiceItemId descending select ii.CreatedAt).FirstOrDefault().ToString(),
                           TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId && ts.Batch.BatchId == si.Batch.BatchId select ts).Sum(ts => (double?)(ts.Quantity + ts.BonusQuantity)) ?? 0,
                           ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId && cs.Batch.BatchId == si.Batch.BatchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                           //HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == ItemId && cs.Batch.BatchId == batchId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
                           ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && es.Batch.BatchId == si.Batch.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0
                                          -
                                         (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && es.Batch.BatchId == si.Batch.BatchId && (b_.Expiry) < (DateTime.Now) select es).Sum(es => (double?)es.Quantity) ?? 0,

                           AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive && adj_item.Batch.BatchId == si.Batch.BatchId select adj_item).Sum(a => (double?)a.Quantity) ?? 0,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToList();

            foreach (var r in res)
            {
                r.Quantity = r.TotalStock + r.AdjustedStock - r.ExpiredStock - r.ConsumedStock;
            }
            return res;
        }

        public StockItemDetailRunTimeVM GetItemDetail(int ItemId)
        {
            return cxt.Items.Where(i => i.ItemId == ItemId)
                .Select(i => new StockItemDetailRunTimeVM
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    ConversionUnit = i.ConversionUnit,
                    Unit = i.Unit,
                    RetailPrice = (from si in cxt.StockItems where si.Item.ItemId == i.ItemId orderby si.StockItemId descending select si.RetailPrice).FirstOrDefault(),
                    CostPrice = (from si in cxt.StockItems where si.Item.ItemId == i.ItemId orderby si.StockItemId descending select si.UnitCost).FirstOrDefault(),
                    TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == i.ItemId).Sum(si => (double?)si.Quantity + si.BonusQuantity) ?? 0,
                    ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == i.ItemId select s).Sum(s => (double?)s.Quantity) ?? 0,
                    ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == i.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0
                                   -
                                   (from s in cxt.StockConsumptionItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == i.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0,
                    AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == i.ItemId select ai).Sum(ai => (double?)ai.Quantity) ?? 0,
                }).FirstOrDefault();
        }

        private void GetAllActiveItemsWithRetailPrice()
        {
            cxt.Items.Where(i => i.IsActive)
                .Select(i => new ItemsVM
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    RetailPrice = (double?)(from si in cxt.StockItems where si.IsActive && si.Item.ItemId == i.ItemId orderby si.Item.ItemId descending select si.RetailPrice).FirstOrDefault() ?? 0

                }).ToList();
        }

        public List<ItemsVM> LoadLowStockItems()
        {
            List<ItemsVM> res = cxt.Items
                .Where(i => i.IsActive && !i.IsDefault)
                .Select(i => new ItemsVM
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    ConversionUnit = i.ConversionUnit,
                    Unit = i.Unit,
                    ReorderingLevel = i.ReOrderingLevel,
                    UnitCost = i.UnitCostPrice,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (double?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == i.ItemId select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && (b_.Expiry) < (DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (double?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (double?)a.Qty) ?? 0
                })
                .OrderByDescending(r => r.ItemId)
                .ToList();

            foreach (ItemsVM i in res)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
                else
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
            }

            return res;
        }
        public List<ItemsVM> LoadSup_LowStockItems(int SupplierId)
        {
            var res =
                cxt.Suppliers
                .Where(s => s.SupplierID == SupplierId)
                .Select(s => s.Items.Where(si => si.IsActive).Select(i => new ItemsVM
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    ConversionUnit = i.ConversionUnit,
                    Unit = i.Unit,
                    ReorderingLevel = i.ReOrderingLevel,
                    UnitCost = i.UnitCostPrice,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (double?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == i.ItemId select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && (b_.Expiry) < (DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (double?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (double?)a.Qty) ?? 0
                }).ToList())
                .FirstOrDefault();

            foreach (ItemsVM i in res)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
                else
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
            }

            return res;
        }

        public List<ItemsVM> LoadSupItems(int SupplierId)
        {
            var res =
                cxt.Suppliers
                .Where(s => s.SupplierID == SupplierId)
                .Select(s => s.Items.Where(si => si.IsActive).Select(i => new ItemsVM
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    ConversionUnit = i.ConversionUnit,
                    Unit = i.Unit,
                    UnitCost = i.UnitCostPrice,
                    ReorderingLevel = i.ReOrderingLevel,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (double?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == i.ItemId select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && (b_.Expiry) < (DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (double?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (double?)a.Qty) ?? 0
                }).ToList())
                .FirstOrDefault();



            foreach (ItemsVM i in res)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
                else
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
            }

            return res;
        }
        public List<ItemsVM> LoadSupItems(int SupplierId, List<int> ItemsIds)
        {
            var res =
                cxt.Suppliers
                .Where(s => s.SupplierID == SupplierId)
                .Select(s => s.Items.Where(si => si.IsActive).Select(i => new ItemsVM
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    ConversionUnit = i.ConversionUnit,
                    Unit = i.Unit,
                    UnitCost = i.UnitCostPrice,
                    ReorderingLevel = i.ReOrderingLevel,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (double?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == i.ItemId select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && (b_.Expiry) < (DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (double?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (double?)a.Qty) ?? 0
                }).ToList())
                .FirstOrDefault();



            foreach (ItemsVM i in res)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
                else
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
            }

            return res;
        }

        public List<ItemsVM> GetMissedSalesItems(List<long> Ids)
        {
            List<ItemsVM> res = cxt.Items
                .Where(i => i.IsActive && !i.IsDefault && Ids.Contains(i.ItemId))
                .Select(i => new ItemsVM
                {
                    ItemId = i.ItemId,
                    ItemName = i.ItemName,
                    ConversionUnit = i.ConversionUnit,
                    Unit = i.Unit,
                    ReorderingLevel = i.ReOrderingLevel,
                    UnitCost = i.UnitCostPrice,
                    TotalStock = (from ts in cxt.StockItems where ts.IsActive && ts.Item.ItemId == i.ItemId select new { Qty = ts.Quantity, bQty = ts.BonusQuantity }).Sum(r => (double?)r.Qty + r.bQty) ?? 0,
                    ConsumedStock = (from cs in cxt.StockConsumptionItems where cs.IsActive && cs.Item.ItemId == i.ItemId select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                    HoldStock = (from cs in cxt.StockConsumptionItems join inv in cxt.Invoices on cs.InvoiceId equals inv.InvoiceId where inv.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == i.ItemId select new { Qty = cs.Quantity }).Sum(cs => (double?)cs.Qty) ?? 0,
                    ExpiredStock = (from es in cxt.StockItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && (b_.Expiry) < (DateTime.Now) select new { Quantity = es.Quantity + es.BonusQuantity }).Sum(rs => (double?)rs.Quantity) ?? 0
                                   -
                                   (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == i.ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                    AdjustedStock = (from adj_item in cxt.AdjustmentItems join Adjustment adj in cxt.Adjustments on adj_item.Adjustment.AdjustmentId equals adj.AdjustmentId where adj.IsActive && adj_item.Item.ItemId == i.ItemId && adj_item.IsActive select new { Qty = adj_item.Quantity }).Sum(a => (double?)a.Qty) ?? 0
                })
                .OrderByDescending(r => r.ItemId)
                .ToList();

            foreach (ItemsVM i in res)
            {
                if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
                else
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ExpiredStock - i.ConsumedStock - i.HoldStock;
                }
            }

            return res;
        }
        public List<ItemsVM> LoadSup_LowStItemIDs(int SupplierId)
        {
            var res =
                cxt.Suppliers
                .Where(s => s.SupplierID == SupplierId)
                .Select(s => s.Items.Where(si => si.IsActive).Select(i => new ItemsVM
                {
                    ItemId = i.ItemId,
                }).ToList())
                .FirstOrDefault();
            return res;
        }

        public Tuple<string, string> GetItemBarcode(int ItemId)
        {
            var data = cxt.Items.Where(i => i.ItemId == ItemId).Select(i => new { barcode = i.Barcode, itemName = i.ItemName }).FirstOrDefault();
            return new Tuple<string, string>(data.barcode, data.itemName);
        }

        public IPagedList<LowStockNotiVM> GetLowStockNotis(int pageNo, int pageSize)
        {
            IPagedList<LowStockNotiVM> r = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Select(vm => new LowStockNotiVM
               {
                   ItemId = vm.ItemId,
                   ItemName = vm.ItemName,
                   Unit = vm.Unit,
                   ConversionUnit = vm.ConversionUnit,
                   ReorderingLevel = vm.ReOrderingLevel,
                   TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == vm.ItemId).Sum(si => (double?)si.Quantity + si.BonusQuantity) ?? 0,
                   ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == vm.ItemId select new { qty = s.Quantity }).Sum(s => (double?)s.qty) ?? 0,
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == vm.ItemId && (b.Expiry) < (DateTime.Now) select new { qty = s.Quantity, BQty = s.BonusQuantity }).Sum(s => (double?)(s.qty + s.BQty)) ?? 0,
                   ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == vm.ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                   AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == vm.ItemId select new { qty = ai.Quantity }).Sum(ai => (double?)ai.qty) ?? 0,
               })
               .OrderByDescending(i => i.ItemId)
               .ToPagedList(pageNo, pageSize);

            // applied outer condition for performance sake..
            if (SharedVariables.AdminPharmacySetting.IsHolsStockOnInvoiceHold)
            {
                foreach (var i in r.Items)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredConsumedStock - i.HoldStock;
                }
            }
            else
            {
                foreach (var i in r.Items)
                {
                    i.AvailableStock = i.TotalStock + i.AdjustedStock - i.ConsumedStock - i.ExpiredConsumedStock;
                }
            }
            return r;
        }

        public IPagedList<ExpiryNotiVM> GetExpiryNotis(DateTime ExpNotiPeriod, int pageNo = 1, int pageSize = 1)
        {
            var res = (from i in cxt.Items
                       join si in cxt.StockItems on i.ItemId equals si.Item.ItemId
                       join b in cxt.Batches on si.Batch.BatchId equals b.BatchId
                       where i.IsActive && si.IsActive && b.Expiry != null
                       &&
                       (
                            (from es in cxt.StockItems where es.Item.ItemId == i.ItemId && es.Batch.BatchId == b.BatchId && (b.Expiry) > (DateTime.Now) && (b.Expiry) < (ExpNotiPeriod) select es).Sum(es => (double?)es.Quantity) ?? 0
                            -
                            (from es in cxt.StockConsumptionItems where es.Item.ItemId == i.ItemId && es.Batch.BatchId == b.BatchId && (b.Expiry) > (DateTime.Now) && (b.Expiry) < (ExpNotiPeriod) select es).Sum(es => (double?)es.Quantity) ?? 0
                       )
                       > 0
                       group si by new { i, b } into g
                       select new ExpiryNotiVM
                       {
                           ItemId = g.Key.i.ItemId,
                           ItemName = g.Key.i.ItemName,
                           BatchNo = g.Key.b.BatchName,
                           BatchExp = g.Key.b.Expiry.Value,
                       }).OrderByDescending(r => r.ItemId).OrderByDescending(r => r.ItemId).ToPagedList(pageNo, pageSize);
            return res;
        }

        public long getItemIdByBarcode(string barcode)
        {
            return cxt.Items.Where(i => i.Barcode.ToLower().Equals(barcode)).Select(i => i.ItemId).FirstOrDefault();
        }

        public Tuple<double, double> GetItemPrices(long itemId)
        {
            var res = cxt.Items.Where(i => i.ItemId == itemId).Select(i => new { retail = i.RetailPrice, cost = i.UnitCostPrice }).FirstOrDefault();
            return new Tuple<double, double>(res.retail, res.cost);
        }
        public List<StockValueRptVM> GetItemsWiseStockValue(long? categoryId, long? subCategoryId)
        {
            var res = cxt.Items
               .Where(i => i.IsActive)
               .Where(i => !i.IsDefault)
               .Where(i => categoryId.HasValue && categoryId.Value > 0 && i.CategoryId.Value == categoryId || !categoryId.HasValue || categoryId.Value <= 0)
               .Where(i => subCategoryId.HasValue && subCategoryId.Value > 0 && i.SubCategoryId.Value == subCategoryId || !subCategoryId.HasValue || subCategoryId.Value <= 0)
               .Select(vm => new
               {
                   vm.ItemId,
                   vm.ItemName,
                   Category = vm.Category == null ? "" : vm.Category.Name,
                   SubCategory = vm.Category == null ? "" : vm.Category.Name,
                   vm.RetailPrice,
                   CostPrice = vm.UnitCostPrice,
                   vm.Unit,
                   TotalStock = cxt.StockItems.Where(si => si.IsActive && si.Item.ItemId == vm.ItemId).Sum(si => (double?)si.Quantity + si.BonusQuantity) ?? 0,
                   ConsumedStock = (from s in cxt.StockConsumptionItems where s.IsActive && s.Item.ItemId == vm.ItemId select s).Sum(s => (double?)s.Quantity) ?? 0,
                   ExpiredStock = (from s in cxt.StockItems join b in cxt.Batches on s.Batch.BatchId equals b.BatchId where s.IsActive && s.Item.ItemId == vm.ItemId && (b.Expiry) < (DateTime.Now) select s).Sum(s => (double?)s.Quantity) ?? 0,
                   AdjustedStock = (from ai in cxt.AdjustmentItems join a in cxt.Adjustments on ai.Adjustment.AdjustmentId equals a.AdjustmentId where ai.IsActive && ai.Item.ItemId == vm.ItemId select ai).Sum(ai => (double?)ai.Quantity) ?? 0,
                   ExpiredConsumedStock = (from es in cxt.StockConsumptionItems join b_ in cxt.Batches on es.Batch.BatchId equals b_.BatchId where es.Item.ItemId == vm.ItemId && (b_.Expiry) < (DateTime.Now) select new { es.Quantity }).Sum(rs => (double?)rs.Quantity) ?? 0,
                   HoldStock = (from cs in cxt.StockConsumptionItems join i in cxt.Invoices on cs.InvoiceId equals i.InvoiceId where i.IsHoldInvoice && cs.IsActive && cs.Item.ItemId == vm.ItemId select cs).Sum(cs => (double?)cs.Quantity) ?? 0,
               }).OrderByDescending(i => i.ItemId).ToList();
            List<StockValueRptVM> resp = new List<StockValueRptVM>();
            foreach (var r in res)
            {
                StockValueRptVM v = new StockValueRptVM();
                v.ItemName = r.ItemName;
                v.Category = r.Category;
                v.RetailPrice = r.RetailPrice;
                v.CostPrice = r.CostPrice;
                v.Unit = r.Unit;
                v.AvailableQuantity = r.TotalStock - r.ConsumedStock - (r.ExpiredStock - r.ExpiredConsumedStock) + r.AdjustedStock;
                v.TCostValue = v.AvailableQuantity * v.CostPrice;
                v.TRetailValue = v.AvailableQuantity * v.RetailPrice;

                resp.Add(v);
            }
            return resp;
        }
    }
}