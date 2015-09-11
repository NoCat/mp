using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using db.Models;

namespace db.DAL
{
    [DbConfigurationType(typeof(MySql.Data.Entity.MySqlEFConfiguration))]
    public class MiaopassContext : DbContext
    {
        public MiaopassContext() : base("name=db") { }
        public DbSet<User> Users { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Activity> Activitys { get; set; }
        public DbSet<LoginCookie> LoginCookies { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ImageTag> ImageTags { get; set; }
        public DbSet<Download> Downloads { get; set; }
        public DbSet<Pick> Picks { get; set; }
        public DbSet<PackageCover> PackageCovers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
