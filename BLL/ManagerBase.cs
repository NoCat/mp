using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mp.DAL;
using System.Data.Entity;

namespace mp.BLL
{
    public class ManagerBase<T> where T : class
    {
        protected MiaopassContext Context { private set; get; }
        protected ManagementService Service { private set; get; }

        public ManagerBase(MiaopassContext context)
        {
            Context = context;
            Service = new ManagementService(Context);
        }

        public IQueryable<T> Items
        {
            get
            {
                return Context.Set<T>();
            }
        }

        virtual public void Insert(T entity, bool save = true)
        {
            Context.Insert(entity, save);
        }

        virtual public void Update(T entity, bool save = true)
        {
            Context.Update(entity, save);
        }

        virtual public void Delete(T entity, bool save = true)
        {
            Context.Delete(entity, save);
        }

        virtual public T CreateIfNotExist(T entity, System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            var result = Items.Where(predicate).FirstOrDefault();
            if (result == null)
            {
                return Insert(entity);
            }
            return result;
        }
    }
}
