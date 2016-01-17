using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq.Expressions;
using System.Transactions;

namespace mp.DAL
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class MiaopassContext : DbContext
    {
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
        public DbSet<Config> Configs { get; set; }
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<AdminSubAccount> AdminSubAccounts { get; set; }
        public DbSet<AdminPixivPickUser> AdminPixivPickUsers { get; set; }
        public DbSet<AdminPixivTag> AdminPixivTags { get; set; }
        public DbSet<AdminPixivWork> AdminPixivWorks { get; set; }
        public DbSet<AdminPixivWorkTag> AdminPixivWorkTags { get; set; }
        public DbSet<ResaveChain> ResaveChains { get; set; }

        public MiaopassContext() : base("name=db") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public T Add<T>(T entity, bool save = true) where T : class
        {
            Set<T>().Add(entity);
            if (save)
            {
                SaveChanges();
            }
            return entity;
        }

        public void AddRange<T>(IEnumerable<T> entities,bool save=true) where T:class
        {
            Set<T>().AddRange(entities);
            if(save)
            {
                SaveChanges();
            }
        }

        public T Remove<T>(T entity, bool save = true) where T : class
        {
            Entry(entity).State = EntityState.Deleted;
            if (save)
            {
                SaveChanges();
            }
            return entity;
        }

        public void RemoveRange<T>(IEnumerable<T> entities, bool save = true) where T : class
        {
            foreach (var entity in entities)
            {
                Entry(entity).State = EntityState.Deleted;
            }
            if (save)
            {
                SaveChanges();
            }
        }


        public T Update<T>(T entity, bool save = true) where T : class
        {
            Entry(entity).State = EntityState.Modified;
            if (save)
            {
                SaveChanges();
            }
            return entity;
        }

        public void UpdateRange<T>(IEnumerable<T> entities, bool save = true) where T : class
        {
            foreach (var entity in entities)
            {
                Entry(entity).State = EntityState.Modified;
            }            
            if (save)
            {
                SaveChanges();
            }
        }

        public void Transaction(Action action)
        {
            using (var transaction = new TransactionScope())
            {
                action();
                transaction.Complete();
            }
        }
    }
}
