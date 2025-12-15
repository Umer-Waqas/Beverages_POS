using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK.Shared.Core.EntityModel;
using GK_SupplierMS.Repostiory;

namespace GK_SupplierMS.Repostiory
{
    public class UnitOfWork : IDisposable
    {
        GK.Shared.Core.EntityModel.GKPOS_Entities _db = new GK.Shared.Core.EntityModel.GKPOS_Entities();
        private SupplierRepository _supplierRepo;
        private IRepository<Expens> _expenseRepo;

        public SupplierRepository SupplierRepo
        {
            get
            {
                if (this._supplierRepo == null)
                {
                    _supplierRepo = new GK_SupplierMS.Repostiory.SupplierRepository(_db);
                }
                return _supplierRepo;
            }
        }

        public IRepository<Expens> ExpenseRepo
        {
            get
            {
                if (_expenseRepo == null)
                {
                    _expenseRepo = new GK_SupplierMS.Repostiory.Repository<Expens>(_db);
                }
                return _expenseRepo;
            }
        }

        public int Save()
        {
            return _db.SaveChanges();
        }

        public void Dispose()
        {
            this._db = null;
        }
    }
}