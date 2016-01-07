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
        protected MiaopassContext DB { private set; get; }
        public ManagerCollection Collection { get; set; }

        public ManagerBase(MiaopassContext db, ManagerCollection colletion)
        {
            this.DB = db;
            this.Collection = colletion;
        }

        public IQueryable<T> Items
        {
            get
            {
                return DB.Set<T>();
            }
        }

        public T Find(params object[] args)
        {
            return DB.Set<T>().Find(args);
        }

        virtual public T Add(T entity, bool save = true)
        {
            return DB.Add(entity, save);
        }

        virtual public T Remove(T entity, bool save = true)
        {
            return DB.Remove(entity, save);
        }

        virtual public T Update(T entity, bool save = true)
        {
            return DB.Update(entity, save);
        }

        virtual public T CreateIfNotExist(T newEntity, System.Linq.Expressions.Expression<Func<T, bool>> predicate, bool save = true)
        {
            var entity = DB.Set<T>().Where(predicate).FirstOrDefault();
            if (entity != null)
                return entity;

            return Add(newEntity, save);
        }

    }
}
