using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data.Entity;
using System.Linq.Expressions;

namespace mp.DAL
{
    public class MpDbSet<T>:DbSet<T> where T : class
    {
        protected MiaopassContext DB { set; get; }

        public MpDbSet(MiaopassContext db)
        {
            DB = db;
        }

        virtual public T AddEx(T entity, bool save = true)
        {
            Add(entity);
            if (save)
            {
                DB.SaveChanges();
            }
            return entity;
        }

        virtual public T RemoveEx(T entity, bool save = true)
        {
            Remove(entity);
            if (save)
            {
                DB.SaveChanges();
            }
            return entity;
        }

        public T Update(T entity, bool save = true)
        {            
            DB.Entry(entity).State = EntityState.Modified;
            if (save)
            {
                DB.SaveChanges();
            }
            return entity;
        }

        virtual public T UpdateEx(T entity, bool save = true)
        {
            return Update(entity, save);
        }

        virtual public T CreateIfNotExist(Expression<Func<T, bool>> predicate, T newEntity) 
        {
            var result = DB.Set<T>().Where(predicate).FirstOrDefault();
            if (result == null)
            {
                AddEx(newEntity);
                result = newEntity;
            }
            
            return result;
        }
    }
}
