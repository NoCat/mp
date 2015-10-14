using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mp.DAL;
using System.Data.Entity;

namespace mp.BLL
{
    public class ManagerBase<T> where T:class
    {
        MiaopassContext _context;

        public ManagerBase(MiaopassContext context)
        {
            _context = context;
        }

        public DbSet<T> Items
        {
            get
            {
                return _context.Set<T>();
            }
        }

        virtual public T Insert(T entity,bool save=true)
        {
            _context.Insert(entity,save);
            return entity;
        }

        virtual public T Update(T entity,bool save=true)
        {
            _context.Update(entity,save);
            return entity;
        }

        virtual public void Delete(T entity, bool save = true)
        {
            _context.Delete(entity,save);            
        }

        public T CreateIfNotExist(T entity,System.Linq.Expressions.Expression<Func<T,bool>> predicate)
        {
            var result = Items.Where(predicate).FirstOrDefault();
            if(result==null)
            {
                return Insert(entity);
            }
            return result;
        }
    }
}
