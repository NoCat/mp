using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mp.DAL;

namespace mp.BLL
{
    public class ManagerCollection
    {
        MiaopassContext db;

        ManagerBase<User> users;
        public ManagerBase<User> Users
        {
            get
            {
                if (users == null)
                    users = new ManagerBase<User>(db, this);
                return users;
            }
        }

        FileManager files;
        public FileManager Files
        {
            get
            {
                if (files == null)
                    files = new FileManager(db, this);
                return files;
            }
        }

        ImageManager images;
        public ImageManager Images
        {
            get
            {
                if (images == null)
                    images = new ImageManager(db, this);
                return images;
            }
        }


        ManagerBase<Package> packages;
        public ManagerBase<Package> Packages
        {
            get
            {
                if (packages == null)
                    packages = new ManagerBase<Package>(db, this);
                return packages;
            }
        }

        ManagerBase<Download> downloads;
        public ManagerBase<Download> Downloads
        {
            get
            {
                if (downloads == null)
                    downloads = new ManagerBase<Download>(db, this);
                return downloads;
            }
        }

        ManagerBase<Pick> picks;
        public ManagerBase<Pick> Picks
        {
            get
            {
                if (picks == null)
                    picks = new ManagerBase<Pick>(db, this);
                return picks;
            }
        }

        ManagerBase<Praise> praise;
        public ManagerBase<Praise> Praise
        {
            get
            {
                if (praise == null)
                    praise = new ManagerBase<Praise>(db, this);
                return praise;
            }
        }

        ManagerBase<Url> urls;
        public ManagerBase<Url> Urls
        {
            get
            {
                if (urls == null)
                    urls = new ManagerBase<Url>(db, this);
                return urls;
            }
        }

        ManagerBase<Config> configs;
        public ManagerBase<Config> Configs
        {
            get
            {
                if (configs == null)
                    configs = new ManagerBase<Config>(db, this);
                return configs;
            }
        }

        ManagerBase<AdminUser> adminUsers;
        public ManagerBase<AdminUser> AdminUsers
        {
            get
            {
                if (adminUsers == null)
                    adminUsers = new ManagerBase<AdminUser>(db, this);
                return adminUsers;
            }
        }

        ManagerBase<AdminSubAccount> adminSubAccounts;
        public ManagerBase<AdminSubAccount> AdminSubAccounts
        {
            get
            {
                if (adminSubAccounts == null)
                    adminSubAccounts = new ManagerBase<AdminSubAccount>(db, this);
                return adminSubAccounts;
            }
        }

        ManagerBase<AdminPixivPickUser> adminPixivPickUsers;
        public ManagerBase<AdminPixivPickUser> AdminPixivPickUsers
        {
            get
            {
                if (adminPixivPickUsers == null)
                    adminPixivPickUsers = new ManagerBase<AdminPixivPickUser>(db, this);
                return adminPixivPickUsers;
            }
        }

        ManagerBase<AdminPixivTag> adminPixivTags;
        public ManagerBase<AdminPixivTag> AdminPixivTags
        {
            get
            {
                if (adminPixivTags == null)
                    adminPixivTags = new ManagerBase<AdminPixivTag>(db, this);
                return adminPixivTags;
            }
        }


        public ManagerCollection()
        {
            db = new MiaopassContext();
        }

        public ManagerCollection(MiaopassContext db)
        {
            this.db = db;
        }
    }
}
