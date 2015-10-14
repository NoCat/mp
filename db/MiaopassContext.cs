using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using mp.DAL;
using System.Linq.Expressions;

namespace mp.DAL
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class MiaopassContext : DbContext
    {
        public StoredProcedure StoredProcedure { get; private set; }
        public MiaopassContext()
            : base("name=db")
        {
            StoredProcedure = new StoredProcedure(this);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Activity> Activitys { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ImageTag> ImageTags { get; set; }
        public DbSet<Download> Downloads { get; set; }
        public DbSet<Pick> Picks { get; set; }
        public DbSet<Praise> Praises { get; set; }
        public DbSet<Url> Urls { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        /// <param name="save">是否马上保存,默认马上保存</param>
        public void Insert<T>(T entity, bool save = true) where T : class
        {
            Set<T>().Add(entity);
            if (save)
            {
                SaveChanges();
            }
        }

        /// <summary>
        /// 插入一组实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体数组</param>
        /// <param name="save">是否马上保存,默认马上保存</param>
        public void InsertRange<T>(IEnumerable<T> entities, bool save = true) where T : class
        {
            Set<T>().AddRange(entities);
            if (save)
            {
                SaveChanges();
            }
        }

        public void Update(object entity,bool save=true)
        {
            Entry(entity).State = EntityState.Modified;
            if (save)
            {
                SaveChanges();
            }
        }

        public void Delete(object entity,bool save=true)
        {
            Entry(entity).State = EntityState.Deleted;
            if(save)
            {
                SaveChanges();
            }
        }

         public T CreateIfNotExist<T>(Expression<Func<T, bool>> predicate, T newEntity) where T: class
        {
            var result = Set<T>().Where(predicate).FirstOrDefault();
            if (result == null)
            {
                Insert(newEntity);
                result = newEntity;
            }
            return result;
        }
    }
}
