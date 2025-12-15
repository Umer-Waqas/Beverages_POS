using GK.Shared.Core.EntityModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK_SupplierMS.Repostiory
{
    public class Repository<T>:IRepository<T> where T : class
    {
        private GKPOS_Entities _context = null;
        private DbSet<T> table = null;
        public Repository()
        {
            this._context = new GKPOS_Entities();
            table = _context.Set<T>();
        }
        public Repository(GKPOS_Entities _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }
        public IQueryable<T> Query()
        {
            return table.AsQueryable<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }
        public T GetById(object id)
        {
            return table.Find(id);
        }
        public void Insert(T obj)
        {
            table.Add(obj);
        }
        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }
        public void Delete(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
