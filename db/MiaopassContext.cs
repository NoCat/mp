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
        public DbSet<AdminUser> AdminUsers { get; set; }
        public DbSet<AdminSubAccount> AdminSubAccounts { get; set; }


        public MiaopassContext() : base("name=db") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        void Insert<T>(T entity, bool save = true) where T : class
        {
            Set<T>().Add(entity);
            if (save)
            {
                SaveChanges();
            }
        }

        void InsertRange<T>(IEnumerable<T> entities, bool save = true) where T : class
        {
            Set<T>().AddRange(entities);
            if (save)
            {
                SaveChanges();
            }
        }

        void Update(object entity, bool save = true)
        {
            Entry(entity).State = EntityState.Modified;
            if (save)
            {
                SaveChanges();
            }
        }

        void Delete(object entity, bool save = true)
        {
            Entry(entity).State = EntityState.Deleted;
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

        public T CreateIfNotExist<T>(Expression<Func<T, bool>> predicate, T newEntity) where T : class
        {
            var result = Set<T>().Where(predicate).FirstOrDefault();
            if (result == null)
            {
                Insert(newEntity);
                result = newEntity;
            }
            return result;
        }

        #region 储存过程

        #region Image
        public void ImageInsert(Image entity)
        {
            Transaction(() =>
            {
                Insert(entity);
                var package = Packages.Find(entity.PackageID);
                if (package.HasCover == false)
                    package.CoverID = entity.ID;
                PackageUpdate(package);
            });
        }
        public void ImageUpdate(Image entity) { Update(entity); }
        public void ImageDelete(Image entity) { Delete(entity); }
        #endregion

        #region Package
        public void PackageInsert(Package entity) { Insert(entity); }
        public void PackageUpdate(Package entity) { Update(entity); }
        public void PackageDelete(Package entity) { Delete(entity); }
        #endregion

        #region User
        public void UserInsert(User entity) { Insert(entity); }
        public void UserUpdate(User entity) { Update(entity); }
        public void UserDelete(User entity) { Delete(entity); }
        #endregion

        #region Url
        public void UrlInsert(Url entity) { Insert(entity); }
        public void UrlUpdate(Url entity) { Update(entity); }
        public void UrlDelete(Url entity) { Delete(entity); }
        public Url UrlCreateIfNotExist(Url entity, Expression<Func<Url, bool>> predicate)
        {
            var result = Urls.Where(predicate).FirstOrDefault();
            if (result == null)
            {
                UrlInsert(entity);
                return entity;
            }
            return result;
        }
        #endregion

        #region Download
        public void DownloadInsert(Download entity) { Insert(entity); }
        public void DownloadUpdate(Download entity) { Update(entity); }
        public void DownloadDelete(Download entity) { Delete(entity); }
        public Download DownloadCreateIfNotExist(Download entity, Expression<Func<Download, bool>> predicate)
        {
            var result = Downloads.Where(predicate).FirstOrDefault();
            if (result == null)
            {
                DownloadInsert(entity);
                return entity;
            }
            return result;
        }
        #endregion

        #region Pick
        public void PickInsert(Pick entity) { Insert(entity); }
        public void PickUpdate(Pick entity) { Update(entity); }
        public void PickDelete(Pick entity) { Delete(entity); }
        #endregion

        #region File
        public void FileInsert(File entity) { Insert(entity); }
        public void FileUpdate(File entity) { Update(entity); }
        public void FileDelete(File entity) { Delete(entity); }
        #endregion

        #region AdminUser
        public void AdminUserInsert(AdminUser entity) { Insert(entity); }
        public void AdminUserUpdate(AdminUser entity) { Update(entity); }
        public void AdminUserDelete(AdminUser entity) { Delete(entity); }
        #endregion

        #region AdminSubAccount
        public void AdminSubAccountInsert(AdminSubAccount entity) { Insert(entity); }
        public void AdminSubAccountUpdate(AdminSubAccount entity) { Update(entity); }
        public void AdminSubAccountDelete(AdminSubAccount entity) { Delete(entity); }
        #endregion
        #endregion
    }
}
