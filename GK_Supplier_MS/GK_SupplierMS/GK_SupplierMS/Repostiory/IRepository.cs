using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_SupplierMS.Repostiory
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        IQueryable<T> Query();
        T GetById(object id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
    }
}
